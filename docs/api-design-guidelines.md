# Guidelines do projeto

Este documento descreve padrões e convenções usadas no projeto, com referências oficiais.

## Convenções

- Versionamento: `api/v1/...` (URL segment).
- DTOs: `Requests` e `Responses` ficam na `Application`.
- Validação e erros: middleware global devolvendo `ProblemDetails`.
- DI: métodos de extensão no `IServiceCollection` por responsabilidade (ex.: `AddInfrastructure`, `AddAuth`).
- EF Core: mappings por `IEntityTypeConfiguration<T>`.
- Controllers finas; a regra de negócio fica no domínio e a orquestração na `Application`.
- Sempre passe `CancellationToken` para queries, comandos e integrações externas.

### Casos de Uso

São implementados no projeto [GoodHamburger.Application](../src/GoodHamburger.Application) seguindo o seguinte padrão:

```text
GoodHamburger.Application
└── [Domínio]
    ├── Requests
    |   └── [CRUD][Entidade]Request.cs
    ├── Responses
    |   └── [Entidade]Response.cs
    ├── Services
    |   └── [Entidade]Service.cs
    ├── Interfaces
    |   └── I[Entidade][Destino do Contrato]*.cs
    └── Mappings
        └── [Domínio]Mappings.cs
    
    * Repositories, Services ou outros contratos necessários para a implementação do caso de uso. Ex.: `IProductRepository`, `IAuthService`, etc.
```
- **Request**
  - É a requisição recebida pela controller.
- **Response**
  - É a resposta retornada à controller.
- **Service**
  - Executa a operação seguindo as regras de negócio.
  - Orquestra repositórios, serviços externos e entidades de domínio.
- **Interfaces**
  - Contrato do serviço, repositório ou outro componente necessário para a implementação do caso de uso.
  - Necessário para respeitar a inversão de dependências e manter o domínio livre de acoplamento a detalhes de implementação.
- **Mappings**
  - Configura o mapeamento entre as entidades e os DTOs de request e response.
  - Feito manualmente para evitar dependências externas, como AutoMapper, e manter controle total sobre o processo de mapeamento, visto simplicidade do modelo atual.

## API REST

- Endpoints versionados em `api/v1`.
- Uso consistente de verbos HTTP e status codes.
- XML comments e Swagger para documentação dos endpoints.
- Respostas de erro padronizadas com `ProblemDetails`.

## O que não fica aqui

- Regras de negócio detalhadas: veja [Decisões de Arquitetura](./architecture.md).
- Execução local: veja [Configuração e execução local](./configuration.md).
- Login, roles e policies: veja [Autenticação e Autorização](./auth.md).

## Referências (Microsoft)

- ASP.NET Core Web API: https://learn.microsoft.com/aspnet/core/web-api/
- Swagger/Swashbuckle: https://learn.microsoft.com/aspnet/core/tutorials/getting-started-with-swashbuckle
- Authentication/Authorization: https://learn.microsoft.com/aspnet/core/security/authentication/
- JWT bearer: https://learn.microsoft.com/aspnet/core/security/authentication/jwtbearer
- ProblemDetails: https://learn.microsoft.com/aspnet/core/web-api/handle-errors
- Options pattern: https://learn.microsoft.com/aspnet/core/fundamentals/configuration/options
- EF Core configurations: https://learn.microsoft.com/ef/core/modeling/
- UI, Blazor Server: https://learn.microsoft.com/aspnet/core/blazor/server
- Mudblazor: https://mudblazor.com/
