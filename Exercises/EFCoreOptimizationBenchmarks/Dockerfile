FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EFCoreOptimizationBenchmarks.csproj", "./"]
RUN dotnet restore "EFCoreOptimizationBenchmarks.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "EFCoreOptimizationBenchmarks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EFCoreOptimizationBenchmarks.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EFCoreOptimizationBenchmarks.dll", "--cli",  "/usr/share/dotnet/dotnet", "--filter", '*Benchmark*']
