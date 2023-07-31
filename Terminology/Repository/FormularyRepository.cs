 //Interneuron synapse

//Copyright(C) 2023  Interneuron Holdings Ltd

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Interneuron.Common.Extensions;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.Search;
using Interneuron.Terminology.Repository.DBModelsContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Interneuron.Terminology.Model.History;
using Interneuron.Terminology.Model.Other;

namespace Interneuron.Terminology.Repository
{
    public partial class FormularyRepository<TEntity> : Repository<TEntity>, IFormularyRepository<TEntity> where TEntity : EntityBase
    {
        private TerminologyDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private IEnumerable<IEntityEventHandler> _entityEventHandlers;

        public FormularyRepository(TerminologyDBContext dbContext, IConfiguration configuration, IEnumerable<IEntityEventHandler> entityEventHandlers) : base(dbContext, entityEventHandlers)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _entityEventHandlers = entityEventHandlers;
        }


        /// <summary>
        /// Search for the records based on criteria and where the record is latest version
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> SearchFormularyBySearchTerm(string searchTerm)
        {
            IEnumerable<FormularyBasicSearchResultModel> results;

            if (searchTerm.IsEmpty()) return null;

            string tokenToSearch = null;
            string codeToSearch = null;

            var isSearcForCode = long.TryParse(searchTerm, out long searchCode);
            if (isSearcForCode)
            {
                codeToSearch = searchCode.ToString();
            }
            else
            {
                TerminologyConstants.PG_ESCAPABLE_CHARS.Each(escRec => searchTerm = searchTerm.Replace(escRec, $"\\{escRec}"));
                var searchTermTokens = searchTerm.Split(" ");
                searchTermTokens.Each(s => { if (s.IsNotEmpty()) tokenToSearch = $"{tokenToSearch} & {s}:*"; });
                tokenToSearch = tokenToSearch.TrimStart(' ', '&');
            }


            var qryStmt = $"SELECT * from local_formulary.udf_formulary_search_nodes_with_descendents(@in_name, @in_search_code)";

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<FormularyBasicSearchResultModel>(qryStmt, new { in_name = tokenToSearch, in_search_code = codeToSearch });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetLatestAMPNodesWithBasicResultsForAttributes(string searchTerm, List<string> recordStatusCodes, List<string> formularyStatusCodes, List<string> flags)
        {
            IEnumerable<FormularyBasicSearchResultModel> basicSearchResults = null;

            string tokenToSearch = null;
            string codeToSearch = null;

            if (searchTerm.IsNotEmpty())
            {
                var isSearcForCode = long.TryParse(searchTerm, out long searchCode);
                if (isSearcForCode)
                {
                    codeToSearch = searchCode.ToString();
                }
                else
                {
                    TerminologyConstants.PG_ESCAPABLE_CHARS.Each(escRec => searchTerm = searchTerm.Replace(escRec, $"\\{escRec}"));

                    var searchTermTokens = searchTerm.Split(" ");
                    searchTermTokens.Each(s => { if (s.IsNotEmpty()) tokenToSearch = $"{tokenToSearch} & {s}:*"; });
                    tokenToSearch = tokenToSearch.TrimStart(' ', '&');
                }
            }

            var qryStmt = @$"select distinct 
                 fh.formulary_id as formularyid,
	             fh.version_id as  versionid,
	             fh.formulary_version_id as formularyversionid,
	             fh.name,
	             fh.code,
	             fh.product_type as  producttype,
	             fh.parent_code as parentcode,
	             fh.parent_name as parentname,
	             fh.parent_product_type as parentproducttype,
	             fh.is_latest as islatest,
	             fh.is_duplicate as isduplicate,
	             fh.rec_status_code as recstatuscode,
	             detail.rnoh_formulary_statuscd as rnohformularystatuscd,
	             detail.prescribable
              from local_formulary.formulary_header fh
              inner join local_formulary.formulary_detail detail on detail.formulary_version_id = fh.formulary_version_id
              where fh.is_latest = true
                and(fh.product_type = 'AMP')
                and (@in_name::text is null or fh.name_tokens @@ to_tsquery(@in_name))
				and (@in_search_code::text is null or fh.code = @in_search_code)
				and (@in_recordstatus_codes::text[] is null or fh.rec_status_code = any(@in_recordstatus_codes))
				and (@in_rnoh_formulary_status_codes::text[] is null or detail.rnoh_formulary_statuscd = any(@in_rnoh_formulary_status_codes))
                { GetConditionByFlags(flags, "detail")}";

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            var in_recordstatus_code_vals = recordStatusCodes.IsCollectionValid() ? recordStatusCodes : null;
            var in_rnoh_formulary_status_code_vals = formularyStatusCodes.IsCollectionValid() ? formularyStatusCodes : null;

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                basicSearchResults = await conn.QueryAsync<FormularyBasicSearchResultModel>(qryStmt,
                    new
                    {
                        in_name = tokenToSearch,
                        in_search_code = codeToSearch,
                        in_recordstatus_codes = in_recordstatus_code_vals,
                        in_rnoh_formulary_status_codes = in_rnoh_formulary_status_code_vals,
                    });
            }

            return (IEnumerable<TEntity>)basicSearchResults;
        }

        private string GetConditionByFlags(List<string> flags, string detailTableAlias)
        {
            if (!flags.IsCollectionValid()) return string.Empty;

            var conditionalString = "";

            var columnNames = GetColumnNames();

            flags.Each(flag =>
            {
                if (flag.IsNotEmpty() && columnNames.ContainsKey(flag))
                {
                    var columnName = columnNames[flag];
                    var typeOfCol = "s";

                    var splitByType = columnName.Split("|");
                    if (splitByType.Length > 1)
                    {
                        columnName = splitByType[0];
                        typeOfCol = splitByType[1];
                    }

                    if (typeOfCol == "b")
                        conditionalString = $"{conditionalString } and {detailTableAlias}.{columnName} = true";
                    else
                        conditionalString = $"{conditionalString } and {detailTableAlias}.{columnName} = '1'";
                }
            });

            return conditionalString;
        }

        private Dictionary<string, string> GetColumnNames()
        {
            return new Dictionary<string, string>
            {
                {"BlackTriangle","black_triangle"},
                {"BloodProduct","is_blood_product|b"},
                {"CFCFree","cfc_free"},
                {"ClinicalTrialMedication","clinical_trial_medication"},
                {"CriticalDrug","critical_drug"},
                {"CustomControlledDrug","is_custom_controlled_drug|b"},
                {"Diluent","is_diluent|b"},
                {"EMAAdditionalMonitoring","ema_additional_monitoring"},
                {"ExpensiveMedication","expensive_medication"},
                {"GastroResistant","is_gastro_resistant|b"},
                {"GlutenFree","gluten_free"},
                {"HighAlertMedication","high_alert_medication"},
                {"IgnoreDuplicateWarnings","ignore_duplicate_warnings"},
                {"IVtoOral","iv_to_oral"},
                {"IsIndicationMandatory", "is_indication_mandatory|b"},
                {"ModifiedRelease","is_modified_release|b"},
                {"NotforPRN","not_for_prn"},
                {"OutpatientMedication","outpatient_medication_cd"},
                {"Parallelimport","parallel_import"},
                {"Prescribable","prescribable|b"},
                {"PreservativeFree","preservative_free"},
                {"SugarFree","sugar_free"},
                {"UnlicensedMedication","unlicensed_medication_cd"},
                {"WitnessingRequired","witnessing_required"}
            };
        }

        public async Task<IEnumerable<TEntity>> GetLatestTopLevelNodesWithBasicResults()
        {
            IEnumerable<FormularyBasicSearchResultModel> basicSearchResults = null;


            var qryStmt = $"SELECT * from local_formulary.udf_formulary_get_latest_top_nodes()";

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                basicSearchResults = await conn.QueryAsync<FormularyBasicSearchResultModel>(qryStmt);
            }

            return (IEnumerable<TEntity>)basicSearchResults;
        }


        public IQueryable<TEntity> GetFormularyDetail(string id)
        {
            var query = this._dbContext.FormularyHeader
                .Include(hdr => hdr.FormularyDetail)// Include<FormularyDetail>("FormularyDetail");
                .Include(hdr => hdr.FormularyAdditionalCode)
                .Include(hdr => hdr.FormularyIndication)
                .Include(hdr => hdr.FormularyIngredient)
                .Include(hdr => hdr.FormularyRouteDetail)
                .Include(hdr => hdr.FormularyOntologyForm)
                .Include(hdr => hdr.FormularyExcipient)
                .Include(hdr => hdr.FormularyLocalRouteDetail)
                .Where(hdr => hdr.FormularyVersionId == id);

            return (IQueryable<TEntity>)query;
        }

        public IQueryable<TEntity> GetFormularyListForIds(List<string> ids, bool onlyNonDeleted = false)
        {
            var query = this._dbContext.FormularyHeader
                .Include(hdr => hdr.FormularyDetail)
                .Include(hdr => hdr.FormularyAdditionalCode)
                .Include(hdr => hdr.FormularyIndication)
                .Include(hdr => hdr.FormularyIngredient)
                .Include(hdr => hdr.FormularyRouteDetail)
                .Include(hdr => hdr.FormularyExcipient)
                .Include(hdr => hdr.FormularyLocalRouteDetail)
                .Where(hdr => ids.Contains(hdr.FormularyVersionId));

            if (onlyNonDeleted)
            {
                query = query.Where(hdr => hdr.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED);
            }

            return (IQueryable<TEntity>)query;
        }

        /// <summary>
        /// Returns the latest version of formularies for the codes
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="onlyNonDeleted"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetLatestFormulariesByCodes(string[] codes, bool onlyNonDeleted = false)
        {
            var query = this._dbContext.FormularyHeader
                .Include(hdr => hdr.FormularyDetail)// Include<FormularyDetail>("FormularyDetail");
                .Include(hdr => hdr.FormularyAdditionalCode)
                .Include(hdr => hdr.FormularyIndication)
                .Include(hdr => hdr.FormularyIngredient)
                .Include(hdr => hdr.FormularyRouteDetail)
                .Include(hdr => hdr.FormularyExcipient)
                .Include(hdr => hdr.FormularyLocalRouteDetail)
                .Where(hdr => codes.Contains(hdr.Code)
                && hdr.IsLatest == true);

            if (onlyNonDeleted)
            {
                query = query.Where(hdr => hdr.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED);
            }

            return (IQueryable<TEntity>)query;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyDescendentForCodes(string[] codes, bool onlyNonDeleted = true)
        {
            if (!codes.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * from local_formulary.udf_formulary_get_descendents_by_codes(@in_codes)";

            IEnumerable<FormularyBasicSearchResultModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<FormularyBasicSearchResultModel>(qryStmt, new { in_codes = codes });
            }
            if (results == null) return null;

            if (onlyNonDeleted)
                results = results.Where(rec => rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED);

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyImmediateDescendentForCodes(string[] codes, bool onlyNonDeleted = true)
        {
            if (!codes.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * from local_formulary.udf_formulary_get_next_descendents_by_codes(@in_codes)";

            IEnumerable<FormularyBasicSearchResultModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<FormularyBasicSearchResultModel>(qryStmt, new { in_codes = codes });
            }
            if (results == null) return null;

            if (onlyNonDeleted)
                results = results.Where(rec => rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED);

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyAncestorForCodes(string[] codes, bool onlyNonDeleted = true)
        {
            if (!codes.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * from local_formulary.udf_formulary_get_ancestors_by_codes(@in_codes)";

            IEnumerable<FormularyBasicSearchResultModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<FormularyBasicSearchResultModel>(qryStmt, new { in_codes = codes });
            }
            if (results == null) return null;

            if (onlyNonDeleted)
                results = results.Where(rec => rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED);

            return (IEnumerable<TEntity>)results;
        }

        public IEnumerable<TEntity> GetLatestFormulariesBriefInfoByNameOrCode(string productNameOrCode, string productType = null, bool isExactMatch = false)
        {
            //Only Non-deleted records and latest
            var query = this._dbContext.FormularyHeader
                            .Where(rec => rec.IsLatest == true && rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED);

            if (productNameOrCode.IsNotEmpty())
            {
                TerminologyConstants.PG_ESCAPABLE_CHARS.Each(escRec => productNameOrCode = productNameOrCode.Replace(escRec, $"\\{escRec}"));

                if (isExactMatch)
                {
                    var searchTermTokens = productNameOrCode.Split(" ");

                    var tokenToSearch = "";

                    searchTermTokens.Each(s => { if (s.IsNotEmpty()) { tokenToSearch = $"{tokenToSearch} & {s}"; } });

                    tokenToSearch = tokenToSearch.TrimStart(' ', '&');

                    query = query.Where(q => EF.Functions.ToTsVector("simple", q.Name).Matches(EF.Functions.ToTsQuery("simple", tokenToSearch)) || q.Code == productNameOrCode);
                }
                else
                {
                    var searchTermTokens = productNameOrCode.Split(" ");

                    var tokenToSearch = "";

                    searchTermTokens.Each(s => { if (s.IsNotEmpty()) { tokenToSearch = $"{tokenToSearch} & {s}:*"; } });

                    tokenToSearch = tokenToSearch.TrimStart(' ', '&');

                    query = query.Where(q => q.NameTokens.Matches(EF.Functions.ToTsQuery(tokenToSearch)) || q.Code == productNameOrCode);
                }
            }
                
            if (productType.IsNotEmpty())
            {
                query = query.Where(q => q.ProductType.ToLower() == productType.ToLower());
            }

            return (IEnumerable<TEntity>)query.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetHistoryOfFormularies()
        {
            IEnumerable<FormularyHistoryModel> historyResults = null;

            var qryStmt = @$"SELECT 
                            fh.code, 
                            fh.name, 
                            fh.product_type as producttype,
                            case when lc.desc is null then '' else lc.desc end as status, 
                            to_char(fh._updateddate, 'DD/MM/YYYY HH:mm') datetime, 
                            fh._updatedby as user, 
                            fh.formulary_version_id as previousformularyversionid,
                            (select fh1.formulary_version_id from local_formulary.formulary_header fh1 where fh1.code = fh.code order by version_id desc limit 1) as currentformularyversionid
                            FROM local_formulary.formulary_header fh
                            inner join local_formulary.formulary_detail fd on fh.formulary_version_id = fd.formulary_version_id
                            left join local_formulary.lookup_common lc on (select fh.rec_status_code from local_formulary.formulary_header fh1 where fh1.code = fh.code order by version_id desc limit 1) = lc.cd and lc.type = 'RecordStatus'
                            where fh.is_latest = false or (fh.is_latest = true and rec_status_code = '006')
                            group by fh.code, fh.name, fh.product_type, lc.desc , fh._updateddate, fh._updatedby, fh.formulary_version_id
                            order by fh.rec_statuschange_date desc, fh.name";

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                historyResults = await conn.QueryAsync<FormularyHistoryModel>(qryStmt);
            }

            return (IEnumerable<TEntity>)historyResults;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyLocalLicensedUse(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_local_licensed_use(@in_formularyversionids)";

            IEnumerable<FormularyLocalLicensedUseModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<FormularyLocalLicensedUseModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyLocalUnlicensedUse(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_local_unlicensed_use(@in_formularyversionids)";

            IEnumerable<FormularyLocalUnlicensedUseModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<FormularyLocalUnlicensedUseModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyLocalLicensedRoute(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_local_licensed_route(@in_formularyversionids)";

            IEnumerable<FormularyLocalLicensedRouteModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<FormularyLocalLicensedRouteModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyLocalUnlicensedRoute(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_local_unlicensed_route(@in_formularyversionids)";

            IEnumerable<FormularyLocalUnlicensedRouteModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<FormularyLocalUnlicensedRouteModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyCustomWarning(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_custom_warning(@in_formularyversionids)";

            IEnumerable<CustomWarningModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<CustomWarningModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyReminder(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_reminder(@in_formularyversionids)";

            IEnumerable<ReminderModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<ReminderModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyEndorsement(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_endorsement(@in_formularyversionids)";

            IEnumerable<EndorsementModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<EndorsementModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyMedusaPreparationInstruction(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_medusa_preparation_instruction(@in_formularyversionids)";

            IEnumerable<MedusaPreparationInstructionModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<MedusaPreparationInstructionModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyTitrationType(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_titration_type(@in_formularyversionids)";

            IEnumerable<TitrationTypeModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<TitrationTypeModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyRoundingFactor(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_rounding_factor(@in_formularyversionids)";

            IEnumerable<RoundingFactorModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<RoundingFactorModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyCompatibleDiluent(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_compatible_diluent(@in_formularyversionids)";

            IEnumerable<CompatibleDiluentModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<CompatibleDiluentModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyClinicalTrialMedication(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_clinical_trial_medication(@in_formularyversionids)";

            IEnumerable<ClinicalTrialMedicationModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<ClinicalTrialMedicationModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyGastroResistant(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_gastro_resistant(@in_formularyversionids)";

            IEnumerable<GastroResistantModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<GastroResistantModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyCriticalDrug(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_critical_drug(@in_formularyversionids)";

            IEnumerable<CriticalDrugModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<CriticalDrugModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyModifiedRelease(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_modified_release(@in_formularyversionids)";

            IEnumerable<ModifiedReleaseModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<ModifiedReleaseModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyExpensiveMedication(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_expensive_medication(@in_formularyversionids)";

            IEnumerable<ExpensiveMedicationModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<ExpensiveMedicationModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyHighAlertMedication(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_high_alert_medication(@in_formularyversionids)";

            IEnumerable<HighAlertMedicationModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<HighAlertMedicationModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyIVToOral(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_iv_to_oral(@in_formularyversionids)";

            IEnumerable<IVToOralModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<IVToOralModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyNotForPRN(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_not_for_prn(@in_formularyversionids)";

            IEnumerable<NotForPRNModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<NotForPRNModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyBloodProduct(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_blood_product(@in_formularyversionids)";

            IEnumerable<BloodProductModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<BloodProductModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyDiluent(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_diluent(@in_formularyversionids)";

            IEnumerable<DiluentModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<DiluentModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyPrescribable(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_prescribable(@in_formularyversionids)";

            IEnumerable<PrescribableModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<PrescribableModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyOutpatientMedication(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_outpatient_medication(@in_formularyversionids)";

            IEnumerable<OutpatientMedicationModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<OutpatientMedicationModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyIgnoreDuplicateWarning(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_ignore_duplicate_warning(@in_formularyversionids)";

            IEnumerable<IgnoreDuplicateWarningModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<IgnoreDuplicateWarningModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyControlledDrug(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_controlled_drug(@in_formularyversionids)";

            IEnumerable<ControlledDrugModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<ControlledDrugModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyPrescriptionPrintingRequired(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_prescription_printing_required(@in_formularyversionids)";

            IEnumerable<ControlledDrugModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<ControlledDrugModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyIndicationMandatory(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_indication_mandatory(@in_formularyversionids)";

            IEnumerable<IndicationMandatoryModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<IndicationMandatoryModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyWitnessingRequired(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_witnessing_required(@in_formularyversionids)";

            IEnumerable<WitnessingRequiredModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<WitnessingRequiredModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetFormularyStatus(List<string> formularyVersionIds)
        {
            if (!formularyVersionIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * FROM local_formulary.udf_formulary_get_formulary_status(@in_formularyversionids)";

            IEnumerable<FormularyStatusModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<FormularyStatusModel>(qryStmt, new { in_formularyversionids = formularyVersionIds });
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }
    }
}
