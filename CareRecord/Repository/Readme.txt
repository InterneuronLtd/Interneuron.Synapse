Uncomment below in Repository.csproj file
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="3.1.0">
      <!--<PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>-->
    </PackageReference>

Uncomment all files in Scafflod Util folder un Repository


To Create:
> <Solution FullPath>\Interneuron.Synapse\CareRecord\Repository>dotnet ef dbcontext scaffold "Server=localhost;Port=54320;User Id=postgres;Password=password;Database=synapse;" -o ..\Model\DomainModels --context-dir DBModelsContext -c SynapseDBContext Npgsql.EntityFrameworkCore.PostgreSQL

Specific Schemas
> <Solution FullPath>\Interneuron.Synapse\CareRecord\Repository>dotnet ef dbcontext scaffold "Server=localhost;Port=54320;User Id=postgres;Password=password;Database=synapse;" -o ..\Model\Temp_DomainModels --context-dir DBModelsContext --schema entitystore --schema entitystorematerialised --schema baseview  -c SynapseDBContext_New Npgsql.EntityFrameworkCore.PostgreSQL

Step 1:
Create New partial Class 'SynapseDBContext.DBSets.cs
Step 2:
Move All DBSet Properties to the new class
Step 3:
Create New partial Class 'SynapseDBContext.ModelConfigs.cs
Step 4:
Move the complete method:  OnModelCreating
Step 5:
In the SynapseDBContext.cs file: Replace the method body of 'OnConfiguring' with below code
if (!optionsBuilder.IsConfigured)
{
    var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

    var config = builder.Build();

    optionsBuilder.UseNpgsql(config.GetConnectionString("SynapseDBConnection"));
}



To Update:
Step 1:
Remove or take backup of 'DomainModels' under 'Model' folder

Step 2:
> <Solution FullPath>\Interneuron.Synapse\CareRecord\Repository>dotnet ef dbcontext scaffold "Server=localhost;Port=54320;User Id=postgres;Password=password;Database=synapse;" -o ..\Model\Temp_DomainModels --context-dir DBModelsContext -c SynapseDBContext_New Npgsql.EntityFrameworkCore.PostgreSQL --force

Specific Schemas
> <Solution FullPath>\Interneuron.Synapse\CareRecord\Repository>dotnet ef dbcontext scaffold "Server=localhost;Port=54320;User Id=postgres;Password=password;Database=synapse;" -o ..\Model\Temp_DomainModels --context-dir DBModelsContext --schema entitystore --schema entitystorematerialised --schema baseview  -c SynapseDBContext_New Npgsql.EntityFrameworkCore.PostgreSQL --force

Replaces all the Domain Models in DomainModels folder with that in Temp_DomainModels folder

Step 3:
Replace all the DBSets in 'SynapseDBContext.DBSets.cs' from DBSets in new 'SynapseDBContext_New.cs' Class 
Step 4:
Replace method 'OnModelCreating' in 'SynapseDBContext.ModelConfigs.cs' from method 'OnModelCreating' in new 'SynapseDBContext_New.cs' Class 
Step 5:
Delete new 'SynapseDBContext_New.cs' file



https://www.learnentityframeworkcore.com/walkthroughs/existing-database

http://www.npgsql.org/efcore/index.html

https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx

https://www.learnentityframeworkcore.com/raw-sql

Customize:
https://github.com/dotnet/efcore/blob/master/src/EFCore.Design/Scaffolding/Internal/CSharpEntityTypeGenerator.cs

https://entityframeworkcore.com/knowledge-base/40728223/entity-framework-core-customize-scaffolding

https://stackoverflow.com/questions/40728223/entity-framework-core-customize-scaffolding

https://stackoverflow.com/questions/42455279/how-to-get-column-name-and-corresponding-database-type-from-dbcontext-in-entity

https://github.com/TrackableEntities/EntityFrameworkCore.Scaffolding.Handlebars

Issues:
EF Core Issue:
"Could not execute because the specified command or file was not found."
Solutions:
https://stackoverflow.com/questions/57066856/dotnet-ef-not-found-in-net-core-3