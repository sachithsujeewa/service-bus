FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ServiceBusMvp.sln ./
COPY src/ServiceBus.Contracts/ServiceBus.Contracts.csproj src/ServiceBus.Contracts/
COPY src/ServiceBus.Messaging/ServiceBus.Messaging.csproj src/ServiceBus.Messaging/
COPY src/ServiceBus.Dispatcher/ServiceBus.Dispatcher.csproj src/ServiceBus.Dispatcher/
RUN dotnet restore src/ServiceBus.Dispatcher/ServiceBus.Dispatcher.csproj
COPY src/ServiceBus.Contracts/ src/ServiceBus.Contracts/
COPY src/ServiceBus.Messaging/ src/ServiceBus.Messaging/
COPY src/ServiceBus.Dispatcher/ src/ServiceBus.Dispatcher/
RUN dotnet publish src/ServiceBus.Dispatcher/ServiceBus.Dispatcher.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ServiceBus.Dispatcher.dll"]
