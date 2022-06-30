FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY buildfolder/* .
EXPOSE 80
ENTRYPOINT ["dotnet", "SynapseDynamicAPI.dll"]