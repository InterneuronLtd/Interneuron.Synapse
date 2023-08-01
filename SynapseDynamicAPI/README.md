# Synapse Dynamic API

## Introduction

This Read Me is provided to give guidance on the installation and configuration of Synapse Dynamic API.

## Prerequisites

Following prerequisites are required for Synapse Dynamic API.

- PostgreSQL 11.x +
- .NET Core 2.x +
- IIS 7 +/ Kestrel / Azure required for deployment of Synapse Dynamic API
- [Configure IIS](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-3.1#iis-configuration)
- Visual Studio Community Edition required for building and publishing Synapse Dynamic API.

## Other Synapse Project Dependency

Following Synapse Project have to be installed and configured first before installing and configuring Synapse Dynamic API application.

- Synapse Identity Server

## Installation and Configuration

This section guides you through the configuration and deployment of Synapse Dynamic API.

### Synapse Dynamic API

1. Restore the Synapse database from [/SynapseDatabase/RestoreSynapseSchema_v2.0.sql](/SynapseDatabase/RestoreSynapseSchema_v2.0.sql) to your PostgreSQL Instance using the PostgreSQL commands.

```shell
psql --host <Hostname> --port <Port> --username <username> --password --file <path to RestoreSynapseSchema_v2.0.sql>
```

*example:*

```sh
cd "c:\Program Files\PostgreSQL\11\bin"

psql --host localhost --port 5432 --user postgres --password -file c:\RestoreSynapseSchema_v2.0.sql
```

2. Download the Synapse Dynamic API source code from repos.
3. Open the source code folder in Visual Studio Community Edition.
4. Go to `appsettings.json` in `SynapseDynamicAPI\SynapseDynamicAPI` folder and update the connection string in: [/SynapseDynamicAPI/SynapseDynamicAPI/appsettings.json](/SynapseDynamicAPI/SynapseDynamicAPI/appsettings.json)

```json
  "ConnectionStrings": {
      "SynapseDataStore": "Server=@@SERVERNAME@@;Port=@@Port@@;User Id=@@USERNAME@@;Password=@@PASSWORD@@;Database=synapse;",
      "SynapseIdentityStore": "Server=@@SERVERNAME@@;Port=@@Port@@;User Id=@@SERVERNAME@@;Password=@@PASSWORD@@;Database=SynapseIdentity;"
    }
```

5. In the same file make changes in the `Settings` section as well.

   ```json
   "Settings": {
     "AuthorizationAuthority": "https://YOUR_SYNAPSE_IDENTITY_SERVER_URL/",
     "AuthorizationAudience": "dynamicapi",
     "WriteAccessAPIScope": "dynamicapi.write",
     "ReadAccessAPIScope": "dynamicapi.read",
     "SynapseRolesClaimType": "SynapseRoles",
     "DynamicAPIWriteAccessRole": "DynamicApiWriters",
     "DynamicAPIReadAccessRole": "DynamicApiReaders",
     "TokenUserIdClaimType": "IPUId",
     "IgnoreIdentitySeverSSLErrors": "true",
     "ShowIdentitySeverPIIinLogs": "true",
     "MRN_ID_TYPE": "MRN",
     "EMPI_ID_TYPE": "NHS"
   }
   ```

   - Replace `YOUR_SYNAPSE_IDENTITY_SERVER_URL` with the URL of your Synapse Identity Server URL.
   - If your source system identifies patient by any internal number such as hospital number, MRN, etc. and you have any acronym for such identification numbering system then you can set it in the `appsettings.json` file by setting `MRN_ID_TYPE` variable as shown in the above code.
   - If your source system identifies patient by any national number as well such as NHS, etc and you have any acronym for such identification numbering system then you can set it in the `appsettings.json` file by setting `EMPI_ID_TYPE` variable as shown in the above code.

6. Use Visual Studio Community Edition to build and publish Synapse Dynamic API

* If you have not already created Interneuron sites in IIS, kindly follow the below procedure to create the sites

  a. Locate and copy the Interneuron-AppPools.xml and Interneuron-Sites.xml in Sample/IISSettings folder.

  b. Open command prompt in administrator mode and execute the below commands.

```shell
%windir%\system32\inetsrv\appcmd add apppool /in < "path to Interneuron-AppPools.xml"

%windir%\system32\inetsrv\appcmd add site /in < "path to Interneuron-Sites.xml"
```

* Publish to a folder profile.
* Copy the published code into the websites physical path.
* Open command prompt and run IISRESET


## Issues and Feedback

We are currently working on our documentation, but if you have any feedback or issues please do drop us a line at open@interneuron.org.

## Author

* GitHub: [Interneuron CIC](https://github.com/InterneuronCIC)

## License

Interneuron Synapse
Copyright(C) 2023  Interneuron Holdings Ltd

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.
