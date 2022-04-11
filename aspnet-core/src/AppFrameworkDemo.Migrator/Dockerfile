#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/AppFrameworkDemo.Migrator/AppFrameworkDemo.Migrator.csproj", "src/AppFrameworkDemo.Migrator/"]
COPY ["src/AppFrameworkDemo.EntityFrameworkCore/AppFrameworkDemo.EntityFrameworkCore.csproj", "src/AppFrameworkDemo.EntityFrameworkCore/"]
COPY ["src/AppFrameworkDemo.Core/AppFrameworkDemo.Core.csproj", "src/AppFrameworkDemo.Core/"]
COPY ["src/AppFrameworkDemo.Core.Shared/AppFrameworkDemo.Core.Shared.csproj", "src/AppFrameworkDemo.Core.Shared/"]
RUN dotnet restore "src/AppFrameworkDemo.Migrator/AppFrameworkDemo.Migrator.csproj"
COPY . .
WORKDIR "/src/src/AppFrameworkDemo.Migrator"
RUN dotnet build "AppFrameworkDemo.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppFrameworkDemo.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppFrameworkDemo.Migrator.dll", "-s"]
