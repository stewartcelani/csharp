FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Movies.Api/Movies.Api.csproj", "src/Movies.Api/"]
COPY ["src/Movies.Application/Movies.Application.csproj", "src/Movies.Application/"]
COPY ["src/Movies.Contracts/Movies.Contracts.csproj", "src/Movies.Contracts/"]
RUN dotnet restore "src/Movies.Api/Movies.Api.csproj"
COPY . .
WORKDIR "/src/src/Movies.Api"
RUN dotnet build "Movies.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Movies.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movies.Api.dll"]
