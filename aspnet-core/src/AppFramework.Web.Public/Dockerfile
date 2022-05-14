#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/AppFramework.Web.Public/AppFramework.Web.Public.csproj", "src/AppFramework.Web.Public/"]
COPY ["src/AppFramework.Web.Core/AppFramework.Web.Core.csproj", "src/AppFramework.Web.Core/"]
COPY ["src/AppFramework.Application/AppFramework.Application.csproj", "src/AppFramework.Application/"]
COPY ["src/AppFramework.Application.Shared/AppFramework.Application.Shared.csproj", "src/AppFramework.Application.Shared/"]
COPY ["src/AppFramework.Core.Shared/AppFramework.Core.Shared.csproj", "src/AppFramework.Core.Shared/"]
COPY ["src/AppFramework.Core/AppFramework.Core.csproj", "src/AppFramework.Core/"]
COPY ["src/AppFramework.EntityFrameworkCore/AppFramework.EntityFrameworkCore.csproj", "src/AppFramework.EntityFrameworkCore/"]
COPY ["src/AppFramework.GraphQL/AppFramework.GraphQL.csproj", "src/AppFramework.GraphQL/"]
RUN dotnet restore "src/AppFramework.Web.Public/AppFramework.Web.Public.csproj"
COPY . .
WORKDIR "/src/src/AppFramework.Web.Public"
RUN dotnet build "AppFramework.Web.Public.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppFramework.Web.Public.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppFramework.Web.Public.dll"]
