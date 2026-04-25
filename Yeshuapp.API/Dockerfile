# =========================
# Etapa de build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia só o csproj primeiro (melhor uso de cache)
COPY Yeshuapp.API.csproj ./
RUN dotnet restore "Yeshuapp.API.csproj"

# Agora copia o restante do código
COPY . .

# Publica a aplicação
RUN dotnet publish "Yeshuapp.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# =========================
# Etapa final (runtime)
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia os arquivos publicados
COPY --from=build /app/publish .

# Configuração da porta (Render usa 8080)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Sobe a aplicação
ENTRYPOINT ["dotnet", "Yeshuapp.API.dll"]