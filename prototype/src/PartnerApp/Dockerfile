FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/ServiceBus.Contracts/ServiceBus.Contracts.csproj src/ServiceBus.Contracts/
COPY src/PartnerApp/PartnerApp.csproj src/PartnerApp/
RUN dotnet restore src/PartnerApp/PartnerApp.csproj
COPY src/ServiceBus.Contracts/ src/ServiceBus.Contracts/
COPY src/PartnerApp/ src/PartnerApp/
RUN dotnet publish src/PartnerApp/PartnerApp.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:5102
EXPOSE 5102
ENTRYPOINT ["dotnet", "PartnerApp.dll"]
