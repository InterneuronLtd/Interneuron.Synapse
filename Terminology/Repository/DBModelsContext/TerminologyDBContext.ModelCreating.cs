//Interneuron Synapse

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


﻿//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Interneuron.Terminology.Repository.DBModelsContext
//{
//    public partial class TerminologyDBContext
//    {
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.HasPostgresExtension("pg_buffercache")
//                .HasPostgresExtension("pg_stat_statements")
//                .HasPostgresExtension("uuid-ossp");

//            modelBuilder.Entity<DmdAmp>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("dmd_amp", "terminology");

//                entity.Property(e => e.Abbrevnm)
//                    .HasColumnName("abbrevnm")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.AmpId)
//                    .HasColumnName("amp_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Apid).HasColumnName("apid");

//                entity.Property(e => e.AvailRestrictcd).HasColumnName("avail_restrictcd");

//                entity.Property(e => e.Combprodcd).HasColumnName("combprodcd");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Desc)
//                    .HasColumnName("desc")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.Ema).HasColumnName("ema");

//                entity.Property(e => e.Flavourcd).HasColumnName("flavourcd");

//                entity.Property(e => e.Invalid).HasColumnName("invalid");

//                entity.Property(e => e.LicAuthPrevcd).HasColumnName("lic_auth_prevcd");

//                entity.Property(e => e.LicAuthcd).HasColumnName("lic_authcd");

//                entity.Property(e => e.LicAuthchangecd).HasColumnName("lic_authchangecd");

//                entity.Property(e => e.LicAuthchangedt).HasColumnName("lic_authchangedt");

//                entity.Property(e => e.Nm)
//                    .HasColumnName("nm")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.NmPrev)
//                    .HasColumnName("nm_prev")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.Nmdt).HasColumnName("nmdt");

//                entity.Property(e => e.ParallelImport).HasColumnName("parallel_import");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.dmd_amp_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Suppcd).HasColumnName("suppcd");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

//                entity.Property(e => e.Vpid).HasColumnName("vpid");
//            });

//            modelBuilder.Entity<DmdLookupControldrugcat>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("dmd_lookup_controldrugcat", "terminology");

//                entity.Property(e => e.Cd).HasColumnName("cd");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Desc)
//                    .HasColumnName("desc")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.LcdId)
//                    .HasColumnName("lcd_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.dmd_lookup_controldrugcat_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
//            });

//            modelBuilder.Entity<DmdLookupForm>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("dmd_lookup_form", "terminology");

//                entity.Property(e => e.Cd).HasColumnName("cd");

//                entity.Property(e => e.Cddt).HasColumnName("cddt");

//                entity.Property(e => e.Cdprev).HasColumnName("cdprev");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Desc)
//                    .HasColumnName("desc")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.LfrId)
//                    .HasColumnName("lfr_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.dmd_lookup_form_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
//            });

//            modelBuilder.Entity<DmdLookupPrescribingstatus>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("dmd_lookup_prescribingstatus", "terminology");

//                entity.Property(e => e.Cd).HasColumnName("cd");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Desc)
//                    .HasColumnName("desc")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.LpsId)
//                    .HasColumnName("lps_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.dmd_lookup_prescribingstatus_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
//            });

//            modelBuilder.Entity<DmdLookupRoute>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("dmd_lookup_route", "terminology");

//                entity.Property(e => e.Cd).HasColumnName("cd");

//                entity.Property(e => e.Cddt).HasColumnName("cddt");

//                entity.Property(e => e.Cdprev).HasColumnName("cdprev");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Desc)
//                    .HasColumnName("desc")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.LrtId)
//                    .HasColumnName("lrt_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.dmd_lookup_route_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
//            });

//            modelBuilder.Entity<DmdLookupSupplier>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("dmd_lookup_supplier", "terminology");

//                entity.Property(e => e.Cd).HasColumnName("cd");

//                entity.Property(e => e.Cddt).HasColumnName("cddt");

//                entity.Property(e => e.Cdprev).HasColumnName("cdprev");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Desc)
//                    .HasColumnName("desc")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.Invalid).HasColumnName("invalid");

//                entity.Property(e => e.LsuId)
//                    .HasColumnName("lsu_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.dmd_lookup_supplier_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
//            });

//            modelBuilder.Entity<DmdVmp>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("dmd_vmp", "terminology");

//                entity.Property(e => e.Abbrevnm)
//                    .HasColumnName("abbrevnm")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.BasisPrevcd).HasColumnName("basis_prevcd");

//                entity.Property(e => e.Basiscd).HasColumnName("basiscd");

//                entity.Property(e => e.CfcF).HasColumnName("cfc_f");

//                entity.Property(e => e.Comprodcd).HasColumnName("comprodcd");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.DfIndcd).HasColumnName("df_indcd");

//                entity.Property(e => e.GluF).HasColumnName("glu_f");

//                entity.Property(e => e.Invalid).HasColumnName("invalid");

//                entity.Property(e => e.Nm)
//                    .HasColumnName("nm")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.Nmchangecd).HasColumnName("nmchangecd");

//                entity.Property(e => e.Nmdt).HasColumnName("nmdt");

//                entity.Property(e => e.Nmprev)
//                    .HasColumnName("nmprev")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.NonAvailcd).HasColumnName("non_availcd");

//                entity.Property(e => e.NonAvaildt).HasColumnName("non_availdt");

//                entity.Property(e => e.PresF).HasColumnName("pres_f");

//                entity.Property(e => e.PresStatcd).HasColumnName("pres_statcd");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.dmd_vmp_sequenceid_seq'::regclass)");

//                entity.Property(e => e.SugF).HasColumnName("sug_f");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

//                entity.Property(e => e.Udfs)
//                    .HasColumnName("udfs")
//                    .HasColumnType("numeric");

//                entity.Property(e => e.UdfsUomcd).HasColumnName("udfs_uomcd");

//                entity.Property(e => e.UnitDoseUomcd).HasColumnName("unit_dose_uomcd");

//                entity.Property(e => e.VmpId)
//                    .HasColumnName("vmp_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Vpid).HasColumnName("vpid");

//                entity.Property(e => e.Vpiddt).HasColumnName("vpiddt");

//                entity.Property(e => e.Vpidprev).HasColumnName("vpidprev");

//                entity.Property(e => e.Vtmid).HasColumnName("vtmid");
//            });

//            modelBuilder.Entity<DmdVmpControldrug>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("dmd_vmp_controldrug", "terminology");

//                entity.Property(e => e.CatPrevcd).HasColumnName("cat_prevcd");

//                entity.Property(e => e.Catcd).HasColumnName("catcd");

//                entity.Property(e => e.Catdt).HasColumnName("catdt");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.dmd_vmp_controldrug_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

//                entity.Property(e => e.VcdId)
//                    .HasColumnName("vcd_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Vpid).HasColumnName("vpid");
//            });

//            modelBuilder.Entity<DmdVmpDrugform>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("dmd_vmp_drugform", "terminology");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Formcd).HasColumnName("formcd");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.dmd_vmp_drugform_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

//                entity.Property(e => e.VdfId)
//                    .HasColumnName("vdf_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Vpid).HasColumnName("vpid");
//            });

//            modelBuilder.Entity<DmdVmpDrugroute>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("dmd_vmp_drugroute", "terminology");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.Routecd).HasColumnName("routecd");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.dmd_vmp_drugroute_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

//                entity.Property(e => e.VdrId)
//                    .HasColumnName("vdr_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Vpid).HasColumnName("vpid");
//            });

//            modelBuilder.Entity<DmdVmpIngredient>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("dmd_vmp_ingredient", "terminology");

//                entity.Property(e => e.BasisStrntcd).HasColumnName("basis_strntcd");

//                entity.Property(e => e.BsSubid).HasColumnName("bs_subid");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Isid).HasColumnName("isid");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.dmd_vmp_ingredient_sequenceid_seq'::regclass)");

//                entity.Property(e => e.StrntDnmtrUomcd).HasColumnName("strnt_dnmtr_uomcd");

//                entity.Property(e => e.StrntDnmtrVal).HasColumnName("strnt_dnmtr_val");

//                entity.Property(e => e.StrntNmrtrUomcd).HasColumnName("strnt_nmrtr_uomcd");

//                entity.Property(e => e.StrntNmrtrVal)
//                    .HasColumnName("strnt_nmrtr_val")
//                    .HasColumnType("numeric");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

//                entity.Property(e => e.VinId)
//                    .HasColumnName("vin_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Vpid).HasColumnName("vpid");
//            });

//            modelBuilder.Entity<DmdVtm>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("dmd_vtm", "terminology");

//                entity.Property(e => e.Abbrevnm)
//                    .HasColumnName("abbrevnm")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Invalid).HasColumnName("invalid");

//                entity.Property(e => e.Nm)
//                    .HasColumnName("nm")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.dmd_vtm_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

//                entity.Property(e => e.VtmId)
//                    .HasColumnName("vtm_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Vtmid1).HasColumnName("vtmid");

//                entity.Property(e => e.Vtmiddt).HasColumnName("vtmiddt");

//                entity.Property(e => e.Vtmidprev).HasColumnName("vtmidprev");
//            });

//            modelBuilder.Entity<MvDmdProductlist>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("mv_dmd_productlist", "terminology");

//                entity.Property(e => e.Code)
//                    .HasColumnName("code")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Form)
//                    .HasColumnName("form")
//                    .HasColumnType("character varying");

//                entity.Property(e => e.Level).HasColumnName("level");

//                entity.Property(e => e.Name)
//                    .HasColumnName("name")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.ParentCode)
//                    .HasColumnName("parent_code")
//                    .HasColumnType("character varying");

//                entity.Property(e => e.ParentLevel).HasColumnName("parent_level");

//                entity.Property(e => e.Route)
//                    .HasColumnName("route")
//                    .HasColumnType("character varying");
//            });

//            modelBuilder.Entity<MvDmdProductlistwithlmc>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("mv_dmd_productlistwithlmc", "terminology");

//                entity.Property(e => e.Code)
//                    .HasColumnName("code")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Form)
//                    .HasColumnName("form")
//                    .HasColumnType("character varying");

//                entity.Property(e => e.Level).HasColumnName("level");

//                entity.Property(e => e.LmcCount).HasColumnName("lmc_count");

//                entity.Property(e => e.Name)
//                    .HasColumnName("name")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.ParentCode)
//                    .HasColumnName("parent_code")
//                    .HasColumnType("character varying");

//                entity.Property(e => e.ParentLevel).HasColumnName("parent_level");

//                entity.Property(e => e.Route)
//                    .HasColumnName("route")
//                    .HasColumnType("character varying");

//                entity.Property(e => e.SupplierName)
//                    .HasColumnName("supplier_name")
//                    .HasColumnType("character varying");
//            });

//            modelBuilder.Entity<SnomedctAssociationrefsetF>(entity =>
//            {
//                entity.HasKey(e => new { e.Id, e.Effectivetime })
//                    .HasName("snomedct_associationrefset_f_pkey");

//                entity.ToTable("snomedct_associationrefset_f", "terminology");

//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.Effectivetime)
//                    .HasColumnName("effectivetime")
//                    .HasMaxLength(8)
//                    .IsFixedLength();

//                entity.Property(e => e.Active).HasColumnName("active");

//                entity.Property(e => e.Moduleid)
//                    .IsRequired()
//                    .HasColumnName("moduleid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Referencedcomponentid)
//                    .IsRequired()
//                    .HasColumnName("referencedcomponentid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Refsetid)
//                    .IsRequired()
//                    .HasColumnName("refsetid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Targetcomponentid)
//                    .IsRequired()
//                    .HasColumnName("targetcomponentid")
//                    .HasMaxLength(18);
//            });

//            modelBuilder.Entity<SnomedctAttributedefinition>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("snomedct_attributedefinition", "terminology");

//                entity.HasIndex(e => e.AttributedefinitionId)
//                    .HasName("snomedct_attributedefinition_attributedefinition_id_idx");

//                entity.HasIndex(e => e.Recordstatus)
//                    .HasName("snomedct_attributedefinition_recordstatus_idx");

//                entity.HasIndex(e => e.Sequenceid)
//                    .HasName("snomedct_attributedefinition_sequenceid_idx");

//                entity.Property(e => e.AttributedefinitionId)
//                    .HasColumnName("attributedefinition_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Attributedefinitionid1).HasColumnName("attributedefinitionid");

//                entity.Property(e => e.Attributedescription).HasColumnName("attributedescription");

//                entity.Property(e => e.Attributename)
//                    .HasColumnName("attributename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Attributesyntax).HasColumnName("attributesyntax");

//                entity.Property(e => e.Attributetypeconceptid).HasColumnName("attributetypeconceptid");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.snomedct_attributedefinition_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
//            });

//            modelBuilder.Entity<SnomedctAttributevaluerefsetF>(entity =>
//            {
//                entity.HasKey(e => new { e.Id, e.Effectivetime })
//                    .HasName("snomedct_attributevaluerefset_f_pkey");

//                entity.ToTable("snomedct_attributevaluerefset_f", "terminology");

//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.Effectivetime)
//                    .HasColumnName("effectivetime")
//                    .HasMaxLength(8)
//                    .IsFixedLength();

//                entity.Property(e => e.Active).HasColumnName("active");

//                entity.Property(e => e.Moduleid)
//                    .IsRequired()
//                    .HasColumnName("moduleid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Referencedcomponentid)
//                    .IsRequired()
//                    .HasColumnName("referencedcomponentid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Refsetid)
//                    .IsRequired()
//                    .HasColumnName("refsetid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Valueid)
//                    .IsRequired()
//                    .HasColumnName("valueid")
//                    .HasMaxLength(18);
//            });

//            modelBuilder.Entity<SnomedctComplexmaprefsetF>(entity =>
//            {
//                entity.HasKey(e => new { e.Id, e.Effectivetime })
//                    .HasName("snomedct_complexmaprefset_f_pkey");

//                entity.ToTable("snomedct_complexmaprefset_f", "terminology");

//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.Effectivetime)
//                    .HasColumnName("effectivetime")
//                    .HasMaxLength(8)
//                    .IsFixedLength();

//                entity.Property(e => e.Active).HasColumnName("active");

//                entity.Property(e => e.Correlationid)
//                    .IsRequired()
//                    .HasColumnName("correlationid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Mapadvice).HasColumnName("mapadvice");

//                entity.Property(e => e.Mapgroup).HasColumnName("mapgroup");

//                entity.Property(e => e.Mappriority).HasColumnName("mappriority");

//                entity.Property(e => e.Maprule).HasColumnName("maprule");

//                entity.Property(e => e.Maptarget).HasColumnName("maptarget");

//                entity.Property(e => e.Moduleid)
//                    .IsRequired()
//                    .HasColumnName("moduleid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Referencedcomponentid)
//                    .IsRequired()
//                    .HasColumnName("referencedcomponentid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Refsetid)
//                    .IsRequired()
//                    .HasColumnName("refsetid")
//                    .HasMaxLength(18);
//            });

//            modelBuilder.Entity<SnomedctConcept>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("snomedct_concept", "terminology");

//                entity.HasIndex(e => e.ConceptId)
//                    .HasName("snomedct_concept_concept_id_idx");

//                entity.HasIndex(e => e.Recordstatus)
//                    .HasName("snomedct_concept_recordstatus_idx");

//                entity.HasIndex(e => e.Sequenceid)
//                    .HasName("snomedct_concept_sequenceid_idx");

//                entity.Property(e => e.ConceptId).HasColumnName("concept_id");

//                entity.Property(e => e.ConceptclassId)
//                    .HasColumnName("conceptclass_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Conceptcode)
//                    .HasColumnName("conceptcode")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Conceptname)
//                    .HasColumnName("conceptname")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.DomainId)
//                    .HasColumnName("domain_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Invalidreason)
//                    .HasColumnName("invalidreason")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.snomedct_concept_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Standardconcept)
//                    .HasColumnName("standardconcept")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

//                entity.Property(e => e.Validenddate)
//                    .HasColumnName("validenddate")
//                    .HasColumnType("date");

//                entity.Property(e => e.Validstartdate)
//                    .HasColumnName("validstartdate")
//                    .HasColumnType("date");

//                entity.Property(e => e.VocabularyId)
//                    .HasColumnName("vocabulary_id")
//                    .HasMaxLength(255);
//            });

//            modelBuilder.Entity<SnomedctConceptF>(entity =>
//            {
//                entity.HasKey(e => new { e.Id, e.Effectivetime })
//                    .HasName("snomedct_concept_f_pkey");

//                entity.ToTable("snomedct_concept_f", "terminology");

//                entity.Property(e => e.Id)
//                    .HasColumnName("id")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Effectivetime)
//                    .HasColumnName("effectivetime")
//                    .HasMaxLength(8)
//                    .IsFixedLength();

//                entity.Property(e => e.Active).HasColumnName("active");

//                entity.Property(e => e.Definitionstatusid)
//                    .IsRequired()
//                    .HasColumnName("definitionstatusid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Moduleid)
//                    .IsRequired()
//                    .HasColumnName("moduleid")
//                    .HasMaxLength(18);
//            });

//            modelBuilder.Entity<SnomedctConceptancestor>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("snomedct_conceptancestor", "terminology");

//                entity.HasIndex(e => e.ConceptancestorId)
//                    .HasName("terminology_conceptancestor_conceptancestor_id_idx");

//                entity.HasIndex(e => e.Recordstatus)
//                    .HasName("snomedct_conceptancestor_recordstatus_idx");

//                entity.HasIndex(e => e.Sequenceid)
//                    .HasName("snomedct_conceptancestor_sequenceid_idx");

//                entity.Property(e => e.Ancestorconceptid).HasColumnName("ancestorconceptid");

//                entity.Property(e => e.ConceptancestorId)
//                    .HasColumnName("conceptancestor_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Descendantconceptid).HasColumnName("descendantconceptid");

//                entity.Property(e => e.Maxlevelsofseparation).HasColumnName("maxlevelsofseparation");

//                entity.Property(e => e.Minlevelsofseparation).HasColumnName("minlevelsofseparation");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.snomedct_conceptancestor_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
//            });

//            modelBuilder.Entity<SnomedctConceptclass>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("snomedct_conceptclass", "terminology");

//                entity.HasIndex(e => e.ConceptclassId)
//                    .HasName("snomedct_conceptclass_conceptclass_id_idx");

//                entity.HasIndex(e => e.Recordstatus)
//                    .HasName("snomedct_conceptclass_recordstatus_idx");

//                entity.HasIndex(e => e.Sequenceid)
//                    .HasName("snomedct_conceptclass_sequenceid_idx");

//                entity.Property(e => e.ConceptclassId)
//                    .HasColumnName("conceptclass_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Conceptclassconceptid).HasColumnName("conceptclassconceptid");

//                entity.Property(e => e.Conceptclassname)
//                    .HasColumnName("conceptclassname")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.snomedct_conceptclass_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
//            });

//            modelBuilder.Entity<SnomedctConceptrelationship>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("snomedct_conceptrelationship", "terminology");

//                entity.HasIndex(e => e.ConceptrelationshipId)
//                    .HasName("snomedct_conceptrelationship_conceptrelationship_id_idx");

//                entity.HasIndex(e => e.Recordstatus)
//                    .HasName("snomedct_conceptrelationship_recordstatus_idx");

//                entity.HasIndex(e => e.Sequenceid)
//                    .HasName("snomedct_conceptrelationship_sequenceid_idx");

//                entity.Property(e => e.Conceptid1).HasColumnName("conceptid1");

//                entity.Property(e => e.Conceptid2).HasColumnName("conceptid2");

//                entity.Property(e => e.ConceptrelationshipId)
//                    .HasColumnName("conceptrelationship_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Invalidreason)
//                    .HasColumnName("invalidreason")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RelationshipId)
//                    .HasColumnName("relationship_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.snomedct_conceptrelationship_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

//                entity.Property(e => e.Validenddate)
//                    .HasColumnName("validenddate")
//                    .HasColumnType("date");

//                entity.Property(e => e.Validstartdate)
//                    .HasColumnName("validstartdate")
//                    .HasColumnType("date");
//            });

//            modelBuilder.Entity<SnomedctConceptsynonym>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("snomedct_conceptsynonym", "terminology");

//                entity.HasIndex(e => e.ConceptsynonymId)
//                    .HasName("snomedct_conceptsynonym_conceptsynonym_id_idx");

//                entity.HasIndex(e => e.Recordstatus)
//                    .HasName("snomedct_conceptsynonym_recordstatus_idx");

//                entity.HasIndex(e => e.Sequenceid)
//                    .HasName("snomedct_conceptsynonym_sequenceid_idx");

//                entity.Property(e => e.Conceptid).HasColumnName("conceptid");

//                entity.Property(e => e.ConceptsynonymId)
//                    .HasColumnName("conceptsynonym_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Conceptsynonymname)
//                    .HasColumnName("conceptsynonymname")
//                    .HasMaxLength(1000);

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Languageconceptid).HasColumnName("languageconceptid");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.snomedct_conceptsynonym_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
//            });

//            modelBuilder.Entity<SnomedctDescriptionF>(entity =>
//            {
//                entity.HasKey(e => new { e.Id, e.Effectivetime })
//                    .HasName("snomedct_description_f_pkey");

//                entity.ToTable("snomedct_description_f", "terminology");

//                entity.Property(e => e.Id)
//                    .HasColumnName("id")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Effectivetime)
//                    .HasColumnName("effectivetime")
//                    .HasMaxLength(8)
//                    .IsFixedLength();

//                entity.Property(e => e.Active).HasColumnName("active");

//                entity.Property(e => e.Casesignificanceid)
//                    .IsRequired()
//                    .HasColumnName("casesignificanceid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Conceptid)
//                    .IsRequired()
//                    .HasColumnName("conceptid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Languagecode)
//                    .IsRequired()
//                    .HasColumnName("languagecode")
//                    .HasMaxLength(2);

//                entity.Property(e => e.Moduleid)
//                    .IsRequired()
//                    .HasColumnName("moduleid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Term)
//                    .IsRequired()
//                    .HasColumnName("term");

//                entity.Property(e => e.Typeid)
//                    .IsRequired()
//                    .HasColumnName("typeid")
//                    .HasMaxLength(18);
//            });

//            modelBuilder.Entity<SnomedctDomain>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("snomedct_domain", "terminology");

//                entity.HasIndex(e => e.DomainId)
//                    .HasName("snomedct_domain_domain_id_idx");

//                entity.HasIndex(e => e.Recordstatus)
//                    .HasName("snomedct_domain_recordstatus_idx");

//                entity.HasIndex(e => e.Sequenceid)
//                    .HasName("snomedct_domain_sequenceid_idx");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.DomainId)
//                    .HasColumnName("domain_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Domainconceptid).HasColumnName("domainconceptid");

//                entity.Property(e => e.Domainname)
//                    .HasColumnName("domainname")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.snomedct_domain_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
//            });

//            modelBuilder.Entity<SnomedctDrugstrength>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("snomedct_drugstrength", "terminology");

//                entity.HasIndex(e => e.DrugstrengthId)
//                    .HasName("snomedct_drugstrength_drugstrength_id_idx");

//                entity.HasIndex(e => e.Recordstatus)
//                    .HasName("snomedct_drugstrength_recordstatus_idx");

//                entity.HasIndex(e => e.Sequenceid)
//                    .HasName("snomedct_drugstrength_sequenceid_idx");

//                entity.Property(e => e.Amountunitconceptid).HasColumnName("amountunitconceptid");

//                entity.Property(e => e.Amountvalue)
//                    .HasColumnName("amountvalue")
//                    .HasColumnType("numeric");

//                entity.Property(e => e.Boxsize).HasColumnName("boxsize");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Denominatorunitconceptid).HasColumnName("denominatorunitconceptid");

//                entity.Property(e => e.Denominatorvalue)
//                    .HasColumnName("denominatorvalue")
//                    .HasColumnType("numeric");

//                entity.Property(e => e.Drugconceptid).HasColumnName("drugconceptid");

//                entity.Property(e => e.DrugstrengthId)
//                    .HasColumnName("drugstrength_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Ingredientconceptid).HasColumnName("ingredientconceptid");

//                entity.Property(e => e.Invalidreason)
//                    .HasColumnName("invalidreason")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Numeratorunitconceptid).HasColumnName("numeratorunitconceptid");

//                entity.Property(e => e.Numeratorvalue)
//                    .HasColumnName("numeratorvalue")
//                    .HasColumnType("numeric");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.snomedct_drugstrength_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

//                entity.Property(e => e.Validenddate)
//                    .HasColumnName("validenddate")
//                    .HasColumnType("date");

//                entity.Property(e => e.Validstartdate)
//                    .HasColumnName("validstartdate")
//                    .HasColumnType("date");
//            });

//            modelBuilder.Entity<SnomedctExtendedmaprefsetF>(entity =>
//            {
//                entity.HasKey(e => new { e.Id, e.Effectivetime })
//                    .HasName("snomedct_extendedmaprefset_f_pkey");

//                entity.ToTable("snomedct_extendedmaprefset_f", "terminology");

//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.Effectivetime)
//                    .HasColumnName("effectivetime")
//                    .HasMaxLength(8)
//                    .IsFixedLength();

//                entity.Property(e => e.Active).HasColumnName("active");

//                entity.Property(e => e.Correlationid)
//                    .HasColumnName("correlationid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Mapadvice).HasColumnName("mapadvice");

//                entity.Property(e => e.Mapcategoryid)
//                    .HasColumnName("mapcategoryid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Mapgroup).HasColumnName("mapgroup");

//                entity.Property(e => e.Mappriority).HasColumnName("mappriority");

//                entity.Property(e => e.Maprule).HasColumnName("maprule");

//                entity.Property(e => e.Maptarget).HasColumnName("maptarget");

//                entity.Property(e => e.Moduleid)
//                    .IsRequired()
//                    .HasColumnName("moduleid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Referencedcomponentid)
//                    .IsRequired()
//                    .HasColumnName("referencedcomponentid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Refsetid)
//                    .IsRequired()
//                    .HasColumnName("refsetid")
//                    .HasMaxLength(18);
//            });

//            modelBuilder.Entity<SnomedctLangrefsetF>(entity =>
//            {
//                entity.HasKey(e => new { e.Id, e.Effectivetime })
//                    .HasName("snomedct_langrefset_f_pkey");

//                entity.ToTable("snomedct_langrefset_f", "terminology");

//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.Effectivetime)
//                    .HasColumnName("effectivetime")
//                    .HasMaxLength(8)
//                    .IsFixedLength();

//                entity.Property(e => e.Acceptabilityid)
//                    .IsRequired()
//                    .HasColumnName("acceptabilityid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Active).HasColumnName("active");

//                entity.Property(e => e.Moduleid)
//                    .IsRequired()
//                    .HasColumnName("moduleid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Referencedcomponentid)
//                    .IsRequired()
//                    .HasColumnName("referencedcomponentid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Refsetid)
//                    .IsRequired()
//                    .HasColumnName("refsetid")
//                    .HasMaxLength(18);
//            });

//            modelBuilder.Entity<SnomedctLookupSemantictag>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("snomedct_lookup_semantictag", "terminology");

//                entity.HasIndex(e => new { e.Domain, e.Tag })
//                    .HasName("snomedct_lkp_semantictag_domain_idx");

//                entity.Property(e => e.Domain)
//                    .IsRequired()
//                    .HasColumnName("domain")
//                    .HasColumnType("character varying");

//                entity.Property(e => e.Id)
//                    .IsRequired()
//                    .HasColumnName("id")
//                    .HasColumnType("character varying")
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Tag)
//                    .IsRequired()
//                    .HasColumnName("tag")
//                    .HasColumnType("character varying");
//            });

//            modelBuilder.Entity<SnomedctRelationship>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("snomedct_relationship", "terminology");

//                entity.HasIndex(e => e.Recordstatus)
//                    .HasName("snomedct_relationship_recordstatus_idx");

//                entity.HasIndex(e => e.RelationshipId)
//                    .HasName("snomedct_relationship_relationship_id_idx");

//                entity.HasIndex(e => e.Sequenceid)
//                    .HasName("snomedct_relationship_sequenceid_idx");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Definesancestry)
//                    .HasColumnName("definesancestry")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Ishierarchical)
//                    .HasColumnName("ishierarchical")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RelationshipId)
//                    .HasColumnName("relationship_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Relationshipconceptid).HasColumnName("relationshipconceptid");

//                entity.Property(e => e.Relationshipname)
//                    .HasColumnName("relationshipname")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Reverserelationshipid)
//                    .HasColumnName("reverserelationshipid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.snomedct_relationship_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");
//            });

//            modelBuilder.Entity<SnomedctRelationshipF>(entity =>
//            {
//                entity.HasKey(e => new { e.Id, e.Effectivetime })
//                    .HasName("snomedct_relationship_f_pkey");

//                entity.ToTable("snomedct_relationship_f", "terminology");

//                entity.Property(e => e.Id)
//                    .HasColumnName("id")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Effectivetime)
//                    .HasColumnName("effectivetime")
//                    .HasMaxLength(8)
//                    .IsFixedLength();

//                entity.Property(e => e.Active).HasColumnName("active");

//                entity.Property(e => e.Characteristictypeid)
//                    .IsRequired()
//                    .HasColumnName("characteristictypeid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Destinationid)
//                    .IsRequired()
//                    .HasColumnName("destinationid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Modifierid)
//                    .IsRequired()
//                    .HasColumnName("modifierid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Moduleid)
//                    .IsRequired()
//                    .HasColumnName("moduleid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Relationshipgroup)
//                    .IsRequired()
//                    .HasColumnName("relationshipgroup")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Sourceid)
//                    .IsRequired()
//                    .HasColumnName("sourceid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Typeid)
//                    .IsRequired()
//                    .HasColumnName("typeid")
//                    .HasMaxLength(18);
//            });

//            modelBuilder.Entity<SnomedctSimplemaprefsetF>(entity =>
//            {
//                entity.HasKey(e => new { e.Id, e.Effectivetime })
//                    .HasName("snomedct_simplemaprefset_f_pkey");

//                entity.ToTable("snomedct_simplemaprefset_f", "terminology");

//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.Effectivetime)
//                    .HasColumnName("effectivetime")
//                    .HasMaxLength(8)
//                    .IsFixedLength();

//                entity.Property(e => e.Active).HasColumnName("active");

//                entity.Property(e => e.Maptarget)
//                    .IsRequired()
//                    .HasColumnName("maptarget");

//                entity.Property(e => e.Moduleid)
//                    .IsRequired()
//                    .HasColumnName("moduleid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Referencedcomponentid)
//                    .IsRequired()
//                    .HasColumnName("referencedcomponentid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Refsetid)
//                    .IsRequired()
//                    .HasColumnName("refsetid")
//                    .HasMaxLength(18);
//            });

//            modelBuilder.Entity<SnomedctSimplerefsetF>(entity =>
//            {
//                entity.HasKey(e => new { e.Id, e.Effectivetime })
//                    .HasName("snomedct_simplerefset_f_pkey");

//                entity.ToTable("snomedct_simplerefset_f", "terminology");

//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.Effectivetime)
//                    .HasColumnName("effectivetime")
//                    .HasMaxLength(8)
//                    .IsFixedLength();

//                entity.Property(e => e.Active).HasColumnName("active");

//                entity.Property(e => e.Moduleid)
//                    .IsRequired()
//                    .HasColumnName("moduleid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Referencedcomponentid)
//                    .IsRequired()
//                    .HasColumnName("referencedcomponentid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Refsetid)
//                    .IsRequired()
//                    .HasColumnName("refsetid")
//                    .HasMaxLength(18);
//            });

//            modelBuilder.Entity<SnomedctSourcetoconceptmap>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("snomedct_sourcetoconceptmap", "terminology");

//                entity.HasIndex(e => e.Recordstatus)
//                    .HasName("snomedct_sourcetoconceptmap_recordstatus_idx");

//                entity.HasIndex(e => e.Sequenceid)
//                    .HasName("snomedct_sourcetoconceptmap_sequenceid_idx");

//                entity.HasIndex(e => e.SourcetoconceptmapId)
//                    .HasName("snomedct_sourcetoconceptmap_sourcetoconceptmap_id_idx");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Invalidreason)
//                    .HasColumnName("invalidreason")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.snomedct_sourcetoconceptmap_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Sourcecode)
//                    .HasColumnName("sourcecode")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Sourcecodedescription)
//                    .HasColumnName("sourcecodedescription")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Sourceconceptid).HasColumnName("sourceconceptid");

//                entity.Property(e => e.SourcetoconceptmapId)
//                    .HasColumnName("sourcetoconceptmap_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Sourcevocabularyid)
//                    .HasColumnName("sourcevocabularyid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Targetconceptid).HasColumnName("targetconceptid");

//                entity.Property(e => e.Targetvocabularyid)
//                    .HasColumnName("targetvocabularyid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

//                entity.Property(e => e.Validenddate)
//                    .HasColumnName("validenddate")
//                    .HasColumnType("date");

//                entity.Property(e => e.Validstartdate)
//                    .HasColumnName("validstartdate")
//                    .HasColumnType("date");
//            });

//            modelBuilder.Entity<SnomedctStatedRelationshipF>(entity =>
//            {
//                entity.HasKey(e => new { e.Id, e.Effectivetime })
//                    .HasName("snomedct_stated_relationship_f_pkey");

//                entity.ToTable("snomedct_stated_relationship_f", "terminology");

//                entity.Property(e => e.Id)
//                    .HasColumnName("id")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Effectivetime)
//                    .HasColumnName("effectivetime")
//                    .HasMaxLength(8)
//                    .IsFixedLength();

//                entity.Property(e => e.Active).HasColumnName("active");

//                entity.Property(e => e.Characteristictypeid)
//                    .IsRequired()
//                    .HasColumnName("characteristictypeid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Destinationid)
//                    .IsRequired()
//                    .HasColumnName("destinationid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Modifierid)
//                    .IsRequired()
//                    .HasColumnName("modifierid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Moduleid)
//                    .IsRequired()
//                    .HasColumnName("moduleid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Relationshipgroup)
//                    .IsRequired()
//                    .HasColumnName("relationshipgroup")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Sourceid)
//                    .IsRequired()
//                    .HasColumnName("sourceid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Typeid)
//                    .IsRequired()
//                    .HasColumnName("typeid")
//                    .HasMaxLength(18);
//            });

//            modelBuilder.Entity<SnomedctTextdefinitionF>(entity =>
//            {
//                entity.HasKey(e => new { e.Id, e.Effectivetime })
//                    .HasName("snomedct_textdefinition_f_pkey");

//                entity.ToTable("snomedct_textdefinition_f", "terminology");

//                entity.Property(e => e.Id)
//                    .HasColumnName("id")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Effectivetime)
//                    .HasColumnName("effectivetime")
//                    .HasMaxLength(8)
//                    .IsFixedLength();

//                entity.Property(e => e.Active).HasColumnName("active");

//                entity.Property(e => e.Casesignificanceid)
//                    .IsRequired()
//                    .HasColumnName("casesignificanceid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Conceptid)
//                    .IsRequired()
//                    .HasColumnName("conceptid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Languagecode)
//                    .IsRequired()
//                    .HasColumnName("languagecode")
//                    .HasMaxLength(2);

//                entity.Property(e => e.Moduleid)
//                    .IsRequired()
//                    .HasColumnName("moduleid")
//                    .HasMaxLength(18);

//                entity.Property(e => e.Term)
//                    .IsRequired()
//                    .HasColumnName("term");

//                entity.Property(e => e.Typeid)
//                    .IsRequired()
//                    .HasColumnName("typeid")
//                    .HasMaxLength(18);
//            });

//            modelBuilder.Entity<SnomedctVocabulary>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("snomedct_vocabulary", "terminology");

//                entity.HasIndex(e => e.Recordstatus)
//                    .HasName("snomedct_vocabulary_recordstatus_idx");

//                entity.HasIndex(e => e.Sequenceid)
//                    .HasName("snomedct_vocabulary_sequenceid_idx");

//                entity.HasIndex(e => e.VocabularyId)
//                    .HasName("snomedct_vocabulary_vocabulary_id_idx");

//                entity.Property(e => e.Contextkey)
//                    .HasColumnName("_contextkey")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdby)
//                    .HasColumnName("_createdby")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createddate)
//                    .HasColumnName("_createddate")
//                    .HasDefaultValueSql("now()");

//                entity.Property(e => e.Createdmessageid)
//                    .HasColumnName("_createdmessageid")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdsource)
//                    .HasColumnName("_createdsource")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Createdtimestamp)
//                    .HasColumnName("_createdtimestamp")
//                    .HasColumnType("timestamp with time zone")
//                    .HasDefaultValueSql("timezone('UTC'::text, now())");

//                entity.Property(e => e.Recordstatus)
//                    .HasColumnName("_recordstatus")
//                    .HasDefaultValueSql("1");

//                entity.Property(e => e.RowId)
//                    .HasColumnName("_row_id")
//                    .HasMaxLength(255)
//                    .HasDefaultValueSql("uuid_generate_v4()");

//                entity.Property(e => e.Sequenceid)
//                    .HasColumnName("_sequenceid")
//                    .HasDefaultValueSql("nextval('terminology.snomedct_vocabulary_sequenceid_seq'::regclass)");

//                entity.Property(e => e.Tenant)
//                    .HasColumnName("_tenant")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezonename)
//                    .HasColumnName("_timezonename")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Timezoneoffset).HasColumnName("_timezoneoffset");

//                entity.Property(e => e.VocabularyId)
//                    .HasColumnName("vocabulary_id")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Vocabularyconceptid).HasColumnName("vocabularyconceptid");

//                entity.Property(e => e.Vocabularyname)
//                    .HasColumnName("vocabularyname")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Vocabularyreference)
//                    .HasColumnName("vocabularyreference")
//                    .HasMaxLength(255);

//                entity.Property(e => e.Vocabularyversion)
//                    .HasColumnName("vocabularyversion")
//                    .HasMaxLength(255);
//            });

//            modelBuilder.HasSequence("dmd_amp_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("dmd_lookup_controldrugcat_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("dmd_lookup_form_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("dmd_lookup_prescribingstatus_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("dmd_lookup_route_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("dmd_lookup_supplier_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("dmd_vmp_controldrug_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("dmd_vmp_drugform_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("dmd_vmp_drugroute_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("dmd_vmp_ingredient_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("dmd_vmp_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("dmd_vtm_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("snomedct_attributedefinition_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("snomedct_concept_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("snomedct_conceptancestor_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("snomedct_conceptclass_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("snomedct_conceptrelationship_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("snomedct_conceptsynonym_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("snomedct_domain_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("snomedct_drugstrength_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("snomedct_relationship_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("snomedct_sourcetoconceptmap_sequenceid_seq", "terminology");

//            modelBuilder.HasSequence("snomedct_vocabulary_sequenceid_seq", "terminology");

//            OnModelCreatingPartial(modelBuilder);
//        }

//    }
//}
