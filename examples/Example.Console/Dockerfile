FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["examples/Example.Console/Example.Console.csproj", "examples/Example.Console/"]
COPY ["src/SaveApis.Common/SaveApis.Common.csproj", "src/SaveApis.Common/"]
RUN dotnet restore "examples/Example.Console/Example.Console.csproj"
COPY . .
WORKDIR "/src/examples/Example.Console"
RUN dotnet build "Example.Console.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Example.Console.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Example.Console.dll"]
