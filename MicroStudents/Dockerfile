# iniciamos estableciendo la base del sdk a usar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-base

#establecemos el directorio de trabajo llamado "app"
WORKDIR /app

#copiamos los archivos del proyecto en el directorio de trabajo
COPY *.csproj ./

#restauramos todo lo que haga falta
RUN dotnet restore

#copiamos todo el contenido del proyecto en el directorio de trabajo
COPY . ./

#compilamos y publicamos el proyecto en modo release en una carpeta llamada "out"
RUN dotnet publish -c Release -o out

#establecemos la base del runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

#establecemos el directorio de trabajo
WORKDIR /app

#copiamos el contenido de la carpeta out del build-base en el directorio de trabajo
COPY --from=build-base /app/out .

#ejecutamos el proyecto
ENTRYPOINT ["dotnet", "MicroStudents.dll"]
