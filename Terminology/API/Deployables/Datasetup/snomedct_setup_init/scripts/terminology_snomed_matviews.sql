-- terminology.snomedct_concept_latest_mat source
drop MATERIALIZED VIEW if exists terminology.snomedct_concept_latest_mat cascade;
CREATE MATERIALIZED VIEW terminology.snomedct_concept_latest_mat
TABLESPACE pg_default
AS SELECT tmp.id,
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
  WHERE tmp.rnk = 1
WITH DATA;

-- View indexes:
DROP INDEX IF EXISTS snomedct_concept_latest_mat_active_idx;
DROP INDEX IF EXISTS snomedct_concept_latest_mat_id_idx;
DROP INDEX IF EXISTS snomedct_concept_latest_mat_moduleid_idx;

CREATE INDEX snomedct_concept_latest_mat_active_idx ON terminology.snomedct_concept_latest_mat USING btree (active);
CREATE INDEX snomedct_concept_latest_mat_id_idx ON terminology.snomedct_concept_latest_mat USING btree (id);
CREATE INDEX snomedct_concept_latest_mat_moduleid_idx ON terminology.snomedct_concept_latest_mat USING btree (moduleid);


-- terminology.snomedct_description_latest_mat source
drop MATERIALIZED VIEW if exists terminology.snomedct_description_latest_mat cascade;
CREATE MATERIALIZED VIEW terminology.snomedct_description_latest_mat
TABLESPACE pg_default
AS SELECT tmp.id,
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
  WHERE tmp.rnk = 1
WITH DATA;

-- View indexes:
DROP INDEX IF EXISTS snomedct_description_latest_mat_active_idx;
DROP INDEX IF EXISTS snomedct_description_latest_mat_conceptid_idx;
DROP INDEX IF EXISTS snomedct_description_latest_mat_languagecode_idx;
DROP INDEX IF EXISTS snomedct_description_latest_mat_term_idx;
DROP INDEX IF EXISTS snomedct_description_latest_mat_term_tokens_idx;
DROP INDEX IF EXISTS snomedct_description_latest_mat_typeid_idx;

CREATE INDEX snomedct_description_latest_mat_active_idx ON terminology.snomedct_description_latest_mat USING btree (active);
CREATE INDEX snomedct_description_latest_mat_conceptid_idx ON terminology.snomedct_description_latest_mat USING btree (conceptid);
CREATE INDEX snomedct_description_latest_mat_languagecode_idx ON terminology.snomedct_description_latest_mat USING btree (languagecode);
CREATE INDEX snomedct_description_latest_mat_term_idx ON terminology.snomedct_description_latest_mat USING btree (term);
CREATE INDEX snomedct_description_latest_mat_term_tokens_idx ON terminology.snomedct_description_latest_mat USING gin (term_tokens);
CREATE INDEX snomedct_description_latest_mat_typeid_idx ON terminology.snomedct_description_latest_mat USING btree (typeid);



-- terminology.snomedct_langrefset_latest_mat source
drop MATERIALIZED VIEW if exists terminology.snomedct_langrefset_latest_mat cascade;
CREATE MATERIALIZED VIEW terminology.snomedct_langrefset_latest_mat
TABLESPACE pg_default
AS SELECT tmp.id,
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
  WHERE tmp.rnk = 1
WITH DATA;

-- View indexes:
DROP INDEX IF EXISTS snomedct_langrefset_latest_mat_acceptabilityid_idx;
DROP INDEX IF EXISTS snomedct_langrefset_latest_mat_active_idx;
DROP INDEX IF EXISTS snomedct_langrefset_latest_mat_moduleid_idx;
DROP INDEX IF EXISTS snomedct_langrefset_latest_mat_referencedcomponentid_idx;
DROP INDEX IF EXISTS snomedct_langrefset_latest_mat_refsetid_idx;

CREATE INDEX snomedct_langrefset_latest_mat_acceptabilityid_idx ON terminology.snomedct_langrefset_latest_mat USING btree (acceptabilityid);
CREATE INDEX snomedct_langrefset_latest_mat_active_idx ON terminology.snomedct_langrefset_latest_mat USING btree (active);
CREATE INDEX snomedct_langrefset_latest_mat_moduleid_idx ON terminology.snomedct_langrefset_latest_mat USING btree (moduleid);
CREATE INDEX snomedct_langrefset_latest_mat_referencedcomponentid_idx ON terminology.snomedct_langrefset_latest_mat USING btree (referencedcomponentid);
CREATE INDEX snomedct_langrefset_latest_mat_refsetid_idx ON terminology.snomedct_langrefset_latest_mat USING btree (refsetid);


-- terminology.snomedct_conceptpreferredname_latest_mat source
drop MATERIALIZED VIEW if exists terminology.snomedct_conceptpreferredname_latest_mat cascade;
CREATE MATERIALIZED VIEW terminology.snomedct_conceptpreferredname_latest_mat
TABLESPACE pg_default
AS SELECT DISTINCT c.id AS conceptid,
    d.term AS preferredname,
    to_tsvector('english'::regconfig, d.term) AS preferredname_tokens,
    d.id AS descriptionid
   FROM terminology.snomedct_concept_latest_mat c
     JOIN terminology.snomedct_description_latest_mat d ON c.id::text = d.conceptid::text AND d.active = '1'::bpchar AND d.typeid::text = '900000000000013009'::text AND c.active = '1'::bpchar
     JOIN terminology.snomedct_langrefset_latest_mat l ON d.id::text = l.referencedcomponentid::text AND l.active = '1'::bpchar AND l.refsetid::text = '900000000000508004'::text AND l.acceptabilityid::text = '900000000000548007'::text
WITH DATA;

-- View indexes:
DROP INDEX IF EXISTS snomedct_conceptpreferredname_latest_mat_conceptid_idx;
DROP INDEX IF EXISTS snomedct_conceptpreferredname_latest_mat_descriptionid_idx;
DROP INDEX IF EXISTS snomedct_conceptpreferredname_latest_mat_preferredname_idx;
DROP INDEX IF EXISTS snomedct_conceptpreferredname_latest_mat_preferredname_tokens_i;

CREATE INDEX snomedct_conceptpreferredname_latest_mat_conceptid_idx ON terminology.snomedct_conceptpreferredname_latest_mat USING btree (conceptid);
CREATE INDEX snomedct_conceptpreferredname_latest_mat_descriptionid_idx ON terminology.snomedct_conceptpreferredname_latest_mat USING btree (descriptionid);
CREATE INDEX snomedct_conceptpreferredname_latest_mat_preferredname_idx ON terminology.snomedct_conceptpreferredname_latest_mat USING btree (preferredname);
CREATE INDEX snomedct_conceptpreferredname_latest_mat_preferredname_tokens_i ON terminology.snomedct_conceptpreferredname_latest_mat USING gin (preferredname_tokens);



-- terminology.snomedct_concept_lookup_mat source
drop MATERIALIZED VIEW if exists terminology.snomedct_concept_lookup_mat cascade;
CREATE MATERIALIZED VIEW terminology.snomedct_concept_lookup_mat
TABLESPACE pg_default
AS SELECT DISTINCT s.conceptid,
    s.preferredname AS preferredterm,
    s.preferredname_tokens,
    d.term AS fsn,
    rtrim("substring"(d.term, '([^\(]+[\)+])$'::text), ')'::text) AS semantictag
   FROM terminology.snomedct_conceptpreferredname_latest_mat s
     JOIN terminology.snomedct_description_latest_mat d ON d.active = '1'::bpchar AND d.conceptid::text = s.conceptid::text AND d.typeid::text = '900000000000003001'::text
WITH DATA;

-- View indexes:
DROP INDEX IF EXISTS snomedct_concept_lookup_mat_conceptid_hash_idx;
DROP INDEX IF EXISTS snomedct_concept_lookup_mat_fsn_idx;
DROP INDEX IF EXISTS snomedct_concept_lookup_mat_preferredname_tokens_idx;
DROP INDEX IF EXISTS snomedct_concept_lookup_mat_preferredname_tokens_tree_idx;
DROP INDEX IF EXISTS snomedct_concept_lookup_mat_preferredterm_idx;
DROP INDEX IF EXISTS snomedct_concept_lookup_mat_semantictag_idx;

CREATE INDEX snomedct_concept_lookup_mat_conceptid_hash_idx ON terminology.snomedct_concept_lookup_mat USING hash (conceptid);
CREATE INDEX snomedct_concept_lookup_mat_fsn_idx ON terminology.snomedct_concept_lookup_mat USING btree (fsn);
CREATE INDEX snomedct_concept_lookup_mat_preferredname_tokens_idx ON terminology.snomedct_concept_lookup_mat USING gin (preferredname_tokens);
CREATE INDEX snomedct_concept_lookup_mat_preferredname_tokens_tree_idx ON terminology.snomedct_concept_lookup_mat USING btree (preferredname_tokens);
CREATE INDEX snomedct_concept_lookup_mat_preferredterm_idx ON terminology.snomedct_concept_lookup_mat USING btree (preferredterm);
CREATE INDEX snomedct_concept_lookup_mat_semantictag_idx ON terminology.snomedct_concept_lookup_mat USING btree (lower(semantictag));





-- terminology.snomedct_relationship_latest_mat source
drop MATERIALIZED VIEW if exists terminology.snomedct_relationship_latest_mat cascade;
CREATE MATERIALIZED VIEW terminology.snomedct_relationship_latest_mat
TABLESPACE pg_default
AS SELECT tmp.id,
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
  WHERE tmp.rnk = 1
WITH DATA;

-- View indexes:
DROP INDEX IF EXISTS snomedct_relationship_latest_mat_active_idx;
DROP INDEX IF EXISTS snomedct_relationship_latest_mat_destinationid_idx;
DROP INDEX IF EXISTS snomedct_relationship_latest_mat_moduleid_idx;
DROP INDEX IF EXISTS snomedct_relationship_latest_mat_sourceid_idx;
DROP INDEX IF EXISTS snomedct_relationship_latest_mat_typeid_idx;

CREATE INDEX snomedct_relationship_latest_mat_active_idx ON terminology.snomedct_relationship_latest_mat USING btree (active);
CREATE INDEX snomedct_relationship_latest_mat_destinationid_idx ON terminology.snomedct_relationship_latest_mat USING btree (destinationid);
CREATE INDEX snomedct_relationship_latest_mat_moduleid_idx ON terminology.snomedct_relationship_latest_mat USING btree (moduleid);
CREATE INDEX snomedct_relationship_latest_mat_sourceid_idx ON terminology.snomedct_relationship_latest_mat USING btree (sourceid);
CREATE INDEX snomedct_relationship_latest_mat_typeid_idx ON terminology.snomedct_relationship_latest_mat USING btree (typeid);


-- terminology.snomedct_relation_active_isa_lookup_mat source
drop MATERIALIZED VIEW if exists terminology.snomedct_relation_active_isa_lookup_mat cascade;
CREATE MATERIALIZED VIEW terminology.snomedct_relation_active_isa_lookup_mat
TABLESPACE pg_default
AS SELECT DISTINCT srlm.sourceid,
    srlm.destinationid
   FROM terminology.snomedct_relationship_latest_mat srlm
  WHERE srlm.active = '1'::bpchar AND srlm.typeid::text = '116680003'::text
WITH DATA;

-- View indexes:
DROP INDEX IF EXISTS  snomedct_relation_active_isa_lookup_mat_destinationid_hash_idx;
DROP INDEX IF EXISTS  snomedct_relation_active_isa_lookup_mat_destinationid_srcid_idx;
DROP INDEX IF EXISTS  snomedct_relation_active_isa_lookup_mat_sourceid_hash_idx;
DROP INDEX IF EXISTS snomedct_relation_active_isa_lookup_mat_srciddestinationid_idx;

CREATE INDEX snomedct_relation_active_isa_lookup_mat_destinationid_hash_idx ON terminology.snomedct_relation_active_isa_lookup_mat USING hash (destinationid);
CREATE INDEX snomedct_relation_active_isa_lookup_mat_destinationid_srcid_idx ON terminology.snomedct_relation_active_isa_lookup_mat USING btree (destinationid) INCLUDE (sourceid);
CREATE INDEX snomedct_relation_active_isa_lookup_mat_sourceid_hash_idx ON terminology.snomedct_relation_active_isa_lookup_mat USING hash (sourceid);
CREATE UNIQUE INDEX snomedct_relation_active_isa_lookup_mat_srciddestinationid_idx ON terminology.snomedct_relation_active_isa_lookup_mat USING btree (sourceid, destinationid);


-- terminology.snomedct_relationshipwithnames_latest_mat source
drop MATERIALIZED VIEW if exists terminology.snomedct_relationshipwithnames_latest_mat cascade;
CREATE MATERIALIZED VIEW terminology.snomedct_relationshipwithnames_latest_mat
TABLESPACE pg_default
AS SELECT relationship.id,
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
  WHERE relationship.moduleid::text = cpn1.conceptid::text AND relationship.sourceid::text = cpn2.conceptid::text AND relationship.destinationid::text = cpn3.conceptid::text AND relationship.typeid::text = cpn4.conceptid::text AND relationship.characteristictypeid::text = cpn5.conceptid::text AND relationship.modifierid::text = cpn6.conceptid::text
WITH DATA;

-- View indexes:
DROP INDEX IF EXISTS  snomedct_relationshipwithnames_latest_mat_active_idx;
DROP INDEX IF EXISTS  snomedct_relationshipwithnames_latest_mat_destinationid_idx;
DROP INDEX IF EXISTS  snomedct_relationshipwithnames_latest_mat_destinationidname_idx;
DROP INDEX IF EXISTS  snomedct_relationshipwithnames_latest_mat_destinationidname_tok;
DROP INDEX IF EXISTS  snomedct_relationshipwithnames_latest_mat_moduleidname_idx;
DROP INDEX IF EXISTS  snomedct_relationshipwithnames_latest_mat_sourceid_destid_idx;
DROP INDEX IF EXISTS  snomedct_relationshipwithnames_latest_mat_sourceid_idx;
DROP INDEX IF EXISTS  snomedct_relationshipwithnames_latest_mat_sourceidname_idx;
DROP INDEX IF EXISTS  snomedct_relationshipwithnames_latest_mat_sourceidname_tokens_i;
DROP INDEX IF EXISTS  snomedct_relationshipwithnames_latest_mat_typeid_idx;
DROP INDEX IF EXISTS  snomedct_relationshipwithnames_latest_mat_typeidname_idx;

CREATE INDEX snomedct_relationshipwithnames_latest_mat_active_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (active);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_destinationid_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (destinationid);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_destinationidname_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (destinationidname);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_destinationidname_tok ON terminology.snomedct_relationshipwithnames_latest_mat USING gin (destinationidname_tokens);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_moduleidname_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (moduleidname);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_sourceid_destid_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (sourceid, destinationid);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_sourceid_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (sourceid);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_sourceidname_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (sourceidname);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_sourceidname_tokens_i ON terminology.snomedct_relationshipwithnames_latest_mat USING gin (sourceidname_tokens);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_typeid_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (typeid);
CREATE INDEX snomedct_relationshipwithnames_latest_mat_typeidname_idx ON terminology.snomedct_relationshipwithnames_latest_mat USING btree (typeidname);


-- terminology.snomedct_simplerefset_latest_mat source
drop MATERIALIZED VIEW if exists terminology.snomedct_simplerefset_latest_mat cascade;
CREATE MATERIALIZED VIEW terminology.snomedct_simplerefset_latest_mat
TABLESPACE pg_default
AS SELECT tmp.id,
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
  WHERE tmp.rnk = 1
WITH DATA;

-- View indexes:

DROP INDEX IF EXISTS snomedct_simplerefset_latest_mat_active_idx;
DROP INDEX IF EXISTS snomedct_simplerefset_latest_mat_id_idx;
DROP INDEX IF EXISTS snomedct_simplerefset_latest_mat_moduleid_idx;
DROP INDEX IF EXISTS snomedct_simplerefset_latest_mat_referencedcomponentid_idx;
DROP INDEX IF EXISTS snomedct_simplerefset_latest_mat_refsetid_idx;

CREATE INDEX snomedct_simplerefset_latest_mat_active_idx ON terminology.snomedct_simplerefset_latest_mat USING btree (active);
CREATE INDEX snomedct_simplerefset_latest_mat_id_idx ON terminology.snomedct_simplerefset_latest_mat USING btree (id);
CREATE INDEX snomedct_simplerefset_latest_mat_moduleid_idx ON terminology.snomedct_simplerefset_latest_mat USING btree (moduleid);
CREATE INDEX snomedct_simplerefset_latest_mat_referencedcomponentid_idx ON terminology.snomedct_simplerefset_latest_mat USING btree (referencedcomponentid);
CREATE INDEX snomedct_simplerefset_latest_mat_refsetid_idx ON terminology.snomedct_simplerefset_latest_mat USING btree (refsetid);


-- terminology.snomedct_tradefamilies_mat source
drop MATERIALIZED VIEW if exists terminology.snomedct_tradefamilies_mat cascade;
CREATE MATERIALIZED VIEW terminology.snomedct_tradefamilies_mat
TABLESPACE pg_default
AS SELECT dp_src.conceptid AS branded_drug_id,
    dp_src.term AS branded_drug_term,
    to_tsvector('english'::regconfig, dp_src.term) AS branded_drug_term_tokens,
    dp_src.conceptid AS trade_family_id,
    dp_dest.term AS trade_family_term,
    to_tsvector('english'::regconfig, dp_dest.term) AS trade_family_term_tokens
   FROM terminology.snomedct_simplerefset_latest_mat sr
     JOIN terminology.snomedct_relationship_latest_mat rf ON sr.referencedcomponentid::text = rf.destinationid::text
     JOIN terminology.snomedct_description_latest_mat dp_dest ON dp_dest.conceptid::text = rf.destinationid::text
     JOIN terminology.snomedct_description_latest_mat dp_src ON dp_src.conceptid::text = rf.sourceid::text
  WHERE sr.refsetid::text = '999000631000001100'::text AND dp_dest.typeid::text = '900000000000013009'::text AND dp_src.typeid::text = '900000000000013009'::text AND sr.active = '1'::bpchar AND rf.active = '1'::bpchar AND dp_dest.active = '1'::bpchar AND dp_src.active = '1'::bpchar
WITH DATA;

-- View indexes:

DROP INDEX IF EXISTS  snomedct_tradefamilies_mat_branded_drug_id_idx;
DROP INDEX IF EXISTS  snomedct_tradefamilies_mat_branded_drug_term_tokens_idx;
DROP INDEX IF EXISTS  snomedct_tradefamilies_mat_trade_family_id_idx;
DROP INDEX IF EXISTS  snomedct_tradefamilies_mat_trade_family_term_tokens_idx;

CREATE INDEX snomedct_tradefamilies_mat_branded_drug_id_idx ON terminology.snomedct_tradefamilies_mat USING btree (branded_drug_id);
CREATE INDEX snomedct_tradefamilies_mat_branded_drug_term_tokens_idx ON terminology.snomedct_tradefamilies_mat USING gin (branded_drug_term_tokens);
CREATE INDEX snomedct_tradefamilies_mat_trade_family_id_idx ON terminology.snomedct_tradefamilies_mat USING btree (trade_family_id);
CREATE INDEX snomedct_tradefamilies_mat_trade_family_term_tokens_idx ON terminology.snomedct_tradefamilies_mat USING gin (trade_family_term_tokens);




-- terminology.snomedct_concept_allname_latest_mat source
DROP MATERIALIZED VIEW IF EXISTS terminology.snomedct_concept_allname_latest_mat;
CREATE MATERIALIZED VIEW terminology.snomedct_concept_allname_latest_mat
TABLESPACE pg_default
AS SELECT DISTINCT c.id AS conceptid,
    d.term AS name,
    to_tsvector('english'::regconfig, d.term) AS name_tokens,
    d.id AS descriptionid
   FROM terminology.snomedct_concept_latest_mat c
     JOIN terminology.snomedct_description_latest_mat d ON c.id::text = d.conceptid::text AND d.active = '1'::bpchar AND d.typeid::text = '900000000000013009'::text AND c.active = '1'::bpchar
     JOIN terminology.snomedct_langrefset_latest_mat l ON d.id::text = l.referencedcomponentid::text AND l.active = '1'::bpchar AND l.refsetid::text = '900000000000508004'::text
WITH DATA;

-- View indexes:
CREATE INDEX snomedct_concept_allname_latest_mat_conceptid_idx ON terminology.snomedct_concept_allname_latest_mat USING btree (conceptid);
CREATE INDEX snomedct_concept_allname_latest_mat_descriptionid_idx ON terminology.snomedct_concept_allname_latest_mat USING btree (descriptionid);
CREATE INDEX snomedct_concept_allname_latest_mat_name_idx ON terminology.snomedct_concept_allname_latest_mat USING btree (name);
CREATE INDEX snomedct_concept_allname_latest_mat_name_tokens_i ON terminology.snomedct_concept_allname_latest_mat USING gin (name_tokens);


-- terminology.snomedct_concept_all_lookup_mat source
DROP MATERIALIZED VIEW IF EXISTS terminology.snomedct_concept_all_lookup_mat;
CREATE MATERIALIZED VIEW terminology.snomedct_concept_all_lookup_mat
TABLESPACE pg_default
AS SELECT DISTINCT s.conceptid,
    s.name AS preferredterm,
    s.name_tokens AS preferredname_tokens,
    d.term AS fsn,
    rtrim("substring"(d.term, '([^\(]+[\)+])$'::text), ')'::text) AS semantictag
   FROM terminology.snomedct_concept_allname_latest_mat s
     JOIN terminology.snomedct_description_latest_mat d ON d.active = '1'::bpchar AND d.conceptid::text = s.conceptid::text AND d.typeid::text = '900000000000003001'::text
WITH DATA;

-- View indexes:
CREATE INDEX snomedct_concept_all_lookup_mat_conceptid_hash_idx ON terminology.snomedct_concept_all_lookup_mat USING hash (conceptid);
CREATE INDEX snomedct_concept_all_lookup_mat_mat_fsn_idx ON terminology.snomedct_concept_all_lookup_mat USING btree (fsn);
CREATE INDEX snomedct_concept_all_lookup_mat_name_idx ON terminology.snomedct_concept_all_lookup_mat USING btree (preferredterm);
CREATE INDEX snomedct_concept_all_lookup_mat_name_tokens_idx ON terminology.snomedct_concept_all_lookup_mat USING gin (preferredname_tokens);
CREATE INDEX snomedct_concept_all_lookup_mat_name_tokens_tree_idx ON terminology.snomedct_concept_all_lookup_mat USING btree (preferredname_tokens);
CREATE INDEX snomedct_concept_all_lookup_mat_semantictag_idx ON terminology.snomedct_concept_all_lookup_mat USING btree (lower(semantictag));