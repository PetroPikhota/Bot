#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
ARG BUILD_DATE
ENV BUILD_DATE=${BUILD_DATE:-}

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Bot_start.csproj", "."]
RUN dotnet restore "./Bot_start.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Bot_start.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bot_start.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bot_start.dll"]