FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["AsyncImage-Fetcher-Service.sln", "./"]

COPY ["Src/Adapters/Api/Digester.Adapters.Api.csproj", "Src/Adapters/Api/"]
COPY ["Src/Adapters/Broker/Digester.Adapters.Broker.csproj", "Src/Adapters/Broker/"]
COPY ["Src/Drivers/Data/Digester.Drivers.Data.csproj", "Src/Drivers/Data/"]
COPY ["Src/Drivers/Utilities/Digester.Drivers.Utilities.csproj", "Src/Drivers/Utilities/"]
COPY ["Src/Logic/Images/Digester.Logic.Images.csproj", "Src/Logic/Images/"]
COPY ["Src/Logic/Utilities/Digester.Logic.Utilities.csproj", "Src/Logic/Utilities/"]
COPY ["Src/Rules/Events/Digester.Rules.Events.csproj", "Src/Rules/Events/"]
COPY ["Src/Rules/Utilities/Digester.Rules.Utilities.csproj", "Src/Rules/Utilities/"]

RUN dotnet restore "AsyncImage-Fetcher-Service.sln"

COPY . .

WORKDIR "/src/Src/Adapters/Api"

RUN dotnet build "Digester.Adapters.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Digester.Adapters.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5000
ENTRYPOINT ["dotnet", "Digester.Adapters.Api.dll"] 