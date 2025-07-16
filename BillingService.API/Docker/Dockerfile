FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln ./
COPY BillingService.API/*.csproj ./BillingService.API/
COPY BillingService.Testes/*.csproj ./BillingService.Testes/
COPY BillingService.Domain/*.csproj ./BillingService.Domain/
COPY BillingService.Application/*.csproj ./BillingService.Application/
COPY BillingService.Infrastructure/*.csproj ./BillingService.Infrastructure/
RUN dotnet restore

COPY . .
RUN dotnet publish BillingService.API/BillingService.API.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "BillingService.API.dll"]