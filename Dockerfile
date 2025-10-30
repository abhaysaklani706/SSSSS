# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy csproj and restore dependencies
COPY AdminServerStub/*.csproj ./AdminServerStub/
RUN dotnet restore ./AdminServerStub/AdminServerStub.csproj

# Copy everything else and build
COPY AdminServerStub/. ./AdminServerStub/
WORKDIR /source/AdminServerStub
RUN dotnet publish -c Release -o /app --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Expose port (Railway will set the PORT environment variable)
EXPOSE 5030

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5030
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "AdminServerStub.dll"]
