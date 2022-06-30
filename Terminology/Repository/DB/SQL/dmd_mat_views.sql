CREATE MATERIALIZED VIEW terminology.mv_dmd_productlist
TABLESPACE pg_default
AS SELECT vtm.nm AS name,
    vtm.vtmid::character varying(255) AS code,
    'VTM'::text AS level,
    ''::character varying AS parent_code,
    ''::text AS parent_level,
    ''::character varying AS route,
    ''::character varying AS form
   FROM terminology.dmd_vtm vtm
UNION
 SELECT vmp.nm AS name,
    vmp.vpid::character varying(255) AS code,
    'VMP'::text AS level,
    vmp.vtmid::character varying(255) AS parent_code,
    'VTM'::text AS parent_level,
    route."desc" AS route,
    form."desc" AS form
   FROM terminology.dmd_vmp vmp
     LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
     LEFT JOIN terminology.dmd_lookup_form form ON form.cd = vdf.formcd
     LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
     LEFT JOIN terminology.dmd_lookup_route route ON route.cd = vdr.routecd
UNION
 SELECT amp.nm AS name,
    amp.apid::character varying(255) AS code,
    'AMP'::text AS level,
    amp.vpid::character varying(255) AS parent_code,
    'VMP'::text AS parent_level,
    ''::character varying AS route,
    ''::character varying AS form
   FROM terminology.dmd_amp amp
  ORDER BY 1
WITH DATA;
 
 
 
 
 
--===================================================
 
-- hierachy
SELECT 
vtm.vtmid::varchar(255) as vtmid,
vtm.nm as vtmname,
vmp.vpid::varchar(255) as vmpid,
vmp.nm as vmpname,
form.desc as vmpform,
route.desc as vmproute,
amp.apid::varchar(255) as ampid,
amp.nm as ampname
FROM terminology.dmd_vtm vtm
LEFT JOIN terminology.dmd_vmp vmp ON vmp.vtmid = vtm.vtmid
LEFT JOIN terminology.dmd_amp amp ON amp.vpid = vmp.vpid
LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
       JOIN terminology.dmd_lookup_form form on form.cd = vdf.formcd
LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
       JOIN terminology.dmd_lookup_route route on route.cd = vdr.routecd
WHERE vtm.invalid is null and vmp.invalid is null and amp.invalid is null
ORDER BY vtmname, vmpname, ampname 
 
 
-- individual products.
SELECT 
vtm.nm as "name",
vtm.vtmid::varchar(255) as code,
'VTM' as "level",
1 as logical_level,
'' as parent_code,
'' as parent_level,
'' as route,
'' as form
FROM terminology.dmd_vtm vtm
UNION 
SELECT 
vmp.nm as "name",
vmp.vpid::varchar(255) as code,
'VMP' as "level",
2 as logical_level,
vmp.vtmid::varchar(255) as parent_code,
'VTM' as parent_level,
route.desc as vmproute,
form.desc as vmpform
FROM terminology.dmd_vmp vmp
LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
       LEFT JOIN terminology.dmd_lookup_form form on form.cd = vdf.formcd
LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
       LEFT JOIN terminology.dmd_lookup_route route on route.cd = vdr.routecd
--LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd = vmp.vpid
UNION
SELECT 
amp.nm as "name",
amp.apid::varchar(255) as code,
'AMP' as "level",
3 as logical_level,
amp.vpid::varchar(255) as parent_code,
'VMP' as parent_level,
'' as route,
'' as form
FROM terminology.dmd_amp amp
ORDER BY logical_level, "name" asc
 
--=========================================START===================================================
--old one
--drop materialized VIEW terminology.dmd_names_lookup_mat;
--CREATE materialized VIEW terminology.dmd_names_lookup_mat AS
--SELECT DISTINCT
--vtm.nm as "name",
--to_tsvector('english',vtm.nm) name_tokens,
--vtm.vtmid::varchar(255) as code,
--'' as route,
--'' as form
--FROM terminology.dmd_vtm vtm
--UNION 
--SELECT DISTINCT
--vmp.nm as "name",
--to_tsvector('english',vmp.nm) name_tokens,
--vmp.vpid::varchar(255) as code,
--route.desc as vmproute,
--form.desc as vmpform
--FROM terminology.dmd_vmp vmp
--LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
--     LEFT JOIN terminology.dmd_lookup_form form on form.cd = vdf.formcd
--LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
--     LEFT JOIN terminology.dmd_lookup_route route on route.cd = vdr.routecd
----LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd = vmp.vpid
--UNION
--SELECT DISTINCT
--amp.nm as "name",
--to_tsvector('english',amp.nm) name_tokens,
--amp.apid::varchar(255) as code,
--'' as route,
--'' as form
--FROM terminology.dmd_amp amp
--ORDER BY "name" asc
--WITH NO DATA;
 
 set schema 'terminology';


--drop materialized VIEW terminology.dmd_names_lookup_mat;
--CREATE materialized VIEW terminology.dmd_names_lookup_mat 
--AS
--SELECT DISTINCT
--vtm.nm as "name",
--to_tsvector('english',vtm.nm) name_tokens,
--vtm.vtmid::varchar(255) as code,
--cast(null as bigint) as vmproute_code,
--cast(null as varchar(1000)) as vmproute,
--cast(null as bigint) as vmpform_code,
--cast(null as varchar(1000)) as vmpform,
--cast(null as bigint) as supplier_code,
--cast(null as varchar(1000)) as supplier,
--to_tsvector('english','') as supplier_name_tokens,
--cast(null as bigint) as prescribing_status_code,
--cast(null as varchar(1000)) as prescribing_status,
--cast(null as bigint) as control_drug_category_code,
--cast(null as varchar(1000)) as control_drug_category
--FROM terminology.dmd_vtm vtm
--where vtm.invalid is null
--UNION 
--SELECT DISTINCT
--vmp.nm as "name",
--to_tsvector('english',vmp.nm) name_tokens,
--vmp.vpid::varchar(255) as code,
--route.cd as vmproute_code,
--route.desc as vmproute,
--form.cd as vmpform_code,
--form.desc as vmpform,
--cast(null as bigint) as supplier_code,
--cast(null as varchar(1000)) as supplier,
--to_tsvector('english','') as supplier_name_tokens,
--pres.cd as prescribing_status_code,
--pres."desc" as prescribing_status,
--lcd.cd as control_drug_category_code,
--lcd."desc" as control_drug_category
--FROM terminology.dmd_vmp vmp
--left outer join terminology.dmd_lookup_prescribingstatus pres on pres.cd = vmp.pres_statcd
--LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
--       LEFT JOIN terminology.dmd_lookup_form form on form.cd = vdf.formcd
--LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
--       LEFT JOIN terminology.dmd_lookup_route route on route.cd = vdr.routecd
--LEFT JOIN terminology.dmd_vmp_controldrug vcd ON vcd.vpid = vmp.vpid
--       LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd = vcd.catcd 
--where vmp.invalid is null
--UNION
--SELECT DISTINCT
--CASE WHEN supp."desc" IS NULL or supp."desc" = ''
--            THEN amp.nm 
--            ELSE amp.nm || ' ('  || supp."desc" || ')'
--    END AS "name",
--CASE WHEN supp."desc" IS NULL or supp."desc" = ''
--            THEN to_tsvector('english',amp.nm) 
--            ELSE to_tsvector('english',amp.nm || ' ('  || supp."desc" || ')')
--    END AS "name_tokens",
--amp.apid::varchar(255) as code,
--cast(null as bigint) as vmproute_code,
--cast(null as varchar(1000)) as vmproute,
--cast(null as bigint) as vmpform_code,
--cast(null as varchar(1000)) as vmpform,
--supp.cd as supplier_code,
--supp."desc" as supplier,
--to_tsvector('english',supp."desc") supplier_name_tokens,
--cast(null as bigint) as prescribing_status_code,
--cast(null as varchar(1000)) as prescribing_status,
--cast(null as bigint) as control_drug_category_code,
--cast(null as varchar(1000)) as control_drug_category
--FROM terminology.dmd_amp amp
--LEFT OUTER JOIN dmd_lookup_supplier supp on supp.cd = amp.suppcd
--where amp.invalid is null
----ORDER BY "name" asc
--WITH NO DATA;
-- 
-- 
--CREATE INDEX dmd_names_lookup_mat_code_hash_idx ON terminology.dmd_names_lookup_mat using hash(code);
-- 
----CREATE INDEX dmd_names_lookup_mat_name_idx ON terminology.dmd_names_lookup_mat (name);
-- 
--CREATE INDEX dmd_names_lookup_mat_code_idx ON terminology.dmd_names_lookup_mat (code);
-- 
--CREATE INDEX dmd_names_lookup_mat_vmproute_code_idx ON terminology.dmd_names_lookup_mat (vmproute_code);
--CREATE INDEX dmd_names_lookup_mat_vmproute_idx ON terminology.dmd_names_lookup_mat (vmproute);
-- 
--CREATE INDEX dmd_names_lookup_mat_vmpform_code_idx ON terminology.dmd_names_lookup_mat (vmpform_code);
--CREATE INDEX dmd_names_lookup_mat_vmpform_idx ON terminology.dmd_names_lookup_mat (vmpform);
-- 
--CREATE INDEX dmd_names_lookup_mat_supplier_code_idx ON terminology.dmd_names_lookup_mat (supplier_code);
--CREATE INDEX dmd_names_lookup_mat_supplier_idx ON terminology.dmd_names_lookup_mat (supplier);
-- 
--CREATE INDEX dmd_names_lookup_mat_control_drug_category_code_idx ON terminology.dmd_names_lookup_mat (control_drug_category_code);
--CREATE INDEX dmd_names_lookup_mat_control_drug_category_idx ON terminology.dmd_names_lookup_mat (control_drug_category);
-- 
--CREATE INDEX dmd_names_lookup_mat_prescribing_status_code_idx ON terminology.dmd_names_lookup_mat (prescribing_status_code);
--CREATE INDEX dmd_names_lookup_mat_prescribing_status_idx ON terminology.dmd_names_lookup_mat (prescribing_status);
-- 
-- 
--CREATE INDEX dmd_names_lookup_mat_name_tokens_idx ON terminology.dmd_names_lookup_mat using gin(name_tokens);
--CREATE INDEX dmd_names_lookup_mat_supplier_name_tokens_idx ON terminology.dmd_names_lookup_mat using gin(supplier_name_tokens);
 
-- terminology.dmd_names_lookup_mat source

CREATE MATERIALIZED VIEW terminology.dmd_names_lookup_mat
TABLESPACE pg_default
AS SELECT DISTINCT vtm.nm AS name,
    to_tsvector('english'::regconfig, vtm.nm::text) AS name_tokens,
    vtm.vtmid::character varying(255) AS code,
    NULL::character varying(255) AS vmproute_code,
    NULL::character varying(1000) AS vmproute,
    NULL::character varying(255) AS vmpform_code,
    NULL::character varying(1000) AS vmpform,
    NULL::character varying(255) AS supplier_code,
    NULL::character varying(1000) AS supplier,
    to_tsvector('english'::regconfig, ''::text) AS supplier_name_tokens,
    NULL::BIGINT AS prescribing_status_code,
    NULL::character varying(1000) AS prescribing_status,
    NULL::BIGINT AS control_drug_category_code,
    NULL::character varying(1000) AS control_drug_category
   FROM terminology.dmd_vtm vtm
  WHERE vtm.invalid IS NULL
UNION
 SELECT DISTINCT vmp.nm AS name,
    to_tsvector('english'::regconfig, vmp.nm::text) AS name_tokens,
    vmp.vpid::character varying(255) AS code,
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
     LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
     LEFT JOIN terminology.dmd_lookup_form form ON form.cd = vdf.formcd
     LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
     LEFT JOIN terminology.dmd_lookup_route route ON route.cd = vdr.routecd
     LEFT JOIN terminology.dmd_vmp_controldrug vcd ON vcd.vpid = vmp.vpid
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
    amp.apid::character varying(255) AS code,
    NULL::character varying(255) AS vmproute_code,
    NULL::character varying(1000) AS vmproute,
    NULL::character varying(255) AS vmpform_code,
    NULL::character varying(1000) AS vmpform,
    supp.cd AS supplier_code,
    supp."desc" AS supplier,
    to_tsvector('english'::regconfig, supp."desc"::text) AS supplier_name_tokens,
    NULL::BIGINT AS prescribing_status_code,
    NULL::character varying(1000) AS prescribing_status,
    NULL::BIGINT AS control_drug_category_code,
    NULL::character varying(1000) AS control_drug_category
   FROM terminology.dmd_amp amp
     LEFT JOIN terminology.dmd_lookup_supplier supp ON supp.cd = amp.suppcd
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
 
REFRESH MATERIALIZED VIEW terminology.dmd_names_lookup_mat;
 
--166550--164943
select count(*) from terminology.dmd_names_lookup_mat;
--======================================================================
set schema 'terminology';

--drop materialized VIEW terminology.dmd_names_lookup_all_mat;
--CREATE materialized VIEW terminology.dmd_names_lookup_all_mat 
--AS
--SELECT DISTINCT
--vtm.nm as "name",
--to_tsvector('english',vtm.nm) name_tokens,
--vtm.vtmid::varchar(255) as code,
--null ::varchar(100) as basiscd,
--null ::varchar(100) as cfcf,
--null ::varchar(100) as gluf,
--null ::varchar(100) as presf ,
--null ::varchar(100) as sugf,
--cast(null as numeric) as udfs,
--null ::varchar(100) as udfsuomcd,
--null ::varchar(100) as unitdoseuomcd,
--null ::varchar(100) as dfindcd,
--null ::varchar(100) as ema, --amp (ema)
--null ::varchar(100) as licauthcd, --amp (lic_authcd)
--null ::varchar(100) as parallelimport, --amp (parallel_import)
--null ::varchar(100) as availrestrictcd,
--cast(null as bigint) as vmproute_code,
--cast(null as varchar(1000)) as vmproute,
--cast(null as bigint) as vmpform_code,
--cast(null as varchar(1000)) as vmpform,
--cast(null as bigint) as supplier_code,
--cast(null as varchar(1000)) as supplier,
--to_tsvector('english','') as supplier_name_tokens,
--cast(null as bigint) as prescribing_status_code,
--cast(null as varchar(1000)) as prescribing_status,
--cast(null as bigint) as control_drug_category_code,
--cast(null as varchar(1000)) as control_drug_category,
--cast(null as bigint) as ingredient_substance_id,
--cast(null as int4) as basis_pharmaceutical_strength_cd,
--cast(null as int8) as basis_strength_substance_id,
--cast(null as int8) as strength_value_nmtr_unit_cd,
--cast(null as numeric) as strength_val_nmtr,
--cast(null as int8) as strength_value_dnmtr_unit_cd,
--cast(null as numeric) as strength_val_dnmtr,
--cast(null as varchar(1000)) as ontcd
--FROM terminology.dmd_vtm vtm
--where vtm.invalid is null
--UNION 
--SELECT DISTINCT
--vmp.nm as "name",
--to_tsvector('english',vmp.nm) name_tokens,
--vmp.vpid::varchar(255) as code,
--vmp.basiscd ::varchar(100) as basiscd,
--vmp.cfc_f::varchar(100) as cfcf,
--vmp.glu_f::varchar(100) as gluf,
--vmp.pres_f::varchar(100) as presf ,
--vmp.sug_f::varchar(100) as sugf,
--vmp.udfs as udfs,
--vmp.udfs_uomcd::varchar(100) as udfsuomcd,
--vmp.unit_dose_uomcd::varchar(100) as unitdoseuomcd,
--vmp.df_indcd::varchar(100) as dfindcd,
--null ::varchar(100) as ema, --amp (ema)
--null ::varchar(100) as licauthcd, --amp (lic_authcd)
--null ::varchar(100) as parallelimport, --amp (parallel_import)
--null ::varchar(100) as availrestrictcd,
--route.cd as vmproute_code,
--route.desc as vmproute,
--form.cd as vmpform_code,
--form.desc as vmpform,
--cast(null as bigint) as supplier_code,
--null ::varchar(100) as supplier,
--to_tsvector('english','') as supplier_name_tokens,
--pres.cd as prescribing_status_code,
--pres."desc" as prescribing_status,
--lcd.cd as control_drug_category_code,
--lcd."desc" as control_drug_category,
--ving.isid as ingredient_substance_id,
--ving.basis_strntcd as basis_pharmaceutical_strength_cd,
--ving.bs_subid as basis_strength_substance_id,
--ving.strnt_nmrtr_uomcd as strength_value_nmtr_unit_cd,
--ving.strnt_nmrtr_val as strength_val_nmtr,
--ving.strnt_dnmtr_uomcd as strength_value_dnmtr_unit_cd,
--ving.strnt_dnmtr_val as strength_val_dnmtr,
--ont.cd as ontcd
--FROM terminology.dmd_vmp vmp
--left outer join terminology.dmd_lookup_prescribingstatus pres on pres.cd = vmp.pres_statcd
--LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
--       LEFT JOIN terminology.dmd_lookup_form form on form.cd = vdf.formcd
--LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
--       LEFT JOIN terminology.dmd_lookup_route route on route.cd = vdr.routecd
--LEFT JOIN terminology.dmd_vmp_controldrug vcd ON vcd.vpid = vmp.vpid
--       LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd = vcd.catcd 
--left outer join terminology.dmd_vmp_ingredient ving on ving.vpid = vmp.vpid
--		LEFT JOIN terminology.dmd_vmp_ontdrugform vondf ON vondf.vpid = vmp.vpid 
--LEFT JOIN terminology.dmd_lookup_ontformroute ont ON ont.cd = vondf.formcd 
--where vmp.invalid is null
--
--UNION
--SELECT DISTINCT
--CASE WHEN supp."desc" IS NULL or supp."desc" = ''
--            THEN amp.nm 
--            ELSE amp.nm || ' ('  || supp."desc" || ')'
--    END AS "name",
--CASE WHEN supp."desc" IS NULL or supp."desc" = ''
--            THEN to_tsvector('english',amp.nm) 
--            ELSE to_tsvector('english',amp.nm || ' ('  || supp."desc" || ')')
--    END AS "name_tokens",
--amp.apid::varchar(255) as code,
--null ::varchar(100) as basiscd,
--null ::varchar(100) as cfcf,
--null ::varchar(100) as gluf,
--null ::varchar(100) as presf ,
--null ::varchar(100) as sugf,
--cast(null as numeric) as udfs,
--null ::varchar(100) as udfsuomcd,
--null ::varchar(100) as unitdoseuomcd,
--null ::varchar(100) as dfindcd,
--amp.ema::varchar(100) as ema, --amp (ema)
--amp.lic_authcd::varchar(100) as licauthcd, --amp (lic_authcd)
--amp.parallel_import::varchar(100) as parallelimport, --amp (parallel_import)
--amp.avail_restrictcd ::varchar(100) as availrestrictcd,
--cast(null as bigint) as vmproute_code,
--null ::varchar(1000) as vmproute,
--cast(null as bigint) as vmpform_code,
--null ::varchar(1000) as vmpform,
--supp.cd as supplier_code,
--supp."desc" as supplier,
--to_tsvector('english',supp."desc") supplier_name_tokens,
--cast(null as bigint) as prescribing_status_code,
--null ::varchar(1000) as prescribing_status,
--cast(null as bigint) as control_drug_category_code,
--null ::varchar(1000) as control_drug_category,
--cast(null as bigint) as ingredient_substance_id,
--cast(null as int4) as basis_pharmaceutical_strength_cd,
--cast(null as int8) as basis_strength_substance_id,
--cast(null as int8) as strength_value_nmtr_unit_cd,
--cast(null as numeric) as strength_val_nmtr,
--cast(null as int8) as strength_value_dnmtr_unit_cd,
--cast(null as numeric) as strength_val_dnmtr,
--null ::varchar(1000) as ontcd
--FROM terminology.dmd_amp amp
--LEFT OUTER JOIN dmd_lookup_supplier supp on supp.cd = amp.suppcd
--where amp.invalid is null
--
--ORDER BY "name" asc
--WITH NO DATA;
-- 
-- 
--CREATE INDEX dmd_names_lookup_all_mat_code_hash_idx ON terminology.dmd_names_lookup_all_mat using hash(code);
-- 
----CREATE INDEX dmd_names_lookup_mat_name_idx ON terminology.dmd_names_lookup_mat (name);
-- 
--CREATE INDEX dmd_names_lookup_all_mat_code_idx ON terminology.dmd_names_lookup_all_mat (code);
-- 
--CREATE INDEX dmd_names_lookup_all_mat_vmproute_code_idx ON terminology.dmd_names_lookup_all_mat (vmproute_code);
--CREATE INDEX dmd_names_lookup_all_mat_vmproute_idx ON terminology.dmd_names_lookup_all_mat (vmproute);
-- 
--CREATE INDEX dmd_names_lookup_all_mat_vmpform_code_idx ON terminology.dmd_names_lookup_all_mat (vmpform_code);
--CREATE INDEX dmd_names_lookup_all_mat_vmpform_idx ON terminology.dmd_names_lookup_all_mat (vmpform);
-- 
--CREATE INDEX dmd_names_lookup_all_mat_supplier_code_idx ON terminology.dmd_names_lookup_all_mat (supplier_code);
--CREATE INDEX dmd_names_lookup_all_mat_supplier_idx ON terminology.dmd_names_lookup_all_mat (supplier);
-- 
--CREATE INDEX dmd_names_lookup_all_mat_control_drug_category_code_idx ON terminology.dmd_names_lookup_all_mat (control_drug_category_code);
--CREATE INDEX dmd_names_lookup_all_mat_control_drug_category_idx ON terminology.dmd_names_lookup_all_mat (control_drug_category);
-- 
--CREATE INDEX dmd_names_lookup_all_mat_prescribing_status_code_idx ON terminology.dmd_names_lookup_all_mat (prescribing_status_code);
--CREATE INDEX dmd_names_lookup_all_mat_prescribing_status_idx ON terminology.dmd_names_lookup_all_mat (prescribing_status);
-- 
-- 
--CREATE INDEX dmd_names_lookup_all_mat_name_tokens_idx ON terminology.dmd_names_lookup_all_mat using gin(name_tokens);
--CREATE INDEX dmd_names_lookup_all_mat_supplier_name_tokens_idx ON terminology.dmd_names_lookup_all_mat using gin(supplier_name_tokens);
-- 
-- 
--REFRESH MATERIALIZED VIEW terminology.dmd_names_lookup_all_mat;

-- terminology.dmd_names_lookup_all_mat source

CREATE MATERIALIZED VIEW terminology.dmd_names_lookup_all_mat
TABLESPACE pg_default
AS SELECT DISTINCT vtm.nm AS name,
    to_tsvector('english'::regconfig, vtm.nm::text) AS name_tokens,
    vtm.vtmid::character varying(255) AS code,
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
    NULL::character varying(255)  AS basis_strength_substance_id,
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
    vmp.vpid::character varying(255) AS code,
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
     LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
     LEFT JOIN terminology.dmd_lookup_form form ON form.cd = vdf.formcd
     LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
     LEFT JOIN terminology.dmd_lookup_route route ON route.cd = vdr.routecd
     LEFT JOIN terminology.dmd_vmp_controldrug vcd ON vcd.vpid = vmp.vpid
     LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd::text = vcd.catcd::text
     LEFT JOIN terminology.dmd_vmp_ingredient ving ON ving.vpid = vmp.vpid
     LEFT JOIN terminology.dmd_vmp_ontdrugform vondf ON vondf.vpid = vmp.vpid
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
    amp.apid::character varying(255) AS code,
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
    NULL::character varying(255)  AS basis_strength_substance_id,
    NULL::character varying(255) AS strength_value_nmtr_unit_cd,
    NULL::numeric AS strength_val_nmtr,
    NULL::character varying(255) AS strength_value_dnmtr_unit_cd,
    NULL::numeric AS strength_val_dnmtr,
    NULL::bigint AS ontcd
   FROM terminology.dmd_amp amp
     LEFT JOIN terminology.dmd_lookup_supplier supp ON supp.cd = amp.suppcd
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
 

REFRESH MATERIALIZED VIEW terminology.dmd_names_lookup_all_mat;

--173903--170532--172450
select count(*) from terminology.dmd_names_lookup_all_mat;

select * from terminology.dmd_names_lookup_all_mat where control_drug_category_code is not null;

--======================================================================
 
set schema 'terminology';
 
--drop materialized VIEW terminology.dmd_relationships_mat;
--CREATE materialized VIEW terminology.dmd_relationships_mat 
--AS
--SELECT DISTINCT
--vtm.vtmid::varchar(255) as code,
--'VTM' as "level",
--1 as logical_level,
--'' as parent_code,
--'' as parent_level,
--0 as parent_logical_level
--FROM terminology.dmd_vtm vtm
--UNION 
--SELECT DISTINCT
--vmp.vpid::varchar(255) as code,
--'VMP' as "level",
--2 as logical_level,
--vmp.vtmid::varchar(255) as parent_code,
--'VTM' as parent_level,
--1 as parent_logical_level
--FROM terminology.dmd_vmp vmp
--UNION
--SELECT DISTINCT
--amp.apid::varchar(255) as code,
--'AMP' as "level",
--3 as logical_level,
--amp.vpid::varchar(255) as parent_code,
--'VMP' as parent_level,
--2 as parent_logical_level
--FROM terminology.dmd_amp amp
--ORDER BY logical_level asc
--WITH NO DATA;
-- 
-- 
-- 
--CREATE INDEX dmd_relationships_mat_code_hash_idx ON terminology.dmd_relationships_mat using hash(code);
--CREATE INDEX dmd_relationships_mat_parent_code_hash_idx ON terminology.dmd_relationships_mat using hash(parent_code);
--CREATE INDEX dmd_relationships_mat_logical_level_idx ON terminology.dmd_relationships_mat using hash(logical_level);
--CREATE INDEX dmd_relationships_mat_parent_logical_level_idx ON terminology.dmd_relationships_mat using hash(parent_logical_level);
-- 
--CREATE INDEX dmd_relationships_mat_level_idx ON terminology.dmd_relationships_mat using btree(level);
--CREATE INDEX dmd_relationships_mat_parent_level_idx ON terminology.dmd_relationships_mat using btree(parent_level);
-- 
--CREATE unique INDEX dmd_relationships_mat_unq_code_parent_code__idx ON terminology.dmd_relationships_mat (code,parent_code);
-- 
--CREATE INDEX dmd_relationships_mat_parent_code_code_idx ON terminology.dmd_relationships_mat (parent_code) include(code);
 
-- terminology.dmd_relationships_mat source

CREATE MATERIALIZED VIEW terminology.dmd_relationships_mat
TABLESPACE pg_default
AS SELECT DISTINCT vtm.vtmid::character varying(255) AS code,
    'VTM'::text AS level,
    1 AS logical_level,
    ''::text AS parent_code,
    ''::text AS parent_level,
    0 AS parent_logical_level
   FROM terminology.dmd_vtm vtm
UNION
 SELECT DISTINCT vmp.vpid::character varying(255) AS code,
    'VMP'::text AS level,
    2 AS logical_level,
    vmp.vtmid::character varying(255) AS parent_code,
    'VTM'::text AS parent_level,
    1 AS parent_logical_level
   FROM terminology.dmd_vmp vmp
UNION
 SELECT DISTINCT amp.apid::character varying(255) AS code,
    'AMP'::text AS level,
    3 AS logical_level,
    amp.vpid::character varying(255) AS parent_code,
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
 
REFRESH MATERIALIZED VIEW terminology.dmd_relationships_mat;
 
--164868--166510
select count(*) from terminology.dmd_relationships_mat where parent_code = '36134911000001109';
 
 
--======================================FUNCTIONS=======================================================================
 
 
set schema 'terminology';
 
--drop function terminology.udf_dmd_get_nodes_by_codes;
 
CREATE or replace function terminology.udf_dmd_get_nodes_by_codes(IN in_codes text[]) 
 
RETURNS TABLE(code varchar(255), name varchar(1022),routecode varchar(255), route varchar(1022),formcode varchar(255), 
form varchar(1022),suppliercode varchar(255), supplier varchar(1022),
prescribingstatuscode bigint, prescribingstatus varchar(1022), controldrugcategorycode bigint, controldrugcategory varchar(1022),
ingredientsubstanceid varchar(255), basispharmaceuticalstrengthcd int8, basisstrengthsubstanceid varchar(255), 
strengthvaluenmtrunitcd varchar(255), strengthvalnmtr numeric, strengthvaluednmtrunitcd varchar(255), strengthvaldnmtr numeric,
basiscd varchar(100),
cfcf varchar(100),
gluf varchar(100),
presf varchar(100) ,
sugf varchar(100),
udfs numeric,
udfsuomcd varchar(255),
unitdoseuomcd varchar(255),
dfindcd varchar(100),
ema varchar(100), --amp (ema)
licauthcd varchar(100), --amp (lic_authcd)
parallelimport varchar(100),
availrestrictcd varchar(100),
ontcd bigint,
parentcode varchar(255), 
logicallevel int) AS 
      
$$
       select distinct 
				 dmd_lkp.code,
				 dmd_lkp.name,
				 dmd_lkp.vmproute_code as routecode,
				 dmd_lkp.vmproute as route,
				 dmd_lkp.vmpform_code as formcode,
				 dmd_lkp.vmpform as form,
				 dmd_lkp.supplier_code as suppliercode,
				 dmd_lkp.supplier,
				 dmd_lkp.prescribing_status_code as prescribingstatuscode,
				 dmd_lkp.prescribing_status as prescribingstatus,
				 dmd_lkp.control_drug_category_code as controldrugcategorycode,
				 dmd_lkp.control_drug_category as controldrugcategory,
				 dmd_lkp.ingredient_substance_id,
				dmd_lkp.basis_pharmaceutical_strength_cd as basispharmaceuticalstrengthcd,
				dmd_lkp.basis_strength_substance_id as basisstrengthsubstanceid,
				dmd_lkp.strength_value_nmtr_unit_cd as strengthvaluenmtrunitcd,
				dmd_lkp.strength_val_nmtr as strengthvalnmtr,
				 dmd_lkp.strength_value_dnmtr_unit_cd as strengthvaluednmtrunitcd,
				 dmd_lkp.strength_val_dnmtr as strengthvaldnmtr,
				 dmd_lkp.basiscd,
				dmd_lkp.cfcf,
				dmd_lkp.gluf,
				dmd_lkp.presf ,
				dmd_lkp.sugf,
				dmd_lkp.udfs,
				dmd_lkp.udfsuomcd,
				dmd_lkp.unitdoseuomcd,
				dmd_lkp.dfindcd,
				dmd_lkp.ema, --amp (ema)
				dmd_lkp.licauthcd, --amp (lic_authcd)
				dmd_lkp.parallelimport, --amp (parallel_import)
				dmd_lkp.availrestrictcd,
				dmd_lkp.ontcd,
				 rel.parent_code,
				 rel.logical_level
				 --rel."level" ,
				 --ARRAY[dmd_lkp.code ]::varchar(1022)[] as srcpath
              from terminology.dmd_names_lookup_all_mat dmd_lkp
              inner join unnest(in_codes) m(code) on m.code = dmd_lkp.code
              inner join terminology.dmd_relationships_mat rel on rel.code = dmd_lkp.code
             
$$
LANGUAGE sql;
 
 
select * from terminology.udf_dmd_get_nodes_by_codes(array['319997009','28807711000001108','3937811000001104']);
--['356138003','28807711000001108']);--'356138003'--'28807711000001108']);
 
 
--================================================
--drop function terminology.udf_dmd_get_child_nodes_search;
 
CREATE or replace function terminology.udf_dmd_get_child_nodes_search(IN in_name text, IN in_code varchar(255)) 
 
RETURNS TABLE(code varchar(255), name varchar(1022),routecode varchar(255), route varchar(1022),formcode varchar(255), form varchar(1022),
suppliercode varchar(255), supplier varchar(1022),
prescribingstatuscode bigint, prescribingstatus varchar(1022), controldrugcategorycode bigint, controldrugcategory varchar(1022), 
parentcode varchar(255), logicallevel int) AS 
      
$$
      
   WITH RECURSIVE search_dmd(      
         code,
         name,
         vmproute_code,
         vmproute,
         vmpform_code,
         vmpform,
         supplier_code,
         supplier,
         prescribing_status_code,
         prescribing_status,
         control_drug_category_code,
         control_drug_category,
         parent_code,
         logical_level,
         --level,  -- depth, starting from 1      
         srcpath  -- path, stored using an array      
       ) as (
                     select distinct 
                                  dmd_name.code,
                                  dmd_name.name,
                                  dmd_name.vmproute_code,
                                  dmd_name.vmproute,
                                  dmd_name.vmpform_code,
                                  dmd_name.vmpform,
                                  dmd_name.supplier_code,
                                  dmd_name.supplier,
                                  dmd_name.prescribing_status_code,
                                  dmd_name.prescribing_status,
                                  dmd_name.control_drug_category_code,
                                  dmd_name.control_drug_category,
                                  rel.parent_code,
                                  rel.logical_level,
                                  --rel."level" ,
                                  ARRAY[dmd_name.code ]::varchar(1022)[] as srcpath
                           from terminology.dmd_names_lookup_mat dmd_name
                           inner join terminology.dmd_relationships_mat rel on rel.code = dmd_name.code
                           where (dmd_name.name_tokens @@ to_tsquery(in_name) or dmd_name.code = in_code)
                     UNION all
                           select 
                                  dmd_name.code,
                                  dmd_name.name,
                                  dmd_name.vmproute_code,
                                  dmd_name.vmproute,
                                  dmd_name.vmpform_code,
                                  dmd_name.vmpform,
                                  dmd_name.supplier_code,
                                  dmd_name.supplier,
                                  dmd_name.prescribing_status_code,
                                  dmd_name.prescribing_status,
                                  dmd_name.control_drug_category_code,
                                  dmd_name.control_drug_category,
                                  rel.parent_code,
                                  rel.logical_level,
                                  --rel."level",
                                  (s.srcpath || rel.code)::varchar(1022)[] as srcpath
                            FROM
                             terminology.dmd_relationships_mat rel
                         INNER JOIN search_dmd s 
                            ON s.code = rel.parent_code --and rel.level > s.level
                           AND (rel.code <> ALL(s.srcpath))        -- prevent from cycling 
                           --AND s.level <= 15 --(Total levels will be <level> + 1 )
                         inner join terminology.dmd_names_lookup_mat dmd_name on dmd_name.code = rel.code 
              )
      
       SELECT distinct
              code,
              name,
              vmproute_code as routecode,
              vmproute as route,
              vmpform_code as formcode,
              vmpform as form,
              supplier_code as suppliercode,
              supplier,
              prescribing_status_code as prescribingstatuscode,
              prescribing_status as prescribingstatus,
              control_drug_category_code as controldrugcategorycode,
              control_drug_category as controldrugcategory,
              parent_code as parentcode,
              logical_level as logicallevel
              --level  -- depth, starting from 1  
       FROM search_dmd;
$$
LANGUAGE sql;
 

select * from terminology.udf_dmd_get_child_nodes_search('Tramadol:* & Paracetamol:* ', '');

 
select * from terminology.udf_dmd_get_child_nodes_search('Asparaginase:* & 10,000unit:* & powder:*', '');
select * from terminology.udf_dmd_get_child_nodes_search('', '28807711000001108');
 
 
--================================================
 
 
--drop function terminology.udf_dmd_get_next_descendent;
 
CREATE or replace function terminology.udf_dmd_get_next_descendent(IN in_codes text[]) 
 
RETURNS TABLE(code varchar(255), name varchar(1022),routecode varchar(255), route varchar(1022),formcode varchar(255), form varchar(1022),
suppliercode varchar(255), supplier varchar(1022),
prescribingstatuscode bigint, prescribingstatus varchar(1022), controldrugcategorycode bigint, controldrugcategory varchar(1022), 
parentcode varchar(255), logicallevel int) AS 
      
$$
       select distinct 
                     dmd_lkp.code,
                     dmd_lkp.name,
                     dmd_lkp.vmproute_code as routecode,
                     dmd_lkp.vmproute as route,
                     dmd_lkp.vmpform_code as formcode,
                     dmd_lkp.vmpform as form,
                     dmd_lkp.supplier_code as suppliercode,
                     dmd_lkp.supplier,
                     dmd_lkp.prescribing_status_code as prescribingstatuscode,
                     dmd_lkp.prescribing_status as prescribingstatus,
                     dmd_lkp.control_drug_category_code as controldrugcategorycode,
                     dmd_lkp.control_drug_category as controldrugcategory,
                     rel.parent_code,
                     rel.logical_level
                     --rel."level" ,
                     --ARRAY[dmd_lkp.code ]::varchar(1022)[] as srcpath
              from terminology.dmd_names_lookup_mat dmd_lkp
              inner join unnest(in_codes) m(code) on m.code = dmd_lkp.code
              inner join terminology.dmd_relationships_mat rel on rel.code = dmd_lkp.code
       UNION all
              select distinct 
                     dmd_lkp.code,
                     dmd_lkp.name,
                     dmd_lkp.vmproute_code as routecode,
                     dmd_lkp.vmproute as route,
                     dmd_lkp.vmpform_code as formcode,
                     dmd_lkp.vmpform as form,
                     dmd_lkp.supplier_code as suppliercode,
                     dmd_lkp.supplier,
                     dmd_lkp.prescribing_status_code as prescribingstatuscode,
                     dmd_lkp.prescribing_status as prescribingstatus,
                     dmd_lkp.control_drug_category_code as controldrugcategorycode,
                     dmd_lkp.control_drug_category as controldrugcategory,
                     rel.parent_code,
                     rel.logical_level
                     --rel."level" ,
                     --ARRAY[dmd_lkp.code ]::varchar(1022)[] as srcpath
              from terminology.dmd_relationships_mat rel
              inner join terminology.dmd_names_lookup_mat dmd_lkp on dmd_lkp.code = rel.code
              inner join unnest(in_codes) m(code) on m.code = rel.parent_code 
             
$$
LANGUAGE sql;
 
 
select * from terminology.udf_dmd_get_next_descendent(array['28807711000001108','3937811000001104']);--['356138003','28807711000001108']);--'356138003'--'28807711000001108']);
 
--==============
 
--drop function terminology.udf_dmd_get_ancestor_nodes_search;
 
CREATE or replace function terminology.udf_dmd_get_ancestor_nodes_search(IN in_name text, IN in_code varchar(255)) 
 
RETURNS TABLE(code varchar(255), name varchar(1022),routecode varchar(255), route varchar(1022),formcode varchar(255), 
form varchar(1022),
suppliercode varchar(255), supplier varchar(1022),
prescribingstatuscode bigint, prescribingstatus varchar(1022), controldrugcategorycode bigint, controldrugcategory varchar(1022), 
parentcode varchar(255), logicallevel int) AS 
      
$$
       WITH RECURSIVE search_dmd(      
         code,
         name,
         vmproute_code,
         vmproute,
         vmpform_code,
         vmpform,
         supplier_code,
         supplier,
         prescribing_status_code,
         prescribing_status,
         control_drug_category_code,
         control_drug_category,
         parent_code,
         logical_level,
         srcpath  -- path, stored using an array     
       ) as (
                     select distinct 
                                  dmd_name.code,
                                  dmd_name.name,
                                  dmd_name.vmproute_code,
                                  dmd_name.vmproute,
                                  dmd_name.vmpform_code,
                                   dmd_name.vmpform,
                                  dmd_name.supplier_code,
                                  dmd_name.supplier,
                                  dmd_name.prescribing_status_code,
                                  dmd_name.prescribing_status,
                                  dmd_name.control_drug_category_code,
                                  dmd_name.control_drug_category,
                                  rel.parent_code,
                                  rel.logical_level,
                                  --rel."level" ,
                                  ARRAY[dmd_name.code ]::varchar(1022)[] as srcpath
                           from terminology.dmd_names_lookup_mat dmd_name
                           inner join terminology.dmd_relationships_mat rel on rel.code = dmd_name.code
                           where (dmd_name.name_tokens @@ to_tsquery(in_name) or dmd_name.code = in_code)
                     UNION all
                           select 
                                  dmd_name.code,
                                  dmd_name.name,
                                  dmd_name.vmproute_code,
                                  dmd_name.vmproute,
                                  dmd_name.vmpform_code,
                                  dmd_name.vmpform,
                                  dmd_name.supplier_code,
                                  dmd_name.supplier,
                                  dmd_name.prescribing_status_code,
                                  dmd_name.prescribing_status,
                                  dmd_name.control_drug_category_code,
                                  dmd_name.control_drug_category,
                                  rel.parent_code,
                                  rel.logical_level,
                                  --rel."level",
                                  (s.srcpath || rel.code )::varchar(1022)[] as srcpath
                            FROM
                             terminology.dmd_relationships_mat rel
                         INNER JOIN search_dmd s 
                            ON s.parent_code = rel.code --and rel.logical_level > s.logical_level
                           AND (rel.code <> ALL(s.srcpath))        -- prevent from cycling 
                           --AND s.level <= 15 --(Total levels will be <level> + 1 )
                         inner join terminology.dmd_names_lookup_mat dmd_name on dmd_name.code = rel.code 
              )
      
       SELECT distinct
              code,
              name,
              vmproute_code as routecode,
              vmproute as route,
              vmpform_code as formcode,
              vmpform as form,
              supplier_code as suppliercode,
              supplier,
              prescribing_status_code as prescribingstatuscode,
              prescribing_status as prescribingstatus,
              control_drug_category_code as controldrugcategorycode,
              control_drug_category as controldrugcategory,
              parent_code as parentcode,
              logical_level as logicallevel
       FROM search_dmd;
   
$$
LANGUAGE sql;
 
 
select * from terminology.udf_dmd_get_ancestor_nodes_search('ast:*','');
select * from terminology.udf_dmd_get_ancestor_nodes_search('','28807711000001108');
 
--================================================
 
set schema 'terminology';
 
--drop function terminology.udf_dmd_get_next_ancestor;
 
CREATE or replace function terminology.udf_dmd_get_next_ancestor(IN in_codes text[]) 
 
RETURNS TABLE(code varchar(255), name varchar(1022),routecode varchar(255), route varchar(1022),
formcode varchar(255), form varchar(1022),suppliercode varchar(255), supplier varchar(1022),
prescribingstatuscode bigint, prescribingstatus varchar(1022), controldrugcategorycode bigint, 
controldrugcategory varchar(1022), parentcode varchar(255), logicallevel int) AS 
      
$$
       select distinct 
                     dmd_lkp.code,
                     dmd_lkp.name,
                     dmd_lkp.vmproute_code as routecode,
                     dmd_lkp.vmproute as route,
                     dmd_lkp.vmpform_code as form_code,
                     dmd_lkp.vmpform as form,
                     dmd_lkp.supplier_code as suppliercode,
                     dmd_lkp.supplier,
                     dmd_lkp.prescribing_status_code as prescribingstatuscode,
                     dmd_lkp.prescribing_status as prescribingstatus,
                     dmd_lkp.control_drug_category_code as controldrugcategorycode,
                     dmd_lkp.control_drug_category as controldrugcategory,
                     rel.parent_code,
                     rel.logical_level
                     --rel."level" ,
                     --ARRAY[dmd_lkp.code ]::varchar(1022)[] as srcpath
              from terminology.dmd_names_lookup_mat dmd_lkp
              inner join unnest(in_codes) m(code) on m.code = dmd_lkp.code
              inner join terminology.dmd_relationships_mat rel on rel.code = dmd_lkp.code
       UNION all
              select distinct 
                     dmd_lkp_parent.code,
                     dmd_lkp_parent.name,
                     dmd_lkp_parent.vmproute_code as routecode,
                     dmd_lkp_parent.vmproute as route,
                     dmd_lkp_parent.vmpform_code as formcode,
                     dmd_lkp_parent.vmpform as form,
                     dmd_lkp_parent.supplier_code as suppliercode,
                     dmd_lkp_parent.supplier,
                     dmd_lkp_parent.prescribing_status_code as prescribingstatuscode,
                     dmd_lkp_parent.prescribing_status as prescribingstatus,
                     dmd_lkp_parent.control_drug_category_code as controldrugcategorycode,
                     dmd_lkp_parent.control_drug_category as controldrugcategory,
                     parent_rel.parent_code,
                     parent_rel.logical_level
                     --parent_rel."level" ,
                     --ARRAY[dmd_lkp_parent.code ]::varchar(1022)[] as srcpath
              from (select 
                           dmd_lkp.code,
                           dmd_lkp.name,
                           rel.parent_code,
                           rel.logical_level
                     from terminology.dmd_relationships_mat rel
                     inner join unnest(in_codes) m(code) on m.code = rel.code
                     inner join terminology.dmd_names_lookup_mat dmd_lkp on dmd_lkp.code = rel.parent_code) 
                     as base
                     inner join terminology.dmd_relationships_mat parent_rel on base.parent_code = parent_rel.code 
                     inner join terminology.dmd_names_lookup_mat dmd_lkp_parent on dmd_lkp_parent.code = parent_rel.code
 
             
$$
LANGUAGE sql;
 
 
select * from terminology.udf_dmd_get_next_ancestor(array['28807711000001108']);--'356138003','28807711000001108','3937811000001104']);
 
--======================================END FUNCTIONS=======================================================================
 
 
 
--=====================================================================================================================
 
 
 
--==========================================================PLAYGROUND=============================================================================
select * from terminology.dmd_names_lookup_mat dmd limit 10;
 
WITH RECURSIVE search_dmd(      
         code,
         name,
         parent_code,
         logical_level,  -- depth, starting from 1      
         srcpath  -- path, stored using an array      
       ) as (
                     select distinct 
                                  dmd_name.code,
                                  dmd_name.name,
                                  rel.parent_code,
                                  rel.logical_level,
                                  ARRAY[dmd_name.code ]::varchar(1022)[] as srcpath
                           from terminology.dmd_names_lookup_mat dmd_name
                           inner join terminology.dmd_relationships_mat rel on rel.code = dmd_name.code
                           where dmd_name.name_tokens @@ to_tsquery('par:*')
                     UNION all
                           select 
                                  dmd_name.code,
                                  dmd_name.name,
                                  rel.parent_code,
                                  rel.logical_level,
                                  (s.srcpath || rel.code)::varchar(1022)[] as srcpath
                            FROM
                             terminology.dmd_relationships_mat rel
                         INNER JOIN search_dmd s 
                            ON s.code = rel.parent_code --and rel.logical_level > s.logical_level
                           AND (rel.code <> ALL(s.srcpath))        -- prevent from cycling 
                           --AND s.level <= 15 --(Total levels will be <level> + 1 )
                         inner join terminology.dmd_names_lookup_mat dmd_name on dmd_name.code = rel.code 
              )
      
       SELECT distinct
              code,
              name,
              parent_code,
              logical_level  -- depth, starting from 1  
       FROM search_dmd;
 
 
 
WITH RECURSIVE search_dmd(      
         code,
         name,
         parent_code,
         logical_level,  -- depth, starting from 1      
         srcpath  -- path, stored using an array      
       ) as (
                     select distinct 
                                  dmd_name.code,
                                  dmd_name.name,
                                  rel.parent_code,
                                  rel.logical_level,
                                  ARRAY[dmd_name.code ]::varchar(1022)[] as srcpath
                           from terminology.dmd_names_lookup_mat dmd_name
                           inner join terminology.dmd_relationships_mat rel on rel.code = dmd_name.code
                           where dmd_name.name_tokens @@ to_tsquery('par:*')
                     UNION all
                           select 
                                  dmd_name.code,
                                  dmd_name.name,
                                  rel.parent_code,
                                  rel.logical_level,
                                  (s.srcpath || rel.code )::varchar(1022)[] as srcpath
                            FROM
                             terminology.dmd_relationships_mat rel
                         INNER JOIN search_dmd s 
                            ON s.parent_code = rel.code --and rel.logical_level > s.logical_level
                           AND (rel.code <> ALL(s.srcpath))        -- prevent from cycling 
                           --AND s.level <= 15 --(Total levels will be <level> + 1 )
                         inner join terminology.dmd_names_lookup_mat dmd_name on dmd_name.code = rel.code 
              )
      
       SELECT distinct
              code,
              name,
              parent_code,
              logical_level  -- depth, starting from 1  
       FROM search_dmd;
 
--===================relationship==============================================================
--drop materialized VIEW snomedct_relationshipwithnames_latest_mat_old1;
CREATE materialized VIEW dmd_relationshipwithnames_latest_mat_old1 AS
SELECT 
vtm.nm as "name",
to_tsvector('english',vtm.nm) name_tokens,
vtm.vtmid::varchar(255) as code,
'VTM' as "level",
1 as logical_level,
'' as parent_code,
'' as parent_level,
0 as parent_logical_level,
'' as route,
'' as form
FROM terminology.dmd_vtm vtm
UNION 
SELECT 
vmp.nm as "name",
to_tsvector('english',vmp.nm) name_tokens,
vmp.vpid::varchar(255) as code,
'VMP' as "level",
2 as logical_level,
vmp.vtmid::varchar(255) as parent_code,
'VTM' as parent_level,
1 as parent_logical_level,
route.desc as vmproute,
form.desc as vmpform
FROM terminology.dmd_vmp vmp
LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
       LEFT JOIN terminology.dmd_lookup_form form on form.cd = vdf.formcd
LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
       LEFT JOIN terminology.dmd_lookup_route route on route.cd = vdr.routecd
--LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd = vmp.vpid
UNION
SELECT 
amp.nm as "name",
to_tsvector('english',amp.nm) name_tokens,
amp.apid::varchar(255) as code,
'AMP' as "level",
3 as logical_level,
amp.vpid::varchar(255) as parent_code,
'VMP' as parent_level,
2 as parent_logical_level,
'' as route,
'' as form
FROM terminology.dmd_amp amp
ORDER BY logical_level, "name" asc
 
 
--===========================================================================================
 
 
select vtmid, count(*) cnt from terminology.dmd_vmp vmp
group by vtmid 
having count(*) > 1 and vtmid is null
 
select vpid, count(*) cnt from terminology.dmd_amp amp
group by amp.vpid 
having count(amp.vpid) > 1 and vpid is null
 
select * from terminology.dmd_vmp dv 
where dv.vpid = '34194311000001106'--'10164411000001102'
 
 
select * from terminology.dmd_amp da 
where da.apid = '10164611000001104'
 
 
SELECT 
vtm.nm as "name",
to_tsvector('english',vtm.nm) name_tokens,
vtm.vtmid::varchar(255) as code,
'VTM' as "level",
1 as logical_level,
'' as parent_code,
'' as parent_level,
0 as parent_logical_level,
'' as route,
'' as form
FROM terminology.dmd_vtm vtm
 
 
SELECT DISTINCT
vmp.vpid::varchar(255) as code,
'VMP' as "level",
2 as logical_level,
vmp.vtmid::varchar(255) as parent_code,
'VTM' as parent_level,
1 as parent_logical_level
FROM terminology.dmd_vmp vmp
where vmp.vtmid is null;
 
 
 
SELECT 
vmp.nm as "name",
to_tsvector('english',vmp.nm) name_tokens,
vmp.vpid::varchar(255) as code,
'VMP' as "level",
2 as logical_level,
vmp.vtmid::varchar(255) as parent_code,
'VTM' as parent_level,
1 as parent_logical_level,
route.desc as vmproute,
form.desc as vmpform
FROM terminology.dmd_vmp vmp
LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
       LEFT JOIN terminology.dmd_lookup_form form on form.cd = vdf.formcd
LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
       LEFT JOIN terminology.dmd_lookup_route route on route.cd = vdr.routecd
--LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd = vmp.vpid
where vmp.vtmid ='356138003'--vmp.vtmid is null;
 
 
 
SELECT 
amp.nm as "name",
to_tsvector('english',amp.nm) name_tokens,
amp.apid::varchar(255) as code,
'AMP' as "level",
3 as logical_level,
amp.vpid::varchar(255) as parent_code,
'VMP' as parent_level,
2 as parent_logical_level,
'' as route,
'' as form
FROM terminology.dmd_amp amp
where  amp.vpid ='36134911000001109'--amp.vpid is null
 
 
set schema 'terminology';
 
 
SELECT DISTINCT
vtm.nm as "name",
to_tsvector('english',vtm.nm) name_tokens,
vtm.vtmid::varchar(255) as code,
'' as route,
'' as form,
'' as supplier,
to_tsvector('english','') as supplier_tokens
FROM terminology.dmd_vtm vtm
UNION 
SELECT DISTINCT
vmp.nm as "name",
to_tsvector('english',vmp.nm) name_tokens,
vmp.vpid::varchar(255) as code,
route.desc as vmproute,
form.desc as vmpform,
'' as supplier,
to_tsvector('english','') as supplier_tokens
FROM terminology.dmd_vmp vmp
LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
       LEFT JOIN terminology.dmd_lookup_form form on form.cd = vdf.formcd
LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
       LEFT JOIN terminology.dmd_lookup_route route on route.cd = vdr.routecd
--LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd = vmp.vpid
UNION
SELECT distinct
CASE WHEN supp."desc" IS NULL or supp."desc" = ''
            THEN amp.nm 
            ELSE amp.nm || ' ('  || supp."desc" || ')'
    END AS "name",
to_tsvector('english',amp.nm) name_tokens,
amp.apid::varchar(255) as code,
'' as route,
'' as form,
supp."desc" as supplier,
to_tsvector('english',supp."desc") supplier_tokens
FROM terminology.dmd_amp amp
left outer join dmd_lookup_supplier supp on supp.cd = amp.suppcd
ORDER BY "name" asc
 
--========================
 
SELECT DISTINCT
vtm.nm as "name",
to_tsvector('english',vtm.nm) name_tokens,
vtm.vtmid::varchar(255) as code,
'' as route,
'' as form,
'' as supplier,
to_tsvector('english','') as supplier_name_tokens
FROM terminology.dmd_vtm vtm
UNION 
SELECT DISTINCT
vmp.nm as "name",
to_tsvector('english',vmp.nm) name_tokens,
vmp.vpid::varchar(255) as code,
route.desc as vmproute,
form.desc as vmpform,
'' as supplier,
to_tsvector('english','') as supplier_name_tokens
FROM terminology.dmd_vmp vmp
LEFT JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
       LEFT JOIN terminology.dmd_lookup_form form on form.cd = vdf.formcd
LEFT JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
       LEFT JOIN terminology.dmd_lookup_route route on route.cd = vdr.routecd
--LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd = vmp.vpid
UNION
SELECT DISTINCT
CASE WHEN supp."desc" IS NULL or supp."desc" = ''
            THEN amp.nm 
            ELSE amp.nm || ' ('  || supp."desc" || ')'
    END AS "name",
CASE WHEN supp."desc" IS NULL or supp."desc" = ''
            THEN to_tsvector('english',amp.nm) 
            ELSE to_tsvector('english',amp.nm || ' ('  || supp."desc" || ')')
    END AS "name_tokens",
amp.apid::varchar(255) as code,
'' as route,
'' as form,
supp."desc" as supplier,
to_tsvector('english',supp."desc") supplier_name_tokens
FROM terminology.dmd_amp amp
LEFT OUTER JOIN dmd_lookup_supplier supp on supp.cd = amp.suppcd
ORDER BY "name" asc
 
set schema 'terminology';
 
SELECT DISTINCT
vtm.nm as "name",
to_tsvector('english',vtm.nm) name_tokens,
vtm.vtmid::varchar(255) as code,
'' as vmproute,
'' as vmpform,
'' as supplier,
to_tsvector('english','') as supplier_name_tokens,
'' as prescribing_status,
'' as control_drug_category
FROM terminology.dmd_vtm vtm
UNION 
SELECT DISTINCT
vmp.nm as "name",
to_tsvector('english',vmp.nm) name_tokens,
vmp.vpid::varchar(255) as code,
route.desc as vmproute,
form.desc as vmpform,
'' as supplier,
to_tsvector('english','') as supplier_name_tokens,
pres."desc" as prescribing_status,
lcd."desc" as control_drug_category
FROM terminology.dmd_vmp vmp
LEFT OUTER JOIN terminology.dmd_lookup_prescribingstatus pres on pres.cd = vmp.pres_statcd
LEFT OUTER JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
       LEFT OUTER JOIN terminology.dmd_lookup_form form on form.cd = vdf.formcd
LEFT OUTER JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
       LEFT OUTER JOIN terminology.dmd_lookup_route route on route.cd = vdr.routecd
LEFT OUTER JOIN terminology.dmd_vmp_controldrug vcd ON vcd.vpid = vmp.vpid
       LEFT OUTER JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd = vcd.catcd 
--where lcd."desc" is not null
 
UNION
SELECT DISTINCT
CASE WHEN supp."desc" IS NULL or supp."desc" = ''
            THEN amp.nm 
            ELSE amp.nm || ' ('  || supp."desc" || ')'
    END AS "name",
CASE WHEN supp."desc" IS NULL or supp."desc" = ''
            THEN to_tsvector('english',amp.nm) 
            ELSE to_tsvector('english',amp.nm || ' ('  || supp."desc" || ')')
    END AS "name_tokens",
amp.apid::varchar(255) as code,
'' as vmproute,
'' as vmpform,
supp."desc" as supplier,
to_tsvector('english',supp."desc") supplier_name_tokens,
'' as prescribing_status,
'' as control_drug_category
FROM terminology.dmd_amp amp
LEFT OUTER JOIN dmd_lookup_supplier supp on supp.cd = amp.suppcd
ORDER BY "name" asc
 
--===========
--166550
select count(*) from (
SELECT DISTINCT
vtm.nm as "name",
to_tsvector('english',vtm.nm) name_tokens,
vtm.vtmid::varchar(255) as code,
'' as vmproute,
'' as vmpform,
'' as supplier,
to_tsvector('english','') as supplier_name_tokens,
'' as prescribing_status,
'' as control_drug_category
FROM terminology.dmd_vtm vtm
UNION 
SELECT DISTINCT
vmp.nm as "name",
to_tsvector('english',vmp.nm) name_tokens,
vmp.vpid::varchar(255) as code,
route.desc as vmproute,
form.desc as vmpform,
'' as supplier,
to_tsvector('english','') as supplier_name_tokens,
pres."desc" as prescribing_status,
lcd."desc" as control_drug_category
FROM terminology.dmd_vmp vmp
LEFT OUTER JOIN terminology.dmd_lookup_prescribingstatus pres on pres.cd = vmp.pres_statcd
LEFT OUTER JOIN terminology.dmd_vmp_drugform vdf ON vdf.vpid = vmp.vpid
       LEFT OUTER JOIN terminology.dmd_lookup_form form on form.cd = vdf.formcd
LEFT OUTER JOIN terminology.dmd_vmp_drugroute vdr ON vdr.vpid = vmp.vpid
       LEFT OUTER JOIN terminology.dmd_lookup_route route on route.cd = vdr.routecd
LEFT OUTER JOIN terminology.dmd_vmp_controldrug vcd ON vcd.vpid = vmp.vpid
       LEFT OUTER JOIN terminology.dmd_lookup_controldrugcat lcd ON lcd.cd = vcd.catcd 
--where lcd."desc" is not null
 
UNION
SELECT DISTINCT
CASE WHEN supp."desc" IS NULL or supp."desc" = ''
            THEN amp.nm 
            ELSE amp.nm || ' ('  || supp."desc" || ')'
    END AS "name",
CASE WHEN supp."desc" IS NULL or supp."desc" = ''
            THEN to_tsvector('english',amp.nm) 
            ELSE to_tsvector('english',amp.nm || ' ('  || supp."desc" || ')')
    END AS "name_tokens",
amp.apid::varchar(255) as code,
'' as vmproute,
'' as vmpform,
supp."desc" as supplier,
to_tsvector('english',supp."desc") supplier_name_tokens,
'' as prescribing_status,
'' as control_drug_category
FROM terminology.dmd_amp amp
LEFT OUTER JOIN dmd_lookup_supplier supp on supp.cd = amp.suppcd
ORDER BY "name" asc) a
 
 
explain analyze
WITH RECURSIVE search_dmd(      
         code,
         name,
         vmproute_code,
         vmproute,
         vmpform_code,
         vmpform,
         supplier_code,
         supplier,
         prescribing_status_code,
         prescribing_status,
         control_drug_category_code,
         control_drug_category,
         parent_code,
         logical_level,
         --level,  -- depth, starting from 1      
         srcpath  -- path, stored using an array      
       ) as (
                     select distinct 
                                  dmd_name.code,
                                  dmd_name.name,
                                  dmd_name.vmproute_code,
                                  dmd_name.vmproute,
                                  dmd_name.vmpform_code,
                                  dmd_name.vmpform,
                                  dmd_name.supplier_code,
                                  dmd_name.supplier,
                                  dmd_name.prescribing_status_code,
                                  dmd_name.prescribing_status,
                                  dmd_name.control_drug_category_code,
                                  dmd_name.control_drug_category,
                                  rel.parent_code,
                                  rel.logical_level,
                                  --rel."level" ,
                                  ARRAY[dmd_name.code ]::varchar(1022)[] as srcpath
                           from terminology.dmd_names_lookup_mat dmd_name
                           inner join terminology.dmd_relationships_mat rel on rel.code = dmd_name.code
                           where (dmd_name.name_tokens @@ to_tsquery('par:*') or dmd_name.code = '')
                     UNION all
                           select 
                                  dmd_name.code,
                                  dmd_name.name,
                                  dmd_name.vmproute_code,
                                  dmd_name.vmproute,
                                  dmd_name.vmpform_code,
                                  dmd_name.vmpform,
                                  dmd_name.supplier_code,
                                  dmd_name.supplier,
                                  dmd_name.prescribing_status_code,
                                  dmd_name.prescribing_status,
                                  dmd_name.control_drug_category_code,
                                  dmd_name.control_drug_category,
                                  rel.parent_code,
                                  rel.logical_level,
                                  --rel."level",
                                  (s.srcpath || rel.code)::varchar(1022)[] as srcpath
                            FROM
                             terminology.dmd_relationships_mat rel
                         INNER JOIN search_dmd s 
                            ON s.code = rel.parent_code --and rel.level > s.level
                           AND (rel.code <> ALL(s.srcpath))        -- prevent from cycling 
                           --AND s.level <= 15 --(Total levels will be <level> + 1 )
                         inner join terminology.dmd_names_lookup_mat dmd_name on dmd_name.code = rel.code 
              )
      
       SELECT distinct
              code,
              name,
              vmproute_code as routecode,
              vmproute as route,
              vmpform_code as formcode,
              vmpform as form,
              supplier_code as suppliercode,
              supplier,
              prescribing_status_code as prescribingstatuscode,
              prescribing_status as prescribingstatus,
              control_drug_category_code as controldrugcategorycode,
              control_drug_category as controldrugcategory,
              parent_code as parentcode,
              logical_level as logicallevel
              --level  -- depth, starting from 1  
       FROM search_dmd;
 
--======================================================
 
set schema 'terminology';
 
 
select* from 
 
select * from dmd_lookup_route dlr;
 
--============================================
 
select dvi.vpid 
from terminology.dmd_vmp_ingredient dvi 
group by dvi.vpid having count(*) > 1
 
select * from terminology.dmd_vmp_ingredient dvi 
where dvi.vpid = '16174411000001100';


--==================
select distinct 
      dmd_name.code,
      dmd_name.name,
      dmd_name.vmproute_code,
      dmd_name.vmproute,
      dmd_name.vmpform_code,
       dmd_name.vmpform,
      dmd_name.supplier_code,
      dmd_name.supplier,
      dmd_name.prescribing_status_code,
      dmd_name.prescribing_status,
      dmd_name.control_drug_category_code,
      dmd_name.control_drug_category,
      rel.parent_code,
      rel.logical_level,
      --rel."level" ,
      ARRAY[dmd_name.code ]::varchar(1022)[] as srcpath
   from terminology.dmd_names_lookup_mat dmd_name
   inner join terminology.dmd_relationships_mat rel on rel.code = dmd_name.code
   where (dmd_name.name_tokens @@ to_tsquery('para:*') or dmd_name.code = '')
   and dmd_name.vmproute_code is not null
   
   --10931911000001101 -- same route - multiple
   
--============================

   select * from terminology.dmd_vmp_ingredient dvi ;
  
  set schema 'terminology';
 
  select cdprev from terminology.dmd_lookup_route dlr
 group by cdprev having count(*) > 1;

select cd from terminology.dmd_lookup_supplier dls where invalid = 1
 group by cdprev having count(*) > 1;

select basiscd,* from terminology.dmd_vmp dv limit 100;

select basiscd,b.desc,* from terminology.dmd_vmp dv
inner join terminology.dmd_lookup_basisofname b on b.cd = dv.basiscd

select cfc_f,* from terminology.dmd_vmp dv
--where cfc_f = 0;
where sug_f is not null;

select vondf.* from terminology.dmd_vmp vmp
inner join terminology.dmd_vmp_ontdrugform vondf ON vondf.vpid = vmp.vpid 
inner JOIN terminology.dmd_lookup_ontformroute ont ON ont.cd = vondf.formcd 
where vondf.formcd is not null

select * from dmd_lookup_ontformroute;

set schema 'terminology';

select vpid,* from dmd_vmp_ingredient dvi  
where dvi.strnt_dnmtr_val is not null
and vpid = '330729005'

select dv.vpid , vt.vtmid ,* from terminology.dmd_vmp dv 
inner join terminology.dmd_vtm vt on vt.vtmid = dv.vtmid 
order by vt.vtmid desc 
limit 1000;

set schema 'terminology';


select * from dmd_lookup_drugformind dld ;

select * from terminology.dmd_vmp dv
where nmchangecd is not null
limit 10;

select * from dmd_lookup_supplier dls 
where dls.desc ilike 'wock%'
where dls.cd ilike '93016%'

select strnt_dnmtr_uomcd,* from dmd_lookup_ingredient dli 
where dli.isid  = '387517004'

select * from dmd_vmp_ingredient dvi 
where dvi.vpid  = '10931911000001101'

select * from dmd_vmp dv where dv.vpid = '330783008'--'330630009'--'330729005'





   