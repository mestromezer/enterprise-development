FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln ./
COPY Pharmacies.Application/*.csproj ./Pharmacies.Application/
COPY Pharmacies.Domain/*.csproj ./Pharmacies.Domain/
COPY Pharmacies.EntityFramework/*.csproj ./Pharmacies.EntityFramework/
COPY Pharmacies.Host/*.csproj ./Pharmacies.Host/
COPY Pharmacies.Domain.Tests/*.csproj ./Pharmacies.Domain.Tests/

RUN dotnet restore

COPY . .
WORKDIR /src/Pharmacies.Host

RUN dotnet publish -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "Pharmacies.Host.dll"]
