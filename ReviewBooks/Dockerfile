# Imagen base de .NET Core SDK para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar archivos de proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código y construir la aplicación en modo Release
COPY . ./
RUN dotnet publish -c Release -o out

# Imagen base de .NET Core Runtime para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS=http://+:8080

# Exponer el puerto en el contenedor
EXPOSE 80

# Comando para iniciar la aplicación
ENTRYPOINT ["dotnet", "ReviewBooks.dll"]
