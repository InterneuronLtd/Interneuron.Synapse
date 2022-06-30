-- terminology.dmd_names_lookup_all_mat source
DROP MATERIALIZED VIEW IF EXISTS terminology.dmd_names_lookup_all_mat;

CREATE MATERIALIZED VIEW terminology.dmd_names_lookup_all_mat
TABLESPACE pg_default
AS SELECT DISTINCT vtm.nm AS name,
    to_tsvector('english'::regconfig, vtm.nm::text) AS name_tokens,
    vtm.vtmid AS code,
    vtm.vtmidprev AS prevcode,
    NULL::character varying(100) AS basiscd,
    NULL::character varying(100) AS cfcf,
    NULL::character varying(100) AS gluf,
    NULL::character varying(100) AS presf,
    NULL::character varying(100) AS sugf,
    NULL::numeric AS udfs,
    NULL::character varying(100) AS udfsuomcd,
    NULL::character varying(100) AS unitdoseuomcd,
    NULL::character varying(100) AS dfindcd,
    NULL::character varying(100) AS ema,
    NULL::character varying(100) AS licauthcd,
    NULL::character varying(100) AS parallelimport,
    NULL::character varying(100) AS availrestrictcd,
    NULL::character varying(255) AS vmproute_code,
    NULL::character varying(1000) AS vmproute,
    NULL::character varying(255) AS vmpform_code,
    NULL::character varying(1000) AS vmpform,
    NULL::character varying(255) AS supplier_code,
    NULL::character varying(1000) AS supplier,
    to_tsvector('english'::regconfig, ''::text) AS supplier_name_tokens,
    NULL::bigint AS prescribing_status_code,
    NULL::character varying(1000) AS prescribing_status,
    NULL::bigint AS control_drug_category_code,
    NULL::character varying(1000) AS control_drug_category,
    NULL::character varying(255) AS ingredient_substance_id,
    NULL::bigint AS basis_pharmaceutical_strength_cd,
    NULL::character varying(255) AS basis_strength_substance_id,
    NULL::character varying(255) AS strength_value_nmtr_unit_cd,
    NULL::numeric AS strength_val_nmtr,
    NULL::character varying(255) AS strength_value_dnmtr_unit_cd,
    NULL::numeric AS strength_val_dnmtr,
    NULL::bigint AS ontcd
   FROM terminology.dmd_vtm vtm
  WHERE vtm.invalid IS NULL
UNION
 SELECT DISTINCT vmp.nm AS name,
    to_tsvector('english'::regconfig, vmp.nm::text) AS name_tokens,
    vmp.vpid AS code,
    vmp.vpidprev AS prevcode,
    vmp.basiscd::character varying(100) AS basiscd,
    vmp.cfc_f::character varying(100) AS cfcf,
    vmp.glu_f::character varying(100) AS gluf,
    vmp.pres_f::character varying(100) AS presf,
    vmp.sug_f::character varying(100) AS sugf,
    vmp.udfs,
    vmp.udfs_uomcd::character varying(100) AS udfsuomcd,
    vmp.unit_dose_uomcd::character varying(100) AS unitdoseuomcd,
    vmp.df_indcd::character varying(100) AS dfindcd,
    NULL::character varying(100) AS ema,
    NULL::character varying(100) AS licauthcd,
    NULL::character varying(100) AS parallelimport,
    NULL::character varying(100) AS availrestrictcd,
    route.cd AS vmproute_code,
    route."desc" AS vmproute,
    form.cd AS vmpform_code,
    form."desc" AS vmpform,
    NULL::character varying(255) AS supplier_code,
    NULL::character varying(100) AS supplier,
    to_tsvector('english'::regconfig, ''::text) AS supplier_name_tokens,
    pres.cd AS prescribing_status_code,
    pres."desc" AS prescribing_status,
    lcd.cd AS control_drug_category_code,
    lcd."desc" AS control_drug_category,
    ving.isid AS ingredient_substance_id,
    ving.basis_strntcd AS basis_pharmaceutical_strength_cd,
    ving.bs_subid AS basis_strength_substance_id,
    ving.strnt_nmrtr_uomcd AS strength_value_nmtr_unit_cd,
    ving.strnt_nmrtr_val AS strength_val_nmtr,
    ving.strnt_dnmtr_uomcd AS strength_value_dnmtr_unit_cd,
    ving.strnt_dnmtr_val AS strength_val_dnmtr,
    ont.cd AS ontcd
   FROM terminology.dmd_vmp vmp
     LEFT JOIN terminology.dmd_lookup_prescribingstatus pres ON pres.cd::text = vmp.pres_statcd::text
     LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid::text = vmp.vpid::text
     LEFT JOIN terminology.dmd_lookup_form form ON form.cd::text = vdf.formcd::text
     LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid::text = vmp.vpid::text
     LEFT JOIN terminology.dmd_lookup_route route ON route.cd::text = vdr.routecd::text
     LEFT JOIN terminology.dmd_vmp_controldrug vcd ON vcd.vpid::text = vmp.vpid::text
     LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd::text = vcd.catcd::text
     LEFT JOIN terminology.dmd_vmp_ingredient ving ON ving.vpid::text = vmp.vpid::text
     LEFT JOIN terminology.dmd_vmp_ontdrugform vondf ON vondf.vpid::text = vmp.vpid::text
     LEFT JOIN terminology.dmd_lookup_ontformroute ont ON ont.cd::text = vondf.formcd::text
  WHERE vmp.invalid IS NULL
UNION
 SELECT DISTINCT
        CASE
            WHEN supp."desc" IS NULL OR supp."desc"::text = ''::text THEN amp.nm::text
            ELSE ((amp.nm::text || ' ('::text) || supp."desc"::text) || ')'::text
        END AS name,
        CASE
            WHEN supp."desc" IS NULL OR supp."desc"::text = ''::text THEN to_tsvector('english'::regconfig, amp.nm::text)
            ELSE to_tsvector('english'::regconfig, ((amp.nm::text || ' ('::text) || supp."desc"::text) || ')'::text)
        END AS name_tokens,
    amp.apid AS code,
    NULL::character varying(255) AS prevcode,
    NULL::character varying(100) AS basiscd,
    NULL::character varying(100) AS cfcf,
    NULL::character varying(100) AS gluf,
    NULL::character varying(100) AS presf,
    NULL::character varying(100) AS sugf,
    NULL::numeric AS udfs,
    NULL::character varying(100) AS udfsuomcd,
    NULL::character varying(100) AS unitdoseuomcd,
    NULL::character varying(100) AS dfindcd,
    amp.ema::character varying(100) AS ema,
    amp.lic_authcd::character varying(100) AS licauthcd,
    amp.parallel_import::character varying(100) AS parallelimport,
    amp.avail_restrictcd::character varying(100) AS availrestrictcd,
    NULL::character varying(255) AS vmproute_code,
    NULL::character varying(1000) AS vmproute,
    NULL::character varying(255) AS vmpform_code,
    NULL::character varying(1000) AS vmpform,
    supp.cd AS supplier_code,
    supp."desc" AS supplier,
    to_tsvector('english'::regconfig, supp."desc"::text) AS supplier_name_tokens,
    NULL::bigint AS prescribing_status_code,
    NULL::character varying(1000) AS prescribing_status,
    NULL::bigint AS control_drug_category_code,
    NULL::character varying(1000) AS control_drug_category,
    NULL::character varying(255) AS ingredient_substance_id,
    NULL::bigint AS basis_pharmaceutical_strength_cd,
    NULL::character varying(255) AS basis_strength_substance_id,
    NULL::character varying(255) AS strength_value_nmtr_unit_cd,
    NULL::numeric AS strength_val_nmtr,
    NULL::character varying(255) AS strength_value_dnmtr_unit_cd,
    NULL::numeric AS strength_val_dnmtr,
    NULL::bigint AS ontcd
   FROM terminology.dmd_amp amp
     LEFT JOIN terminology.dmd_lookup_supplier supp ON supp.cd::text = amp.suppcd::text
  WHERE amp.invalid IS NULL
  ORDER BY 1
WITH DATA;

-- View indexes:
CREATE INDEX dmd_names_lookup_all_mat_code_hash_idx ON terminology.dmd_names_lookup_all_mat USING hash (code);
CREATE INDEX dmd_names_lookup_all_mat_code_idx ON terminology.dmd_names_lookup_all_mat USING btree (code);
CREATE INDEX dmd_names_lookup_all_mat_control_drug_category_code_idx ON terminology.dmd_names_lookup_all_mat USING btree (control_drug_category_code);
CREATE INDEX dmd_names_lookup_all_mat_control_drug_category_idx ON terminology.dmd_names_lookup_all_mat USING btree (control_drug_category);
CREATE INDEX dmd_names_lookup_all_mat_name_tokens_idx ON terminology.dmd_names_lookup_all_mat USING gin (name_tokens);
CREATE INDEX dmd_names_lookup_all_mat_prescribing_status_code_idx ON terminology.dmd_names_lookup_all_mat USING btree (prescribing_status_code);
CREATE INDEX dmd_names_lookup_all_mat_prescribing_status_idx ON terminology.dmd_names_lookup_all_mat USING btree (prescribing_status);
CREATE INDEX dmd_names_lookup_all_mat_supplier_code_idx ON terminology.dmd_names_lookup_all_mat USING btree (supplier_code);
CREATE INDEX dmd_names_lookup_all_mat_supplier_idx ON terminology.dmd_names_lookup_all_mat USING btree (supplier);
CREATE INDEX dmd_names_lookup_all_mat_supplier_name_tokens_idx ON terminology.dmd_names_lookup_all_mat USING gin (supplier_name_tokens);
CREATE INDEX dmd_names_lookup_all_mat_vmpform_code_idx ON terminology.dmd_names_lookup_all_mat USING btree (vmpform_code);
CREATE INDEX dmd_names_lookup_all_mat_vmpform_idx ON terminology.dmd_names_lookup_all_mat USING btree (vmpform);
CREATE INDEX dmd_names_lookup_all_mat_vmproute_code_idx ON terminology.dmd_names_lookup_all_mat USING btree (vmproute_code);
CREATE INDEX dmd_names_lookup_all_mat_vmproute_idx ON terminology.dmd_names_lookup_all_mat USING btree (vmproute);


-- terminology.dmd_names_lookup_mat source
DROP MATERIALIZED VIEW IF EXISTS terminology.dmd_names_lookup_mat;
CREATE MATERIALIZED VIEW terminology.dmd_names_lookup_mat
TABLESPACE pg_default
AS SELECT DISTINCT vtm.nm AS name,
    to_tsvector('english'::regconfig, vtm.nm::text) AS name_tokens,
    vtm.vtmid AS code,
    NULL::character varying(255) AS vmproute_code,
    NULL::character varying(1000) AS vmproute,
    NULL::character varying(255) AS vmpform_code,
    NULL::character varying(1000) AS vmpform,
    NULL::character varying(255) AS supplier_code,
    NULL::character varying(1000) AS supplier,
    to_tsvector('english'::regconfig, ''::text) AS supplier_name_tokens,
    NULL::bigint AS prescribing_status_code,
    NULL::character varying(1000) AS prescribing_status,
    NULL::bigint AS control_drug_category_code,
    NULL::character varying(1000) AS control_drug_category
   FROM terminology.dmd_vtm vtm
  WHERE vtm.invalid IS NULL
UNION
 SELECT DISTINCT vmp.nm AS name,
    to_tsvector('english'::regconfig, vmp.nm::text) AS name_tokens,
    vmp.vpid AS code,
    route.cd AS vmproute_code,
    route."desc" AS vmproute,
    form.cd AS vmpform_code,
    form."desc" AS vmpform,
    NULL::character varying(255) AS supplier_code,
    NULL::character varying(1000) AS supplier,
    to_tsvector('english'::regconfig, ''::text) AS supplier_name_tokens,
    pres.cd AS prescribing_status_code,
    pres."desc" AS prescribing_status,
    lcd.cd AS control_drug_category_code,
    lcd."desc" AS control_drug_category
   FROM terminology.dmd_vmp vmp
     LEFT JOIN terminology.dmd_lookup_prescribingstatus pres ON pres.cd::text = vmp.pres_statcd::text
     LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid::text = vmp.vpid::text
     LEFT JOIN terminology.dmd_lookup_form form ON form.cd::text = vdf.formcd::text
     LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid::text = vmp.vpid::text
     LEFT JOIN terminology.dmd_lookup_route route ON route.cd::text = vdr.routecd::text
     LEFT JOIN terminology.dmd_vmp_controldrug vcd ON vcd.vpid::text = vmp.vpid::text
     LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd::text = vcd.catcd::text
  WHERE vmp.invalid IS NULL
UNION
 SELECT DISTINCT
        CASE
            WHEN supp."desc" IS NULL OR supp."desc"::text = ''::text THEN amp.nm::text
            ELSE ((amp.nm::text || ' ('::text) || supp."desc"::text) || ')'::text
        END AS name,
        CASE
            WHEN supp."desc" IS NULL OR supp."desc"::text = ''::text THEN to_tsvector('english'::regconfig, amp.nm::text)
            ELSE to_tsvector('english'::regconfig, ((amp.nm::text || ' ('::text) || supp."desc"::text) || ')'::text)
        END AS name_tokens,
    amp.apid AS code,
    NULL::character varying(255) AS vmproute_code,
    NULL::character varying(1000) AS vmproute,
    NULL::character varying(255) AS vmpform_code,
    NULL::character varying(1000) AS vmpform,
    supp.cd AS supplier_code,
    supp."desc" AS supplier,
    to_tsvector('english'::regconfig, supp."desc"::text) AS supplier_name_tokens,
    NULL::bigint AS prescribing_status_code,
    NULL::character varying(1000) AS prescribing_status,
    NULL::bigint AS control_drug_category_code,
    NULL::character varying(1000) AS control_drug_category
   FROM terminology.dmd_amp amp
     LEFT JOIN terminology.dmd_lookup_supplier supp ON supp.cd::text = amp.suppcd::text
  WHERE amp.invalid IS NULL
WITH DATA;

-- View indexes:
CREATE INDEX dmd_names_lookup_mat_code_hash_idx ON terminology.dmd_names_lookup_mat USING hash (code);
CREATE INDEX dmd_names_lookup_mat_code_idx ON terminology.dmd_names_lookup_mat USING btree (code);
CREATE INDEX dmd_names_lookup_mat_control_drug_category_code_idx ON terminology.dmd_names_lookup_mat USING btree (control_drug_category_code);
CREATE INDEX dmd_names_lookup_mat_control_drug_category_idx ON terminology.dmd_names_lookup_mat USING btree (control_drug_category);
CREATE INDEX dmd_names_lookup_mat_name_tokens_idx ON terminology.dmd_names_lookup_mat USING gin (name_tokens);
CREATE INDEX dmd_names_lookup_mat_prescribing_status_code_idx ON terminology.dmd_names_lookup_mat USING btree (prescribing_status_code);
CREATE INDEX dmd_names_lookup_mat_prescribing_status_idx ON terminology.dmd_names_lookup_mat USING btree (prescribing_status);
CREATE INDEX dmd_names_lookup_mat_supplier_code_idx ON terminology.dmd_names_lookup_mat USING btree (supplier_code);
CREATE INDEX dmd_names_lookup_mat_supplier_idx ON terminology.dmd_names_lookup_mat USING btree (supplier);
CREATE INDEX dmd_names_lookup_mat_supplier_name_tokens_idx ON terminology.dmd_names_lookup_mat USING gin (supplier_name_tokens);
CREATE INDEX dmd_names_lookup_mat_vmpform_code_idx ON terminology.dmd_names_lookup_mat USING btree (vmpform_code);
CREATE INDEX dmd_names_lookup_mat_vmpform_idx ON terminology.dmd_names_lookup_mat USING btree (vmpform);
CREATE INDEX dmd_names_lookup_mat_vmproute_code_idx ON terminology.dmd_names_lookup_mat USING btree (vmproute_code);
CREATE INDEX dmd_names_lookup_mat_vmproute_idx ON terminology.dmd_names_lookup_mat USING btree (vmproute);


-- terminology.dmd_relationships_mat source
DROP MATERIALIZED VIEW IF EXISTS terminology.dmd_relationships_mat;

CREATE MATERIALIZED VIEW terminology.dmd_relationships_mat
TABLESPACE pg_default
AS SELECT DISTINCT vtm.vtmid AS code,
    'VTM'::text AS level,
    1 AS logical_level,
    ''::text AS parent_code,
    ''::text AS parent_level,
    0 AS parent_logical_level
   FROM terminology.dmd_vtm vtm
UNION
 SELECT DISTINCT vmp.vpid AS code,
    'VMP'::text AS level,
    2 AS logical_level,
    vmp.vtmid AS parent_code,
    'VTM'::text AS parent_level,
    1 AS parent_logical_level
   FROM terminology.dmd_vmp vmp
UNION
 SELECT DISTINCT amp.apid AS code,
    'AMP'::text AS level,
    3 AS logical_level,
    amp.vpid AS parent_code,
    'VMP'::text AS parent_level,
    2 AS parent_logical_level
   FROM terminology.dmd_amp amp
  ORDER BY 3
WITH DATA;

-- View indexes:
CREATE INDEX dmd_relationships_mat_code_hash_idx ON terminology.dmd_relationships_mat USING hash (code);
CREATE INDEX dmd_relationships_mat_level_idx ON terminology.dmd_relationships_mat USING btree (level);
CREATE INDEX dmd_relationships_mat_logical_level_idx ON terminology.dmd_relationships_mat USING hash (logical_level);
CREATE INDEX dmd_relationships_mat_parent_code_code_idx ON terminology.dmd_relationships_mat USING btree (parent_code) INCLUDE (code);
CREATE INDEX dmd_relationships_mat_parent_code_hash_idx ON terminology.dmd_relationships_mat USING hash (parent_code);
CREATE INDEX dmd_relationships_mat_parent_level_idx ON terminology.dmd_relationships_mat USING btree (parent_level);
CREATE INDEX dmd_relationships_mat_parent_logical_level_idx ON terminology.dmd_relationships_mat USING hash (parent_logical_level);
CREATE UNIQUE INDEX dmd_relationships_mat_unq_code_parent_code__idx ON terminology.dmd_relationships_mat USING btree (code, parent_code);