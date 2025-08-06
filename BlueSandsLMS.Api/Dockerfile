# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project files
COPY ["stemlabs-backend-new/BlueSandsLMS.Api/BlueSandsLMS.Api.csproj", "BlueSandsLMS.Api/"]
COPY ["stemlabs-backend-new/BlueSandsLMS.Application/BlueSandsLMS.Application.csproj", "BlueSandsLMS.Application/"]
COPY ["stemlabs-backend-new/BlueSandsLMS.Infrastructure/BlueSandsLMS.Infrastructure.csproj", "BlueSandsLMS.Infrastructure/"]
COPY ["stemlabs-backend-new/BlueSandsLMS.Core/BlueSandsLMS.Core.csproj", "BlueSandsLMS.Core/"]
COPY ["stemlabs-backend-new/BlueSandsLMS.Common/BlueSandsLMS.Common.csproj", "BlueSandsLMS.Common/"]

# Restore dependencies
RUN dotnet restore "BlueSandsLMS.Api/BlueSandsLMS.Api.csproj"

# Copy everything else
COPY stemlabs-backend-new/ .

# Build and publish
WORKDIR "/src/BlueSandsLMS.Api"
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "BlueSandsLMS.Api.dll"]
