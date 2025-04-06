# https://hub.docker.com/_/microsoft-dotnet-sdk

# Submodule stage
FROM --platform=linux/amd64 alpine/git AS submodules
WORKDIR /app
RUN git clone --branch develop/v2 https://github.com/mixcore/mix.heart.git src/platform/core/mix-heart && \
    cd src/platform/core/mix-heart && \
    echo "Directory structure:" && \
    find . -type f -name "*.csproj" && \
    echo "Current directory contents:" && \
    ls -la

# Build stage
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Create necessary directories
RUN mkdir -p src/platform/core/mix-heart

# Copy submodules
COPY --from=submodules /app/src/platform/core/mix-heart /app/src/platform/core/mix-heart

# Copy project files first to leverage Docker cache
COPY ["src/applications/mixcore/mixcore.csproj", "src/applications/mixcore/"]
COPY ["src/applications/mixcore.gateway/mixcore.gateway.csproj", "src/applications/mixcore.gateway/"]
COPY ["src/applications/mixcore.host.aspire.ServiceDefaults/mixcore.host.aspire.ServiceDefaults.csproj", "src/applications/mixcore.host.aspire.ServiceDefaults/"]
COPY ["src/applications/mixcore.host.aspire.AppHost/mixcore.host.aspire.AppHost.csproj", "src/applications/mixcore.host.aspire.AppHost/"]

# Platform projects
COPY ["src/platform/mix.identity/mix.identity.csproj", "src/platform/mix.identity/"]
COPY ["src/platform/mix.database/mix.database.csproj", "src/platform/mix.database/"]
COPY ["src/platform/mix.shared/mix.shared.csproj", "src/platform/mix.shared/"]
COPY ["src/platform/mix.constant/mix.constant.csproj", "src/platform/mix.constant/"]
COPY ["src/platform/mix.library/mix.library.csproj", "src/platform/mix.library/"]
COPY ["src/platform/core/mix.mixdb.event/mix.mixdb.event.csproj", "src/platform/core/mix.mixdb.event/"]
COPY ["src/platform/mix.mixdb/mix.mixdb.csproj", "src/platform/mix.mixdb/"]
COPY ["src/platform/mix.service/mix.service.csproj", "src/platform/mix.service/"]
COPY ["src/platform/mix.quartz/mix.quartz.csproj", "src/platform/mix.quartz/"]
COPY ["src/platform/mix.queue/mix.queue.csproj", "src/platform/mix.queue/"]
COPY ["src/platform/mix.signalr/mix.signalr.csproj", "src/platform/mix.signalr/"]
COPY ["src/platform/mix.repodb/mix.repodb.csproj", "src/platform/mix.repodb/"]
COPY ["src/platform/mix.communicator/mix.communicator.csproj", "src/platform/mix.communicator/"]
COPY ["src/platform/mix.signalr.hub/mix.signalr.hub.csproj", "src/platform/mix.signalr.hub/"]
COPY ["src/platform/mix.log/mix.log.lib.csproj", "src/platform/mix.log/"]
COPY ["src/platform/mix.storage.lib/mix.storage.lib.csproj", "src/platform/mix.storage.lib/"]
COPY ["src/platform/mix.auth/mix.auth.csproj", "src/platform/mix.auth/"]
COPY ["src/platform/mix.scylladb/mix.scylladb.csproj", "src/platform/mix.scylladb/"]

# Module projects
COPY ["src/modules/mix.grpc/mix.grpc.csproj", "src/modules/mix.grpc/"]
COPY ["src/modules/mix.common/mix.common.csproj", "src/modules/mix.common/"]
COPY ["src/modules/mix.messenger/mix.messenger.csproj", "src/modules/mix.messenger/"]
COPY ["src/modules/mix.portal/mix.portal.csproj", "src/modules/mix.portal/"]
COPY ["src/modules/mix.scheduler/mix.scheduler.csproj", "src/modules/mix.scheduler/"]
COPY ["src/modules/mix.log/mix.log.csproj", "src/modules/mix.log/"]
COPY ["src/modules/mix.storage/mix.storage.csproj", "src/modules/mix.storage/"]
COPY ["src/modules/mix.tenancy/mix.tenancy.csproj", "src/modules/mix.tenancy/"]

# Service projects
COPY ["src/services/core/mix-message-queue/mix.mq.server/mix.mq.server.csproj", "src/services/core/mix-message-queue/mix.mq.server/"]
COPY ["src/services/core/mix-message-queue/mix.mq.lib/mix.mq.lib.csproj", "src/services/core/mix-message-queue/mix.mq.lib/"]
COPY ["src/services/core/mix-databases/mix.services.databases.lib/mix.services.databases.lib.csproj", "src/services/core/mix-databases/mix.services.databases.lib/"]
COPY ["src/services/core/mix-databases/mix.servives.databases/mix.services.databases.csproj", "src/services/core/mix-databases/mix.servives.databases/"]
COPY ["src/services/core/ecommerces/mix.services.ecommerce/mix.services.ecommerce.csproj", "src/services/core/ecommerces/mix.services.ecommerce/"]
COPY ["src/services/core/ecommerces/mix.services.ecommerce.lib/mix.services.ecommerce.lib.csproj", "src/services/core/ecommerces/mix.services.ecommerce.lib/"]
COPY ["src/services/core/graphql/mix.services.graphql/mix.services.graphql.csproj", "src/services/core/graphql/mix.services.graphql/"]
COPY ["src/services/core/graphql/mix.services.graphql.lib/mix.services.graphql.lib.csproj", "src/services/core/graphql/mix.services.graphql.lib/"]
COPY ["src/services/core/mix-auth-service/mix.auth.api/mix.auth.api.csproj", "src/services/core/mix-auth-service/mix.auth.api/"]
COPY ["src/services/mix.automation/mix.automation.api/mix.automation.api.csproj", "src/services/mix.automation/mix.automation.api/"]
COPY ["src/services/mix.automation/mix.automation.lib/mix.automation.lib.csproj", "src/services/mix.automation/mix.automation.lib/"]

# Install protoc for x64
RUN apt-get update && \
    apt-get install -y protobuf-compiler && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Restore dependencies
RUN dotnet restore "src/applications/mixcore/mixcore.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish the main application
RUN dotnet publish "src/applications/mixcore/mixcore.csproj" -c Release -o /app/publish

# Build gateway
FROM build AS gateway
RUN dotnet publish "src/applications/mixcore.gateway/mixcore.gateway.csproj" -c Release -o /app/publish

# Runtime image
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose ports
EXPOSE 80
EXPOSE 443

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENV ASPNETCORE_ENVIRONMENT=Development

# Health check
HEALTHCHECK --interval=30s --timeout=10s --retries=3 \
    CMD curl -f http://localhost/health || exit 1

# Entry point
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
