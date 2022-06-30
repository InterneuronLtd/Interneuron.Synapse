--
-- PostgreSQL database dump
--

-- Dumped from database version 13.4
-- Dumped by pg_dump version 14.1 (Ubuntu 14.1-1.pgdg18.04+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: local_formulary; Type: SCHEMA; Schema: -; Owner: owner_name
--

CREATE SCHEMA local_formulary;


ALTER SCHEMA local_formulary OWNER TO owner_name;

--
-- Name: terminology; Type: SCHEMA; Schema: -; Owner: owner_name
--

CREATE SCHEMA terminology;


ALTER SCHEMA terminology OWNER TO owner_name;

--
-- Name: terminology_staging; Type: SCHEMA; Schema: -; Owner: owner_name
--

CREATE SCHEMA terminology_staging;


ALTER SCHEMA terminology_staging OWNER TO owner_name;

--
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


--
-- Name: get_difference_bw_active_draft(); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.get_difference_bw_active_draft() RETURNS text
    LANGUAGE plpgsql
    AS $$
declare
	 flag boolean default false;
	 codes text default '';
	 rec_code   record;
	 cur_codes cursor 
		 for select code, "name" from local_formulary.formulary_header fh 
			 where rec_status_code in ('001', '003') and is_latest = true and product_type = 'AMP'
			 group by code, "name"
			 having count(*) > 1
			 order by "name"; 
begin
   -- open the cursor
   open cur_codes;
	
   loop
    -- fetch row into the code
      fetch cur_codes into rec_code;
    -- exit when no more row to fetch
      exit when not found;
	
     flag = false;
     
    -- build the output
     if ((select count(*) from
     		( 
     			select 1 FROM local_formulary.formulary_header
				where formulary_version_id in (
					select formulary_version_id from local_formulary.formulary_header fh where code = rec_code.code and rec_status_code in ('001', '003') and is_latest = true and product_type = 'AMP'
				)
				group by parent_code, parent_product_type, rec_source, vtm_id, vmp_id, rec_statuschange_msg
				having count(*) != 2
			) as x) > 0
		)then
      	
		flag = true;
        
        codes := codes || '#' || rec_code.code || ' - Header';
     
      end if;
    
     if ((select count(*) from 
     		(
     			select 1 from local_formulary.formulary_detail fd 
				where formulary_version_id in (
					select formulary_version_id from local_formulary.formulary_header fh where code = rec_code.code and rec_status_code in ('001', '003') and is_latest = true and product_type = 'AMP'
				)
				group by medication_type_code, rnoh_formulary_statuscd, inpatient_medication_cd, outpatient_medication_cd, prescribing_status_cd, rules_cd, unlicensed_medication_cd, defined_daily_dose, not_for_prn, 
				high_alert_medication, ignore_duplicate_warnings, medusa_preparation_instructions, critical_drug, controlled_drug_category_cd, cytotoxic, clinical_trial_medication, fluid, antibiotic, anticoagulant, 
				antipsychotic, antimicrobial, add_review_reminder, iv_to_oral, titration_type_cd, rounding_factor_cd, max_dose_numerator, maximum_dose_unit_cd, witnessing_required, nice_ta, marked_modifier_cd, insulins, 
				mental_health_drug, basis_of_preferred_name_cd, sugar_free, gluten_free, preservative_free, cfc_free, dose_form_cd, unit_dose_form_size, unit_dose_form_units, unit_dose_unit_of_measure_cd, form_cd, 
				trade_family_cd, modified_release_cd, black_triangle, supplier_cd, current_licensing_authority_cd, ema_additional_monitoring, parallel_import, restrictions_on_availability_cd, drug_class, restriction_note, 
				restricted_prescribing, side_effect, caution, contra_indication, safety_message, custom_warning, endorsement, licensed_use, unlicensed_use, orderable_formtype_cd, trade_family_name, expensive_medication, 
				is_blood_product, is_diluent, is_modified_release, is_gastro_resistant, prescribable, controlled_drug_category_source, black_triangle_source, high_alert_medication_source, prescribable_source, 
				is_custom_controlled_drug, diluent, supplier_name, is_indication_mandatory, reminder, local_licensed_use, local_unlicensed_use
				having count(*) != 2
			) as x) > 0
		) then
      		
			flag = true;
			
			codes := codes || '#' || rec_code.code || ' - Detail';
		
	end if;
     
    if ((select count(*) from 
    		(
    			SELECT 1 FROM local_formulary.formulary_excipient
				where formulary_version_id in (
					select formulary_version_id from local_formulary.formulary_header fh where code = rec_code.code and rec_status_code in ('001', '003') and is_latest = true and product_type = 'AMP'
				)
				group by ingredient_cd, strength, strength_unit_cd
				having count(*) != 2
			) as x) > 0
		) then 
			
		flag = true;	
	
		codes := codes || '#' || rec_code.code || ' - Excipient';
    
	end if;
	
    if ((select count(*) from 
    		(
    			SELECT 1 FROM local_formulary.formulary_additional_code
				where formulary_version_id in (
					select formulary_version_id from local_formulary.formulary_header fh where code = rec_code.code and rec_status_code in ('001', '003') and is_latest = true and product_type = 'AMP'
				)
				group by additional_code, additional_code_system, additional_code_desc, "source", code_type
				having count(*) != 2
			) as x) > 0
	   ) then
      	 
			flag = true;
		
			codes := codes || '#' || rec_code.code || ' - Additional Code';
    
	end if;	
		
	if ((select count(*) from 
			(
				SELECT 1 FROM local_formulary.formulary_ingredient
				where formulary_version_id in (
					select formulary_version_id from local_formulary.formulary_header fh where code = rec_code.code and rec_status_code in ('001', '003') and is_latest = true and product_type = 'AMP'
				)
				group by ingredient_cd, basis_of_pharmaceutical_strength_cd, strength_value_numerator, strength_value_numerator_unit_cd, strength_value_denominator, strength_value_denominator_unit_cd, ingredient_name
				having count(*) != 2
			) as x) > 0
		) then 
	
			flag = true;
		
			codes := codes || '#' || rec_code.code || ' - Ingredient';
	
	end if;	
		
	if ((select count(*) from 
			(
				SELECT 1 FROM local_formulary.formulary_local_route_detail
				where formulary_version_id in (
					select formulary_version_id from local_formulary.formulary_header fh where code = rec_code.code and rec_status_code in ('001', '003') and is_latest = true and product_type = 'AMP'
				)
				group by route_cd, route_field_type_cd, "source"
				having count(*) != 2
			) as x) > 0
		) then 
	
			flag = true;
		
			codes := codes || '#' || rec_code.code || ' - Local Route';
	
	end if;

	if ((select count(*) from 
			(	
				SELECT 1 FROM local_formulary.formulary_route_detail
				where formulary_version_id in (
					select formulary_version_id from local_formulary.formulary_header fh where code = rec_code.code and rec_status_code in ('001', '003') and is_latest = true and product_type = 'AMP'
				)
				group by route_cd, route_field_type_cd, "source"
				having count(*) != 2
			) as x) > 0
		) then 
	
			flag = true;
		
			codes := codes || '#' || rec_code.code || ' - Route';
		
	end if;
	
    if(flag = false) then 
        
        codes := codes || '#' || rec_code.code || ' - No Change';
         
    end if;
     
   end loop;
  
   -- close the cursor
   close cur_codes;

   return codes;
end; $$;


ALTER FUNCTION local_formulary.get_difference_bw_active_draft() OWNER TO owner_name;

--
-- Name: set_vmp_rec_status(); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.set_vmp_rec_status() RETURNS text
    LANGUAGE plpgsql
    AS $$
declare 
	 vmps text default '';
	 rec_vmp   record;
	 cur_vmps cursor 
		 for select code, "name" 
		 from local_formulary.formulary_header
		 where product_type = 'VMP' and is_latest = true --and rec_status_code = '003'
		 order by "name" ;
begin
   -- open the cursor
   open cur_vmps;
	
   loop
    -- fetch row into the vmp
      fetch cur_vmps into rec_vmp;
    -- exit when no more row to fetch
      exit when not found;

    -- build the output
     if ((select '001' = all(select rec_status_code from local_formulary.formulary_header fh where parent_code = rec_vmp.code and is_latest = true)) = true) then
      	 update local_formulary.formulary_header 
         set rec_status_code = '001'
         where code = rec_vmp.code and product_type = 'VMP';
        
        vmps := vmps || ', ' || rec_vmp.code || ' - ' || rec_vmp.name || ' - Draft';
       
    
     elsif ((select '002' = all(select rec_status_code from local_formulary.formulary_header fh where parent_code = rec_vmp.code and is_latest = true)) = true) then
      	 update local_formulary.formulary_header 
         set rec_status_code = '002'
         where code = rec_vmp.code and product_type = 'VMP';
        
        vmps := vmps || ', ' || rec_vmp.code || ' - ' || rec_vmp.name || ' - Ready for review';
       
     
     elsif ((select '003' = any(select rec_status_code from local_formulary.formulary_header fh where parent_code = rec_vmp.code and is_latest = true)) = true) then 
         update local_formulary.formulary_header 
         set rec_status_code = '003'
         where code = rec_vmp.code and product_type = 'VMP';
        
        vmps := vmps || ', ' || rec_vmp.code || ' - ' || rec_vmp.name || ' - Active';
         
       
    elsif ((select '004' = all(select rec_status_code from local_formulary.formulary_header fh where parent_code = rec_vmp.code and is_latest = true)) = true) then
      	 update local_formulary.formulary_header 
         set rec_status_code = '004'
         where code = rec_vmp.code and product_type = 'VMP';
        
        vmps := vmps || ', ' || rec_vmp.code || ' - ' || rec_vmp.name || ' - Archived';
       
    else 
      	 update local_formulary.formulary_header 
         set rec_status_code = '001'
         where code = rec_vmp.code and product_type = 'VMP';
        
        vmps := vmps || ', ' || rec_vmp.code || ' - ' || rec_vmp.name || ' - Draft';
         
      end if;
   end loop;
  
   -- close the cursor
   close cur_vmps;

   return vmps;
end; $$;


ALTER FUNCTION local_formulary.set_vmp_rec_status() OWNER TO owner_name;

--
-- Name: set_vtm_rec_status(); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.set_vtm_rec_status() RETURNS text
    LANGUAGE plpgsql
    AS $$
declare 
	 vtms text default '';
	 rec_vtm   record;
	 cur_vtms cursor 
		 for select code, "name" 
		 from local_formulary.formulary_header
		 where product_type = 'VTM' and is_latest = true --and rec_status_code = '003'
		 order by "name" ;
begin
   -- open the cursor
   open cur_vtms;
	
   loop
    -- fetch row into the vtm
      fetch cur_vtms into rec_vtm;
    -- exit when no more row to fetch
      exit when not found;

    -- build the output
     if ((select '001' = all(select rec_status_code from local_formulary.formulary_header fh where parent_code = rec_vtm.code and is_latest = true)) = true) then
      	 update local_formulary.formulary_header 
         set rec_status_code = '001'
         where code = rec_vtm.code and product_type = 'VTM';
        
        vtms := vtms || ', ' || rec_vtm.code || ' - ' || rec_vtm.name || ' - Draft';
       
    
     elsif ((select '002' = all(select rec_status_code from local_formulary.formulary_header fh where parent_code = rec_vtm.code and is_latest = true)) = true) then
      	 update local_formulary.formulary_header 
         set rec_status_code = '002'
         where code = rec_vtm.code and product_type = 'VTM';
        
        vtms := vtms || ', ' || rec_vtm.code || ' - ' || rec_vtm.name || ' - Ready for review';
       
     
     elsif ((select '003' = any(select rec_status_code from local_formulary.formulary_header fh where parent_code = rec_vtm.code and is_latest = true)) = true) then 
         update local_formulary.formulary_header 
         set rec_status_code = '003'
         where code = rec_vtm.code and product_type = 'VTM';
        
        vtms := vtms || ', ' || rec_vtm.code || ' - ' || rec_vtm.name || ' - Active';
         
       
    elsif ((select '004' = all(select rec_status_code from local_formulary.formulary_header fh where parent_code = rec_vtm.code and is_latest = true)) = true) then
      	 update local_formulary.formulary_header 
         set rec_status_code = '004'
         where code = rec_vtm.code and product_type = 'VTM';
        
        vtms := vtms || ', ' || rec_vtm.code || ' - ' || rec_vtm.name || ' - Archived';
       
    else 
      	 update local_formulary.formulary_header 
         set rec_status_code = '001'
         where code = rec_vtm.code and product_type = 'VTM';
        
        vtms := vtms || ', ' || rec_vtm.code || ' - ' || rec_vtm.name || ' - Draft';
         
      end if;
   end loop;
  
   -- close the cursor
   close cur_vtms;

   return vtms;
end; $$;


ALTER FUNCTION local_formulary.set_vtm_rec_status() OWNER TO owner_name;

--
-- Name: udf_formulary_get_ancestors_by_codes(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_ancestors_by_codes(in_codes text[]) RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean, recstatuscode character varying)
    LANGUAGE sql
    AS $$
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
	  	srcpath,  -- path, stored using an array     
		rec_status_code 
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
			ARRAY[fh.code ]::varchar(1022)[] as srcpath,
			fh.rec_status_code 
		from local_formulary.formulary_header fh 
		inner join unnest(in_codes) m(code) on m.code = fh.code 
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
					ARRAY[fh.code]::varchar(1022)[] as srcpath,
					fh.rec_status_code 
			  	FROM
			         local_formulary.formulary_header fh
			     INNER JOIN search_formulary s 
			     	ON s.parent_code = fh.code
			     	--ON s.code = fh.parent_code
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
		fh.is_duplicate,
		fh.rec_status_code 
	FROM search_formulary fh;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_ancestors_by_codes(in_codes text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_blood_product(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_blood_product(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, bloodproduct character varying, bloodproductmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, is_blood_product::text as bloodproduct, md5(is_blood_product::text) bloodproductmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by is_blood_product, bloodproductmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_blood_product(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_clinical_trial_medication(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_clinical_trial_medication(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, clinicaltrialmedication character varying, clinicaltrialmedicationmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, case clinical_trial_medication when '0' then 'false' when '1' then 'true' else null end as clinicaltrialmedication, md5(clinical_trial_medication) clinicaltrialmedicationmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by clinical_trial_medication, clinicaltrialmedicationmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_clinical_trial_medication(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_compatible_diluent(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_compatible_diluent(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, compatiblediluent character varying, compatiblediluentmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, diluent as compatiblediluent, md5(diluent) compatiblediluentmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by diluent, compatiblediluentmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_compatible_diluent(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_controlled_drug(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_controlled_drug(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, controlleddrug character varying, controlleddrugmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, is_custom_controlled_drug::text as controlleddrug, md5(is_custom_controlled_drug::text) controlleddrugmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by is_custom_controlled_drug, controlleddrugmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_controlled_drug(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_critical_drug(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_critical_drug(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, criticaldrug character varying, criticaldrugmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, case critical_drug when '0' then 'false' when '1' then 'true' else null end as criticaldrug, md5(critical_drug) criticaldrugmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by critical_drug, criticaldrugmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_critical_drug(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_custom_warning(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_custom_warning(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, customwarning character varying, customwarningmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, custom_warning as customwarning, md5(custom_warning) customwarningmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by custom_warning, customwarningmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_custom_warning(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_descendents(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_descendents(in_formulary_version_ids text[]) RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean)
    LANGUAGE sql
    AS $$
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

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_descendents(in_formulary_version_ids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_descendents_by_codes(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_descendents_by_codes(in_codes text[]) RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean, recstatuscode character varying)
    LANGUAGE sql
    AS $$
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
	  	srcpath,  -- path, stored using an array  
  	 	rec_status_code  

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
			ARRAY[fh.code ]::varchar(1022)[] as srcpath,
			fh.rec_status_code 
		from local_formulary.formulary_header fh 
		inner join unnest(in_codes) m(code) on m.code = fh.code 
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
					ARRAY[fh.code]::varchar(1022)[] as srcpath,
					fh.rec_status_code 
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
		fh.is_duplicate,
		fh.rec_status_code
	FROM search_formulary fh;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_descendents_by_codes(in_codes text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_diluent(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_diluent(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, diluent character varying, diluentmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, is_diluent::text as diluent, md5(is_diluent::text) diluentmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by is_diluent, diluentmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_diluent(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_endorsement(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_endorsement(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, endorsement character varying, endorsementmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, endorsement, md5(endorsement) endorsementmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by endorsement, endorsementmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_endorsement(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_expensive_medication(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_expensive_medication(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, expensivemedication character varying, expensivemedicationmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, case expensive_medication when '0' then 'false' when '1' then 'true' else null end as expensivemedication, md5(expensive_medication) expensivemedicationmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by expensive_medication, expensivemedicationmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_expensive_medication(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_formulary_status(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_formulary_status(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, formularystatus character varying, formularystatusmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, rnoh_formulary_statuscd as formularystatus, md5(rnoh_formulary_statuscd) formularystatusmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by rnoh_formulary_statuscd, formularystatusmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_formulary_status(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_gastro_resistant(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_gastro_resistant(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, gastroresistant character varying, gastroresistantmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, is_gastro_resistant::text as gastroresistant, md5(is_gastro_resistant::text) gastroresistantmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by is_gastro_resistant, gastroresistantmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_gastro_resistant(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_high_alert_medication(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_high_alert_medication(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, highalertmedication character varying, highalertmedicationmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, case high_alert_medication when '0' then 'false' when '1' then 'true' else null end as highalertmedication, md5(high_alert_medication) highalertmedicationmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by high_alert_medication, highalertmedicationmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_high_alert_medication(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_ignore_duplicate_warning(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_ignore_duplicate_warning(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, ignoreduplicatewarning character varying, ignoreduplicatewarningmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, case ignore_duplicate_warnings when '0' then 'false' when '1' then 'true' else null end as ignoreduplicatewarning, md5(ignore_duplicate_warnings) ignoreduplicatewarningmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by ignore_duplicate_warnings, ignoreduplicatewarningmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_ignore_duplicate_warning(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_indication_mandatory(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_indication_mandatory(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, indicationmandatory character varying, indicationmandatorymd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, is_indication_mandatory::text as indicationmandatory, md5(is_indication_mandatory::text) indicationmandatorymd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by is_indication_mandatory, indicationmandatorymd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_indication_mandatory(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_iv_to_oral(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_iv_to_oral(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, ivtooral character varying, ivtooralmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, case iv_to_oral when '0' then 'false' when '1' then 'true' else null end as ivtooral, md5(iv_to_oral) ivtooralmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by iv_to_oral, ivtooralmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_iv_to_oral(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_latest_top_nodes(); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_latest_top_nodes() RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean, recstatuscode character varying, rnohformularystatuscd character varying)
    LANGUAGE plpgsql
    AS $$
begin
	RETURN QUERY

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
				detail.rnoh_formulary_statuscd
			from local_formulary.formulary_header fh 
			inner join local_formulary.formulary_detail detail on detail.formulary_version_id  = fh.formulary_version_id 
			where fh.is_latest = true
			and (fh.product_type = 'VTM' or fh.parent_code is null or fh.parent_code = '')
			and (fh.rec_status_code != '006' or fh.rec_status_code is null or fh.rec_status_code = '');
	
end
$$;


ALTER FUNCTION local_formulary.udf_formulary_get_latest_top_nodes() OWNER TO owner_name;

--
-- Name: udf_formulary_get_local_licensed_route(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_local_licensed_route(in_formularyversionids text[]) RETURNS TABLE(formularyversionid character varying, locallicensedroute character varying, locallicensedroutemd5 character varying)
    LANGUAGE sql
    AS $$

select formulary_version_id as formularyversionid, jsonb_agg(route_cd)::text as locallicensedroute, md5(jsonb_agg(route_cd)::text) locallicensedroutemd5 
from local_formulary.formulary_local_route_detail
where route_field_type_cd = '003' and formulary_version_id = any(in_formularyversionids)
group by formulary_version_id;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_local_licensed_route(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_local_licensed_use(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_local_licensed_use(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, locallicenseduse character varying, locallicensedusemd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, local_licensed_use as locallicenseduse, md5(local_licensed_use) locallicensedusemd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by local_licensed_use, locallicensedusemd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_local_licensed_use(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_local_unlicensed_route(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_local_unlicensed_route(in_formularyversionids text[]) RETURNS TABLE(formularyversionid character varying, localunlicensedroute character varying, localunlicensedroutemd5 character varying)
    LANGUAGE sql
    AS $$

select formulary_version_id as formularyversionid, jsonb_agg(route_cd)::text as localunlicensedroute, md5(jsonb_agg(route_cd)::text) as localunlicensedroutemd5 
from local_formulary.formulary_local_route_detail
where route_field_type_cd = '002' and formulary_version_id = any(in_formularyversionids)
group by formulary_version_id

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_local_unlicensed_route(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_local_unlicensed_use(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_local_unlicensed_use(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, localunlicenseduse character varying, localunlicensedusemd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, local_unlicensed_use as localunlicenseduse, md5(local_unlicensed_use) localunlicensedusemd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by local_unlicensed_use, localunlicensedusemd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_local_unlicensed_use(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_medusa_preparation_instruction(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_medusa_preparation_instruction(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, medusapreparationinstruction character varying, medusapreparationinstructionmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, medusa_preparation_instructions as medusapreparationinstruction, md5(medusa_preparation_instructions) medusapreparationinstructionmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by medusapreparationinstruction, medusapreparationinstructionmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_medusa_preparation_instruction(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_modified_release(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_modified_release(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, modifiedrelease character varying, modifiedreleasemd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, is_modified_release::text as modifiedrelease, md5(is_modified_release::text) modifiedreleasemd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by is_modified_release, modifiedreleasemd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_modified_release(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_next_descendents_by_codes(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_next_descendents_by_codes(in_codes text[]) RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean, recstatuscode character varying)
    LANGUAGE sql
    AS $$
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
			fh.rec_status_code 
		from local_formulary.formulary_header fh 
		inner join unnest(in_codes) m(code) on m.code = fh.parent_code 
		where fh.is_latest = true and (fh.rec_status_code != '006' or fh.rec_status_code is null or fh.rec_status_code = '');
		

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_next_descendents_by_codes(in_codes text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_not_for_prn(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_not_for_prn(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, notforprn character varying, notforprnmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, case not_for_prn when '0' then 'false' when '1' then 'true' else null end as notforprn, md5(not_for_prn) notforprnmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by not_for_prn, notforprnmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_not_for_prn(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_outpatient_medication(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_outpatient_medication(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, outpatientmedication character varying, outpatientmedicationmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, case outpatient_medication_cd when '0' then 'false' when '1' then 'true' else null end as outpatientmedication, md5(outpatient_medication_cd) outpatientmedicationmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by outpatient_medication_cd, outpatientmedicationmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_outpatient_medication(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_prescribable(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_prescribable(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, prescribable character varying, prescribablemd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, prescribable::text as prescribable, md5(prescribable::text) prescribablemd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by prescribable, prescribablemd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_prescribable(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_prescription_printing_required(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_prescription_printing_required(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, prescriptionprintingrequired character varying, prescriptionprintingrequiredmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, is_prescription_printing_required::text as prescriptionprintingrequired, md5(is_prescription_printing_required::text) prescriptionprintingrequiredmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by is_prescription_printing_required, prescriptionprintingrequiredmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_prescription_printing_required(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_reminder(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_reminder(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, reminder character varying, remindermd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, reminder, md5(reminder) remindermd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by reminder, remindermd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_reminder(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_rounding_factor(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_rounding_factor(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, roundingfactor character varying, roundingfactormd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, rounding_factor_cd as roundingfactor, md5(rounding_factor_cd) roundingfactormd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by rounding_factor_cd, roundingfactormd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_rounding_factor(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_titration_type(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_titration_type(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, titrationtype character varying, titrationtypemd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, titration_type_cd as titrationtype, md5(titration_type_cd) titrationtypemd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by titration_type_cd, titrationtypemd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_titration_type(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_get_witnessing_required(text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_get_witnessing_required(in_formularyversionids text[]) RETURNS TABLE(formularycount bigint, witnessingrequired character varying, witnessingrequiredmd5 character varying)
    LANGUAGE sql
    AS $$

select count(formulary_version_id) as formularycount, case witnessing_required when '0' then 'false' when '1' then 'true' else null end as witnessingrequired, md5(witnessing_required) witnessingrequiredmd5 
from local_formulary.formulary_detail
where formulary_version_id = any(in_formularyversionids)
group by witnessing_required, witnessingrequiredmd5;

$$;


ALTER FUNCTION local_formulary.udf_formulary_get_witnessing_required(in_formularyversionids text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_process_epf(); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_process_epf() RETURNS text
    LANGUAGE plpgsql
    AS $$
declare 
	 processedcode text default '';
	 rec_med record;
	 vmps record;
	 vtms record;
	 cur_meds cursor 
	 for select code, "name", "level", formulary_status, critical_drug, indication_mandatory, unlicensed_route_code, expensive_medication
	 from local_formulary.excelimport
	 order by "level", "name" ;

begin
	
   CREATE TEMPORARY TABLE temp_amps_to_ignore_for_processing(ampcode varchar UNIQUE);
	
   CREATE TEMPORARY TABLE temp_vmps_to_ignore_for_processing(vmpcode varchar UNIQUE);
    
   CREATE TEMPORARY TABLE temp_vtms_to_ignore_for_processing(vtmcode varchar UNIQUE);
  
  
  --set all amps to non prescribable initially
update local_formulary.formulary_detail 
set prescribable = false, prescribable_source = 'Local'
where formulary_version_id in (select formulary_version_id from local_formulary.formulary_header where product_type = 'AMP' and is_latest = true);
  
  --set all amps from ePF to prescribable
 update local_formulary.formulary_detail 
 set prescribable = true, prescribable_source = 'Local'
 where formulary_version_id in (select formulary_version_id from local_formulary.formulary_header where code in (select code from local_formulary.excelimport where "level" = 'AMP') and product_type = 'AMP' and is_latest = true);
  
  --set all amps from MMC to prescribable whose prescribing status is in (3, 4, 5, 9)
  UPDATE local_formulary.formulary_detail
  SET prescribable = case WHEN prescribing_status_cd = '3' OR prescribing_status_cd = '4' or prescribing_status_cd = '5' or prescribing_status_cd = '9' THEN true end, prescribable_source = 'Local'
  where formulary_version_id in (select formulary_version_id from local_formulary.formulary_header where product_type = 'AMP' and is_latest = true);

 -- open the cursor
   open cur_meds;
 	
   loop
    -- fetch row
      fetch cur_meds into rec_med;
    -- exit when no more row to fetch
      exit when not found;

    --start processing AMP
      if rec_med."level" = 'AMP' then 
      
        if rec_med.unlicensed_route_code != '' then
       		INSERT INTO local_formulary.formulary_route_detail
			("_row_id", "_createdtimestamp", "_createddate", "_createdby", "_timezonename", "_timezoneoffset", "_tenant", formulary_version_id, route_cd, route_field_type_cd, "_updatedtimestamp", "_updateddate", "_updatedby", "source")
			VALUES(uuid_generate_v4(), NOW(), NOW(), 'RNOHT\Gautam.Bhatt', NULL, NULL, NULL, (select formulary_version_id from local_formulary.formulary_header fh where code = rec_med.code and is_latest = true and product_type = 'AMP'), rec_med.unlicensed_route_code, '002', NOW(), NOW(), 'RNOHT\Gautam.Bhatt', NULL);
        end if;
      
        if rec_med.expensive_medication = 'Yes' then 
        	 update local_formulary.formulary_detail 
	         set expensive_medication = '1'
	         where formulary_version_id in (select formulary_version_id from local_formulary.formulary_header fh where code = rec_med.code and is_latest = true and product_type = 'AMP');
        end if;
       
       update local_formulary.formulary_detail 
  	   set prescribable = true 
  	   where formulary_version_id in (select formulary_version_id from local_formulary.formulary_header where code = rec_med.code and product_type = 'AMP' and is_latest = true);
       
       INSERT INTO temp_amps_to_ignore_for_processing(ampcode) select code from local_formulary.formulary_header fh where code = rec_med.code and is_latest = true and product_type = 'AMP' 
       ON CONFLICT (ampcode) DO NOTHING;
       
       INSERT INTO temp_vmps_to_ignore_for_processing(vmpcode) select parent_code from local_formulary.formulary_header fh where code = rec_med.code and is_latest = true and product_type = 'AMP' 
       ON CONFLICT (vmpcode) DO NOTHING;
       
      processedcode := processedcode || rec_med.code || ' : AMP Code, ';
      
      end if;
     --end processing AMP
     
     --start processing VMP
     if rec_med."level" = 'VMP' then 
     	
     	if rec_med.unlicensed_route_code != '' then
        	
        	for vmps in select formulary_version_id from local_formulary.formulary_header fh 
        		where parent_code = rec_med.code and is_latest = true and product_type = 'AMP' 
	         	--ignore amp's which are already processed
	         	and code not in (select ampcode from temp_amps_to_ignore_for_processing)
	         	--ignore vmp's of already processed amp's
	         	and parent_code not in (select vmpcode from temp_vmps_to_ignore_for_processing)
	         	order by formulary_version_id 
	         
	         loop
	         	
	         	INSERT INTO local_formulary.formulary_route_detail
				("_row_id", "_createdtimestamp", "_createddate", "_createdby", "_timezonename", "_timezoneoffset", "_tenant", formulary_version_id, route_cd, route_field_type_cd, "_updatedtimestamp", "_updateddate", "_updatedby", "source")
				VALUES(uuid_generate_v4(), NOW(), NOW(), 'RNOHT\Gautam.Bhatt', NULL, NULL, NULL, vmps.formulary_version_id, rec_med.unlicensed_route_code, '002', NOW(), NOW(), 'RNOHT\Gautam.Bhatt', NULL);
	         
	         end loop;
        	
       		
        end if;
      
        if rec_med.expensive_medication = 'Yes' then 
        	 update local_formulary.formulary_detail 
	         set expensive_medication = '1'
	         where formulary_version_id in (
	         select formulary_version_id from local_formulary.formulary_header fh where parent_code = rec_med.code and is_latest = true and product_type = 'AMP' 
	         --ignore amp's which are already processed
	         and code not in (select ampcode from temp_amps_to_ignore_for_processing)
	         --ignore vmp's of already processed amp's
	         and parent_code not in (select vmpcode from temp_vmps_to_ignore_for_processing)
	         );
        end if;
       
       INSERT INTO temp_amps_to_ignore_for_processing(ampcode) 
       select code from local_formulary.formulary_header fh 
       where parent_code = rec_med.code and is_latest = true and product_type = 'AMP' 
       --ignore amp's which are already processed
       and code not in (select ampcode from temp_amps_to_ignore_for_processing) 
       --ignore vmp's of already processed amp's
       and parent_code not in (select vmpcode from temp_vmps_to_ignore_for_processing)
       ON CONFLICT (ampcode) DO NOTHING;
       
       INSERT INTO temp_vmps_to_ignore_for_processing(vmpcode) 
       select code from local_formulary.formulary_header fh 
       where code = rec_med.code and is_latest = true and product_type = 'VMP'
       --ignore vmp's of already processed amp's
       and code not in (select vmpcode from temp_vmps_to_ignore_for_processing)
       ON CONFLICT (vmpcode) DO NOTHING;
      
      INSERT INTO temp_vtms_to_ignore_for_processing(vtmcode) 
       select parent_code from local_formulary.formulary_header fh 
       where code = rec_med.code and is_latest = true and product_type = 'VMP'
       --ignore vmp's of already processed amp's
       and code not in (select vmpcode from temp_vmps_to_ignore_for_processing)
       ON CONFLICT (vtmcode) DO NOTHING;
      
      processedcode := processedcode || rec_med.code || ' : VMP Code, ';
      
     end if;
     --end processing VMP
    
    --start processing VTM
    if rec_med."level" = 'VTM' then
    
    	if rec_med.unlicensed_route_code != '' then
        
        	for vtms in select fh2.formulary_version_id from local_formulary.formulary_header fh2 where fh2.parent_code in (
		         select code from local_formulary.formulary_header fh where parent_code = rec_med.code and is_latest = true and product_type = 'VMP' 
		         --ignore vmp's of already processed amp's
		         and code not in (select vmpcode from temp_vmps_to_ignore_for_processing)
		         --ignore vtm's of already processed vmp's
		         and parent_code not in (select vtmcode from temp_vtms_to_ignore_for_processing)
		         )
	         	and fh2.is_latest = true and fh2.product_type = 'AMP'
	         	--ignore amp's which are already processed
	         	and fh2.code not in (select ampcode from temp_amps_to_ignore_for_processing)
	         	order by fh2.formulary_version_id 
        
        	loop
	         	
	         	INSERT INTO local_formulary.formulary_route_detail
				("_row_id", "_createdtimestamp", "_createddate", "_createdby", "_timezonename", "_timezoneoffset", "_tenant", formulary_version_id, route_cd, route_field_type_cd, "_updatedtimestamp", "_updateddate", "_updatedby", "source")
				VALUES(uuid_generate_v4(), NOW(), NOW(), 'RNOHT\Gautam.Bhatt', NULL, NULL, NULL, vtms.formulary_version_id, rec_med.unlicensed_route_code, '002', NOW(), NOW(), 'RNOHT\Gautam.Bhatt', NULL);
	         
	         end loop;

        end if;
      
        if rec_med.expensive_medication = 'Yes' then 
        	 update local_formulary.formulary_detail 
	         set expensive_medication = '1'
	         where formulary_version_id in (
	         select formulary_version_id from local_formulary.formulary_header fh2 where fh2.parent_code in (
		         select code from local_formulary.formulary_header fh where parent_code = rec_med.code and is_latest = true and product_type = 'VMP' 
		         --ignore vmp's of already processed amp's
		         and code not in (select vmpcode from temp_vmps_to_ignore_for_processing)
		         --ignore vtm's of already processed vmp's
		         and parent_code not in (select vtmcode from temp_vtms_to_ignore_for_processing)
		         )
	         and fh2.is_latest = true and fh2.product_type = 'AMP'
	         --ignore amp's which are already processed
	         and fh2.code not in (select ampcode from temp_amps_to_ignore_for_processing)
	        );
        end if;
       
       processedcode := processedcode || rec_med.code || ' : VTM Code, ';
       
    end if;
    --end processing VTM
   end loop;
     
   -- close the cursor
   close cur_meds;
  
   drop table temp_amps_to_ignore_for_processing;
   drop table temp_vmps_to_ignore_for_processing;
   drop table temp_vtms_to_ignore_for_processing;
  
   return processedcode;
end; $$;


ALTER FUNCTION local_formulary.udf_formulary_process_epf() OWNER TO owner_name;

--
-- Name: udf_formulary_process_formulary_status(); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_process_formulary_status() RETURNS text
    LANGUAGE plpgsql
    AS $$
declare 
	 processedcode text default '';
	 rec_med record;
	 vmps record;
	 vtms record;
	 cur_meds cursor 
	 for select code, "name", "level", formulary_status, critical_drug, indication_mandatory, unlicensed_route_code, expensive_medication
	 from local_formulary.excelimport
	 order by "level", "name" ;

begin
  
--set all amps to non formulary
update local_formulary.formulary_detail 
set rnoh_formulary_statuscd = '002'
where formulary_version_id in (select formulary_version_id from local_formulary.formulary_header where product_type = 'AMP' and is_latest = true);
  
  
 -- open the cursor
   open cur_meds;
 	
   loop
    -- fetch row
      fetch cur_meds into rec_med;
    -- exit when no more row to fetch
      exit when not found;

    --start processing AMP
      if rec_med."level" = 'AMP' then 
      
        if rec_med.formulary_status = 'Yes' then
	         update local_formulary.formulary_detail 
	         set rnoh_formulary_statuscd = '001'
	         where formulary_version_id in (select formulary_version_id from local_formulary.formulary_header fh where code = rec_med.code and is_latest = true and product_type = 'AMP');
        end if;

      	
       
      processedcode := processedcode || rec_med.code || ' : AMP Code, ';
      
      end if;
     --end processing AMP
     
     --start processing VMP
     if rec_med."level" = 'VMP' then 
     	
     	if rec_med.formulary_status = 'Yes' then
	        if not exists (select 1 from local_formulary.excelimport where "level" = 'AMP' and code in (select code from local_formulary.formulary_header where parent_code = rec_med.code and is_latest = true and product_type = 'AMP')) then
				update local_formulary.formulary_detail 
				set rnoh_formulary_statuscd = '001'
				where formulary_version_id in (select formulary_version_id from local_formulary.formulary_header fh where parent_code = rec_med.code and is_latest = true and product_type = 'AMP');
			end if;
        end if;
     
     	
      processedcode := processedcode || rec_med.code || ' : VMP Code, ';
      
     end if;
     --end processing VMP
    
    --start processing VTM
    if rec_med."level" = 'VTM' then
    
    	if rec_med.formulary_status = 'Yes' then
	         if not exists (select 1 from local_formulary.excelimport where "level" = 'VMP' and code in (select code from local_formulary.formulary_header where parent_code = rec_med.code and is_latest = true and product_type = 'VMP')) then
				update local_formulary.formulary_detail 
				set rnoh_formulary_statuscd = '001'
				where formulary_version_id in (
					select formulary_version_id from local_formulary.formulary_header 
					where parent_code in 
					(
						select code from local_formulary.formulary_header where parent_code = rec_med.code and is_latest = true and product_type = 'VMP'
					)
					and product_type = 'AMP' and is_latest = true
				);
			end if;
        end if;
    
    	
       
       processedcode := processedcode || rec_med.code || ' : VTM Code, ';
       
    end if;
    --end processing VTM
   end loop;
     
   -- close the cursor
   close cur_meds;
  

  
   return processedcode;
end; $$;


ALTER FUNCTION local_formulary.udf_formulary_process_formulary_status() OWNER TO owner_name;

--
-- Name: udf_formulary_search_amp_by_attributes(text, text, text[], text[]); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_search_amp_by_attributes(in_name text DEFAULT NULL::text, in_search_code text DEFAULT NULL::text, in_recordstatus_codes text[] DEFAULT NULL::text[], in_rnoh_formulary_status_codes text[] DEFAULT NULL::text[]) RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean, recstatuscode text, rnohformularystatuscd text, prescribable boolean)
    LANGUAGE sql
    AS $$
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
	             detail.prescribable 
              from local_formulary.formulary_header fh 
              inner join local_formulary.formulary_detail detail on detail.formulary_version_id  = fh.formulary_version_id 
              where fh.is_latest = true
				and (fh.product_type = 'AMP')
				and (in_name is null or fh.name_tokens @@ to_tsquery(in_name))
				and (in_search_code is null or fh.code = in_search_code)
				and (in_recordstatus_codes is null or fh.rec_status_code = any(in_recordstatus_codes))
				and (in_rnoh_formulary_status_codes is null or detail.rnoh_formulary_statuscd = any(in_rnoh_formulary_status_codes))
 
$$;


ALTER FUNCTION local_formulary.udf_formulary_search_amp_by_attributes(in_name text, in_search_code text, in_recordstatus_codes text[], in_rnoh_formulary_status_codes text[]) OWNER TO owner_name;

--
-- Name: udf_formulary_search_nodes_with_descendents(text, text); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_formulary_search_nodes_with_descendents(in_name text DEFAULT NULL::text, in_search_code text DEFAULT NULL::text) RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean, recstatuscode text, rnohformularystatuscd text, prescribable boolean)
    LANGUAGE sql
    AS $$
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
		prescribable,
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
			detail.prescribable,
			ARRAY[fh.code ]::varchar(1022)[] as srcpath
		from local_formulary.formulary_header fh 
		inner join local_formulary.formulary_detail detail on detail.formulary_version_id  = fh.formulary_version_id 
		where fh.is_latest = true
		--and (in_name is null or fh.name ilike '%'|| in_name ||'%' or fh.name ilike in_name || '%' or fh.name ilike '%'|| in_name )
		--and (in_name is null or (to_tsvector(fh.name) @@ to_tsquery(in_name)))
		--and (in_name is null or fh.name_tokens @@ to_tsquery(in_name))
		and (in_name is null or to_tsvector('simple', fh.name) @@ to_tsquery('simple', in_name))
		and (in_search_code is null or fh.code = in_search_code)
		--and (in_recordstatus_codes is null or fh.rec_status_code = any(in_recordstatus_codes))
		--and (in_rnoh_formulary_status_codes is null or detail.rnoh_formulary_statuscd = any(in_rnoh_formulary_status_codes))
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
					detail.prescribable,
					ARRAY[fh.code ]::varchar(1022)[] as srcpath
			  	FROM
			         local_formulary.formulary_header fh
			     INNER JOIN search_formulary s 
			     	ON s.code = fh.parent_code
	      			AND (fh.code <> ALL(s.srcpath))        -- prevent from cycling 
  				inner join local_formulary.formulary_detail detail on detail.formulary_version_id  = fh.formulary_version_id 
		      	where fh.is_latest = true
				--and (in_recordstatus_codes is null or fh.rec_status_code = any(in_recordstatus_codes))
				--and (in_rnoh_formulary_status_codes is null or detail.rnoh_formulary_statuscd = any(in_rnoh_formulary_status_codes))
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
		fh.rnoh_formulary_statuscd,
		fh.prescribable
	FROM search_formulary fh;

$$;


ALTER FUNCTION local_formulary.udf_formulary_search_nodes_with_descendents(in_name text, in_search_code text) OWNER TO owner_name;

--
-- Name: udf_update_formulary_record_changes(); Type: FUNCTION; Schema: local_formulary; Owner: owner_name
--

CREATE FUNCTION local_formulary.udf_update_formulary_record_changes() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
	IF NEW."name" is not NULL THEN
		update local_formulary.formulary_header
		set name_tokens = to_tsvector('english'::regconfig, "name")
		where "code" = new."code";
	END IF;

	RETURN NEW;
END;
$$;


ALTER FUNCTION local_formulary.udf_update_formulary_record_changes() OWNER TO owner_name;

--
-- Name: udf_dmd_get_all_ancestors(text[]); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_dmd_get_all_ancestors(in_codes text[]) RETURNS TABLE(code character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, parentcode character varying, logicallevel integer)
    LANGUAGE sql
    AS $$
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
   
$$;


ALTER FUNCTION terminology.udf_dmd_get_all_ancestors(in_codes text[]) OWNER TO owner_name;

--
-- Name: udf_dmd_get_all_descendents(text[]); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_dmd_get_all_descendents(in_codes text[]) RETURNS TABLE(code character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, parentcode character varying, logicallevel integer)
    LANGUAGE sql
    AS $$
      
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
$$;


ALTER FUNCTION terminology.udf_dmd_get_all_descendents(in_codes text[]) OWNER TO owner_name;

--
-- Name: udf_dmd_get_ancestor_nodes_search(text, character varying); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_dmd_get_ancestor_nodes_search(in_name text, in_code character varying) RETURNS TABLE(code character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, parentcode character varying, logicallevel integer)
    LANGUAGE sql
    AS $$
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
   
$$;


ALTER FUNCTION terminology.udf_dmd_get_ancestor_nodes_search(in_name text, in_code character varying) OWNER TO owner_name;

--
-- Name: udf_dmd_get_child_nodes_search(text, character varying); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_dmd_get_child_nodes_search(in_name text, in_code character varying) RETURNS TABLE(code character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, parentcode character varying, logicallevel integer)
    LANGUAGE sql
    AS $$
      
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
                           --where (dmd_name.name_tokens @@ to_tsquery(in_name) or dmd_name.code = in_code)
                           where (to_tsvector('simple', dmd_name.name) @@ to_tsquery('simple', in_name) or dmd_name.code = in_code)
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
$$;


ALTER FUNCTION terminology.udf_dmd_get_child_nodes_search(in_name text, in_code character varying) OWNER TO owner_name;

--
-- Name: udf_dmd_get_next_ancestor(text[]); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_dmd_get_next_ancestor(in_codes text[]) RETURNS TABLE(code character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, parentcode character varying, logicallevel integer)
    LANGUAGE sql
    AS $$
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
 
             
$$;


ALTER FUNCTION terminology.udf_dmd_get_next_ancestor(in_codes text[]) OWNER TO owner_name;

--
-- Name: udf_dmd_get_next_descendent(text[]); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_dmd_get_next_descendent(in_codes text[]) RETURNS TABLE(code character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, parentcode character varying, logicallevel integer)
    LANGUAGE sql
    AS $$
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
             
$$;


ALTER FUNCTION terminology.udf_dmd_get_next_descendent(in_codes text[]) OWNER TO owner_name;

--
-- Name: udf_dmd_get_nodes_by_codes(text[]); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_dmd_get_nodes_by_codes(in_codes text[]) RETURNS TABLE(code character varying, prevcode character varying, name character varying, routecode character varying, route character varying, formcode character varying, form character varying, suppliercode character varying, supplier character varying, prescribingstatuscode bigint, prescribingstatus character varying, controldrugcategorycode bigint, controldrugcategory character varying, ingredientsubstanceid character varying, basispharmaceuticalstrengthcd bigint, basisstrengthsubstanceid character varying, strengthvaluenmtrunitcd character varying, strengthvalnmtr numeric, strengthvaluednmtrunitcd character varying, strengthvaldnmtr numeric, basiscd character varying, cfcf character varying, gluf character varying, presf character varying, sugf character varying, udfs numeric, udfsuomcd character varying, unitdoseuomcd character varying, dfindcd character varying, ema character varying, licauthcd character varying, parallelimport character varying, availrestrictcd character varying, ontcd bigint, parentcode character varying, logicallevel integer)
    LANGUAGE sql
    AS $$
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
             
$$;


ALTER FUNCTION terminology.udf_dmd_get_nodes_by_codes(in_codes text[]) OWNER TO owner_name;

--
-- Name: udf_snomed_get_ancestor_nodes_search_term_by_tag(text, text, text); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_snomed_get_ancestor_nodes_search_term_by_tag(in_searchtext text, in_conceptid text, in_semantictag text) RETURNS TABLE(conceptid character varying, preferredterm text, parentid character varying, fsn text, level integer)
    LANGUAGE sql
    AS $$


WITH RECURSIVE search_snomed(      
	  conceptid,
	  preferredTerm,
	  parentId,
	  fsn,
	  level,  -- depth, starting from 1      
	  srcpath  -- path, stored using an array      
	) as (
				select distinct 
					clkp.conceptid,
					clkp.preferredTerm,
					r.destinationid as parentid,
					clkp.fsn,
					1 as level,
					ARRAY[clkp.conceptid ]::varchar(1022)[] as srcpath
				from terminology.snomedct_concept_lookup_mat clkp
				inner join terminology.snomedct_relation_active_isa_lookup_mat r on r.sourceid = clkp.conceptid
				where ((clkp.preferredname_tokens @@ to_tsquery(in_searchText)) or (clkp.conceptid = in_conceptId))
				and clkp.fsn ilike '%% ' || '('|| in_semanticTag || ')' 
			UNION all
			
				select 
					clkp.conceptid,
					clkp.preferredTerm,
					r.destinationid as parentId,
					clkp.fsn,
					-1 as level,
					--sg.level + 1 as level,
					(sg.srcpath || r.sourceid )::varchar(1022)[] as srcpath
			  	FROM
			         terminology.snomedct_relation_active_isa_lookup_mat r
			     INNER JOIN search_snomed sg 
			     	ON sg.parentid = r.sourceid 
			      	AND (r.sourceid <> ALL(sg.srcpath))        -- prevent from cycling 
			      	--AND sg.level <= 1 --(Total levels will be <level> + 1 )
			     inner join terminology.snomedct_concept_lookup_mat clkp on clkp.conceptid = r.sourceid
		)
	
	SELECT distinct
		conceptid,
		preferredTerm,
		parentId,
		fsn,
		level  -- depth, starting from 1      
		--srcpath  -- path, stored using an array  
	FROM search_snomed;


$$;


ALTER FUNCTION terminology.udf_snomed_get_ancestor_nodes_search_term_by_tag(in_searchtext text, in_conceptid text, in_semantictag text) OWNER TO owner_name;

--
-- Name: udf_snomed_get_child_nodes_search_term_by_tag(text, text, text); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_snomed_get_child_nodes_search_term_by_tag(in_searchtext text, in_conceptid text, in_semantictag text) RETURNS TABLE(conceptid character varying, preferredterm text, parentid character varying, fsn text, level integer)
    LANGUAGE sql
    AS $$
	
   WITH RECURSIVE search_snomed(      
	  conceptid,
	  preferredTerm,
	  parentId,
	  fsn,
	  level,  -- depth, starting from 1      
	  srcpath  -- path, stored using an array      
	) as (
			select distinct 
					clkp.conceptid,
					clkp.preferredTerm,
					null as parentid,
					--r.destinationid as parentid,
					clkp.fsn,
					1 as level,
					ARRAY[clkp.conceptid ]::varchar(1022)[] as srcpath
				from terminology.snomedct_concept_lookup_mat clkp
				inner join terminology.snomedct_relation_active_isa_lookup_mat r on r.sourceid = clkp.conceptid
				where ((clkp.preferredname_tokens @@ to_tsquery(in_searchText)) or (clkp.conceptid = in_conceptId))
				and clkp.fsn ilike '%% ' || '('|| in_semanticTag || ')' --'% (disorder)'

				--and clkp.semantictag = 'disorder'
			
			UNION all
				select 
					clkp.conceptid,
					clkp.preferredTerm,
					r.destinationid as parentId,
					clkp.fsn,
					-1 as level,
					--s.level + 1 as level,
					(s.srcpath || r.sourceid)::varchar(1022)[] as srcpath
			  	FROM
			         terminology.snomedct_relation_active_isa_lookup_mat r
			     INNER JOIN search_snomed s 
			     	ON s.conceptid = r.destinationid
			      	AND (r.sourceid <> ALL(s.srcpath))        -- prevent from cycling 
			      	--AND s.level <= 15 --(Total levels will be <level> + 1 )
			     inner join terminology.snomedct_concept_lookup_mat clkp on clkp.conceptid = r.sourceid 
		)
	
	SELECT distinct
		conceptid,
		preferredTerm,
		parentId,
		fsn,
		level  -- depth, starting from 1      
		--path  -- path, stored using an array  
	FROM search_snomed 
$$;


ALTER FUNCTION terminology.udf_snomed_get_child_nodes_search_term_by_tag(in_searchtext text, in_conceptid text, in_semantictag text) OWNER TO owner_name;

--
-- Name: udf_snomed_get_next_ancestors(text[]); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_snomed_get_next_ancestors(in_conceptids text[]) RETURNS TABLE(conceptid character varying, preferredterm text, parentid character varying, fsn text, level integer)
    LANGUAGE sql
    AS $$
	select distinct 
		clkp.conceptId,
		clkp.preferredTerm,
		r.destinationid as parentId,
		clkp.fsn,
		1 as level
	from terminology.snomedct_concept_lookup_mat clkp
	inner join unnest(in_conceptIds) m(conceptid) on m.conceptid = clkp.conceptid
	inner join terminology.snomedct_relation_active_isa_lookup_mat r on r.sourceid = clkp.conceptid
	--where  array_length(in_conceptIds, 1) is null or clkp.conceptid  = any(in_conceptIds);
	union all
	select distinct 
		clkp_parent.conceptId,
		clkp_parent.preferredTerm,
		r_parent.destinationid as parentId,
		clkp_parent.fsn,
		-1 as level
	from (select 
			clkp.conceptid,
			clkp.preferredTerm,
			r.destinationid as parentid,
			clkp.fsn
	from terminology.snomedct_relation_active_isa_lookup_mat r
	inner join unnest(in_conceptIds) m(conceptid) on m.conceptid = r.sourceid 
	inner join terminology.snomedct_concept_lookup_mat clkp on clkp.conceptid = r.destinationid)
		as base
	inner join terminology.snomedct_relation_active_isa_lookup_mat r_parent on base.parentid = r_parent.sourceid 
	inner join terminology.snomedct_concept_lookup_mat clkp_parent on clkp_parent.conceptid = r_parent.sourceid 

$$;


ALTER FUNCTION terminology.udf_snomed_get_next_ancestors(in_conceptids text[]) OWNER TO owner_name;

--
-- Name: udf_snomed_get_next_descendents(text[]); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_snomed_get_next_descendents(in_conceptids text[]) RETURNS TABLE(conceptid character varying, preferredterm text, parentid character varying, fsn text, level integer)
    LANGUAGE sql
    AS $$
	select distinct 
		clkp.conceptId,
		clkp.preferredTerm,
		r.destinationid as parentId,
		clkp.fsn,
		1 as level
	from terminology.snomedct_concept_lookup_mat clkp
	inner join unnest(in_conceptIds) m(conceptid) on m.conceptid = clkp.conceptid
	inner join terminology.snomedct_relation_active_isa_lookup_mat r on r.sourceid = clkp.conceptid
	--where  array_length(in_conceptIds, 1) is null or clkp.conceptid  = any(in_conceptIds);
	union all
	select distinct 
		clkp.conceptid,
		clkp.preferredTerm,
		r.destinationid as parentid,
		clkp.fsn,
		-1 as level
	from terminology.snomedct_concept_lookup_mat clkp
	inner join terminology.snomedct_relation_active_isa_lookup_mat r on r.sourceid = clkp.conceptid
	inner join unnest(in_conceptIds) m(conceptid) on m.conceptid = r.destinationid 
$$;


ALTER FUNCTION terminology.udf_snomed_get_next_descendents(in_conceptids text[]) OWNER TO owner_name;

--
-- Name: udf_tg_dmd_amp_drugroute_hash_update(); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_tg_dmd_amp_drugroute_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology.udf_tg_dmd_amp_drugroute_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_amp_excipient_hash_update(); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_tg_dmd_amp_excipient_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology.udf_tg_dmd_amp_excipient_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_amp_hash_update(); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_tg_dmd_amp_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology.udf_tg_dmd_amp_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vmp_controldrug_hash_update(); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_tg_dmd_vmp_controldrug_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology.udf_tg_dmd_vmp_controldrug_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vmp_drugform_hash_update(); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_tg_dmd_vmp_drugform_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology.udf_tg_dmd_vmp_drugform_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vmp_drugroute_hash_update(); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_tg_dmd_vmp_drugroute_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology.udf_tg_dmd_vmp_drugroute_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vmp_hash_update(); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_tg_dmd_vmp_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology.udf_tg_dmd_vmp_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vmp_ingredient_hash_update(); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_tg_dmd_vmp_ingredient_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology.udf_tg_dmd_vmp_ingredient_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vmp_ontdrugform_hash_update(); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_tg_dmd_vmp_ontdrugform_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology.udf_tg_dmd_vmp_ontdrugform_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vtm_hash_update(); Type: FUNCTION; Schema: terminology; Owner: owner_name
--

CREATE FUNCTION terminology.udf_tg_dmd_vtm_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology.udf_tg_dmd_vtm_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_amp_drugroute_hash_update(); Type: FUNCTION; Schema: terminology_staging; Owner: owner_name
--

CREATE FUNCTION terminology_staging.udf_tg_dmd_amp_drugroute_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology_staging.udf_tg_dmd_amp_drugroute_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_amp_excipient_hash_update(); Type: FUNCTION; Schema: terminology_staging; Owner: owner_name
--

CREATE FUNCTION terminology_staging.udf_tg_dmd_amp_excipient_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology_staging.udf_tg_dmd_amp_excipient_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_amp_hash_update(); Type: FUNCTION; Schema: terminology_staging; Owner: owner_name
--

CREATE FUNCTION terminology_staging.udf_tg_dmd_amp_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology_staging.udf_tg_dmd_amp_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vmp_controldrug_hash_update(); Type: FUNCTION; Schema: terminology_staging; Owner: owner_name
--

CREATE FUNCTION terminology_staging.udf_tg_dmd_vmp_controldrug_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology_staging.udf_tg_dmd_vmp_controldrug_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vmp_drugform_hash_update(); Type: FUNCTION; Schema: terminology_staging; Owner: owner_name
--

CREATE FUNCTION terminology_staging.udf_tg_dmd_vmp_drugform_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology_staging.udf_tg_dmd_vmp_drugform_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vmp_drugroute_hash_update(); Type: FUNCTION; Schema: terminology_staging; Owner: owner_name
--

CREATE FUNCTION terminology_staging.udf_tg_dmd_vmp_drugroute_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology_staging.udf_tg_dmd_vmp_drugroute_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vmp_hash_update(); Type: FUNCTION; Schema: terminology_staging; Owner: owner_name
--

CREATE FUNCTION terminology_staging.udf_tg_dmd_vmp_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology_staging.udf_tg_dmd_vmp_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vmp_ingredient_hash_update(); Type: FUNCTION; Schema: terminology_staging; Owner: owner_name
--

CREATE FUNCTION terminology_staging.udf_tg_dmd_vmp_ingredient_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology_staging.udf_tg_dmd_vmp_ingredient_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vmp_ontdrugform_hash_update(); Type: FUNCTION; Schema: terminology_staging; Owner: owner_name
--

CREATE FUNCTION terminology_staging.udf_tg_dmd_vmp_ontdrugform_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology_staging.udf_tg_dmd_vmp_ontdrugform_hash_update() OWNER TO owner_name;

--
-- Name: udf_tg_dmd_vtm_hash_update(); Type: FUNCTION; Schema: terminology_staging; Owner: owner_name
--

CREATE FUNCTION terminology_staging.udf_tg_dmd_vtm_hash_update() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION terminology_staging.udf_tg_dmd_vtm_hash_update() OWNER TO owner_name;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: excelimport; Type: TABLE; Schema: local_formulary; Owner: owner_name
--

CREATE TABLE local_formulary.excelimport (
    name character varying,
    code character varying,
    level character varying,
    formulary_status character varying,
    critical_drug character varying,
    expensive_medication character varying,
    indication_mandatory character varying,
    unlicensed_route_code character varying,
    unlicensed_route_desc character varying
);


ALTER TABLE local_formulary.excelimport OWNER TO owner_name;

--
-- Name: formulary_additional_code; Type: TABLE; Schema: local_formulary; Owner: owner_name
--

CREATE TABLE local_formulary.formulary_additional_code (
    _row_id character varying(255) DEFAULT public.uuid_generate_v4() NOT NULL,
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdby character varying(255),
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    formulary_version_id character varying(255),
    additional_code character varying(500),
    additional_code_system character varying(500),
    _updatedtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _updateddate timestamp without time zone DEFAULT now(),
    _updatedby character varying(255),
    additional_code_desc text,
    attr1 text,
    meta_json text,
    source character varying(500) DEFAULT 'M'::character varying,
    code_type character varying(1000) DEFAULT 'Classification'::character varying
);


ALTER TABLE local_formulary.formulary_additional_code OWNER TO owner_name;

--
-- Name: formulary_detail; Type: TABLE; Schema: local_formulary; Owner: owner_name
--

CREATE TABLE local_formulary.formulary_detail (
    _row_id character varying(255) DEFAULT public.uuid_generate_v4() NOT NULL,
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdby character varying(255),
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    formulary_version_id character varying(255),
    medication_type_code character varying(50),
    rnoh_formulary_statuscd character varying(50),
    inpatient_medication_cd character varying(50),
    outpatient_medication_cd character varying(50),
    prescribing_status_cd character varying(50),
    rules_cd character varying(50),
    unlicensed_medication_cd character varying(50),
    defined_daily_dose character varying(255),
    not_for_prn character varying(10),
    high_alert_medication character varying(10),
    ignore_duplicate_warnings character varying(10),
    medusa_preparation_instructions text,
    critical_drug character varying(10),
    controlled_drug_category_cd character varying(50),
    cytotoxic character varying(10),
    clinical_trial_medication character varying(10),
    fluid character varying(10),
    antibiotic character varying(10),
    anticoagulant character varying(10),
    antipsychotic character varying(10),
    antimicrobial character varying(10),
    add_review_reminder boolean,
    iv_to_oral character varying(10),
    titration_type_cd character varying(50),
    rounding_factor_cd character varying(50),
    max_dose_numerator numeric(100,4),
    maximum_dose_unit_cd character varying(50),
    witnessing_required character varying(10),
    nice_ta character varying(255),
    marked_modifier_cd character varying(50),
    insulins character varying(10),
    mental_health_drug character varying(10),
    basis_of_preferred_name_cd character varying(50),
    sugar_free character varying(10),
    gluten_free character varying(10),
    preservative_free character varying(10),
    cfc_free character varying(10),
    dose_form_cd character varying(50),
    unit_dose_form_size numeric(20,4),
    unit_dose_form_units character varying(18),
    unit_dose_unit_of_measure_cd character varying(50),
    form_cd character varying(255),
    trade_family_cd character varying(18),
    modified_release_cd character varying(50),
    black_triangle character varying(10),
    supplier_cd character varying(50),
    current_licensing_authority_cd character varying(50),
    ema_additional_monitoring character varying(10),
    parallel_import character varying(10),
    restrictions_on_availability_cd character varying(50),
    _updatedtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _updateddate timestamp without time zone DEFAULT now(),
    _updatedby character varying(255),
    drug_class character varying(255),
    restriction_note text,
    restricted_prescribing character varying(10),
    side_effect text,
    caution text,
    contra_indication text,
    safety_message text,
    custom_warning text,
    endorsement text,
    licensed_use text,
    unlicensed_use text,
    orderable_formtype_cd character varying(50),
    trade_family_name character varying(500),
    expensive_medication character varying(50),
    is_blood_product boolean,
    is_diluent boolean,
    is_modified_release boolean,
    is_gastro_resistant boolean,
    prescribable boolean DEFAULT true,
    controlled_drug_category_source character varying(100),
    black_triangle_source character varying(100),
    high_alert_medication_source character varying(100),
    prescribable_source character varying(100),
    is_custom_controlled_drug boolean,
    diluent text,
    supplier_name character varying(1000),
    is_indication_mandatory boolean,
    reminder text,
    local_licensed_use text,
    local_unlicensed_use text,
    is_prescription_printing_required boolean
);


ALTER TABLE local_formulary.formulary_detail OWNER TO owner_name;

--
-- Name: formulary_excipient; Type: TABLE; Schema: local_formulary; Owner: owner_name
--

CREATE TABLE local_formulary.formulary_excipient (
    _row_id character varying(255) DEFAULT public.uuid_generate_v4() NOT NULL,
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdby character varying(255),
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    formulary_version_id character varying(255),
    ingredient_cd character varying(18),
    strength character varying(20),
    strength_unit_cd character varying(18),
    _updatedtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _updateddate timestamp without time zone DEFAULT now(),
    _updatedby character varying(255)
);


ALTER TABLE local_formulary.formulary_excipient OWNER TO owner_name;

--
-- Name: formulary_header; Type: TABLE; Schema: local_formulary; Owner: owner_name
--

CREATE TABLE local_formulary.formulary_header (
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdby character varying(255),
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    formulary_id character varying(255),
    version_id integer,
    formulary_version_id character varying(255) NOT NULL,
    code character varying(255),
    name text,
    name_tokens tsvector,
    product_type character varying(100),
    parent_code character varying(255),
    parent_name text,
    parent_name_tokens tsvector,
    parent_product_type character varying(100),
    rec_status_code character varying(8),
    rec_statuschange_ts timestamp with time zone,
    rec_statuschange_date timestamp without time zone,
    rec_statuschange_tzname character varying(255),
    rec_statuschange_tzoffset integer,
    is_duplicate boolean,
    is_latest boolean,
    rec_source character varying(50),
    vtm_id character varying(100),
    vmp_id character varying(100),
    rec_statuschange_msg text,
    duplicate_of_formulary_id character varying(255),
    _updatedtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _updateddate timestamp without time zone DEFAULT now(),
    _updatedby character varying(255),
    code_system character varying(500) DEFAULT 'DMD'::character varying
);


ALTER TABLE local_formulary.formulary_header OWNER TO owner_name;

--
-- Name: formulary_indication; Type: TABLE; Schema: local_formulary; Owner: owner_name
--

CREATE TABLE local_formulary.formulary_indication (
    _row_id character varying(255) DEFAULT public.uuid_generate_v4() NOT NULL,
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdby character varying(255),
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    formulary_version_id character varying(255),
    indication_cd character varying(50),
    _updatedtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _updateddate timestamp without time zone DEFAULT now(),
    _updatedby character varying(255),
    indication_name character varying(500)
);


ALTER TABLE local_formulary.formulary_indication OWNER TO owner_name;

--
-- Name: formulary_ingredient; Type: TABLE; Schema: local_formulary; Owner: owner_name
--

CREATE TABLE local_formulary.formulary_ingredient (
    _row_id character varying(255) DEFAULT public.uuid_generate_v4() NOT NULL,
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdby character varying(255),
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    formulary_version_id character varying(255),
    ingredient_cd character varying(18),
    basis_of_pharmaceutical_strength_cd character varying(50),
    strength_value_numerator character varying(20),
    strength_value_numerator_unit_cd character varying(18),
    _updatedtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _updateddate timestamp without time zone DEFAULT now(),
    _updatedby character varying(255),
    strength_value_denominator character varying(20),
    strength_value_denominator_unit_cd character varying(20),
    ingredient_name character varying(1000)
);


ALTER TABLE local_formulary.formulary_ingredient OWNER TO owner_name;

--
-- Name: formulary_local_route_detail; Type: TABLE; Schema: local_formulary; Owner: owner_name
--

CREATE TABLE local_formulary.formulary_local_route_detail (
    _row_id character varying(255) DEFAULT public.uuid_generate_v4() NOT NULL,
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdby character varying(255),
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    formulary_version_id character varying(255),
    route_cd character varying(50),
    route_field_type_cd character varying(50),
    _updatedtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _updateddate timestamp without time zone DEFAULT now(),
    _updatedby character varying(255),
    source character varying(100)
);


ALTER TABLE local_formulary.formulary_local_route_detail OWNER TO owner_name;

--
-- Name: formulary_ontology_form; Type: TABLE; Schema: local_formulary; Owner: owner_name
--

CREATE TABLE local_formulary.formulary_ontology_form (
    _row_id character varying(255) DEFAULT public.uuid_generate_v4() NOT NULL,
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdby character varying(255),
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    formulary_version_id character varying(255),
    form_cd character varying(50),
    _updatedtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _updateddate timestamp without time zone DEFAULT now(),
    _updatedby character varying(255)
);


ALTER TABLE local_formulary.formulary_ontology_form OWNER TO owner_name;

--
-- Name: formulary_route_detail; Type: TABLE; Schema: local_formulary; Owner: owner_name
--

CREATE TABLE local_formulary.formulary_route_detail (
    _row_id character varying(255) DEFAULT public.uuid_generate_v4() NOT NULL,
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdby character varying(255),
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    formulary_version_id character varying(255),
    route_cd character varying(50),
    route_field_type_cd character varying(50),
    _updatedtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _updateddate timestamp without time zone DEFAULT now(),
    _updatedby character varying(255),
    source character varying(100)
);


ALTER TABLE local_formulary.formulary_route_detail OWNER TO owner_name;

--
-- Name: formulary_rule_config; Type: TABLE; Schema: local_formulary; Owner: owner_name
--

CREATE TABLE local_formulary.formulary_rule_config (
    _row_id character varying(255) DEFAULT public.uuid_generate_v4() NOT NULL,
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    name character varying(100),
    config_json text,
    _updatedtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _updateddate timestamp without time zone DEFAULT now(),
    _updatedby character varying(255)
);


ALTER TABLE local_formulary.formulary_rule_config OWNER TO owner_name;

--
-- Name: formulary_rule_config__sequenceid_seq; Type: SEQUENCE; Schema: local_formulary; Owner: owner_name
--

CREATE SEQUENCE local_formulary.formulary_rule_config__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE local_formulary.formulary_rule_config__sequenceid_seq OWNER TO owner_name;

--
-- Name: formulary_rule_config__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: local_formulary; Owner: owner_name
--

ALTER SEQUENCE local_formulary.formulary_rule_config__sequenceid_seq OWNED BY local_formulary.formulary_rule_config._sequenceid;


--
-- Name: lookup_common; Type: TABLE; Schema: local_formulary; Owner: owner_name
--

CREATE TABLE local_formulary.lookup_common (
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(50),
    "desc" character varying(1000),
    type character varying(100),
    isdefault boolean,
    _updatedtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _updateddate timestamp without time zone DEFAULT now(),
    _updatedby character varying(255),
    additionalproperties text
);


ALTER TABLE local_formulary.lookup_common OWNER TO owner_name;

--
-- Name: lookup_common__sequenceid_seq; Type: SEQUENCE; Schema: local_formulary; Owner: owner_name
--

CREATE SEQUENCE local_formulary.lookup_common__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE local_formulary.lookup_common__sequenceid_seq OWNER TO owner_name;

--
-- Name: lookup_common__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: local_formulary; Owner: owner_name
--

ALTER SEQUENCE local_formulary.lookup_common__sequenceid_seq OWNED BY local_formulary.lookup_common._sequenceid;


--
-- Name: atc_lookup; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.atc_lookup (
    atc_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(255),
    "desc" text,
    short_cd character varying(255)
);


ALTER TABLE terminology.atc_lookup OWNER TO owner_name;

--
-- Name: atc_lookup__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.atc_lookup__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.atc_lookup__sequenceid_seq OWNER TO owner_name;

--
-- Name: atc_lookup__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.atc_lookup__sequenceid_seq OWNED BY terminology.atc_lookup._sequenceid;


--
-- Name: bnf_lookup; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.bnf_lookup (
    bnf_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(255),
    name text
);


ALTER TABLE terminology.bnf_lookup OWNER TO owner_name;

--
-- Name: bnf_lookup__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.bnf_lookup__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.bnf_lookup__sequenceid_seq OWNER TO owner_name;

--
-- Name: bnf_lookup__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.bnf_lookup__sequenceid_seq OWNED BY terminology.bnf_lookup._sequenceid;


--
-- Name: dmd_amp; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_amp (
    amp_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    apid character varying(255),
    invalid smallint,
    vpid character varying(255),
    nm character varying(1000),
    abbrevnm character varying(1000),
    "desc" character varying(1000),
    nmdt timestamp without time zone,
    nm_prev character varying(1000),
    suppcd character varying(255),
    lic_authcd bigint,
    lic_auth_prevcd bigint,
    lic_authchangecd bigint,
    lic_authchangedt timestamp without time zone,
    combprodcd bigint,
    flavourcd bigint,
    ema integer,
    parallel_import integer,
    avail_restrictcd bigint,
    col_val_hash uuid
);


ALTER TABLE terminology.dmd_amp OWNER TO owner_name;

--
-- Name: dmd_amp__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_amp__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_amp__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_amp__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_amp__sequenceid_seq OWNED BY terminology.dmd_amp._sequenceid;


--
-- Name: dmd_amp_drugroute; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_amp_drugroute (
    adr_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    apid character varying(255),
    routecd character varying(255),
    col_val_hash uuid
);


ALTER TABLE terminology.dmd_amp_drugroute OWNER TO owner_name;

--
-- Name: dmd_amp_drugroute__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_amp_drugroute__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_amp_drugroute__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_amp_drugroute__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_amp_drugroute__sequenceid_seq OWNED BY terminology.dmd_amp_drugroute._sequenceid;


--
-- Name: dmd_amp_excipient; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_amp_excipient (
    aex_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    apid character varying(255),
    isid character varying(255),
    strnth numeric,
    strnth_uomcd character varying(255),
    col_val_hash uuid
);


ALTER TABLE terminology.dmd_amp_excipient OWNER TO owner_name;

--
-- Name: dmd_amp_excipient__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_amp_excipient__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_amp_excipient__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_amp_excipient__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_amp_excipient__sequenceid_seq OWNED BY terminology.dmd_amp_excipient._sequenceid;


--
-- Name: dmd_atc; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_atc (
    dat_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    atc_cd character varying(255),
    atc_short_cd character varying(255),
    dmd_cd character varying(255)
);


ALTER TABLE terminology.dmd_atc OWNER TO owner_name;

--
-- Name: dmd_atc__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_atc__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_atc__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_atc__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_atc__sequenceid_seq OWNED BY terminology.dmd_atc._sequenceid;


--
-- Name: dmd_bnf; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_bnf (
    dbn_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    bnf_cd character varying(255),
    dmd_cd character varying(255),
    dmd_level character varying(255)
);


ALTER TABLE terminology.dmd_bnf OWNER TO owner_name;

--
-- Name: dmd_bnf__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_bnf__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_bnf__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_bnf__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_bnf__sequenceid_seq OWNED BY terminology.dmd_bnf._sequenceid;


--
-- Name: dmd_lookup_availrestrict; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_availrestrict (
    lar_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology.dmd_lookup_availrestrict OWNER TO owner_name;

--
-- Name: dmd_lookup_availrestrict__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_availrestrict__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_availrestrict__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_availrestrict__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_availrestrict__sequenceid_seq OWNED BY terminology.dmd_lookup_availrestrict._sequenceid;


--
-- Name: dmd_lookup_basisofname; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_basisofname (
    bon_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology.dmd_lookup_basisofname OWNER TO owner_name;

--
-- Name: dmd_lookup_basisofname__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_basisofname__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_basisofname__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_basisofname__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_basisofname__sequenceid_seq OWNED BY terminology.dmd_lookup_basisofname._sequenceid;


--
-- Name: dmd_lookup_basisofstrength; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_basisofstrength (
    lbs_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology.dmd_lookup_basisofstrength OWNER TO owner_name;

--
-- Name: dmd_lookup_basisofstrength__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_basisofstrength__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_basisofstrength__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_basisofstrength__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_basisofstrength__sequenceid_seq OWNED BY terminology.dmd_lookup_basisofstrength._sequenceid;


--
-- Name: dmd_lookup_controldrugcat; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_controldrugcat (
    lcd_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology.dmd_lookup_controldrugcat OWNER TO owner_name;

--
-- Name: dmd_lookup_controldrugcat__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_controldrugcat__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_controldrugcat__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_controldrugcat__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_controldrugcat__sequenceid_seq OWNED BY terminology.dmd_lookup_controldrugcat._sequenceid;


--
-- Name: dmd_lookup_drugformind; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_drugformind (
    lfi_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology.dmd_lookup_drugformind OWNER TO owner_name;

--
-- Name: dmd_lookup_drugformind__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_drugformind__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_drugformind__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_drugformind__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_drugformind__sequenceid_seq OWNED BY terminology.dmd_lookup_drugformind._sequenceid;


--
-- Name: dmd_lookup_form; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_form (
    lfr_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(255),
    cddt timestamp without time zone,
    cdprev character varying(255),
    "desc" character varying(1000),
    is_latest boolean
);


ALTER TABLE terminology.dmd_lookup_form OWNER TO owner_name;

--
-- Name: dmd_lookup_form__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_form__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_form__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_form__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_form__sequenceid_seq OWNED BY terminology.dmd_lookup_form._sequenceid;


--
-- Name: dmd_lookup_ingredient; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_ingredient (
    lin_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    isid character varying(255),
    isiddt timestamp without time zone,
    isidprev character varying(255),
    invalid smallint,
    nm character varying(1000),
    is_latest boolean
);


ALTER TABLE terminology.dmd_lookup_ingredient OWNER TO owner_name;

--
-- Name: dmd_lookup_ingredient__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_ingredient__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_ingredient__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_ingredient__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_ingredient__sequenceid_seq OWNED BY terminology.dmd_lookup_ingredient._sequenceid;


--
-- Name: dmd_lookup_licauth; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_licauth (
    lau_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology.dmd_lookup_licauth OWNER TO owner_name;

--
-- Name: dmd_lookup_licauth__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_licauth__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_licauth__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_licauth__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_licauth__sequenceid_seq OWNED BY terminology.dmd_lookup_licauth._sequenceid;


--
-- Name: dmd_lookup_ontformroute; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_ontformroute (
    ofr_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology.dmd_lookup_ontformroute OWNER TO owner_name;

--
-- Name: dmd_lookup_ontformroute__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_ontformroute__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_ontformroute__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_ontformroute__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_ontformroute__sequenceid_seq OWNED BY terminology.dmd_lookup_ontformroute._sequenceid;


--
-- Name: dmd_lookup_prescribingstatus; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_prescribingstatus (
    lps_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology.dmd_lookup_prescribingstatus OWNER TO owner_name;

--
-- Name: dmd_lookup_prescribingstatus__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_prescribingstatus__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_prescribingstatus__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_prescribingstatus__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_prescribingstatus__sequenceid_seq OWNED BY terminology.dmd_lookup_prescribingstatus._sequenceid;


--
-- Name: dmd_lookup_route; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_route (
    lrt_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(255),
    cddt timestamp without time zone,
    cdprev character varying(255),
    "desc" character varying(1000),
    source character varying(50),
    is_latest boolean
);


ALTER TABLE terminology.dmd_lookup_route OWNER TO owner_name;

--
-- Name: dmd_lookup_route__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_route__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_route__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_route__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_route__sequenceid_seq OWNED BY terminology.dmd_lookup_route._sequenceid;


--
-- Name: dmd_lookup_supplier; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_supplier (
    lsu_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(255),
    cddt timestamp without time zone,
    cdprev character varying(255),
    invalid smallint,
    "desc" character varying(1000),
    is_latest boolean
);


ALTER TABLE terminology.dmd_lookup_supplier OWNER TO owner_name;

--
-- Name: dmd_lookup_supplier__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_supplier__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_supplier__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_supplier__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_supplier__sequenceid_seq OWNED BY terminology.dmd_lookup_supplier._sequenceid;


--
-- Name: dmd_lookup_uom; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_lookup_uom (
    uom_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(255),
    cddt timestamp without time zone,
    cdprev character varying(255),
    "desc" character varying(1000),
    is_latest boolean
);


ALTER TABLE terminology.dmd_lookup_uom OWNER TO owner_name;

--
-- Name: dmd_lookup_uom__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_lookup_uom__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_lookup_uom__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_uom__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_lookup_uom__sequenceid_seq OWNED BY terminology.dmd_lookup_uom._sequenceid;


--
-- Name: dmd_vmp; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_vmp (
    vmp_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vpid character varying(255),
    vpiddt timestamp without time zone,
    vpidprev character varying(255),
    vtmid character varying(255),
    invalid smallint,
    nm character varying(1000),
    abbrevnm character varying(1000),
    basiscd bigint,
    nmdt timestamp without time zone,
    nmprev character varying(1000),
    basis_prevcd bigint,
    nmchangecd bigint,
    comprodcd bigint,
    pres_statcd bigint,
    sug_f integer,
    glu_f integer,
    pres_f integer,
    cfc_f integer,
    non_availcd integer,
    non_availdt timestamp without time zone,
    df_indcd bigint,
    udfs numeric,
    udfs_uomcd character varying(255),
    unit_dose_uomcd character varying(255),
    col_val_hash uuid
);


ALTER TABLE terminology.dmd_vmp OWNER TO owner_name;

--
-- Name: dmd_vmp_controldrug; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_vmp_controldrug (
    vcd_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vpid character varying(255),
    catcd bigint,
    catdt timestamp without time zone,
    cat_prevcd bigint,
    col_val_hash uuid
);


ALTER TABLE terminology.dmd_vmp_controldrug OWNER TO owner_name;

--
-- Name: dmd_vmp_drugform; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_vmp_drugform (
    vdf_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vpid character varying(255),
    formcd character varying(255),
    col_val_hash uuid
);


ALTER TABLE terminology.dmd_vmp_drugform OWNER TO owner_name;

--
-- Name: dmd_vmp_drugroute; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_vmp_drugroute (
    vdr_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vpid character varying(255),
    routecd character varying(255),
    col_val_hash uuid
);


ALTER TABLE terminology.dmd_vmp_drugroute OWNER TO owner_name;

--
-- Name: dmd_vmp_ingredient; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_vmp_ingredient (
    vin_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vpid character varying(255),
    isid character varying(255),
    basis_strntcd bigint,
    bs_subid character varying(255),
    strnt_nmrtr_val numeric,
    strnt_nmrtr_uomcd character varying(255),
    strnt_dnmtr_val numeric,
    strnt_dnmtr_uomcd character varying(255),
    col_val_hash uuid
);


ALTER TABLE terminology.dmd_vmp_ingredient OWNER TO owner_name;

--
-- Name: dmd_vmp_ontdrugform; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_vmp_ontdrugform (
    odf_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vpid character varying(255),
    formcd bigint,
    col_val_hash uuid
);


ALTER TABLE terminology.dmd_vmp_ontdrugform OWNER TO owner_name;

--
-- Name: dmd_vtm; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_vtm (
    vtm_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vtmid character varying(255),
    invalid smallint,
    nm character varying(1000),
    abbrevnm character varying(1000),
    vtmidprev character varying(255),
    vtmiddt timestamp without time zone,
    col_val_hash uuid
);


ALTER TABLE terminology.dmd_vtm OWNER TO owner_name;

--
-- Name: dmd_names_lookup_all_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.dmd_names_lookup_all_mat AS
 SELECT DISTINCT vtm.nm AS name,
    to_tsvector('english'::regconfig, (vtm.nm)::text) AS name_tokens,
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
  WHERE (vtm.invalid IS NULL)
UNION
 SELECT DISTINCT vmp.nm AS name,
    to_tsvector('english'::regconfig, (vmp.nm)::text) AS name_tokens,
    vmp.vpid AS code,
    vmp.vpidprev AS prevcode,
    (vmp.basiscd)::character varying(100) AS basiscd,
    (vmp.cfc_f)::character varying(100) AS cfcf,
    (vmp.glu_f)::character varying(100) AS gluf,
    (vmp.pres_f)::character varying(100) AS presf,
    (vmp.sug_f)::character varying(100) AS sugf,
    vmp.udfs,
    (vmp.udfs_uomcd)::character varying(100) AS udfsuomcd,
    (vmp.unit_dose_uomcd)::character varying(100) AS unitdoseuomcd,
    (vmp.df_indcd)::character varying(100) AS dfindcd,
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
   FROM ((((((((((terminology.dmd_vmp vmp
     LEFT JOIN terminology.dmd_lookup_prescribingstatus pres ON (((pres.cd)::text = (vmp.pres_statcd)::text)))
     LEFT JOIN terminology.dmd_vmp_drugform vdf ON (((vdf.vpid)::text = (vmp.vpid)::text)))
     LEFT JOIN terminology.dmd_lookup_form form ON (((form.cd)::text = (vdf.formcd)::text)))
     LEFT JOIN terminology.dmd_vmp_drugroute vdr ON (((vdr.vpid)::text = (vmp.vpid)::text)))
     LEFT JOIN terminology.dmd_lookup_route route ON (((route.cd)::text = (vdr.routecd)::text)))
     LEFT JOIN terminology.dmd_vmp_controldrug vcd ON (((vcd.vpid)::text = (vmp.vpid)::text)))
     LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON (((lcd.cd)::text = (vcd.catcd)::text)))
     LEFT JOIN terminology.dmd_vmp_ingredient ving ON (((ving.vpid)::text = (vmp.vpid)::text)))
     LEFT JOIN terminology.dmd_vmp_ontdrugform vondf ON (((vondf.vpid)::text = (vmp.vpid)::text)))
     LEFT JOIN terminology.dmd_lookup_ontformroute ont ON (((ont.cd)::text = (vondf.formcd)::text)))
  WHERE (vmp.invalid IS NULL)
UNION
 SELECT DISTINCT
        CASE
            WHEN ((supp."desc" IS NULL) OR ((supp."desc")::text = ''::text)) THEN (amp.nm)::text
            ELSE ((((amp.nm)::text || ' ('::text) || (supp."desc")::text) || ')'::text)
        END AS name,
        CASE
            WHEN ((supp."desc" IS NULL) OR ((supp."desc")::text = ''::text)) THEN to_tsvector('english'::regconfig, (amp.nm)::text)
            ELSE to_tsvector('english'::regconfig, ((((amp.nm)::text || ' ('::text) || (supp."desc")::text) || ')'::text))
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
    (amp.ema)::character varying(100) AS ema,
    (amp.lic_authcd)::character varying(100) AS licauthcd,
    (amp.parallel_import)::character varying(100) AS parallelimport,
    (amp.avail_restrictcd)::character varying(100) AS availrestrictcd,
    NULL::character varying(255) AS vmproute_code,
    NULL::character varying(1000) AS vmproute,
    NULL::character varying(255) AS vmpform_code,
    NULL::character varying(1000) AS vmpform,
    supp.cd AS supplier_code,
    supp."desc" AS supplier,
    to_tsvector('english'::regconfig, (supp."desc")::text) AS supplier_name_tokens,
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
   FROM (terminology.dmd_amp amp
     LEFT JOIN terminology.dmd_lookup_supplier supp ON (((supp.cd)::text = (amp.suppcd)::text)))
  WHERE (amp.invalid IS NULL)
  ORDER BY 1
  WITH NO DATA;


ALTER TABLE terminology.dmd_names_lookup_all_mat OWNER TO owner_name;

--
-- Name: dmd_names_lookup_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.dmd_names_lookup_mat AS
 SELECT DISTINCT vtm.nm AS name,
    to_tsvector('english'::regconfig, (vtm.nm)::text) AS name_tokens,
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
  WHERE (vtm.invalid IS NULL)
UNION
 SELECT DISTINCT vmp.nm AS name,
    to_tsvector('english'::regconfig, (vmp.nm)::text) AS name_tokens,
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
   FROM (((((((terminology.dmd_vmp vmp
     LEFT JOIN terminology.dmd_lookup_prescribingstatus pres ON (((pres.cd)::text = (vmp.pres_statcd)::text)))
     LEFT JOIN terminology.dmd_vmp_drugform vdf ON (((vdf.vpid)::text = (vmp.vpid)::text)))
     LEFT JOIN terminology.dmd_lookup_form form ON (((form.cd)::text = (vdf.formcd)::text)))
     LEFT JOIN terminology.dmd_vmp_drugroute vdr ON (((vdr.vpid)::text = (vmp.vpid)::text)))
     LEFT JOIN terminology.dmd_lookup_route route ON (((route.cd)::text = (vdr.routecd)::text)))
     LEFT JOIN terminology.dmd_vmp_controldrug vcd ON (((vcd.vpid)::text = (vmp.vpid)::text)))
     LEFT JOIN terminology.dmd_lookup_controldrugcat lcd ON (((lcd.cd)::text = (vcd.catcd)::text)))
  WHERE (vmp.invalid IS NULL)
UNION
 SELECT DISTINCT
        CASE
            WHEN ((supp."desc" IS NULL) OR ((supp."desc")::text = ''::text)) THEN (amp.nm)::text
            ELSE ((((amp.nm)::text || ' ('::text) || (supp."desc")::text) || ')'::text)
        END AS name,
        CASE
            WHEN ((supp."desc" IS NULL) OR ((supp."desc")::text = ''::text)) THEN to_tsvector('english'::regconfig, (amp.nm)::text)
            ELSE to_tsvector('english'::regconfig, ((((amp.nm)::text || ' ('::text) || (supp."desc")::text) || ')'::text))
        END AS name_tokens,
    amp.apid AS code,
    NULL::character varying(255) AS vmproute_code,
    NULL::character varying(1000) AS vmproute,
    NULL::character varying(255) AS vmpform_code,
    NULL::character varying(1000) AS vmpform,
    supp.cd AS supplier_code,
    supp."desc" AS supplier,
    to_tsvector('english'::regconfig, (supp."desc")::text) AS supplier_name_tokens,
    NULL::bigint AS prescribing_status_code,
    NULL::character varying(1000) AS prescribing_status,
    NULL::bigint AS control_drug_category_code,
    NULL::character varying(1000) AS control_drug_category
   FROM (terminology.dmd_amp amp
     LEFT JOIN terminology.dmd_lookup_supplier supp ON (((supp.cd)::text = (amp.suppcd)::text)))
  WHERE (amp.invalid IS NULL)
  WITH NO DATA;


ALTER TABLE terminology.dmd_names_lookup_mat OWNER TO owner_name;

--
-- Name: dmd_relationships_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.dmd_relationships_mat AS
 SELECT DISTINCT vtm.vtmid AS code,
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
  WITH NO DATA;


ALTER TABLE terminology.dmd_relationships_mat OWNER TO owner_name;

--
-- Name: dmd_snomed_version; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_snomed_version (
    dmdversion character varying,
    snomedversion character varying
);


ALTER TABLE terminology.dmd_snomed_version OWNER TO owner_name;

--
-- Name: dmd_sync_log; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.dmd_sync_log (
    dmd_id character varying(1000),
    sync_process_id character varying(255),
    dmd_entity_name character varying(255),
    created_dt timestamp with time zone DEFAULT now(),
    row_action character varying(10),
    serial_num bigint NOT NULL,
    is_dmd_updated boolean,
    dmd_update_dt timestamp with time zone,
    is_formulary_updated boolean,
    formulary_update_dt timestamp with time zone,
    dmd_version character varying(100),
    sl_id character varying(255) DEFAULT public.uuid_generate_v4() NOT NULL
);


ALTER TABLE terminology.dmd_sync_log OWNER TO owner_name;

--
-- Name: dmd_sync_log_serial_num_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_sync_log_serial_num_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_sync_log_serial_num_seq OWNER TO owner_name;

--
-- Name: dmd_sync_log_serial_num_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_sync_log_serial_num_seq OWNED BY terminology.dmd_sync_log.serial_num;


--
-- Name: dmd_vmp__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_vmp__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_vmp__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vmp__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_vmp__sequenceid_seq OWNED BY terminology.dmd_vmp._sequenceid;


--
-- Name: dmd_vmp_controldrug__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_vmp_controldrug__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_vmp_controldrug__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vmp_controldrug__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_vmp_controldrug__sequenceid_seq OWNED BY terminology.dmd_vmp_controldrug._sequenceid;


--
-- Name: dmd_vmp_drugform__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_vmp_drugform__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_vmp_drugform__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vmp_drugform__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_vmp_drugform__sequenceid_seq OWNED BY terminology.dmd_vmp_drugform._sequenceid;


--
-- Name: dmd_vmp_drugroute__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_vmp_drugroute__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_vmp_drugroute__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vmp_drugroute__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_vmp_drugroute__sequenceid_seq OWNED BY terminology.dmd_vmp_drugroute._sequenceid;


--
-- Name: dmd_vmp_ingredient__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_vmp_ingredient__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_vmp_ingredient__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vmp_ingredient__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_vmp_ingredient__sequenceid_seq OWNED BY terminology.dmd_vmp_ingredient._sequenceid;


--
-- Name: dmd_vmp_ontdrugform__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_vmp_ontdrugform__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_vmp_ontdrugform__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vmp_ontdrugform__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_vmp_ontdrugform__sequenceid_seq OWNED BY terminology.dmd_vmp_ontdrugform._sequenceid;


--
-- Name: dmd_vtm__sequenceid_seq; Type: SEQUENCE; Schema: terminology; Owner: owner_name
--

CREATE SEQUENCE terminology.dmd_vtm__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology.dmd_vtm__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vtm__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology; Owner: owner_name
--

ALTER SEQUENCE terminology.dmd_vtm__sequenceid_seq OWNED BY terminology.dmd_vtm._sequenceid;


--
-- Name: snomedct_associationrefset_f; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_associationrefset_f (
    id uuid NOT NULL,
    effectivetime character(8) NOT NULL,
    active character(1) NOT NULL,
    moduleid character varying(18) NOT NULL,
    refsetid character varying(18) NOT NULL,
    referencedcomponentid character varying(18) NOT NULL,
    targetcomponentid character varying(18) NOT NULL
);


ALTER TABLE terminology.snomedct_associationrefset_f OWNER TO owner_name;

--
-- Name: snomedct_attributevaluerefset_f; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_attributevaluerefset_f (
    id uuid NOT NULL,
    effectivetime character(8) NOT NULL,
    active character(1) NOT NULL,
    moduleid character varying(18) NOT NULL,
    refsetid character varying(18) NOT NULL,
    referencedcomponentid character varying(18) NOT NULL,
    valueid character varying(18) NOT NULL
);


ALTER TABLE terminology.snomedct_attributevaluerefset_f OWNER TO owner_name;

--
-- Name: snomedct_complexmaprefset_f; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_complexmaprefset_f (
    id uuid NOT NULL,
    effectivetime character(8) NOT NULL,
    active character(1) NOT NULL,
    moduleid character varying(18) NOT NULL,
    refsetid character varying(18) NOT NULL,
    referencedcomponentid character varying(18) NOT NULL,
    mapgroup smallint NOT NULL,
    mappriority smallint NOT NULL,
    maprule text,
    mapadvice text,
    maptarget text,
    correlationid character varying(18) NOT NULL
);


ALTER TABLE terminology.snomedct_complexmaprefset_f OWNER TO owner_name;

--
-- Name: snomedct_concept_f; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_concept_f (
    id character varying(18) NOT NULL,
    effectivetime character(8) NOT NULL,
    active character(1) NOT NULL,
    moduleid character varying(18) NOT NULL,
    definitionstatusid character varying(18) NOT NULL
);


ALTER TABLE terminology.snomedct_concept_f OWNER TO owner_name;

--
-- Name: snomedct_concept_latest_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_concept_latest_mat AS
 SELECT tmp.id,
    tmp.effectivetime,
    tmp.active,
    tmp.moduleid,
    tmp.definitionstatusid
   FROM ( SELECT rank() OVER (PARTITION BY cf.id ORDER BY cf.effectivetime DESC) AS rnk,
            cf.id,
            cf.effectivetime,
            cf.active,
            cf.moduleid,
            cf.definitionstatusid
           FROM terminology.snomedct_concept_f cf) tmp
  WHERE (tmp.rnk = 1)
  WITH NO DATA;


ALTER TABLE terminology.snomedct_concept_latest_mat OWNER TO owner_name;

--
-- Name: snomedct_description_f; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_description_f (
    id character varying(18) NOT NULL,
    effectivetime character(8) NOT NULL,
    active character(1) NOT NULL,
    moduleid character varying(18) NOT NULL,
    conceptid character varying(18) NOT NULL,
    languagecode character varying(2) NOT NULL,
    typeid character varying(18) NOT NULL,
    term text NOT NULL,
    casesignificanceid character varying(18) NOT NULL
);


ALTER TABLE terminology.snomedct_description_f OWNER TO owner_name;

--
-- Name: snomedct_description_latest_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_description_latest_mat AS
 SELECT tmp.id,
    tmp.effectivetime,
    tmp.active,
    tmp.moduleid,
    tmp.conceptid,
    tmp.languagecode,
    tmp.typeid,
    tmp.term,
    to_tsvector('english'::regconfig, tmp.term) AS term_tokens,
    tmp.casesignificanceid
   FROM ( SELECT rank() OVER (PARTITION BY df.id ORDER BY df.effectivetime DESC) AS rnk,
            df.id,
            df.effectivetime,
            df.active,
            df.moduleid,
            df.conceptid,
            df.languagecode,
            df.typeid,
            df.term,
            df.casesignificanceid
           FROM terminology.snomedct_description_f df) tmp
  WHERE (tmp.rnk = 1)
  WITH NO DATA;


ALTER TABLE terminology.snomedct_description_latest_mat OWNER TO owner_name;

--
-- Name: snomedct_langrefset_f; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_langrefset_f (
    id uuid NOT NULL,
    effectivetime character(8) NOT NULL,
    active character(1) NOT NULL,
    moduleid character varying(18) NOT NULL,
    refsetid character varying(18) NOT NULL,
    referencedcomponentid character varying(18) NOT NULL,
    acceptabilityid character varying(18) NOT NULL
);


ALTER TABLE terminology.snomedct_langrefset_f OWNER TO owner_name;

--
-- Name: snomedct_langrefset_latest_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_langrefset_latest_mat AS
 SELECT tmp.id,
    tmp.effectivetime,
    tmp.active,
    tmp.moduleid,
    tmp.refsetid,
    tmp.referencedcomponentid,
    tmp.acceptabilityid
   FROM ( SELECT rank() OVER (PARTITION BY lf.id, lf.moduleid, lf.refsetid, lf.referencedcomponentid, lf.acceptabilityid ORDER BY lf.effectivetime DESC) AS rnk,
            lf.id,
            lf.effectivetime,
            lf.active,
            lf.moduleid,
            lf.refsetid,
            lf.referencedcomponentid,
            lf.acceptabilityid
           FROM terminology.snomedct_langrefset_f lf) tmp
  WHERE (tmp.rnk = 1)
  WITH NO DATA;


ALTER TABLE terminology.snomedct_langrefset_latest_mat OWNER TO owner_name;

--
-- Name: snomedct_concept_allname_latest_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_concept_allname_latest_mat AS
 SELECT DISTINCT c.id AS conceptid,
    d.term AS name,
    to_tsvector('english'::regconfig, d.term) AS name_tokens,
    d.id AS descriptionid
   FROM ((terminology.snomedct_concept_latest_mat c
     JOIN terminology.snomedct_description_latest_mat d ON ((((c.id)::text = (d.conceptid)::text) AND (d.active = '1'::bpchar) AND ((d.typeid)::text = '900000000000013009'::text) AND (c.active = '1'::bpchar))))
     JOIN terminology.snomedct_langrefset_latest_mat l ON ((((d.id)::text = (l.referencedcomponentid)::text) AND (l.active = '1'::bpchar) AND ((l.refsetid)::text = '900000000000508004'::text))))
  WITH NO DATA;


ALTER TABLE terminology.snomedct_concept_allname_latest_mat OWNER TO owner_name;

--
-- Name: snomedct_concept_all_lookup_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_concept_all_lookup_mat AS
 SELECT DISTINCT s.conceptid,
    s.name AS preferredterm,
    s.name_tokens AS preferredname_tokens,
    d.term AS fsn,
    rtrim("substring"(d.term, '([^\(]+[\)+])$'::text), ')'::text) AS semantictag
   FROM (terminology.snomedct_concept_allname_latest_mat s
     JOIN terminology.snomedct_description_latest_mat d ON (((d.active = '1'::bpchar) AND ((d.conceptid)::text = (s.conceptid)::text) AND ((d.typeid)::text = '900000000000003001'::text))))
  WITH NO DATA;


ALTER TABLE terminology.snomedct_concept_all_lookup_mat OWNER TO owner_name;

--
-- Name: snomedct_conceptpreferredname_latest_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_conceptpreferredname_latest_mat AS
 SELECT DISTINCT c.id AS conceptid,
    d.term AS preferredname,
    to_tsvector('english'::regconfig, d.term) AS preferredname_tokens,
    d.id AS descriptionid
   FROM ((terminology.snomedct_concept_latest_mat c
     JOIN terminology.snomedct_description_latest_mat d ON ((((c.id)::text = (d.conceptid)::text) AND (d.active = '1'::bpchar) AND ((d.typeid)::text = '900000000000013009'::text) AND (c.active = '1'::bpchar))))
     JOIN terminology.snomedct_langrefset_latest_mat l ON ((((d.id)::text = (l.referencedcomponentid)::text) AND (l.active = '1'::bpchar) AND ((l.refsetid)::text = '900000000000508004'::text) AND ((l.acceptabilityid)::text = '900000000000548007'::text))))
  WITH NO DATA;


ALTER TABLE terminology.snomedct_conceptpreferredname_latest_mat OWNER TO owner_name;

--
-- Name: snomedct_concept_lookup_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_concept_lookup_mat AS
 SELECT DISTINCT s.conceptid,
    s.preferredname AS preferredterm,
    s.preferredname_tokens,
    d.term AS fsn,
    rtrim("substring"(d.term, '([^\(]+[\)+])$'::text), ')'::text) AS semantictag
   FROM (terminology.snomedct_conceptpreferredname_latest_mat s
     JOIN terminology.snomedct_description_latest_mat d ON (((d.active = '1'::bpchar) AND ((d.conceptid)::text = (s.conceptid)::text) AND ((d.typeid)::text = '900000000000003001'::text))))
  WITH NO DATA;


ALTER TABLE terminology.snomedct_concept_lookup_mat OWNER TO owner_name;

--
-- Name: snomedct_extendedmaprefset_f; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_extendedmaprefset_f (
    id uuid NOT NULL,
    effectivetime character(8) NOT NULL,
    active character(1) NOT NULL,
    moduleid character varying(18) NOT NULL,
    refsetid character varying(18) NOT NULL,
    referencedcomponentid character varying(18) NOT NULL,
    mapgroup smallint NOT NULL,
    mappriority smallint NOT NULL,
    maprule text,
    mapadvice text,
    maptarget text,
    correlationid character varying(18),
    mapcategoryid character varying(18)
);


ALTER TABLE terminology.snomedct_extendedmaprefset_f OWNER TO owner_name;

--
-- Name: snomedct_lookup_semantictag; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_lookup_semantictag (
    id character varying DEFAULT public.uuid_generate_v4() NOT NULL,
    domain character varying NOT NULL,
    tag character varying NOT NULL
);


ALTER TABLE terminology.snomedct_lookup_semantictag OWNER TO owner_name;

--
-- Name: snomedct_relationship_f; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_relationship_f (
    id character varying(18) NOT NULL,
    effectivetime character(8) NOT NULL,
    active character(1) NOT NULL,
    moduleid character varying(18) NOT NULL,
    sourceid character varying(18) NOT NULL,
    destinationid character varying(18) NOT NULL,
    relationshipgroup character varying(18) NOT NULL,
    typeid character varying(18) NOT NULL,
    characteristictypeid character varying(18) NOT NULL,
    modifierid character varying(18) NOT NULL
);


ALTER TABLE terminology.snomedct_relationship_f OWNER TO owner_name;

--
-- Name: snomedct_relationship_latest_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_relationship_latest_mat AS
 SELECT tmp.id,
    tmp.effectivetime,
    tmp.active,
    tmp.moduleid,
    tmp.sourceid,
    tmp.destinationid,
    tmp.relationshipgroup,
    tmp.typeid,
    tmp.characteristictypeid,
    tmp.modifierid
   FROM ( SELECT rank() OVER (PARTITION BY rf.id, rf.moduleid, rf.sourceid, rf.destinationid, rf.relationshipgroup, rf.typeid, rf.characteristictypeid, rf.modifierid ORDER BY rf.effectivetime DESC) AS rnk,
            rf.id,
            rf.effectivetime,
            rf.active,
            rf.moduleid,
            rf.sourceid,
            rf.destinationid,
            rf.relationshipgroup,
            rf.typeid,
            rf.characteristictypeid,
            rf.modifierid
           FROM terminology.snomedct_relationship_f rf) tmp
  WHERE (tmp.rnk = 1)
  WITH NO DATA;


ALTER TABLE terminology.snomedct_relationship_latest_mat OWNER TO owner_name;

--
-- Name: snomedct_simplerefset_f; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_simplerefset_f (
    id character varying(50) NOT NULL,
    effectivetime character(8) NOT NULL,
    active character(1) NOT NULL,
    moduleid character varying(18) NOT NULL,
    refsetid character varying(18) NOT NULL,
    referencedcomponentid character varying(18) NOT NULL
);


ALTER TABLE terminology.snomedct_simplerefset_f OWNER TO owner_name;

--
-- Name: snomedct_simplerefset_latest_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_simplerefset_latest_mat AS
 SELECT tmp.id,
    tmp.effectivetime,
    tmp.active,
    tmp.moduleid,
    tmp.refsetid,
    tmp.referencedcomponentid
   FROM ( SELECT rank() OVER (PARTITION BY cf.id ORDER BY cf.effectivetime DESC) AS rnk,
            cf.id,
            cf.effectivetime,
            cf.active,
            cf.moduleid,
            cf.refsetid,
            cf.referencedcomponentid
           FROM terminology.snomedct_simplerefset_f cf) tmp
  WHERE (tmp.rnk = 1)
  WITH NO DATA;


ALTER TABLE terminology.snomedct_simplerefset_latest_mat OWNER TO owner_name;

--
-- Name: snomedct_modified_release_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_modified_release_mat AS
 SELECT DISTINCT sr.refsetid AS mr_id,
        CASE sr.refsetid
            WHEN '999000501000001105'::text THEN '12'::text
            WHEN '999000511000001107'::text THEN '24'::text
            ELSE NULL::text
        END AS mr_cd,
    dp_dest.conceptid AS drug_id,
    dp_dest.term AS drug_term,
    to_tsvector('english'::regconfig, dp_dest.term) AS drug_term_tokens
   FROM (((terminology.snomedct_simplerefset_latest_mat sr
     JOIN terminology.snomedct_relationship_latest_mat rf ON (((sr.referencedcomponentid)::text = (rf.destinationid)::text)))
     JOIN terminology.snomedct_description_latest_mat dp_dest ON (((dp_dest.conceptid)::text = (rf.destinationid)::text)))
     JOIN terminology.snomedct_description_latest_mat dp_src ON (((dp_src.conceptid)::text = (rf.sourceid)::text)))
  WHERE ((((sr.refsetid)::text = '999000501000001105'::text) OR ((sr.refsetid)::text = '999000511000001107'::text)) AND ((dp_dest.typeid)::text = '900000000000013009'::text) AND ((dp_src.typeid)::text = '900000000000013009'::text) AND (sr.active = '1'::bpchar) AND (rf.active = '1'::bpchar) AND (dp_dest.active = '1'::bpchar) AND (dp_src.active = '1'::bpchar))
  WITH NO DATA;


ALTER TABLE terminology.snomedct_modified_release_mat OWNER TO owner_name;

--
-- Name: snomedct_relation_active_isa_lookup_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_relation_active_isa_lookup_mat AS
 SELECT DISTINCT srlm.sourceid,
    srlm.destinationid
   FROM terminology.snomedct_relationship_latest_mat srlm
  WHERE ((srlm.active = '1'::bpchar) AND ((srlm.typeid)::text = '116680003'::text))
  WITH NO DATA;


ALTER TABLE terminology.snomedct_relation_active_isa_lookup_mat OWNER TO owner_name;

--
-- Name: snomedct_relationshipwithnames_latest_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_relationshipwithnames_latest_mat AS
 SELECT relationship.id,
    relationship.effectivetime,
    relationship.active,
    relationship.moduleid,
    cpn1.preferredname AS moduleidname,
    relationship.sourceid,
    cpn2.preferredname AS sourceidname,
    to_tsvector('english'::regconfig, cpn2.preferredname) AS sourceidname_tokens,
    relationship.destinationid,
    cpn3.preferredname AS destinationidname,
    to_tsvector('english'::regconfig, cpn3.preferredname) AS destinationidname_tokens,
    relationship.relationshipgroup,
    relationship.typeid,
    cpn4.preferredname AS typeidname,
    relationship.characteristictypeid,
    cpn5.preferredname AS characteristictypeidname,
    relationship.modifierid,
    cpn6.preferredname AS modifieridname
   FROM terminology.snomedct_relationship_latest_mat relationship,
    terminology.snomedct_conceptpreferredname_latest_mat cpn1,
    terminology.snomedct_conceptpreferredname_latest_mat cpn2,
    terminology.snomedct_conceptpreferredname_latest_mat cpn3,
    terminology.snomedct_conceptpreferredname_latest_mat cpn4,
    terminology.snomedct_conceptpreferredname_latest_mat cpn5,
    terminology.snomedct_conceptpreferredname_latest_mat cpn6
  WHERE (((relationship.moduleid)::text = (cpn1.conceptid)::text) AND ((relationship.sourceid)::text = (cpn2.conceptid)::text) AND ((relationship.destinationid)::text = (cpn3.conceptid)::text) AND ((relationship.typeid)::text = (cpn4.conceptid)::text) AND ((relationship.characteristictypeid)::text = (cpn5.conceptid)::text) AND ((relationship.modifierid)::text = (cpn6.conceptid)::text))
  WITH NO DATA;


ALTER TABLE terminology.snomedct_relationshipwithnames_latest_mat OWNER TO owner_name;

--
-- Name: snomedct_simplemaprefset_f; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_simplemaprefset_f (
    id uuid NOT NULL,
    effectivetime character(8) NOT NULL,
    active character(1) NOT NULL,
    moduleid character varying(18) NOT NULL,
    refsetid character varying(18) NOT NULL,
    referencedcomponentid character varying(18) NOT NULL,
    maptarget text NOT NULL
);


ALTER TABLE terminology.snomedct_simplemaprefset_f OWNER TO owner_name;

--
-- Name: snomedct_stated_relationship_f; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_stated_relationship_f (
    id character varying(18) NOT NULL,
    effectivetime character(8) NOT NULL,
    active character(1) NOT NULL,
    moduleid character varying(18) NOT NULL,
    sourceid character varying(18) NOT NULL,
    destinationid character varying(18) NOT NULL,
    relationshipgroup character varying(18) NOT NULL,
    typeid character varying(18) NOT NULL,
    characteristictypeid character varying(18) NOT NULL,
    modifierid character varying(18) NOT NULL
);


ALTER TABLE terminology.snomedct_stated_relationship_f OWNER TO owner_name;

--
-- Name: snomedct_textdefinition_f; Type: TABLE; Schema: terminology; Owner: owner_name
--

CREATE TABLE terminology.snomedct_textdefinition_f (
    id character varying(18) NOT NULL,
    effectivetime character(8) NOT NULL,
    active character(1) NOT NULL,
    moduleid character varying(18) NOT NULL,
    conceptid character varying(18) NOT NULL,
    languagecode character varying(2) NOT NULL,
    typeid character varying(18) NOT NULL,
    term text NOT NULL,
    casesignificanceid character varying(18) NOT NULL
);


ALTER TABLE terminology.snomedct_textdefinition_f OWNER TO owner_name;

--
-- Name: snomedct_tradefamilies_mat; Type: MATERIALIZED VIEW; Schema: terminology; Owner: owner_name
--

CREATE MATERIALIZED VIEW terminology.snomedct_tradefamilies_mat AS
 SELECT dp_src.conceptid AS branded_drug_id,
    dp_src.term AS branded_drug_term,
    to_tsvector('english'::regconfig, dp_src.term) AS branded_drug_term_tokens,
    dp_src.conceptid AS trade_family_id,
    dp_dest.term AS trade_family_term,
    to_tsvector('english'::regconfig, dp_dest.term) AS trade_family_term_tokens
   FROM (((terminology.snomedct_simplerefset_latest_mat sr
     JOIN terminology.snomedct_relationship_latest_mat rf ON (((sr.referencedcomponentid)::text = (rf.destinationid)::text)))
     JOIN terminology.snomedct_description_latest_mat dp_dest ON (((dp_dest.conceptid)::text = (rf.destinationid)::text)))
     JOIN terminology.snomedct_description_latest_mat dp_src ON (((dp_src.conceptid)::text = (rf.sourceid)::text)))
  WHERE (((sr.refsetid)::text = '999000631000001100'::text) AND ((dp_dest.typeid)::text = '900000000000013009'::text) AND ((dp_src.typeid)::text = '900000000000013009'::text) AND (sr.active = '1'::bpchar) AND (rf.active = '1'::bpchar) AND (dp_dest.active = '1'::bpchar) AND (dp_src.active = '1'::bpchar))
  WITH NO DATA;


ALTER TABLE terminology.snomedct_tradefamilies_mat OWNER TO owner_name;

--
-- Name: atc_lookup; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.atc_lookup (
    atc_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(255),
    "desc" text,
    short_cd character varying(255)
);


ALTER TABLE terminology_staging.atc_lookup OWNER TO owner_name;

--
-- Name: atc_lookup__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.atc_lookup__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.atc_lookup__sequenceid_seq OWNER TO owner_name;

--
-- Name: atc_lookup__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.atc_lookup__sequenceid_seq OWNED BY terminology_staging.atc_lookup._sequenceid;


--
-- Name: bnf_lookup; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.bnf_lookup (
    bnf_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(255),
    name text
);


ALTER TABLE terminology_staging.bnf_lookup OWNER TO owner_name;

--
-- Name: bnf_lookup__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.bnf_lookup__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.bnf_lookup__sequenceid_seq OWNER TO owner_name;

--
-- Name: bnf_lookup__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.bnf_lookup__sequenceid_seq OWNED BY terminology_staging.bnf_lookup._sequenceid;


--
-- Name: dmd_amp; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_amp (
    amp_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    apid character varying(255),
    invalid smallint,
    vpid character varying(255),
    nm character varying(1000),
    abbrevnm character varying(1000),
    "desc" character varying(1000),
    nmdt timestamp without time zone,
    nm_prev character varying(1000),
    suppcd character varying(255),
    lic_authcd bigint,
    lic_auth_prevcd bigint,
    lic_authchangecd bigint,
    lic_authchangedt timestamp without time zone,
    combprodcd bigint,
    flavourcd bigint,
    ema integer,
    parallel_import integer,
    avail_restrictcd bigint,
    col_val_hash uuid
);


ALTER TABLE terminology_staging.dmd_amp OWNER TO owner_name;

--
-- Name: dmd_amp__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_amp__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_amp__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_amp__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_amp__sequenceid_seq OWNED BY terminology_staging.dmd_amp._sequenceid;


--
-- Name: dmd_amp_drugroute; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_amp_drugroute (
    adr_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    apid character varying(255),
    routecd character varying(255),
    col_val_hash uuid
);


ALTER TABLE terminology_staging.dmd_amp_drugroute OWNER TO owner_name;

--
-- Name: dmd_amp_drugroute__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_amp_drugroute__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_amp_drugroute__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_amp_drugroute__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_amp_drugroute__sequenceid_seq OWNED BY terminology_staging.dmd_amp_drugroute._sequenceid;


--
-- Name: dmd_amp_excipient; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_amp_excipient (
    aex_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    apid character varying(255),
    isid character varying(255),
    strnth numeric,
    strnth_uomcd character varying(255),
    col_val_hash uuid
);


ALTER TABLE terminology_staging.dmd_amp_excipient OWNER TO owner_name;

--
-- Name: dmd_amp_excipient__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_amp_excipient__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_amp_excipient__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_amp_excipient__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_amp_excipient__sequenceid_seq OWNED BY terminology_staging.dmd_amp_excipient._sequenceid;


--
-- Name: dmd_atc; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_atc (
    dat_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    atc_cd character varying(255),
    atc_short_cd character varying(255),
    dmd_cd character varying(255)
);


ALTER TABLE terminology_staging.dmd_atc OWNER TO owner_name;

--
-- Name: dmd_atc__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_atc__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_atc__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_atc__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_atc__sequenceid_seq OWNED BY terminology_staging.dmd_atc._sequenceid;


--
-- Name: dmd_bnf; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_bnf (
    dbn_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    bnf_cd character varying(255),
    dmd_cd character varying(255),
    dmd_level character varying(255)
);


ALTER TABLE terminology_staging.dmd_bnf OWNER TO owner_name;

--
-- Name: dmd_bnf__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_bnf__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_bnf__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_bnf__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_bnf__sequenceid_seq OWNED BY terminology_staging.dmd_bnf._sequenceid;


--
-- Name: dmd_lookup_availrestrict; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_availrestrict (
    lar_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology_staging.dmd_lookup_availrestrict OWNER TO owner_name;

--
-- Name: dmd_lookup_availrestrict__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_availrestrict__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_availrestrict__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_availrestrict__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_availrestrict__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_availrestrict._sequenceid;


--
-- Name: dmd_lookup_basisofname; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_basisofname (
    bon_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology_staging.dmd_lookup_basisofname OWNER TO owner_name;

--
-- Name: dmd_lookup_basisofname__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_basisofname__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_basisofname__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_basisofname__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_basisofname__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_basisofname._sequenceid;


--
-- Name: dmd_lookup_basisofstrength; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_basisofstrength (
    lbs_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology_staging.dmd_lookup_basisofstrength OWNER TO owner_name;

--
-- Name: dmd_lookup_basisofstrength__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_basisofstrength__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_basisofstrength__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_basisofstrength__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_basisofstrength__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_basisofstrength._sequenceid;


--
-- Name: dmd_lookup_controldrugcat; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_controldrugcat (
    lcd_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology_staging.dmd_lookup_controldrugcat OWNER TO owner_name;

--
-- Name: dmd_lookup_controldrugcat__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_controldrugcat__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_controldrugcat__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_controldrugcat__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_controldrugcat__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_controldrugcat._sequenceid;


--
-- Name: dmd_lookup_drugformind; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_drugformind (
    lfi_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology_staging.dmd_lookup_drugformind OWNER TO owner_name;

--
-- Name: dmd_lookup_drugformind__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_drugformind__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_drugformind__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_drugformind__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_drugformind__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_drugformind._sequenceid;


--
-- Name: dmd_lookup_form; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_form (
    lfr_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(255),
    cddt timestamp without time zone,
    cdprev character varying(255),
    "desc" character varying(1000)
);


ALTER TABLE terminology_staging.dmd_lookup_form OWNER TO owner_name;

--
-- Name: dmd_lookup_form__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_form__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_form__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_form__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_form__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_form._sequenceid;


--
-- Name: dmd_lookup_ingredient; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_ingredient (
    lin_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    isid character varying(255),
    isiddt timestamp without time zone,
    isidprev character varying(255),
    invalid smallint,
    nm character varying(1000)
);


ALTER TABLE terminology_staging.dmd_lookup_ingredient OWNER TO owner_name;

--
-- Name: dmd_lookup_ingredient__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_ingredient__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_ingredient__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_ingredient__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_ingredient__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_ingredient._sequenceid;


--
-- Name: dmd_lookup_licauth; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_licauth (
    lau_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology_staging.dmd_lookup_licauth OWNER TO owner_name;

--
-- Name: dmd_lookup_licauth__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_licauth__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_licauth__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_licauth__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_licauth__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_licauth._sequenceid;


--
-- Name: dmd_lookup_ontformroute; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_ontformroute (
    ofr_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology_staging.dmd_lookup_ontformroute OWNER TO owner_name;

--
-- Name: dmd_lookup_ontformroute__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_ontformroute__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_ontformroute__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_ontformroute__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_ontformroute__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_ontformroute._sequenceid;


--
-- Name: dmd_lookup_prescribingstatus; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_prescribingstatus (
    lps_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd bigint,
    "desc" character varying(1000)
);


ALTER TABLE terminology_staging.dmd_lookup_prescribingstatus OWNER TO owner_name;

--
-- Name: dmd_lookup_prescribingstatus__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_prescribingstatus__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_prescribingstatus__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_prescribingstatus__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_prescribingstatus__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_prescribingstatus._sequenceid;


--
-- Name: dmd_lookup_route; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_route (
    lrt_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(255),
    cddt timestamp without time zone,
    cdprev character varying(255),
    "desc" character varying(1000),
    source character varying(50)
);


ALTER TABLE terminology_staging.dmd_lookup_route OWNER TO owner_name;

--
-- Name: dmd_lookup_route__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_route__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_route__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_route__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_route__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_route._sequenceid;


--
-- Name: dmd_lookup_supplier; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_supplier (
    lsu_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(255),
    cddt timestamp without time zone,
    cdprev character varying(255),
    invalid smallint,
    "desc" character varying(1000)
);


ALTER TABLE terminology_staging.dmd_lookup_supplier OWNER TO owner_name;

--
-- Name: dmd_lookup_supplier__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_supplier__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_supplier__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_supplier__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_supplier__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_supplier._sequenceid;


--
-- Name: dmd_lookup_uom; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_lookup_uom (
    uom_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    cd character varying(255),
    cddt timestamp without time zone,
    cdprev character varying(255),
    "desc" character varying(1000)
);


ALTER TABLE terminology_staging.dmd_lookup_uom OWNER TO owner_name;

--
-- Name: dmd_lookup_uom__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_lookup_uom__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_lookup_uom__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_lookup_uom__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_lookup_uom__sequenceid_seq OWNED BY terminology_staging.dmd_lookup_uom._sequenceid;


--
-- Name: dmd_sync_log; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_sync_log (
    dmd_id character varying(1000),
    sync_process_id character varying(255),
    dmd_entity_name character varying(255),
    created_dt timestamp with time zone DEFAULT now(),
    row_action character varying(10)
);


ALTER TABLE terminology_staging.dmd_sync_log OWNER TO owner_name;

--
-- Name: dmd_vmp; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_vmp (
    vmp_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vpid character varying(255),
    vpiddt timestamp without time zone,
    vpidprev character varying(255),
    vtmid character varying(255),
    invalid smallint,
    nm character varying(1000),
    abbrevnm character varying(1000),
    basiscd bigint,
    nmdt timestamp without time zone,
    nmprev character varying(1000),
    basis_prevcd bigint,
    nmchangecd bigint,
    comprodcd bigint,
    pres_statcd bigint,
    sug_f integer,
    glu_f integer,
    pres_f integer,
    cfc_f integer,
    non_availcd integer,
    non_availdt timestamp without time zone,
    df_indcd bigint,
    udfs numeric,
    udfs_uomcd character varying(255),
    unit_dose_uomcd character varying(255),
    col_val_hash uuid
);


ALTER TABLE terminology_staging.dmd_vmp OWNER TO owner_name;

--
-- Name: dmd_vmp__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_vmp__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_vmp__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vmp__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_vmp__sequenceid_seq OWNED BY terminology_staging.dmd_vmp._sequenceid;


--
-- Name: dmd_vmp_controldrug; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_vmp_controldrug (
    vcd_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vpid character varying(255),
    catcd bigint,
    catdt timestamp without time zone,
    cat_prevcd bigint,
    col_val_hash uuid
);


ALTER TABLE terminology_staging.dmd_vmp_controldrug OWNER TO owner_name;

--
-- Name: dmd_vmp_controldrug__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_vmp_controldrug__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_vmp_controldrug__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vmp_controldrug__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_vmp_controldrug__sequenceid_seq OWNED BY terminology_staging.dmd_vmp_controldrug._sequenceid;


--
-- Name: dmd_vmp_drugform; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_vmp_drugform (
    vdf_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vpid character varying(255),
    formcd character varying(255),
    col_val_hash uuid
);


ALTER TABLE terminology_staging.dmd_vmp_drugform OWNER TO owner_name;

--
-- Name: dmd_vmp_drugform__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_vmp_drugform__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_vmp_drugform__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vmp_drugform__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_vmp_drugform__sequenceid_seq OWNED BY terminology_staging.dmd_vmp_drugform._sequenceid;


--
-- Name: dmd_vmp_drugroute; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_vmp_drugroute (
    vdr_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vpid character varying(255),
    routecd character varying(255),
    col_val_hash uuid
);


ALTER TABLE terminology_staging.dmd_vmp_drugroute OWNER TO owner_name;

--
-- Name: dmd_vmp_drugroute__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_vmp_drugroute__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_vmp_drugroute__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vmp_drugroute__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_vmp_drugroute__sequenceid_seq OWNED BY terminology_staging.dmd_vmp_drugroute._sequenceid;


--
-- Name: dmd_vmp_ingredient; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_vmp_ingredient (
    vin_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vpid character varying(255),
    isid character varying(255),
    basis_strntcd bigint,
    bs_subid character varying(255),
    strnt_nmrtr_val numeric,
    strnt_nmrtr_uomcd character varying(255),
    strnt_dnmtr_val numeric,
    strnt_dnmtr_uomcd character varying(255),
    col_val_hash uuid
);


ALTER TABLE terminology_staging.dmd_vmp_ingredient OWNER TO owner_name;

--
-- Name: dmd_vmp_ingredient__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_vmp_ingredient__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_vmp_ingredient__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vmp_ingredient__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_vmp_ingredient__sequenceid_seq OWNED BY terminology_staging.dmd_vmp_ingredient._sequenceid;


--
-- Name: dmd_vmp_ontdrugform; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_vmp_ontdrugform (
    odf_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vpid character varying(255),
    formcd bigint,
    col_val_hash uuid
);


ALTER TABLE terminology_staging.dmd_vmp_ontdrugform OWNER TO owner_name;

--
-- Name: dmd_vmp_ontdrugform__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_vmp_ontdrugform__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_vmp_ontdrugform__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vmp_ontdrugform__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_vmp_ontdrugform__sequenceid_seq OWNED BY terminology_staging.dmd_vmp_ontdrugform._sequenceid;


--
-- Name: dmd_vtm; Type: TABLE; Schema: terminology_staging; Owner: owner_name
--

CREATE TABLE terminology_staging.dmd_vtm (
    vtm_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _row_id character varying(255) DEFAULT public.uuid_generate_v4(),
    _sequenceid integer NOT NULL,
    _contextkey character varying(255),
    _createdtimestamp timestamp with time zone DEFAULT timezone('UTC'::text, now()),
    _createddate timestamp without time zone DEFAULT now(),
    _createdsource character varying(255),
    _createdmessageid character varying(255),
    _createdby character varying(255),
    _recordstatus smallint DEFAULT 1,
    _timezonename character varying(255),
    _timezoneoffset integer,
    _tenant character varying(255),
    vtmid character varying(255),
    invalid smallint,
    nm character varying(1000),
    abbrevnm character varying(1000),
    vtmidprev character varying(255),
    vtmiddt timestamp without time zone,
    col_val_hash uuid
);


ALTER TABLE terminology_staging.dmd_vtm OWNER TO owner_name;

--
-- Name: dmd_vtm__sequenceid_seq; Type: SEQUENCE; Schema: terminology_staging; Owner: owner_name
--

CREATE SEQUENCE terminology_staging.dmd_vtm__sequenceid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE terminology_staging.dmd_vtm__sequenceid_seq OWNER TO owner_name;

--
-- Name: dmd_vtm__sequenceid_seq; Type: SEQUENCE OWNED BY; Schema: terminology_staging; Owner: owner_name
--

ALTER SEQUENCE terminology_staging.dmd_vtm__sequenceid_seq OWNED BY terminology_staging.dmd_vtm._sequenceid;


--
-- Name: formulary_rule_config _sequenceid; Type: DEFAULT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_rule_config ALTER COLUMN _sequenceid SET DEFAULT nextval('local_formulary.formulary_rule_config__sequenceid_seq'::regclass);


--
-- Name: lookup_common _sequenceid; Type: DEFAULT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.lookup_common ALTER COLUMN _sequenceid SET DEFAULT nextval('local_formulary.lookup_common__sequenceid_seq'::regclass);


--
-- Name: atc_lookup _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.atc_lookup ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.atc_lookup__sequenceid_seq'::regclass);


--
-- Name: bnf_lookup _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.bnf_lookup ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.bnf_lookup__sequenceid_seq'::regclass);


--
-- Name: dmd_amp _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_amp ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_amp__sequenceid_seq'::regclass);


--
-- Name: dmd_amp_drugroute _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_amp_drugroute ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_amp_drugroute__sequenceid_seq'::regclass);


--
-- Name: dmd_amp_excipient _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_amp_excipient ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_amp_excipient__sequenceid_seq'::regclass);


--
-- Name: dmd_atc _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_atc ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_atc__sequenceid_seq'::regclass);


--
-- Name: dmd_bnf _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_bnf ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_bnf__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_availrestrict _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_availrestrict ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_availrestrict__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_basisofname _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_basisofname ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_basisofname__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_basisofstrength _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_basisofstrength ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_basisofstrength__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_controldrugcat _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_controldrugcat ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_controldrugcat__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_drugformind _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_drugformind ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_drugformind__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_form _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_form ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_form__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_ingredient _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_ingredient ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_ingredient__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_licauth _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_licauth ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_licauth__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_ontformroute _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_ontformroute ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_ontformroute__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_prescribingstatus _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_prescribingstatus ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_prescribingstatus__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_route _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_route ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_route__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_supplier _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_supplier ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_supplier__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_uom _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_lookup_uom ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_lookup_uom__sequenceid_seq'::regclass);


--
-- Name: dmd_sync_log serial_num; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_sync_log ALTER COLUMN serial_num SET DEFAULT nextval('terminology.dmd_sync_log_serial_num_seq'::regclass);


--
-- Name: dmd_vmp _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_vmp ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_vmp__sequenceid_seq'::regclass);


--
-- Name: dmd_vmp_controldrug _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_vmp_controldrug ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_vmp_controldrug__sequenceid_seq'::regclass);


--
-- Name: dmd_vmp_drugform _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_vmp_drugform ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_vmp_drugform__sequenceid_seq'::regclass);


--
-- Name: dmd_vmp_drugroute _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_vmp_drugroute ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_vmp_drugroute__sequenceid_seq'::regclass);


--
-- Name: dmd_vmp_ingredient _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_vmp_ingredient ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_vmp_ingredient__sequenceid_seq'::regclass);


--
-- Name: dmd_vmp_ontdrugform _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_vmp_ontdrugform ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_vmp_ontdrugform__sequenceid_seq'::regclass);


--
-- Name: dmd_vtm _sequenceid; Type: DEFAULT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_vtm ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology.dmd_vtm__sequenceid_seq'::regclass);


--
-- Name: atc_lookup _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.atc_lookup ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.atc_lookup__sequenceid_seq'::regclass);


--
-- Name: bnf_lookup _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.bnf_lookup ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.bnf_lookup__sequenceid_seq'::regclass);


--
-- Name: dmd_amp _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_amp ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_amp__sequenceid_seq'::regclass);


--
-- Name: dmd_amp_drugroute _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_amp_drugroute ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_amp_drugroute__sequenceid_seq'::regclass);


--
-- Name: dmd_amp_excipient _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_amp_excipient ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_amp_excipient__sequenceid_seq'::regclass);


--
-- Name: dmd_atc _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_atc ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_atc__sequenceid_seq'::regclass);


--
-- Name: dmd_bnf _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_bnf ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_bnf__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_availrestrict _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_availrestrict ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_availrestrict__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_basisofname _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_basisofname ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_basisofname__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_basisofstrength _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_basisofstrength ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_basisofstrength__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_controldrugcat _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_controldrugcat ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_controldrugcat__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_drugformind _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_drugformind ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_drugformind__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_form _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_form ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_form__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_ingredient _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_ingredient ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_ingredient__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_licauth _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_licauth ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_licauth__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_ontformroute _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_ontformroute ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_ontformroute__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_prescribingstatus _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_prescribingstatus ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_prescribingstatus__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_route _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_route ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_route__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_supplier _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_supplier ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_supplier__sequenceid_seq'::regclass);


--
-- Name: dmd_lookup_uom _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_lookup_uom ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_lookup_uom__sequenceid_seq'::regclass);


--
-- Name: dmd_vmp _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_vmp ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_vmp__sequenceid_seq'::regclass);


--
-- Name: dmd_vmp_controldrug _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_vmp_controldrug ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_vmp_controldrug__sequenceid_seq'::regclass);


--
-- Name: dmd_vmp_drugform _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_vmp_drugform ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_vmp_drugform__sequenceid_seq'::regclass);


--
-- Name: dmd_vmp_drugroute _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_vmp_drugroute ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_vmp_drugroute__sequenceid_seq'::regclass);


--
-- Name: dmd_vmp_ingredient _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_vmp_ingredient ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_vmp_ingredient__sequenceid_seq'::regclass);


--
-- Name: dmd_vmp_ontdrugform _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_vmp_ontdrugform ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_vmp_ontdrugform__sequenceid_seq'::regclass);


--
-- Name: dmd_vtm _sequenceid; Type: DEFAULT; Schema: terminology_staging; Owner: owner_name
--

ALTER TABLE ONLY terminology_staging.dmd_vtm ALTER COLUMN _sequenceid SET DEFAULT nextval('terminology_staging.dmd_vtm__sequenceid_seq'::regclass);


--
-- Name: formulary_additional_code formulary_additional_code_pk; Type: CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_additional_code
    ADD CONSTRAINT formulary_additional_code_pk PRIMARY KEY (_row_id);


--
-- Name: formulary_detail formulary_detail_pk; Type: CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_detail
    ADD CONSTRAINT formulary_detail_pk PRIMARY KEY (_row_id);


--
-- Name: formulary_excipient formulary_excipient_pk; Type: CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_excipient
    ADD CONSTRAINT formulary_excipient_pk PRIMARY KEY (_row_id);


--
-- Name: formulary_header formulary_header_pk; Type: CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_header
    ADD CONSTRAINT formulary_header_pk PRIMARY KEY (formulary_version_id);


--
-- Name: formulary_indication formulary_indication_pk; Type: CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_indication
    ADD CONSTRAINT formulary_indication_pk PRIMARY KEY (_row_id);


--
-- Name: formulary_ingredient formulary_ingredient_pk; Type: CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_ingredient
    ADD CONSTRAINT formulary_ingredient_pk PRIMARY KEY (_row_id);


--
-- Name: formulary_local_route_detail formulary_local_route_detail_pk; Type: CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_local_route_detail
    ADD CONSTRAINT formulary_local_route_detail_pk PRIMARY KEY (_row_id);


--
-- Name: formulary_ontology_form formulary_ontology_form_pk; Type: CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_ontology_form
    ADD CONSTRAINT formulary_ontology_form_pk PRIMARY KEY (_row_id);


--
-- Name: formulary_route_detail formulary_route_detail_pk; Type: CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_route_detail
    ADD CONSTRAINT formulary_route_detail_pk PRIMARY KEY (_row_id);


--
-- Name: formulary_rule_config formulary_rule_config_pkey; Type: CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_rule_config
    ADD CONSTRAINT formulary_rule_config_pkey PRIMARY KEY (_row_id);


--
-- Name: dmd_sync_log dmd_sync_log_pk; Type: CONSTRAINT; Schema: terminology; Owner: owner_name
--

ALTER TABLE ONLY terminology.dmd_sync_log
    ADD CONSTRAINT dmd_sync_log_pk PRIMARY KEY (sl_id);


--
-- Name: formulary_additional_code_formulary_version_id_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_additional_code_formulary_version_id_idx ON local_formulary.formulary_additional_code USING btree (formulary_version_id);


--
-- Name: formulary_detail_formulary_version_id_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_detail_formulary_version_id_idx ON local_formulary.formulary_detail USING btree (formulary_version_id);


--
-- Name: formulary_detail_is_diluent_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_detail_is_diluent_idx ON local_formulary.formulary_detail USING btree (is_diluent);


--
-- Name: formulary_detail_rnoh_formulary_statuscd_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_detail_rnoh_formulary_statuscd_idx ON local_formulary.formulary_detail USING btree (rnoh_formulary_statuscd);


--
-- Name: formulary_excipient_formulary_version_id_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_excipient_formulary_version_id_idx ON local_formulary.formulary_excipient USING btree (formulary_version_id);


--
-- Name: formulary_header_code_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_header_code_idx ON local_formulary.formulary_header USING btree (code);


--
-- Name: formulary_header_formulary_version_id_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_header_formulary_version_id_idx ON local_formulary.formulary_header USING btree (formulary_version_id);


--
-- Name: formulary_header_is_latest_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_header_is_latest_idx ON local_formulary.formulary_header USING btree (is_latest);


--
-- Name: formulary_header_name_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_header_name_idx ON local_formulary.formulary_header USING btree (name);


--
-- Name: formulary_header_name_tokens_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_header_name_tokens_idx ON local_formulary.formulary_header USING gin (name_tokens);


--
-- Name: formulary_header_parent_code_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_header_parent_code_idx ON local_formulary.formulary_header USING btree (parent_code);


--
-- Name: formulary_header_product_type_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_header_product_type_idx ON local_formulary.formulary_header USING btree (product_type);


--
-- Name: formulary_header_rec_status_code_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_header_rec_status_code_idx ON local_formulary.formulary_header USING btree (rec_status_code);


--
-- Name: formulary_ingredient_formulary_version_id_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_ingredient_formulary_version_id_idx ON local_formulary.formulary_ingredient USING btree (formulary_version_id);


--
-- Name: formulary_local_route_detail_formulary_version_id_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_local_route_detail_formulary_version_id_idx ON local_formulary.formulary_local_route_detail USING btree (formulary_version_id);


--
-- Name: formulary_route_detail_formulary_version_id_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX formulary_route_detail_formulary_version_id_idx ON local_formulary.formulary_route_detail USING btree (formulary_version_id);


--
-- Name: lookup_common_cd_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX lookup_common_cd_idx ON local_formulary.lookup_common USING btree (cd);


--
-- Name: lookup_common_desc_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX lookup_common_desc_idx ON local_formulary.lookup_common USING btree ("desc");


--
-- Name: lookup_common_type_idx; Type: INDEX; Schema: local_formulary; Owner: owner_name
--

CREATE INDEX lookup_common_type_idx ON local_formulary.lookup_common USING btree (type);


--
-- Name: dmd_names_lookup_all_mat_code_hash_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_code_hash_idx ON terminology.dmd_names_lookup_all_mat USING hash (code);


--
-- Name: dmd_names_lookup_all_mat_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_code_idx ON terminology.dmd_names_lookup_all_mat USING btree (code);


--
-- Name: dmd_names_lookup_all_mat_control_drug_category_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_control_drug_category_code_idx ON terminology.dmd_names_lookup_all_mat USING btree (control_drug_category_code);


--
-- Name: dmd_names_lookup_all_mat_control_drug_category_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_control_drug_category_idx ON terminology.dmd_names_lookup_all_mat USING btree (control_drug_category);


--
-- Name: dmd_names_lookup_all_mat_name_tokens_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_name_tokens_idx ON terminology.dmd_names_lookup_all_mat USING gin (name_tokens);


--
-- Name: dmd_names_lookup_all_mat_prescribing_status_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_prescribing_status_code_idx ON terminology.dmd_names_lookup_all_mat USING btree (prescribing_status_code);


--
-- Name: dmd_names_lookup_all_mat_prescribing_status_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_prescribing_status_idx ON terminology.dmd_names_lookup_all_mat USING btree (prescribing_status);


--
-- Name: dmd_names_lookup_all_mat_supplier_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_supplier_code_idx ON terminology.dmd_names_lookup_all_mat USING btree (supplier_code);


--
-- Name: dmd_names_lookup_all_mat_supplier_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_supplier_idx ON terminology.dmd_names_lookup_all_mat USING btree (supplier);


--
-- Name: dmd_names_lookup_all_mat_supplier_name_tokens_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_supplier_name_tokens_idx ON terminology.dmd_names_lookup_all_mat USING gin (supplier_name_tokens);


--
-- Name: dmd_names_lookup_all_mat_vmpform_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_vmpform_code_idx ON terminology.dmd_names_lookup_all_mat USING btree (vmpform_code);


--
-- Name: dmd_names_lookup_all_mat_vmpform_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_vmpform_idx ON terminology.dmd_names_lookup_all_mat USING btree (vmpform);


--
-- Name: dmd_names_lookup_all_mat_vmproute_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_vmproute_code_idx ON terminology.dmd_names_lookup_all_mat USING btree (vmproute_code);


--
-- Name: dmd_names_lookup_all_mat_vmproute_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_all_mat_vmproute_idx ON terminology.dmd_names_lookup_all_mat USING btree (vmproute);


--
-- Name: dmd_names_lookup_mat_code_hash_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_code_hash_idx ON terminology.dmd_names_lookup_mat USING hash (code);


--
-- Name: dmd_names_lookup_mat_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_code_idx ON terminology.dmd_names_lookup_mat USING btree (code);


--
-- Name: dmd_names_lookup_mat_control_drug_category_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_control_drug_category_code_idx ON terminology.dmd_names_lookup_mat USING btree (control_drug_category_code);


--
-- Name: dmd_names_lookup_mat_control_drug_category_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_control_drug_category_idx ON terminology.dmd_names_lookup_mat USING btree (control_drug_category);


--
-- Name: dmd_names_lookup_mat_name_tokens_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_name_tokens_idx ON terminology.dmd_names_lookup_mat USING gin (name_tokens);


--
-- Name: dmd_names_lookup_mat_prescribing_status_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_prescribing_status_code_idx ON terminology.dmd_names_lookup_mat USING btree (prescribing_status_code);


--
-- Name: dmd_names_lookup_mat_prescribing_status_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_prescribing_status_idx ON terminology.dmd_names_lookup_mat USING btree (prescribing_status);


--
-- Name: dmd_names_lookup_mat_supplier_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_supplier_code_idx ON terminology.dmd_names_lookup_mat USING btree (supplier_code);


--
-- Name: dmd_names_lookup_mat_supplier_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_supplier_idx ON terminology.dmd_names_lookup_mat USING btree (supplier);


--
-- Name: dmd_names_lookup_mat_supplier_name_tokens_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_supplier_name_tokens_idx ON terminology.dmd_names_lookup_mat USING gin (supplier_name_tokens);


--
-- Name: dmd_names_lookup_mat_vmpform_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_vmpform_code_idx ON terminology.dmd_names_lookup_mat USING btree (vmpform_code);


--
-- Name: dmd_names_lookup_mat_vmpform_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_vmpform_idx ON terminology.dmd_names_lookup_mat USING btree (vmpform);


--
-- Name: dmd_names_lookup_mat_vmproute_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_vmproute_code_idx ON terminology.dmd_names_lookup_mat USING btree (vmproute_code);


--
-- Name: dmd_names_lookup_mat_vmproute_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_names_lookup_mat_vmproute_idx ON terminology.dmd_names_lookup_mat USING btree (vmproute);


--
-- Name: dmd_relationships_mat_code_hash_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_relationships_mat_code_hash_idx ON terminology.dmd_relationships_mat USING hash (code);


--
-- Name: dmd_relationships_mat_level_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_relationships_mat_level_idx ON terminology.dmd_relationships_mat USING btree (level);


--
-- Name: dmd_relationships_mat_logical_level_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_relationships_mat_logical_level_idx ON terminology.dmd_relationships_mat USING hash (logical_level);


--
-- Name: dmd_relationships_mat_parent_code_code_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_relationships_mat_parent_code_code_idx ON terminology.dmd_relationships_mat USING btree (parent_code) INCLUDE (code);


--
-- Name: dmd_relationships_mat_parent_code_hash_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_relationships_mat_parent_code_hash_idx ON terminology.dmd_relationships_mat USING hash (parent_code);


--
-- Name: dmd_relationships_mat_parent_level_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_relationships_mat_parent_level_idx ON terminology.dmd_relationships_mat USING btree (parent_level);


--
-- Name: dmd_relationships_mat_parent_logical_level_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX dmd_relationships_mat_parent_logical_level_idx ON terminology.dmd_relationships_mat USING hash (parent_logical_level);


--
-- Name: dmd_relationships_mat_unq_code_parent_code__idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE UNIQUE INDEX dmd_relationships_mat_unq_code_parent_code__idx ON terminology.dmd_relationships_mat USING btree (code, parent_code);


--
-- Name: snomedct_associationrefset_f_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_associationrefset_f_idx ON terminology.snomedct_associationrefset_f USING btree (referencedcomponentid, targetcomponentid);


--
-- Name: snomedct_attributevaluerefset_f_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_attributevaluerefset_f_idx ON terminology.snomedct_attributevaluerefset_f USING btree (referencedcomponentid, valueid);


--
-- Name: snomedct_complexmaprefset_referencedcomponentid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_complexmaprefset_referencedcomponentid_idx ON terminology.snomedct_complexmaprefset_f USING btree (referencedcomponentid);


--
-- Name: snomedct_concept_all_lookup_mat_conceptid_hash_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_all_lookup_mat_conceptid_hash_idx ON terminology.snomedct_concept_all_lookup_mat USING hash (conceptid);


--
-- Name: snomedct_concept_all_lookup_mat_mat_fsn_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_all_lookup_mat_mat_fsn_idx ON terminology.snomedct_concept_all_lookup_mat USING btree (fsn);


--
-- Name: snomedct_concept_all_lookup_mat_name_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_all_lookup_mat_name_idx ON terminology.snomedct_concept_all_lookup_mat USING btree (preferredterm);


--
-- Name: snomedct_concept_all_lookup_mat_name_tokens_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_all_lookup_mat_name_tokens_idx ON terminology.snomedct_concept_all_lookup_mat USING gin (preferredname_tokens);


--
-- Name: snomedct_concept_all_lookup_mat_name_tokens_tree_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_all_lookup_mat_name_tokens_tree_idx ON terminology.snomedct_concept_all_lookup_mat USING btree (preferredname_tokens);


--
-- Name: snomedct_concept_all_lookup_mat_semantictag_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_all_lookup_mat_semantictag_idx ON terminology.snomedct_concept_all_lookup_mat USING btree (lower(semantictag));


--
-- Name: snomedct_concept_allname_latest_mat_conceptid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_allname_latest_mat_conceptid_idx ON terminology.snomedct_concept_allname_latest_mat USING btree (conceptid);


--
-- Name: snomedct_concept_allname_latest_mat_descriptionid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_allname_latest_mat_descriptionid_idx ON terminology.snomedct_concept_allname_latest_mat USING btree (descriptionid);


--
-- Name: snomedct_concept_allname_latest_mat_name_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_allname_latest_mat_name_idx ON terminology.snomedct_concept_allname_latest_mat USING btree (name);


--
-- Name: snomedct_concept_allname_latest_mat_name_tokens_i; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_allname_latest_mat_name_tokens_i ON terminology.snomedct_concept_allname_latest_mat USING gin (name_tokens);


--
-- Name: snomedct_concept_latest_mat_active_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_latest_mat_active_idx ON terminology.snomedct_concept_latest_mat USING btree (active);


--
-- Name: snomedct_concept_latest_mat_id_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_latest_mat_id_idx ON terminology.snomedct_concept_latest_mat USING btree (id);


--
-- Name: snomedct_concept_latest_mat_moduleid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_latest_mat_moduleid_idx ON terminology.snomedct_concept_latest_mat USING btree (moduleid);


--
-- Name: snomedct_concept_lookup_mat_conceptid_hash_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_lookup_mat_conceptid_hash_idx ON terminology.snomedct_concept_lookup_mat USING hash (conceptid);


--
-- Name: snomedct_concept_lookup_mat_fsn_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_lookup_mat_fsn_idx ON terminology.snomedct_concept_lookup_mat USING btree (fsn);


--
-- Name: snomedct_concept_lookup_mat_preferredname_tokens_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_lookup_mat_preferredname_tokens_idx ON terminology.snomedct_concept_lookup_mat USING gin (preferredname_tokens);


--
-- Name: snomedct_concept_lookup_mat_preferredname_tokens_tree_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_lookup_mat_preferredname_tokens_tree_idx ON terminology.snomedct_concept_lookup_mat USING btree (preferredname_tokens);


--
-- Name: snomedct_concept_lookup_mat_preferredterm_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_lookup_mat_preferredterm_idx ON terminology.snomedct_concept_lookup_mat USING btree (preferredterm);


--
-- Name: snomedct_concept_lookup_mat_semantictag_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_concept_lookup_mat_semantictag_idx ON terminology.snomedct_concept_lookup_mat USING btree (lower(semantictag));


--
-- Name: snomedct_conceptpreferredname_latest_mat_conceptid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_conceptpreferredname_latest_mat_conceptid_idx ON terminology.snomedct_conceptpreferredname_latest_mat USING btree (conceptid);


--
-- Name: snomedct_conceptpreferredname_latest_mat_descriptionid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_conceptpreferredname_latest_mat_descriptionid_idx ON terminology.snomedct_conceptpreferredname_latest_mat USING btree (descriptionid);


--
-- Name: snomedct_conceptpreferredname_latest_mat_preferredname_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_conceptpreferredname_latest_mat_preferredname_idx ON terminology.snomedct_conceptpreferredname_latest_mat USING btree (preferredname);


--
-- Name: snomedct_conceptpreferredname_latest_mat_preferredname_tokens_i; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_conceptpreferredname_latest_mat_preferredname_tokens_i ON terminology.snomedct_conceptpreferredname_latest_mat USING gin (preferredname_tokens);


--
-- Name: snomedct_description_conceptid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_description_conceptid_idx ON terminology.snomedct_description_f USING btree (conceptid);


--
-- Name: snomedct_description_latest_mat_active_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_description_latest_mat_active_idx ON terminology.snomedct_description_latest_mat USING btree (active);


--
-- Name: snomedct_description_latest_mat_conceptid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_description_latest_mat_conceptid_idx ON terminology.snomedct_description_latest_mat USING btree (conceptid);


--
-- Name: snomedct_description_latest_mat_languagecode_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_description_latest_mat_languagecode_idx ON terminology.snomedct_description_latest_mat USING btree (languagecode);


--
-- Name: snomedct_description_latest_mat_term_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_description_latest_mat_term_idx ON terminology.snomedct_description_latest_mat USING btree (term);


--
-- Name: snomedct_description_latest_mat_term_tokens_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_description_latest_mat_term_tokens_idx ON terminology.snomedct_description_latest_mat USING gin (term_tokens);


--
-- Name: snomedct_description_latest_mat_typeid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_description_latest_mat_typeid_idx ON terminology.snomedct_description_latest_mat USING btree (typeid);


--
-- Name: snomedct_extendedmaprefset_referencedcomponentid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_extendedmaprefset_referencedcomponentid_idx ON terminology.snomedct_extendedmaprefset_f USING btree (referencedcomponentid);


--
-- Name: snomedct_langrefset_latest_mat_acceptabilityid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_langrefset_latest_mat_acceptabilityid_idx ON terminology.snomedct_langrefset_latest_mat USING btree (acceptabilityid);


--
-- Name: snomedct_langrefset_latest_mat_active_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_langrefset_latest_mat_active_idx ON terminology.snomedct_langrefset_latest_mat USING btree (active);


--
-- Name: snomedct_langrefset_latest_mat_moduleid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_langrefset_latest_mat_moduleid_idx ON terminology.snomedct_langrefset_latest_mat USING btree (moduleid);


--
-- Name: snomedct_langrefset_latest_mat_referencedcomponentid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_langrefset_latest_mat_referencedcomponentid_idx ON terminology.snomedct_langrefset_latest_mat USING btree (referencedcomponentid);


--
-- Name: snomedct_langrefset_latest_mat_refsetid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_langrefset_latest_mat_refsetid_idx ON terminology.snomedct_langrefset_latest_mat USING btree (refsetid);


--
-- Name: snomedct_langrefset_referencedcomponentid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_langrefset_referencedcomponentid_idx ON terminology.snomedct_langrefset_f USING btree (referencedcomponentid);


--
-- Name: snomedct_lkp_semantictag_domain_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_lkp_semantictag_domain_idx ON terminology.snomedct_lookup_semantictag USING btree (domain, tag);


--
-- Name: snomedct_modified_release_mat_drug_id_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_modified_release_mat_drug_id_idx ON terminology.snomedct_modified_release_mat USING btree (drug_id);


--
-- Name: snomedct_modified_release_mat_drug_term_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_modified_release_mat_drug_term_idx ON terminology.snomedct_modified_release_mat USING btree (drug_term);


--
-- Name: snomedct_modified_release_mat_drug_term_tokens_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_modified_release_mat_drug_term_tokens_idx ON terminology.snomedct_modified_release_mat USING gin (drug_term_tokens);


--
-- Name: snomedct_modified_release_mat_mr_id_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_modified_release_mat_mr_id_idx ON terminology.snomedct_modified_release_mat USING btree (mr_id);


--
-- Name: snomedct_relation_active_isa_lookup_mat_destinationid_hash_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relation_active_isa_lookup_mat_destinationid_hash_idx ON terminology.snomedct_relation_active_isa_lookup_mat USING hash (destinationid);


--
-- Name: snomedct_relation_active_isa_lookup_mat_destinationid_srcid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relation_active_isa_lookup_mat_destinationid_srcid_idx ON terminology.snomedct_relation_active_isa_lookup_mat USING btree (destinationid) INCLUDE (sourceid);


--
-- Name: snomedct_relation_active_isa_lookup_mat_sourceid_hash_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relation_active_isa_lookup_mat_sourceid_hash_idx ON terminology.snomedct_relation_active_isa_lookup_mat USING hash (sourceid);


--
-- Name: snomedct_relation_active_isa_lookup_mat_srciddestinationid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE UNIQUE INDEX snomedct_relation_active_isa_lookup_mat_srciddestinationid_idx ON terminology.snomedct_relation_active_isa_lookup_mat USING btree (sourceid, destinationid);


--
-- Name: snomedct_relationship_f_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationship_f_idx ON terminology.snomedct_relationship_f USING btree (sourceid, destinationid);


--
-- Name: snomedct_relationship_latest_mat_active_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationship_latest_mat_active_idx ON terminology.snomedct_relationship_latest_mat USING btree (active);


--
-- Name: snomedct_relationship_latest_mat_destinationid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationship_latest_mat_destinationid_idx ON terminology.snomedct_relationship_latest_mat USING btree (destinationid);


--
-- Name: snomedct_relationship_latest_mat_moduleid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationship_latest_mat_moduleid_idx ON terminology.snomedct_relationship_latest_mat USING btree (moduleid);


--
-- Name: snomedct_relationship_latest_mat_sourceid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationship_latest_mat_sourceid_idx ON terminology.snomedct_relationship_latest_mat USING btree (sourceid);


--
-- Name: snomedct_relationship_latest_mat_typeid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationship_latest_mat_typeid_idx ON terminology.snomedct_relationship_latest_mat USING btree (typeid);


--
-- Name: snomedct_relationshipwithnames_latest_mat_active_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationshipwithnames_latest_mat_active_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (active);


--
-- Name: snomedct_relationshipwithnames_latest_mat_destinationid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationshipwithnames_latest_mat_destinationid_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (destinationid);


--
-- Name: snomedct_relationshipwithnames_latest_mat_destinationidname_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationshipwithnames_latest_mat_destinationidname_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (destinationidname);


--
-- Name: snomedct_relationshipwithnames_latest_mat_destinationidname_tok; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationshipwithnames_latest_mat_destinationidname_tok ON terminology.snomedct_relationshipwithnames_latest_mat USING gin (destinationidname_tokens);


--
-- Name: snomedct_relationshipwithnames_latest_mat_moduleidname_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationshipwithnames_latest_mat_moduleidname_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (moduleidname);


--
-- Name: snomedct_relationshipwithnames_latest_mat_sourceid_destid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationshipwithnames_latest_mat_sourceid_destid_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (sourceid, destinationid);


--
-- Name: snomedct_relationshipwithnames_latest_mat_sourceid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationshipwithnames_latest_mat_sourceid_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (sourceid);


--
-- Name: snomedct_relationshipwithnames_latest_mat_sourceidname_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationshipwithnames_latest_mat_sourceidname_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (sourceidname);


--
-- Name: snomedct_relationshipwithnames_latest_mat_sourceidname_tokens_i; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationshipwithnames_latest_mat_sourceidname_tokens_i ON terminology.snomedct_relationshipwithnames_latest_mat USING gin (sourceidname_tokens);


--
-- Name: snomedct_relationshipwithnames_latest_mat_typeid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationshipwithnames_latest_mat_typeid_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (typeid);


--
-- Name: snomedct_relationshipwithnames_latest_mat_typeidname_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_relationshipwithnames_latest_mat_typeidname_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (typeidname);


--
-- Name: snomedct_simplerefset_latest_mat_active_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_simplerefset_latest_mat_active_idx ON terminology.snomedct_simplerefset_latest_mat USING btree (active);


--
-- Name: snomedct_simplerefset_latest_mat_id_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_simplerefset_latest_mat_id_idx ON terminology.snomedct_simplerefset_latest_mat USING btree (id);


--
-- Name: snomedct_simplerefset_latest_mat_moduleid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_simplerefset_latest_mat_moduleid_idx ON terminology.snomedct_simplerefset_latest_mat USING btree (moduleid);


--
-- Name: snomedct_simplerefset_latest_mat_referencedcomponentid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_simplerefset_latest_mat_referencedcomponentid_idx ON terminology.snomedct_simplerefset_latest_mat USING btree (referencedcomponentid);


--
-- Name: snomedct_simplerefset_latest_mat_refsetid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_simplerefset_latest_mat_refsetid_idx ON terminology.snomedct_simplerefset_latest_mat USING btree (refsetid);


--
-- Name: snomedct_simplerefset_referencedcomponentid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_simplerefset_referencedcomponentid_idx ON terminology.snomedct_simplerefset_f USING btree (referencedcomponentid);


--
-- Name: snomedct_stated_relationship_f_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_stated_relationship_f_idx ON terminology.snomedct_stated_relationship_f USING btree (sourceid, destinationid);


--
-- Name: snomedct_textdefinition_conceptid_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_textdefinition_conceptid_idx ON terminology.snomedct_textdefinition_f USING btree (conceptid);


--
-- Name: snomedct_tradefamilies_mat_branded_drug_id_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_tradefamilies_mat_branded_drug_id_idx ON terminology.snomedct_tradefamilies_mat USING btree (branded_drug_id);


--
-- Name: snomedct_tradefamilies_mat_branded_drug_term_tokens_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_tradefamilies_mat_branded_drug_term_tokens_idx ON terminology.snomedct_tradefamilies_mat USING gin (branded_drug_term_tokens);


--
-- Name: snomedct_tradefamilies_mat_trade_family_id_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_tradefamilies_mat_trade_family_id_idx ON terminology.snomedct_tradefamilies_mat USING btree (trade_family_id);


--
-- Name: snomedct_tradefamilies_mat_trade_family_term_tokens_idx; Type: INDEX; Schema: terminology; Owner: owner_name
--

CREATE INDEX snomedct_tradefamilies_mat_trade_family_term_tokens_idx ON terminology.snomedct_tradefamilies_mat USING gin (trade_family_term_tokens);


--
-- Name: formulary_header udt_formulary_record_changes; Type: TRIGGER; Schema: local_formulary; Owner: owner_name
--

CREATE TRIGGER udt_formulary_record_changes AFTER INSERT OR UPDATE OF name ON local_formulary.formulary_header FOR EACH ROW EXECUTE FUNCTION local_formulary.udf_update_formulary_record_changes();


--
-- Name: dmd_amp_drugroute dmd_amp_drugroute_hash_update; Type: TRIGGER; Schema: terminology; Owner: owner_name
--

CREATE TRIGGER dmd_amp_drugroute_hash_update BEFORE INSERT OR UPDATE ON terminology.dmd_amp_drugroute FOR EACH ROW EXECUTE FUNCTION terminology.udf_tg_dmd_amp_drugroute_hash_update();


--
-- Name: dmd_amp_excipient dmd_amp_excipient_hash_update; Type: TRIGGER; Schema: terminology; Owner: owner_name
--

CREATE TRIGGER dmd_amp_excipient_hash_update BEFORE INSERT OR UPDATE ON terminology.dmd_amp_excipient FOR EACH ROW EXECUTE FUNCTION terminology.udf_tg_dmd_amp_excipient_hash_update();


--
-- Name: dmd_amp dmd_amp_hash_update; Type: TRIGGER; Schema: terminology; Owner: owner_name
--

CREATE TRIGGER dmd_amp_hash_update BEFORE INSERT OR UPDATE ON terminology.dmd_amp FOR EACH ROW EXECUTE FUNCTION terminology.udf_tg_dmd_amp_hash_update();


--
-- Name: dmd_vmp_controldrug dmd_vmp_controldrug_hash_update; Type: TRIGGER; Schema: terminology; Owner: owner_name
--

CREATE TRIGGER dmd_vmp_controldrug_hash_update BEFORE INSERT OR UPDATE ON terminology.dmd_vmp_controldrug FOR EACH ROW EXECUTE FUNCTION terminology.udf_tg_dmd_vmp_controldrug_hash_update();


--
-- Name: dmd_vmp_drugform dmd_vmp_drugform_hash_update; Type: TRIGGER; Schema: terminology; Owner: owner_name
--

CREATE TRIGGER dmd_vmp_drugform_hash_update BEFORE INSERT OR UPDATE ON terminology.dmd_vmp_drugform FOR EACH ROW EXECUTE FUNCTION terminology.udf_tg_dmd_vmp_drugform_hash_update();


--
-- Name: dmd_vmp_drugroute dmd_vmp_drugroute_hash_update; Type: TRIGGER; Schema: terminology; Owner: owner_name
--

CREATE TRIGGER dmd_vmp_drugroute_hash_update BEFORE INSERT OR UPDATE ON terminology.dmd_vmp_drugroute FOR EACH ROW EXECUTE FUNCTION terminology.udf_tg_dmd_vmp_drugroute_hash_update();


--
-- Name: dmd_vmp dmd_vmp_hash_update; Type: TRIGGER; Schema: terminology; Owner: owner_name
--

CREATE TRIGGER dmd_vmp_hash_update BEFORE INSERT OR UPDATE ON terminology.dmd_vmp FOR EACH ROW EXECUTE FUNCTION terminology.udf_tg_dmd_vmp_hash_update();


--
-- Name: dmd_vmp_ingredient dmd_vmp_ingredient_hash_update; Type: TRIGGER; Schema: terminology; Owner: owner_name
--

CREATE TRIGGER dmd_vmp_ingredient_hash_update BEFORE INSERT OR UPDATE ON terminology.dmd_vmp_ingredient FOR EACH ROW EXECUTE FUNCTION terminology.udf_tg_dmd_vmp_ingredient_hash_update();


--
-- Name: dmd_vmp_ontdrugform dmd_vmp_ontdrugform_hash_update; Type: TRIGGER; Schema: terminology; Owner: owner_name
--

CREATE TRIGGER dmd_vmp_ontdrugform_hash_update BEFORE INSERT OR UPDATE ON terminology.dmd_vmp_ontdrugform FOR EACH ROW EXECUTE FUNCTION terminology.udf_tg_dmd_vmp_ontdrugform_hash_update();


--
-- Name: dmd_vtm dmd_vtm_hash_update; Type: TRIGGER; Schema: terminology; Owner: owner_name
--

CREATE TRIGGER dmd_vtm_hash_update BEFORE INSERT OR UPDATE ON terminology.dmd_vtm FOR EACH ROW EXECUTE FUNCTION terminology.udf_tg_dmd_vtm_hash_update();


--
-- Name: dmd_amp_drugroute dmd_amp_drugroute_hash_update; Type: TRIGGER; Schema: terminology_staging; Owner: owner_name
--

CREATE TRIGGER dmd_amp_drugroute_hash_update BEFORE INSERT OR UPDATE ON terminology_staging.dmd_amp_drugroute FOR EACH ROW EXECUTE FUNCTION terminology_staging.udf_tg_dmd_amp_drugroute_hash_update();


--
-- Name: dmd_amp_excipient dmd_amp_excipient_hash_update; Type: TRIGGER; Schema: terminology_staging; Owner: owner_name
--

CREATE TRIGGER dmd_amp_excipient_hash_update BEFORE INSERT OR UPDATE ON terminology_staging.dmd_amp_excipient FOR EACH ROW EXECUTE FUNCTION terminology_staging.udf_tg_dmd_amp_excipient_hash_update();


--
-- Name: dmd_amp dmd_amp_hash_update; Type: TRIGGER; Schema: terminology_staging; Owner: owner_name
--

CREATE TRIGGER dmd_amp_hash_update BEFORE INSERT OR UPDATE ON terminology_staging.dmd_amp FOR EACH ROW EXECUTE FUNCTION terminology_staging.udf_tg_dmd_amp_hash_update();


--
-- Name: dmd_vmp_controldrug dmd_vmp_controldrug_hash_update; Type: TRIGGER; Schema: terminology_staging; Owner: owner_name
--

CREATE TRIGGER dmd_vmp_controldrug_hash_update BEFORE INSERT OR UPDATE ON terminology_staging.dmd_vmp_controldrug FOR EACH ROW EXECUTE FUNCTION terminology_staging.udf_tg_dmd_vmp_controldrug_hash_update();


--
-- Name: dmd_vmp_drugform dmd_vmp_drugform_hash_update; Type: TRIGGER; Schema: terminology_staging; Owner: owner_name
--

CREATE TRIGGER dmd_vmp_drugform_hash_update BEFORE INSERT OR UPDATE ON terminology_staging.dmd_vmp_drugform FOR EACH ROW EXECUTE FUNCTION terminology_staging.udf_tg_dmd_vmp_drugform_hash_update();


--
-- Name: dmd_vmp_drugroute dmd_vmp_drugroute_hash_update; Type: TRIGGER; Schema: terminology_staging; Owner: owner_name
--

CREATE TRIGGER dmd_vmp_drugroute_hash_update BEFORE INSERT OR UPDATE ON terminology_staging.dmd_vmp_drugroute FOR EACH ROW EXECUTE FUNCTION terminology_staging.udf_tg_dmd_vmp_drugroute_hash_update();


--
-- Name: dmd_vmp dmd_vmp_hash_update; Type: TRIGGER; Schema: terminology_staging; Owner: owner_name
--

CREATE TRIGGER dmd_vmp_hash_update BEFORE INSERT OR UPDATE ON terminology_staging.dmd_vmp FOR EACH ROW EXECUTE FUNCTION terminology_staging.udf_tg_dmd_vmp_hash_update();


--
-- Name: dmd_vmp_ingredient dmd_vmp_ingredient_hash_update; Type: TRIGGER; Schema: terminology_staging; Owner: owner_name
--

CREATE TRIGGER dmd_vmp_ingredient_hash_update BEFORE INSERT OR UPDATE ON terminology_staging.dmd_vmp_ingredient FOR EACH ROW EXECUTE FUNCTION terminology_staging.udf_tg_dmd_vmp_ingredient_hash_update();


--
-- Name: dmd_vmp_ontdrugform dmd_vmp_ontdrugform_hash_update; Type: TRIGGER; Schema: terminology_staging; Owner: owner_name
--

CREATE TRIGGER dmd_vmp_ontdrugform_hash_update BEFORE INSERT OR UPDATE ON terminology_staging.dmd_vmp_ontdrugform FOR EACH ROW EXECUTE FUNCTION terminology_staging.udf_tg_dmd_vmp_ontdrugform_hash_update();


--
-- Name: dmd_vtm dmd_vtm_hash_update; Type: TRIGGER; Schema: terminology_staging; Owner: owner_name
--

CREATE TRIGGER dmd_vtm_hash_update BEFORE INSERT OR UPDATE ON terminology_staging.dmd_vtm FOR EACH ROW EXECUTE FUNCTION terminology_staging.udf_tg_dmd_vtm_hash_update();


--
-- Name: formulary_additional_code formulary_additional_code_fk; Type: FK CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_additional_code
    ADD CONSTRAINT formulary_additional_code_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);


--
-- Name: formulary_detail formulary_detail_fk; Type: FK CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_detail
    ADD CONSTRAINT formulary_detail_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);


--
-- Name: formulary_excipient formulary_excipient_fk; Type: FK CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_excipient
    ADD CONSTRAINT formulary_excipient_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);


--
-- Name: formulary_indication formulary_indication_fk; Type: FK CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_indication
    ADD CONSTRAINT formulary_indication_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);


--
-- Name: formulary_ingredient formulary_ingredient_fk; Type: FK CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_ingredient
    ADD CONSTRAINT formulary_ingredient_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);


--
-- Name: formulary_local_route_detail formulary_local_route_detail_fk; Type: FK CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_local_route_detail
    ADD CONSTRAINT formulary_local_route_detail_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);


--
-- Name: formulary_ontology_form formulary_ontology_form_fk; Type: FK CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_ontology_form
    ADD CONSTRAINT formulary_ontology_form_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);


--
-- Name: formulary_route_detail formulary_route_detail_fk; Type: FK CONSTRAINT; Schema: local_formulary; Owner: owner_name
--

ALTER TABLE ONLY local_formulary.formulary_route_detail
    ADD CONSTRAINT formulary_route_detail_fk FOREIGN KEY (formulary_version_id) REFERENCES local_formulary.formulary_header(formulary_version_id);


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: owner_name
--

REVOKE ALL ON SCHEMA public FROM rdsadmin;
REVOKE ALL ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO owner_name;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- PostgreSQL database dump complete
--

