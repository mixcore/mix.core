#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 50301
EXPOSE 44394

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["Mix.Cms.Api/Mix.Cms.Api.csproj", "Mix.Cms.Api/"]
RUN dotnet restore "Mix.Cms.Api/Mix.Cms.Api.csproj"
COPY . .
WORKDIR "/src/Mix.Cms.Api"
RUN dotnet build "Mix.Cms.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Mix.Cms.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mix.Cms.Api.dll"]