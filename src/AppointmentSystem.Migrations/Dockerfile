FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AppointmentSystem.Migrations/AppointmentSystem.Migrations.csproj", "src/AppointmentSystem.Migrations/"]
RUN dotnet restore "src/AppointmentSystem.Migrations/AppointmentSystem.Migrations.csproj"
COPY . .
WORKDIR "/src/src/AppointmentSystem.Migrations"
RUN dotnet build "AppointmentSystem.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AppointmentSystem.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppointmentSystem.Migrations.dll"]
