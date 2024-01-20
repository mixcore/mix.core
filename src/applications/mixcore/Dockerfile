#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY ["src/applications/mixcore/mixcore.csproj", "src/applications/mixcore/"]
COPY ["src/applications/mixcore.host.aspire/mixcore.host.aspire.ServiceDefaults/mixcore.host.aspire.ServiceDefaults.csproj", "src/applications/mixcore.host.aspire/mixcore.host.aspire.ServiceDefaults/"]
COPY ["src/platform/mix.identity/mix.identity.csproj", "src/platform/mix.identity/"]
COPY ["src/platform/mix.database/mix.database.csproj", "src/platform/mix.database/"]
COPY ["src/platform/mix.shared/mix.shared.csproj", "src/platform/mix.shared/"]
COPY ["src/platform/core/mix-heart/src/mix.heart/mix.heart.csproj", "src/platform/core/mix-heart/src/mix.heart/"]
COPY ["src/platform/mix.constant/mix.constant.csproj", "src/platform/mix.constant/"]
COPY ["src/platform/mix.library/mix.library.csproj", "src/platform/mix.library/"]
COPY ["src/platform/core/mix.mixdb.event/mix.mixdb.event.csproj", "src/platform/core/mix.mixdb.event/"]
COPY ["src/platform/mix.mixdb/mix.mixdb.csproj", "src/platform/mix.mixdb/"]
COPY ["src/platform/mix.service/mix.service.csproj", "src/platform/mix.service/"]
COPY ["src/platform/mix.quartz/mix.quartz.csproj", "src/platform/mix.quartz/"]
COPY ["src/platform/mix.queue/mix.queue.csproj", "src/platform/mix.queue/"]
COPY ["src/services/mix-message-queue/mix.mq.server/mix.mq.server.csproj", "src/services/mix-message-queue/mix.mq/"]
COPY ["src/services/mix-message-queue/mix.mq.lib/mix.mq.lib.csproj", "src/services/mix-message-queue/mix.mq.lib/"]
COPY ["src/platform/mix.signalr/mix.signalr.csproj", "src/platform/mix.signalr/"]
COPY ["src/platform/mix.repodb/mix.repodb.csproj", "src/platform/mix.repodb/"]
COPY ["src/platform/mix.communicator/mix.communicator.csproj", "src/platform/mix.communicator/"]
COPY ["src/platform/mix.signalr.hub/mix.signalr.hub.csproj", "src/platform/mix.signalr.hub/"]
COPY ["src/platform/mix.log/mix.log.lib.csproj", "src/platform/mix.log/"]
COPY ["src/modules/mix.grpc/mix.grpc.csproj", "src/modules/mix.grpc/"]
COPY ["src/modules/mix.common/mix.common.csproj", "src/modules/mix.common/"]
COPY ["src/modules/mix.messenger/mix.messenger.csproj", "src/modules/mix.messenger/"]
COPY ["src/modules/mix.portal/mix.portal.csproj", "src/modules/mix.portal/"]
COPY ["src/services/core/mix-databases/mix.services.databases.lib/mix.services.databases.lib.csproj", "src/services/core/mix-databases/mix.services.databases.lib/"]
COPY ["src/modules/mix.scheduler/mix.scheduler.csproj", "src/modules/mix.scheduler/"]
COPY ["src/modules/mix.log/mix.log.csproj", "src/modules/mix.log/"]
COPY ["src/modules/mix.storage/mix.storage.csproj", "src/modules/mix.storage/"]
COPY ["src/platform/mix.storage.lib/mix.storage.lib.csproj", "src/platform/mix.storage.lib/"]
COPY ["src/modules/mix.tenancy/mix.tenancy.csproj", "src/modules/mix.tenancy/"]
COPY ["src/services/core/ecommerces/mix.services.ecommerce/mix.services.ecommerce.csproj", "src/services/core/ecommerces/mix.services.ecommerce/"]
COPY ["src/services/core/ecommerces/mix.services.ecommerce.lib/mix.services.ecommerce.lib.csproj", "src/services/core/ecommerces/mix.services.ecommerce.lib/"]
COPY ["src/services/core/graphql/mix.services.graphql/mix.services.graphql.csproj", "src/services/core/graphql/mix.services.graphql/"]
COPY ["src/services/core/graphql/mix.services.graphql.lib/mix.services.graphql.lib.csproj", "src/services/core/graphql/mix.services.graphql.lib/"]
COPY ["src/services/core/mix-databases/mix.servives.databases/mix.services.databases.csproj", "src/services/core/mix-databases/mix.servives.databases/"]
RUN dotnet restore "/src/applications/mixcore/mixcore.csproj"
COPY . .
RUN dotnet build "/src/applications/mixcore/mixcore.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "/src/applications/mixcore/mixcore.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mixcore.dll"]