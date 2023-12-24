#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["applications/mixcore/mixcore.csproj", "applications/mixcore/"]
COPY ["applications/mixcore.host.aspire/mixcore.host.aspire.ServiceDefaults/mixcore.host.aspire.ServiceDefaults.csproj", "applications/mixcore.host.aspire/mixcore.host.aspire.ServiceDefaults/"]
COPY ["modules/mix.account/mix.account.csproj", "modules/mix.account/"]
COPY ["platform/mix.identity/mix.identity.csproj", "platform/mix.identity/"]
COPY ["platform/mix.auth/mix.auth.csproj", "platform/mix.auth/"]
COPY ["platform/mix.database/mix.database.csproj", "platform/mix.database/"]
COPY ["platform/mix.shared/mix.shared.csproj", "platform/mix.shared/"]
COPY ["platform/core/mix-heart/src/mix.heart/mix.heart.csproj", "platform/core/mix-heart/src/mix.heart/"]
COPY ["platform/mix.constant/mix.constant.csproj", "platform/mix.constant/"]
COPY ["platform/mix.library/mix.library.csproj", "platform/mix.library/"]
COPY ["platform/core/mix.mixdb.event/mix.mixdb.event.csproj", "platform/core/mix.mixdb.event/"]
COPY ["platform/mix.mixdb/mix.mixdb.csproj", "platform/mix.mixdb/"]
COPY ["platform/mix.service/mix.service.csproj", "platform/mix.service/"]
COPY ["platform/mix.quartz/mix.quartz.csproj", "platform/mix.quartz/"]
COPY ["platform/mix.queue/mix.queue.csproj", "platform/mix.queue/"]
COPY ["services/mix-message-queue/mix.mq/mix.mq.csproj", "services/mix-message-queue/mix.mq/"]
COPY ["services/mix-message-queue/mix.mq.lib/mix.mq.lib.csproj", "services/mix-message-queue/mix.mq.lib/"]
COPY ["platform/mix.signalr/mix.signalr.csproj", "platform/mix.signalr/"]
COPY ["platform/mix.repodb/mix.repodb.csproj", "platform/mix.repodb/"]
COPY ["platform/mix.communicator/mix.communicator.csproj", "platform/mix.communicator/"]
COPY ["platform/mix.signalr.hub/mix.signalr.hub.csproj", "platform/mix.signalr.hub/"]
COPY ["platform/mix.log/mix.log.lib.csproj", "platform/mix.log/"]
COPY ["modules/mix.grpc/mix.grpc.csproj", "modules/mix.grpc/"]
COPY ["modules/mix.common/mix.common.csproj", "modules/mix.common/"]
COPY ["modules/mix.messenger/mix.messenger.csproj", "modules/mix.messenger/"]
COPY ["modules/mix.portal/mix.portal.csproj", "modules/mix.portal/"]
COPY ["services/core/mix-databases/mix.services.databases.lib/mix.services.databases.lib.csproj", "services/core/mix-databases/mix.services.databases.lib/"]
COPY ["modules/mix.scheduler/mix.scheduler.csproj", "modules/mix.scheduler/"]
COPY ["modules/mix.log/mix.log.csproj", "modules/mix.log/"]
COPY ["modules/mix.storage/mix.storage.csproj", "modules/mix.storage/"]
COPY ["platform/mix.storage.lib/mix.storage.lib.csproj", "platform/mix.storage.lib/"]
COPY ["modules/mix.tenancy/mix.tenancy.csproj", "modules/mix.tenancy/"]
COPY ["services/core/ecommerces/mix.services.ecommerce/mix.services.ecommerce.csproj", "services/core/ecommerces/mix.services.ecommerce/"]
COPY ["services/core/ecommerces/mix.services.ecommerce.lib/mix.services.ecommerce.lib.csproj", "services/core/ecommerces/mix.services.ecommerce.lib/"]
COPY ["services/core/graphql/mix.services.graphql/mix.services.graphql.csproj", "services/core/graphql/mix.services.graphql/"]
COPY ["services/core/graphql/mix.services.graphql.lib/mix.services.graphql.lib.csproj", "services/core/graphql/mix.services.graphql.lib/"]
COPY ["services/core/mix-databases/mix.servives.databases/mix.services.databases.csproj", "services/core/mix-databases/mix.servives.databases/"]
RUN dotnet restore "./applications/mixcore/./mixcore.csproj"
COPY . .
WORKDIR "/src/applications/mixcore"
RUN dotnet build "./mixcore.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./mixcore.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mixcore.dll"]