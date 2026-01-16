FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["global.json", "./"]
COPY ["ChatOps.sln", "./"]
COPY ["ChatOps.Api/ChatOps.Api.csproj", "ChatOps.Api/"]
COPY ["ChatOps.App/ChatOps.App.csproj", "ChatOps.App/"]
COPY ["ChatOps.SharedKernel/ChatOps.SharedKernel.csproj", "ChatOps.SharedKernel/"]
COPY ["ChatOps.Infra/ChatOps.Infra.csproj", "ChatOps.Infra/"]
RUN dotnet restore "ChatOps.Api/ChatOps.Api.csproj"
COPY . .
WORKDIR "/src/ChatOps.Api"
RUN dotnet build "./ChatOps.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ChatOps.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatOps.Api.dll"]
