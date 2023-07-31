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


﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Interneuron.Terminology.Repository;
using Interneuron.Terminology.Model.DomainModels;
using Microsoft.Extensions.Configuration;

namespace Interneuron.Terminology.Repository.DBModelsContext
{
    public partial class TerminologyDBContext : DbContext
    {
        private IConfiguration _configuration;

        public TerminologyDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public TerminologyDBContext(DbContextOptions<TerminologyDBContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public virtual DbSet<AtcLookup> AtcLookup { get; set; }
        public virtual DbSet<BnfLookup> BnfLookup { get; set; }
        public virtual DbSet<DmdAmp> DmdAmp { get; set; }
        public virtual DbSet<DmdAmpDrugroute> DmdAmpDrugroute { get; set; }
        public virtual DbSet<DmdAmpExcipient> DmdAmpExcipient { get; set; }
        public virtual DbSet<DmdAtc> DmdAtc { get; set; }
        public virtual DbSet<DmdBnf> DmdBnf { get; set; }
        public virtual DbSet<DmdLookupAvailrestrict> DmdLookupAvailrestrict { get; set; }
        public virtual DbSet<DmdLookupBasisofname> DmdLookupBasisofname { get; set; }
        public virtual DbSet<DmdLookupBasisofstrength> DmdLookupBasisofstrength { get; set; }
        public virtual DbSet<DmdLookupControldrugcat> DmdLookupControldrugcat { get; set; }
        public virtual DbSet<DmdLookupDrugformind> DmdLookupDrugformind { get; set; }
        public virtual DbSet<DmdLookupForm> DmdLookupForm { get; set; }
        public virtual DbSet<DmdLookupIngredient> DmdLookupIngredient { get; set; }
        public virtual DbSet<DmdLookupLicauth> DmdLookupLicauth { get; set; }
        public virtual DbSet<DmdLookupOntformroute> DmdLookupOntformroute { get; set; }
        public virtual DbSet<DmdLookupPrescribingstatus> DmdLookupPrescribingstatus { get; set; }
        public virtual DbSet<DmdLookupRoute> DmdLookupRoute { get; set; }
        public virtual DbSet<DmdLookupSupplier> DmdLookupSupplier { get; set; }
        public virtual DbSet<DmdLookupUom> DmdLookupUom { get; set; }
        public virtual DbSet<DmdNamesLookupAllMat> DmdNamesLookupAllMat { get; set; }
        public virtual DbSet<DmdNamesLookupMat> DmdNamesLookupMat { get; set; }
        public virtual DbSet<DmdRelationshipsMat> DmdRelationshipsMat { get; set; }
        public virtual DbSet<DmdSnomedVersion> DmdSnomedVersion { get; set; }
        public virtual DbSet<DmdSyncLog> DmdSyncLog { get; set; }
        public virtual DbSet<DmdVmp> DmdVmp { get; set; }
        public virtual DbSet<DmdVmpControldrug> DmdVmpControldrug { get; set; }
        public virtual DbSet<DmdVmpDrugform> DmdVmpDrugform { get; set; }
        public virtual DbSet<DmdVmpDrugroute> DmdVmpDrugroute { get; set; }
        public virtual DbSet<DmdVmpIngredient> DmdVmpIngredient { get; set; }
        public virtual DbSet<DmdVmpOntdrugform> DmdVmpOntdrugform { get; set; }
        public virtual DbSet<DmdVtm> DmdVtm { get; set; }
        public virtual DbSet<Excelimport> Excelimport { get; set; }
        public virtual DbSet<FormularyAdditionalCode> FormularyAdditionalCode { get; set; }
        public virtual DbSet<FormularyDetail> FormularyDetail { get; set; }
        public virtual DbSet<FormularyExcipient> FormularyExcipient { get; set; }
        public virtual DbSet<FormularyHeader> FormularyHeader { get; set; }
        public virtual DbSet<FormularyIndication> FormularyIndication { get; set; }
        public virtual DbSet<FormularyIngredient> FormularyIngredient { get; set; }
        public virtual DbSet<FormularyLocalRouteDetail> FormularyLocalRouteDetail { get; set; }
        public virtual DbSet<FormularyOntologyForm> FormularyOntologyForm { get; set; }
        public virtual DbSet<FormularyRouteDetail> FormularyRouteDetail { get; set; }
        public virtual DbSet<FormularyRuleConfig> FormularyRuleConfig { get; set; }
        public virtual DbSet<LookupCommon> LookupCommon { get; set; }
        public virtual DbSet<SnomedctAssociationrefsetF> SnomedctAssociationrefsetF { get; set; }
        public virtual DbSet<SnomedctAttributevaluerefsetF> SnomedctAttributevaluerefsetF { get; set; }
        public virtual DbSet<SnomedctComplexmaprefsetF> SnomedctComplexmaprefsetF { get; set; }
        public virtual DbSet<SnomedctConceptAllLookupMat> SnomedctConceptAllLookupMat { get; set; }
        public virtual DbSet<SnomedctConceptAllnameLatestMat> SnomedctConceptAllnameLatestMat { get; set; }
        public virtual DbSet<SnomedctConceptF> SnomedctConceptF { get; set; }
        public virtual DbSet<SnomedctConceptLatestMat> SnomedctConceptLatestMat { get; set; }
        public virtual DbSet<SnomedctConceptLookupMat> SnomedctConceptLookupMat { get; set; }
        public virtual DbSet<SnomedctConceptpreferrednameLatestMat> SnomedctConceptpreferrednameLatestMat { get; set; }
        public virtual DbSet<SnomedctDescriptionF> SnomedctDescriptionF { get; set; }
        public virtual DbSet<SnomedctDescriptionLatestMat> SnomedctDescriptionLatestMat { get; set; }
        public virtual DbSet<SnomedctExtendedmaprefsetF> SnomedctExtendedmaprefsetF { get; set; }
        public virtual DbSet<SnomedctLangrefsetF> SnomedctLangrefsetF { get; set; }
        public virtual DbSet<SnomedctLangrefsetLatestMat> SnomedctLangrefsetLatestMat { get; set; }
        public virtual DbSet<SnomedctLookupSemantictag> SnomedctLookupSemantictag { get; set; }
        public virtual DbSet<SnomedctModifiedReleaseMat> SnomedctModifiedReleaseMat { get; set; }
        public virtual DbSet<SnomedctRelationActiveIsaLookupMat> SnomedctRelationActiveIsaLookupMat { get; set; }
        public virtual DbSet<SnomedctRelationshipF> SnomedctRelationshipF { get; set; }
        public virtual DbSet<SnomedctRelationshipLatestMat> SnomedctRelationshipLatestMat { get; set; }
        public virtual DbSet<SnomedctRelationshipwithnamesLatestMat> SnomedctRelationshipwithnamesLatestMat { get; set; }
        public virtual DbSet<SnomedctSimplemaprefsetF> SnomedctSimplemaprefsetF { get; set; }
        public virtual DbSet<SnomedctSimplerefsetF> SnomedctSimplerefsetF { get; set; }
        public virtual DbSet<SnomedctSimplerefsetLatestMat> SnomedctSimplerefsetLatestMat { get; set; }
        public virtual DbSet<SnomedctStatedRelationshipF> SnomedctStatedRelationshipF { get; set; }
        public virtual DbSet<SnomedctTextdefinitionF> SnomedctTextdefinitionF { get; set; }
        public virtual DbSet<SnomedctTradefamiliesMat> SnomedctTradefamiliesMat { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //            if (!optionsBuilder.IsConfigured)
            //            {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            //                optionsBuilder.UseNpgsql("Server=XXXXXXX;User Id=XXXXXXX;Password=XXXXXX;Database=XXXXXX;Port=xxxxxx;SSL Mode=Require;");
            //            }

            if (!optionsBuilder.IsConfigured)
            {
                var connString = _configuration.GetValue<string>("TerminologyConfig:Connectionstring");
                optionsBuilder.UseNpgsql(connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<AtcLookup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("atc_lookup", "terminology");

                entity.Property(e => e.AtcId)
                    .HasMaxLength(255)
                    .HasColumnName("atc_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Cd)
                    .HasMaxLength(255)
                    .HasColumnName("cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc).HasColumnName("desc");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.ShortCd)
                    .HasMaxLength(255)
                    .HasColumnName("short_cd");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<BnfLookup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bnf_lookup", "terminology");

                entity.Property(e => e.BnfId)
                    .HasMaxLength(255)
                    .HasColumnName("bnf_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Cd)
                    .HasMaxLength(255)
                    .HasColumnName("cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdAmp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_amp", "terminology");

                entity.Property(e => e.Abbrevnm)
                    .HasMaxLength(1000)
                    .HasColumnName("abbrevnm");

                entity.Property(e => e.AmpId)
                    .HasMaxLength(255)
                    .HasColumnName("amp_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Apid)
                    .HasMaxLength(255)
                    .HasColumnName("apid");

                entity.Property(e => e.AvailRestrictcd).HasColumnName("avail_restrictcd");

                entity.Property(e => e.ColValHash).HasColumnName("col_val_hash");

                entity.Property(e => e.Combprodcd).HasColumnName("combprodcd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.Ema).HasColumnName("ema");

                entity.Property(e => e.Flavourcd).HasColumnName("flavourcd");

                entity.Property(e => e.Invalid).HasColumnName("invalid");

                entity.Property(e => e.LicAuthPrevcd).HasColumnName("lic_auth_prevcd");

                entity.Property(e => e.LicAuthcd).HasColumnName("lic_authcd");

                entity.Property(e => e.LicAuthchangecd).HasColumnName("lic_authchangecd");

                entity.Property(e => e.LicAuthchangedt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("lic_authchangedt");

                entity.Property(e => e.Nm)
                    .HasMaxLength(1000)
                    .HasColumnName("nm");

                entity.Property(e => e.NmPrev)
                    .HasMaxLength(1000)
                    .HasColumnName("nm_prev");

                entity.Property(e => e.Nmdt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("nmdt");

                entity.Property(e => e.ParallelImport).HasColumnName("parallel_import");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Suppcd)
                    .HasMaxLength(255)
                    .HasColumnName("suppcd");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Vpid)
                    .HasMaxLength(255)
                    .HasColumnName("vpid");
            });

            modelBuilder.Entity<DmdAmpDrugroute>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_amp_drugroute", "terminology");

                entity.Property(e => e.AdrId)
                    .HasMaxLength(255)
                    .HasColumnName("adr_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Apid)
                    .HasMaxLength(255)
                    .HasColumnName("apid");

                entity.Property(e => e.ColValHash).HasColumnName("col_val_hash");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Routecd)
                    .HasMaxLength(255)
                    .HasColumnName("routecd");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdAmpExcipient>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_amp_excipient", "terminology");

                entity.Property(e => e.AexId)
                    .HasMaxLength(255)
                    .HasColumnName("aex_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Apid)
                    .HasMaxLength(255)
                    .HasColumnName("apid");

                entity.Property(e => e.ColValHash).HasColumnName("col_val_hash");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Isid)
                    .HasMaxLength(255)
                    .HasColumnName("isid");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Strnth).HasColumnName("strnth");

                entity.Property(e => e.StrnthUomcd)
                    .HasMaxLength(255)
                    .HasColumnName("strnth_uomcd");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdAtc>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_atc", "terminology");

                entity.Property(e => e.AtcCd)
                    .HasMaxLength(255)
                    .HasColumnName("atc_cd");

                entity.Property(e => e.AtcShortCd)
                    .HasMaxLength(255)
                    .HasColumnName("atc_short_cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.DatId)
                    .HasMaxLength(255)
                    .HasColumnName("dat_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.DmdCd)
                    .HasMaxLength(255)
                    .HasColumnName("dmd_cd");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdBnf>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_bnf", "terminology");

                entity.Property(e => e.BnfCd)
                    .HasMaxLength(255)
                    .HasColumnName("bnf_cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.DbnId)
                    .HasMaxLength(255)
                    .HasColumnName("dbn_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.DmdCd)
                    .HasMaxLength(255)
                    .HasColumnName("dmd_cd");

                entity.Property(e => e.DmdLevel)
                    .HasMaxLength(255)
                    .HasColumnName("dmd_level");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupAvailrestrict>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_availrestrict", "terminology");

                entity.Property(e => e.Cd).HasColumnName("cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.LarId)
                    .HasMaxLength(255)
                    .HasColumnName("lar_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupBasisofname>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_basisofname", "terminology");

                entity.Property(e => e.BonId)
                    .HasMaxLength(255)
                    .HasColumnName("bon_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Cd).HasColumnName("cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupBasisofstrength>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_basisofstrength", "terminology");

                entity.Property(e => e.Cd).HasColumnName("cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.LbsId)
                    .HasMaxLength(255)
                    .HasColumnName("lbs_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupControldrugcat>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_controldrugcat", "terminology");

                entity.Property(e => e.Cd).HasColumnName("cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.LcdId)
                    .HasMaxLength(255)
                    .HasColumnName("lcd_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupDrugformind>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_drugformind", "terminology");

                entity.Property(e => e.Cd).HasColumnName("cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.LfiId)
                    .HasMaxLength(255)
                    .HasColumnName("lfi_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupForm>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_form", "terminology");

                entity.Property(e => e.Cd)
                    .HasMaxLength(255)
                    .HasColumnName("cd");

                entity.Property(e => e.Cddt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("cddt");

                entity.Property(e => e.Cdprev)
                    .HasMaxLength(255)
                    .HasColumnName("cdprev");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.IsLatest).HasColumnName("is_latest");

                entity.Property(e => e.LfrId)
                    .HasMaxLength(255)
                    .HasColumnName("lfr_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupIngredient>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_ingredient", "terminology");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Invalid).HasColumnName("invalid");

                entity.Property(e => e.IsLatest).HasColumnName("is_latest");

                entity.Property(e => e.Isid)
                    .HasMaxLength(255)
                    .HasColumnName("isid");

                entity.Property(e => e.Isiddt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("isiddt");

                entity.Property(e => e.Isidprev)
                    .HasMaxLength(255)
                    .HasColumnName("isidprev");

                entity.Property(e => e.LinId)
                    .HasMaxLength(255)
                    .HasColumnName("lin_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Nm)
                    .HasMaxLength(1000)
                    .HasColumnName("nm");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupLicauth>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_licauth", "terminology");

                entity.Property(e => e.Cd).HasColumnName("cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.LauId)
                    .HasMaxLength(255)
                    .HasColumnName("lau_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupOntformroute>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_ontformroute", "terminology");

                entity.Property(e => e.Cd).HasColumnName("cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.OfrId)
                    .HasMaxLength(255)
                    .HasColumnName("ofr_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupPrescribingstatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_prescribingstatus", "terminology");

                entity.Property(e => e.Cd).HasColumnName("cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.LpsId)
                    .HasMaxLength(255)
                    .HasColumnName("lps_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupRoute>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_route", "terminology");

                entity.Property(e => e.Cd)
                    .HasMaxLength(255)
                    .HasColumnName("cd");

                entity.Property(e => e.Cddt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("cddt");

                entity.Property(e => e.Cdprev)
                    .HasMaxLength(255)
                    .HasColumnName("cdprev");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.IsLatest).HasColumnName("is_latest");

                entity.Property(e => e.LrtId)
                    .HasMaxLength(255)
                    .HasColumnName("lrt_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Source)
                    .HasMaxLength(50)
                    .HasColumnName("source");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupSupplier>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_supplier", "terminology");

                entity.Property(e => e.Cd)
                    .HasMaxLength(255)
                    .HasColumnName("cd");

                entity.Property(e => e.Cddt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("cddt");

                entity.Property(e => e.Cdprev)
                    .HasMaxLength(255)
                    .HasColumnName("cdprev");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.Invalid).HasColumnName("invalid");

                entity.Property(e => e.IsLatest).HasColumnName("is_latest");

                entity.Property(e => e.LsuId)
                    .HasMaxLength(255)
                    .HasColumnName("lsu_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
            });

            modelBuilder.Entity<DmdLookupUom>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_lookup_uom", "terminology");

                entity.Property(e => e.Cd)
                    .HasMaxLength(255)
                    .HasColumnName("cd");

                entity.Property(e => e.Cddt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("cddt");

                entity.Property(e => e.Cdprev)
                    .HasMaxLength(255)
                    .HasColumnName("cdprev");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.IsLatest).HasColumnName("is_latest");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.UomId)
                    .HasMaxLength(255)
                    .HasColumnName("uom_id")
                    .HasDefaultValueSql("uuid_generate_v4()");
            });

            modelBuilder.Entity<DmdNamesLookupAllMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("dmd_names_lookup_all_mat", "terminology");

                entity.Property(e => e.Availrestrictcd)
                    .HasMaxLength(100)
                    .HasColumnName("availrestrictcd");

                entity.Property(e => e.BasisPharmaceuticalStrengthCd).HasColumnName("basis_pharmaceutical_strength_cd");

                entity.Property(e => e.BasisStrengthSubstanceId)
                    .HasMaxLength(255)
                    .HasColumnName("basis_strength_substance_id");

                entity.Property(e => e.Basiscd)
                    .HasMaxLength(100)
                    .HasColumnName("basiscd");

                entity.Property(e => e.Cfcf)
                    .HasMaxLength(100)
                    .HasColumnName("cfcf");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.ControlDrugCategory)
                    .HasMaxLength(1000)
                    .HasColumnName("control_drug_category");

                entity.Property(e => e.ControlDrugCategoryCode).HasColumnName("control_drug_category_code");

                entity.Property(e => e.Dfindcd)
                    .HasMaxLength(100)
                    .HasColumnName("dfindcd");

                entity.Property(e => e.Ema)
                    .HasMaxLength(100)
                    .HasColumnName("ema");

                entity.Property(e => e.Gluf)
                    .HasMaxLength(100)
                    .HasColumnName("gluf");

                entity.Property(e => e.IngredientSubstanceId)
                    .HasMaxLength(255)
                    .HasColumnName("ingredient_substance_id");

                entity.Property(e => e.Licauthcd)
                    .HasMaxLength(100)
                    .HasColumnName("licauthcd");

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("name");

                entity.Property(e => e.NameTokens).HasColumnName("name_tokens");

                entity.Property(e => e.Ontcd).HasColumnName("ontcd");

                entity.Property(e => e.Parallelimport)
                    .HasMaxLength(100)
                    .HasColumnName("parallelimport");

                entity.Property(e => e.PrescribingStatus)
                    .HasMaxLength(1000)
                    .HasColumnName("prescribing_status");

                entity.Property(e => e.PrescribingStatusCode).HasColumnName("prescribing_status_code");

                entity.Property(e => e.Presf)
                    .HasMaxLength(100)
                    .HasColumnName("presf");

                entity.Property(e => e.Prevcode)
                    .HasMaxLength(255)
                    .HasColumnName("prevcode");

                entity.Property(e => e.StrengthValDnmtr).HasColumnName("strength_val_dnmtr");

                entity.Property(e => e.StrengthValNmtr).HasColumnName("strength_val_nmtr");

                entity.Property(e => e.StrengthValueDnmtrUnitCd)
                    .HasMaxLength(255)
                    .HasColumnName("strength_value_dnmtr_unit_cd");

                entity.Property(e => e.StrengthValueNmtrUnitCd)
                    .HasMaxLength(255)
                    .HasColumnName("strength_value_nmtr_unit_cd");

                entity.Property(e => e.Sugf)
                    .HasMaxLength(100)
                    .HasColumnName("sugf");

                entity.Property(e => e.Supplier)
                    .HasColumnType("character varying")
                    .HasColumnName("supplier");

                entity.Property(e => e.SupplierCode)
                    .HasMaxLength(255)
                    .HasColumnName("supplier_code");

                entity.Property(e => e.SupplierNameTokens).HasColumnName("supplier_name_tokens");

                entity.Property(e => e.Udfs).HasColumnName("udfs");

                entity.Property(e => e.Udfsuomcd)
                    .HasMaxLength(100)
                    .HasColumnName("udfsuomcd");

                entity.Property(e => e.Unitdoseuomcd)
                    .HasMaxLength(100)
                    .HasColumnName("unitdoseuomcd");

                entity.Property(e => e.Vmpform)
                    .HasMaxLength(1000)
                    .HasColumnName("vmpform");

                entity.Property(e => e.VmpformCode)
                    .HasMaxLength(255)
                    .HasColumnName("vmpform_code");

                entity.Property(e => e.Vmproute)
                    .HasMaxLength(1000)
                    .HasColumnName("vmproute");

                entity.Property(e => e.VmprouteCode)
                    .HasMaxLength(255)
                    .HasColumnName("vmproute_code");
            });

            modelBuilder.Entity<DmdNamesLookupMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("dmd_names_lookup_mat", "terminology");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.ControlDrugCategory)
                    .HasMaxLength(1000)
                    .HasColumnName("control_drug_category");

                entity.Property(e => e.ControlDrugCategoryCode).HasColumnName("control_drug_category_code");

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("name");

                entity.Property(e => e.NameTokens).HasColumnName("name_tokens");

                entity.Property(e => e.PrescribingStatus)
                    .HasMaxLength(1000)
                    .HasColumnName("prescribing_status");

                entity.Property(e => e.PrescribingStatusCode).HasColumnName("prescribing_status_code");

                entity.Property(e => e.Supplier)
                    .HasMaxLength(1000)
                    .HasColumnName("supplier");

                entity.Property(e => e.SupplierCode)
                    .HasMaxLength(255)
                    .HasColumnName("supplier_code");

                entity.Property(e => e.SupplierNameTokens).HasColumnName("supplier_name_tokens");

                entity.Property(e => e.Vmpform)
                    .HasMaxLength(1000)
                    .HasColumnName("vmpform");

                entity.Property(e => e.VmpformCode)
                    .HasMaxLength(255)
                    .HasColumnName("vmpform_code");

                entity.Property(e => e.Vmproute)
                    .HasMaxLength(1000)
                    .HasColumnName("vmproute");

                entity.Property(e => e.VmprouteCode)
                    .HasMaxLength(255)
                    .HasColumnName("vmproute_code");
            });

            modelBuilder.Entity<DmdRelationshipsMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("dmd_relationships_mat", "terminology");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.LogicalLevel).HasColumnName("logical_level");

                entity.Property(e => e.ParentCode).HasColumnName("parent_code");

                entity.Property(e => e.ParentLevel).HasColumnName("parent_level");

                entity.Property(e => e.ParentLogicalLevel).HasColumnName("parent_logical_level");
            });

            modelBuilder.Entity<DmdSnomedVersion>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_snomed_version", "terminology");

                entity.Property(e => e.Dmdversion)
                    .HasColumnType("character varying")
                    .HasColumnName("dmdversion");

                entity.Property(e => e.Snomedversion)
                    .HasColumnType("character varying")
                    .HasColumnName("snomedversion");
            });

            modelBuilder.Entity<DmdSyncLog>(entity =>
            {
                entity.HasKey(e => e.SlId)
                    .HasName("dmd_sync_log_pk");

                entity.ToTable("dmd_sync_log", "terminology");

                entity.Property(e => e.SlId)
                    .HasMaxLength(255)
                    .HasColumnName("sl_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CreatedDt)
                    .HasColumnName("created_dt")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DmdEntityName)
                    .HasMaxLength(255)
                    .HasColumnName("dmd_entity_name");

                entity.Property(e => e.DmdId)
                    .HasMaxLength(1000)
                    .HasColumnName("dmd_id");

                entity.Property(e => e.DmdUpdateDt).HasColumnName("dmd_update_dt");

                entity.Property(e => e.DmdVersion)
                    .HasMaxLength(100)
                    .HasColumnName("dmd_version");

                entity.Property(e => e.FormularyUpdateDt).HasColumnName("formulary_update_dt");

                entity.Property(e => e.IsDmdUpdated).HasColumnName("is_dmd_updated");

                entity.Property(e => e.IsFormularyUpdated).HasColumnName("is_formulary_updated");

                entity.Property(e => e.RowAction)
                    .HasMaxLength(10)
                    .HasColumnName("row_action");

                entity.Property(e => e.SerialNum)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("serial_num");

                entity.Property(e => e.SyncProcessId)
                    .HasMaxLength(255)
                    .HasColumnName("sync_process_id");
            });

            modelBuilder.Entity<DmdVmp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_vmp", "terminology");

                entity.Property(e => e.Abbrevnm)
                    .HasMaxLength(1000)
                    .HasColumnName("abbrevnm");

                entity.Property(e => e.BasisPrevcd).HasColumnName("basis_prevcd");

                entity.Property(e => e.Basiscd).HasColumnName("basiscd");

                entity.Property(e => e.CfcF).HasColumnName("cfc_f");

                entity.Property(e => e.ColValHash).HasColumnName("col_val_hash");

                entity.Property(e => e.Comprodcd).HasColumnName("comprodcd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.DfIndcd).HasColumnName("df_indcd");

                entity.Property(e => e.GluF).HasColumnName("glu_f");

                entity.Property(e => e.Invalid).HasColumnName("invalid");

                entity.Property(e => e.Nm)
                    .HasMaxLength(1000)
                    .HasColumnName("nm");

                entity.Property(e => e.Nmchangecd).HasColumnName("nmchangecd");

                entity.Property(e => e.Nmdt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("nmdt");

                entity.Property(e => e.Nmprev)
                    .HasMaxLength(1000)
                    .HasColumnName("nmprev");

                entity.Property(e => e.NonAvailcd).HasColumnName("non_availcd");

                entity.Property(e => e.NonAvaildt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("non_availdt");

                entity.Property(e => e.PresF).HasColumnName("pres_f");

                entity.Property(e => e.PresStatcd).HasColumnName("pres_statcd");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.SugF).HasColumnName("sug_f");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Udfs).HasColumnName("udfs");

                entity.Property(e => e.UdfsUomcd)
                    .HasMaxLength(255)
                    .HasColumnName("udfs_uomcd");

                entity.Property(e => e.UnitDoseUomcd)
                    .HasMaxLength(255)
                    .HasColumnName("unit_dose_uomcd");

                entity.Property(e => e.VmpId)
                    .HasMaxLength(255)
                    .HasColumnName("vmp_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Vpid)
                    .HasMaxLength(255)
                    .HasColumnName("vpid");

                entity.Property(e => e.Vpiddt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("vpiddt");

                entity.Property(e => e.Vpidprev)
                    .HasMaxLength(255)
                    .HasColumnName("vpidprev");

                entity.Property(e => e.Vtmid)
                    .HasMaxLength(255)
                    .HasColumnName("vtmid");
            });

            modelBuilder.Entity<DmdVmpControldrug>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_vmp_controldrug", "terminology");

                entity.Property(e => e.CatPrevcd).HasColumnName("cat_prevcd");

                entity.Property(e => e.Catcd).HasColumnName("catcd");

                entity.Property(e => e.Catdt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("catdt");

                entity.Property(e => e.ColValHash).HasColumnName("col_val_hash");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.VcdId)
                    .HasMaxLength(255)
                    .HasColumnName("vcd_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Vpid)
                    .HasMaxLength(255)
                    .HasColumnName("vpid");
            });

            modelBuilder.Entity<DmdVmpDrugform>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_vmp_drugform", "terminology");

                entity.Property(e => e.ColValHash).HasColumnName("col_val_hash");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Formcd)
                    .HasMaxLength(255)
                    .HasColumnName("formcd");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.VdfId)
                    .HasMaxLength(255)
                    .HasColumnName("vdf_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Vpid)
                    .HasMaxLength(255)
                    .HasColumnName("vpid");
            });

            modelBuilder.Entity<DmdVmpDrugroute>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_vmp_drugroute", "terminology");

                entity.Property(e => e.ColValHash).HasColumnName("col_val_hash");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Routecd)
                    .HasMaxLength(255)
                    .HasColumnName("routecd");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.VdrId)
                    .HasMaxLength(255)
                    .HasColumnName("vdr_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Vpid)
                    .HasMaxLength(255)
                    .HasColumnName("vpid");
            });

            modelBuilder.Entity<DmdVmpIngredient>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_vmp_ingredient", "terminology");

                entity.Property(e => e.BasisStrntcd).HasColumnName("basis_strntcd");

                entity.Property(e => e.BsSubid)
                    .HasMaxLength(255)
                    .HasColumnName("bs_subid");

                entity.Property(e => e.ColValHash).HasColumnName("col_val_hash");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Isid)
                    .HasMaxLength(255)
                    .HasColumnName("isid");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.StrntDnmtrUomcd)
                    .HasMaxLength(255)
                    .HasColumnName("strnt_dnmtr_uomcd");

                entity.Property(e => e.StrntDnmtrVal).HasColumnName("strnt_dnmtr_val");

                entity.Property(e => e.StrntNmrtrUomcd)
                    .HasMaxLength(255)
                    .HasColumnName("strnt_nmrtr_uomcd");

                entity.Property(e => e.StrntNmrtrVal).HasColumnName("strnt_nmrtr_val");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.VinId)
                    .HasMaxLength(255)
                    .HasColumnName("vin_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Vpid)
                    .HasMaxLength(255)
                    .HasColumnName("vpid");
            });

            modelBuilder.Entity<DmdVmpOntdrugform>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_vmp_ontdrugform", "terminology");

                entity.Property(e => e.ColValHash).HasColumnName("col_val_hash");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Formcd).HasColumnName("formcd");

                entity.Property(e => e.OdfId)
                    .HasMaxLength(255)
                    .HasColumnName("odf_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Vpid)
                    .HasMaxLength(255)
                    .HasColumnName("vpid");
            });

            modelBuilder.Entity<DmdVtm>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dmd_vtm", "terminology");

                entity.Property(e => e.Abbrevnm)
                    .HasMaxLength(1000)
                    .HasColumnName("abbrevnm");

                entity.Property(e => e.ColValHash).HasColumnName("col_val_hash");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Invalid).HasColumnName("invalid");

                entity.Property(e => e.Nm)
                    .HasMaxLength(1000)
                    .HasColumnName("nm");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.VtmId)
                    .HasMaxLength(255)
                    .HasColumnName("vtm_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Vtmid1)
                    .HasMaxLength(255)
                    .HasColumnName("vtmid");

                entity.Property(e => e.Vtmiddt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("vtmiddt");

                entity.Property(e => e.Vtmidprev)
                    .HasMaxLength(255)
                    .HasColumnName("vtmidprev");
            });

            modelBuilder.Entity<Excelimport>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("excelimport", "local_formulary");

                entity.Property(e => e.Code)
                    .HasColumnType("character varying")
                    .HasColumnName("code");

                entity.Property(e => e.CriticalDrug)
                    .HasColumnType("character varying")
                    .HasColumnName("critical_drug");

                entity.Property(e => e.ExpensiveMedication)
                    .HasColumnType("character varying")
                    .HasColumnName("expensive_medication");

                entity.Property(e => e.FormularyStatus)
                    .HasColumnType("character varying")
                    .HasColumnName("formulary_status");

                entity.Property(e => e.IndicationMandatory)
                    .HasColumnType("character varying")
                    .HasColumnName("indication_mandatory");

                entity.Property(e => e.Level)
                    .HasColumnType("character varying")
                    .HasColumnName("level");

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("name");

                entity.Property(e => e.UnlicensedRouteCode)
                    .HasColumnType("character varying")
                    .HasColumnName("unlicensed_route_code");

                entity.Property(e => e.UnlicensedRouteDesc)
                    .HasColumnType("character varying")
                    .HasColumnName("unlicensed_route_desc");
            });

            modelBuilder.Entity<FormularyAdditionalCode>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("formulary_additional_code_pk");

                entity.ToTable("formulary_additional_code", "local_formulary");

                entity.HasIndex(e => e.FormularyVersionId, "formulary_additional_code_formulary_version_id_idx");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.AdditionalCode)
                    .HasMaxLength(500)
                    .HasColumnName("additional_code");

                entity.Property(e => e.AdditionalCodeDesc).HasColumnName("additional_code_desc");

                entity.Property(e => e.AdditionalCodeSystem)
                    .HasMaxLength(500)
                    .HasColumnName("additional_code_system");

                entity.Property(e => e.Attr1).HasColumnName("attr1");

                entity.Property(e => e.CodeType)
                    .HasMaxLength(1000)
                    .HasColumnName("code_type")
                    .HasDefaultValueSql("'Classification'::character varying");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.FormularyVersionId)
                    .HasMaxLength(255)
                    .HasColumnName("formulary_version_id");

                entity.Property(e => e.MetaJson).HasColumnName("meta_json");

                entity.Property(e => e.Source)
                    .HasMaxLength(500)
                    .HasColumnName("source")
                    .HasDefaultValueSql("'M'::character varying");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(255)
                    .HasColumnName("_updatedby");

                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_updateddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Updatedtimestamp)
                    .HasColumnName("_updatedtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.HasOne(d => d.FormularyVersion)
                    .WithMany(p => p.FormularyAdditionalCode)
                    .HasForeignKey(d => d.FormularyVersionId)
                    .HasConstraintName("formulary_additional_code_fk");
            });

            modelBuilder.Entity<FormularyDetail>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("formulary_detail_pk");

                entity.ToTable("formulary_detail", "local_formulary");

                entity.HasIndex(e => e.FormularyVersionId, "formulary_detail_formulary_version_id_idx");

                entity.HasIndex(e => e.RnohFormularyStatuscd, "formulary_detail_rnoh_formulary_statuscd_idx");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.AddReviewReminder).HasColumnName("add_review_reminder");

                entity.Property(e => e.Antibiotic)
                    .HasMaxLength(10)
                    .HasColumnName("antibiotic");

                entity.Property(e => e.Anticoagulant)
                    .HasMaxLength(10)
                    .HasColumnName("anticoagulant");

                entity.Property(e => e.Antimicrobial)
                    .HasMaxLength(10)
                    .HasColumnName("antimicrobial");

                entity.Property(e => e.Antipsychotic)
                    .HasMaxLength(10)
                    .HasColumnName("antipsychotic");

                entity.Property(e => e.BasisOfPreferredNameCd)
                    .HasMaxLength(50)
                    .HasColumnName("basis_of_preferred_name_cd");

                entity.Property(e => e.BlackTriangle)
                    .HasMaxLength(10)
                    .HasColumnName("black_triangle");

                entity.Property(e => e.BlackTriangleSource)
                    .HasMaxLength(100)
                    .HasColumnName("black_triangle_source");

                entity.Property(e => e.Caution).HasColumnName("caution");

                entity.Property(e => e.CfcFree)
                    .HasMaxLength(10)
                    .HasColumnName("cfc_free");

                entity.Property(e => e.ClinicalTrialMedication)
                    .HasMaxLength(10)
                    .HasColumnName("clinical_trial_medication");

                entity.Property(e => e.ContraIndication).HasColumnName("contra_indication");

                entity.Property(e => e.ControlledDrugCategoryCd)
                    .HasMaxLength(50)
                    .HasColumnName("controlled_drug_category_cd");

                entity.Property(e => e.ControlledDrugCategorySource)
                    .HasMaxLength(100)
                    .HasColumnName("controlled_drug_category_source");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.CriticalDrug)
                    .HasMaxLength(10)
                    .HasColumnName("critical_drug");

                entity.Property(e => e.CurrentLicensingAuthorityCd)
                    .HasMaxLength(50)
                    .HasColumnName("current_licensing_authority_cd");

                entity.Property(e => e.CustomWarning).HasColumnName("custom_warning");

                entity.Property(e => e.Cytotoxic)
                    .HasMaxLength(10)
                    .HasColumnName("cytotoxic");

                entity.Property(e => e.DefinedDailyDose)
                    .HasMaxLength(255)
                    .HasColumnName("defined_daily_dose");

                entity.Property(e => e.Diluent).HasColumnName("diluent");

                entity.Property(e => e.DoseFormCd)
                    .HasMaxLength(50)
                    .HasColumnName("dose_form_cd");

                entity.Property(e => e.DrugClass)
                    .HasMaxLength(255)
                    .HasColumnName("drug_class");

                entity.Property(e => e.EmaAdditionalMonitoring)
                    .HasMaxLength(10)
                    .HasColumnName("ema_additional_monitoring");

                entity.Property(e => e.Endorsement).HasColumnName("endorsement");

                entity.Property(e => e.ExpensiveMedication)
                    .HasMaxLength(50)
                    .HasColumnName("expensive_medication");

                entity.Property(e => e.Fluid)
                    .HasMaxLength(10)
                    .HasColumnName("fluid");

                entity.Property(e => e.FormCd)
                    .HasMaxLength(255)
                    .HasColumnName("form_cd");

                entity.Property(e => e.FormularyVersionId)
                    .HasMaxLength(255)
                    .HasColumnName("formulary_version_id");

                entity.Property(e => e.GlutenFree)
                    .HasMaxLength(10)
                    .HasColumnName("gluten_free");

                entity.Property(e => e.HighAlertMedication)
                    .HasMaxLength(10)
                    .HasColumnName("high_alert_medication");

                entity.Property(e => e.HighAlertMedicationSource)
                    .HasMaxLength(100)
                    .HasColumnName("high_alert_medication_source");

                entity.Property(e => e.IgnoreDuplicateWarnings)
                    .HasMaxLength(10)
                    .HasColumnName("ignore_duplicate_warnings");

                entity.Property(e => e.InpatientMedicationCd)
                    .HasMaxLength(50)
                    .HasColumnName("inpatient_medication_cd");

                entity.Property(e => e.Insulins)
                    .HasMaxLength(10)
                    .HasColumnName("insulins");

                entity.Property(e => e.IsBloodProduct).HasColumnName("is_blood_product");

                entity.Property(e => e.IsCustomControlledDrug).HasColumnName("is_custom_controlled_drug");

                entity.Property(e => e.IsDiluent).HasColumnName("is_diluent");

                entity.Property(e => e.IsGastroResistant).HasColumnName("is_gastro_resistant");

                entity.Property(e => e.IsIndicationMandatory).HasColumnName("is_indication_mandatory");

                entity.Property(e => e.IsModifiedRelease).HasColumnName("is_modified_release");

                entity.Property(e => e.IsPrescriptionPrintingRequired).HasColumnName("is_prescription_printing_required");

                entity.Property(e => e.IvToOral)
                    .HasMaxLength(10)
                    .HasColumnName("iv_to_oral");

                entity.Property(e => e.LicensedUse).HasColumnName("licensed_use");

                entity.Property(e => e.LocalLicensedUse).HasColumnName("local_licensed_use");

                entity.Property(e => e.LocalUnlicensedUse).HasColumnName("local_unlicensed_use");

                entity.Property(e => e.MarkedModifierCd)
                    .HasMaxLength(50)
                    .HasColumnName("marked_modifier_cd");

                entity.Property(e => e.MaxDoseNumerator)
                    .HasPrecision(100, 4)
                    .HasColumnName("max_dose_numerator");

                entity.Property(e => e.MaximumDoseUnitCd)
                    .HasMaxLength(50)
                    .HasColumnName("maximum_dose_unit_cd");

                entity.Property(e => e.MedicationTypeCode)
                    .HasMaxLength(50)
                    .HasColumnName("medication_type_code");

                entity.Property(e => e.MedusaPreparationInstructions).HasColumnName("medusa_preparation_instructions");

                entity.Property(e => e.MentalHealthDrug)
                    .HasMaxLength(10)
                    .HasColumnName("mental_health_drug");

                entity.Property(e => e.ModifiedReleaseCd)
                    .HasMaxLength(50)
                    .HasColumnName("modified_release_cd");

                entity.Property(e => e.NiceTa)
                    .HasMaxLength(255)
                    .HasColumnName("nice_ta");

                entity.Property(e => e.NotForPrn)
                    .HasMaxLength(10)
                    .HasColumnName("not_for_prn");

                entity.Property(e => e.OrderableFormtypeCd)
                    .HasMaxLength(50)
                    .HasColumnName("orderable_formtype_cd");

                entity.Property(e => e.OutpatientMedicationCd)
                    .HasMaxLength(50)
                    .HasColumnName("outpatient_medication_cd");

                entity.Property(e => e.ParallelImport)
                    .HasMaxLength(10)
                    .HasColumnName("parallel_import");

                entity.Property(e => e.Prescribable)
                    .HasColumnName("prescribable")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.PrescribableSource)
                    .HasMaxLength(100)
                    .HasColumnName("prescribable_source");

                entity.Property(e => e.PrescribingStatusCd)
                    .HasMaxLength(50)
                    .HasColumnName("prescribing_status_cd");

                entity.Property(e => e.PreservativeFree)
                    .HasMaxLength(10)
                    .HasColumnName("preservative_free");

                entity.Property(e => e.Reminder).HasColumnName("reminder");

                entity.Property(e => e.RestrictedPrescribing)
                    .HasMaxLength(10)
                    .HasColumnName("restricted_prescribing");

                entity.Property(e => e.RestrictionNote).HasColumnName("restriction_note");

                entity.Property(e => e.RestrictionsOnAvailabilityCd)
                    .HasMaxLength(50)
                    .HasColumnName("restrictions_on_availability_cd");

                entity.Property(e => e.RnohFormularyStatuscd)
                    .HasMaxLength(50)
                    .HasColumnName("rnoh_formulary_statuscd");

                entity.Property(e => e.RoundingFactorCd)
                    .HasMaxLength(50)
                    .HasColumnName("rounding_factor_cd");

                entity.Property(e => e.RulesCd)
                    .HasMaxLength(50)
                    .HasColumnName("rules_cd");

                entity.Property(e => e.SafetyMessage).HasColumnName("safety_message");

                entity.Property(e => e.SideEffect).HasColumnName("side_effect");

                entity.Property(e => e.SugarFree)
                    .HasMaxLength(10)
                    .HasColumnName("sugar_free");

                entity.Property(e => e.SupplierCd)
                    .HasMaxLength(50)
                    .HasColumnName("supplier_cd");

                entity.Property(e => e.SupplierName)
                    .HasMaxLength(1000)
                    .HasColumnName("supplier_name");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.TitrationTypeCd)
                    .HasMaxLength(50)
                    .HasColumnName("titration_type_cd");

                entity.Property(e => e.TradeFamilyCd)
                    .HasMaxLength(18)
                    .HasColumnName("trade_family_cd");

                entity.Property(e => e.TradeFamilyName)
                    .HasMaxLength(500)
                    .HasColumnName("trade_family_name");

                entity.Property(e => e.UnitDoseFormSize)
                    .HasPrecision(20, 4)
                    .HasColumnName("unit_dose_form_size");

                entity.Property(e => e.UnitDoseFormUnits)
                    .HasMaxLength(18)
                    .HasColumnName("unit_dose_form_units");

                entity.Property(e => e.UnitDoseUnitOfMeasureCd)
                    .HasMaxLength(50)
                    .HasColumnName("unit_dose_unit_of_measure_cd");

                entity.Property(e => e.UnlicensedMedicationCd)
                    .HasMaxLength(50)
                    .HasColumnName("unlicensed_medication_cd");

                entity.Property(e => e.UnlicensedUse).HasColumnName("unlicensed_use");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(255)
                    .HasColumnName("_updatedby");

                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_updateddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Updatedtimestamp)
                    .HasColumnName("_updatedtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.WitnessingRequired)
                    .HasMaxLength(10)
                    .HasColumnName("witnessing_required");

                entity.HasOne(d => d.FormularyVersion)
                    .WithMany(p => p.FormularyDetail)
                    .HasForeignKey(d => d.FormularyVersionId)
                    .HasConstraintName("formulary_detail_fk");
            });

            modelBuilder.Entity<FormularyExcipient>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("formulary_excipient_pk");

                entity.ToTable("formulary_excipient", "local_formulary");

                entity.HasIndex(e => e.FormularyVersionId, "formulary_excipient_formulary_version_id_idx");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.FormularyVersionId)
                    .HasMaxLength(255)
                    .HasColumnName("formulary_version_id");

                entity.Property(e => e.IngredientCd)
                    .HasMaxLength(18)
                    .HasColumnName("ingredient_cd");

                entity.Property(e => e.Strength)
                    .HasMaxLength(20)
                    .HasColumnName("strength");

                entity.Property(e => e.StrengthUnitCd)
                    .HasMaxLength(18)
                    .HasColumnName("strength_unit_cd");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(255)
                    .HasColumnName("_updatedby");

                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_updateddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Updatedtimestamp)
                    .HasColumnName("_updatedtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.HasOne(d => d.FormularyVersion)
                    .WithMany(p => p.FormularyExcipient)
                    .HasForeignKey(d => d.FormularyVersionId)
                    .HasConstraintName("formulary_excipient_fk");
            });

            modelBuilder.Entity<FormularyHeader>(entity =>
            {
                entity.HasKey(e => e.FormularyVersionId)
                    .HasName("formulary_header_pk");

                entity.ToTable("formulary_header", "local_formulary");

                entity.HasIndex(e => e.Code, "formulary_header_code_idx");

                entity.HasIndex(e => e.FormularyVersionId, "formulary_header_formulary_version_id_idx");

                entity.HasIndex(e => e.IsLatest, "formulary_header_is_latest_idx");

                entity.HasIndex(e => e.Name, "formulary_header_name_idx");

                entity.HasIndex(e => e.NameTokens, "formulary_header_name_tokens_idx")
                    .HasMethod("gin");

                entity.HasIndex(e => e.ParentCode, "formulary_header_parent_code_idx");

                entity.HasIndex(e => e.ProductType, "formulary_header_product_type_idx");

                entity.HasIndex(e => e.RecStatusCode, "formulary_header_rec_status_code_idx");

                entity.Property(e => e.FormularyVersionId)
                    .HasMaxLength(255)
                    .HasColumnName("formulary_version_id");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.CodeSystem)
                    .HasMaxLength(500)
                    .HasColumnName("code_system")
                    .HasDefaultValueSql("'DMD'::character varying");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.DuplicateOfFormularyId)
                    .HasMaxLength(255)
                    .HasColumnName("duplicate_of_formulary_id");

                entity.Property(e => e.FormularyId)
                    .HasMaxLength(255)
                    .HasColumnName("formulary_id");

                entity.Property(e => e.IsDuplicate).HasColumnName("is_duplicate");

                entity.Property(e => e.IsLatest).HasColumnName("is_latest");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.NameTokens).HasColumnName("name_tokens");

                entity.Property(e => e.ParentCode)
                    .HasMaxLength(255)
                    .HasColumnName("parent_code");

                entity.Property(e => e.ParentName).HasColumnName("parent_name");

                entity.Property(e => e.ParentNameTokens).HasColumnName("parent_name_tokens");

                entity.Property(e => e.ParentProductType)
                    .HasMaxLength(100)
                    .HasColumnName("parent_product_type");

                entity.Property(e => e.ProductType)
                    .HasMaxLength(100)
                    .HasColumnName("product_type");

                entity.Property(e => e.RecSource)
                    .HasMaxLength(50)
                    .HasColumnName("rec_source");

                entity.Property(e => e.RecStatusCode)
                    .HasMaxLength(8)
                    .HasColumnName("rec_status_code");

                entity.Property(e => e.RecStatuschangeDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("rec_statuschange_date");

                entity.Property(e => e.RecStatuschangeMsg).HasColumnName("rec_statuschange_msg");

                entity.Property(e => e.RecStatuschangeTs).HasColumnName("rec_statuschange_ts");

                entity.Property(e => e.RecStatuschangeTzname)
                    .HasMaxLength(255)
                    .HasColumnName("rec_statuschange_tzname");

                entity.Property(e => e.RecStatuschangeTzoffset).HasColumnName("rec_statuschange_tzoffset");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(255)
                    .HasColumnName("_updatedby");

                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_updateddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Updatedtimestamp)
                    .HasColumnName("_updatedtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.VersionId).HasColumnName("version_id");

                entity.Property(e => e.VmpId)
                    .HasMaxLength(100)
                    .HasColumnName("vmp_id");

                entity.Property(e => e.VtmId)
                    .HasMaxLength(100)
                    .HasColumnName("vtm_id");
            });

            modelBuilder.Entity<FormularyIndication>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("formulary_indication_pk");

                entity.ToTable("formulary_indication", "local_formulary");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.FormularyVersionId)
                    .HasMaxLength(255)
                    .HasColumnName("formulary_version_id");

                entity.Property(e => e.IndicationCd)
                    .HasMaxLength(50)
                    .HasColumnName("indication_cd");

                entity.Property(e => e.IndicationName)
                    .HasMaxLength(500)
                    .HasColumnName("indication_name");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(255)
                    .HasColumnName("_updatedby");

                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_updateddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Updatedtimestamp)
                    .HasColumnName("_updatedtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.HasOne(d => d.FormularyVersion)
                    .WithMany(p => p.FormularyIndication)
                    .HasForeignKey(d => d.FormularyVersionId)
                    .HasConstraintName("formulary_indication_fk");
            });

            modelBuilder.Entity<FormularyIngredient>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("formulary_ingredient_pk");

                entity.ToTable("formulary_ingredient", "local_formulary");

                entity.HasIndex(e => e.FormularyVersionId, "formulary_ingredient_formulary_version_id_idx");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.BasisOfPharmaceuticalStrengthCd)
                    .HasMaxLength(50)
                    .HasColumnName("basis_of_pharmaceutical_strength_cd");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.FormularyVersionId)
                    .HasMaxLength(255)
                    .HasColumnName("formulary_version_id");

                entity.Property(e => e.IngredientCd)
                    .HasMaxLength(18)
                    .HasColumnName("ingredient_cd");

                entity.Property(e => e.IngredientName)
                    .HasMaxLength(1000)
                    .HasColumnName("ingredient_name");

                entity.Property(e => e.StrengthValueDenominator)
                    .HasMaxLength(20)
                    .HasColumnName("strength_value_denominator");

                entity.Property(e => e.StrengthValueDenominatorUnitCd)
                    .HasMaxLength(20)
                    .HasColumnName("strength_value_denominator_unit_cd");

                entity.Property(e => e.StrengthValueNumerator)
                    .HasMaxLength(20)
                    .HasColumnName("strength_value_numerator");

                entity.Property(e => e.StrengthValueNumeratorUnitCd)
                    .HasMaxLength(18)
                    .HasColumnName("strength_value_numerator_unit_cd");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(255)
                    .HasColumnName("_updatedby");

                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_updateddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Updatedtimestamp)
                    .HasColumnName("_updatedtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.HasOne(d => d.FormularyVersion)
                    .WithMany(p => p.FormularyIngredient)
                    .HasForeignKey(d => d.FormularyVersionId)
                    .HasConstraintName("formulary_ingredient_fk");
            });

            modelBuilder.Entity<FormularyLocalRouteDetail>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("formulary_local_route_detail_pk");

                entity.ToTable("formulary_local_route_detail", "local_formulary");

                entity.HasIndex(e => e.FormularyVersionId, "formulary_local_route_detail_formulary_version_id_idx");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.FormularyVersionId)
                    .HasMaxLength(255)
                    .HasColumnName("formulary_version_id");

                entity.Property(e => e.RouteCd)
                    .HasMaxLength(50)
                    .HasColumnName("route_cd");

                entity.Property(e => e.RouteFieldTypeCd)
                    .HasMaxLength(50)
                    .HasColumnName("route_field_type_cd");

                entity.Property(e => e.Source)
                    .HasMaxLength(100)
                    .HasColumnName("source");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(255)
                    .HasColumnName("_updatedby");

                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_updateddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Updatedtimestamp)
                    .HasColumnName("_updatedtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.HasOne(d => d.FormularyVersion)
                    .WithMany(p => p.FormularyLocalRouteDetail)
                    .HasForeignKey(d => d.FormularyVersionId)
                    .HasConstraintName("formulary_local_route_detail_fk");
            });

            modelBuilder.Entity<FormularyOntologyForm>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("formulary_ontology_form_pk");

                entity.ToTable("formulary_ontology_form", "local_formulary");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.FormCd)
                    .HasMaxLength(50)
                    .HasColumnName("form_cd");

                entity.Property(e => e.FormularyVersionId)
                    .HasMaxLength(255)
                    .HasColumnName("formulary_version_id");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(255)
                    .HasColumnName("_updatedby");

                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_updateddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Updatedtimestamp)
                    .HasColumnName("_updatedtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.HasOne(d => d.FormularyVersion)
                    .WithMany(p => p.FormularyOntologyForm)
                    .HasForeignKey(d => d.FormularyVersionId)
                    .HasConstraintName("formulary_ontology_form_fk");
            });

            modelBuilder.Entity<FormularyRouteDetail>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("formulary_route_detail_pk");

                entity.ToTable("formulary_route_detail", "local_formulary");

                entity.HasIndex(e => e.FormularyVersionId, "formulary_route_detail_formulary_version_id_idx");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.FormularyVersionId)
                    .HasMaxLength(255)
                    .HasColumnName("formulary_version_id");

                entity.Property(e => e.RouteCd)
                    .HasMaxLength(50)
                    .HasColumnName("route_cd");

                entity.Property(e => e.RouteFieldTypeCd)
                    .HasMaxLength(50)
                    .HasColumnName("route_field_type_cd");

                entity.Property(e => e.Source)
                    .HasMaxLength(100)
                    .HasColumnName("source");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(255)
                    .HasColumnName("_updatedby");

                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_updateddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Updatedtimestamp)
                    .HasColumnName("_updatedtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.HasOne(d => d.FormularyVersion)
                    .WithMany(p => p.FormularyRouteDetail)
                    .HasForeignKey(d => d.FormularyVersionId)
                    .HasConstraintName("formulary_route_detail_fk");
            });

            modelBuilder.Entity<FormularyRuleConfig>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("formulary_rule_config_pkey");

                entity.ToTable("formulary_rule_config", "local_formulary");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.ConfigJson).HasColumnName("config_json");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(255)
                    .HasColumnName("_updatedby");

                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_updateddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Updatedtimestamp)
                    .HasColumnName("_updatedtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");
            });

            modelBuilder.Entity<LookupCommon>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("lookup_common", "local_formulary");

                entity.HasIndex(e => e.Cd, "lookup_common_cd_idx");

                entity.HasIndex(e => e.Desc, "lookup_common_desc_idx");

                entity.HasIndex(e => e.Type, "lookup_common_type_idx");

                entity.Property(e => e.Additionalproperties).HasColumnName("additionalproperties");

                entity.Property(e => e.Cd)
                    .HasMaxLength(50)
                    .HasColumnName("cd");

                entity.Property(e => e.Contextkey)
                    .HasMaxLength(255)
                    .HasColumnName("_contextkey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(255)
                    .HasColumnName("_createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Createdmessageid)
                    .HasMaxLength(255)
                    .HasColumnName("_createdmessageid");

                entity.Property(e => e.Createdsource)
                    .HasMaxLength(255)
                    .HasColumnName("_createdsource");

                entity.Property(e => e.Createdtimestamp)
                    .HasColumnName("_createdtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");

                entity.Property(e => e.Desc)
                    .HasMaxLength(1000)
                    .HasColumnName("desc");

                entity.Property(e => e.Isdefault).HasColumnName("isdefault");

                entity.Property(e => e.Recordstatus)
                    .HasColumnName("_recordstatus")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.RowId)
                    .HasMaxLength(255)
                    .HasColumnName("_row_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sequenceid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("_sequenceid");

                entity.Property(e => e.Tenant)
                    .HasMaxLength(255)
                    .HasColumnName("_tenant");

                entity.Property(e => e.Timezonename)
                    .HasMaxLength(255)
                    .HasColumnName("_timezonename");

                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .HasColumnName("type");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(255)
                    .HasColumnName("_updatedby");

                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("_updateddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Updatedtimestamp)
                    .HasColumnName("_updatedtimestamp")
                    .HasDefaultValueSql("timezone('UTC'::text, now())");
            });

            modelBuilder.Entity<SnomedctAssociationrefsetF>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_associationrefset_f", "terminology");

                entity.HasIndex(e => new { e.Referencedcomponentid, e.Targetcomponentid }, "snomedct_associationrefset_f_idx");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Effectivetime)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Moduleid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Referencedcomponentid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("referencedcomponentid");

                entity.Property(e => e.Refsetid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("refsetid");

                entity.Property(e => e.Targetcomponentid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("targetcomponentid");
            });

            modelBuilder.Entity<SnomedctAttributevaluerefsetF>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_attributevaluerefset_f", "terminology");

                entity.HasIndex(e => new { e.Referencedcomponentid, e.Valueid }, "snomedct_attributevaluerefset_f_idx");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Effectivetime)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Moduleid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Referencedcomponentid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("referencedcomponentid");

                entity.Property(e => e.Refsetid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("refsetid");

                entity.Property(e => e.Valueid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("valueid");
            });

            modelBuilder.Entity<SnomedctComplexmaprefsetF>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_complexmaprefset_f", "terminology");

                entity.HasIndex(e => e.Referencedcomponentid, "snomedct_complexmaprefset_referencedcomponentid_idx");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Correlationid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("correlationid");

                entity.Property(e => e.Effectivetime)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Mapadvice).HasColumnName("mapadvice");

                entity.Property(e => e.Mapgroup).HasColumnName("mapgroup");

                entity.Property(e => e.Mappriority).HasColumnName("mappriority");

                entity.Property(e => e.Maprule).HasColumnName("maprule");

                entity.Property(e => e.Maptarget).HasColumnName("maptarget");

                entity.Property(e => e.Moduleid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Referencedcomponentid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("referencedcomponentid");

                entity.Property(e => e.Refsetid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("refsetid");
            });

            modelBuilder.Entity<SnomedctConceptAllLookupMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_concept_all_lookup_mat", "terminology");

                entity.Property(e => e.Conceptid)
                    .HasMaxLength(18)
                    .HasColumnName("conceptid");

                entity.Property(e => e.Fsn).HasColumnName("fsn");

                entity.Property(e => e.PreferrednameTokens).HasColumnName("preferredname_tokens");

                entity.Property(e => e.Preferredterm).HasColumnName("preferredterm");

                entity.Property(e => e.Semantictag).HasColumnName("semantictag");
            });

            modelBuilder.Entity<SnomedctConceptAllnameLatestMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_concept_allname_latest_mat", "terminology");

                entity.Property(e => e.Conceptid)
                    .HasMaxLength(18)
                    .HasColumnName("conceptid");

                entity.Property(e => e.Descriptionid)
                    .HasMaxLength(18)
                    .HasColumnName("descriptionid");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.NameTokens).HasColumnName("name_tokens");
            });

            modelBuilder.Entity<SnomedctConceptF>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_concept_f", "terminology");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Definitionstatusid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("definitionstatusid");

                entity.Property(e => e.Effectivetime)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("id");

                entity.Property(e => e.Moduleid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");
            });

            modelBuilder.Entity<SnomedctConceptLatestMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_concept_latest_mat", "terminology");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Definitionstatusid)
                    .HasMaxLength(18)
                    .HasColumnName("definitionstatusid");

                entity.Property(e => e.Effectivetime)
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .HasMaxLength(18)
                    .HasColumnName("id");

                entity.Property(e => e.Moduleid)
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");
            });

            modelBuilder.Entity<SnomedctConceptLookupMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_concept_lookup_mat", "terminology");

                entity.Property(e => e.Conceptid)
                    .HasMaxLength(18)
                    .HasColumnName("conceptid");

                entity.Property(e => e.Fsn).HasColumnName("fsn");

                entity.Property(e => e.PreferrednameTokens).HasColumnName("preferredname_tokens");

                entity.Property(e => e.Preferredterm).HasColumnName("preferredterm");

                entity.Property(e => e.Semantictag).HasColumnName("semantictag");
            });

            modelBuilder.Entity<SnomedctConceptpreferrednameLatestMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_conceptpreferredname_latest_mat", "terminology");

                entity.Property(e => e.Conceptid)
                    .HasMaxLength(18)
                    .HasColumnName("conceptid");

                entity.Property(e => e.Descriptionid)
                    .HasMaxLength(18)
                    .HasColumnName("descriptionid");

                entity.Property(e => e.Preferredname).HasColumnName("preferredname");

                entity.Property(e => e.PreferrednameTokens).HasColumnName("preferredname_tokens");
            });

            modelBuilder.Entity<SnomedctDescriptionF>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_description_f", "terminology");

                entity.HasIndex(e => e.Conceptid, "snomedct_description_conceptid_idx");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Casesignificanceid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("casesignificanceid");

                entity.Property(e => e.Conceptid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("conceptid");

                entity.Property(e => e.Effectivetime)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("id");

                entity.Property(e => e.Languagecode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("languagecode");

                entity.Property(e => e.Moduleid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Term)
                    .IsRequired()
                    .HasColumnName("term");

                entity.Property(e => e.Typeid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("typeid");
            });

            modelBuilder.Entity<SnomedctDescriptionLatestMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_description_latest_mat", "terminology");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Casesignificanceid)
                    .HasMaxLength(18)
                    .HasColumnName("casesignificanceid");

                entity.Property(e => e.Conceptid)
                    .HasMaxLength(18)
                    .HasColumnName("conceptid");

                entity.Property(e => e.Effectivetime)
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .HasMaxLength(18)
                    .HasColumnName("id");

                entity.Property(e => e.Languagecode)
                    .HasMaxLength(2)
                    .HasColumnName("languagecode");

                entity.Property(e => e.Moduleid)
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Term).HasColumnName("term");

                entity.Property(e => e.TermTokens).HasColumnName("term_tokens");

                entity.Property(e => e.Typeid)
                    .HasMaxLength(18)
                    .HasColumnName("typeid");
            });

            modelBuilder.Entity<SnomedctExtendedmaprefsetF>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_extendedmaprefset_f", "terminology");

                entity.HasIndex(e => e.Referencedcomponentid, "snomedct_extendedmaprefset_referencedcomponentid_idx");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Correlationid)
                    .HasMaxLength(18)
                    .HasColumnName("correlationid");

                entity.Property(e => e.Effectivetime)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Mapadvice).HasColumnName("mapadvice");

                entity.Property(e => e.Mapcategoryid)
                    .HasMaxLength(18)
                    .HasColumnName("mapcategoryid");

                entity.Property(e => e.Mapgroup).HasColumnName("mapgroup");

                entity.Property(e => e.Mappriority).HasColumnName("mappriority");

                entity.Property(e => e.Maprule).HasColumnName("maprule");

                entity.Property(e => e.Maptarget).HasColumnName("maptarget");

                entity.Property(e => e.Moduleid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Referencedcomponentid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("referencedcomponentid");

                entity.Property(e => e.Refsetid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("refsetid");
            });

            modelBuilder.Entity<SnomedctLangrefsetF>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_langrefset_f", "terminology");

                entity.HasIndex(e => e.Referencedcomponentid, "snomedct_langrefset_referencedcomponentid_idx");

                entity.Property(e => e.Acceptabilityid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("acceptabilityid");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Effectivetime)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Moduleid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Referencedcomponentid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("referencedcomponentid");

                entity.Property(e => e.Refsetid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("refsetid");
            });

            modelBuilder.Entity<SnomedctLangrefsetLatestMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_langrefset_latest_mat", "terminology");

                entity.Property(e => e.Acceptabilityid)
                    .HasMaxLength(18)
                    .HasColumnName("acceptabilityid");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Effectivetime)
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Moduleid)
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Referencedcomponentid)
                    .HasMaxLength(18)
                    .HasColumnName("referencedcomponentid");

                entity.Property(e => e.Refsetid)
                    .HasMaxLength(18)
                    .HasColumnName("refsetid");
            });

            modelBuilder.Entity<SnomedctLookupSemantictag>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_lookup_semantictag", "terminology");

                entity.HasIndex(e => new { e.Domain, e.Tag }, "snomedct_lkp_semantictag_domain_idx");

                entity.Property(e => e.Domain)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("domain");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Tag)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("tag");
            });

            modelBuilder.Entity<SnomedctModifiedReleaseMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_modified_release_mat", "terminology");

                entity.Property(e => e.DrugId)
                    .HasMaxLength(18)
                    .HasColumnName("drug_id");

                entity.Property(e => e.DrugTerm).HasColumnName("drug_term");

                entity.Property(e => e.DrugTermTokens).HasColumnName("drug_term_tokens");

                entity.Property(e => e.MrCd).HasColumnName("mr_cd");

                entity.Property(e => e.MrId)
                    .HasMaxLength(18)
                    .HasColumnName("mr_id");
            });

            modelBuilder.Entity<SnomedctRelationActiveIsaLookupMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_relation_active_isa_lookup_mat", "terminology");

                entity.Property(e => e.Destinationid)
                    .HasMaxLength(18)
                    .HasColumnName("destinationid");

                entity.Property(e => e.Sourceid)
                    .HasMaxLength(18)
                    .HasColumnName("sourceid");
            });

            modelBuilder.Entity<SnomedctRelationshipF>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_relationship_f", "terminology");

                entity.HasIndex(e => new { e.Sourceid, e.Destinationid }, "snomedct_relationship_f_idx");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Characteristictypeid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("characteristictypeid");

                entity.Property(e => e.Destinationid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("destinationid");

                entity.Property(e => e.Effectivetime)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("id");

                entity.Property(e => e.Modifierid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("modifierid");

                entity.Property(e => e.Moduleid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Relationshipgroup)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("relationshipgroup");

                entity.Property(e => e.Sourceid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("sourceid");

                entity.Property(e => e.Typeid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("typeid");
            });

            modelBuilder.Entity<SnomedctRelationshipLatestMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_relationship_latest_mat", "terminology");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Characteristictypeid)
                    .HasMaxLength(18)
                    .HasColumnName("characteristictypeid");

                entity.Property(e => e.Destinationid)
                    .HasMaxLength(18)
                    .HasColumnName("destinationid");

                entity.Property(e => e.Effectivetime)
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .HasMaxLength(18)
                    .HasColumnName("id");

                entity.Property(e => e.Modifierid)
                    .HasMaxLength(18)
                    .HasColumnName("modifierid");

                entity.Property(e => e.Moduleid)
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Relationshipgroup)
                    .HasMaxLength(18)
                    .HasColumnName("relationshipgroup");

                entity.Property(e => e.Sourceid)
                    .HasMaxLength(18)
                    .HasColumnName("sourceid");

                entity.Property(e => e.Typeid)
                    .HasMaxLength(18)
                    .HasColumnName("typeid");
            });

            modelBuilder.Entity<SnomedctRelationshipwithnamesLatestMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_relationshipwithnames_latest_mat", "terminology");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Characteristictypeid)
                    .HasMaxLength(18)
                    .HasColumnName("characteristictypeid");

                entity.Property(e => e.Characteristictypeidname).HasColumnName("characteristictypeidname");

                entity.Property(e => e.Destinationid)
                    .HasMaxLength(18)
                    .HasColumnName("destinationid");

                entity.Property(e => e.Destinationidname).HasColumnName("destinationidname");

                entity.Property(e => e.DestinationidnameTokens).HasColumnName("destinationidname_tokens");

                entity.Property(e => e.Effectivetime)
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .HasMaxLength(18)
                    .HasColumnName("id");

                entity.Property(e => e.Modifierid)
                    .HasMaxLength(18)
                    .HasColumnName("modifierid");

                entity.Property(e => e.Modifieridname).HasColumnName("modifieridname");

                entity.Property(e => e.Moduleid)
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Moduleidname).HasColumnName("moduleidname");

                entity.Property(e => e.Relationshipgroup)
                    .HasMaxLength(18)
                    .HasColumnName("relationshipgroup");

                entity.Property(e => e.Sourceid)
                    .HasMaxLength(18)
                    .HasColumnName("sourceid");

                entity.Property(e => e.Sourceidname).HasColumnName("sourceidname");

                entity.Property(e => e.SourceidnameTokens).HasColumnName("sourceidname_tokens");

                entity.Property(e => e.Typeid)
                    .HasMaxLength(18)
                    .HasColumnName("typeid");

                entity.Property(e => e.Typeidname).HasColumnName("typeidname");
            });

            modelBuilder.Entity<SnomedctSimplemaprefsetF>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_simplemaprefset_f", "terminology");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Effectivetime)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Maptarget)
                    .IsRequired()
                    .HasColumnName("maptarget");

                entity.Property(e => e.Moduleid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Referencedcomponentid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("referencedcomponentid");

                entity.Property(e => e.Refsetid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("refsetid");
            });

            modelBuilder.Entity<SnomedctSimplerefsetF>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_simplerefset_f", "terminology");

                entity.HasIndex(e => e.Referencedcomponentid, "snomedct_simplerefset_referencedcomponentid_idx");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Effectivetime)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.Moduleid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Referencedcomponentid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("referencedcomponentid");

                entity.Property(e => e.Refsetid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("refsetid");
            });

            modelBuilder.Entity<SnomedctSimplerefsetLatestMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_simplerefset_latest_mat", "terminology");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Effectivetime)
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.Moduleid)
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Referencedcomponentid)
                    .HasMaxLength(18)
                    .HasColumnName("referencedcomponentid");

                entity.Property(e => e.Refsetid)
                    .HasMaxLength(18)
                    .HasColumnName("refsetid");
            });

            modelBuilder.Entity<SnomedctStatedRelationshipF>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_stated_relationship_f", "terminology");

                entity.HasIndex(e => new { e.Sourceid, e.Destinationid }, "snomedct_stated_relationship_f_idx");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Characteristictypeid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("characteristictypeid");

                entity.Property(e => e.Destinationid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("destinationid");

                entity.Property(e => e.Effectivetime)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("id");

                entity.Property(e => e.Modifierid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("modifierid");

                entity.Property(e => e.Moduleid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Relationshipgroup)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("relationshipgroup");

                entity.Property(e => e.Sourceid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("sourceid");

                entity.Property(e => e.Typeid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("typeid");
            });

            modelBuilder.Entity<SnomedctTextdefinitionF>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("snomedct_textdefinition_f", "terminology");

                entity.HasIndex(e => e.Conceptid, "snomedct_textdefinition_conceptid_idx");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("active");

                entity.Property(e => e.Casesignificanceid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("casesignificanceid");

                entity.Property(e => e.Conceptid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("conceptid");

                entity.Property(e => e.Effectivetime)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("effectivetime")
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("id");

                entity.Property(e => e.Languagecode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("languagecode");

                entity.Property(e => e.Moduleid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("moduleid");

                entity.Property(e => e.Term)
                    .IsRequired()
                    .HasColumnName("term");

                entity.Property(e => e.Typeid)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("typeid");
            });

            modelBuilder.Entity<SnomedctTradefamiliesMat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("snomedct_tradefamilies_mat", "terminology");

                entity.Property(e => e.BrandedDrugId)
                    .HasMaxLength(18)
                    .HasColumnName("branded_drug_id");

                entity.Property(e => e.BrandedDrugTerm).HasColumnName("branded_drug_term");

                entity.Property(e => e.BrandedDrugTermTokens).HasColumnName("branded_drug_term_tokens");

                entity.Property(e => e.TradeFamilyId)
                    .HasMaxLength(18)
                    .HasColumnName("trade_family_id");

                entity.Property(e => e.TradeFamilyTerm).HasColumnName("trade_family_term");

                entity.Property(e => e.TradeFamilyTermTokens).HasColumnName("trade_family_term_tokens");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
