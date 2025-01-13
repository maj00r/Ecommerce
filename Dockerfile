FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Ecommerce.csproj", "./"]
RUN dotnet restore "Ecommerce.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Ecommerce.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Ecommerce.dll"]
