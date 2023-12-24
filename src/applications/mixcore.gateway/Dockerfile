#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["applications/mixcore.gateway/mixcore.gateway.csproj", "applications/mixcore.gateway/"]
COPY ["applications/mixcore.host.aspire/mixcore.host.aspire.ServiceDefaults/mixcore.host.aspire.ServiceDefaults.csproj", "applications/mixcore.host.aspire/mixcore.host.aspire.ServiceDefaults/"]
RUN dotnet restore "./applications/mixcore.gateway/./mixcore.gateway.csproj"
COPY . .
WORKDIR "/src/applications/mixcore.gateway"
RUN dotnet build "./mixcore.gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./mixcore.gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mixcore.gateway.dll"]