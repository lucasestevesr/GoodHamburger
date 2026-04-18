#### Clean Architecture

O projeto segue os princípios de Clean Architecture, organizando as camadas para garantir separação de responsabilidades, baixo acoplamento e alta testabilidade.

##### Camadas

##### Core

- Domain: Entidades e regras de negócio. Não depende de nenhuma outra camada.
- Application: Casos de uso e orquestração da lógica. Depende apenas do Domain.

##### Infrastructure

- Infra.Data: Implementações de acesso a dados Depende da Application.

##### Presentation

- API: Exposição dos endpoints REST e entrada/saída da aplicação. Depende da Application e da Infrastructure.

##### Dependências
- Domain → sem dependências
- Application → depende de Domain
- Infrastructure → depende de Application
- API → depende de Application e Infrastructure

Essa organização garante isolamento das regras de negócio, facilitando manutenção, testes e evolução do sistema.
