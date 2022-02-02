#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["API/Coodes.Back.End.Challenge2021.CSharp.Api.csproj", "API/"]
COPY ["Core/Coodes.Back.End.Challenge2021.CSharp.Core.csproj", "Core/"]
RUN dotnet restore "API/Coodes.Back.End.Challenge2021.CSharp.Api.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "Coodes.Back.End.Challenge2021.CSharp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Coodes.Back.End.Challenge2021.CSharp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Coodes.Back.End.Challenge2021.CSharp.Api.dll"]