#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR "/src"
COPY ["Coodesh.Back.End.Challenge2021.CSharp.sln", "./"]
COPY ["Core/Coodesh.Back.End.Challenge2021.CSharp.Core.csproj", "Core/"]
COPY ["Api/Coodesh.Back.End.Challenge2021.CSharp.Api.csproj", "Api/"]
COPY ["Cron/Coodesh.Back.End.Challenge2021.CSharp.Cron.csproj", "Cron/"]

RUN dotnet restore
COPY . .

WORKDIR "/src/Core"
RUN dotnet build -c Release -o /app

WORKDIR "/src/Api"
RUN dotnet build -c Release -o /app

WORKDIR "/src/Cron"
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Coodesh.Back.End.Challenge2021.CSharp.Api.dll"]