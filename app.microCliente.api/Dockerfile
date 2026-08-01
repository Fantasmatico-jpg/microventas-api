FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore app.microCliente.api/app.microCliente.api.csproj

RUN dotnet publish \
    app.microCliente.api/app.microCliente.api.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_HTTP_PORTS=8081

EXPOSE 8081

ENTRYPOINT ["dotnet", "app.microCliente.api.dll"]