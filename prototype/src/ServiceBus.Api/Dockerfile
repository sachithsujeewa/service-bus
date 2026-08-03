FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ServiceBusMvp.sln ./
COPY src/ServiceBus.Contracts/ServiceBus.Contracts.csproj src/ServiceBus.Contracts/
COPY src/ServiceBus.Messaging/ServiceBus.Messaging.csproj src/ServiceBus.Messaging/
COPY src/ServiceBus.Api/ServiceBus.Api.csproj src/ServiceBus.Api/
RUN dotnet restore src/ServiceBus.Api/ServiceBus.Api.csproj
COPY src/ServiceBus.Contracts/ src/ServiceBus.Contracts/
COPY src/ServiceBus.Messaging/ src/ServiceBus.Messaging/
COPY src/ServiceBus.Api/ src/ServiceBus.Api/
RUN dotnet publish src/ServiceBus.Api/ServiceBus.Api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "ServiceBus.Api.dll"]
