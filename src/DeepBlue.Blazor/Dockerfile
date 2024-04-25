#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

## OLD:

FROM nginx AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DeepBlue.Blazor/nginx.conf", "/etc/nginx/nginx.conf"]
COPY ["src/DeepBlue.Blazor/DeepBlue.Blazor.csproj", "src/DeepBlue.Blazor/"]
COPY ["src/DeepBlue.Shared/DeepBlue.Shared.csproj", "src/DeepBlue.Shared/"]
RUN dotnet restore "./src/DeepBlue.Blazor/DeepBlue.Blazor.csproj"
COPY . .
WORKDIR "/src/src/DeepBlue.Blazor"
RUN dotnet build "./DeepBlue.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DeepBlue.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .