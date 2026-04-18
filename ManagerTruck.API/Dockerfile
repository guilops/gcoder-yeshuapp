# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia tudo (inclui o .csproj)
COPY . .


# Restaura as dependências
RUN dotnet restore "ManagerTruck.API.csproj"

# Publica o projeto para a pasta /app/publish
RUN dotnet publish "ManagerTruck.API.csproj" -c Release -o /app/publish

# Etapa final de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copia o resultado da publicação do build
COPY --from=build /app/publish .

# Define a porta exposta e URL de escuta
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Define o comando de entrada
ENTRYPOINT ["dotnet", "ManagerTruck.API.dll"]
