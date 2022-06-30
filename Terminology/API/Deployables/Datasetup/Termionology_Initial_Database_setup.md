# Terminology Initial Database setup

## Setup SNOMEDCT

 This will create required SNOMED CT tables, seed tables with required data and create the required views and materialized views.

1.  Open the folder 'snomedct_setup_init' under 'interneuron-synapse\Terminology\API\Deployables\Datasetup' folder

2. Open the batch file 'snomed_setup_init.bat' in notepad (or any text editor)

   - Modify the parameters appropriately

     ```
     --context_param snomed_int_file_path="<Path to downloaded SNOMED CT files Int version>/Full" 
     E.g. C:/Projects/Interneuron/TalendPlayground/uk_sct2cl_30.0.0_20200805000001/SnomedCT_InternationalRF2_PRODUCTION_20190731T120000Z/Full
     
     --context_param snomed_db_script_path="<Absolute path>/snomedct_setup_init/scripts" 
     --context_param snomed_db_host=<DB Server> 
     --context_param snomed_db_port=<Db port> 
     --context_param snomed_db_user=<Db User Id> 
     --context_param snomed_db_password=<Db User Password> 
     --context_param snomed_db_pwd_string=<Db User Password>  
     --context_param snomed_db_schema=terminology 
     --context_param snomed_db_name=<Db Name> 
     --context_param snomed_db_additionalparams= 
     --context_param snomed_int_release=<E.g. sct2_Concept_Full_INT_<"This Content">.txt> 
     --context_param snomed_uk_release=<E.g. sct2_Concept_Full_UK_<"This Content">.txt>
     --context_param snomed_uk_file_path="<Path to downloaded SNOMED CT files UK version>/Full
     
     E.g.
     C:/Projects/Interneuron/TalendPlayground/uk_sct2cl_30.0.0_20200805000001/SnomedCT_UKClinicalRF2_PRODUCTION_20200805T000001Z/Full
     
     --context_param snomed_db_psql_path=<Path to psql bin - Empty if set as environment variable>
     ```

     

3. Execute the batch job 'snomed_setup_init.bat'

   ```
   > cd <path>\interneuron-synapse\Terminology\API\Deployables\Datasetup\snomedct_setup_init
   > snomed_setup_init.bat
   ```

   

   ## Setup DMD

    This will create required DMD tables, seed tables with required data and create the required views and materialized views.

   1.  Open the folder 'dmd_setup_init' under 'interneuron-synapse\Terminology\API\Deployables\Datasetup' folder

   2. Open the batch file 'dmd_setup_init.bat' in notepad (or any text editor)

      - Modify the parameters appropriately

        ```
        :: --context_param dmd_db_psql_path=<Path to psql bin - Empty if set as environment variable>
        :: --context_param dmd_file_path=
        :: E.g. "C:/dmd_extract_tool/XMLToUnzipInHere/nhsbsa_dmd_8.2.0_20200817000001" 
        :: --context_param dmd_db_script_path="<Absolute path>/dmd_setup_init/scripts" 
        :: --context_param dmd_db_host=<Db Server>   
        :: --context_param dmd_db_port=<Db Server port>   
        :: --context_param dmd_db_user=<Db User Id>   
        :: --context_param dmd_db_password=<Db User Password>   
        :: --context_param dmd_db_pwd_string=<Db User Password>   
        :: --context_param dmd_db_schema=terminology 
        :: --context_param dmd_db_name=<Db Name>  
        ```

        

   3. Execute the batch job 'dmd_setup_init.bat'

      ```
      > cd <path>\interneuron-synapse\Terminology\API\Deployables\Datasetup\dmd_setup_init
      > dmd_setup_init.bat
      ```



## Setup Formulary

 This will create required Formulary tables, seed tables with required data and create the required views and materialized views.

1.  Open the folder 'formulary_setup_init' under 'interneuron-synapse\Terminology\API\Deployables\Datasetup' folder

2. Open the batch file 'formulary_setup_init.bat' in notepad (or any text editor)

   - Modify the parameters appropriately

     ```
     --context_param formulary_db_script_path="<Absolute path>/formulary_setup_init/scripts"  
     --context_param formulary_db_host=<Db Server>  
     --context_param formulary_db_port=<Db Server Port>  
     --context_param formulary_db_user=<Db User Id>  
     --context_param formulary_db_password=<Db User Pwd>  
     --context_param formulary_db_pwd_string=<Db User Pwd>  
     --context_param formulary_db_schema=local_formulary 
     --context_param formulary_db_name=<Db Name>  
     --context_param formulary_db_additionalparams= 
     --context_param formulary_db_psql_path=<Path to psql bin - Empty if set as environment variable>
     ```

     

3. Execute the batch job 'formulary_setup_init.bat'

   ```
   > cd <path>\interneuron-synapse\Terminology\API\Deployables\Datasetup\formulary_setup_init
   > formulary_setup_init.bat
   ```

   

### Update ATC Code

This job will update the required formulary table with the ATC Code and its description

1. Open the folder 'formulary_setup_init' under 'interneuron-synapse\Terminology\API\Deployables\Datasetup' folder

2. Open the batch file 'formulary_atc_update.bat' in notepad (or any text editor)

   - Modify the parameters appropriately

     ```
     --context_param formulary_db_script_path="<Absolute path>/formulary_setup_init/scripts"  
     --context_param formulary_db_host=<Db Server>  
     --context_param formulary_db_port=<Db Server Port>  
     --context_param formulary_db_user=<Db User Id>  
     --context_param formulary_db_password=<Db User Pwd>  
     --context_param formulary_db_pwd_string=<Db User Pwd>  
     --context_param formulary_db_schema=local_formulary 
     --context_param formulary_db_name=<Db Name>  
     --context_param formulary_db_additionalparams= 
     --context_param formulary_db_psql_path=<Path to psql bin - Empty if set as environment variable>
     --context_param formulary_file_path=<path>/interneuron-synapse/Terminology/API/Deployables/Datasetup/formulary_setup_init/files
     Note: Copy the ATC files from TRUD to this folder
     ```

     

3. Execute the batch job 'formulary_setup_init.bat'

   ```
   > cd <path>\interneuron-synapse\Terminology\API\Deployables\Datasetup\formulary_setup_init
   > formulary_atc_update.bat
   ```

   

