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


﻿using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Repository.DBModelsContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Interneuron.Terminology.Model.DomainModels;
using Interneuron.Terminology.Model.Search;
using Interneuron.Common.Extensions;
using Dapper;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Interneuron.Terminology.Repository
{
    public partial class TerminologyRepository<TEntity> : Repository<TEntity>, ITerminologyRepository<TEntity> where TEntity : EntityBase
    {
        private TerminologyDBContext _dbContext;
        private readonly DbSet<SnomedctConceptpreferrednameLatestMat> _snomedctConceptpreferrednameLatestMat;
        private readonly IConfiguration _configuration;
        private IEnumerable<IEntityEventHandler> _entityEventHandlers;

        public TerminologyRepository(TerminologyDBContext dbContext, IConfiguration configuration, IEnumerable<IEntityEventHandler> entityEventHandlers) : base(dbContext, entityEventHandlers)
        {
            _dbContext = dbContext;
            _snomedctConceptpreferrednameLatestMat = dbContext.Set<SnomedctConceptpreferrednameLatestMat>();
            _configuration = configuration;
            _entityEventHandlers = entityEventHandlers;
        }

        public IQueryable<TEntity> SearchSnomedTermBySemanticTag(string searchTerm, string semanticTag)
        {
            var searchTermTokens = searchTerm.Split(" ");

            var tokenToSearch = "";

            searchTermTokens.Each(s => { if (s.IsNotEmpty()) tokenToSearch = $"{tokenToSearch} & {s}:*"; });

            tokenToSearch = tokenToSearch.TrimStart(' ', '&');


            //var query = (from preferredConcept in this._dbContext.SnomedctConceptpreferrednameLatestMat
            //             join desc in this._dbContext.SnomedctDescriptionLatestMat on
            //              preferredConcept.Conceptid equals desc.Conceptid
            //             where
            //             //desc.Active == '1' && (EF.Functions.ILike(preferredConcept.Preferredname, $"{searchTerm}%") || EF.Functions.ILike(preferredConcept.Preferredname, $"% {searchTerm}%"))
            //             //&& EF.Functions.ILike(desc.Term, $"% ({semanticTag})") 
            //             //&& desc.Typeid == "900000000000003001"
            //             desc.Typeid == "900000000000003001" && desc.Active == '1' && EF.Functions.ILike(desc.Term, $"% ({semanticTag})")
            //             && preferredConcept.PreferrednameTokens.Matches(EF.Functions.ToTsQuery(tokenToSearch))
            //             select new SNOMEDCTSearchResultModel
            //             {
            //                 ConceptId = preferredConcept.Conceptid,
            //                 DescriptionId = preferredConcept.Descriptionid,
            //                 PreferredTerm = preferredConcept.Preferredname,
            //                 FSN = desc.Term
            //             }).Distinct();


            var query = (
                //from conceptLkp in this._dbContext.SnomedctConceptLookupMat
                from conceptLkp in this._dbContext.SnomedctConceptAllLookupMat
                where ((conceptLkp.PreferrednameTokens.Matches(EF.Functions.ToTsQuery(tokenToSearch))) || (conceptLkp.Conceptid == searchTerm))
                && EF.Functions.ILike(conceptLkp.Fsn, $"% ({semanticTag})")
                select new SNOMEDCTSearchResultModel
                {
                    ConceptId = conceptLkp.Conceptid,
                    PreferredTerm = conceptLkp.Preferredterm,
                    FSN = conceptLkp.Fsn
                }).Distinct();


            //var query =  this._snomedctConceptpreferrednameLatestMat.Where(con => con.Preferredname == con.Preferredname + " (" + semanticTag + ")");

            //var query = baseQuery.Where(con=> con.Active == '1' && EF.Functions.ILike(con.Preferredname, $"{searchTerm}%") || (EF.Functions.ILike(con.Preferredname, $"% {searchTerm}%")));

            var sqlGenerated = this.GetSql(query);

            return (IQueryable<TEntity>)query;
        }

        public async Task<IEnumerable<TEntity>> SearchSnomedTermsGetWithAllDescendents(string searchTerm, string semanticTag)
        {
            var searchTermTokens = searchTerm.Split(" ");

            var tokenToSearch = "";

            searchTermTokens.Each(s => { if (s.IsNotEmpty()) tokenToSearch = $"{tokenToSearch} & {s}:*"; });

            tokenToSearch = tokenToSearch.TrimStart(' ', '&');

            var qryStmt = $"SELECT * from terminology.udf_snomed_get_child_nodes_search_term_by_tag('{tokenToSearch}', '{searchTerm}','{semanticTag}')";

            //this._dbContext.Database.fro("SELECT * from generate_accession_attributes({0}, {1})", tokenToSearch, );

            //this._snomedctConceptpreferrednameLatestMat
            //.FromSql<SnomedctConceptpreferrednameLatestMat>("select myDbFunction({0})", myParam);

            IEnumerable<SNOMEDCTSearchResultModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<SNOMEDCTSearchResultModel>(qryStmt);
            }

            if (results == null) return null;

            return (IEnumerable<TEntity>)results;

            //using (var command = this._dbContext.Database.GetDbConnection().CreateCommand())
            //{
            //    command.CommandType = CommandType.StoredProcedure;
            //    command.CommandText = "bs_p_get_host_seqno";
            //    command.Parameters.Add(new Npgsql.NpgsqlParameter("host_id", NpgsqlTypes.NpgsqlDbType.Smallint)
            //    { Value = hostId });
            //    command.Parameters.Add(new Npgsql.NpgsqlParameter("srv_id", NpgsqlTypes.NpgsqlDbType.Smallint)
            //    { Value = srvId });
            //    if (command.Connection.State == ConnectionState.Closed)
            //        command.Connection.Open();
            //    var res = (long)command.ExecuteScalar();
            //    return res;
            //}

        }

        public async Task<IEnumerable<TEntity>> SearchSnomedTermsGetWithAllAncestors(string searchTerm, string semanticTag)
        {
            var searchTermTokens = searchTerm.Split(" ");

            var tokenToSearch = "";

            searchTermTokens.Each(s => { if (s.IsNotEmpty()) tokenToSearch = $"{tokenToSearch} & {s}:*"; });

            tokenToSearch = tokenToSearch.TrimStart(' ', '&');

            var qryStmt = $"SELECT * from terminology.udf_snomed_get_ancestor_nodes_search_term_by_tag('{tokenToSearch}', '{searchTerm}', '{semanticTag}')";

            IEnumerable<SNOMEDCTSearchResultModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<SNOMEDCTSearchResultModel>(qryStmt);
            }
            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetSnomedDescendentForConceptIds(string[] conceptIds)
        {
            if (!conceptIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * from terminology.udf_snomed_get_next_descendents(@in_conceptIds)";

            IEnumerable<SNOMEDCTSearchResultModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<SNOMEDCTSearchResultModel>(qryStmt, new { in_conceptIds = conceptIds });
            }
            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        public async Task<IEnumerable<TEntity>> GetSnomedAncestorForConceptIds(string[] conceptIds)
        {
            if (!conceptIds.IsCollectionValid()) return null;

            var qryStmt = $"SELECT * from terminology.udf_snomed_get_next_ancestors(@in_conceptIds)";

            IEnumerable<SNOMEDCTSearchResultModel> results;

            var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");

            using (var conn = new Npgsql.NpgsqlConnection(connString))
            {
                results = await conn.QueryAsync<SNOMEDCTSearchResultModel>(qryStmt, new { in_conceptIds = conceptIds });
            }
            if (results == null) return null;

            return (IEnumerable<TEntity>)results;
        }

        
    }
}
