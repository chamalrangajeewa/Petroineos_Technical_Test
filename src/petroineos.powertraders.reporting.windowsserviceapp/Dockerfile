#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/petroineos.powertraders.reporting.windowsserviceapp/petroineos.powertraders.reporting.windowsserviceapp.csproj", "src/petroineos.powertraders.reporting.windowsserviceapp/"]
RUN dotnet restore "src/petroineos.powertraders.reporting.windowsserviceapp/petroineos.powertraders.reporting.windowsserviceapp.csproj"
COPY . .
WORKDIR "/src/src/petroineos.powertraders.reporting.windowsserviceapp"
RUN dotnet build "petroineos.powertraders.reporting.windowsserviceapp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "petroineos.powertraders.reporting.windowsserviceapp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "petroineos.powertraders.reporting.windowsserviceapp.dll"]