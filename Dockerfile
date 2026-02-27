FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TT_ECommerce.csproj ./
RUN dotnet restore ./TT_ECommerce.csproj

COPY . .
RUN dotnet publish ./TT_ECommerce.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 10000
ENTRYPOINT ["sh","-c","dotnet TT_ECommerce.dll --urls http://0.0.0.0:${PORT:-10000}"]
