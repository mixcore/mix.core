# https://hub.docker.com/_/microsoft-dotnet-sdk

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the entire source directory
COPY src/ .

# Restore dependencies
RUN dotnet restore

# Build and publish the main application
RUN dotnet publish applications/mixcore/mixcore.csproj -c Release -o /app/publish

# Build gateway
FROM build AS gateway
RUN dotnet publish applications/mixcore.gateway/mixcore.gateway.csproj -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
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
