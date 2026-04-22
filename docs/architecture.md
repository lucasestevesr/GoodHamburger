# Decisões de arquitetura

O projeto foi organizado em Clean Arch;

- `Domain`: entidades e regras de negócio.
- `Application`: services, interfaces e DTOs.
- `Infra.Data`: EF Core, repositórios, migrations, seeds e serviços técnicos de autenticação.
- `CrossCutting.IoC`: registro de dependências por responsabilidade.
- `Api`: controllers, Swagger, autenticação HTTP, autorização e middleware de erro.

```mermaid
flowchart LR
  %% Camadas externas (consomem o core)
  BLAZOR(["Blazor UI"])
  API(["API"])
  IOC(["IoC"])
  INFRA(["Infra"])
  DB[("SQL Server")]

  %% CORE como destaque central
  subgraph COREBOX["CORE"]
    direction TB
    APP(("Application"))
    DOMAIN(("Domain"))
  end

  %% Fluxo principal
  BLAZOR -->|"HTTP"| API

  %% DI
  API -.->|"configura DI"| IOC
  IOC -.->|"services"| APP
  IOC -.->|"repositories"| INFRA

  %% Dependências
  API -->|"usa"| APP
  APP -->|"regras"| DOMAIN
  INFRA -.->|"implementa interfaces"| APP
  INFRA -->|"SQL"| DB

  %% Estilo
  style COREBOX fill:#0b1320,stroke:#60a5fa,stroke-width:2px,color:#e5e7eb

  style APP fill:#38B6FF,stroke:#1d4ed8,stroke-width:2px,color:#000
  style DOMAIN fill:#38B6FF,stroke:#1d4ed8,stroke-width:2px,color:#000

  style BLAZOR fill:#e5e7eb,stroke:#64748b,color:#000
  style API fill:#e5e7eb,stroke:#64748b,color:#000
  style IOC fill:#e5e7eb,stroke:#64748b,color:#000
  style INFRA fill:#e5e7eb,stroke:#64748b,color:#000
  style DB fill:#fde68a,stroke:#f59e0b,color:#000
```

A API atua como camada de entrada e invoca os services.
Os services orquestram a aplicação e aplicam regras do domínio.
A infraestrutura implementa interfaces definidas na Application, seguindo o princípio da inversão de dependência.
Assim, o Core não depende de nenhuma tecnologia externa.
