# https://hub.docker.com/_/microsoft-dotnet-sdk

# Stage 1: Base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Stage 2: Get submodules
FROM alpine/git:latest AS submodules
WORKDIR /app
RUN git clone --branch develop/v2 --depth 1 https://github.com/mixcore/mix.heart.git src/platform/core/mix-heart

# Stage 3: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Install protobuf compiler
RUN apt-get update && apt-get install -y protobuf-compiler && \
    rm -rf /var/lib/apt/lists/*

# Copy project files
# Platform Core
COPY src/platform/core/mix-heart/src/mix.heart/mix.heart.csproj src/platform/core/mix-heart/src/mix.heart/
COPY src/platform/mix.constant/mix.constant.csproj src/platform/mix.constant/
COPY src/platform/mix.shared/mix.shared.csproj src/platform/mix.shared/

# Platform Services
COPY src/platform/mix.database/mix.database.csproj src/platform/mix.database/
COPY src/platform/mix.auth/mix.auth.csproj src/platform/mix.auth/
COPY src/platform/mix.queue/mix.queue.csproj src/platform/mix.queue/
COPY src/platform/mix.identity/mix.identity.csproj src/platform/mix.identity/
COPY src/platform/mix.signalr/mix.signalr.csproj src/platform/mix.signalr/
COPY src/platform/mix.quartz/mix.quartz.csproj src/platform/mix.quartz/
COPY src/platform/mix.communicator/mix.communicator.csproj src/platform/mix.communicator/
COPY src/platform/mix.service/mix.service.csproj src/platform/mix.service/
COPY src/platform/mix.log/mix.log.lib.csproj src/platform/mix.log/
COPY src/platform/mix.repodb/mix.repodb.csproj src/platform/mix.repodb/
COPY src/platform/mix.scylladb/mix.scylladb.csproj src/platform/mix.scylladb/

# Modules
COPY src/modules/mix.common/mix.common.csproj src/modules/mix.common/
COPY src/modules/mix.grpc/mix.grpc.csproj src/modules/mix.grpc/
COPY src/modules/mix.portal/mix.portal.csproj src/modules/mix.portal/
COPY src/modules/mix.log/mix.log.csproj src/modules/mix.log/
COPY src/modules/mix.scheduler/mix.scheduler.csproj src/modules/mix.scheduler/
COPY src/modules/mix.storage/mix.storage.csproj src/modules/mix.storage/
COPY src/modules/mix.tenancy/mix.tenancy.csproj src/modules/mix.tenancy/

# Services
COPY src/services/core/ecommerces/mix.services.ecommerce/mix.services.ecommerce.csproj src/services/core/ecommerces/mix.services.ecommerce/
COPY src/services/core/graphql/mix.services.graphql/mix.services.graphql.csproj src/services/core/graphql/mix.services.graphql/
COPY src/services/core/mix-auth-service/mix.auth.api/mix.auth.api.csproj src/services/core/mix-auth-service/mix.auth.api/
COPY src/services/core/mix-databases/mix.servives.databases/mix.services.databases.csproj src/services/core/mix-databases/mix.servives.databases/
COPY src/services/core/mix-message-queue/mix.mq.server/mix.mq.server.csproj src/services/core/mix-message-queue/mix.mq.server/
COPY src/services/mix.automation/mix.automation.api/mix.automation.api.csproj src/services/mix.automation/mix.automation.api/

# Main application
COPY src/applications/mixcore/mixcore.csproj src/applications/mixcore/
COPY src/applications/mixcore.host.aspire.ServiceDefaults/mixcore.host.aspire.ServiceDefaults.csproj src/applications/mixcore.host.aspire.ServiceDefaults/

# Copy submodule from previous stage
COPY --from=submodules /app/src/platform/core/mix-heart src/platform/core/mix-heart

# Restore packages
RUN dotnet restore src/applications/mixcore/mixcore.csproj

# Copy remaining source code
COPY . .

# Build
RUN dotnet build src/applications/mixcore/mixcore.csproj -c Release -o /app/build --no-restore

# Stage 4: Publish
FROM build AS publish
RUN dotnet publish src/applications/mixcore/mixcore.csproj -c Release -o /app/publish /p:UseAppHost=false --no-restore

# Stage 5: Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mixcore.dll"]


#############################
# PUBLISH NEW IMAGE GUIDELINE
#############################
# Build -> Tag -> Push process
# docker build -t mixcoreimage -f Dockerfile .
# docker tag mixcoreimage mixcore/mix.core:v1.0.0-alpha.1
# docker push mixcore/mix.core:v1.0.0-alpha.1
# docker run -it --rm -p 5000:80 --name aspnetcore_sample mixcoreimage
# Server=db;Database=master;User=sa;Password=P@ssw0rd;
# docker-compose build
# docker-compose up
