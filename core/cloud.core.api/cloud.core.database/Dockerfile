#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["cloud.core.database/cloud.core.database.csproj", "cloud.core.database/"]
COPY ["cloud.core.objects/cloud.core.objects.csproj", "cloud.core.objects/"]
RUN dotnet restore "cloud.core.database/cloud.core.database.csproj"
COPY . .
WORKDIR "/src/cloud.core.database"
RUN dotnet build "cloud.core.database.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cloud.core.database.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cloud.core.database.dll"]
