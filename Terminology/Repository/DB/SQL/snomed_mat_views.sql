
-- DROP EXTENSION pg_trgm;
--for like query indexing

CREATE EXTENSION pg_trgm
	SCHEMA "terminology"
	VERSION null;

--=========================================================================================
set schema 'terminology';

--drop materialized view terminology.snomedct_description_latest_mat;
create materialized view terminology.snomedct_description_latest_mat as
select 
	tmp.id,
	tmp.effectivetime,
	tmp.active,
	tmp.moduleid,
	tmp.conceptid,
	tmp.languagecode,
	tmp.typeid,
	tmp.term,
	to_tsvector('english',tmp.term) term_tokens,
	tmp.casesignificanceid
from (select
	--rank() over(partition by id, casesignificanceid order by effectivetime desc ) as rnk,
	rank() over(partition by id order by effectivetime desc ) as rnk,
	df.*
	from terminology.snomedct_description_f df) as tmp
where tmp.rnk = 1 
--and moduleid = '900000000000207008' --considering only core module;
WITH NO DATA;

CREATE INDEX snomedct_description_latest_mat_conceptid_idx ON terminology.snomedct_description_latest_mat (conceptid);
CREATE INDEX snomedct_description_latest_mat_typeid_idx ON terminology.snomedct_description_latest_mat (typeid);
CREATE INDEX snomedct_description_latest_mat_active_idx ON terminology.snomedct_description_latest_mat (active);
CREATE INDEX snomedct_description_latest_mat_term_idx ON terminology.snomedct_description_latest_mat (term);
CREATE INDEX snomedct_description_latest_mat_languagecode_idx ON terminology.snomedct_description_latest_mat (languagecode);
CREATE INDEX snomedct_description_latest_mat_term_tokens_idx ON terminology.snomedct_description_latest_mat using gin(term_tokens);


REFRESH MATERIALIZED VIEW terminology.snomedct_description_latest_mat;

select count(*) from terminology.snomedct_description_latest_mat;

--============================
--drop materialized view terminology.snomedct_concept_latest_mat;
create materialized view terminology.snomedct_concept_latest_mat as
select
	tmp.id,
	tmp.effectivetime,
	tmp.active,
	tmp.moduleid,
	tmp.definitionstatusid
from (
	select
		rank() over(partition by id order by effectivetime desc ) as rnk,
		*
	from terminology.snomedct_concept_f cf) as tmp
where tmp.rnk = 1 
--and moduleid = '900000000000207008' --considering only core module;
WITH NO DATA;

CREATE INDEX snomedct_concept_latest_mat_id_idx ON terminology.snomedct_concept_latest_mat (id);
CREATE INDEX snomedct_concept_latest_mat_active_idx ON terminology.snomedct_concept_latest_mat (active);
CREATE INDEX snomedct_concept_latest_mat_moduleid_idx ON terminology.snomedct_concept_latest_mat (moduleid);


REFRESH MATERIALIZED VIEW terminology.snomedct_concept_latest_mat;

select count(*) from terminology.snomedct_concept_latest_mat;


--===========================

--drop materialized view terminology.snomedct_relationship_latest_mat;
create materialized view terminology.snomedct_relationship_latest_mat as
select 
	tmp.id,
	tmp.effectivetime,
	tmp.active,
	tmp.moduleid,
	tmp.sourceid,
	tmp.destinationid,
	tmp.relationshipgroup,
	tmp.typeid,
	tmp.characteristictypeid,
	tmp.modifierid
from (
	select
		rank() over(partition by id,moduleid,sourceid,destinationid,relationshipgroup,typeid,characteristictypeid,modifierid order by effectivetime desc ) as rnk,
		*
		from terminology.snomedct_relationship_f rf) as tmp
where tmp.rnk = 1 
--and moduleid = '900000000000207008' --considering only core module;
WITH NO DATA;

CREATE INDEX snomedct_relationship_latest_mat_active_idx ON terminology.snomedct_relationship_latest_mat (active);
CREATE INDEX snomedct_relationship_latest_mat_moduleid_idx ON terminology.snomedct_relationship_latest_mat (moduleid);
CREATE INDEX snomedct_relationship_latest_mat_sourceid_idx ON terminology.snomedct_relationship_latest_mat (sourceid);
CREATE INDEX snomedct_relationship_latest_mat_destinationid_idx ON terminology.snomedct_relationship_latest_mat (destinationid);
CREATE INDEX snomedct_relationship_latest_mat_typeid_idx ON terminology.snomedct_relationship_latest_mat (typeid);


REFRESH MATERIALIZED VIEW terminology.snomedct_relationship_latest_mat;

select count(*) from terminology.snomedct_relationship_latest_mat;

--==============================================

--drop materialized view terminology.snomedct_langrefset_latest_mat;
create materialized view terminology.snomedct_langrefset_latest_mat as
select 
	tmp.id,
	tmp.effectivetime,
	tmp.active,
	tmp.moduleid,
	tmp.refsetid,
	tmp.referencedcomponentid,
	tmp.acceptabilityid
from (
	select
	rank() over(partition by id,moduleid,refsetid,referencedcomponentid,acceptabilityid order by effectivetime desc ) as rnk,
	*
	from terminology.snomedct_langrefset_f lf) as tmp
where tmp.rnk = 1 
--and moduleid = '900000000000207008'--considering only core module;
WITH NO DATA;

CREATE INDEX snomedct_langrefset_latest_mat_active_idx ON terminology.snomedct_langrefset_latest_mat (active);
CREATE INDEX snomedct_langrefset_latest_mat_moduleid_idx ON terminology.snomedct_langrefset_latest_mat (moduleid);
CREATE INDEX snomedct_langrefset_latest_mat_refsetid_idx ON terminology.snomedct_langrefset_latest_mat (refsetid);
CREATE INDEX snomedct_langrefset_latest_mat_referencedcomponentid_idx ON terminology.snomedct_langrefset_latest_mat (referencedcomponentid);
CREATE INDEX snomedct_langrefset_latest_mat_acceptabilityId_idx ON terminology.snomedct_langrefset_latest_mat (acceptabilityId);


REFRESH MATERIALIZED VIEW terminology.snomedct_langrefset_latest_mat;

select count(*) from terminology.snomedct_langrefset_latest_mat;

--==============================================================

--drop materialized view terminology.snomedct_conceptpreferredname_latest_mat;
create materialized view terminology.snomedct_conceptpreferredname_latest_mat as
SELECT distinct 
		c.id conceptId, 
		d.term preferredName,
		to_tsvector('english',d.term) preferredName_tokens,
		d.id descriptionId
FROM terminology.snomedct_concept_latest_mat c
inner JOIN terminology.snomedct_description_latest_mat d
  		ON 	c.id = d.conceptId
	  	AND d.active = '1'
	  	AND d.typeId = '900000000000013009'
		and c.active = '1'
inner JOIN terminology.snomedct_langrefset_latest_mat l
  ON d.id = l.referencedComponentId
  AND l.active = '1'
  AND l.refSetId = '900000000000508004'  -- GB English
  AND l.acceptabilityId = '900000000000548007' -- Preferred term
  WITH NO DATA;
 
CREATE INDEX snomedct_conceptpreferredname_latest_mat_conceptid_idx ON terminology.snomedct_conceptpreferredname_latest_mat (conceptid);
CREATE INDEX snomedct_conceptpreferredname_latest_mat_descriptionid_idx ON terminology.snomedct_conceptpreferredname_latest_mat (descriptionid);
CREATE INDEX snomedct_conceptpreferredname_latest_mat_preferredname_idx ON terminology.snomedct_conceptpreferredname_latest_mat (preferredname);
CREATE INDEX snomedct_conceptpreferredname_latest_mat_preferredName_tokens_idx ON terminology.snomedct_conceptpreferredname_latest_mat using gin(preferredName_tokens);
 
REFRESH MATERIALIZED VIEW terminology.snomedct_conceptpreferredname_latest_mat;

select count(*) from terminology.snomedct_conceptpreferredname_latest_mat;


--===============================================================

--drop materialized VIEW terminology.snomedct_relationshipwithnames_latest_mat;
CREATE materialized VIEW terminology.snomedct_relationshipwithnames_latest_mat AS
SELECT id, 
	   	effectiveTime, 
	   	active,
		moduleId, 
	    cpn1.preferredName moduleIdName,
	    sourceId, 
	    cpn2.preferredName sourceIdName,
	    to_tsvector('english',cpn2.preferredName) sourceIdName_tokens,
	    destinationId, 
	    cpn3.preferredName destinationIdName,
	    to_tsvector('english',cpn3.preferredName) destinationIdName_tokens,
	    relationshipGroup,
	    typeId, 
	    cpn4.preferredName typeIdName,
	    characteristicTypeId, 
	    cpn5.preferredName characteristicTypeIdName,
	    modifierId, 
	    cpn6.preferredName modifierIdName
from 	terminology.snomedct_relationship_latest_mat relationship,
		terminology.snomedct_conceptpreferredname_latest_mat cpn1,
		terminology.snomedct_conceptpreferredname_latest_mat cpn2,
		terminology.snomedct_conceptpreferredname_latest_mat cpn3,
		terminology.snomedct_conceptpreferredname_latest_mat cpn4,
		terminology.snomedct_conceptpreferredname_latest_mat cpn5,
		terminology.snomedct_conceptpreferredname_latest_mat cpn6
WHERE relationship.moduleId = cpn1.conceptId
AND relationship.sourceId = cpn2.conceptId
AND destinationId = cpn3.conceptId
AND typeId = cpn4.conceptId
AND characteristicTypeId = cpn5.conceptId
AND modifierId = cpn6.conceptId
WITH NO DATA;
 
 
CREATE INDEX snomedct_relationshipwithnames_latest_mat_active_idx ON terminology.snomedct_relationshipwithnames_latest_mat (active);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_moduleidname_idx ON terminology.snomedct_relationshipwithnames_latest_mat (moduleidname);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_sourceid_idx ON terminology.snomedct_relationshipwithnames_latest_mat (sourceid);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_sourceidname_idx ON terminology.snomedct_relationshipwithnames_latest_mat (sourceidname);

CREATE INDEX snomedct_relationshipwithnames_latest_mat_destinationid_idx ON terminology.snomedct_relationshipwithnames_latest_mat (destinationid);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_destinationidname_idx ON terminology.snomedct_relationshipwithnames_latest_mat (destinationidname);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_typeid_idx ON terminology.snomedct_relationshipwithnames_latest_mat (typeid);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_typeidname_idx ON terminology.snomedct_relationshipwithnames_latest_mat (typeidname);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_sourceIdName_tokens_idx ON terminology.snomedct_relationshipwithnames_latest_mat using gin(sourceIdName_tokens);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_destinationIdName_tokens_idx ON terminology.snomedct_relationshipwithnames_latest_mat using gin(destinationIdName_tokens);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_sourceid_destid_idx ON terminology.snomedct_relationshipwithnames_latest_mat (sourceid, destinationid);

 
REFRESH MATERIALIZED VIEW terminology.snomedct_relationshipwithnames_latest_mat;

select count(*) from terminology.snomedct_relationshipwithnames_latest_mat;
 

--===============================================================

--NOT IN USE --This is to build the tree
--set schema 'terminology';
--
----drop materialized VIEW terminology.snomedct_tree_mat;
--CREATE materialized VIEW terminology.snomedct_tree_mat as
--select 
--		sourceid,
--		destinationid,
--		path,
--		level
--from (
--		WITH RECURSIVE search_graph(      
--		  sourceid,
--		  destinationid,
--		  path,
--		  level
--		  --pathArr
--	) as (
--			select 
--				r.sourceid,
--				r.destinationid,
--				(r.destinationid::ltree || r.sourceid::ltree)::ltree as path,
--				1 as level
--				--ARRAY[r.destinationid || r.sourceid ]::varchar(1022)[] as pathArr
--	
--			from terminology.snomedct_relationship_latest_mat r
--			where r.active = '1' 
--			and r.typeid = '116680003' -- Is a
--			and r.destinationid = '138875005'--root node
--			UNION all
--			select 
--				r.sourceid,
--				r.destinationid,
--				(sg.path::ltree || r.sourceid::ltree )::ltree as path,
--				level + 1 as level
--				--(sg.pathArr || r.sourceid)::varchar(1022)[] as pathArr
--	
--			from terminology.snomedct_relationship_latest_mat r
--			inner join search_graph sg on r.destinationid = sg.sourceid
--		      	--AND (r.sourceid <> ALL(sg.pathArr))        -- prevent from cycling 
--		      	--AND sg.level <= 25
--			where r.active = '1' and r.typeid = '116680003')
--	SELECT 
--		sourceid,
--		destinationid,
--		path,
--		level
--	FROM search_graph
--	) tbl
--WITH NO DATA;
--
--
--CREATE INDEX snomedct_tree_mat_parent_path_idx ON terminology.snomedct_tree_mat USING GIST(path);
--CREATE INDEX snomedct_tree_mat_path_idx ON terminology.snomedct_tree_mat (path);
--
----CREATE INDEX snomedct_tree_mat_sourceid_idx ON terminology.snomedct_tree_mat using hash(sourceid);
----CREATE INDEX snomedct_tree_mat_destinationid_idx ON terminology.snomedct_tree_mat using hash(destinationid);
--CREATE INDEX snomedct_tree_mat_sourceid_tree_idx ON terminology.snomedct_tree_mat (sourceid);
--CREATE INDEX snomedct_tree_mat_destinationid_tree_idx ON terminology.snomedct_tree_mat (destinationid);
--CREATE INDEX snomedct_tree_mat_srcid_destinationid_idx ON terminology.snomedct_tree_mat (sourceid, destinationid);
--
--CREATE INDEX snomedct_tree_mat_level_idx ON terminology.snomedct_tree_mat (level);
--
--REFRESH MATERIALIZED VIEW terminology.snomedct_tree_mat;


--==================================================================

set schema 'terminology';

--drop materialized VIEW terminology.snomedct_concept_lookup_mat;
CREATE materialized VIEW terminology.snomedct_concept_lookup_mat as
		select distinct 
		s.conceptid,
		s.preferredname as preferredTerm,
		s.preferredname_tokens as preferredname_tokens,
		d.term as fsn,
		--trim(trailing  ')' from (trim(leading ' (' from substring(lower(d.term) from '\s\(.*\)$')))) as semantictag
		trim(trailing  ')' from substring(d.term from '([^\(]+[\)+])$')) as semantictag
		FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
		inner join terminology.snomedct_description_latest_mat d 
			on d.active = '1' and d.conceptid = s.conceptid and d.typeid = '900000000000003001'
WITH NO DATA;

--Hash may have issues for non-unique values
CREATE INDEX snomedct_concept_lookup_mat_conceptid_hash_idx ON terminology.snomedct_concept_lookup_mat using hash(conceptid);

--create INDEX snomedct_concept_lookup_mat_conceptid_idx ON terminology.snomedct_concept_lookup_mat (conceptid)
--include(preferredTerm, fsn);

CREATE INDEX snomedct_concept_lookup_mat_preferredTerm_idx ON terminology.snomedct_concept_lookup_mat (preferredTerm);
CREATE INDEX snomedct_concept_lookup_mat_fsn_idx ON terminology.snomedct_concept_lookup_mat (fsn);

CREATE INDEX snomedct_concept_lookup_mat_semantictag_idx ON terminology.snomedct_concept_lookup_mat (lower(semantictag));
--create INDEX snomedct_concept_lookup_mat_semantictag_idx ON terminology.snomedct_concept_lookup_mat (semantictag);


CREATE INDEX snomedct_concept_lookup_mat_preferredname_tokens_idx ON terminology.snomedct_concept_lookup_mat using gin(preferredname_tokens);
CREATE INDEX snomedct_concept_lookup_mat_preferredname_tokens_tree_idx ON terminology.snomedct_concept_lookup_mat (preferredname_tokens);


REFRESH MATERIALIZED VIEW terminology.snomedct_concept_lookup_mat;

select count(*) from terminology.snomedct_concept_lookup_mat;

select distinct semantictag from terminology.snomedct_concept_lookup_mat where fsn like '% (%';

select fsn from terminology.snomedct_concept_lookup_mat where fsn like '%medical equipment) (occupation%';
--Electronics fitter (medical equipment) (occupation)

select distinct trim(trailing  ')' from substring(fsn from '([^\(]+[\)+])$')) from terminology.snomedct_concept_lookup_mat where fsn ~ 'Electronics fitter.*\)$'
--'([^\(]+)$'


--===================================================================

set schema 'terminology';

--drop materialized VIEW terminology.snomedct_relation_active_isa_lookup_mat;
CREATE materialized VIEW terminology.snomedct_relation_active_isa_lookup_mat as
		select distinct 
		srlm.sourceid,
		srlm.destinationid 
		from terminology.snomedct_relationship_latest_mat srlm 
		where srlm.active = '1' and srlm.typeid = '116680003' -- Is a
WITH NO DATA;

CREATE INDEX snomedct_relation_active_isa_lookup_mat_sourceid_hash_idx ON terminology.snomedct_relation_active_isa_lookup_mat using hash(sourceid);
CREATE INDEX snomedct_relation_active_isa_lookup_mat_destinationid_hash_idx ON terminology.snomedct_relation_active_isa_lookup_mat using hash(destinationid);

--CREATE INDEX snomedct_relation_active_isa_lookup_mat_sourceid_idx ON terminology.snomedct_relation_active_isa_lookup_mat (sourceid);
--CREATE INDEX snomedct_relation_active_isa_lookup_mat_destinationid_idx ON terminology.snomedct_relation_active_isa_lookup_mat (destinationid);

CREATE unique INDEX snomedct_relation_active_isa_lookup_mat_srciddestinationid_idx ON terminology.snomedct_relation_active_isa_lookup_mat (sourceid,destinationid);

CREATE INDEX snomedct_relation_active_isa_lookup_mat_destinationid_srcid_idx ON terminology.snomedct_relation_active_isa_lookup_mat (destinationid) include(sourceid);

REFRESH MATERIALIZED VIEW terminology.snomedct_relation_active_isa_lookup_mat;

select count(*) from terminology.snomedct_relation_active_isa_lookup_mat 

--===================================================================

/* Not Executed
 *
drop materialized view "SCT_1".snomedct.conceptpreferredname_mat;
create materialized view "SCT_1".snomedct.conceptpreferredname_mat as
SELECT distinct 
       c.id conceptId, 
       d.term preferredName,
d.id descriptionId
FROM "SCT_1".snomedct.concept_f c
inner JOIN "SCT_1".snomedct.description_f d
  ON c.id = d.conceptId
  AND d.active = '1'
  AND d.typeId = '900000000000013009'
inner JOIN "SCT_1".snomedct.langrefset_f l
  ON d.id = l.referencedComponentId
  AND l.active = '1'
  AND l.refSetId = '900000000000508004'  -- GB English
  AND l.acceptabilityId = '900000000000548007' -- Preferred term
  WITH NO DATA;
 
CREATE INDEX conceptpreferredname_mat_conceptid_idx ON snomedct.conceptpreferredname_mat (conceptid);
CREATE INDEX conceptpreferredname_mat_descriptionid_idx ON snomedct.conceptpreferredname_mat (descriptionid);
CREATE INDEX conceptpreferredname_mat_preferredname_idx ON snomedct.conceptpreferredname_mat (preferredname);
 
 
REFRESH MATERIALIZED VIEW snomedct.conceptpreferredname_mat;

select count(*) from snomedct.conceptpreferredname_mat; 

============================================
 
drop materialized VIEW "SCT_1".snomedct.relationshipwithnames_mat;
CREATE materialized VIEW "SCT_1".snomedct.relationshipwithnames_mat AS
SELECT 
       id, 
       effectiveTime, 
       active,
    moduleId, 
    cpn1.preferredName moduleIdName,
    sourceId, 
    cpn2.preferredName sourceIdName,
    destinationId, 
    cpn3.preferredName destinationIdName,
    relationshipGroup,
    typeId, 
    cpn4.preferredName typeIdName,
    characteristicTypeId, 
    cpn5.preferredName characteristicTypeIdName,
    modifierId, 
    cpn6.preferredName modifierIdName
from "SCT_1".snomedct.relationship_f relationship,
    "SCT_1".snomedct.conceptpreferredname_mat cpn1,
    "SCT_1".snomedct.conceptpreferredname_mat cpn2,
    "SCT_1".snomedct.conceptpreferredname_mat cpn3,
    "SCT_1".snomedct.conceptpreferredname_mat cpn4,
    "SCT_1".snomedct.conceptpreferredname_mat cpn5,
    "SCT_1".snomedct.conceptpreferredname_mat cpn6
WHERE moduleId = cpn1.conceptId
AND sourceId = cpn2.conceptId
AND destinationId = cpn3.conceptId
AND typeId = cpn4.conceptId
AND characteristicTypeId = cpn5.conceptId
AND modifierId = cpn6.conceptId
WITH NO DATA;
 
 
CREATE INDEX relationshipwithnames_mat_active_idx ON snomedct.relationshipwithnames_mat (active);
CREATE INDEX relationshipwithnames_mat_moduleidname_idx ON snomedct.relationshipwithnames_mat (moduleidname);
CREATE INDEX relationshipwithnames_mat_sourceid_idx ON snomedct.relationshipwithnames_mat (sourceid);
CREATE INDEX relationshipwithnames_mat_sourceidname_idx ON snomedct.relationshipwithnames_mat (sourceidname);
CREATE INDEX relationshipwithnames_mat_destinationid_idx ON snomedct.relationshipwithnames_mat (destinationid);
CREATE INDEX relationshipwithnames_mat_destinationidname_idx ON snomedct.relationshipwithnames_mat (destinationidname);
CREATE INDEX relationshipwithnames_mat_typeid_idx ON snomedct.relationshipwithnames_mat (typeid);
CREATE INDEX relationshipwithnames_mat_typeidname_idx ON snomedct.relationshipwithnames_mat (typeidname);
 
 
REFRESH MATERIALIZED VIEW snomedct.relationshipwithnames_mat;

select count(*) from snomedct.relationshipwithnames_mat;


 */ 
--========================================================================================================



--=======================================STORED PROCEDURE=================================================================

set schema 'terminology';

--=====================SEARCH AND GET CHILDREN=================================================
--=========================OPTION 1=============================================

--OPTION 1 is not being used in the application
--
--drop FUNCTION terminology.udf_snomed_get_child_nodes_search_term_by_tag;
--CREATE or replace FUNCTION terminology.udf_snomed_get_child_nodes_search_term_by_tag (
--in searchQuery text, 
--in semanticTag text,
--out conceptid varchar(18), 
--out preferredTerm text, 
--out parentId text, 
--out typeid varchar(18), 
--out fsn text, 
--out relationship text,
--out level int
--out path varchar(1022)[]
--) 
--RETURNS TABLE (cid varchar(18), t text, d varchar(18), ty varchar(18), f text, p varchar, depdth int, pa varchar(1022)[])
--RETURNS TABLE (cid varchar(18))
--returns setof record as
--
--$$
--	declare 
--			sql_stmt text;
--			semanticTagToSearch text;
--begin
--	semanticTagToSearch := '% ' || '('|| semanticTag ||')';	
--select '% ' + 'semanticTag' into semanticTagToSearch;
--
--sql_stmt := format($_$
--	
--	WITH RECURSIVE search_snomed(      
--	  conceptid,
--	  preferredTerm,
--	  parentId,
--	  typeid,
--	  fsn,
--	  prop,   -- edge property      
--	  level,  -- depth, starting from 1      
--	  path  -- path, stored using an array      
--	) as (
--				SELECT distinct 
--					s.conceptid ,
--					s.preferredname as preferredTerm,
--					null as parentId,
--					d.typeid, 
--					d.term as fsn,
--					null as prop,
--					1 as level,
--					ARRAY[s.conceptid]::varchar(1022)[] as path
--				FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
--				inner join terminology.snomedct_description_latest_mat d 
--				on d.active = '1' and d.conceptid = s.conceptid 
--				WHERE 
--				(s.preferredname_tokens @@ to_tsquery('%s'))
--				and d.term ilike '%s'
--				and d.typeid = '900000000000003001'
--			UNION all
--				select 
--					r.sourceid as conceptid,
--					r.sourceidname as preferredTerm,
--					r.destinationid as parentId,
--					d.typeid, 
--					d.term as fsn,
--					r.typeIdName as prop,
--					-1 as level,
--s.level + 1 as level,
--					(s.path || r.sourceid)::varchar(1022)[] as path
--			  	FROM
--			         terminology.snomedct_relationshipwithnames_latest_mat r
--			     INNER JOIN search_snomed s 
--			     	ON s.conceptid = r.destinationid
--			      	AND (r.sourceid <> ALL(s.path))        -- prevent from cycling 
--AND s.level <= 15 --(Total levels will be <level> + 1 )
--			    inner join terminology.snomedct_description_latest_mat d 
--				on d.active = '1' and d.conceptid = r.sourceid and d.typeid = '900000000000003001'
--				where r.active = '1' and r.typeIdName = 'Is a' 
--			
--		)
--	
--	SELECT 
--		conceptid,
--		preferredTerm,
--		parentId,
--		typeid,
--		fsn,
--		prop as relationship, -- edge property      
--		level  -- depth, starting from 1      
--path  -- path, stored using an array  
--	FROM search_snomed;
--	$_$, searchQuery, semanticTagToSearch   
--
--);
--raise notice '%s', sql_stmt;
--
--return query execute sql_stmt;    
--
--end;
--$$
--LANGUAGE plpgsql strict;
--===============
--
--select * from terminology.udf_snomed_get_child_nodes_search_term_by_tag('par:*', 'disorder')
--where level = 2;

--=========================END OPTION 1=============================================

--=========================OPTION 2 -- Not being used =============================================


--drop function terminology.fn1;
--CREATE or replace function terminology.fn1(IN p1 text, in p2 text) 
--RETURNS TABLE(conceptid varchar(18), term text,destinationid text, typeid varchar(18),fsn text,relationship text,depth int,
--path varchar(1022)[]) AS 
--	
--$$
--	
--declare derivedSemanticTag varchar(255);
--
--
--RAISE NOTICE 'Procedure Parameter: %', p1 ;
--   
--select id from terminology.snomedct_concept_latest_mat sclm limit 10;
--	
--	
--derivedSemanticTag := '%% (%s)', p2;
--
--	
--   WITH RECURSIVE search_snomed(      
--	  conceptid,
--	  term,
--	  destinationid,
--	  typeid,
--	  fsn,
--	  prop,   -- edge property      
--	  depth,  -- depth, starting from 1      
--	  path  -- path, stored using an array      
--	) as (
--				SELECT distinct 
--					s.conceptid ,
--					s.preferredname as term,
--					null as destinationid,
--					d.typeid, 
--					d.term as fsn,
--					null as prop,
--					1 as depth,
--					ARRAY[s.conceptid]::varchar(1022)[] as path
--				FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
--				inner join snomedct_description_latest_mat d 
--				on d.active = '1' and d.conceptid = s.conceptid 
--				WHERE 
--				(s.preferredname_tokens @@ to_tsquery(p1))
--				and d.term ilike '%% ' || '('|| p2 || ')' --'% (disorder)'
--				and d.typeid = '900000000000003001'
--			UNION all
--				select 
--					r.sourceid as conceptid,
--					r.sourceidname as term,
--					r.destinationid,
--					d.typeid, 
--					d.term as fsn,
--					r.typeIdName as prop,
--					s.depth + 1 as depth,
--					(s.path || r.destinationid)::varchar(1022)[] as path
--			  	FROM
--			         snomedct_relationshipwithnames_latest_mat r
--			     INNER JOIN search_snomed s 
--			     	ON s.conceptid = r.destinationid
--			      	AND (r.sourceid <> ALL(s.path))        -- prevent from cycling 
--			      	AND s.depth <= 15 --(Total levels will be <level> + 1 )
--			     inner join snomedct_description_latest_mat d 
--					on d.active = '1' and d.conceptid = r.sourceid and d.typeid = '900000000000003001'
--				where r.active = '1' and r.typeIdName = 'Is a' 
--			
--		)
--	
--	SELECT 
--		conceptid,
--		term,
--		destinationid,
--		typeid,
--		fsn,
--		prop as relationship, -- edge property      
--		depth,  -- depth, starting from 1      
--		path  -- path, stored using an array  
--	FROM search_snomed;
--
--$$
--LANGUAGE sql ;
--
--select * from terminology.fn1('par:*', 'disorder');

--=========================END OPTION 2=========================================================

--=========================OPTION 3=============================================
--
----drop FUNCTION terminology.udf_snomed_get_child_nodes_search_term_by_tag;
--CREATE or replace FUNCTION terminology.udf_snomed_get_child_nodes_search_term_by_tag (
--in searchQuery text, 
--in semanticTag text,
--out conceptid varchar(18), 
--out preferredTerm text, 
--out parentId varchar(18),
--out fsn text, 
--out level int
----out path varchar(1022)[]
--) 
----RETURNS TABLE (cid varchar(18), t text, d varchar(18), ty varchar(18), f text, p varchar, depdth int, pa varchar(1022)[])
----RETURNS TABLE (cid varchar(18))
--returns setof record as
--
--$$
--	declare 
--			sql_stmt text;
--			semanticTagToSearch text;
--begin
--	semanticTagToSearch := '% ' || '('|| semanticTag ||')';
--	--semanticTagToSearch := lower(semanticTag);	
--	--select '% ' + 'semanticTag' into semanticTagToSearch;
--
--sql_stmt := format($_$
--	
--
--	WITH RECURSIVE search_snomed(      
--	  conceptid,
--	  preferredTerm,
--	  parentId,
--	  fsn,
--	  level,  -- depth, starting from 1      
--	  srcpath  -- path, stored using an array      
--	) as (
--			select distinct 
--					clkp.conceptid,
--					clkp.preferredTerm,
--					r.destinationid as parentid,
--					clkp.fsn,
--					1 as level,
--					ARRAY[clkp.conceptid ]::varchar(1022)[] as srcpath
--				from terminology.snomedct_concept_lookup_mat clkp
--				inner join terminology.snomedct_relation_active_isa_lookup_mat r on r.sourceid = clkp.conceptid
--				where clkp.preferredname_tokens @@ to_tsquery('%s')
--				and clkp.fsn ilike ('%s')
--			UNION all
--				select 
--					clkp.conceptid,
--					clkp.preferredTerm,
--					r.destinationid as parentId,
--					clkp.fsn,
--					-1 as level,
--					--s.level + 1 as level,
--					(s.srcpath || r.sourceid)::varchar(1022)[] as srcpath
--			  	FROM
--			         terminology.snomedct_relation_active_isa_lookup_mat r
--			     INNER JOIN search_snomed s 
--			     	ON s.conceptid = r.destinationid
--			      	AND (r.sourceid <> ALL(s.srcpath))        -- prevent from cycling 
--			      	--AND s.level <= 15 --(Total levels will be <level> + 1 )
--			     inner join terminology.snomedct_concept_lookup_mat clkp on clkp.conceptid = r.sourceid
--		)
--	
--	SELECT 
--		conceptid,
--		preferredTerm,
--		parentId,
--		fsn,
--		level  -- depth, starting from 1      
--		--path  -- path, stored using an array  
--	FROM search_snomed;
--
--	$_$, searchQuery, semanticTagToSearch   
--
--);
----raise notice '%s', sql_stmt;
--
--return query execute sql_stmt;    
--
--end;
--$$
--LANGUAGE plpgsql strict;
----===============
--
----select * from terminology.udf_snomed_get_child_nodes_search_term_by_tag('par:*', 'disorder')
----where level = 2;
--
--select * from terminology.udf_snomed_get_child_nodes_search_term_by_tag('par:*', 'medicinal product form');


--=========================END OPTION 3=============================================

--=========================OPTION 4=============================================
--Option 4 is being used in the application

--drop function terminology.udf_snomed_get_child_nodes_search_term_by_tag;

CREATE or replace function terminology.udf_snomed_get_child_nodes_search_term_by_tag(IN in_searchText text, IN in_conceptId text, in in_semanticTag text) 

RETURNS TABLE(conceptid varchar(18), preferredTerm text,parentId varchar(18), 
fsn text,level int) AS 
	
$$
	
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
$$
LANGUAGE sql;

select * from terminology.udf_snomed_get_child_nodes_search_term_by_tag('par:*','', 'disorder');

--=========================END OPTION 4=============================================

--=================================END SEARCH AND GET CHILDREN===================================

--=====================SEARCH AND GET ANCESTORS=================================================
--========================= OPTION 1=============================================

--THIS OPTION is being used in the application

--drop FUNCTION terminology.udf_snomed_get_ancestor_nodes_search_term_by_tag;
CREATE or replace FUNCTION terminology.udf_snomed_get_ancestor_nodes_search_term_by_tag (IN in_searchText text, IN in_conceptId text, in in_semanticTag text)  

RETURNS TABLE(conceptid varchar(18), preferredTerm text,parentId varchar(18), 
fsn text,level int) AS 
	
$$


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


$$
LANGUAGE sql;

select * from terminology.udf_snomed_get_ancestor_nodes_search_term_by_tag('par:*','', 'disorder');

--=========================END OPTION 1=============================================

--========================= OPTION 2=============================================

--
----drop FUNCTION terminology.udf_snomed_get_ancestor_nodes_search_term_by_tag;
--CREATE or replace FUNCTION terminology.udf_snomed_get_ancestor_nodes_search_term_by_tag (
--in searchQuery text, 
--in semanticTag text,
--out conceptid varchar(18) , 
--out preferredTerm text, 
--out parentId varchar(18) , 
--out fsn text, 
--out level int
----out path varchar(1022)[]
--) 
--returns setof record as
--
--$$
--	declare 
--			sql_stmt text;
--			semanticTagToSearch text;
--begin
--	semanticTagToSearch := '% ' || '('|| semanticTag ||')';
--	--semanticTagToSearch := lower(semanticTag);	
--	--select '% ' + 'semanticTag' into semanticTagToSearch;
--
--sql_stmt := format($_$
--	
--	WITH RECURSIVE search_snomed(      
--	  conceptid,
--	  preferredTerm,
--	  parentId,
--	  fsn,
--	  level,  -- depth, starting from 1      
--	  srcpath  -- path, stored using an array      
--	) as (
--				select distinct 
--					clkp.conceptid,
--					clkp.preferredTerm,
--					r.destinationid as parentid,
--					clkp.fsn,
--					1 as level,
--					ARRAY[clkp.conceptid ]::varchar(1022)[] as srcpath
--				from terminology.snomedct_concept_lookup_mat clkp
--				inner join terminology.snomedct_relation_active_isa_lookup_mat r on r.sourceid = clkp.conceptid
--				where clkp.preferredname_tokens @@ to_tsquery('%s')
--				and clkp.fsn ilike ('%s')
--			UNION all
--			
--				select 
--					clkp.conceptid,
--					clkp.preferredTerm,
--					r.destinationid as parentId,
--					clkp.fsn,
--					-1 as level,
--					--sg.level + 1 as level,
--					(sg.srcpath || r.destinationid )::varchar(1022)[] as srcpath
--			  	FROM
--			         terminology.snomedct_relation_active_isa_lookup_mat r
--			     INNER JOIN search_snomed sg 
--			     	ON sg.parentid = r.sourceid 
--			      	AND (r.destinationid <> ALL(sg.srcpath))        -- prevent from cycling 
--			      	--AND sg.level <= 1 --(Total levels will be <level> + 1 )
--			     inner join terminology.snomedct_concept_lookup_mat clkp on clkp.conceptid = r.sourceid
--		)
--	
--	SELECT distinct
--		conceptid,
--		preferredTerm,
--		parentId,
--		fsn,
--		level  -- depth, starting from 1      
--		--srcpath  -- path, stored using an array  
--	FROM search_snomed;
--	$_$, searchQuery, semanticTagToSearch   
--
--);
----raise notice '%s', sql_stmt;
--
--return query execute sql_stmt;    
--
--end;
--$$
--LANGUAGE plpgsql strict;
--
----===============
--
--select * from terminology.udf_snomed_get_ancestor_nodes_search_term_by_tag('par:*', 'disorder');

--=========================END OPTION 2=============================================


--=========================END SEARCH AND GET ANCESTORS=============================================

--=======================================GET IMMEDIATE DESCENDENTS==============================================================

set schema 'terminology';

--drop function terminology.udf_snomed_get_next_descendents;

create or replace function terminology.udf_snomed_get_next_descendents(IN in_conceptIds text[]) 
RETURNS TABLE(conceptId varchar(18), preferredTerm text,parentId varchar(18), fsn text,level int)
language sql as 
$$
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

select * from terminology.udf_snomed_get_next_descendents(array['78399007','195967001','304527002','390798007','389145006']);
select * from terminology.udf_snomed_get_next_descendents(array['78399007']);

--=======================================END GET IMMEDIATE DESCENDENTS==============================================================

--=======================================GET IMMEDIATE ANCESTORS==============================================================

set schema 'terminology';

--drop function terminology.udf_snomed_get_next_ancestors;

create or replace function terminology.udf_snomed_get_next_ancestors(IN in_conceptIds text[]) 
RETURNS TABLE(conceptId varchar(18), preferredTerm text,parentId varchar(18), fsn text,level int)
language sql as 
$$
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

select * from terminology.udf_snomed_get_next_ancestors(array['78399007','195967001','304527002','390798007','389145006']);
select * from terminology.udf_snomed_get_next_ancestors(array['78399007']);

--=======================================END GET IMMEDIATE ANCESTORS==============================================================

--===============================================================================================


--call terminology.procedure1('aa');
--
--set schema 'terminology';
--
--CREATE or replace PROCEDURE terminology.procedure1(INOUT p1 TEXT) 
--AS $$
--BEGIN
--    RAISE NOTICE 'Procedure Parameter: %', p1 ;
--   
--   select 1;
--END ;
--$$
--LANGUAGE plpgsql ;



--=======================================STORED PROCEDURE=================================================================

--============================================DATA IMPORT====================================================================

--========================================================================================================
set schema 'terminology';

--183472
--167899 and 315051
select count(*) from terminology.snomedct_associationrefset_f saf;
--truncate table terminology.snomedct_associationrefset_f;

--727395
--665802 and 1027219
select count(*) from terminology.snomedct_attributevaluerefset_f af2 ;
--truncate table terminology.snomedct_attributevaluerefset_f;

--0
--0 - No txt file - Int
select count(*) from terminology.snomedct_complexmaprefset_f cf2;
--truncate table terminology.snomedct_complexmaprefset_f;

--614011
--581635 and 719275
select count(*) from terminology.snomedct_concept_f cf;
--truncate table terminology.snomedct_concept_f;

--2635910
--2513951 and 2759880 (with Clinical) 
select count(*) from terminology.snomedct_description_f df;

select * from description_f df where conceptid = '17458004';
--truncate table terminology.snomedct_description_f;

--TRUNCATE TABLE snomedct.description_f;
--execute "\COPY snomedct.description_f (id, effectivetime, active, moduleid, conceptid, languagecode, typeid, term, casesignificanceid) FROM '/Users/arunnanjundaswamy/Documents/interneuron/projects/EPMA/uk_sct2cl_29.3.0_20200610000001/SnomedCT_InternationalRF2_PRODUCTION_20180731T120000Z/Full/Terminology/sct2_Description_Full-en_INT_20180731.txt' WITH (FORMAT csv, HEADER true, DELIMITER '	', quote '"')";

--233140
--224459 and 7213754
select count(*) from terminology.snomedct_extendedmaprefset_f ef;
--truncate table terminology.snomedct_extendedmaprefset_f;

--3422345
--3207060 and 5935622
select count(*) from terminology.snomedct_langrefset_f lf;
select * from terminology.snomedct_langrefset_f lf order by lf.id;

--truncate table terminology.snomedct_langrefset_f;

--5720907
--5112181 and 5448996
select count(*) from terminology.snomedct_relationship_f rf;
--truncate table terminology.snomedct_relationship_f;

--503664
--484195 and 484406
select count(*) from terminology.snomedct_simplemaprefset_f sf ;
--truncate table terminology.snomedct_simplemaprefset_f;

--20320
--19870 and 1154729
select count(*) from terminology.snomedct_simplerefset_f sf ;
--truncate table terminology.snomedct_simplerefset_f;

--2063781
--1130957 and 1130957
select count(*) from terminology.snomedct_stated_relationship_f srf ;
--truncate table terminology.snomedct_stated_relationship_f;

--7078
--5151 and 5151
select count(*) from terminology.snomedct_textdefinition_f tf ;
--truncate table terminology.snomedct_textdefinition_f;


--============================================END DATA IMPORT=========================================================


