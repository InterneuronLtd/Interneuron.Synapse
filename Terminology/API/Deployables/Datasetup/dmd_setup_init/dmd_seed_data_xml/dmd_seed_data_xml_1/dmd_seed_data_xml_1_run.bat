%~d0
cd %~dp0
java -Dtalend.component.manager.m2.repository="%cd%/../lib" -Xms1024M -Xmx8196M -cp .;../lib/routines.jar;../lib/log4j-slf4j-impl-2.12.1.jar;../lib/log4j-api-2.12.1.jar;../lib/log4j-core-2.12.1.jar;../lib/xpathutil-1.0.0.jar;../lib/jakarta-oro-2.0.8.jar;../lib/jaxen-1.1.6.jar;../lib/postgresql-42.2.9.jar;../lib/talendcsv.jar;../lib/crypto-utils.jar;../lib/slf4j-api-1.7.25.jar;../lib/dom4j-2.1.1.jar;dmd_seed_data_xml_1_0_1.jar; local_project.dmd_seed_data_xml_1_0_1.dmd_seed_data_xml_1  --context=Default %*