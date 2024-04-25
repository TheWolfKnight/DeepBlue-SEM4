FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/api/DeepBlue.Api.MoveValidator/DeepBlue.Api.MoveValidator.csproj", "src/api/DeepBlue.Api.MoveValidator/"]
COPY ["src/DeepBlue.Shared/DeepBlue.Shared.csproj", "src/DeepBlue.Shared/"]
RUN dotnet restore "./src/api/DeepBlue.Api.MoveValidator/DeepBlue.Api.MoveValidator.csproj"
COPY . .
WORKDIR "/src/src/api/DeepBlue.Api.MoveValidator"
RUN dotnet build "./DeepBlue.Api.MoveValidator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DeepBlue.Api.MoveValidator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeepBlue.Api.MoveValidator.dll"]