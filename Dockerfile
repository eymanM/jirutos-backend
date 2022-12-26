#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JiruTosEndpoint/JiruTosEndpoint.csproj", "JiruTosEndpoint/"]
COPY ["JiraService/JiraService.csproj", "JiraService/"]
COPY ["Foundation/Foundation.csproj", "Foundation/"]
COPY ["ClickUpService/ClickUpService.csproj", "ClickUpService/"]
RUN dotnet restore "JiruTosEndpoint/JiruTosEndpoint.csproj"
COPY . .
WORKDIR "/src/JiruTosEndpoint"
RUN dotnet build "JiruTosEndpoint.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JiruTosEndpoint.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JiruTosEndpoint.dll"]