#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy everything else and build
COPY src/. ./
RUN dotnet restore Mix.Cms.Web/Mix.Cms.Web.csproj
RUN dotnet publish Mix.Cms.Web/Mix.Cms.Web.csproj -c Release

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/Mix.Cms.Web/bin/Release/netcoreapp2.2/publish .
ENTRYPOINT ["dotnet", "app/Mix.Cms.Web.dll"]