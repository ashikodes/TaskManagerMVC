# Use official .NET SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Use .NET SDK for building the app
COPY . ./
RUN dotnet restore

# Install EF CLI inside the container
RUN dotnet tool install --global dotnet-ef

# Publish the app
RUN dotnet publish -c Release -o publish


# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskManagerMVC.dll"]
