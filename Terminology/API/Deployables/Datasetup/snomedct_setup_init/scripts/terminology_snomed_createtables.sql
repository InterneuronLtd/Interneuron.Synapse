-- terminology.snomedct_associationrefset_f definition

-- Drop table

-- DROP TABLE terminology.snomedct_associationrefset_f;

--DROP SCHEMA IF EXISTS terminology CASCADE;

CREATE SCHEMA IF NOT EXISTS terminology;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";


--=================================ABOVE STATEMENTS SHOULD NOT BE DELETED ==========================================================================


-- terminology.snomedct_associationrefset_f definition

-- Drop table

DROP TABLE IF EXISTS terminology.snomedct_associationrefset_f CASCADE;
CREATE TABLE IF NOT EXISTS terminology.snomedct_associationrefset_f (
	id uuid NOT NULL,
	effectivetime bpchar(8) NOT NULL,
	active bpchar(1) NOT NULL,
	moduleid varchar(18) NOT NULL,
	refsetid varchar(18) NOT NULL,
	referencedcomponentid varchar(18) NOT NULL,
	targetcomponentid varchar(18) NOT NULL
);
CREATE INDEX snomedct_associationrefset_f_idx ON terminology.snomedct_associationrefset_f USING btree (referencedcomponentid, targetcomponentid);


-- terminology.snomedct_attributevaluerefset_f definition

-- --DROP TABLE

DROP TABLE IF EXISTS terminology.snomedct_attributevaluerefset_f CASCADE;

CREATE TABLE IF NOT EXISTS terminology.snomedct_attributevaluerefset_f (
	id uuid NOT NULL,
	effectivetime bpchar(8) NOT NULL,
	active bpchar(1) NOT NULL,
	moduleid varchar(18) NOT NULL,
	refsetid varchar(18) NOT NULL,
	referencedcomponentid varchar(18) NOT NULL,
	valueid varchar(18) NOT NULL
);
CREATE INDEX snomedct_attributevaluerefset_f_idx ON terminology.snomedct_attributevaluerefset_f USING btree (referencedcomponentid, valueid);


-- terminology.snomedct_complexmaprefset_f definition

-- Drop table

DROP TABLE IF EXISTS terminology.snomedct_complexmaprefset_f CASCADE;

CREATE TABLE IF NOT EXISTS terminology.snomedct_complexmaprefset_f (
	id uuid NOT NULL,
	effectivetime bpchar(8) NOT NULL,
	active bpchar(1) NOT NULL,
	moduleid varchar(18) NOT NULL,
	refsetid varchar(18) NOT NULL,
	referencedcomponentid varchar(18) NOT NULL,
	mapgroup int2 NOT NULL,
	mappriority int2 NOT NULL,
	maprule text NULL,
	mapadvice text NULL,
	maptarget text NULL,
	correlationid varchar(18) NOT NULL
);
CREATE INDEX snomedct_complexmaprefset_referencedcomponentid_idx ON terminology.snomedct_complexmaprefset_f USING btree (referencedcomponentid);


-- terminology.snomedct_concept_f definition

-- Drop table

DROP TABLE IF EXISTS terminology.snomedct_concept_f CASCADE;

CREATE TABLE IF NOT EXISTS terminology.snomedct_concept_f (
	id varchar(18) NOT NULL,
	effectivetime bpchar(8) NOT NULL,
	active bpchar(1) NOT NULL,
	moduleid varchar(18) NOT NULL,
	definitionstatusid varchar(18) NOT NULL
);


-- terminology.snomedct_description_f definition

-- Drop table

 DROP TABLE IF EXISTS terminology.snomedct_description_f CASCADE;

CREATE TABLE IF NOT EXISTS terminology.snomedct_description_f (
	id varchar(18) NOT NULL,
	effectivetime bpchar(8) NOT NULL,
	active bpchar(1) NOT NULL,
	moduleid varchar(18) NOT NULL,
	conceptid varchar(18) NOT NULL,
	languagecode varchar(2) NOT NULL,
	typeid varchar(18) NOT NULL,
	term text NOT NULL,
	casesignificanceid varchar(18) NOT NULL
);
CREATE INDEX snomedct_description_conceptid_idx ON terminology.snomedct_description_f USING btree (conceptid);


-- terminology.snomedct_extendedmaprefset_f definition

-- Drop table

 DROP TABLE IF EXISTS terminology.snomedct_extendedmaprefset_f CASCADE;

CREATE TABLE IF NOT EXISTS terminology.snomedct_extendedmaprefset_f (
	id uuid NOT NULL,
	effectivetime bpchar(8) NOT NULL,
	active bpchar(1) NOT NULL,
	moduleid varchar(18) NOT NULL,
	refsetid varchar(18) NOT NULL,
	referencedcomponentid varchar(18) NOT NULL,
	mapgroup int2 NOT NULL,
	mappriority int2 NOT NULL,
	maprule text NULL,
	mapadvice text NULL,
	maptarget text NULL,
	correlationid varchar(18) NULL,
	mapcategoryid varchar(18) NULL
);
CREATE INDEX snomedct_extendedmaprefset_referencedcomponentid_idx ON terminology.snomedct_extendedmaprefset_f USING btree (referencedcomponentid);


-- terminology.snomedct_langrefset_f definition

-- Drop table

DROP TABLE IF EXISTS terminology.snomedct_langrefset_f CASCADE;

CREATE TABLE IF NOT EXISTS terminology.snomedct_langrefset_f (
	id uuid NOT NULL,
	effectivetime bpchar(8) NOT NULL,
	active bpchar(1) NOT NULL,
	moduleid varchar(18) NOT NULL,
	refsetid varchar(18) NOT NULL,
	referencedcomponentid varchar(18) NOT NULL,
	acceptabilityid varchar(18) NOT NULL
);
CREATE INDEX snomedct_langrefset_referencedcomponentid_idx ON terminology.snomedct_langrefset_f USING btree (referencedcomponentid);


-- terminology.snomedct_lookup_semantictag definition

-- Drop table

DROP TABLE IF EXISTS terminology.snomedct_lookup_semantictag CASCADE;

CREATE TABLE IF NOT EXISTS terminology.snomedct_lookup_semantictag (
	id varchar NOT NULL DEFAULT public.uuid_generate_v4(),
	"domain" varchar NOT NULL,
	tag varchar NOT NULL
);
CREATE INDEX snomedct_lkp_semantictag_domain_idx ON terminology.snomedct_lookup_semantictag USING btree (domain, tag);


-- terminology.snomedct_relationship_f definition

-- Drop table

DROP TABLE IF EXISTS terminology.snomedct_relationship_f CASCADE;

CREATE TABLE IF NOT EXISTS terminology.snomedct_relationship_f (
	id varchar(18) NOT NULL,
	effectivetime bpchar(8) NOT NULL,
	active bpchar(1) NOT NULL,
	moduleid varchar(18) NOT NULL,
	sourceid varchar(18) NOT NULL,
	destinationid varchar(18) NOT NULL,
	relationshipgroup varchar(18) NOT NULL,
	typeid varchar(18) NOT NULL,
	characteristictypeid varchar(18) NOT NULL,
	modifierid varchar(18) NOT NULL
);
CREATE INDEX snomedct_relationship_f_idx ON terminology.snomedct_relationship_f USING btree (sourceid, destinationid);


-- terminology.snomedct_simplemaprefset_f definition

-- Drop table

DROP TABLE IF EXISTS terminology.snomedct_simplemaprefset_f CASCADE;

CREATE TABLE IF NOT EXISTS terminology.snomedct_simplemaprefset_f (
	id uuid NOT NULL,
	effectivetime bpchar(8) NOT NULL,
	active bpchar(1) NOT NULL,
	moduleid varchar(18) NOT NULL,
	refsetid varchar(18) NOT NULL,
	referencedcomponentid varchar(18) NOT NULL,
	maptarget text NOT NULL
);


-- terminology.snomedct_simplerefset_f definition

-- Drop table

DROP TABLE IF EXISTS terminology.snomedct_simplerefset_f CASCADE;

CREATE TABLE IF NOT EXISTS terminology.snomedct_simplerefset_f (
	id varchar(50) NOT NULL,
	effectivetime bpchar(8) NOT NULL,
	active bpchar(1) NOT NULL,
	moduleid varchar(18) NOT NULL,
	refsetid varchar(18) NOT NULL,
	referencedcomponentid varchar(18) NOT NULL
);
CREATE INDEX snomedct_simplerefset_referencedcomponentid_idx ON terminology.snomedct_simplerefset_f USING btree (referencedcomponentid);


-- terminology.snomedct_stated_relationship_f definition

-- Drop table

DROP TABLE IF EXISTS terminology.snomedct_stated_relationship_f CASCADE;

CREATE TABLE IF NOT EXISTS terminology.snomedct_stated_relationship_f (
	id varchar(18) NOT NULL,
	effectivetime bpchar(8) NOT NULL,
	active bpchar(1) NOT NULL,
	moduleid varchar(18) NOT NULL,
	sourceid varchar(18) NOT NULL,
	destinationid varchar(18) NOT NULL,
	relationshipgroup varchar(18) NOT NULL,
	typeid varchar(18) NOT NULL,
	characteristictypeid varchar(18) NOT NULL,
	modifierid varchar(18) NOT NULL
);
CREATE INDEX snomedct_stated_relationship_f_idx ON terminology.snomedct_stated_relationship_f USING btree (sourceid, destinationid);


-- terminology.snomedct_textdefinition_f definition

-- Drop table

DROP TABLE IF EXISTS terminology.snomedct_textdefinition_f CASCADE;

CREATE TABLE IF NOT EXISTS terminology.snomedct_textdefinition_f (
	id varchar(18) NOT NULL,
	effectivetime bpchar(8) NOT NULL,
	active bpchar(1) NOT NULL,
	moduleid varchar(18) NOT NULL,
	conceptid varchar(18) NOT NULL,
	languagecode varchar(2) NOT NULL,
	typeid varchar(18) NOT NULL,
	term text NOT NULL,
	casesignificanceid varchar(18) NOT NULL
);
CREATE INDEX snomedct_textdefinition_conceptid_idx ON terminology.snomedct_textdefinition_f USING btree (conceptid);