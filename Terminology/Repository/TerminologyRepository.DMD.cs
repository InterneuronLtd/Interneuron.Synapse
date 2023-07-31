//BEGIN LICENSE BLOCK 
//Interneuron Synapse

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
//END LICENSE BLOCK 
﻿//Interneuron Synapse

//Copyright(C) 2021  Interneuron CIC

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


﻿using System.Linq;
using Interneuron.Terminology.Model.DomainModels;
using Interneuron.Terminology.Model.Search;
using Interneuron.Common.Extensions;
using Dapper;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Interneuron.Terminology.Infrastructure.Domain;
using System.Collections.Generic;

namespace Interneuron.Terminology.Repository
{
    public partial class TerminologyRepository<TEntity> : Repository<TEntity>, ITerminologyRepository<TEntity> where TEntity : EntityBase
    {
        public IEnumerable<TEntity> SearchDMDName(string searchTerm)
        {
            TerminologyConstants.PG_ESCAPABLE_CHARS.Each(item => searchTerm = searchTerm.Replace(item, $"\\{item}"));

            var searchTermTokens = searchTerm.Split(" ");

            var tokenToSearch = "";

            searchTermTokens.Each(s => { if (s.IsNotEmpty()) tokenToSearch = $"{tokenToSearch} & {s}:*"; });

            tokenToSearch = tokenToSearch.TrimStart(' ', '&');

            var query = (
                from nameLkp in this._dbContext.DmdNamesLookupMat
                join rel in this._dbContext.DmdRelationshipsMat on
                 nameLkp.Code equals rel.Code
                where (nameLkp.NameTokens.Matches(EF.Functions.ToTsQuery(tokenToSearch)) || nameLkp.Code == searchTerm)
                select new DMDSearchResultFlattenedModel
                {
                    Code = nameLkp.Code,
                    Name = nameLkp.Name,
                    Form = nameLkp.Vmpform,
                    Route = nameLkp.Vmproute,
                    ParentCode = rel.ParentCode,
                    PrescribingStatus = nameLkp.PrescribingStatus,
                    Supplier = nameLkp.Supplier,
                    ControlDrugCategory = nameLkp.ControlDrugCategory,
                    FormCode = nameLkp.VmpformCode,
                    RouteCode = nameLkp.VmprouteCode,
                    PrescribingStatusCode = nameLkp.PrescribingStatusCode,
                    SupplierCode = nameLkp.SupplierCode,
                    ControlDrugCategoryCode = nameLkp.ControlDrugCategoryCode,
                    LogicalLevel = rel.LogicalLevel

                }).Distinct();

#if DEBUG
            var sqlGenerated = this.GetSql(query);
#endif
            var flattenedDataList = query.ToList();

            var completeResults = FillEntitiesFromFlatList(flattenedDataList);

            if (completeResults == null) return null;

            return (IEnumerable<TEntity>)completeResults;
        }

        private List<DMDDetailResultModel> FillEntitiesFromFlatListWithDetails(List<DMDSearchResultFlattenedModel> flattenedDataList)
        {
            if (!flattenedDataList.IsCollectionValid()) return null;

            var resultsModel = flattenedDataList.AsParallel().Select(flatItem =>
            {
                var resultsModel = new DMDDetailResultModel
                {
                    Code = flatItem.Code,
                    PrevCode = flatItem.PrevCode,
                    Name = flatItem.Name,
                    ParentCode = flatItem.ParentCode,
                    Supplier = flatItem.Supplier,
                    Udfs = flatItem.Udfs,
                    SugF = flatItem.SugF,
                    GluF = flatItem.GluF,
                    PresF = flatItem.PresF,
                    CfcF = flatItem.CfcF,
                    Ema = flatItem.Ema,
                    ParallelImport = flatItem.ParallelImport,
                    SupplierCode = flatItem.SupplierCode,
                    LogicalLevel = flatItem.LogicalLevel
                };
                return resultsModel;
            }).Distinct(r => r.Code);

            var completeResults = resultsModel.AsParallel().Select(r =>
            {
                r.Routes = new List<DmdLookupRoute>();
                r.VMPIngredients = new List<DmdVmpIngredient>();

                var relatedEntities = flattenedDataList.Where(f => f.Code == r.Code);

                if (relatedEntities != null)
                {
                    r.OntologyFormRoutes = relatedEntities.Where(re => re.Ontcd.HasValue).Distinct(re => re.Ontcd).Select(n =>
                    {
                        return new DmdLookupOntformroute() { Cd = n.Ontcd };
                    }).ToList();

                    r.AvailableRestriction = relatedEntities.Where(re => re.AvailRestrictcd.HasValue).Distinct(re => re.AvailRestrictcd).Select(n =>
                    {
                        return new DmdLookupAvailrestrict() { Cd = n.AvailRestrictcd };
                    }).FirstOrDefault();

                    r.BasisOfName = relatedEntities.Where(re => re.Basiscd.HasValue).Distinct(re => re.Basiscd).Select(n =>
                    {
                        return new DmdLookupBasisofname() { Cd = n.Basiscd };
                    }).FirstOrDefault();

                    r.DoseForm = relatedEntities.Where(re => re.DfIndcd.HasValue).Distinct(re => re.DfIndcd).Select(n =>
                    {
                        return new DmdLookupDrugformind() { Cd = n.DfIndcd };
                    }).FirstOrDefault();

                    r.LicensingAuthority = relatedEntities.Where(re => re.LicAuthcd.HasValue).Distinct(re => re.LicAuthcd).Select(n =>
                    {
                        return new DmdLookupLicauth() { Cd = n.LicAuthcd };
                    }).FirstOrDefault();

                    r.UnitDoseFormSizeUOM = relatedEntities.Where(re => re.UdfsUomcd.IsNotEmpty()).Distinct(re => re.UdfsUomcd).Select(n =>
                    {
                        return new DmdLookupUom() { Cd = n.UdfsUomcd };
                    }).FirstOrDefault();

                    r.UnitDoseUOM = relatedEntities.Where(re => re.UnitDoseUomcd.IsNotEmpty()).Distinct(re => re.UnitDoseUomcd).Select(n =>
                    {
                        return new DmdLookupUom() { Cd = n.UnitDoseUomcd };
                    }).FirstOrDefault();

                    r.Form = relatedEntities.Where(re => re.FormCode.IsNotEmpty()).Distinct(re => re.FormCode).Select(n =>
                    {
                        return new DmdLookupForm() { Cd = n.FormCode, Desc = n.Form };
                    }).FirstOrDefault();

                    r.PrescribingStatus = relatedEntities.Where(re => re.PrescribingStatus.IsNotEmpty()).Distinct(re => re.PrescribingStatusCode).Select(re =>
                    {
                        return new DmdLookupPrescribingstatus
                        {
                            Cd = re.PrescribingStatusCode,
                            Desc = re.PrescribingStatus
                        };
                    }).FirstOrDefault();

                    r.ControlDrugCategory = relatedEntities.Where(re => re.ControlDrugCategory.IsNotEmpty()).Distinct(re => re.ControlDrugCategoryCode).Select(n =>
                    {
                        return new DmdLookupControldrugcat() { Cd = n.ControlDrugCategoryCode, Desc = n.ControlDrugCategory };
                    }).FirstOrDefault();

                    var routeList = relatedEntities.Where(re => re.Route.IsNotEmpty()).Distinct(re => re.RouteCode).Select(n =>
                    {
                        return new DmdLookupRoute() { Cd = n.RouteCode, Desc = n.Route };
                    }).ToList();

                    if (routeList.IsCollectionValid())
                    {
                        routeList.Each(n =>
                        {
                            r.Routes.Add(n);
                        });
                    }

                    var ingredientsList = relatedEntities.Where(re => re.IngredientSubstanceId.IsNotEmpty()).Distinct(re => re.IngredientSubstanceId).Select(n =>
                    {
                        return new DmdVmpIngredient()
                        {
                            BasisStrntcd = n.BasisPharmaceuticalStrengthCd,
                            BsSubid = n.BasisStrengthSubstanceId,
                            Isid = n.IngredientSubstanceId,
                            StrntDnmtrUomcd = n.StrengthValueDnmtrUnitCd,
                            StrntDnmtrVal = n.StrengthValDnmtr,
                            StrntNmrtrUomcd = n.StrengthValueNmtrUnitCd,
                            StrntNmrtrVal = n.StrengthValNmtr
                        };
                    }).ToList();

                    ingredientsList.Each(n =>
                    {
                        r.VMPIngredients.Add(n);
                    });
                }

                return r;
            }).ToList();

            return completeResults;
        }

        private List<DMDSearchResultModel> FillEntitiesFromFlatList(List<DMDSearchResultFlattenedModel> flattenedDataList)
        {
            if (!flattenedDataList.IsCollectionValid()) return new List<DMDSearchResultModel>();

            var forms = new List<DmdLookupForm>();

            var resultsModel = flattenedDataList.AsParallel().Select(flatItem =>
            {
                var resultsModel = new DMDSearchResultModel
                {
                    Code = flatItem.Code,
                    Name = flatItem.Name,
                    ParentCode = flatItem.ParentCode,
                    Supplier = flatItem.Supplier,
                    SupplierCode = flatItem.SupplierCode,
                    LogicalLevel = flatItem.LogicalLevel
                };
                return resultsModel;
            }).Distinct(r => r.Code);

            var completeResults = resultsModel.AsParallel().Select(r =>
            {
                r.Routes = new List<DmdLookupRoute>();
                r.VMPIngredients = new List<DmdVmpIngredient>();

                var relatedEntities = flattenedDataList.Where(f => f.Code == r.Code);

                if (relatedEntities != null)
                {
                    r.Form = relatedEntities.Where(re => re.FormCode.IsNotEmpty()).Distinct(re => re.FormCode).Select(n =>
                     {
                         return new DmdLookupForm() { Cd = n.FormCode, Desc = n.Form };
                     }).FirstOrDefault();

                    r.PrescribingStatus = relatedEntities.Where(re => !re.PrescribingStatus.IsEmpty()).Distinct(re => re.PrescribingStatusCode).Select(re =>
                     {
                         return new DmdLookupPrescribingstatus
                         {
                             Cd = re.PrescribingStatusCode,
                             Desc = re.PrescribingStatus
                         };
                     }).FirstOrDefault();

                    r.ControlDrugCategory = relatedEntities.Where(re => !re.ControlDrugCategory.IsEmpty()).Distinct(re => re.ControlDrugCategoryCode).Select(n =>
                     {
                         return new DmdLookupControldrugcat() { Cd = n.ControlDrugCategoryCode, Desc = n.ControlDrugCategory };
                     }).FirstOrDefault();

                    var routeList = relatedEntities.Where(re => !re.Route.IsEmpty()).Distinct(re => re.RouteCode).Select(n =>
                    {
                        return new DmdLookupRoute() { Cd = n.RouteCode, Desc = n.Route };
                    }).ToList();

                    if (routeList.IsCollectionValid())
                    {
                        routeList.Each(n =>
                        {
                            r.Routes.Add(n);
                        });
                    }

                    var ingredientsList = relatedEntities.Where(re => re.IngredientSubstanceId.IsNotEmpty()).Distinct(re => re.IngredientSubstanceId).Select(n =>
                    {
                        return new DmdVmpIngredient()
                        {
                            BasisStrntcd = n.BasisPharmaceuticalStrengthCd,
                            BsSubid = n.BasisStrengthSubstanceId,
                            Isid = n.IngredientSubstanceId,
                            StrntDnmtrUomcd = n.StrengthValueDnmtrUnitCd,
                            StrntDnmtrVal = n.StrengthValDnmtr,
                            StrntNmrtrUomcd = n.StrengthValueNmtrUnitCd,
                            StrntNmrtrVal = n.StrengthValNmtr
                        };
                    }).ToList();

                    ingredientsList.Each(n =>
                    {
                        r.VMPIngredients.Add(n);
                    });
                }

                return r;
            }).ToList();

            return completeResults;
        }

        public async Task<IEnumerable<TEntity>> SearchDMDGetWithAllDescendents(string searchTerm)
        {
            TerminologyConstants.PG_ESCAPABLE_CHARS.Each(item => searchTerm = searchTerm.Replace(item, $"\\{item}"));

            var searchTermTokens = searchTerm.Split(" ");

            var tokenToSearch = "";

            searchTermTokens.Each(s => { if (s.IsNotEmpty()) tokenToSearch = $"{tokenToSearch} & {s}:*"; });

            tokenToSearch = tokenToSearch.TrimStart(' ', '&');

            var qryStmt = $"SELECT * from terminology.udf_dmd_get_child_nodes_search('{tokenToSearch}', '{searchTerm}')";

            IEnumerable<DMDSearchResultFlattenedModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<DMDSearchResultFlattenedModel>(qryStmt);
            }

            var completedResults = FillEntitiesFromFlatList(results.ToList());

            if (completedResults == null) return null;

            return (IEnumerable<TEntity>)completedResults;
        }

        public async Task<IEnumerable<TEntity>> SearchDMDGetWithAllAncestors(string searchTerm)
        {
            TerminologyConstants.PG_ESCAPABLE_CHARS.Each(item => searchTerm = searchTerm.Replace(item, $"\\{item}"));

            var searchTermTokens = searchTerm.Split(" ");

            var tokenToSearch = "";

            searchTermTokens.Each(s => { if (s.IsNotEmpty()) tokenToSearch = $"{tokenToSearch} & {s}:*"; });

            tokenToSearch = tokenToSearch.TrimStart(' ', '&');

            var qryStmt = $"SELECT * from terminology.udf_dmd_get_ancestor_nodes_search('{tokenToSearch}', '{searchTerm}')";

            IEnumerable<DMDSearchResultFlattenedModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<DMDSearchResultFlattenedModel>(qryStmt);
            }
            var completedResults = FillEntitiesFromFlatList(results.ToList());

            if (completedResults == null) return null;

            return (IEnumerable<TEntity>)completedResults;
        }

        public async Task<IEnumerable<TEntity>> GetDMDDescendentForCodes(string[] codes)
        {
            if (!codes.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * from terminology.udf_dmd_get_next_descendent(@in_codes)";

            IEnumerable<DMDSearchResultFlattenedModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<DMDSearchResultFlattenedModel>(qryStmt, new { in_codes = codes });
            }
            var completedResults = FillEntitiesFromFlatList(results.ToList());

            if (completedResults == null) return null;

            return (IEnumerable<TEntity>)completedResults;
        }

        public async Task<IEnumerable<TEntity>> GetDMDFullDataForCodes(string[] codes)
        {
            if (!codes.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * from terminology.udf_dmd_get_nodes_by_codes(@in_codes)";

            IEnumerable<DMDSearchResultFlattenedModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<DMDSearchResultFlattenedModel>(qryStmt, new { in_codes = codes });
            }

            var completedResults = FillEntitiesFromFlatListWithDetails(results.ToList());

            if (completedResults == null) return null;

            return (IEnumerable<TEntity>)completedResults;
        }

        public async Task<IEnumerable<TEntity>> GetDMDAncestorForCodes(string[] codes)
        {
            if (!codes.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * from terminology.udf_dmd_get_next_ancestor(@in_codes)";

            IEnumerable<DMDSearchResultFlattenedModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<DMDSearchResultFlattenedModel>(qryStmt, new { in_codes = codes });
            }

            var completedResults = FillEntitiesFromFlatList(results.ToList());

            if (completedResults == null) return null;

            return (IEnumerable<TEntity>)completedResults;
        }

        public async Task<IEnumerable<TEntity>> GetDMDAllAncestorsForCodes(string[] codes)
        {
            if (!codes.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * from terminology.udf_dmd_get_all_ancestors(@in_codes)";

            IEnumerable<DMDSearchResultFlattenedModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<DMDSearchResultFlattenedModel>(qryStmt, new { in_codes = codes });
            }

            var completedResults = FillEntitiesFromFlatList(results.ToList());

            if (completedResults == null) return null;

            return (IEnumerable<TEntity>)completedResults;
        }

        public async Task<IEnumerable<TEntity>> GetDMDAllDescendentsForCodes(string[] codes)
        {
            if (!codes.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * from terminology.udf_dmd_get_all_descendents(@in_codes)";

            IEnumerable<DMDSearchResultFlattenedModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<DMDSearchResultFlattenedModel>(qryStmt, new { in_codes = codes });
            }

            var completedResults = FillEntitiesFromFlatList(results.ToList());

            if (completedResults == null) return null;

            return (IEnumerable<TEntity>)completedResults;
        }

    }
}
