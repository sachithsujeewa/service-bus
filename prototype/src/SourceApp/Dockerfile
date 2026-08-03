FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/SourceApp/SourceApp.csproj src/SourceApp/
RUN dotnet restore src/SourceApp/SourceApp.csproj
COPY src/SourceApp/ src/SourceApp/
RUN dotnet publish src/SourceApp/SourceApp.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:5101
EXPOSE 5101
ENTRYPOINT ["dotnet", "SourceApp.dll"]
