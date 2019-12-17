# Synapse Studio

## Introduction

This Read Me is provided to give guidance on the installation and configuration of Synapse Studio.


## Prerequisites

**Other Synapse Project Dependencies**

```
1. Synapse Dynamic API
2. Synapse Identity server
```

**Development**

```
1. Visual Studio Community / Visual Studio Code
2. ASP.NET Core SDK 2.1 or higher
3. Postgres 11
```

**Deployment** [windows]

```
1. IIS 7 or higher
2. Postgres 11
```

## Installation & Configuration

This section guides you through the deployment and configuration of Synapse Studio.

### Synapse Studio

1. Restore the Synapse database from [/SynapseSchema/RestoreSynapseSchema_v2.0.sql](/SynapseSchema/RestoreSynapseSchema_v2.0.sql) to your PostgreSQL Instance.

```sh
psql --host <Hostname> --port <Port> --username <username> --password --file <path to RestoreSynapseSchema_v2.0.sql>
```

  *example:*

```sh
cd "c:\Program Files\PostgreSQL\11\bin"
psql --host localhost --port 5432 --user postgres --password -file c:\RestoreSynapseSchema_v2.0.sql

```

4. Copy the package folder from [/Interneuron.Synapse](/Interneuron.Synapse) to  [SynapseStudio](/Interneuron.Synapse/SynapseStudio)  for **Reference dependency**
5. Update the connection string in [SynapseStudio/SynapseStudio/Web.config](/SynapseStudio/SynapseStudio/Web.config)

```xml
<connectionStrings>
     <add name="PGSQLConnection" connectionString="Server=@@SERVER_NAME@@;Port=5432;User Id=@@USER_NAME@@;Password=@@PASSWORD@@;Database=synapse;" />
    <add name="PGSQLConnectionPostgresDB" connectionString="Server=synapse-data-store.postgres.database.azure.com;User Id=XXXX@synapse-data-store;Password=XXXX;Database=postgres;Port=5432" />
       <add name="PGSQLConnectionSIS" connectionString="Server=@@SERVER_NAME@@;Port=5432;User Id=@@USER_NAME@@;Password=@@PASSWORD@@;Database=SynapseIdentity;" />
  </connectionStrings>
```

6. Update the Global Settings in [SynapseStudio/GlobalSettings.js](/SynapseStudio/SynGlobalSettings.js)

   ​	a. update GlobalServiceURL with Synapse Dynamic API URL.

   ​	b. Update authority with  Synapse Identity server URL.

```js
var GlobalServiceURL = 'DYNAMIC_API_URL';
var config = {
    //configure authority and client    
    authority:"IDENTITY_SERVER_URL"
    }
```

## Publish and Install

```
Use visual studio to publish the application directly to a web service or,
```

1. Publish to a folder profile

2. Locate and copy the Interneuron-AppPools.xml and Interneuron-Sites.xml in Sample/IISSettings folder

3. Open command prompt in administrator mode and execute the below commands

4. Copy the published code into the websites physical path.

5. Open command prompt and run IISRESET


```
   %windir%\system32\inetsrv\appcmd add apppool /in < "path to Interneuron-AppPools.xml"

   %windir%\system32\inetsrv\appcmd add site /in < "path to  Interneuron-Sites.xml"
```

6. .Browse to http://your-chosen-url/SynapseStudio/RegisterAsSuperUser.aspx and provide credentials to register your super user account.
7. Use the new super register to logon.

## Author

* GitHub: [Interneuron CIC](https://github.com/InterneuronCIC)

## License

Interneuron Synapse

Copyright(C) 2019  Interneuron CIC

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.

If not, see <http://www.gnu.org/licenses/>.
