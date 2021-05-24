# https://hub.docker.com/_/microsoft-dotnet-sdk

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy everything else and build
COPY src/. ./
# RUN dotnet restore Mix.Cms.Web/Mixcore.csproj
RUN dotnet publish Mix.Cms.Web/Mixcore.csproj -c Release

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS runtime
WORKDIR /app
# COPY --from=node-env /app/Mix.Cms.Web/wwwroot .
COPY --from=build-env /app/Mix.Cms.Web/bin/Release/net5.0/publish .
EXPOSE 80
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
