FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY GoodHamburger.sln ./
COPY src/GoodHamburger.Api/GoodHamburger.Api.csproj src/GoodHamburger.Api/
COPY src/GoodHamburger.Application/GoodHamburger.Application.csproj src/GoodHamburger.Application/
COPY src/GoodHamburger.Domain/GoodHamburger.Domain.csproj src/GoodHamburger.Domain/
COPY src/GoodHamburger.Infra.Data/GoodHamburger.Infra.Data.csproj src/GoodHamburger.Infra.Data/
COPY src/GoodHamburger.Infra.Identity/GoodHamburger.Infra.Identity.csproj src/GoodHamburger.Infra.Identity/
COPY src/GoodHamburger.CrossCutting.IoC/GoodHamburger.Infra.CrossCutting.IoC.csproj src/GoodHamburger.CrossCutting.IoC/
COPY src/GoodHamburger.Web/GoodHamburger.Web.csproj src/GoodHamburger.Web/
COPY tests/GoodHamburger.Tests/GoodHamburger.Tests.Unit.csproj tests/GoodHamburger.Tests/

RUN dotnet restore GoodHamburger.sln

COPY . .
RUN dotnet publish src/GoodHamburger.Api/GoodHamburger.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "GoodHamburger.Api.dll"]
