# Etapa 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o .csproj e restaura
COPY ManagerTruck.Web.csproj ./
RUN dotnet restore ManagerTruck.Web.csproj

# Copia o restante dos arquivos
COPY . .

# Publica a aplicação
RUN dotnet publish -c Release -o /app/publish

# Etapa 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Porta usada pelo Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ManagerTruck.Web.dll"]
