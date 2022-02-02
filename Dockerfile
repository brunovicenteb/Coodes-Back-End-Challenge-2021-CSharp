#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Api/Coodesh.Back.End.Challenge2021.CSharp.Api.csproj", "Api/"]
COPY ["Core/Coodesh.Back.End.Challenge2021.CSharp.Core.csproj", "Api/"]
RUN dotnet restore "Api/Coodesh.Back.End.Challenge2021.CSharp.Api.csproj"
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "Coodesh.Back.End.Challenge2021.CSharp.Core.csproj" -c Release -o /app/build
RUN dotnet build "Coodesh.Back.End.Challenge2021.CSharp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api/Coodesh.Back.End.Challenge2021.CSharp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Coodesh.Back.End.Challenge2021.CSharp.Api.dll"]