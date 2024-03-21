FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
USER app
WORKDIR /app
EXPOSE 5000 
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
COPY . .
RUN dotnet restore
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM  build as publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/out

FROM  base as final
WORKDIR /app
COPY --from=publish /app/out .
ENTRYPOINT ["dotnet", "RinhaBackend.dll"]