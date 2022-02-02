#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["API/Coodesh.Back.End.Challenge2021.CSharp.Api.csproj", "API/"]
COPY ["Core/Coodesh.Back.End.Challenge2021.CSharp.Core.csproj", "Core/"]
RUN dotnet restore "API/Coodesh.Back.End.Challenge2021.CSharp.Api.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "Coodesh.Back.End.Challenge2021.CSharp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Coodesh.Back.End.Challenge2021.CSharp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Coodesh.Back.End.Challenge2021.CSharp.Api.dll"]