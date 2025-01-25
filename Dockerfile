FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /srv/endpoint

COPY *.csproj ./
RUN dotnet restore -s https://api.nuget.org/v3/index.json

COPY . .
RUN dotnet publish -c Release -o /bin/app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /srv/endpoint
COPY --from=build /bin/app ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "Ecommerce.dll"]
