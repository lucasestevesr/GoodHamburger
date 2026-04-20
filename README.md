# GoodHamburger


Migrations

```dotnet ef migrations add InitialDataModel --project src/GoodHamburger.Infra.Data --startup-project src/GoodHamburger.Api --context AppDbContext --output-dir Migrationsdotnet ef migrations add InitialDataModel --project src/GoodHamburger.Infra.Data --startup-project src/GoodHamburger.Api --context AppDbContext --output-dir Migrations```

```dotnet ef database update --project src/GoodHamburger.Infra.Data --startup-project src/GoodHamburger.Api --context AppDbContextdotnet ef database update --project src/GoodHamburger.Infra.Data --startup-project src/GoodHamburger.Api --context AppDbContext```