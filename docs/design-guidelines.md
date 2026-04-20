#### Clean Architecture

Foi escolhido a arquitetura Clean Architecture pensando na escalabilidade do projeto, 
visto o baixo acoplamento à dependências externas e alta testabilidade.

##### Camadas

##### Core

- Domain: Entidades e regras de negócio. Não depende de nenhuma outra camada.
- Application: Casos de uso e orquestração da lógica. Depende apenas do Domain.

##### Infrastructure

- Infra.Data: Implementações de acesso a dados. Depende da Application e de Domain.
- Infra.CrossCutting.IoC: Configuração de injeção de dependências.
##### Presentation

- API: Exposição dos endpoints REST e entrada/saída da aplicação. Depende da Application e da CrossCutting.IoC.

##### Dependências
- Domain → sem dependências
- Application → depende de Domain
- Infrastructure → depende de Application e Domain
- API → depende de Application e CrossCutting.IoC

