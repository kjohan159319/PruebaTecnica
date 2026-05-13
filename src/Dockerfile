FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/GestionUsuarios/GestionUsuarios.csproj", "src/GestionUsuarios/"]
RUN dotnet restore "src/GestionUsuarios/GestionUsuarios.csproj"
COPY . .
WORKDIR "/src/src/GestionUsuarios"
RUN dotnet build "GestionUsuarios.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GestionUsuarios.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GestionUsuarios.dll"]
