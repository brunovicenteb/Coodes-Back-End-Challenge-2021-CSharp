#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
#EXPOSE 80

# Install Cron
RUN apt-get update -qq && apt-get -y install cron -qq --force-yes

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR "/src"
COPY ["Coodesh.Back.End.Challenge2021.CSharp.sln", "./"]
COPY ["Toolkit/Coodesh.Back.End.Challenge2021.CSharp.Toolkit.csproj", "Toolkit/"]
COPY ["Domain/Coodesh.Back.End.Challenge2021.CSharp.Domain.csproj", "Domain/"]
COPY ["Infra/Coodesh.Back.End.Challenge2021.CSharp.Infra.csproj", "Infra/"]
COPY ["Service/Coodesh.Back.End.Challenge2021.CSharp.Service.csproj", "Service/"]
COPY ["Api/Coodesh.Back.End.Challenge2021.CSharp.Api.csproj", "Api/"]
COPY ["Cron/Coodesh.Back.End.Challenge2021.CSharp.Cron.csproj", "Cron/"]
COPY ["Test/Coodesh.Back.End.Challenge2021.CSharp.Test.csproj", "Test/"]

RUN dotnet restore
COPY . .

WORKDIR "/src/Toolkit"
RUN dotnet build -c Release -o /app

WORKDIR "/src/Domain"
RUN dotnet build -c Release -o /app

WORKDIR "/src/Infra"
RUN dotnet build -c Release -o /app

WORKDIR "/src/Service"
RUN dotnet build -c Release -o /app

WORKDIR "/src/Api"
RUN dotnet build -c Release -o /app

WORKDIR "/src/Cron"
RUN dotnet build -c Release -o /app

WORKDIR "/src/Test"
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

#Schedule Task
ADD crontab.txt /app/crontab.txt
ADD script.sh /app/script.sh
COPY entry.sh /app/entry.sh
RUN chmod 755 /app/script.sh /app/entry.sh
RUN /usr/bin/crontab /app/crontab.txt
RUN touch /var/log/script.log

ENTRYPOINT ["/app/entry.sh"]