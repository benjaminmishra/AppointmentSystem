FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION
WORKDIR /src
COPY . .
WORKDIR "/src/AppointmentSystem.MigrationRunner"
RUN dotnet build "./AppointmentSystem.MigrationRunner.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION
RUN dotnet publish "./AppointmentSystem.MigrationRunner.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppointmentSystem.MigrationRunner.dll"]
