
CREATE SCHEMA IF NOT EXISTS local_formulary;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";



SET SCHEMA 'local_formulary';

--=================================ABOVE STATEMENTS SHOULD NOT BE DELETED ==========================================================================

truncate table local_formulary.lookup_common CASCADE;
INSERT INTO local_formulary.lookup_common
( "_contextkey", "_createdtimestamp", "_createddate", "_createdsource", "_createdmessageid", "_createdby", "_recordstatus", "_timezonename", "_timezoneoffset", "_tenant", cd, "desc","type")
values 

--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Medication','MedicationType'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', 'Diluent','MedicationType'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '003', 'Medicinal gas','MedicationType'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '004', 'Blood product','MedicationType'),
--===========
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 1','Multi-ingredient drugs','Rules')
--,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 2','No VTM','Rules')
--,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 3','Never valid as a VMP','Rules')
--,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 4','‘Co-‘name drugs','Rules')
--,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 5a',' VMP not recommended (non-equivalence)','Rules')
--,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 5b','VMP not recommended (patient training)','Rules')
--,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 5c','VMP not recommended (no product specification)','Rules')
--,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 6','Controlled Drugs','Rules')
--,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 7',' Injection or infusion','Rules')
--,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 8',' Inhaled dose forms','Rules')
--,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 9','Differing release characteristics','Rules')
--,('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Rule 10','Topical forms','Rules'),

--====
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Draft','RecordStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', 'Approved','RecordStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '003', 'Active','RecordStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '004', 'Archived','RecordStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '005', 'Inactive','RecordStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '006', 'Deleted','RecordStatus'),

--===================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Formulary','FormularyStatus'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', 'Non-Formulary','FormularyStatus'),
--==================
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '000', 'Not Orderable','OrderableStatus'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Orderable','OrderableStatus'),
--==============================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Blood Sugar','TitrationType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', 'INR','TitrationType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '003', 'MAP','TitrationType'),
--==============================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', '0.5','RoundingFactor'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', '1','RoundingFactor'),
--==============================
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '9906301000001103', 'Modified Release','Modifier'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '9906701000001104', 'Gastro-Resistant','Modifier'),
--==============================
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '1', 'Discrete','DoseForm'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '2', 'Continuous','DoseForm'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '3', 'Not applicable','DoseForm'),
--==============================
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '1', 'Based on Ingredient Substance','PharamceuticalStrength'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '2', 'Based on Base Substance','PharamceuticalStrength'),
--==============================
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '9906401000001106', 'Modified release 12 hour','ModifiedRelease'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '9906901000001102', 'Modified release 24 hour','ModifiedRelease'),
--===================================
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Additonal','RouteFieldType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', 'UnLicensed','RouteFieldType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '003', 'Normal','RouteFieldType'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '004', 'Discretionary','RouteFieldType'),

--===================================
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '001', 'Simple','OrderFormType'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '002', 'Complex','OrderFormType'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', '003', 'Descriptive','OrderFormType'),
--====================================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'VTM', 'VTM','ProductType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'VMP', 'VMP','ProductType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'AMP', 'AMP','ProductType'),
--===========================================
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'BNF', 'BNF','ClassificationCodeType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'ATC', 'ATC','ClassificationCodeType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'FDB', 'FDB','ClassificationCodeType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'CustomGroup', 'Custom Group','ClassificationCodeType'),

('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'DMD', 'DMD','IdentificationCodeType'),
('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Custom', 'Custom','IdentificationCodeType')

--=============================================
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Generic', 'Generic','DrugClass'),
--('', timezone('UTC'::text, now()), now(), '', '', '', 1, '', 0, '', 'Branded', 'Brand','DrugClass')
--=============================================
;
