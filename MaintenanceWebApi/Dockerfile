#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MaintenanceWebApi/MaintenanceWebApi.csproj", "MaintenanceWebApi/"]
COPY ["GoogleCalendarService/GoogleCalendarService.csproj", "GoogleCalendarService/"]
COPY ["JwtAuthenticationManager/JwtAuthenticationManager.csproj", "JwtAuthenticationManager/"]
COPY ["RedisCachingService/RedisCachingService.csproj", "RedisCachingService/"]
RUN dotnet restore "MaintenanceWebApi/MaintenanceWebApi.csproj"
COPY . .
WORKDIR "/src/MaintenanceWebApi"
RUN dotnet build "MaintenanceWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MaintenanceWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MaintenanceWebApi.dll"]