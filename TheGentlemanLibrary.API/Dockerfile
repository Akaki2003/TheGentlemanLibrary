#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TheGentlemanLibrary.API/TheGentlemanLibrary.API.csproj", "TheGentlemanLibrary.API/"]
COPY ["TheGentlemanLibrary.Application/TheGentlemanLibrary.Application.csproj", "TheGentlemanLibrary.Application/"]
COPY ["TheGentlemanLibrary.Domain/TheGentlemanLibrary.Domain.csproj", "TheGentlemanLibrary.Domain/"]
COPY ["TheGentlemanLibrary.Common/TheGentlemanLibrary.Common.csproj", "TheGentlemanLibrary.Common/"]
COPY ["TheGentlemanLibrary.Infrastructure/TheGentlemanLibrary.Infrastructure.csproj", "TheGentlemanLibrary.Infrastructure/"]
RUN dotnet restore "./TheGentlemanLibrary.API/./TheGentlemanLibrary.API.csproj"
COPY . .
WORKDIR "/src/TheGentlemanLibrary.API"
RUN dotnet build "./TheGentlemanLibrary.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TheGentlemanLibrary.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheGentlemanLibrary.API.dll"]