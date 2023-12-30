#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

COPY ["src/services/mix-message-queue/mix.mq/mix.mq.csproj", "src/services/mix-message-queue/mix.mq/"]
COPY ["src/applications/mixcore.host.aspire/mixcore.host.aspire.ServiceDefaults/mixcore.host.aspire.ServiceDefaults.csproj", "src/applications/mixcore.host.aspire/mixcore.host.aspire.ServiceDefaults/"]
COPY ["src/services/mix-message-queue/mix.mq.lib/mix.mq.lib.csproj", "src/services/mix-message-queue/mix.mq.lib/"]
COPY ["src/platform/core/mix-heart/src/mix.heart/mix.heart.csproj", "src/platform/core/mix-heart/src/mix.heart/"]
COPY ["src/platform/mix.constant/mix.constant.csproj", "src/platform/mix.constant/"]
RUN dotnet restore "src/services/mix-message-queue/mix.mq/mix.mq.csproj"
COPY . .
RUN dotnet build "src/services/mix-message-queue/mix.mq/mix.mq.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/services/mix-message-queue/mix.mq/mix.mq.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mix.mq.dll"]