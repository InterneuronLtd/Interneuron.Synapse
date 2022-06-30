@echo off 
:: Parameter Details:

:: --context_param dmd_db_psql_path=<Path to psql bin - Empty if set as environment variable>
:: --context_param dmd_file_path=
:: E.g. "C:/dmd_extract_tool/XMLToUnzipInHere/nhsbsa_dmd_8.2.0_20200817000001" 
:: --context_param dmd_db_script_path="<Absolute path>/dmd_setup_init/scripts" 
:: --context_param dmd_db_host=localhost 
:: --context_param dmd_db_port=5432 
:: --context_param dmd_db_user=admin 
:: --context_param dmd_db_password=admin 
:: --context_param dmd_db_pwd_string=admin 
:: --context_param dmd_db_schema=terminology 
:: --context_param dmd_db_name=mmc


echo "Started creating required tables"
echo "%~dp0"
cd "%~dp0/dmd_db_create/dmd_db_create"
call "dmd_db_create_run.bat" --context_param dmd_db_additionalparams=  --context_param dmd_db_host=localhost --context_param dmd_db_name=mmc_demo --context_param dmd_db_password=admin@010 --context_param dmd_db_port=5432 --context_param dmd_db_psql_path=  --context_param dmd_db_pwd_string=admin@010 --context_param dmd_db_schema=terminology --context_param dmd_db_script_path="C:/Projects/Interneuron/POCs/Apps/interneuron-synapse/Terminology/API/Deployables/Datasetup/dmd_setup_init/scripts" --context_param dmd_db_user=admin --context_param dmd_file_path="C:/MMC_Deployment/Deployment_Demo_07122020/DMDFiles"

if %ERRORLEVEL% GEQ 1 (
echo ERROR
EXIT /B 1
)

echo "Finished creating required tables"

echo "Started importing data to tables"
echo "%~dp0"
cd "%~dp0/dmd_seed_data_xml/dmd_seed_data_xml_1"
call "dmd_seed_data_xml_1_run.bat" --context_param dmd_db_additionalparams=  --context_param dmd_db_host=localhost --context_param dmd_db_name=mmc_demo --context_param dmd_db_password=admin@010 --context_param dmd_db_port=5432 --context_param dmd_db_psql_path=  --context_param dmd_db_pwd_string=admin@010 --context_param dmd_db_schema=terminology --context_param dmd_db_script_path="C:/Projects/Interneuron/POCs/Apps/interneuron-synapse/Terminology/API/Deployables/Datasetup/dmd_setup_init/scripts" --context_param dmd_db_user=admin --context_param dmd_file_path="C:/MMC_Deployment/Deployment_Demo_07122020/DMDFiles"

if %ERRORLEVEL% GEQ 1 (
echo ERROR
EXIT /B 1
)

echo "Finished importing data to tables"

echo "Started importing ATC and BNF data to tables"
echo "%~dp0"
cd "%~dp0/dmd_seed_atc_bnf/dmd_seed_atc_bnf"
call "dmd_seed_atc_bnf_run.bat" --context_param dmd_db_additionalparams=  --context_param dmd_db_host=localhost --context_param dmd_db_name=mmc_demo --context_param dmd_db_password=admin@010 --context_param dmd_db_port=5432 --context_param dmd_db_psql_path=  --context_param dmd_db_pwd_string=admin@010 --context_param dmd_db_schema=terminology --context_param dmd_db_script_path="C:/Projects/Interneuron/POCs/Apps/interneuron-synapse/Terminology/API/Deployables/Datasetup/dmd_setup_init/scripts" --context_param dmd_db_user=admin --context_param dmd_file_path="C:/MMC_Deployment/Deployment_Demo_07122020/DMDFiles"

if %ERRORLEVEL% GEQ 1 (
echo ERROR
EXIT /B 1
)

echo "Finished importing ATC and BNF data to tables"



echo "Started creating views and UDFs"
echo "%~dp0"
cd "%~dp0/dmd_db_views_udfs/dmd_db_views_udfs"
call "dmd_db_views_udfs_run.bat" --context_param dmd_db_additionalparams=  --context_param dmd_db_host=localhost --context_param dmd_db_name=mmc_demo --context_param dmd_db_password=admin@010 --context_param dmd_db_port=5432 --context_param dmd_db_psql_path=  --context_param dmd_db_pwd_string=admin@010 --context_param dmd_db_schema=terminology --context_param dmd_db_script_path="C:/Projects/Interneuron/POCs/Apps/interneuron-synapse/Terminology/API/Deployables/Datasetup/dmd_setup_init/scripts" --context_param dmd_db_user=admin --context_param dmd_file_path="C:/MMC_Deployment/Deployment_Demo_07122020/DMDFiles"

if %ERRORLEVEL% GEQ 1 (
echo ERROR
EXIT /B 1
)

echo "Finished creating views and UDFs"