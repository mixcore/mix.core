#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

# Removed because SPA Portal to new repo here: https://github.com/mixcore/mix.spa.portal
# FROM node:10.16.3 AS node-env
# WORKDIR /app
# COPY src/. ./
# WORKDIR /app/portal-app
# RUN npm install
# RUN npm install gulp-cli -g
# RUN npm install gulp -D
# RUN gulp build
# mcr.microsoft.com/dotnet/core/sdk:3.1

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy everything else and build
COPY src/. ./
RUN dotnet restore Mix.Cms.Web/Mix.Cms.Web.csproj
RUN dotnet publish Mix.Cms.Web/Mix.Cms.Web.csproj -c Release

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
# COPY --from=node-env /app/Mix.Cms.Web/wwwroot .
COPY --from=build-env /app/Mix.Cms.Web/bin/Release/netcoreapp3.1/publish .
ENTRYPOINT ["dotnet", "Mix.Cms.Web.dll"]

# Build -> Tag -> Push process
# docker build -t mixcoreimage -f Dockerfile .
# docker tag mixcoreimage mixcore/mix.core:v1.0.0-alpha.1
# docker push mixcore/mix.core:v1.0.0-alpha.1
# docker run -it --rm -p 5000:80 --name aspnetcore_sample mixcoreimage
# Server=db;Database=master;User=sa;Password=P@ssw0rd;
# docker-compose build
# docker-compose up
