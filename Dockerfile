
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /srv/endpoint

COPY *.csproj .
RUN dotnet restore -s https://api.nuget.org/v3/index.json

COPY . .
RUN dotnet public -c Release -o /bin/app
RUN dotnet publish "Ecommerce.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /srv/endpoint
COPY --from=build /srv/endpoint/bin/app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Ecommerce.dll"]
