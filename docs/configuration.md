# Configuracao do ambiente

Siga os passos abaixo para executar o projeto localmente;

1. [Entenda o ambiente](#ambiente)
2. [Execute com Docker](#execucao-com-docker)
3. [Execute com dotnet run](#execucao-com-dotnet-run)
4. [Defina configurações locais, se precisar](#configuracoes-locais)

## Ambiente

Em execução local, a API e o Web usam `ASPNETCORE_ENVIRONMENT=Development`.

Nesse ambiente, o projeto já vem pronto para desenvolvimento com estes valores padrão:

- API HTTP em `http://localhost:5020`
- Web HTTP em `http://localhost:5220`
- SQL Server em `localhost:1433`
- Banco `GoodHamburgerDb`
- User `sa`
- Senha `GoodHamburger@12345`
- JWT local de desenvolvimento já configurado

Ao iniciar a API em `Development`, ela aplica as migrations automaticamente e executa o seed inicial do Identity.
Para mais detalhes sobre usuários, login e roles, veja [Autenticação e Autorização](./auth.md).

Usuários criados automaticamente:

- `admin@goodhamburger.com`
- `manager@goodhamburger.com`
- `attendant@goodhamburger.com`

Senha padrão dos usuários:

- `S3cr3tP@ssw0rd`

O `docker compose` também está configurado para rodar em `Development`.

## Execução com Docker

Certifique-se de que o Docker está em execução e rode na raiz do projeto:

```powershell
docker compose up --build
```

Essa opção sobe:

- SQL Server
- API
- Web

Acesse:

- Swagger: http://localhost:18080/swagger
- Web: http://localhost:18081

Se quiser customizar a senha do SQL Server ou a chave JWT do compose, crie um arquivo `.env` na raiz com este conteúdo e ajuste os valores:

```shell
JWT_SECRET_KEY=GoodHamburger.SecretKey.Jwt.Local.Test.2026.With.More.Than.64.Characters
SQL_PASSWORD=SuaSenha
```

## Execução com `dotnet run`

### Opção recomendada para desenvolvimento

Se quiser depurar API e Web localmente, suba apenas o banco com Docker:

```powershell
docker compose up sqlserver -d
```

Essa opção faz mais sentido quando o objetivo é colocar breakpoints, acompanhar logs da API e iterar no código localmente.
Esse fluxo depende de um SQL Server acessível em `localhost:1433`; se o banco não estiver rodando, a API não sobe porque aplica migrations automaticamente no startup.

### API

Rode o comando a partir da raiz do repositório:

```powershell
dotnet run --project .\src\GoodHamburger.Api
```

Swagger:

- http://localhost:5020/swagger

### Web

Em outro terminal:

```powershell
dotnet run --project .\src\GoodHamburger.Web
```

Acesse:

- Web: http://localhost:5220

## Configurações locais

Voce nao precisa alterar nenhum arquivo para fazer a primeira execução.

No fluxo convencional do ASP.NET Core, o projeto lê por padrão:

- `appsettings.json`
- `appsettings.Development.json`
- variáveis de ambiente
- `user-secrets` no projeto da API em `Development`

Ou seja, para sobrescrever configurações locais sem mexer em arquivos versionados, o caminho recomendado aqui é usar variáveis de ambiente e, no caso da API, `dotnet user-secrets`.

Exemplo com variáveis de ambiente no PowerShell para a API:

```powershell
$env:ConnectionStrings__Default = "Server=localhost,1433;Database=GoodHamburgerDb;User Id=sa;Password=SuaSenhaAqui;TrustServerCertificate=True"
$env:Jwt__SecretKey = "SuaChaveJwtLocalComMaisDe64Caracteres"
$env:App__UseHttpsRedirection = "false"
$env:Seed__DefaultPassword = "OutraSenhaForte@123"
```

Exemplo com variável de ambiente para o Web:

```powershell
$env:Api__BaseUrl = "http://localhost:5020"
```

Exemplo com `user-secrets` para a API:

```powershell
dotnet user-secrets set "ConnectionStrings:Default" "Server=localhost,1433;Database=GoodHamburgerDb;User Id=sa;Password=SuaSenhaAqui;TrustServerCertificate=True" --project .\src\GoodHamburger.Api
dotnet user-secrets set "Jwt:SecretKey" "SuaChaveJwtLocalComMaisDe64Caracteres" --project .\src\GoodHamburger.Api
```

No Docker, o `docker compose` já injeta as variáveis de ambiente diretamente nos containers, que é o comportamento padrão esperado pelo ASP.NET Core.
