# Use the official .NET 10 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["NammaHall.Api/NammaHall.Api.csproj", "NammaHall.Api/"]
COPY ["NammaHall.Application/NammaHall.Application.csproj", "NammaHall.Application/"]
COPY ["NammaHall.Domain/NammaHall.Domain.csproj", "NammaHall.Domain/"]
COPY ["NammaHall.Infrastructure/NammaHall.Infrastructure.csproj", "NammaHall.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "NammaHall.Api/NammaHall.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/NammaHall.Api"
RUN dotnet build "NammaHall.Api.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "NammaHall.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy published app
COPY --from=publish /app/publish .

# Use a startup script to handle Render's PORT environment variable
# Render provides PORT at runtime, we need to use it
ENTRYPOINT ["sh", "-c", "export ASPNETCORE_URLS=http://+:${PORT:-8080} && dotnet NammaHall.Api.dll"]

