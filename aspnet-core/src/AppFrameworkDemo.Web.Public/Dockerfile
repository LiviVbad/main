#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/AppFrameworkDemo.Web.Public/AppFrameworkDemo.Web.Public.csproj", "src/AppFrameworkDemo.Web.Public/"]
COPY ["src/AppFrameworkDemo.Web.Core/AppFrameworkDemo.Web.Core.csproj", "src/AppFrameworkDemo.Web.Core/"]
COPY ["src/AppFrameworkDemo.Application/AppFrameworkDemo.Application.csproj", "src/AppFrameworkDemo.Application/"]
COPY ["src/AppFrameworkDemo.Application.Shared/AppFrameworkDemo.Application.Shared.csproj", "src/AppFrameworkDemo.Application.Shared/"]
COPY ["src/AppFrameworkDemo.Core.Shared/AppFrameworkDemo.Core.Shared.csproj", "src/AppFrameworkDemo.Core.Shared/"]
COPY ["src/AppFrameworkDemo.Core/AppFrameworkDemo.Core.csproj", "src/AppFrameworkDemo.Core/"]
COPY ["src/AppFrameworkDemo.EntityFrameworkCore/AppFrameworkDemo.EntityFrameworkCore.csproj", "src/AppFrameworkDemo.EntityFrameworkCore/"]
COPY ["src/AppFrameworkDemo.GraphQL/AppFrameworkDemo.GraphQL.csproj", "src/AppFrameworkDemo.GraphQL/"]
RUN dotnet restore "src/AppFrameworkDemo.Web.Public/AppFrameworkDemo.Web.Public.csproj"
COPY . .
WORKDIR "/src/src/AppFrameworkDemo.Web.Public"
RUN dotnet build "AppFrameworkDemo.Web.Public.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppFrameworkDemo.Web.Public.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppFrameworkDemo.Web.Public.dll"]
