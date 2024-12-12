# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER $APP_UID
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SkyEagle/SkyEagle.csproj", "SkyEagle/"]
COPY ["SkyModel/SkyModel.csproj", "SkyModel/"]
COPY ["SkyDTO/SkyDTO.csproj", "SkyDTO/"]
RUN dotnet restore "./SkyEagle/SkyEagle.csproj"
COPY . .
WORKDIR "/src/SkyEagle"
RUN dotnet build "./SkyEagle.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SkyEagle.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV DOTNET_GENERATE_ASPNET_CERTIFICATE=false DOTNET_NOLOGO=true NUGET_XMLDOC_MODE=skip ASPNETCORE_ENVIRONMENT=Production
EXPOSE 82
ENTRYPOINT ["dotnet", "SkyEagle.dll"]