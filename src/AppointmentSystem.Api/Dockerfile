FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5049

ENV ASPNETCORE_URLS=http://+:5049

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION
WORKDIR /src
COPY . .
WORKDIR "/src/AppointmentSystem.Api"
RUN dotnet build "./AppointmentSystem.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION
RUN dotnet publish "./AppointmentSystem.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppointmentSystem.Api.dll"]
