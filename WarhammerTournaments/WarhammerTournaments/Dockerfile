FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WarhammerTournaments/WarhammerTournaments.csproj", "WarhammerTournaments/"]
COPY ["WarhammerTournaments.DAL/WarhammerTournaments.DAL.csproj", "WarhammerTournaments.DAL/"]
COPY ["User.ManagementService/User.ManagementService.csproj", "User.ManagementService/"]

RUN dotnet restore "WarhammerTournaments/WarhammerTournaments.csproj"
COPY . .
WORKDIR "/src/WarhammerTournaments"
RUN dotnet build "WarhammerTournaments.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WarhammerTournaments.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WarhammerTournaments.dll"]
