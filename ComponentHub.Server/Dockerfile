FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ComponentHub/Server/ComponentHub.Server.csproj", "ComponentHub/Server/"]
COPY ["ComponentHub/Client/ComponentHub.Client.csproj", "ComponentHub/Client/"]
COPY ["ComponentHub/Shared/ComponentHub.Shared.csproj", "ComponentHub/Shared/"]
RUN dotnet restore "ComponentHub/Server/ComponentHub.Server.csproj"
COPY . .
WORKDIR "/src/ComponentHub/Server"
RUN dotnet build "ComponentHub.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ComponentHub.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ComponentHub.Server.dll"]
