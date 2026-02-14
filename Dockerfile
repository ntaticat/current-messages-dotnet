FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY WebAPI/WebAPI.csproj WebAPI/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Persistence/Persistence.csproj Persistence/

RUN dotnet restore WebAPI/WebAPI.csproj

COPY . .

RUN dotnet publish WebAPI/WebAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet", "WebAPI.dll" ]

