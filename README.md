# GoodHamburger

## Migrations

```bash
dotnet ef migrations add InitialDataModel --project src/GoodHamburger.Infra.Data --startup-project src/GoodHamburger.Api --context AppDbContext --output-dir Migrations
```

```bash
dotnet ef database update --project src/GoodHamburger.Infra.Data --startup-project src/GoodHamburger.Api --context AppDbContext
```