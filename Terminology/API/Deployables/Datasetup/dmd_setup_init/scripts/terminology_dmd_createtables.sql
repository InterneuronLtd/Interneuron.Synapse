--===================================================================LIVE TABLES====================================================

CREATE SCHEMA IF NOT EXISTS terminology;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";


--=================================ABOVE STATEMENTS SHOULD NOT BE DELETED ==========================================================================

SET SCHEMA 'terminology';


--=========================================================================================================================

CREATE OR REPLACE FUNCTION terminology.udf_tg_dmd_amp_drugroute_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.apid
			,NEW.routecd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology.udf_tg_dmd_amp_excipient_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.apid
			,NEW.isid
			,NEW.strnth
			,NEW.strnth_uomcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology.udf_tg_dmd_amp_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.apid
			,NEW.invalid
			,NEW.vpid
			,NEW.nm
			,NEW.abbrevnm
			,NEW."desc"
			,NEW.nmdt
			,NEW.nm_prev
			,NEW.suppcd
			,NEW.lic_authcd
			,NEW.lic_auth_prevcd
			,NEW.lic_authchangecd
			,NEW.lic_authchangedt
			,NEW.combprodcd
			,NEW.flavourcd
			,NEW.ema
			,NEW.parallel_import
			,NEW.avail_restrictcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology.udf_tg_dmd_vmp_controldrug_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vpid
			,NEW.catcd
			,NEW.catdt
			,NEW.cat_prevcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology.udf_tg_dmd_vmp_drugform_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vpid
			,NEW.formcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology.udf_tg_dmd_vmp_drugroute_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vpid
			,NEW.routecd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology.udf_tg_dmd_vmp_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vpid
			,NEW.vpiddt
			,NEW.vpidprev
			,NEW.vtmid
			,NEW.invalid
			,NEW.nm
			,NEW.abbrevnm
			,NEW.basiscd
			,NEW.nmdt
			,NEW.nmprev
			,NEW.basis_prevcd
			,NEW.nmchangecd
			,NEW.comprodcd
			,NEW.pres_statcd
			,NEW.sug_f
			,NEW.glu_f
			,NEW.pres_f
			,NEW.cfc_f
			,NEW.non_availcd
			,NEW.non_availdt
			,NEW.df_indcd
			,NEW.udfs
			,NEW.udfs_uomcd
			,NEW.unit_dose_uomcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology.udf_tg_dmd_vmp_ingredient_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vpid
			,NEW.isid
			,NEW.basis_strntcd
			,NEW.bs_subid
			,NEW.strnt_nmrtr_val
			,NEW.strnt_nmrtr_uomcd
			,NEW.strnt_dnmtr_val
			,NEW.strnt_dnmtr_uomcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology.udf_tg_dmd_vmp_ontdrugform_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vpid
			,NEW.formcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology.udf_tg_dmd_vtm_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vtmid
			,NEW.invalid
			,NEW.nm
			,NEW.abbrevnm
			,NEW.vtmidprev
			,NEW.vtmiddt
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;


--=========================================================================================================================


-- terminology.atc_lookup definition

-- Drop table

DROP TABLE IF EXISTS terminology.atc_lookup;

CREATE TABLE terminology.atc_lookup (
	atc_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd varchar(255) NULL,
	"desc" text NULL,
	short_cd varchar(255) NULL
);


-- terminology.bnf_lookup definition

-- Drop table

DROP TABLE IF EXISTS terminology.bnf_lookup;

CREATE TABLE terminology.bnf_lookup (
	bnf_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd varchar(255) NULL,
	"name" text NULL
);


-- terminology.dmd_amp definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_amp;

CREATE TABLE terminology.dmd_amp (
	amp_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	apid varchar(255) NULL,
	invalid int2 NULL,
	vpid varchar(255) NULL,
	nm varchar(1000) NULL,
	abbrevnm varchar(1000) NULL,
	"desc" varchar(1000) NULL,
	nmdt timestamp NULL,
	nm_prev varchar(1000) NULL,
	suppcd varchar(255) NULL,
	lic_authcd int8 NULL,
	lic_auth_prevcd int8 NULL,
	lic_authchangecd int8 NULL,
	lic_authchangedt timestamp NULL,
	combprodcd int8 NULL,
	flavourcd int8 NULL,
	ema int4 NULL,
	parallel_import int4 NULL,
	avail_restrictcd int8 NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_amp_hash_update before
insert
    or
update
    on
    terminology.dmd_amp for each row execute procedure terminology.udf_tg_dmd_amp_hash_update();


-- terminology.dmd_amp_drugroute definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_amp_drugroute;

CREATE TABLE terminology.dmd_amp_drugroute (
	adr_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	apid varchar(255) NULL,
	routecd varchar(255) NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_amp_drugroute_hash_update before
insert
    or
update
    on
    terminology.dmd_amp_drugroute for each row execute procedure terminology.udf_tg_dmd_amp_drugroute_hash_update();


-- terminology.dmd_amp_excipient definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_amp_excipient;

CREATE TABLE terminology.dmd_amp_excipient (
	aex_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	apid varchar(255) NULL,
	isid varchar(255) NULL,
	strnth numeric NULL,
	strnth_uomcd varchar(255) NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_amp_excipient_hash_update before
insert
    or
update
    on
    terminology.dmd_amp_excipient for each row execute procedure terminology.udf_tg_dmd_amp_excipient_hash_update();


-- terminology.dmd_atc definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_atc;

CREATE TABLE terminology.dmd_atc (
	dat_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	atc_cd varchar(255) NULL,
	atc_short_cd varchar(255) NULL,
	dmd_cd varchar(255) NULL
);


-- terminology.dmd_bnf definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_bnf;

CREATE TABLE terminology.dmd_bnf (
	dbn_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	bnf_cd varchar(255) NULL,
	dmd_cd varchar(255) NULL,
	dmd_level varchar(255) NULL
);


-- terminology.dmd_lookup_availrestrict definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_availrestrict;

CREATE TABLE terminology.dmd_lookup_availrestrict (
	lar_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology.dmd_lookup_basisofname definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_basisofname;

CREATE TABLE terminology.dmd_lookup_basisofname (
	bon_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology.dmd_lookup_basisofstrength definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_basisofstrength;

CREATE TABLE terminology.dmd_lookup_basisofstrength (
	lbs_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology.dmd_lookup_controldrugcat definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_controldrugcat;

CREATE TABLE terminology.dmd_lookup_controldrugcat (
	lcd_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology.dmd_lookup_drugformind definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_drugformind;

CREATE TABLE terminology.dmd_lookup_drugformind (
	lfi_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology.dmd_lookup_form definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_form;

CREATE TABLE terminology.dmd_lookup_form (
	lfr_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd varchar(255) NULL,
	cddt timestamp NULL,
	cdprev varchar(255) NULL,
	"desc" varchar(1000) NULL
);


-- terminology.dmd_lookup_ingredient definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_ingredient;

CREATE TABLE terminology.dmd_lookup_ingredient (
	lin_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	isid varchar(255) NULL,
	isiddt timestamp NULL,
	isidprev varchar(255) NULL,
	invalid int2 NULL,
	nm varchar(1000) NULL
);


-- terminology.dmd_lookup_licauth definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_licauth;

CREATE TABLE terminology.dmd_lookup_licauth (
	lau_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology.dmd_lookup_ontformroute definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_ontformroute;

CREATE TABLE terminology.dmd_lookup_ontformroute (
	ofr_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology.dmd_lookup_prescribingstatus definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_prescribingstatus;

CREATE TABLE terminology.dmd_lookup_prescribingstatus (
	lps_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology.dmd_lookup_route definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_route;

CREATE TABLE terminology.dmd_lookup_route (
	lrt_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd varchar(255) NULL,
	cddt timestamp NULL,
	cdprev varchar(255) NULL,
	"desc" varchar(1000) NULL,
	"source" varchar(50) NULL
);


-- terminology.dmd_lookup_supplier definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_supplier;

CREATE TABLE terminology.dmd_lookup_supplier (
	lsu_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd varchar(255) NULL,
	cddt timestamp NULL,
	cdprev varchar(255) NULL,
	invalid int2 NULL,
	"desc" varchar(1000) NULL
);


-- terminology.dmd_lookup_uom definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_lookup_uom;

CREATE TABLE terminology.dmd_lookup_uom (
	uom_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd varchar(255) NULL,
	cddt timestamp NULL,
	cdprev varchar(255) NULL,
	"desc" varchar(1000) NULL
);


-- terminology.dmd_sync_log definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_sync_log;

CREATE TABLE terminology.dmd_sync_log (
	dmd_id varchar(1000) NULL,
	sync_process_id varchar(255) NULL,
	dmd_entity_name varchar(255) NULL,
	created_dt timestamptz NULL DEFAULT now(),
	row_action varchar(10) NULL,
	serial_num bigserial NOT NULL,
	is_dmd_updated bool NULL,
	dmd_update_dt timestamptz NULL,
	is_formulary_updated bool NULL,
	formulary_update_dt timestamptz NULL,
	dmd_version varchar(100) NULL,
	sl_id varchar(255) NOT NULL DEFAULT public.uuid_generate_v4(),
	CONSTRAINT dmd_sync_log_pk PRIMARY KEY (sl_id)
);


-- terminology.dmd_vmp definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_vmp;

CREATE TABLE terminology.dmd_vmp (
	vmp_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vpid varchar(255) NULL,
	vpiddt timestamp NULL,
	vpidprev varchar(255) NULL,
	vtmid varchar(255) NULL,
	invalid int2 NULL,
	nm varchar(1000) NULL,
	abbrevnm varchar(1000) NULL,
	basiscd int8 NULL,
	nmdt timestamp NULL,
	nmprev varchar(1000) NULL,
	basis_prevcd int8 NULL,
	nmchangecd int8 NULL,
	comprodcd int8 NULL,
	pres_statcd int8 NULL,
	sug_f int4 NULL,
	glu_f int4 NULL,
	pres_f int4 NULL,
	cfc_f int4 NULL,
	non_availcd int4 NULL,
	non_availdt timestamp NULL,
	df_indcd int8 NULL,
	udfs numeric NULL,
	udfs_uomcd varchar(255) NULL,
	unit_dose_uomcd varchar(255) NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vmp_hash_update before
insert
    or
update
    on
    terminology.dmd_vmp for each row execute procedure terminology.udf_tg_dmd_vmp_hash_update();


-- terminology.dmd_vmp_controldrug definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_vmp_controldrug;

CREATE TABLE terminology.dmd_vmp_controldrug (
	vcd_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vpid varchar(255) NULL,
	catcd int8 NULL,
	catdt timestamp NULL,
	cat_prevcd int8 NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vmp_controldrug_hash_update before
insert
    or
update
    on
    terminology.dmd_vmp_controldrug for each row execute procedure terminology.udf_tg_dmd_vmp_controldrug_hash_update();


-- terminology.dmd_vmp_drugform definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_vmp_drugform;

CREATE TABLE terminology.dmd_vmp_drugform (
	vdf_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vpid varchar(255) NULL,
	formcd varchar(255) NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vmp_drugform_hash_update before
insert
    or
update
    on
    terminology.dmd_vmp_drugform for each row execute procedure terminology.udf_tg_dmd_vmp_drugform_hash_update();


-- terminology.dmd_vmp_drugroute definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_vmp_drugroute;

CREATE TABLE terminology.dmd_vmp_drugroute (
	vdr_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vpid varchar(255) NULL,
	routecd varchar(255) NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vmp_drugroute_hash_update before
insert
    or
update
    on
    terminology.dmd_vmp_drugroute for each row execute procedure terminology.udf_tg_dmd_vmp_drugroute_hash_update();


-- terminology.dmd_vmp_ingredient definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_vmp_ingredient;

CREATE TABLE terminology.dmd_vmp_ingredient (
	vin_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vpid varchar(255) NULL,
	isid varchar(255) NULL,
	basis_strntcd int8 NULL,
	bs_subid varchar(255) NULL,
	strnt_nmrtr_val numeric NULL,
	strnt_nmrtr_uomcd varchar(255) NULL,
	strnt_dnmtr_val numeric NULL,
	strnt_dnmtr_uomcd varchar(255) NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vmp_ingredient_hash_update before
insert
    or
update
    on
    terminology.dmd_vmp_ingredient for each row execute procedure terminology.udf_tg_dmd_vmp_ingredient_hash_update();


-- terminology.dmd_vmp_ontdrugform definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_vmp_ontdrugform;

CREATE TABLE terminology.dmd_vmp_ontdrugform (
	odf_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vpid varchar(255) NULL,
	formcd int8 NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vmp_ontdrugform_hash_update before
insert
    or
update
    on
    terminology.dmd_vmp_ontdrugform for each row execute procedure terminology.udf_tg_dmd_vmp_ontdrugform_hash_update();


-- terminology.dmd_vtm definition

-- Drop table

DROP TABLE IF EXISTS terminology.dmd_vtm;

CREATE TABLE terminology.dmd_vtm (
	vtm_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vtmid varchar(255) NULL,
	invalid int2 NULL,
	nm varchar(1000) NULL,
	abbrevnm varchar(1000) NULL,
	vtmidprev varchar(255) NULL,
	vtmiddt timestamp NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vtm_hash_update before
insert
    or
update
    on
    terminology.dmd_vtm for each row execute procedure terminology.udf_tg_dmd_vtm_hash_update();
	
	
	
--========================================================================START STAGING TABLES==================================================


CREATE SCHEMA IF NOT EXISTS terminology_staging;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";


--=================================ABOVE STATEMENTS SHOULD NOT BE DELETED ==========================================================================

SET SCHEMA 'terminology_staging';

--==================================================================================================================================================
CREATE OR REPLACE FUNCTION terminology_staging.udf_tg_dmd_amp_drugroute_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.apid
			,NEW.routecd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology_staging.udf_tg_dmd_amp_excipient_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.apid
			,NEW.isid
			,NEW.strnth
			,NEW.strnth_uomcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology_staging.udf_tg_dmd_amp_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.apid
			,NEW.invalid
			,NEW.vpid
			,NEW.nm
			,NEW.abbrevnm
			,NEW."desc"
			,NEW.nmdt
			,NEW.nm_prev
			,NEW.suppcd
			,NEW.lic_authcd
			,NEW.lic_auth_prevcd
			,NEW.lic_authchangecd
			,NEW.lic_authchangedt
			,NEW.combprodcd
			,NEW.flavourcd
			,NEW.ema
			,NEW.parallel_import
			,NEW.avail_restrictcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology_staging.udf_tg_dmd_vmp_controldrug_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vpid
			,NEW.catcd
			,NEW.catdt
			,NEW.cat_prevcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology_staging.udf_tg_dmd_vmp_drugform_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vpid
			,NEW.formcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology_staging.udf_tg_dmd_vmp_drugroute_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vpid
			,NEW.routecd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology_staging.udf_tg_dmd_vmp_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vpid
			,NEW.vpiddt
			,NEW.vpidprev
			,NEW.vtmid
			,NEW.invalid
			,NEW.nm
			,NEW.abbrevnm
			,NEW.basiscd
			,NEW.nmdt
			,NEW.nmprev
			,NEW.basis_prevcd
			,NEW.nmchangecd
			,NEW.comprodcd
			,NEW.pres_statcd
			,NEW.sug_f
			,NEW.glu_f
			,NEW.pres_f
			,NEW.cfc_f
			,NEW.non_availcd
			,NEW.non_availdt
			,NEW.df_indcd
			,NEW.udfs
			,NEW.udfs_uomcd
			,NEW.unit_dose_uomcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology_staging.udf_tg_dmd_vmp_ingredient_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vpid
			,NEW.isid
			,NEW.basis_strntcd
			,NEW.bs_subid
			,NEW.strnt_nmrtr_val
			,NEW.strnt_nmrtr_uomcd
			,NEW.strnt_dnmtr_val
			,NEW.strnt_dnmtr_uomcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology_staging.udf_tg_dmd_vmp_ontdrugform_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vpid
			,NEW.formcd
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

CREATE OR REPLACE FUNCTION terminology_staging.udf_tg_dmd_vtm_hash_update()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF tg_op = 'INSERT' OR tg_op = 'UPDATE' THEN
        NEW.col_val_hash = 
       (md5(row(
      		NEW.vtmid
			,NEW.invalid
			,NEW.nm
			,NEW.abbrevnm
			,NEW.vtmidprev
			,NEW.vtmiddt
      	)::text)::uuid);
        RETURN NEW;
    END IF;
END;
$function$
;

--==================================================================================================================================================
-- terminology_staging.atc_lookup definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.atc_lookup;

CREATE TABLE terminology_staging.atc_lookup (
	atc_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd varchar(255) NULL,
	"desc" text NULL,
	short_cd varchar(255) NULL
);


-- terminology_staging.bnf_lookup definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.bnf_lookup;

CREATE TABLE terminology_staging.bnf_lookup (
	bnf_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd varchar(255) NULL,
	"name" text NULL
);


-- terminology_staging.dmd_amp definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_amp;

CREATE TABLE terminology_staging.dmd_amp (
	amp_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	apid varchar(255) NULL,
	invalid int2 NULL,
	vpid varchar(255) NULL,
	nm varchar(1000) NULL,
	abbrevnm varchar(1000) NULL,
	"desc" varchar(1000) NULL,
	nmdt timestamp NULL,
	nm_prev varchar(1000) NULL,
	suppcd varchar(255) NULL,
	lic_authcd int8 NULL,
	lic_auth_prevcd int8 NULL,
	lic_authchangecd int8 NULL,
	lic_authchangedt timestamp NULL,
	combprodcd int8 NULL,
	flavourcd int8 NULL,
	ema int4 NULL,
	parallel_import int4 NULL,
	avail_restrictcd int8 NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_amp_hash_update before
insert
    or
update
    on
    terminology_staging.dmd_amp for each row execute procedure terminology_staging.udf_tg_dmd_amp_hash_update();


-- terminology_staging.dmd_amp_drugroute definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_amp_drugroute;

CREATE TABLE terminology_staging.dmd_amp_drugroute (
	adr_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	apid varchar(255) NULL,
	routecd varchar(255) NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_amp_drugroute_hash_update before
insert
    or
update
    on
    terminology_staging.dmd_amp_drugroute for each row execute procedure terminology_staging.udf_tg_dmd_amp_drugroute_hash_update();


-- terminology_staging.dmd_amp_excipient definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_amp_excipient;

CREATE TABLE terminology_staging.dmd_amp_excipient (
	aex_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	apid varchar(255) NULL,
	isid varchar(255) NULL,
	strnth numeric NULL,
	strnth_uomcd varchar(255) NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_amp_excipient_hash_update before
insert
    or
update
    on
    terminology_staging.dmd_amp_excipient for each row execute procedure terminology_staging.udf_tg_dmd_amp_excipient_hash_update();


-- terminology_staging.dmd_atc definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_atc;

CREATE TABLE terminology_staging.dmd_atc (
	dat_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	atc_cd varchar(255) NULL,
	atc_short_cd varchar(255) NULL,
	dmd_cd varchar(255) NULL
);


-- terminology_staging.dmd_bnf definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_bnf;

CREATE TABLE terminology_staging.dmd_bnf (
	dbn_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	bnf_cd varchar(255) NULL,
	dmd_cd varchar(255) NULL,
	dmd_level varchar(255) NULL
);


-- terminology_staging.dmd_lookup_availrestrict definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_availrestrict;

CREATE TABLE terminology_staging.dmd_lookup_availrestrict (
	lar_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology_staging.dmd_lookup_basisofname definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_basisofname;

CREATE TABLE terminology_staging.dmd_lookup_basisofname (
	bon_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology_staging.dmd_lookup_basisofstrength definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_basisofstrength;

CREATE TABLE terminology_staging.dmd_lookup_basisofstrength (
	lbs_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology_staging.dmd_lookup_controldrugcat definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_controldrugcat;

CREATE TABLE terminology_staging.dmd_lookup_controldrugcat (
	lcd_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology_staging.dmd_lookup_drugformind definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_drugformind;

CREATE TABLE terminology_staging.dmd_lookup_drugformind (
	lfi_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology_staging.dmd_lookup_form definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_form;

CREATE TABLE terminology_staging.dmd_lookup_form (
	lfr_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd varchar(255) NULL,
	cddt timestamp NULL,
	cdprev varchar(255) NULL,
	"desc" varchar(1000) NULL
);


-- terminology_staging.dmd_lookup_ingredient definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_ingredient;

CREATE TABLE terminology_staging.dmd_lookup_ingredient (
	lin_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	isid varchar(255) NULL,
	isiddt timestamp NULL,
	isidprev varchar(255) NULL,
	invalid int2 NULL,
	nm varchar(1000) NULL
);


-- terminology_staging.dmd_lookup_licauth definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_licauth;

CREATE TABLE terminology_staging.dmd_lookup_licauth (
	lau_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology_staging.dmd_lookup_ontformroute definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_ontformroute;

CREATE TABLE terminology_staging.dmd_lookup_ontformroute (
	ofr_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology_staging.dmd_lookup_prescribingstatus definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_prescribingstatus;

CREATE TABLE terminology_staging.dmd_lookup_prescribingstatus (
	lps_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd int8 NULL,
	"desc" varchar(1000) NULL
);


-- terminology_staging.dmd_lookup_route definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_route;

CREATE TABLE terminology_staging.dmd_lookup_route (
	lrt_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd varchar(255) NULL,
	cddt timestamp NULL,
	cdprev varchar(255) NULL,
	"desc" varchar(1000) NULL,
	"source" varchar(50) NULL
);


-- terminology_staging.dmd_lookup_supplier definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_supplier;

CREATE TABLE terminology_staging.dmd_lookup_supplier (
	lsu_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd varchar(255) NULL,
	cddt timestamp NULL,
	cdprev varchar(255) NULL,
	invalid int2 NULL,
	"desc" varchar(1000) NULL
);


-- terminology_staging.dmd_lookup_uom definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_lookup_uom;

CREATE TABLE terminology_staging.dmd_lookup_uom (
	uom_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	cd varchar(255) NULL,
	cddt timestamp NULL,
	cdprev varchar(255) NULL,
	"desc" varchar(1000) NULL
);


-- terminology_staging.dmd_sync_log definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_sync_log;

CREATE TABLE terminology_staging.dmd_sync_log (
	dmd_id varchar(1000) NULL,
	sync_process_id varchar(255) NULL,
	dmd_entity_name varchar(255) NULL,
	created_dt timestamptz NULL DEFAULT now(),
	row_action varchar(10) NULL
);


-- terminology_staging.dmd_vmp definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_vmp;

CREATE TABLE terminology_staging.dmd_vmp (
	vmp_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vpid varchar(255) NULL,
	vpiddt timestamp NULL,
	vpidprev varchar(255) NULL,
	vtmid varchar(255) NULL,
	invalid int2 NULL,
	nm varchar(1000) NULL,
	abbrevnm varchar(1000) NULL,
	basiscd int8 NULL,
	nmdt timestamp NULL,
	nmprev varchar(1000) NULL,
	basis_prevcd int8 NULL,
	nmchangecd int8 NULL,
	comprodcd int8 NULL,
	pres_statcd int8 NULL,
	sug_f int4 NULL,
	glu_f int4 NULL,
	pres_f int4 NULL,
	cfc_f int4 NULL,
	non_availcd int4 NULL,
	non_availdt timestamp NULL,
	df_indcd int8 NULL,
	udfs numeric NULL,
	udfs_uomcd varchar(255) NULL,
	unit_dose_uomcd varchar(255) NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vmp_hash_update before
insert
    or
update
    on
    terminology_staging.dmd_vmp for each row execute procedure terminology_staging.udf_tg_dmd_vmp_hash_update();


-- terminology_staging.dmd_vmp_controldrug definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_vmp_controldrug;

CREATE TABLE terminology_staging.dmd_vmp_controldrug (
	vcd_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vpid varchar(255) NULL,
	catcd int8 NULL,
	catdt timestamp NULL,
	cat_prevcd int8 NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vmp_controldrug_hash_update before
insert
    or
update
    on
    terminology_staging.dmd_vmp_controldrug for each row execute procedure terminology_staging.udf_tg_dmd_vmp_controldrug_hash_update();


-- terminology_staging.dmd_vmp_drugform definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_vmp_drugform;

CREATE TABLE terminology_staging.dmd_vmp_drugform (
	vdf_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vpid varchar(255) NULL,
	formcd varchar(255) NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vmp_drugform_hash_update before
insert
    or
update
    on
    terminology_staging.dmd_vmp_drugform for each row execute procedure terminology_staging.udf_tg_dmd_vmp_drugform_hash_update();


-- terminology_staging.dmd_vmp_drugroute definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_vmp_drugroute;

CREATE TABLE terminology_staging.dmd_vmp_drugroute (
	vdr_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vpid varchar(255) NULL,
	routecd varchar(255) NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vmp_drugroute_hash_update before
insert
    or
update
    on
    terminology_staging.dmd_vmp_drugroute for each row execute procedure terminology_staging.udf_tg_dmd_vmp_drugroute_hash_update();


-- terminology_staging.dmd_vmp_ingredient definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_vmp_ingredient;

CREATE TABLE terminology_staging.dmd_vmp_ingredient (
	vin_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vpid varchar(255) NULL,
	isid varchar(255) NULL,
	basis_strntcd int8 NULL,
	bs_subid varchar(255) NULL,
	strnt_nmrtr_val numeric NULL,
	strnt_nmrtr_uomcd varchar(255) NULL,
	strnt_dnmtr_val numeric NULL,
	strnt_dnmtr_uomcd varchar(255) NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vmp_ingredient_hash_update before
insert
    or
update
    on
    terminology_staging.dmd_vmp_ingredient for each row execute procedure terminology_staging.udf_tg_dmd_vmp_ingredient_hash_update();


-- terminology_staging.dmd_vmp_ontdrugform definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_vmp_ontdrugform;

CREATE TABLE terminology_staging.dmd_vmp_ontdrugform (
	odf_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vpid varchar(255) NULL,
	formcd int8 NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vmp_ontdrugform_hash_update before
insert
    or
update
    on
    terminology_staging.dmd_vmp_ontdrugform for each row execute procedure terminology_staging.udf_tg_dmd_vmp_ontdrugform_hash_update();


-- terminology_staging.dmd_vtm definition

-- Drop table

DROP TABLE IF EXISTS terminology_staging.dmd_vtm;

CREATE TABLE terminology_staging.dmd_vtm (
	vtm_id varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdmessageid" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	vtmid varchar(255) NULL,
	invalid int2 NULL,
	nm varchar(1000) NULL,
	abbrevnm varchar(1000) NULL,
	vtmidprev varchar(255) NULL,
	vtmiddt timestamp NULL,
	col_val_hash uuid NULL
);

-- Table Triggers

create trigger dmd_vtm_hash_update before
insert
    or
update
    on
    terminology_staging.dmd_vtm for each row execute procedure terminology_staging.udf_tg_dmd_vtm_hash_update();
	
--================================================================END STAGING TABLES===============================================================