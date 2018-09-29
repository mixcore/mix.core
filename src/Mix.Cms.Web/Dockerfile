#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 54647
EXPOSE 44319

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["Mix.Cms.Web/Mix.Cms.Web.csproj", "Mix.Cms.Web/"]
RUN dotnet restore "Mix.Cms.Web/Mix.Cms.Web.csproj"
COPY . .
WORKDIR "/src/Mix.Cms.Web"
RUN dotnet build "Mix.Cms.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Mix.Cms.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mix.Cms.Web.dll"]