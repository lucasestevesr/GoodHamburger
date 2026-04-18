## Clean Arch

O projeto é desenvolvido seguindo a arquitetura limpa, onde as camadas são organizadas de forma a promover a separação 
de responsabilidades e facilitar a manutenção do código. 


A estrutura do projeto é composta pelas seguintes camadas:

Core
- **Domain**: Contém as entidades, interfaces e regras de negócio do sistema. 
Esta camada é independente de qualquer tecnologia ou framework específico.

- **Application**: Contém os casos de uso e a lógica de aplicação. 
Esta camada é responsável por orquestrar as operações do sistema, utilizando as entidades e interfaces definidas na camada Domain.

Infrastructure
- **Infra.Data**: Contém a implementação de acesso a dados, como repositórios e mapeamento de entidades para o banco de dados. 
Esta camada é responsável por interagir com o banco de dados e fornecer os dados necessários para as camadas superiores.

Presentation
- **API**: Contém a implementação da API RESTful, incluindo os controladores, rotas e modelos de I/O. 
Esta camada é responsável por expor os serviços do sistema para os clientes.


Essa organização em camadas permite uma melhor modularização do código,
facilitando a manutenção, testes e evolução do sistema ao longo do tempo.

As referências seguem,

Camadas externas:
GoodHamburger.Infra.Data referenciando GoodHamburger.Core.Application
GoodHamburger.API referenciando GoodHamburger.Core.Application
GoodHamburger.API referenciando GoodHamburger.Infra.~
Camadas mais internas:
GoodHamburger.Application referenciando GoodHamburger.Core.Domain.

Domain: Zero dependências.
Application: Depende de Domain.
Infra: Depende de Application.
API: Depende de Application e Infra.
