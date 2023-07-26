FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /src
COPY ["Domain", "Domain/"]
COPY ["Infrastructure", "Infrastructure/"]
COPY ["Authentication", "Authentication/"]
COPY ["Application", "Application/"]
COPY ["WebApi3", "WebApi/"]
RUN dotnet restore "WebApi/WebApi.csproj"
#COPY "/WebApi3" "WebApi"
WORKDIR "/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]