# Use a specific version of the runtime image for reproducibility and security
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
# Set a default non-root user for security
ARG APP_UID=1000
USER $APP_UID
WORKDIR /app

# Use a specific version of the SDK image for reproducibility
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# Set build configuration as an argument with a default value
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the project files first to optimize caching
COPY ["examples/Example.Console/Example.Console.csproj", "examples/Example.Console/"]
COPY ["src/SaveApis.Common/SaveApis.Common.csproj", "src/SaveApis.Common/"]

# Restore dependencies using BuildKit cache for efficiency
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore "examples/Example.Console/Example.Console.csproj"

# Copy the rest of the source code
COPY . .

# Set the working directory to the project folder
WORKDIR "/src/examples/Example.Console"

# Build the project using BuildKit cache
RUN --mount=type=cache,target=/root/.nuget/packages dotnet build "Example.Console.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application to a minimal output directory
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN --mount=type=cache,target=/root/.nuget/packages dotnet publish "Example.Console.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the runtime image for the final stage
FROM base AS final
WORKDIR /app

# Copy the published application from the publish stage
COPY --from=publish /app/publish .

# Set a non-root user explicitly in the final stage for security
USER $APP_UID

# Use a specific entrypoint for the application
ENTRYPOINT ["dotnet", "Example.Console.dll"]
