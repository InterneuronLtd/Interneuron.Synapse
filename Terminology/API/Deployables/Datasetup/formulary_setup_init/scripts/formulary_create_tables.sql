
CREATE SCHEMA IF NOT EXISTS local_formulary;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";



SET SCHEMA 'local_formulary';

--=================================ABOVE STATEMENTS SHOULD NOT BE DELETED ==========================================================================

--====================================================================START TRIGGERS========================================================================
DROP FUNCTION IF EXISTS local_formulary.udf_update_formulary_record_changes() CASCADE;

CREATE OR REPLACE FUNCTION local_formulary.udf_update_formulary_record_changes()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
	IF NEW."name" is not NULL THEN
		update local_formulary.formulary_header
		set name_tokens = to_tsvector('english'::regconfig, "name")
		where "code" = new."code";
	END IF;

	RETURN NEW;
END;
$function$
;


--====================================================================END TRIGGERS========================================================================


--==========================================================================TABLES=============================================================================


-- local_formulary.formulary_header definition

-- Drop table

DROP TABLE IF EXISTS local_formulary.formulary_header CASCADE;

CREATE TABLE local_formulary.formulary_header (
	"_row_id" varchar(255) NULL DEFAULT public.uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_id varchar(255) NULL,
	version_id int4 NULL,
	formulary_version_id varchar(255) NOT NULL,
	code varchar(255) NULL,
	"name" text NULL,
	name_tokens tsvector NULL,
	product_type varchar(100) NULL,
	parent_code varchar(255) NULL,
	parent_name text NULL,
	parent_name_tokens tsvector NULL,
	parent_product_type varchar(100) NULL,
	rec_status_code varchar(8) NULL,
	rec_statuschange_ts timestamptz NULL,
	rec_statuschange_date timestamp NULL,
	rec_statuschange_tzname varchar(255) NULL,
	rec_statuschange_tzoffset int4 NULL,
	is_duplicate bool NULL,
	is_latest bool NULL,
	rec_source varchar(50) NULL,
	vtm_id varchar(100) NULL,
	vmp_id varchar(100) NULL,
	rec_statuschange_msg text NULL,
	duplicate_of_formulary_id varchar(255) NULL,
	"_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_updateddate" timestamp NULL DEFAULT now(),
	"_updatedby" varchar(255) NULL,
	code_system varchar(500) NULL DEFAULT 'DMD'::character varying,
	CONSTRAINT formulary_header_pk PRIMARY KEY (formulary_version_id)
);
CREATE INDEX formulary_header_code_idx ON local_formulary.formulary_header USING btree (code);
CREATE INDEX formulary_header_is_latest_idx ON local_formulary.formulary_header USING btree (is_latest);
CREATE INDEX formulary_header_name_idx ON local_formulary.formulary_header USING btree (name);
CREATE INDEX formulary_header_name_tokens_idx ON local_formulary.formulary_header USING gin (name_tokens);
CREATE INDEX formulary_header_parent_code_idx ON local_formulary.formulary_header USING btree (parent_code);
CREATE INDEX formulary_header_product_type_idx ON local_formulary.formulary_header USING btree (product_type);
CREATE INDEX formulary_header_rec_status_code_idx ON local_formulary.formulary_header USING btree (rec_status_code);

-- Table Triggers

create trigger udt_formulary_record_changes after
insert
    or
update
    of name on
    local_formulary.formulary_header for each row execute procedure local_formulary.udf_update_formulary_record_changes();


-- local_formulary.formulary_rule_config definition

-- Drop table

DROP TABLE IF EXISTS local_formulary.formulary_rule_config CASCADE;

CREATE TABLE local_formulary.formulary_rule_config (
	"_row_id" varchar(255) NOT NULL DEFAULT public.uuid_generate_v4(),
	"_sequenceid" serial NOT NULL,
	"_contextkey" varchar(255) NULL,
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdsource" varchar(255) NULL,
	"_createdby" varchar(255) NULL,
	"_recordstatus" int2 NULL DEFAULT 1,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	"name" varchar(100) NULL,
	config_json text NULL,
	"_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_updateddate" timestamp NULL DEFAULT now(),
	"_updatedby" varchar(255) NULL,
	CONSTRAINT formulary_rule_config_pkey PRIMARY KEY (_row_id)
);


-- local_formulary.lookup_common definition

-- Drop table

DROP TABLE IF EXISTS local_formulary.lookup_common CASCADE;

CREATE TABLE local_formulary.lookup_common (
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
	cd varchar(50) NULL,
	"desc" varchar(1000) NULL,
	"type" varchar(100) NULL,
	isdefault bool NULL,
	"_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_updateddate" timestamp NULL DEFAULT now(),
	"_updatedby" varchar(255) NULL
);
CREATE INDEX lookup_common_cd_idx ON local_formulary.lookup_common USING btree (cd);
CREATE INDEX lookup_common_desc_idx ON local_formulary.lookup_common USING btree ("desc");
CREATE INDEX lookup_common_type_idx ON local_formulary.lookup_common USING btree (type);


-- local_formulary.formulary_additional_code definition

-- Drop table

DROP TABLE IF EXISTS local_formulary.formulary_additional_code CASCADE;

CREATE TABLE local_formulary.formulary_additional_code (
	"_row_id" varchar(255) NOT NULL DEFAULT public.uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_version_id varchar(255) NULL,
	additional_code varchar(500) NULL,
	additional_code_system varchar(500) NULL,
	"_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_updateddate" timestamp NULL DEFAULT now(),
	"_updatedby" varchar(255) NULL,
	additional_code_desc text NULL,
	attr1 text NULL,
	meta_json text NULL,
	"source" varchar(500) NULL DEFAULT 'M'::character varying,
	code_type varchar(1000) NULL DEFAULT 'Classification'::character varying,
	CONSTRAINT formulary_additional_code_pk PRIMARY KEY (_row_id),
	CONSTRAINT formulary_additional_code_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id)
);


-- local_formulary.formulary_detail definition

-- Drop table

DROP TABLE IF EXISTS local_formulary.formulary_detail CASCADE;

CREATE TABLE local_formulary.formulary_detail (
	"_row_id" varchar(255) NOT NULL DEFAULT public.uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_version_id varchar(255) NULL,
	medication_type_code varchar(50) NULL,
	rnoh_formulary_statuscd varchar(50) NULL,
	inpatient_medication_cd varchar(50) NULL,
	outpatient_medication_cd varchar(50) NULL,
	prescribing_status_cd varchar(50) NULL,
	rules_cd varchar(50) NULL,
	unlicensed_medication_cd varchar(50) NULL,
	defined_daily_dose varchar(255) NULL,
	not_for_prn varchar(10) NULL,
	high_alert_medication varchar(10) NULL,
	ignore_duplicate_warnings varchar(10) NULL,
	medusa_preparation_instructions varchar(255) NULL,
	critical_drug varchar(10) NULL,
	controlled_drug_category_cd varchar(50) NULL,
	cytotoxic varchar(10) NULL,
	clinical_trial_medication varchar(10) NULL,
	fluid varchar(10) NULL,
	antibiotic varchar(10) NULL,
	anticoagulant varchar(10) NULL,
	antipsychotic varchar(10) NULL,
	antimicrobial varchar(10) NULL,
	add_review_reminder bool NULL,
	iv_to_oral varchar(10) NULL,
	titration_type_cd varchar(50) NULL,
	rounding_factor_cd varchar(50) NULL,
	max_dose_numerator numeric(100,4) NULL,
	maximum_dose_unit_cd varchar(50) NULL,
	witnessing_required varchar(10) NULL,
	nice_ta varchar(255) NULL,
	marked_modifier_cd varchar(50) NULL,
	insulins varchar(10) NULL,
	mental_health_drug varchar(10) NULL,
	basis_of_preferred_name_cd varchar(50) NULL,
	sugar_free varchar(10) NULL,
	gluten_free varchar(10) NULL,
	preservative_free varchar(10) NULL,
	cfc_free varchar(10) NULL,
	dose_form_cd varchar(50) NULL,
	unit_dose_form_size numeric(20,4) NULL,
	unit_dose_form_units varchar(18) NULL,
	unit_dose_unit_of_measure_cd varchar(50) NULL,
	form_cd varchar(255) NULL,
	trade_family_cd varchar(18) NULL,
	modified_release_cd varchar(50) NULL,
	black_triangle varchar(10) NULL,
	supplier_cd varchar(50) NULL,
	current_licensing_authority_cd varchar(50) NULL,
	ema_additional_monitoring varchar(10) NULL,
	parallel_import varchar(10) NULL,
	restrictions_on_availability_cd varchar(50) NULL,
	"_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_updateddate" timestamp NULL DEFAULT now(),
	"_updatedby" varchar(255) NULL,
	drug_class varchar(255) NULL,
	restriction_note text NULL,
	restricted_prescribing varchar(10) NULL,
	side_effect text NULL,
	caution text NULL,
	contra_indication text NULL,
	safety_message text NULL,
	custom_warning text NULL,
	endorsement text NULL,
	licensed_use text NULL,
	unlicensed_use text NULL,
	orderable_formtype_cd varchar(50) NULL,
	trade_family_name varchar(500) NULL,
	expensive_medication varchar(50) NULL,
	is_blood_product bool NULL,
	is_diluent bool NULL,
	is_modified_release bool NULL,
	is_gastro_resistant bool NULL,
	prescribable bool NULL DEFAULT true,
	controlled_drug_category_source varchar(100) NULL,
	black_triangle_source varchar(100) NULL,
	high_alert_medication_source varchar(100) NULL,
	prescribable_source varchar(100) NULL,
	is_custom_controlled_drug bool NULL,
	diluent text NULL,
	supplier_name varchar(1000) NULL,
	is_indication_mandatory bool NULL,
	CONSTRAINT formulary_detail_pk PRIMARY KEY (_row_id),
	CONSTRAINT formulary_detail_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id)
);
CREATE INDEX formulary_detail_formulary_version_id_idx ON local_formulary.formulary_detail USING btree (formulary_version_id);
CREATE INDEX formulary_detail_rnoh_formulary_statuscd_idx ON local_formulary.formulary_detail USING btree (rnoh_formulary_statuscd);


-- local_formulary.formulary_excipient definition

-- Drop table

DROP TABLE IF EXISTS local_formulary.formulary_excipient CASCADE;

CREATE TABLE local_formulary.formulary_excipient (
	"_row_id" varchar(255) NOT NULL DEFAULT public.uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_version_id varchar(255) NULL,
	ingredient_cd varchar(18) NULL,
	strength varchar(20) NULL,
	strength_unit_cd varchar(18) NULL,
	"_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_updateddate" timestamp NULL DEFAULT now(),
	"_updatedby" varchar(255) NULL,
	CONSTRAINT formulary_excipient_pk PRIMARY KEY (_row_id),
	CONSTRAINT formulary_excipient_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id)
);


-- local_formulary.formulary_indication definition

-- Drop table

DROP TABLE IF EXISTS local_formulary.formulary_indication CASCADE;

CREATE TABLE local_formulary.formulary_indication (
	"_row_id" varchar(255) NOT NULL DEFAULT public.uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_version_id varchar(255) NULL,
	indication_cd varchar(50) NULL,
	"_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_updateddate" timestamp NULL DEFAULT now(),
	"_updatedby" varchar(255) NULL,
	indication_name varchar(500) NULL,
	CONSTRAINT formulary_indication_pk PRIMARY KEY (_row_id),
	CONSTRAINT formulary_indication_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id)
);


-- local_formulary.formulary_ingredient definition

-- Drop table

DROP TABLE IF EXISTS local_formulary.formulary_ingredient CASCADE;

CREATE TABLE local_formulary.formulary_ingredient (
	"_row_id" varchar(255) NOT NULL DEFAULT public.uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_version_id varchar(255) NULL,
	ingredient_cd varchar(18) NULL,
	basis_of_pharmaceutical_strength_cd varchar(50) NULL,
	strength_value_numerator varchar(20) NULL,
	strength_value_numerator_unit_cd varchar(18) NULL,
	"_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_updateddate" timestamp NULL DEFAULT now(),
	"_updatedby" varchar(255) NULL,
	strength_value_denominator varchar(20) NULL,
	strength_value_denominator_unit_cd varchar(20) NULL,
	ingredient_name varchar(1000) NULL,
	CONSTRAINT formulary_ingredient_pk PRIMARY KEY (_row_id),
	CONSTRAINT formulary_ingredient_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id)
);


-- local_formulary.formulary_ontology_form definition

-- Drop table

DROP TABLE IF EXISTS local_formulary.formulary_ontology_form CASCADE;

CREATE TABLE local_formulary.formulary_ontology_form (
	"_row_id" varchar(255) NOT NULL DEFAULT public.uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_version_id varchar(255) NULL,
	form_cd varchar(50) NULL,
	"_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_updateddate" timestamp NULL DEFAULT now(),
	"_updatedby" varchar(255) NULL,
	CONSTRAINT formulary_ontology_form_pk PRIMARY KEY (_row_id),
	CONSTRAINT formulary_ontology_form_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id)
);


-- local_formulary.formulary_route_detail definition

-- Drop table

DROP TABLE IF EXISTS local_formulary.formulary_route_detail CASCADE;

CREATE TABLE local_formulary.formulary_route_detail (
	"_row_id" varchar(255) NOT NULL DEFAULT public.uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_version_id varchar(255) NULL,
	route_cd varchar(50) NULL,
	route_field_type_cd varchar(50) NULL,
	"_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_updateddate" timestamp NULL DEFAULT now(),
	"_updatedby" varchar(255) NULL,
	"source" varchar(100) NULL,
	CONSTRAINT formulary_route_detail_pk PRIMARY KEY (_row_id),
	CONSTRAINT formulary_route_detail_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id)
);