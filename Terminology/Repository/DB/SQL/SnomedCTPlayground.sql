select * from snomedct_conceptpreferredname_latest_mat m
where m.preferredname = 'asthma'

SELECT distinct s.conceptid, s.descriptionid, s.preferredname, d.typeid, d.term 
FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
inner join snomedct_description_latest_mat d 
on d.active = '1' and d.conceptid = s.conceptid 
WHERE 
--d.term = concat(s.preferredname,' (regime/therapy)') and
--(d.term ilike 'Asth%' or d.term ilike ('% asth%'))
(s.preferredname ilike 'Ast%' or s.preferredname ilike '% ast%')
and d.term ilike '% (situation)' 

and d.typeid = '900000000000003001'
--and d.conceptid = '734346005'
--and d.casesignificanceid ='900000000000020002'               
--order by d.term 

SELECT s.conceptid AS "Conceptid", s.descriptionid AS "Descriptionid", s.preferredname AS "Preferredname"
FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
INNER JOIN terminology.snomedct_description_latest_mat AS s0 ON s.conceptid = s0.conceptid
WHERE ((s0.active = '1') AND s.preferredname ILIKE 'Asth%' ESCAPE '') 
OR ((s.preferredname ILIKE '% asth%' ESCAPE '' AND s0.term ILIKE '% (regime/therapy)' ESCAPE '') 
AND (s0.typeid = '900000000000003001'))

select l.*, d.term, d.typeid from snomedct_description_latest_mat  d 
inner JOIN snomedct_langrefset_latest_mat l
  ON d.id = l.referencedComponentId
  AND l.active = '1'
  AND l.refSetId = '900000000000508004'  -- GB English
  AND l.acceptabilityId = '900000000000548007' -- Preferred term
where 
d.conceptid = '160377001'--'106844005' --'734346005'
and 
d.active = '1' and d.typeid ='900000000000003001'

select * from snomedct_concept_latest_mat sclm 
where id = '106844005'--'734346005'

select * from snomedct_langrefset_f slf 
where slf.referencedcomponentid = ''


set schema 'terminology'

select * from snomedct_concept_f scf 
where id in (
SELECT s.conceptid
FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
WHERE s.preferredname = 'Asthma'

SELECT s.conceptid, s.descriptionid, s.preferredname, d.typeid, d.term 
FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
inner join snomedct_description_f d on d.active = '1' and d.conceptid = s.conceptid 
WHERE  s.preferredname = concat(s.preferredname,' (disorder)')
	and (s.preferredname ilike ('%asth%') or s.preferredname ilike ('%asth') 
		or s.preferredname ilike ('asth%'));
	

set scheme 'terminology'
--this is inusage
SELECT distinct s.conceptid, s.descriptionid, s.preferredname, d.typeid, d.term 
FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
inner join snomedct_description_latest_mat d 
on d.active = '1' and d.conceptid = s.conceptid 
--inner JOIN snomedct_langrefset_latest_mat l
--  ON d.id = l.referencedComponentId
--  AND l.active = '1'
--  AND l.refSetId = '900000000000508004'  -- GB English
--  AND l.acceptabilityId = '900000000000548007' -- Preferred term
WHERE 
--d.term = concat(s.preferredname,' (regime/therapy)') and
--(d.term ilike 'Asth%' or d.term ilike ('% asth%'))
(s.preferredname ilike 'Ast%' or s.preferredname ilike '% ast%')
and d.term ilike '% (finding)' 
and d.typeid = '900000000000003001'
--and d.conceptid = '734346005'
--and d.casesignificanceid ='900000000000020002'               
--order by d.term 

--using above but with FTS
SELECT distinct s.conceptid, s.descriptionid, s.preferredname, d.typeid, d.term 
FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
inner join snomedct_description_latest_mat d 
on d.active = '1' and d.conceptid = s.conceptid 
--inner JOIN snomedct_langrefset_latest_mat l
--  ON d.id = l.referencedComponentId
--  AND l.active = '1'
--  AND l.refSetId = '900000000000508004'  -- GB English
--  AND l.acceptabilityId = '900000000000548007' -- Preferred term
WHERE 
--d.term = concat(s.preferredname,' (regime/therapy)') and
--(d.term ilike 'Asth%' or d.term ilike ('% asth%'))
(s.preferredname_tokens @@ to_tsquery('Ast:* & wo:*'))
and d.term ilike '% (disorder)' 
and d.typeid = '900000000000003001'
--and d.conceptid = '734346005'
--and d.casesignificanceid ='900000000000020002'               
--order by d.term 

SELECT distinct s.conceptid, s.descriptionid, s.preferredname, d.typeid, d.term, d.* 
FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
inner join snomedct_description_latest_mat d 
on d.active = '1' and d.conceptid = s.conceptid 
inner JOIN snomedct_langrefset_latest_mat l
  ON d.id = l.referencedComponentId
  AND l.active = '1'
  AND l.refSetId = '900000000000508004'  -- GB English
  AND l.acceptabilityId = '900000000000548007' -- Preferred term
WHERE 
--d.term = concat(s.preferredname,' (regime/therapy)') and
--(d.term ilike 'Asth%' or d.term ilike ('% asth%'))
(s.preferredname ilike 'Ast%' or s.preferredname ilike '% ast%')
and d.term ilike '% (finding)' 
and d.typeid = '900000000000003001'
and d.conceptid = '294111000'--'734346005'
--and d.casesignificanceid ='900000000000020002'               
--order by d.term 

select * from snomedct_langrefset_latest_mat l
where l.referencedcomponentid in ('3748346014', '434367019') 
and l.active = '1'
and l.refsetid = '900000000000508004'
and l.acceptabilityid = '900000000000548007'


--294111000
--294197000



select * from snomedct_description_latest_mat sdlm 
where 
--sdlm .conceptid = '160377001' --'60441008'-- '106844005'
sdlm.id = '3748346014'--'100428015'--'3778518019'

select id from snomedct_description_latest_mat
group by id having count(*) > 2 


--=============================Depth TRAVERSING===================================

set schema 'terminology';

--Incorrect
WITH RECURSIVE search_graph(      
  conceptid,
  term,
  parentConceptId,
  typeid,
  fsn,
  prop,   -- edge property      
  depth,  -- depth, starting from 1      
  path  -- path, stored using an array      
) as 

	(
		SELECT distinct 
			s.conceptid,
			s.preferredname as term,
		  	null as parentConceptId,
			d.typeid, 
			d.term as fsn,
			null as prop,
			1 as depth,
			ARRAY[s.conceptid]::varchar(255)[] as path
		FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
		inner join snomedct_description_latest_mat d 
		on d.active = '1' and d.conceptid = s.conceptid 
		WHERE 
		(s.preferredname_tokens @@ to_tsquery('par:*'))
		and d.term ilike '% (medicinal product form)' 
		and d.typeid = '900000000000003001'
	UNION all
		select distinct
			r.sourceid as conceptid,
			r.sourceidname as term,
			r.destinationid as parenConceptId,
			d.typeid, 
			d.term as fsn,
			null as prop,
			s.depth + 1 as depth,
			(s.path || r.destinationid)::varchar(255)[] as path
	  	FROM
	         snomedct_relationshipwithnames_latest_mat r
	     INNER JOIN search_graph s 
	     	ON s.conceptid = r.destinationid
	      	AND (r.sourceid <> ALL(s.path))        -- prevent from cycling 
	      	AND s.depth <= 14
	     inner join snomedct_description_latest_mat d 
			on d.active = '1' and d.conceptid = r.sourceid and d.typeid = '900000000000003001'
		where r.active = '1' and r.typeIdName = 'Is a' 

	)
SELECT distinct * FROM search_graph order by depth;
where depth = 1; limit 4000;

{195967001,195967001,370219009,427295004,707446004,135171000119106,707980005}
{195967001,195967001,370219009,427295004,707446004,135171000119106,707980005}

{437876006,437876006,772363000,772360002,768588002,778578002}
{437876006,437876006,772361003,772360002,768588002,778578002}

--==========
explain
WITH RECURSIVE search_graph(      
  conceptid,
  term,
  destinationid,
  typeid,
  fsn,
  prop,   -- edge property      
  depth,  -- depth, starting from 1      
  path  -- path, stored using an array      
) as 

	(
		SELECT distinct 
			s.conceptid ,
			s.preferredname as term,
			null as destinationid,
			d.typeid, 
			d.term as fsn,
			null as prop,
			1 as depth,
			ARRAY[s.conceptid]::varchar(1022)[] as path
		FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
		inner join terminology.snomedct_description_latest_mat d 
		on d.active = '1' and d.conceptid = s.conceptid 
		WHERE 
		(s.preferredname_tokens @@ to_tsquery('par:* & 80:*'))
		and d.term ilike '% (clinical drug)' 
		and d.typeid = '900000000000003001'
		UNION all
		select 
			r.sourceid as conceptid,
			r.sourceidname as term,
			r.destinationid,
			d.typeid, 
			d.term as fsn,
			r.typeIdName as prop,
			s.depth + 1 as depth,
			(s.path || r.destinationid)::varchar(1022)[] as path
	  	FROM
	         snomedct_relationshipwithnames_latest_mat r
	     INNER JOIN search_graph s 
	     	ON s.conceptid = r.destinationid
	      	AND (r.sourceid <> ALL(s.path))        -- prevent from cycling 
	      	AND s.depth <= 15
	     inner join snomedct_description_latest_mat d 
			on d.active = '1' and d.conceptid = r.sourceid and d.typeid = '900000000000003001'
		where r.active = '1' and r.typeIdName = 'Is a' 
		
	)
SELECT * FROM search_graph;

--where destinationid = '13791008'
--limit 4000;
--order by depth;

{13791008,13791008,161874006,373931001}
	


WITH RECURSIVE allchild AS (
       select distinct 
		con.id as conceptid,
		concat(preferredname,' (disorder)') as term,
		null as parenConcepttId,
		1 as n
		from snomedct.conceptpreferredname_mat prefname,
				snomedct.description_latest_mat des,
				snomedct.concept_f con,
				snomedct.langrefset_f lang
		where des.id = lang.referencedcomponentid 
			and lang.active = '1' 
			and lang.refsetid = '900000000000508004'  -- GB English
			and con.id = des.conceptid 
			and con.active = '1'
			and des.term = concat(prefname.preferredname,' (disorder)')
			--and (prefname.preferredname ilike ('%ast%') or prefname.preferredname ilike ('%ast') 
				--or prefname.preferredname ilike ('ast%'))
			and (prefname.preferredname ilike ('ast%') or prefname.preferredname ilike (' ast%') or 
			 prefname.preferredname ilike ('% ast%'))
       union all 
              select 
	              r.sourceid as conceptid,
	              r.sourceidname as term,
          		  r.destinationid as parenConcepttId,
	              n+1 as n
              FROM
                     "SCT_1".snomedct.relationshipwithnames_mat r
                    
              INNER JOIN allchild s ON s.conceptid = r.destinationid
              where  r.active = '1' and r.typeIdName = 'Is a'
) 
SELECT
       *
FROM
       allchild limit 1000;
       
      
--========================LTREE EXPERIMENTS========================================
--LTREE

set schema 'terminology';

CREATE EXTENSION  IF NOT EXISTS  LTREE
     WITH   SCHEMA  terminology;

--DROP TABLE mcc_terminology.terminology.tmp_ltree_exp1;
CREATE TABLE mcc_terminology.terminology.tmp_ltree_exp1 (
	conceptid text,
    term TEXT,
    term_tokens tsvector,
    parentid text,
    typeid text,
    fsn text,
    depth int,
    parentpath LTREE,
    parentpath_arr varchar(1022)[]
);

CREATE INDEX tmp_ltree_exp1_term_tokens_idx ON terminology.tmp_ltree_exp1 using gin(term_tokens);
CREATE INDEX tmp_ltree_exp1_parent_path_idx ON terminology.tmp_ltree_exp1  USING GIST (parentpath);
CREATE INDEX tmp_ltree_exp1_parent_path_tree_idx ON terminology.tmp_ltree_exp1  USING btree(parentpath);

CREATE INDEX tmp_ltree_exp1_parent_id_idx ON terminology.tmp_ltree_exp1  (parentid);

--CREATE OR REPLACE FUNCTION terminology.udf_get_source_id_node_path(param_source_id text)
--  RETURNS ltree AS
--$$
--SELECT  CASE WHEN (r.destinationid IS null or r.destinationid = '138875005') THEN r.sourceid::text::ltree 
--            ELSE udf_get_source_id_node_path(r.destinationid ) || r.sourceid ::text END
--    FROM terminology.snomedct_relationship_f As r
--    WHERE r.sourceid = $1 and r.typeid = '116680003' and r.active = '1';
--$$
--  LANGUAGE sql;
-- 
-- select * from  terminology.udf_get_source_id_node_path('10401000087105');

select * from snomedct_relationship_f 
where sourceid  = '10401000087105' --'123037004'--'138875005'--is null
limit 10;

select * from terminology.snomedct_relationshipwithnames_latest_mat 
where sourceid  = '10401000087105' --'123037004'--'138875005'--is null
limit 10;
 
-- //to test
 select 
 r.sourceid, 
 r.sourceidname , 
 r.destinationid,
 r.typeidname,
 r.typeid
 --terminology.udf_get_source_id_node_path(r.sourceid) as parentpath
 from terminology.snomedct_relationshipwithnames_latest_mat r
 where r.typeidname = 'Is a'
 limit 57;

select * from terminology.tmp_ltree_exp1
where 
--depth = 1
--term = 'Specimen from trophoblast'
parentpath <@ '138875005.123038009' or parentpath <@ '138875005.123037004';
--conceptid = '123038009';

--truncate table terminology.tmp_ltree_exp1;

--drop materialized VIEW terminology.tmp_ltree_exp1_mat;
CREATE materialized VIEW terminology.tmp_ltree_exp1_mat as
select * from terminology.tmp_ltree_exp1;

CREATE INDEX tmp_ltree_exp1_mat_term_tokens_idx ON terminology.tmp_ltree_exp1_mat using gin(term_tokens);
CREATE INDEX tmp_ltree_exp1_mat_parent_path_idx ON terminology.tmp_ltree_exp1_mat  USING GIST (parentpath);
CREATE INDEX tmp_ltree_exp1_mat_parent_id_idx ON terminology.tmp_ltree_exp1_mat  (parentid);

select * from terminology.tmp_ltree_exp1_mat
where 
--depth = 1
--term = 'Specimen from trophoblast'
parentpath <@ '138875005.123038009' or parentpath <@ '138875005.123037004';
--conceptid = '123038009';


--insert into terminology.tmp_ltree_exp1 
CREATE materialized VIEW terminology.tmp_ltree_exp1_mat as

select * from (

WITH RECURSIVE search_graph(      
  sourceid,
  term,
  term_tokens,
  destinationid,
  typeid,
  fsn,
  depth,  -- depth, starting from 1      
  path,
  path_arr-- path, stored using an array      
) as 

	(
		SELECT distinct 
			r.sourceid ,
			con.preferredname as term,
		    to_tsvector(con.preferredname) term_tokens,
			r.destinationid,
			r.typeid, 
			d.term as fsn,
			1 as depth, --this is already 2nd level --since root level is always 138875005
			r.destinationid || r.sourceid::text::ltree as path,
			ARRAY[r.destinationid || r.sourceid ]::varchar(1022)[] as path_arr
		FROM terminology.snomedct_relationship_latest_mat r
		inner join terminology.snomedct_conceptpreferredname_latest_mat AS con 
			on con.conceptid = r.sourceid 
		inner join terminology.snomedct_description_latest_mat d 
			on d.active = '1' and d.conceptid = con.conceptid and d.typeid = '900000000000003001'
		where r.active = '1' and r.destinationid = '138875005' -- root node of all
		and r.typeid = '116680003'
		
		UNION all
		select 
			r.sourceid,
			conc.preferredname as term,
		    to_tsvector(conc.preferredname) term_tokens,
			r.destinationid,
			r.typeid, 
			d.term as fsn,
			sg.depth + 1 as depth,
			(sg.path || r.sourceid )::text::ltree as path,
			(sg.path_arr || r.sourceid)::varchar(1022)[] as path_arr
	  	FROM
	         terminology.snomedct_relationship_latest_mat r
	     INNER JOIN search_graph sg 
	     	ON sg.sourceid = r.destinationid
	      	AND (r.sourceid <> ALL(sg.path_arr))        -- prevent from cycling 
	      	AND sg.depth <= 15
      	inner join terminology.snomedct_conceptpreferredname_latest_mat AS conc 
      		on conc.conceptid = r.sourceid 
		inner join terminology.snomedct_description_latest_mat d 
		on d.active = '1' and d.conceptid = conc.conceptid and d.typeid = '900000000000003001'
	     where r.active = '1' and r.typeid = '116680003'
		
	)
SELECT * FROM search_graph
--where depth = 3;
) a;

{105590001,105590001}

--==============option 2=====================
--drop materialized VIEW terminology.tmp_ltree_exp2_mat;
CREATE materialized VIEW terminology.tmp_ltree_exp2_mat as
select * from (
		WITH RECURSIVE search_graph(      
		  sourceid,
		  destinationid,
		  path,
		  level
		  --pathArr
		) as (
				select 
					r.sourceid,
					r.destinationid,
					r.destinationid || r.sourceid::text::ltree as path,
					1 as level
					--ARRAY[r.destinationid || r.sourceid ]::varchar(1022)[] as pathArr
		
				from terminology.snomedct_relationship_latest_mat r
				where r.active = '1' 
				and r.typeid = '116680003' 
				and r.destinationid = '138875005'--root node
				UNION all
				select 
					r.sourceid,
					r.destinationid,
					(sg.path || r.sourceid )::text::ltree as path,
					level + 1 as level
					--(sg.pathArr || r.sourceid)::varchar(1022)[] as pathArr
		
				from terminology.snomedct_relationship_latest_mat r
				inner join search_graph sg on r.destinationid = sg.sourceid
			      	--AND (r.sourceid <> ALL(sg.pathArr))        -- prevent from cycling 
			      	--AND sg.level <= 25
				where r.active = '1' and r.typeid = '116680003')
		SELECT * FROM search_graph
) tbl;


CREATE INDEX tmp_ltree_exp2_mat_parent_path_idx ON terminology.tmp_ltree_exp2_mat USING GIST(path);
--CREATE INDEX tmp_ltree_exp2_mat_path_btree_idx ON terminology.tmp_ltree_exp2_mat using btree(path);
CREATE INDEX tmp_ltree_exp2_mat_path_idx ON terminology.tmp_ltree_exp2_mat (path);

--CREATE INDEX tmp_ltree_exp2_mat_sourceid_idx ON terminology.tmp_ltree_exp2_mat using hash(sourceid);
--CREATE INDEX tmp_ltree_exp2_mat_destinationid_idx ON terminology.tmp_ltree_exp2_mat using hash(destinationid);
DROP INDEX tmp_ltree_exp2_mat_destinationid_idx ON terminology.tmp_ltree_exp2_mat using hash(destinationid)
CREATE INDEX tmp_ltree_exp2_mat_sourceid_tree_idx ON terminology.tmp_ltree_exp2_mat using btree(sourceid);
CREATE INDEX tmp_ltree_exp2_mat_destinationid_tree_idx ON terminology.tmp_ltree_exp2_mat using btree(destinationid);
CREATE INDEX tmp_ltree_exp2_mat_srcid_destinationid_idx ON terminology.tmp_ltree_exp2_mat (sourceid, destinationid);

CREATE INDEX tmp_ltree_exp2_mat_level_idx ON terminology.tmp_ltree_exp2_mat using btree(level);

   explain   
  select * from terminology.tmp_ltree_exp2_mat
where 
--level = 6
--term = 'Specimen from trophoblast'
path <@ '138875005.105590001.255640000.259105007.59545008.45620004' or path <@ '138875005.123037004';
--conceptid = '123038009';    
  
--Waste
select * from
(select distinct
r.sourceid as conceptid,
r.sourceidname as preferredTerm,
r.destinationid as parentId,
d.typeid,
d.term as fsn
--tree."level" as level,
--tree."path" as path
from terminology.snomedct_relationshipwithnames_latest_mat r
--inner join terminology.tmp_ltree_exp2_mat tree on tree.sourceid = r.sourceid and tree.destinationid = r.destinationid
inner join terminology.snomedct_description_latest_mat d on d.active = '1' and d.conceptid = r.sourceid 
	WHERE r.active = '1'
	and (r.sourceidname_tokens @@ to_tsquery('ast:*'))
	and d.term ilike '% (disorder)'
	and d.typeid = '900000000000003001') as baseQuery
union all 
(
select distinct
r.sourceid as conceptid,
r.sourceidname as preferredTerm,
r.destinationid as parentId,
d.typeid,
d.term as fsn,
tree."level" as level,
tree."path" as path
from terminology.snomedct_relationshipwithnames_latest_mat r
inner join terminology.tmp_ltree_exp2_mat tree on tree.sourceid = r.sourceid and tree.destinationid = r.destinationid
inner join terminology.snomedct_description_latest_mat d on d.active = '1' and d.conceptid = r.sourceid 
	WHERE r.active = '1'
	and tree."path" <@ baseQuery.path
	and (r.sourceidname_tokens @@ to_tsquery('ast:*'))
	and d.term ilike '% (disorder)'
	and d.typeid = '900000000000003001') as childQuery

--test1
select distinct
r.sourceid as conceptid,
r.sourceidname as preferredTerm,
r.destinationid as parentId,
d.typeid,
d.term as fsn,
tree."level" as level,
tree."path" as path
from terminology.snomedct_relationshipwithnames_latest_mat r
inner join terminology.tmp_ltree_exp2_mat tree on tree.sourceid = r.sourceid and tree.destinationid = r.destinationid
inner join terminology.snomedct_description_latest_mat d on d.active = '1' and d.conceptid = r.sourceid 
	WHERE r.active = '1'
	and (r.sourceidname_tokens @@ to_tsquery('ast:*'))
	and d.term ilike '% (disorder)'
	and d.typeid = '900000000000003001'
	and tree."path"  in (

	--test1
select distinct
tree."path" as path
from terminology.snomedct_relationshipwithnames_latest_mat r
inner join terminology.tmp_ltree_exp2_mat tree on tree.sourceid = r.sourceid and tree.destinationid = r.destinationid
inner join terminology.snomedct_description_latest_mat d on d.active = '1' and d.conceptid = r.sourceid 
	WHERE r.active = '1'
	and (r.sourceidname_tokens @@ to_tsquery('ast:*'))
	and d.term ilike '% (disorder)'
	and d.typeid = '900000000000003001') baseQuery

	--test2
explain
;with sample_graph_cte as
(select distinct
r.sourceid as conceptid,
r.sourceidname as preferredTerm,
r.destinationid as parentId,
d.typeid,
d.term as fsn,
1 as level,
null as path
--tree."level" as level,
--tree."path" as path
from terminology.snomedct_relationshipwithnames_latest_mat r
--inner join terminology.tmp_ltree_exp2_mat tree on tree.sourceid = r.sourceid and tree.destinationid = r.destinationid
inner join terminology.snomedct_description_latest_mat d on d.active = '1' and d.conceptid = r.sourceid 
	WHERE r.active = '1'
	and (r.sourceidname_tokens @@ to_tsquery('ast:*'))
	and d.term ilike '% (disorder)'
	and d.typeid = '900000000000003001' )--order by level)
select distinct
r.sourceid as conceptid,
r.sourceidname as preferredTerm,
r.destinationid as parentId,
d.typeid,
(sgc.parentId || sgc.conceptid)::text::ltree as fsn,
--d.term as fsn,
tree."level" as level,
--nlevel(tree."path")
index(tree."path", ((sgc.parentId::ltree || sgc.conceptid::ltree)::ltree)) as path
--tree."path" as path
from terminology.tmp_ltree_exp2_mat tree
--inner join sample_graph_cte sgc on tree."path" <@ (sgc.path)
--inner join sample_graph_cte sgc on tree."path" <@ ((sgc.parentId)::ltree)
--inner join sample_graph_cte sgc on tree.destinationid = sgc.parentId and tree.sourceid = sgc.conceptid
inner join sample_graph_cte sgc on sgc.conceptid = tree.destinationid 
	and index(tree."path", ((sgc.parentId::ltree || sgc.conceptid::ltree)::ltree)) > 0
inner join terminology.snomedct_relationshipwithnames_latest_mat r  on tree.sourceid = r.sourceid and tree.destinationid = r.destinationid
inner join terminology.snomedct_description_latest_mat d on d.active = '1' and d.conceptid = r.sourceid 
	WHERE r.active = '1'
	and d.typeid = '900000000000003001' order by tree.level;

explain analyze
select * from terminology.udf_search_term_by_tag('par:*', 'disorder');

select * from terminology.tmp_ltree_exp2_mat t
where t.destinationid = '116175006' and t.sourceid = '10002003'

select tree.sourceid , tree.destinationid, count(*) as cnt  from terminology.tmp_ltree_exp2_mat tree
group by tree.sourceid , tree.destinationid 
having count(*) > 1
	
138875005.404684003.118234003.123946008.118934005.81308009.126952004.254936001.276826005.254938000.100721000119109
--==========================
explain analyze 
with sample_graph_cte as (
	SELECT distinct 
			s.conceptid ,
			s.preferredname as preferredTerm,
			null as parentId,
			d.typeid, 
			d.term as fsn,
			null as prop,
			1 as level
			--tree."path" 
		FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
		inner join terminology.snomedct_description_latest_mat d 
			on d.active = '1' and d.conceptid = s.conceptid 
--		inner join terminology.tmp_ltree_exp2_mat tree 
--			on tree.sourceid = s.conceptid and tree."path" ~ ('*.'||s.conceptid)::lquery--'*.254938000.100721000119109'
		WHERE 
		(s.preferredname_tokens @@ to_tsquery('ast:*'))
		and d.term ilike '% (disorder)' and s.conceptid = '100731000119107'
		and d.typeid = '900000000000003001')
	select distinct
			r.sourceid as conceptid,
			r.sourceidname as preferredTerm,
			r.destinationid as parentId,
			d.typeid,
			--(sgc.parentId || sgc.conceptid)::text::ltree as fsn,
			d.term as fsn,
			tree."level" as level
			--nlevel(tree."path")
			--index(tree."path", ((sgc.parentId::ltree || sgc.conceptid::ltree)::ltree)) as path
			--tree."path" as path
		from terminology.tmp_ltree_exp2_mat tree
		--inner join sample_graph_cte sgc on tree."path" <@ (sgc.path)
		--inner join sample_graph_cte sgc on tree."path" <@ ((sgc.parentId)::ltree)
		--inner join sample_graph_cte sgc on tree.destinationid = sgc.parentId and tree.sourceid = sgc.conceptid
		inner join sample_graph_cte sgc 
				on tree."path" ~ ('*.'|| sgc.conceptid || '.*')::lquery
				--on tree."path" @> ('*.'|| sgc.conceptid || '.*')::ltree
				--on tree.destinationid  = sgc.conceptid --and
				--index(tree."path", (sgc.parentId::ltree || sgc.conceptid::ltree)::ltree, -1) == 0
		inner join terminology.snomedct_relationshipwithnames_latest_mat r  
			on tree.sourceid = r.sourceid and tree.destinationid = r.destinationid
		inner join terminology.snomedct_description_latest_mat d on d.active = '1' and d.conceptid = r.sourceid 
			WHERE r.active = '1'
			and d.typeid = '900000000000003001' 
		order by tree.level;


select 
index(path, '245679003.245669000', -0) as ind,
*
from terminology.tmp_ltree_exp2_mat tree
--where tree."path"
where index(path, ('245679003.245669000')::ltree, 0) > 0
limit 10;

138875005.123037004.442083009.91723000.113343008.49596003.119210004.410613002.245643006.245669000.245680000
--===========================
set schema 'terminology';


SELECT distinct 
		s.conceptid ,
		s.preferredname as preferredTerm,
		null as parentId,
		d.typeid, 
		d.term as fsn,
		null as prop,
		1 as level,
		ARRAY[s.conceptid]::varchar(1022)[] as path
	FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
	inner join terminology.snomedct_description_latest_mat d 
	on d.active = '1' and d.conceptid = s.conceptid 
	WHERE 
	(s.preferredname_tokens @@ to_tsquery('ast:*'))
	and d.term ilike '% (disorder)'
	and d.typeid = '900000000000003001'
	


--========================END LTREE EXPERIMENTS========================================
select a.sourceid, a.destinationid,count(*) as cnt from 
(select *
	FROM
     terminology.snomedct_relationshipwithnames_latest_mat r
inner join terminology.snomedct_description_latest_mat d 
on d.active = '1' and d.conceptid = r.sourceid and d.typeid = '900000000000003001'
where r.active = '1' and r.typeIdName = 'Is a') a 
group by a.sourceid, a.destinationid having count(*) > 1;

select * from terminology.snomedct_relationshipwithnames_latest_mat srlm 
where srlm.sourceid = '10001005' and srlm.destinationid =	'91302008'

select * from terminology.snomedct_relationship_latest_mat srlm 
where srlm.sourceid = '10001005' and srlm.destinationid =	'91302008'

select * from
     terminology.snomedct_relationshipwithnames_latest_mat r
inner join terminology.snomedct_description_latest_mat d 
on d.active = '1' and d.conceptid = r.sourceid and d.typeid = '900000000000003001'
where r.active = '1' and r.typeIdName = 'Is a'
and d.conceptid ='1092611000119100';

select * from terminology.snomedct_relation_active_isa_lookup_mat srailm 
where srailm.sourceid = '1092611000119100'

select * from terminology.snomedct_concept_lookup_mat srailm 
where srailm.conceptid = '1092611000119100'

				
	select count(*) from terminology.snomedct_relation_active_isa_lookup_mat;
				
	select distinct 
		s.conceptid,
		s.preferredname as preferredTerm,
		s.preferredname_tokens as preferredname_tokens,
		d.term as fsn,
		trim(trailing  ')' from (trim(leading ' (' from substring(lower(d.term) from '\s\(.*\)$')))) as semantictag
		FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
		inner join terminology.snomedct_description_latest_mat d 
			on d.active = '1' and d.conceptid = s.conceptid and d.typeid = '900000000000003001'
	where (s.preferredname_tokens @@ to_tsquery('par:*'))
				--and d.term ilike ('% (disorder)')
				and d.typeid = '900000000000003001'
				
--3042877
--3042877
--3042877
select a.sourceid, a.destinationid, a.* from (			
SELECT distinct 
		id, 
	   	effectiveTime, 
	   	active,
		moduleId, 
	    cpn1.preferredName moduleIdName,
	    sourceId, 
	    cpn2.preferredName sourceIdName,
	    --to_tsvector(cpn2.preferredName) sourceIdName_tokens,
	    destinationId, 
	    cpn3.preferredName destinationIdName,
	    --to_tsvector(cpn3.preferredName) destinationIdName_tokens,
	    relationshipGroup,
	    typeId, 
	    cpn4.preferredName typeIdName,
	    characteristicTypeId, 
	    cpn5.preferredName characteristicTypeIdName,
	    modifierId, 
	    cpn6.preferredName modifierIdName
from 	snomedct_relationship_latest_mat relationship,
		snomedct_conceptpreferredname_latest_mat cpn1,
		snomedct_conceptpreferredname_latest_mat cpn2,
		snomedct_conceptpreferredname_latest_mat cpn3,
		snomedct_conceptpreferredname_latest_mat cpn4,
		snomedct_conceptpreferredname_latest_mat cpn5,
		snomedct_conceptpreferredname_latest_mat cpn6
WHERE relationship.moduleId = cpn1.conceptId
AND relationship.sourceId = cpn2.conceptId
AND destinationId = cpn3.conceptId
AND typeId = cpn4.conceptId
AND characteristicTypeId = cpn5.conceptId
AND modifierId = cpn6.conceptId) a
where a.sourceid = '10001005' and a.destinationid =	'91302008'

select * from snomedct_conceptpreferredname_latest_mat s
where s.conceptid = '10001005'

select * from snomedct_description_latest_mat d 
where conceptid = '10001005' and d.active = '1'

select * from terminology.snomedct_description_f d 
where conceptid = '10001005' and d.active = '1'
				
				
-- Basics ops improvement================
	explain

select a.conceptid,b.conceptid,a.parentid,b.parentid,a.preferredTerm,a.fsn 
,b.preferredTerm,b.fsn 
from (

	WITH RECURSIVE search_snomed(      
	  conceptid,
	  preferredTerm,
	  parentId,
	  --typeid,
	  fsn,
	  --prop,   -- edge property      
	  level  -- depth, starting from 1      
	  --path  -- path, stored using an array      
	) as (
				SELECT distinct 
					s.conceptid ,
					s.preferredname as preferredTerm,
					null as parentId,
					--d.typeid, 
					d.term as fsn,
					--null as prop,
					1 as level,
					ARRAY[s.conceptid]::varchar(1022)[] as path
				FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
				inner join terminology.snomedct_description_latest_mat d 
				on d.active = '1' and d.conceptid = s.conceptid 
				WHERE 
				(s.preferredname_tokens @@ to_tsquery('par:*'))
				and d.term ilike ('% (disorder)')
				and d.typeid = '900000000000003001'
			UNION all
				select 
					r.sourceid as conceptid,
					r.sourceidname as preferredTerm,
					r.destinationid as parentId,
					--d.typeid, 
					d.term as fsn,
					--r.typeIdName as prop,
					-1 as level,
					--s.level + 1 as level,
					(s.path || r.sourceid)::varchar(1022)[] as path
			  	FROM
			         terminology.snomedct_relationshipwithnames_latest_mat r
			     INNER JOIN search_snomed s 
			     	ON s.conceptid = r.destinationid
			      	AND (r.sourceid <> ALL(s.path))        -- prevent from cycling 
			      	--AND s.level <= 15 --(Total levels will be <level> + 1 )
			    inner join terminology.snomedct_description_latest_mat d 
				on d.active = '1' and d.conceptid = r.sourceid and d.typeid = '900000000000003001'
				where r.active = '1' and r.typeIdName = 'Is a' 
		)
	
	SELECT distinct
		conceptid,
		preferredTerm,
		parentId,
		--typeid,
		fsn,
		--prop as relationship, -- edge property      
		level  -- depth, starting from 1      
		--path  -- path, stored using an array  
	FROM search_snomed 
	order by conceptid 
--where conceptid = '10001005'
) as a

left outer join (

--option 2

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
				where clkp.preferredname_tokens @@ to_tsquery('par:*')
				and clkp.fsn ilike ('% (disorder)')

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
	--where conceptid = '127271000119106'
	--order by conceptid 
	) b
on a.conceptid = b.conceptid 
and ((a.parentid is null and b.parentid is null and a.level <> 1 and b.level <> 1) 
--and (( a.level != 1 and b.level != 1) 
or (a.parentid = b.parentid));
and b.conceptid is null;
--and b.parentid='1092611000119100';

--where a.conceptid is null;
--where conceptid = '10001005';

--=========================

select r.sourceid, 
--r.destinationid, 
count(*) cnt from 
terminology.snomedct_relationship_latest_mat r
group by r.sourceid, 
--r.destinationid, 
r.active, r.typeid 
having count(*) > 2 and r.active = '1' and r.typeid = '116680003'

select c.preferredname ,* from 
terminology.snomedct_relationship_latest_mat r
inner join terminology.snomedct_conceptpreferredname_latest_mat c on c.conceptid = r.typeid 
where r.sourceid = '10019001' 
--and r.destinationid = '408739003'
 and r.typeid = '116680003'
and r.active ='1'


--===========================GET ANCESTORS=====================
select distinct 
s.conceptid,
s.preferredname as preferredTerm,
s.preferredname_tokens as preferredname_tokens,
d.term as fsn
FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
inner join terminology.snomedct_description_latest_mat d 
	on d.active = '1' and d.conceptid = s.conceptid and d.typeid = '900000000000003001'

SELECT distinct 
	s.conceptid,
	s.preferredname as preferredTerm,
	r.destinationid as parentId,
	d.typeid, 
	d.term as fsn,
	null as prop,
	1 as level,
	ARRAY[s.conceptid]::varchar(1022)[] as srcpath
FROM terminology.snomedct_conceptpreferredname_latest_mat AS s
inner join terminology.snomedct_description_latest_mat d 
	on d.active = '1' and d.conceptid = s.conceptid 
inner join terminology.snomedct_relationshipwithnames_latest_mat r
	on r.active = '1' and r.sourceid = s.conceptid and r.typeidname ='Is a'
WHERE 
(s.preferredname_tokens @@ to_tsquery('par:*'))
and d.term ilike '% (disorder)'
and d.typeid = '900000000000003001' 

--REINDEX DATABASE mcc_terminology;

explain (analyze on, timing on, FORMAT JSON, ANALYZE, BUFFERS)
WITH RECURSIVE search_snomed (      
	  conceptid,
	  preferredTerm,
	  parentId,
	  --typeid,
	  fsn,
	  --prop,   -- edge property      
	  level,  -- depth, starting from 1      
	  srcpath  -- path, stored using an array      
	) AS (
				--explain analyze
				select  
					clkp.conceptid,
					clkp.preferredTerm,
					r.destinationid as parentid,
					clkp.fsn,
					--clkp.semantictag,
					1 as level,
					ARRAY[clkp.conceptid ]::varchar(1022)[] as srcpath
				from terminology.snomedct_concept_lookup_mat clkp
				inner join terminology.snomedct_relation_active_isa_lookup_mat r on clkp.conceptid = r.sourceid
				where clkp.preferredname_tokens @@ to_tsquery('par:*')
				--and clkp.fsn like '% (disorder)' --and clkp.conceptid = '722209002'
				and clkp.semantictag = 'disorder'
				--and r.active = '1' and r.typeidname ='Is a'
					
--				SELECT distinct 
--					r.sourceid,
--					r.sourceidname as preferredTerm,
--					r.destinationid as parentId,
--					d.typeid, 
--					d.term as fsn,
--					null as prop,
--					1 as level,
--					ARRAY[r.sourceid ]::varchar(1022)[] as srcpath
--				FROM terminology.snomedct_relationshipwithnames_latest_mat r
--				inner join terminology.snomedct_description_latest_mat d 
--					on d.active = '1' and d.conceptid = r.sourceid 
--				 
--				WHERE 
--				(r.sourceidname_tokens @@ to_tsquery('par:*'))
--				and d.term ilike '% (disorder)'
--				and d.typeid = '900000000000003001'
--				and r.active = '1'  and r.typeidname ='Is a'
			UNION all
				select 
					clkp.conceptid,
					clkp.preferredTerm,
					r.destinationid as parentId,
					clkp.fsn,
					-1 as level,
					--sg.level + 1 as level,
					(sg.srcpath || r.destinationid )::varchar(1022)[] as srcpath
			  	FROM
			         terminology.snomedct_relation_active_isa_lookup_mat r
			     INNER JOIN search_snomed sg 
			     	ON sg.parentid = r.sourceid 
			      	AND (r.destinationid <> ALL(sg.srcpath))        -- prevent from cycling 
			      	--AND sg.level <= 1 --(Total levels will be <level> + 1 )
			     inner join terminology.snomedct_concept_lookup_mat clkp on clkp.conceptid = r.sourceid
--			    inner join terminology.snomedct_description_latest_mat d 
--				on d.active = '1' and d.conceptid = r.sourceid and d.typeid = '900000000000003001'
				--where r.active = '1' and r.typeIdName = 'Is a' 
		)
SELECT distinct
		conceptid,
		preferredTerm,
		parentId,
		--typeid,
		fsn,
		--prop as relationship, -- edge property      
		level  -- depth, starting from 1      
		--srcpath  -- path, stored using an array  
	FROM search_snomed;

select * from terminology.snomedct_relationshipwithnames_latest_mat r 
where r.typeIdName = 'Is a' limit 10;

explain analyse 
--set enable_seqscan=off;

	select distinct 
		clkp.conceptid,
		clkp.preferredTerm,
		r.destinationid as parentid,
		clkp.fsn,
		--clkp.semantictag,
		1 as level
		--ARRAY[clkp.conceptid ]::varchar(1022)[] as srcpath
	from terminology.snomedct_concept_lookup_mat clkp
	inner join terminology.snomedct_relation_active_isa_lookup_mat r on clkp.conceptid = r.sourceid
	where clkp.preferredname_tokens @@ to_tsquery('par:*')
	and clkp.fsn like '% (disorder)' --and clkp.conceptid = '722209002'
	and clkp.semantictag = 'disorder'
	
	
explain analyze 
select * from terminology.snomedct_concept_lookup_mat clkp
				inner join terminology.snomedct_relation_active_isa_lookup_mat r on r.sourceid = clkp.conceptid 
				where clkp.preferredname_tokens @@ to_tsquery('par:*')
				--and clkp.fsn like '% (disorder)' --and clkp.conceptid = '722209002'
				and clkp.semantictag = 'disorder';
			
set schema 'terminology';

explain analyze 

CREATE TEMP TABLE mytable AS
select distinct 
		clkp.conceptid,
		clkp.preferredTerm,
		r.destinationid as parentid,
		clkp.fsn,
		--clkp.semantictag,
		1 as level,
		ARRAY[clkp.conceptid ]::varchar(1022)[] as srcpath
	from terminology.snomedct_concept_lookup_mat clkp
	inner join terminology.snomedct_relation_active_isa_lookup_mat r on clkp.conceptid = r.sourceid
	where clkp.preferredname_tokens @@ to_tsquery('par:*')
	--and clkp.fsn like '% (disorder)' --and clkp.conceptid = '722209002'
	and clkp.semantictag = 'disorder';
			
WITH RECURSIVE search_snomed (      
	  conceptid,
	  preferredTerm,
	  parentId,
	  --typeid,
	  fsn,
	  --prop,   -- edge property      
	  level,  -- depth, starting from 1      
	  srcpath  -- path, stored using an array      
	) AS (
				--explain analyze
				select  
					clkp.conceptid,
					clkp.preferredTerm,
					clkp.parentid,
					clkp.fsn,
					--clkp.semantictag,
					1 as level,
					ARRAY[clkp.conceptid ]::varchar(1022)[] as srcpath
				from mytable clkp
			UNION all
				select 
					clkp.conceptid,
					clkp.preferredTerm,
					r.destinationid as parentId,
					clkp.fsn,
					-1 as level,
					--sg.level + 1 as level,
					(sg.srcpath || r.destinationid )::varchar(1022)[] as srcpath
			  	FROM
			         terminology.snomedct_relation_active_isa_lookup_mat r
			     INNER JOIN search_snomed sg 
			     	ON sg.parentid = r.sourceid 
			      	AND (r.destinationid <> ALL(sg.srcpath))        -- prevent from cycling 
			      	--AND sg.level <= 1 --(Total levels will be <level> + 1 )
			     inner join terminology.snomedct_concept_lookup_mat clkp on clkp.conceptid = r.sourceid
--			    inner join terminology.snomedct_description_latest_mat d 
--				on d.active = '1' and d.conceptid = r.sourceid and d.typeid = '900000000000003001'
				--where r.active = '1' and r.typeIdName = 'Is a' 
		)
SELECT distinct
		conceptid,
		preferredTerm,
		parentId,
		--typeid,
		fsn,
		--prop as relationship, -- edge property      
		level  -- depth, starting from 1      
		--srcpath  -- path, stored using an array  
	FROM search_snomed;
			
drop table mytable;
				

	