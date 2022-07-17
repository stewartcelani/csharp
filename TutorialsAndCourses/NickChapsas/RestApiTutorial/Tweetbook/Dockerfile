FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ARG Mode=Debug
RUN echo "$Mode"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tweetbook/Tweetbook.csproj", "Tweetbook/"]
RUN dotnet restore "Tweetbook/Tweetbook.csproj"
COPY . .
WORKDIR "/src/Tweetbook"
RUN dotnet build "Tweetbook.csproj" -c $Mode -o /app/build

FROM build AS publish
RUN dotnet publish "Tweetbook.csproj" -c $Mode -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Tweetbook.dll"]
