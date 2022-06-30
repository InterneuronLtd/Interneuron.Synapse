DROP FUNCTION IF EXISTS terminology.udf_dmd_get_all_ancestors;
CREATE OR REPLACE FUNCTION terminology.udf_dmd_get_all_ancestors(in_codes text[])
 RETURNS TABLE(code character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, parentcode character varying, logicallevel integer)
 LANGUAGE sql
AS $function$
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
							inner join unnest(in_codes) m(code) on m.code = dmd_name.code

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
   
$function$
;
DROP FUNCTION IF EXISTS terminology.udf_dmd_get_all_descendents;

CREATE OR REPLACE FUNCTION terminology.udf_dmd_get_all_descendents(in_codes text[])
 RETURNS TABLE(code character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, parentcode character varying, logicallevel integer)
 LANGUAGE sql
AS $function$
      
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
			              inner join unnest(in_codes) m(code) on m.code = dmd_name.code
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
$function$
;
DROP FUNCTION IF EXISTS terminology.udf_dmd_get_ancestor_nodes_search;

CREATE OR REPLACE FUNCTION terminology.udf_dmd_get_ancestor_nodes_search(in_name text, in_code character varying)
 RETURNS TABLE(code character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, parentcode character varying, logicallevel integer)
 LANGUAGE sql
AS $function$
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
   
$function$
;
DROP FUNCTION IF EXISTS terminology.udf_dmd_get_child_nodes_search;

CREATE OR REPLACE FUNCTION terminology.udf_dmd_get_child_nodes_search(in_name text, in_code character varying)
 RETURNS TABLE(code character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, parentcode character varying, logicallevel integer)
 LANGUAGE sql
AS $function$
      
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
$function$
;
DROP FUNCTION IF EXISTS terminology.udf_dmd_get_next_ancestor;

CREATE OR REPLACE FUNCTION terminology.udf_dmd_get_next_ancestor(in_codes text[])
 RETURNS TABLE(code character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, parentcode character varying, logicallevel integer)
 LANGUAGE sql
AS $function$
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
 
             
$function$
;
DROP FUNCTION IF EXISTS terminology.udf_dmd_get_next_descendent;

CREATE OR REPLACE FUNCTION terminology.udf_dmd_get_next_descendent(in_codes text[])
 RETURNS TABLE(code character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, parentcode character varying, logicallevel integer)
 LANGUAGE sql
AS $function$
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
             
$function$
;
DROP FUNCTION IF EXISTS terminology.udf_dmd_get_nodes_by_codes;

CREATE OR REPLACE FUNCTION terminology.udf_dmd_get_nodes_by_codes(in_codes text[])
 RETURNS TABLE(code character varying, prevcode character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, ingredientsubstanceid character varying, basispharmaceuticalstrengthcd bigint, basisstrengthsubstanceid character varying, strengthvaluenmtrunitcd character varying, strengthvalnmtr numeric, strengthvaluednmtrunitcd character varying, strengthvaldnmtr numeric, basiscd character varying, cfcf character varying, gluf character varying, presf character varying, sugf character varying, udfs numeric, udfsuomcd character varying, unitdoseuomcd character varying, dfindcd character varying, ema character varying, licauthcd character varying, parallelimport character varying, availrestrictcd character varying, ontcd bigint, parentcode character varying, logicallevel integer)
 LANGUAGE sql
AS $function$
       select distinct 
				 dmd_lkp.code,
				 dmd_lkp.prevcode,
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
             
$function$
;
