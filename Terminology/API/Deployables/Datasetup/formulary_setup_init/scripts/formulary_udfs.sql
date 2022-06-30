CREATE SCHEMA IF NOT EXISTS local_formulary;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";



SET SCHEMA 'local_formulary';


--=================================ABOVE STATEMENTS SHOULD NOT BE DELETED ==========================================================================

DROP FUNCTION IF EXISTS local_formulary.udf_formulary_get_ancestors_by_codes;

CREATE OR REPLACE FUNCTION local_formulary.udf_formulary_get_ancestors_by_codes(in_codes text[])
 RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean, recstatuscode character varying)
 LANGUAGE sql
AS $function$
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

$function$
;
DROP FUNCTION IF EXISTS local_formulary.udf_formulary_get_descendents;

CREATE OR REPLACE FUNCTION local_formulary.udf_formulary_get_descendents(in_formulary_version_ids text[])
 RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean)
 LANGUAGE sql
AS $function$
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

$function$
;
DROP FUNCTION IF EXISTS local_formulary.udf_formulary_get_descendents_by_codes;

CREATE OR REPLACE FUNCTION local_formulary.udf_formulary_get_descendents_by_codes(in_codes text[])
 RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean, recstatuscode character varying)
 LANGUAGE sql
AS $function$
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

$function$
;
DROP FUNCTION IF EXISTS local_formulary.udf_formulary_get_latest_top_nodes;

CREATE OR REPLACE FUNCTION local_formulary.udf_formulary_get_latest_top_nodes()
 RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean, recstatuscode character varying, rnohformularystatuscd character varying)
 LANGUAGE plpgsql
AS $function$
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
			and (fh.product_type = 'VTM' or fh.parent_code is null or fh.parent_code = '');
	
end
$function$
;
DROP FUNCTION IF EXISTS local_formulary.udf_formulary_get_next_descendents_by_codes;

CREATE OR REPLACE FUNCTION local_formulary.udf_formulary_get_next_descendents_by_codes(in_codes text[])
 RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean, recstatuscode character varying)
 LANGUAGE sql
AS $function$
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
		where fh.is_latest = true;
		

$function$
;
DROP FUNCTION IF EXISTS local_formulary.udf_formulary_search_amp_by_attributes;

CREATE OR REPLACE FUNCTION local_formulary.udf_formulary_search_amp_by_attributes(in_name text DEFAULT NULL::text, in_search_code text DEFAULT NULL::text, in_recordstatus_codes text[] DEFAULT NULL::text[], in_rnoh_formulary_status_codes text[] DEFAULT NULL::text[])
 RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean, recstatuscode text, rnohformularystatuscd text, prescribable boolean)
 LANGUAGE sql
AS $function$
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
 
$function$
;

DROP FUNCTION IF EXISTS local_formulary.udf_formulary_search_nodes_with_descendents;

CREATE OR REPLACE FUNCTION local_formulary.udf_formulary_search_nodes_with_descendents(in_name text DEFAULT NULL::text, in_search_code text DEFAULT NULL::text)
 RETURNS TABLE(formularyid character varying, versionid integer, formularyversionid character varying, name text, code character varying, producttype character varying, parentcode character varying, parentname text, parentproducttype character varying, isduplicate boolean, recstatuscode text, rnohformularystatuscd text, prescribable boolean)
 LANGUAGE sql
AS $function$
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
		and (in_name is null or fh.name_tokens @@ to_tsquery(in_name))
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

$function$
;
