$fileDir = Split-Path -Parent $MyInvocation.MyCommand.Path
cd $fileDir
java '-Dtalend.component.manager.m2.repository=%cd%/../lib' '-Xms256M' '-Xmx4096M' -cp '.;../lib/routines.jar;../lib/log4j-slf4j-impl-2.12.1.jar;../lib/log4j-api-2.12.1.jar;../lib/log4j-core-2.12.1.jar;../lib/log4j-1.2-api-2.12.1.jar;../lib/geronimo-stax-api_1.0_spec-1.0.1.jar;../lib/poi-ooxml-4.1.0-20190523141255_modified_talend.jar;../lib/commons-compress-1.19.jar;../lib/jaxen-1.1.6.jar;../lib/poi-scratchpad-4.1.0-20190523141255_modified_talend.jar;../lib/poi-ooxml-schemas-4.1.0-20190523141255_modified_talend.jar;../lib/postgresql-42.2.9.jar;../lib/slf4j-api-1.7.25.jar;../lib/dom4j-2.1.1.jar;../lib/poi-4.1.0-20190523141255_modified_talend.jar;../lib/xpathutil-1.0.0.jar;../lib/jakarta-oro-2.0.8.jar;../lib/commons-math3-3.6.1.jar;../lib/xmlbeans-3.1.0.jar;../lib/crypto-utils.jar;../lib/commons-collections4-4.1.jar;dmd_seed_atc_bnf_0_1.jar;' local_project.dmd_seed_atc_bnf_0_1.dmd_seed_atc_bnf  --context=Default $args