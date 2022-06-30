set schema 'local_formulary';

--DROP table local_formulary.lookup_common;
CREATE TABLE local_formulary.lookup_common (
	"_row_id" varchar(255) null default uuid_generate_v4(),
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
	"desc" varchar(1000) null,
	"type" varchar(100) null,
	--"islatest" int2 default 1,
	"isdefault" bool null
);

CREATE INDEX lookup_common_desc_idx ON local_formulary.lookup_common ("desc");
CREATE INDEX lookup_common_cd_idx ON local_formulary.lookup_common (cd);
CREATE INDEX lookup_common_type_idx ON local_formulary.lookup_common ("type");

--truncate table local_formulary.lookup_common;
--select * from local_formulary.lookup_common;
INSERT INTO local_formulary.lookup_common
( "_contextkey", "_createdtimestamp", "_createddate", "_createdsource", "_createdmessageid", "_createdby", "_recordstatus", "_timezonename", "_timezoneoffset", "_tenant", cd, "desc","type")
values 

('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Medication','MedicationType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', 'Diluent','MedicationType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '003', 'Medicinal gas','MedicationType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '004', 'Blood product','MedicationType'),
--===========
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 1','Multi-ingredient drugs','Rules')
,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 2','No VTM','Rules')
,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 3','Never valid as a VMP','Rules')
,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 4','‘Co-‘name drugs','Rules')
,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 5a',' VMP not recommended (non-equivalence)','Rules')
,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 5b','VMP not recommended (patient training)','Rules')
,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 5c','VMP not recommended (no product specification)','Rules')
,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 6','Controlled Drugs','Rules')
,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 7',' Injection or infusion','Rules')
,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 8',' Inhaled dose forms','Rules')
,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 9','Differing release characteristics','Rules')
,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 10','Topical forms','Rules'),

--====
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Draft','RecordStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', 'Approved','RecordStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '003', 'Active','RecordStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '004', 'Archived','RecordStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '005', 'Inactive','RecordStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '006', 'Deleted','RecordStatus'),

--===================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Formulary','FormularyStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', 'Non-Formulary','FormularyStatus'),
--==================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '000', 'Not Orderable','OrderableStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Orderable','OrderableStatus'),
--==============================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Blood Sugar','TitrationType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', 'INR','TitrationType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '003', 'MAP','TitrationType'),
--==============================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', '0.5','RoundingFactor'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', '1','RoundingFactor'),
--==============================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '9906301000001103', 'Modified Release','Modifier'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '9906701000001104', 'Gastro-Resistant','Modifier'),
--==============================
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '1', 'Discrete','DoseForm'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '2', 'Continuous','DoseForm'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '3', 'Not applicable','DoseForm'),
--==============================
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '1', 'Based on Ingredient Substance','PharamceuticalStrength'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '2', 'Based on Base Substance','PharamceuticalStrength'),
--==============================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '9906401000001106', 'Modified release 12 hour','ModifiedRelease'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '9906901000001102', 'Modified release 24 hour','ModifiedRelease'),
--===================================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Additonal','RouteFieldType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', 'UnLicensed','RouteFieldType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '003', 'Normal','RouteFieldType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '004', 'Discretionary','RouteFieldType'),

--===================================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Simple','OrderFormType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', 'Complex','OrderFormType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '003', 'Descriptive','OrderFormType'),
--====================================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'VTM', 'VTM','ProductType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'VMP', 'VMP','ProductType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'AMP', 'AMP','ProductType'),
--===========================================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'BNF', 'BNF','CodeSystem'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'ATC', 'ATC','CodeSystem'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Local', 'Local','CodeSystem'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Other', 'Other','CodeSystem'),

--=============================================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Generic', 'Generic','DrugClass'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Branded', 'Brand','DrugClass')
--=============================================
;



INSERT INTO local_formulary.formulary_rule_config
("_row_id", "_contextkey", "_createdtimestamp", "_createddate", "_createdsource", "_createdby", "_recordstatus", 
"_timezonename", "_timezoneoffset", "_tenant", "name", config_json, "_updatedtimestamp", "_updateddate", "_updatedby")
VALUES(
uuid_generate_v4(), '', timezone('UTC'::text, now()), now(), '', 'SYSTEM', 1, '', 0, '', 'DUPLICATION_CHECK_FIELDS', 
'{"context":{"header":["Name","ProductType","Code"],"detail":["ControlledDrugCategoryCd","DoseFormCd",
"UnitDoseFormUnits","UnitDoseUnitOfMeasureCd","FormCd","SupplierCd"],"ingredient":["IngredientCd",
"StrengthValueNumerator","StrengthValueNumeratorUnitCd","StrengthValueDenominator","StrengthValueDenominatorUnitCd"],
"ontology":["FormCd"]},"noncontext":{"additonalcodes":["AdditionalCode","AdditionalCodeSystem"],
"detail":["MedicationTypeCode","CodeSystem","RnohFormularyStatuscd","OrderableCd","InpatientMedicationCd",
"OutpatientMedicationCd","PrescribingStatusCd","RulesCd","UnlicensedMedicationCd","DefinedDailyDose","NotForPrn",
"HighAlertMedication","IgnoreDuplicateWarnings","MedusaPreparationInstructions","DrugClass","CriticalDrug",
"Cytotoxic","ClinicalTrialMedication","Fluid","Antibiotic","Anticoagulant","Antipsychotic","Antimicrobial",
"AddReviewReminder","IvToOral","TitrationTypeCd","RoundingFactorCd","MaxDoseNumerator","MaximumDoseUnitCd",
"WitnessingRequired","RestrictedPrescribing","NiceTa","MarkedModifierCd","Insulins","MentalHealthDrug",
"BasisOfPreferredNameCd","SugarFree","GlutenFree","PreservativeFree","CfcFree","UnitDoseFormSize","TradeFamilyCd",
"OrderableFormtypeCd","ModifiedReleaseCd","BlackTriangle","CurrentLicensingAuthorityCd","EmaAdditionalMonitoring",
"ParallelImport","RestrictionsOnAvailabilityCd"],"indication":["IndicationCd"],"ingredient":["IngredientCd",
"BasisOfPharmaceuticalStrengthCd"],"routedetail":["RouteCd","RouteFieldTypeCd"]}}',
timezone('UTC'::text, now()),
now(), 'SYSTEM');

INSERT INTO local_formulary.formulary_rule_config
("_row_id", "_contextkey", "_createdtimestamp", "_createddate", "_createdsource", "_createdby", "_recordstatus", 
"_timezonename", "_timezoneoffset", "_tenant", "name", config_json, "_updatedtimestamp", "_updateddate", "_updatedby")
VALUES(
uuid_generate_v4(), '', timezone('UTC'::text, now()), now(), '', 'SYSTEM', 1, '', 0, '', 'MMC_Control_Source', 
'{"MMC_Control_Source":[{"ControlLabelId":"Name","ControlLabelName":"Name","ControlDataSource":"DMD"},
{"ControlLabelId":"Product_Type","ControlLabelName":"Product Type","ControlDataSource":"DMD"},
{"ControlLabelId":"Medication_Type","ControlLabelName":"Medication Type","ControlDataSource":"RNOH"},
{"ControlLabelId":"Fluid","ControlLabelName":"Fluid","ControlDataSource":"RNOH"},
{"ControlLabelId":"Code","ControlLabelName":"Code","ControlDataSource":"DMD"},
{"ControlLabelId":"Code_System","ControlLabelName":"Code System","ControlDataSource":"System"},
{"ControlLabelId":"Additional_Code","ControlLabelName":"Additional Code","ControlDataSource":"RNOH"},
{"ControlLabelId":"Additional_Code_Type","ControlLabelName":"Additional Code Type","ControlDataSource":"RNOH"},
{"ControlLabelId":"Basis_of_preferred_name","ControlLabelName":"Basis of preferred name","ControlDataSource":"DMD"},
{"ControlLabelId":"Licensed_Use","ControlLabelName":"Licensed Use","ControlDataSource":"FDB"},
{"ControlLabelId":"Current_licensing_authority","ControlLabelName":"Current licensing authority","ControlDataSource":"DMD"},
{"ControlLabelId":"Supplier","ControlLabelName":"Supplier","ControlDataSource":"DMD"},
{"ControlLabelId":"Trade_Family","ControlLabelName":"Trade Family","ControlDataSource":"MMC"},
{"ControlLabelId":"Virtual_Therapeutic_Moiety","ControlLabelName":"Virtual Therapeutic Moiety","ControlDataSource":"DMD"},
{"ControlLabelId":"Virtual_Medicinal_Product","ControlLabelName":"Virtual Medicinal Product","ControlDataSource":"DMD"},
{"ControlLabelId":"Virtual_Medicinal_Product","ControlLabelName":"Virtual Medicinal Product","ControlDataSource":"DMD"},
{"ControlLabelId":"Dose_Form","ControlLabelName":"Dose Form","ControlDataSource":"DMD"},
{"ControlLabelId":"Form_&_Route","ControlLabelName":"Form & Route","ControlDataSource":"DMD"},
{"ControlLabelId":"Maximum_Dose_Numerator","ControlLabelName":"Maximum Dose Numerator","ControlDataSource":"MMC"},
{"ControlLabelId":"Maximum_Dose_Unit","ControlLabelName":"Maximum Dose Unit","ControlDataSource":"MMC"},
{"ControlLabelId":"Rounding_Factor","ControlLabelName":"Rounding Factor","ControlDataSource":"RNOH"},
{"ControlLabelId":"Route","ControlLabelName":"Route","ControlDataSource":"DMD"},
{"ControlLabelId":"Unlicensed_Route","ControlLabelName":"Unlicensed Route","ControlDataSource":"RNOH"},
{"ControlLabelId":"Form","ControlLabelName":"Form","ControlDataSource":"DMD"},
{"ControlLabelId":"Unit_dose_form_size","ControlLabelName":"Unit dose form size","ControlDataSource":"DMD"},
{"ControlLabelId":"Unit_dose_form_units","ControlLabelName":"Unit dose form units","ControlDataSource":"DMD"},
{"ControlLabelId":"Unit_dose_unit_of_measure","ControlLabelName":"Unit dose unit of measure","ControlDataSource":"DMD"},
{"ControlLabelId":"Order_Form_Type","ControlLabelName":"Order Form Type","ControlDataSource":"RNOH"},
{"ControlLabelId":"Ingredient","ControlLabelName":"Ingredient","ControlDataSource":"DMD"},
{"ControlLabelId":"Strength_Value_Numerator","ControlLabelName":"Strength Value Numerator","ControlDataSource":"DMD"},
{"ControlLabelId":"Strength_Value_NumeratorUOM","ControlLabelName":"Strength Value NumeratorUOM","ControlDataSource":"DMD"},
{"ControlLabelId":"Strength_Value_Denominator","ControlLabelName":"Strength Value Denominator","ControlDataSource":"DMD"},
{"ControlLabelId":"Strength_Value_DenominatorUOM","ControlLabelName":"Strength Value DenominatorUOM","ControlDataSource":"DMD"},
{"ControlLabelId":"Basis_of_pharmaceutical_strength","ControlLabelName":"Basis of pharmaceutical strength","ControlDataSource":"DMD"},
{"ControlLabelId":"Unlicensed_Use","ControlLabelName":"Unlicensed Use","ControlDataSource":"FDB"},
{"ControlLabelId":"Contraindications","ControlLabelName":"Contraindications","ControlDataSource":"FDB"},
{"ControlLabelId":"Custom_Warning","ControlLabelName":"Custom Warning","ControlDataSource":"RNOH"},
{"ControlLabelId":"Endorsements","ControlLabelName":"Endorsements","ControlDataSource":"RNOH"},
{"ControlLabelId":"Important_Safety_Information","ControlLabelName":"Important Safety Information","ControlDataSource":"FDB"},
{"ControlLabelId":"Indication","ControlLabelName":"Indication","ControlDataSource":"RNOH"},
{"ControlLabelId":"Medusa_Preparation_Instructions","ControlLabelName":"Medusa Preparation Instructions","ControlDataSource":"RNOH"},
{"ControlLabelId":"NICE_TA","ControlLabelName":"NICE TA","ControlDataSource":"RNOH"},
{"ControlLabelId":"Notes_for_Restriction","ControlLabelName":"Notes for Restriction","ControlDataSource":"RNOH"},
{"ControlLabelId":"Side_Effects","ControlLabelName":"Side Effects","ControlDataSource":"FDB"},
{"ControlLabelId":"Antibiotic","ControlLabelName":"Antibiotic","ControlDataSource":"RNOH"},
{"ControlLabelId":"Anticoagulant","ControlLabelName":"Anticoagulant","ControlDataSource":"RNOH"},
{"ControlLabelId":"Antipsychotic","ControlLabelName":"Antipsychotic","ControlDataSource":"RNOH"},
{"ControlLabelId":"Antimicrobial","ControlLabelName":"Antimicrobial","ControlDataSource":"RNOH"},
{"ControlLabelId":"ATC_Code","ControlLabelName":"ATC Code","ControlDataSource":"DMD"},
{"ControlLabelId":"Black_Triangle","ControlLabelName":"Black Triangle","ControlDataSource":"FDB"},
{"ControlLabelId":"Cautions","ControlLabelName":"Cautions","ControlDataSource":"FDB"},
{"ControlLabelId":"CFC_Free","ControlLabelName":"CFC Free","ControlDataSource":"DMD"},
{"ControlLabelId":"Class","ControlLabelName":"Class","ControlDataSource":"RNOH"},
{"ControlLabelId":"Clinical_Trial_Medication","ControlLabelName":"Clinical Trial Medication","ControlDataSource":"RNOH"},
{"ControlLabelId":"Controlled_Drug_Category","ControlLabelName":"Controlled Drug Category","ControlDataSource":"DMD"},
{"ControlLabelId":"Critical_Drug","ControlLabelName":"Critical Drug","ControlDataSource":"RNOH"},
{"ControlLabelId":"Cytotoxic","ControlLabelName":"Cytotoxic","ControlDataSource":"RNOH"},
{"ControlLabelId":"EMA_additional_monitoring","ControlLabelName":"EMA additional monitoring","ControlDataSource":"DMD"},
{"ControlLabelId":"Expensive_Medication","ControlLabelName":"Expensive Medication","ControlDataSource":"RNOH"},
{"ControlLabelId":"Gluten_Free","ControlLabelName":"Gluten Free","ControlDataSource":"DMD"},
{"ControlLabelId":"High_Alert_Medication","ControlLabelName":"High Alert Medication","ControlDataSource":"RNOH"},{"ControlLabelId":"Insulins","ControlLabelName":"Insulins","ControlDataSource":"RNOH"},{"ControlLabelId":"IV_to_Oral","ControlLabelName":"IV to Oral","ControlDataSource":"RNOH"},{"ControlLabelId":"Marked_Modifier","ControlLabelName":"Marked Modifier","ControlDataSource":"RNOH"},{"ControlLabelId":"Mental_Health_Drug","ControlLabelName":"Mental Health Drug","ControlDataSource":"RNOH"},{"ControlLabelId":"Modified_Release","ControlLabelName":"Modified Release","ControlDataSource":"RNOH"},{"ControlLabelId":"Not_for_PRN","ControlLabelName":"Not for PRN","ControlDataSource":"RNOH"},{"ControlLabelId":"Orderable","ControlLabelName":"Orderable","ControlDataSource":"RNOH"},{"ControlLabelId":"Outpatient_Medication","ControlLabelName":"Outpatient Medication","ControlDataSource":"RNOH"},{"ControlLabelId":"Prescribing_Status","ControlLabelName":"Prescribing Status","ControlDataSource":"DMD"},{"ControlLabelId":"Preservative_Free","ControlLabelName":"Preservative Free","ControlDataSource":"DMD"},{"ControlLabelId":"Restricted_Prescribing","ControlLabelName":"Restricted Prescribing","ControlDataSource":"RNOH"},{"ControlLabelId":"Restrictions_on_availability","ControlLabelName":"Restrictions on availability","ControlDataSource":"DMD"},{"ControlLabelId":"Review_Reminder","ControlLabelName":"Review Reminder","ControlDataSource":"RNOH"},{"ControlLabelId":"Sugar_Free","ControlLabelName":"Sugar Free","ControlDataSource":"DMD"},{"ControlLabelId":"Titration_Type","ControlLabelName":"Titration Type","ControlDataSource":"RNOH"},{"ControlLabelId":"Unlicensed_Medication","ControlLabelName":"Unlicensed Medication","ControlDataSource":"RNOH"},{"ControlLabelId":"Parallel_import","ControlLabelName":"Parallel import","ControlDataSource":"DMD"},
{"ControlLabelId":"ID","ControlLabelName":"ID","ControlDataSource":"System"},{"ControlLabelId":"Status","ControlLabelName":"Status","ControlDataSource":"RNOH"},{"ControlLabelId":"Formulary_Status","ControlLabelName":"Formulary Status","ControlDataSource":"RNOH"},{"ControlLabelId":"Ignore_Duplicate_Warnings","ControlLabelName":"Ignore Duplicate Warnings","ControlDataSource":"RNOH"},{"ControlLabelId":"Inpatient_Medication","ControlLabelName":"Inpatient Medication","ControlDataSource":"RNOH"},{"ControlLabelId":"Witnessing_Required",
"ControlLabelName":"Witnessing Required","ControlDataSource":"RNOH"}]}',
timezone('UTC'::text, now()),
now(), 'SYSTEM');


--===================================================================================================================
set schema 'local_formulary';

--drop table local_formulary.formulary_header;
CREATE TABLE local_formulary.formulary_header (
	"_row_id" varchar(255) null default uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_id varchar(255),
	"version_id" int,
	formulary_version_id varchar(255) null,
	code varchar(255),
	"name" text,
	name_tokens tsvector null,
	"product_type" varchar(100),
	"parent_code" varchar(255) NULL,
	"parent_name" text NULL,
	parent_name_tokens tsvector null,
	"parent_product_type" varchar(100) NULL,
	"rec_status_code" varchar(8) NULL,
	"rec_statuschange_ts" timestamptz null,
	"rec_statuschange_date" timestamp null,
	"rec_statuschange_tzname" varchar(255) NULL,
	"rec_statuschange_tzoffset" int4 NULL,
	is_duplicate bool NULL,
	is_latest bool null,
	rec_source varchar(50) null,
	vtm_id varchar(100) null,
	vmp_id varchar(100) null
);

ALTER TABLE local_formulary.formulary_header ADD CONSTRAINT formulary_header_pk PRIMARY KEY (formulary_version_id);
CREATE INDEX formulary_header_name_tokens_idx ON local_formulary.formulary_header using gin(name_tokens);

-- local_formulary.formulary_ontology_form definition

-- Drop table

-- DROP TABLE local_formulary.formulary_ontology_form;

CREATE TABLE local_formulary.formulary_ontology_form (
	"_row_id" varchar(255) NOT NULL DEFAULT uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_id varchar(255) NULL,
	version_id int4 NULL,
	formulary_version_id varchar(255) NULL,
	form_cd varchar(50) NULL,
	"_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_updateddate" timestamp NULL DEFAULT now(),
	"_updatedby" varchar(255) NULL,
	CONSTRAINT formulary_ontology_form_pk PRIMARY KEY (_row_id)
);


-- local_formulary.formulary_ontology_form foreign keys

ALTER TABLE local_formulary.formulary_ontology_form ADD CONSTRAINT formulary_ontology_form_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);


--drop table local_formulary.formulary_detail;
CREATE TABLE local_formulary.formulary_detail (
	"_row_id" varchar(255) null default uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_id varchar(255),
	"version_id" int,
	formulary_version_id varchar(255) null,-- default uuid_generate_v4(),
	"medication_type_code" int8 NULL,
	"code_system" varchar(100) null,
	atc_code varchar(255) null,
	rnoh_formulary_statuscd varchar(50) null,
	orderable_cd varchar(50) null,
	inpatient_medication_cd varchar(50) null,
	outpatient_medication_cd varchar(50) null,
	prescribing_status_cd varchar(50) null,
	rules_cd varchar(50) null,
	unlicensed_medication_cd varchar(50) null,
	defined_daily_dose varchar(255) NULL,
	is_not_for_prn bool null,
	is_high_alert_medication bool null,
	ignore_duplicate_warnings bool null,
	medusa_preparation_instructions varchar(255) NULL,
	is_critical_drug bool null,
	controlled_drug_category_cd varchar(50) null,
	is_cytotoxic bool null,
	is_clinical_trial_medication bool null,
	is_fluid bool null,
	is_antibiotic bool null,
	is_anticoagulant bool null,
	is_antipsychotic bool null,
	is_antimicrobial bool null,
	add_review_reminder bool null,
	is_IV_to_oral bool null,
	titration_type_cd varchar(50) null,
	rounding_factor_cd varchar(50) null, 
	max_dose_numerator numeric(100,4) null,
	maximum_dose_unit_cd varchar(50) null, 
	is_witnessing_required bool null,
	nice_ta varchar(255) null,
	--vtm_id varchar(18) null,
	marked_modifier_cd varchar(50) null,
	is_insulins bool null,
	is_mental_health_drug bool null,
	basis_of_preferred_name_cd  varchar(50) null, 
	is_sugar_free bool null,
	is_gluten_free bool null,
	is_preservative_free bool null,
	is_cfc_free bool null,
	dose_form_cd  varchar(50) null, 
	unit_dose_form_size numeric(20,4) null,
	unit_dose_form_units varchar(18) null, --SNOMED Code
	unit_dose_unit_of_measure_cd varchar(50) null,
	form_route_cd varchar(50) null,
	formulation_cd varchar(50) null,
	trade_family_cd varchar(18) null, --SNOMED CT Code
	modified_release_cd varchar(50) null,
	is_black_triangle bool null,
	supplier_cd varchar(50) null,
	current_licensing_authority_cd varchar(50) null,
	is_EMA_additional_monitoring bool null,
	is_Parallel_import  bool null,
	restrictions_on_availability_cd varchar(50) null

);

ALTER TABLE local_formulary.formulary_detail ADD CONSTRAINT formulary_detail_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);
ALTER TABLE local_formulary.formulary_detail ADD CONSTRAINT formulary_detail_pk PRIMARY KEY ("_row_id");



--drop table local_formulary.formulary_additional_code;
CREATE TABLE local_formulary.formulary_additional_code (
	"_row_id" varchar(255) null default uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_id varchar(255),
	"version_id" int,
	formulary_version_id varchar(255) null,-- default uuid_generate_v4(),
	"additional_code" varchar(50) null,
	"additional_code_system" varchar(50) null
);

ALTER TABLE local_formulary.formulary_additional_code ADD CONSTRAINT formulary_additional_code_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);
ALTER TABLE local_formulary.formulary_additional_code ADD CONSTRAINT formulary_additional_code_pk PRIMARY KEY ("_row_id");


--drop table local_formulary.formulary_route_detail;
CREATE TABLE local_formulary.formulary_route_detail (
	"_row_id" varchar(255) null default uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_id varchar(255),
	"version_id" int,
	formulary_version_id varchar(255) null,-- default uuid_generate_v4(),	
	"route_cd" varchar(50) null,
	"route_field_type_cd" varchar(50) null
);

ALTER TABLE local_formulary.formulary_route_detail ADD CONSTRAINT formulary_route_detail_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);
ALTER TABLE local_formulary.formulary_route_detail ADD CONSTRAINT formulary_route_detail_pk PRIMARY KEY ("_row_id");


--drop table local_formulary.formulary_supplier;
--CREATE TABLE local_formulary.formulary_supplier (
--	"_row_id" varchar(255) null default uuid_generate_v4(),
--	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
--	"_createddate" timestamp NULL DEFAULT now(),
--	"_createdby" varchar(255) NULL,
--	"_timezonename" varchar(255) NULL,
--	"_timezoneoffset" int4 NULL,
--	"_tenant" varchar(255) NULL,
--	formulary_id varchar(255),
--	"version_id" int,
--	formulary_version_id varchar(255) null,-- default uuid_generate_v4(),
--	"supplier_cd" varchar(50) null
--);
--
--ALTER TABLE local_formulary.formulary_supplier ADD CONSTRAINT formulary_supplier_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);
--ALTER TABLE local_formulary.formulary_supplier ADD CONSTRAINT formulary_supplier_pk PRIMARY KEY ("_row_id");


--drop table local_formulary.formulary_indication;
CREATE TABLE local_formulary.formulary_indication (
	"_row_id" varchar(255) null default uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_id varchar(255),
	"version_id" int,
	formulary_version_id varchar(255) null,-- default uuid_generate_v4(),
	"indication_cd" varchar(50) null -- SNOMED CT Code
);

ALTER TABLE local_formulary.formulary_indication ADD CONSTRAINT formulary_indication_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);
ALTER TABLE local_formulary.formulary_indication ADD CONSTRAINT formulary_indication_pk PRIMARY KEY ("_row_id");


--drop table local_formulary.formulary_ingredient;
CREATE TABLE local_formulary.formulary_ingredient (
	"_row_id" varchar(255) null default uuid_generate_v4(),
	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
	"_createddate" timestamp NULL DEFAULT now(),
	"_createdby" varchar(255) NULL,
	"_timezonename" varchar(255) NULL,
	"_timezoneoffset" int4 NULL,
	"_tenant" varchar(255) NULL,
	formulary_id varchar(255),
	"version_id" int,
	formulary_version_id varchar(255) null,-- default uuid_generate_v4(),
	"ingredient_cd" varchar(18) null, -- SNOMED CT Code
	basis_of_pharmaceutical_strength_cd varchar(50) null,
	strength_value_numerator varchar(20) null,
	strength_value_numerator_unit_cd varchar(18) null -- SNOMED CT Code
	
);

ALTER TABLE local_formulary.formulary_ingredient ADD CONSTRAINT formulary_ingredient_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);
ALTER TABLE local_formulary.formulary_ingredient ADD CONSTRAINT formulary_ingredient_pk PRIMARY KEY ("_row_id");



--drop table local_formulary.formulary_rule_config;
CREATE TABLE local_formulary.formulary_rule_config (
	"_row_id" varchar(255) null default uuid_generate_v4(),
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
	"name" varchar(100),
	config_json text,
  	PRIMARY KEY(_row_id)

);

select * from local_formulary.formulary_rule_config;
--===================================================================================================
--No underscore for columns
--=====================================
select * from local_formulary.formulary_header fh;

set schema 'local_formulary';

--drop function local_formulary.udf_formulary_search_nodes_with_descendents;
CREATE or replace function local_formulary.udf_formulary_search_nodes_with_descendents(
IN in_name text default null,
in in_recordstatus_codes text[] default null,
in in_rnoh_formulary_status_codes text[] default null
) 
RETURNS TABLE(formularyid varchar(255), versionid INT, formularyversionid varchar(255), name text, code varchar(255),
"producttype" varchar(100),parentcode varchar(255),parentname text,"parentproducttype" varchar(100),isduplicate bool,
recstatuscode text, rnohformularystatuscd text) 

AS 
	
$$
WITH RECURSIVE search_formulary(      
		formulary_id,
		version_id,
		formulary_version_id,
		name,
		code,
		"product_type",
		parent_code,
		parent_name,
		"parent_product_type",
		is_duplicate,
		rec_status_code,
		rnoh_formulary_statuscd,
	  	srcpath  -- path, stored using an array 
	  	

	) as (
		select distinct 
			fh.formulary_id,
			fh.version_id,
			fh.formulary_version_id,
			fh."name" as "name" ,
			fh.code,
			fh.product_type,
			fh.parent_code,
			fh.parent_name,
			fh.parent_product_type,
			fh.is_duplicate,
			fh.rec_status_code,
			detail.rnoh_formulary_statuscd,
			ARRAY[fh.code ]::varchar(1022)[] as srcpath
		from local_formulary.formulary_header fh 
		inner join local_formulary.formulary_detail detail on detail.formulary_version_id  = fh.formulary_version_id 
		where fh.is_latest = true
		and (in_name is null or fh.name ilike '%'|| in_name ||'%' or fh.name ilike in_name || '%' or fh.name ilike '%'|| in_name )
		and (in_recordstatus_codes is null or fh.rec_status_code = any(in_recordstatus_codes))
		and (in_rnoh_formulary_status_codes is null or detail.rnoh_formulary_statuscd = any(in_rnoh_formulary_status_codes))
		UNION all
				select 
					fh.formulary_id,
					fh.version_id,
					fh.formulary_version_id,
					fh."name" as "name" ,
					fh.code,
					fh.product_type,
					fh.parent_code,
					fh.parent_name,
					fh.parent_product_type,
					fh.is_duplicate,
					fh.rec_status_code,
					detail.rnoh_formulary_statuscd,
					ARRAY[fh.code ]::varchar(1022)[] as srcpath
			  	FROM
			         local_formulary.formulary_header fh
			     INNER JOIN search_formulary s 
			     	ON s.code = fh.parent_code
	      			AND (fh.code <> ALL(s.srcpath))        -- prevent from cycling 
  				inner join local_formulary.formulary_detail detail on detail.formulary_version_id  = fh.formulary_version_id 
		      	where fh.is_latest = true
				and (in_recordstatus_codes is null or fh.rec_status_code = any(in_recordstatus_codes))
				and (in_rnoh_formulary_status_codes is null or detail.rnoh_formulary_statuscd = any(in_rnoh_formulary_status_codes))
		)
	
	SELECT distinct
		fh.formulary_id,
		fh.version_id,
		fh.formulary_version_id,
		fh."name" as "name" ,
		fh.code,
		fh.product_type,
		fh.parent_code,
		fh.parent_name,
		fh.parent_product_type,
		fh.is_duplicate,
		fh.rec_status_code,
		fh.rnoh_formulary_statuscd
	FROM search_formulary fh;

$$
LANGUAGE sql;

select * from local_formulary.udf_formulary_search_nodes_with_descendents('los', Array['001']);
select * from local_formulary.udf_formulary_search_nodes_with_descendents()
where formularyid = '0cd0f56e-c42f-459b-8785-d948626abb84' 
where formularyversionid = '4fc37cc1-45cd-46a0-967b-1f63aceabf05'
order by code desc;
select * from local_formulary.udf_formulary_search_nodes_with_descendents('paracetamol')

--===================
--drop function local_formulary.udf_formulary_get_descendents;
CREATE or replace function local_formulary.udf_formulary_get_descendents(IN in_formulary_version_ids text[]) 
RETURNS TABLE(formularyid varchar(255), versionid INT, formularyversionid varchar(255), name text, code varchar(255),
"producttype" varchar(100),parentcode varchar(255),parentname text,"parentproducttype" varchar(100),isduplicate bool) AS 
	
$$
WITH RECURSIVE search_formulary(      
		formulary_id,
		version_id,
		formulary_version_id,
		name,
		code,
		"product_type",
		parent_code,
		parent_name,
		"parent_product_type",
		is_duplicate,
	  	srcpath  -- path, stored using an array     

	) as (
		select distinct 
			fh.formulary_id,
			fh.version_id,
			fh.formulary_version_id,
			fh."name" as "name" ,
			fh.code,
			fh.product_type,
			fh.parent_code,
			fh.parent_name,
			fh.parent_product_type,
			fh.is_duplicate,
			ARRAY[fh.code ]::varchar(1022)[] as srcpath
		from local_formulary.formulary_header fh 
		inner join unnest(in_formulary_version_ids) m(formulary_version_id) on m.formulary_version_id = fh.formulary_version_id
		where fh.is_latest = true
		
		UNION all
				select 
					fh.formulary_id,
					fh.version_id,
					fh.formulary_version_id,
					fh."name" as "name" ,
					fh.code,
					fh.product_type,
					fh.parent_code,
					fh.parent_name,
					fh.parent_product_type,
					fh.is_duplicate,
					ARRAY[fh.code ]::varchar(1022)[] as srcpath
			  	FROM
			         local_formulary.formulary_header fh
			     INNER JOIN search_formulary s 
			     	--ON s.parent_code = fh.code
			     	ON s.code = fh.parent_code
	      			AND (fh.code <> ALL(s.srcpath))        -- prevent from cycling 
		      	where fh.is_latest = true
		)
	
	SELECT distinct
		fh.formulary_id,
		fh.version_id,
		fh.formulary_version_id,
		fh."name" as "name" ,
		fh.code,
		fh.product_type,
		fh.parent_code,
		fh.parent_name,
		fh.parent_product_type,
		fh.is_duplicate
	FROM search_formulary fh;

$$
LANGUAGE sql;

--===================================================================================================


--
---- local_formulary.lookup_material_type definition
--
---- Drop table
--
---- DROP TABLE local_formulary.lookup_material_type;
--
--CREATE TABLE local_formulary.lookup_material_type (
--	"_row_id" varchar(255) null default uuid_generate_v4(),
--	"_sequenceid" serial NOT NULL,
--	"_contextkey" varchar(255) NULL,
--	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
--	"_createddate" timestamp NULL DEFAULT now(),
--	"_createdsource" varchar(255) NULL,
--	"_createdmessageid" varchar(255) NULL,
--	"_createdby" varchar(255) NULL,
--	"_recordstatus" int2 NULL DEFAULT 1,
--	"_timezonename" varchar(255) NULL,
--	"_timezoneoffset" int4 NULL,
--	"_tenant" varchar(255) NULL,
--	cd int8 NULL,
--	"desc" varchar(1000) NULL
--);
--
--INSERT INTO local_formulary.lookup_material_type
--("_row_id", "_contextkey", "_createdtimestamp", "_createddate", "_createdsource", "_createdmessageid", "_createdby", "_recordstatus", "_timezonename", "_timezoneoffset", "_tenant", cd, "desc")
--(uuid_generate_v4(), '', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 2, 'Diluent'),
--(uuid_generate_v4(), '', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 3, 'Medicinal gas '),
--(uuid_generate_v4(), '', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 4, 'Blood product');
--
---- local_formulary.lookup_rules definition
--
---- Drop table
--
---- DROP TABLE local_formulary.lookup_rules;
--
--CREATE TABLE local_formulary.lookup_rules (
--	"_row_id" varchar(255) null default uuid_generate_v4(),
--	"_sequenceid" serial NOT NULL,
--	"_contextkey" varchar(255) NULL,
--	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
--	"_createddate" timestamp NULL DEFAULT now(),
--	"_createdsource" varchar(255) NULL,
--	"_createdmessageid" varchar(255) NULL,
--	"_createdby" varchar(255) NULL,
--	"_recordstatus" int2 NULL DEFAULT 1,
--	"_timezonename" varchar(255) NULL,
--	"_timezoneoffset" int4 NULL,
--	"_tenant" varchar(255) NULL,
--	cd varchar(100) NULL,
--	"desc" varchar(1000) NULL
--);
--
----INSERT INTO local_formulary.lookup_rules
----("_row_id", "_contextkey", "_createdtimestamp", "_createddate", "_createdsource", "_createdmessageid", "_createdby", "_recordstatus", "_timezonename", "_timezoneoffset", "_tenant", cd, "desc")
----(uuid_generate_v4(), '', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 2, 'Diluent'),
----(uuid_generate_v4(), '', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 3, 'Medicinal gas '),
----(uuid_generate_v4(), '', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 4, 'Blood product');
--
----====================================================================
--
--
---- local_formulary.lookup_record_status definition
--
---- Drop table
--
---- DROP TABLE local_formulary.lookup_record_status;
--
--CREATE TABLE local_formulary.lookup_record_status (
--	"_row_id" varchar(255) null default uuid_generate_v4(),
--	"_sequenceid" serial NOT NULL,
--	"_contextkey" varchar(255) NULL,
--	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
--	"_createddate" timestamp NULL DEFAULT now(),
--	"_createdsource" varchar(255) NULL,
--	"_createdmessageid" varchar(255) NULL,
--	"_createdby" varchar(255) NULL,
--	"_recordstatus" int2 NULL DEFAULT 1,
--	"_timezonename" varchar(255) NULL,
--	"_timezoneoffset" int4 NULL,
--	"_tenant" varchar(255) NULL,
--	cd varchar(100) NULL,
--	"desc" varchar(1000) NULL
--);
--
----truncate table local_formulary.lookup_record_status
--INSERT INTO local_formulary.lookup_record_status
--("_contextkey", "_createdtimestamp", "_createddate", "_createdsource", "_createdmessageid", "_createdby", "_recordstatus", "_timezonename", "_timezoneoffset", "_tenant", cd, "desc")
--values
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '1', 'Draft'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '2', 'Approved'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '3', 'Active'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '4', 'Archived'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '5', 'InActive')
--;
--
--
----====================================================================
--
--
---- local_formulary.lookup_rnoh_formulary_status definition
--
---- Drop table
--
---- DROP TABLE local_formulary.lookup_rnoh_formulary_status;
--
--CREATE TABLE local_formulary.lookup_rnoh_formulary_status (
--	"_row_id" varchar(255) null default uuid_generate_v4(),
--	"_sequenceid" serial NOT NULL,
--	"_contextkey" varchar(255) NULL,
--	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
--	"_createddate" timestamp NULL DEFAULT now(),
--	"_createdsource" varchar(255) NULL,
--	"_createdmessageid" varchar(255) NULL,
--	"_createdby" varchar(255) NULL,
--	"_recordstatus" int2 NULL DEFAULT 1,
--	"_timezonename" varchar(255) NULL,
--	"_timezoneoffset" int4 NULL,
--	"_tenant" varchar(255) NULL,
--	cd varchar(100) NULL,
--	"desc" varchar(1000) NULL
--);
--
----truncate table local_formulary.lookup_rnoh_formulary_status
--INSERT INTO local_formulary.lookup_rnoh_formulary_status
--("_contextkey", "_createdtimestamp", "_createddate", "_createdsource", "_createdmessageid", "_createdby", "_recordstatus", "_timezonename", "_timezoneoffset", "_tenant", cd, "desc")
--values
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '1', 'Formulary'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '2', 'Non-Formulary')
--;
--
--
----====================================================================
--
--
--
---- local_formulary.lookup_orderable_status definition
--
---- Drop table
--
---- DROP TABLE local_formulary.lookup_orderable_status;
--
--CREATE TABLE local_formulary.lookup_orderable_status (
--	"_row_id" varchar(255) null default uuid_generate_v4(),
--	"_sequenceid" serial NOT NULL,
--	"_contextkey" varchar(255) NULL,
--	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
--	"_createddate" timestamp NULL DEFAULT now(),
--	"_createdsource" varchar(255) NULL,
--	"_createdmessageid" varchar(255) NULL,
--	"_createdby" varchar(255) NULL,
--	"_recordstatus" int2 NULL DEFAULT 1,
--	"_timezonename" varchar(255) NULL,
--	"_timezoneoffset" int4 NULL,
--	"_tenant" varchar(255) NULL,
--	cd varchar(100) NULL,
--	"desc" varchar(1000) NULL
--);
--
----truncate table local_formulary.lookup_orderable_status
--INSERT INTO local_formulary.lookup_orderable_status
--("_contextkey", "_createdtimestamp", "_createddate", "_createdsource", "_createdmessageid", "_createdby", "_recordstatus", "_timezonename", "_timezoneoffset", "_tenant", cd, "desc")
--values
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '0', 'Not Orderable'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '1', 'Orderable')
--;
--
----====================================================================
--
---- local_formulary.lookup_orderable_status definition
--
---- Drop table
--
---- DROP TABLE local_formulary.lookup_orderable_status;
--
--CREATE TABLE local_formulary.lookup_orderable_status (
--	"_row_id" varchar(255) null default uuid_generate_v4(),
--	"_sequenceid" serial NOT NULL,
--	"_contextkey" varchar(255) NULL,
--	"_createdtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now()),
--	"_createddate" timestamp NULL DEFAULT now(),
--	"_createdsource" varchar(255) NULL,
--	"_createdmessageid" varchar(255) NULL,
--	"_createdby" varchar(255) NULL,
--	"_recordstatus" int2 NULL DEFAULT 1,
--	"_timezonename" varchar(255) NULL,
--	"_timezoneoffset" int4 NULL,
--	"_tenant" varchar(255) NULL,
--	cd varchar(100) NULL,
--	"desc" varchar(1000) NULL
--);
--
----truncate table local_formulary.lookup_orderable_status
--INSERT INTO local_formulary.lookup_orderable_status
--("_contextkey", "_createdtimestamp", "_createddate", "_createdsource", "_createdmessageid", "_createdby", "_recordstatus", "_timezonename", "_timezoneoffset", "_tenant", cd, "desc")
--values
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '0', 'Not Orderable'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '1', 'Orderable')
--;

--====================================================================


--================================================Playground=============================================

set schema 'local_formulary';

select * from local_formulary.formulary_header fh2 ;

select * from local_formulary.formulary_header fh2 
where code = '318956006';

select * from local_formulary.formulary_additional_code fac
where fac.formulary_version_id ='2826de56-2ef4-4c16-9ad9-6bbcfc3f00dd'

select * from local_formulary.formulary_ingredient fac
where fac.formulary_version_id ='2826de56-2ef4-4c16-9ad9-6bbcfc3f00dd'

select * from terminology.dmd_vmp dv where dv.vpid = '10931911000001101'
select * from terminology.dmd_vmp_ingredient dv where dv.vpid = '7821611000001105'

select vpid from terminology.dmd_vmp_ingredient dv group by dv.vpid having count(*) > 1;



select * from local_formulary.formulary_indication fac 
where fac.formulary_version_id ='2826de56-2ef4-4c16-9ad9-6bbcfc3f00dd'


select * from local_formulary.formulary_route_detail frd 
where formulary_version_id ='2826de56-2ef4-4c16-9ad9-6bbcfc3f00dd'


select * from terminology.dmd_vmp_drugroute dv where dv.vpid = '10931911000001101'


select * from local_formulary.formulary_detail  
where formulary_version_id ='2826de56-2ef4-4c16-9ad9-6bbcfc3f00dd'

select * from terminology.dmd_lookup_form 
where cd = '385223009'

select formulary_version_id ,is_latest ,code,* from local_formulary.formulary_header
where 
code = '330404003'-- '330729005' --'330286001'--'38611811000001100'
--formulary_version_id = '4fc37cc1-45cd-46a0-967b-1f63aceabf05'
order by code desc;


select * from local_formulary.formulary_detail fd 
where formulary_version_id in 
('4a0fff40-1cb6-4dd4-bd57-d9b3f42be936',
'84a7666a-2e3e-4900-b118-2a2dbd54a27a')
--('6b1985aa-a16d-46f8-9c64-8e5ce82584b6',
--'bbfc2aa9-1d00-40d8-94e5-74ed3705c14e')
--('efd26173-8be2-4139-b916-fddb93f84968','50485b6f-230f-4b77-a24b-097f8dfe8b17') 
--'479121bd-5940-4c54-b34a-79c384d8541a'

select * from local_formulary.formulary_ingredient fi
where formulary_version_id 
in 
('4a0fff40-1cb6-4dd4-bd57-d9b3f42be936',
'84a7666a-2e3e-4900-b118-2a2dbd54a27a')--('6b1985aa-a16d-46f8-9c64-8e5ce82584b6',
--'bbfc2aa9-1d00-40d8-94e5-74ed3705c14e')
--= 'efd26173-8be2-4139-b916-fddb93f84968'--'50485b6f-230f-4b77-a24b-097f8dfe8b17'--'4fc37cc1-45cd-46a0-967b-1f63aceabf05'--'479121bd-5940-4c54-b34a-79c384d8541a'



--=================================================================

set schema 'local_formulary';

ALTER TABLE local_formulary.formulary_header ADD "_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now());
ALTER TABLE local_formulary.formulary_header ADD "_updateddate" timestamp NULL DEFAULT now();
ALTER TABLE local_formulary.formulary_header ADD "_updatedby" varchar(255) NULL;

ALTER TABLE local_formulary.formulary_detail ADD "_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now());
ALTER TABLE local_formulary.formulary_detail ADD "_updateddate" timestamp NULL DEFAULT now();
ALTER TABLE local_formulary.formulary_detail ADD "_updatedby" varchar(255) NULL;

ALTER TABLE local_formulary.formulary_additional_code ADD "_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now());
ALTER TABLE local_formulary.formulary_additional_code ADD "_updateddate" timestamp NULL DEFAULT now();
ALTER TABLE local_formulary.formulary_additional_code ADD "_updatedby" varchar(255) NULL;

ALTER TABLE local_formulary.formulary_indication ADD "_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now());
ALTER TABLE local_formulary.formulary_indication ADD "_updateddate" timestamp NULL DEFAULT now();
ALTER TABLE local_formulary.formulary_indication ADD "_updatedby" varchar(255) NULL;


ALTER TABLE local_formulary.formulary_ingredient ADD "_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now());
ALTER TABLE local_formulary.formulary_ingredient ADD "_updateddate" timestamp NULL DEFAULT now();
ALTER TABLE local_formulary.formulary_ingredient ADD "_updatedby" varchar(255) NULL;

ALTER TABLE local_formulary.formulary_route_detail ADD "_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now());
ALTER TABLE local_formulary.formulary_route_detail ADD "_updateddate" timestamp NULL DEFAULT now();
ALTER TABLE local_formulary.formulary_route_detail ADD "_updatedby" varchar(255) NULL;

ALTER TABLE local_formulary.formulary_rule_config ADD "_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now());
ALTER TABLE local_formulary.formulary_rule_config ADD "_updateddate" timestamp NULL DEFAULT now();
ALTER TABLE local_formulary.formulary_rule_config ADD "_updatedby" varchar(255) NULL;

ALTER TABLE local_formulary.lookup_common ADD "_updatedtimestamp" timestamptz NULL DEFAULT timezone('UTC'::text, now());
ALTER TABLE local_formulary.lookup_common ADD "_updateddate" timestamp NULL DEFAULT now();
ALTER TABLE local_formulary.lookup_common ADD "_updatedby" varchar(255) NULL;

--==========================================

set schema 'local_formulary';

/*
 * 
 truncate table local_formulary.formulary_additional_code ;
truncate table local_formulary.formulary_detail ;
truncate table local_formulary.formulary_indication ;
truncate table local_formulary.formulary_ingredient ;
truncate table local_formulary.formulary_ontology_form ;
truncate table local_formulary.formulary_route_detail ;
truncate table local_formulary.formulary_header cascade;
truncate table local_formulary.formulary_additional_code ; 
*/

select is_latest , rec_status_code ,formulary_id ,code,formulary_version_id, is_duplicate , duplicate_of_formulary_id, * from local_formulary.formulary_header
where code = '18595211000001106'--'27658006'--'330629004'--'38377311000001100'--'330629004'
--c80a0105-bc98-4036-998d-86125616be45

select * from local_formulary.formulary_header 
--where formulary_version_id = 'd5d6de46-885d-4096-a36d-03b44594c34a';
where formulary_id  = '0ca141ee-0a11-4a5b-9439-305b8cfc199c'
--where rec_status_code  = '004'

select rnoh_formulary_statuscd, supplier_cd, trade_family_cd ,* from local_formulary.formulary_detail 
where formulary_version_id = 'd5d6de46-885d-4096-a36d-03b44594c34a';

select * from local_formulary.formulary_additional_code ;
select * from local_formulary.formulary_indication ;
select * from local_formulary.formulary_ingredient ;
select * from local_formulary.formulary_ontology_form ;
select * from local_formulary.formulary_route_detail ;
select * from local_formulary.formulary_additional_code ;


select code_system ,* from local_formulary.formulary_detail
where side_effect is not null;

select is_duplicate ,name,product_type ,formulary_version_id , code, * 
from local_formulary.formulary_header fh 
--where fh.formulary_version_id ='708dc486-8e65-49f8-8bfc-bd26b1a348de';
where fh.code = '33568211000001100'
fh.is_latest = true;

select * from local_formulary.formulary_header fh 
where fh.code = '4593311000001104'--'35901911000001104';

select fd.side_effect, fh.version_id,fh.formulary_version_id ,fh.code, * 
from local_formulary.formulary_header fh

inner join local_formulary.formulary_detail fd on fd.formulary_version_id =fh.formulary_version_id 
where fh.code = '4593311000001104'--'27658006' --'35901911000001104';

select * from local_formulary.formulary_ingredient fi 
where fi.formulary_version_id = '828b9b30-8c51-4fb8-b595-32a821e90193'
--'a3b855a7-9099-44b5-9f27-e5c59fe008d4'

select * from local_formulary.formulary_header fh where fh.rec_source = 'M';


select fluid,current_licensing_authority_cd ,dose_form_cd, trade_family_cd, * from local_formulary.formulary_detail fd 
where formulary_version_id  = '4a3a3c5f-b724-4728-8e42-d725faa6df0c'--'e13c1125-2639-4c10-93e6-c5230bf9f1d8'

select * from local_formulary.formulary_route_detail frd  
where formulary_version_id  = 'e13c1125-2639-4c10-93e6-c5230bf9f1d8'

select * from local_formulary.formulary_ingredient fi   
where formulary_version_id  = 'e13c1125-2639-4c10-93e6-c5230bf9f1d8'

select * from local_formulary.formulary_indication fi   
where formulary_version_id  = 'e13c1125-2639-4c10-93e6-c5230bf9f1d8'

5
select code,* from local_formulary.formulary_header 
--where formulary_version_id = 'd5d6de46-885d-4096-a36d-03b44594c34a';
where formulary_id  = '0ca141ee-0a11-4a5b-9439-305b8cfc199c'
--where rec_status_code  = '004'

select * from local_formulary.formulary_additional_code 
where formulary_version_id = 'ee394ccf-740b-4d75-9f39-9d66ac985945';
select * from local_formulary.formulary_indication 
where formulary_version_id = 'ee394ccf-740b-4d75-9f39-9d66ac985945';

select * from local_formulary.formulary_ingredient 
where formulary_version_id = 'ee394ccf-740b-4d75-9f39-9d66ac985945';

select * from local_formulary.formulary_ontology_form 
where formulary_version_id = 'ee394ccf-740b-4d75-9f39-9d66ac985945';

select * from local_formulary.formulary_route_detail 
where formulary_version_id = 'ee394ccf-740b-4d75-9f39-9d66ac985945';

select * from local_formulary.formulary_additional_code 
where formulary_version_id = 'ee394ccf-740b-4d75-9f39-9d66ac985945';





