FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BCinema.API/BCinema.API.csproj", "BCinema.API/"]
COPY ["BCinema.Application/BCinema.Application.csproj", "BCinema.Application/"]
COPY ["BCinema.Domain/BCinema.Domain.csproj", "BCinema.Domain/"]
COPY ["BCinema.Infrastructure/BCinema.Infrastructure.csproj", "BCinema.Infrastructure/"]
COPY ["BCinema.Persistence/BCinema.Persistence.csproj", "BCinema.Persistence/"]
RUN dotnet restore "BCinema.API/BCinema.API.csproj"
COPY . .
WORKDIR "/src/BCinema.API"
RUN dotnet build "BCinema.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BCinema.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BCinema.API.dll"]
