FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY Rezerv.WhatsApp.slnx ./
COPY Rezerv.WhatsApp.Api/Rezerv.WhatsApp.Api.csproj Rezerv.WhatsApp.Api/
COPY Rezerv.WhatsApp.Application/Rezerv.WhatsApp.Application.csproj Rezerv.WhatsApp.Application/
COPY Rezerv.WhatsApp.Domain/Rezerv.WhatsApp.Domain.csproj Rezerv.WhatsApp.Domain/
COPY Rezerv.WhatsApp.Infrastructure/Rezerv.WhatsApp.Infrastructure.csproj Rezerv.WhatsApp.Infrastructure/

RUN dotnet restore Rezerv.WhatsApp.slnx

COPY . .

RUN dotnet publish Rezerv.WhatsApp.Api/Rezerv.WhatsApp.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Rezerv.WhatsApp.Api.dll"]