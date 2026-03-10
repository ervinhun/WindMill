# Multi-stage build for ASP.NET Core app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file first to leverage Docker layer caching
COPY ["WindMill.csproj", "./"]
RUN dotnet restore "./WindMill.csproj"

# Copy source and publish
COPY . .
RUN dotnet publish "./WindMill.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Bind Kestrel to a container-friendly HTTP port
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "WindMill.dll"]

