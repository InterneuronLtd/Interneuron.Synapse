<h1>Synapse Identity Server</h1>   

> Synapse implements IdentityServer4 to authenticate and authorize all apps and users requesting resources in the Synapse Platform.
>
> Synapse Identity Server supports local logins and ADFS via ws-Federation and AAD vis OIDC as external identity providers




## Prerequisites

**Development**

```
1. Visual Studio Community / Visual Studio Code
2. ASP.NET Core SDK 2.0.9 or higher
3. Postgres 11
```

**Deployment** [windows]

```
1. IIS 7 or higher
2. Postgres 11
```



## Configure and Install

#### Install the Identity Database

* Ensure Postgres 11.x is installed.
* Open windows PowerShell or windows command prompt
* Install the packaged Synapse Identity Database - *RestoreSISSchema_v2.0.sql* - using the PostgresSql shell psql

```sh
psql --host <Hostname> --port <Port> --username <username> --password --file <path to RestoreSISSchema_v2.0.sql>
```

  *example:*

```sh
cd "c:\Program Files\PostgreSQL\11\bin"
psql --host localhost --port 5432 --user postgres --password -file c:\RestoreSISSchema_v2.0.sql

```


#### Application Configurations

*appsettings.json*

**1. Log Level**

​	Controls the amount of information that is written into system logs.

```json
"LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
```

**2. Connection String**

PostgreSql Connection string to the Identity Database. Update this to point to your Postgres instance.

```json
"ConnectionString": "Server=@@SERVER_NAME@@;User Id=XXXXXXXXX;Password=XXXXXX;Database=SynapseIdentity;Port=XXXX"

```

**3. LogFilePath**

This specifies the file path where the identity server operational logs are written. Defaults to ".\\OperationalLogs\\SISLog.txt". A log file for each day is created separately. E.G. : SISLog20191210.txt.

```json
"LogFilePath": ".\\OperationalLogs\\SISLog.txt"
```

**4. LogLevel**

 Controls the amount of information that is written into the operational logs

```json
"LogLevel": "verbose"
```

**5. ADFS ws-Federation Settings**

 These settings configure the ADFS identity provider. [Please refer Microsoft Documentation for more information on authenticating with ws-Federation](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/ws-federation?view=aspnetcore-3.1)

​	**ADFSDisplayName**

​	Configures the display name for ADFS identity provider on the Identity Sever Login Screen

```json
    "ADFSDisplayName": "ADFS"
```

​	**WindowsAccountNameClaimtype**

​	Configures the claim type that has the "Subject" and maps to the OIDC **Sub** claim. Change this according 	to how your relying party is configured to pass the Subject claim. This is used as a primary key to identity 	subjects in the system.

```json
"WindowsAccountNameClaimtype": "http://schemas.microsoft.com/2012/12/certificatecontext/field/subject"
```

​	**ADFSMetaAddress**

​	Configures the address to get the ws-Federation metadata

```json
"ADFSMetaAddress": "https://YOUR_ADFS_Server_DOMAIN/FederationMetadata/2007-06/FederationMetadata.xml",

```

​	**ADFSWtrealm**

​	Configures the identifier of the relying party for the app.

```json
"ADFSWtrealm": "https://yourdomain/yourapp"
```

**6. Azure AD Settings**

Configures Azure AD Identity provider. [Please refer Microsoft Documentation for more information on authenticating with Azure AD via OIDC](https://docs.microsoft.com/en-us/azure/active-directory/develop/v1-protocols-openid-connect-code)

​	**AzureClientId and AzureTenantId **

​	Configures the Azure app identifiers.

​	Tenant Id is the ID of the AAD in which you created an entry for Synapse application. Client Id is the Id of 	Synapse application on AAD.

```json
"AzureClientId": "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXX",
"AzureTenantId": "XXXXXXX-XXXX-XXXX-XXXX-fXXXXXXXXX",
```

​	**AADIPUIdClaimType**

​    Configures the Subject Identifier in the AAD Claims, defaults to the UPN claim

```json
"AADIPUIdClaimType": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn"
```

   **AADDisplayName**

   Configures the display name for Azure AD identity provider on the Identity Sever Login Screen.

```json
"AADDisplayName": "Azure Active Directory"
```

**6. Other Settings**

​	**RemoteAuthTimeoutSecs**

​	Configures the time limit in seconds to complete an external authentication.

```json
"RemoteAuthTimeoutSecs": 172800
```

   **SigningCredentialsTP**

​	Configures the Thumbprint of the Certificate used to sign the tokens by Identity Server. If  the value is  empty, system will use a temp developer certificate packaged with the  application.

```json
"SigningCredentialsTP": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"

```



#### Publish and Install

​	Use visual studio to publish the application directly to a web service or,

1. Publish to a folder profile
2. Create a New Website on IIS (prefer not to host this in a sub directory) and add bindings to https on port 5000. [if you have already created Interneuron sites using Interneuron-Sites.xml in Sample/IISSettings folder, you can skip this step and use InterneuronSIS site instead]
3. Copy the published code into the websites physical path.
4. Open command prompt and run IISRESET



If you are using a certificate to sign tokens, please ensure that the certificate is in personal store of the computer and the application pool running the website on IIS has access to the private keys of this certificate.

1. open MMC and add certificate snap in--> computer account -->local computer then click ok.
2. Expand personal and click on Certificates
3. Right click on the signing certificate --> click All Tasks -- click "manager private keys"
4. Click on Add  
5. Type IIS AppPool\AppPoolName  [<AppPoolName> is name of the app pool that is running the SynapseIdentityServer Website on IIS] and click ok
6. Give at least Read permissions and click ok.



## Author

* GitHub: [Interneuron CIC](https://github.com/InterneuronCIC)



---

## 📝 License

/Interneuron Synapse
//Copyright(C) 2023  Interneuron Holdings Ltd

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program. If not, see <http://www.gnu.org/licenses/>.
