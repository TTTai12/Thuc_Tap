FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ThucTap/TT_ECommerce.csproj ThucTap/
RUN dotnet restore ThucTap/TT_ECommerce.csproj

COPY . .
RUN dotnet publish ThucTap/TT_ECommerce.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 10000
ENTRYPOINT ["sh","-c","dotnet TT_ECommerce.dll --urls http://0.0.0.0:${PORT:-10000}"]
