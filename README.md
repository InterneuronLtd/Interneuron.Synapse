# Interneuron Synapse

## Introduction

This Read Me is provided to give guidance on the installation and configuration of Interneuron Synapse v0.1.

## Prerequisities

The following prerequisites are required for Interneuron Synapse. The installation and configuration of these applications is not covered by this read me.

- PostgreSQL v10.x, v11.x
- .NET Core 2.x
- IIS / Kestrel / Azure
- Visual Studio Community 2017 required for publishing Synapse Studio.

## Installation & Configuration

This section guides you through the deployment and configuration of your development environment for the Synapse DynamicAPI and Synapse Studio components.

### Synapse Dynamic API
1. Restore the Synapse database from [/SynapseDatabase/RestoreSynapseSchema_v1.0.sql](/SynapseDatabase/RestoreSynapseSchema_v1.0.sql) to your PostgreSQL Instance.

2. Create a PostgreSQL user for Synapse with full access to the Synapse database.

3. Update the connection string in: [/SynapseDynamicAPI/SynapseDynamicAPI/appsettings.json](/SynapseDynamicAPI/SynapseDynamicAPI/appsettings.json)
```json
  "ConnectionStrings": {
    "SynapseDataStore": "Server=@@SERVERNAME@@;User Id=@@USERNAME@@;Password=@@PASSWORD@@;Database=synapse;",
    "SynapseIdentityStore": "Server=@@SERVERNAME@@;User Id=@@SERVERNAME@@;Password=@@PASSWORD@@;Database=synapseidentity;"
  }
```

### Synapse Studio

4. Using Visual Studio, modify the DynamicAPI publish profile for your chosen deployment setup (IIS etc) and Publish the project.

5. Update the connection string in [SynapseStudio/SynapseStudio/Web.config](/SynapseStudio/SynapseStudio/Web.config)
```xml
<connectionStrings>
  <add name="PGSQLConnection" connectionString="Server=@@SERVERNAME@@;User Id=@@USERNAME@@;Password=@@PASSWORD@@;Database=synapse;Port=5432"/>
</connectionStrings>
```

6. Modify the Synapse publish profile for your chosen deployment setup (IIS etc) and Publish.

7. Browse to http://your-chosen-url/SynapseStudio/RegisterAsSuperUser.aspx and provide credentials to register your super user account.

8. Use the new super register to logon.

You have now successfully configured Synapse v0.1!

## Issues & Feedback

We are currently working on our documentation, but if you have any feedback or issues please do drop us a line at open@interneuron.org.
