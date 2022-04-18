#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/AppFramework.Migrator/AppFramework.Migrator.csproj", "src/AppFramework.Migrator/"]
COPY ["src/AppFramework.EntityFrameworkCore/AppFramework.EntityFrameworkCore.csproj", "src/AppFramework.EntityFrameworkCore/"]
COPY ["src/AppFramework.Core/AppFramework.Core.csproj", "src/AppFramework.Core/"]
COPY ["src/AppFramework.Core.Shared/AppFramework.Core.Shared.csproj", "src/AppFramework.Core.Shared/"]
RUN dotnet restore "src/AppFramework.Migrator/AppFramework.Migrator.csproj"
COPY . .
WORKDIR "/src/src/AppFramework.Migrator"
RUN dotnet build "AppFramework.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppFramework.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppFramework.Migrator.dll", "-s"]
