DROP FUNCTION IF EXISTS terminology.udf_snomed_get_ancestor_nodes_search_term_by_tag;
CREATE OR REPLACE FUNCTION terminology.udf_snomed_get_ancestor_nodes_search_term_by_tag(in_searchtext text, in_conceptid text, in_semantictag text)
 RETURNS TABLE(conceptid character varying, preferredterm text, parentid character varying, fsn text, level integer)
 LANGUAGE sql
AS $function$


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


$function$
;

DROP FUNCTION IF EXISTS terminology.udf_snomed_get_child_nodes_search_term_by_tag;
CREATE OR REPLACE FUNCTION terminology.udf_snomed_get_child_nodes_search_term_by_tag(in_searchtext text, in_conceptid text, in_semantictag text)
 RETURNS TABLE(conceptid character varying, preferredterm text, parentid character varying, fsn text, level integer)
 LANGUAGE sql
AS $function$
	
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
$function$
;

DROP FUNCTION IF EXISTS terminology.udf_snomed_get_next_ancestors;
CREATE OR REPLACE FUNCTION terminology.udf_snomed_get_next_ancestors(in_conceptids text[])
 RETURNS TABLE(conceptid character varying, preferredterm text, parentid character varying, fsn text, level integer)
 LANGUAGE sql
AS $function$
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

$function$
;

DROP FUNCTION IF EXISTS terminology.udf_snomed_get_next_descendents;
CREATE OR REPLACE FUNCTION terminology.udf_snomed_get_next_descendents(in_conceptids text[])
 RETURNS TABLE(conceptid character varying, preferredterm text, parentid character varying, fsn text, level integer)
 LANGUAGE sql
AS $function$
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
$function$
;
