# Decisões de arquitetura

- [Clean Arch](#clean-architecture)
- [Regras de Negócio](#regras-de-negocio)
- [API](#api)
- [Persistência](#persistencia)
- [Frontend](#frontend)
- [Testes](#testes)

#### Clean Architecture
O projeto foi organizado em Clean Architecture para preservar o domínio e as regras de negócio de dependências externas.

- **Domain**: entidades e regras de negócio.
- **Application**: casos de uso, contratos (interfaces), dtos.
- **Infra.Data**: persistência de dados (EF Core, repositórios, migrations).
- **Infra.Identity**: autenticação e autorização (JWT/ASP.NET Core Identity).
- **CrossCutting.IoC**: composição de dependências.
- **API**: Servidor REST (controllers, validação, autenticação HTTP, middleware e Swagger).
- **Web**: Cliente frontend com Blazor Server.

![Desenho da Arquitetura](./images/mermaid-diagram-clean-arch.png)

A API atua como ponto de entrada da aplicação, delegando a execução para os casos de uso da camada Application.
A camada Application orquestra o fluxo da aplicação, aplicando regras de negócio definidas no Domain e dependendo apenas de contratos (interfaces).
A infraestrutura implementa esses contratos, garantindo o desacoplamento entre regras de negócio e detalhes técnicos, como banco de dados ou autenticação.
As dependências sempre apontam para o Core (Application e Domain), seguindo o princípio da inversão de dependência. Dessa forma, o núcleo da aplicação permanece independente de frameworks e tecnologias externas.

#### Regras de Negócio
As regras de desconto e restrição de categorias foram colocadas no domínio porque representam comportamento central do negócio, não detalhes de API ou banco de dados.

A lógica ficou bem concentrada no domínio do pedido, principalmente na entidade `Order`.
A camada de aplicação só busca `Order` e `Product` no repositório e delega a regra de negócio para a entidade.

##### Como o pedido funciona

A manipulação real dos itens acontece pelos métodos da própria entidade `Order`: `AddItem`, `UpdateItemQuantity`, `RemoveItem` e `CalculateTotalPrice`.

Quando um item é adicionado, o fluxo é:

- Validar o novo item em `ValidateNewItem`.
- Criar um `OrderItem` com o produto, categoria e preço atual do produto.
- Validar a quantidade no próprio `OrderItem`.
- Adicionar na lista e recalcular subtotal, desconto e total.

##### Regra de desconto
Está em `CalculateDiscountRate`.

- Burger + Side + Drink: desconto de 20%.
- Burger + Drink: desconto de 15%.
- Burger + Side: desconto de 10%.
- Qualquer outra combinação: 0%.

Depois calcula:

- `SubTotal = soma de todos os LineTotal`
- `DiscountRate = taxa calculada`
- `Total = SubTotal * (1 - DiscountRate)`

##### Interpretação da regra de itens duplicados

O código proíbe duas coisas ao adicionar item:

- Repetir o mesmo produto no pedido.
- Repetir a mesma categoria no pedido.

Na prática isso significa:

- Pode existir 1 item de X-Burguer com quantidade 3.
- Não pode existir dois itens separados do mesmo produto.
- Não pode existir dois X-Burguer diferentes no mesmo pedido.
- O mesmo raciocínio vale para acompanhamento e bebida.

Ou seja, o modelo aceita multiplicidade por `Quantity`, mas não por duplicação de linhas de item.

##### Decisões adicionais da modelagem
Após o pedido ter sido criado, é possível alterar o mesmo.
A cada alteração é recalculado todo o pedido, ou seja, não há acúmulo de desconto ou regras de negócio que dependam do histórico de alterações. 
O pedido é sempre recalculado a partir do estado atual dos itens.

Um detalhe importante da modelagem:

Quando o item entra no pedido, o preço do produto é copiado. Isso preserva o preço praticado no momento do pedido, mesmo que o cadastro do produto mude depois.

#### API
A API é RESTful, seguindo padrões de verbos HTTP e status codes.
Endpoints versionados com `api/v1/...`.
Documentação automática via Swagger e com padrões de XML para descrição de endpoints, parâmetros e modelos.

A segurança é feita via JWT, com ASP.NET Core Identity para gerenciamento de usuários e roles.
Para mais informações sobre autenticação e autorização, veja [Autenticação e Autorização](./auth.md).

##### Cardápio como Produto

O desafio pede endpoint para consultar o cardápio. 
No projeto, o cardápio foi modelado como produtos ativos em `GET /api/v1/products`, evitando duplicar conceitos entre Menu e Product.

##### Atualização granular de pedidos
Em vez de substituir um pedido inteiro com um único `PUT`, a API oferece operações granulares para adicionar item, alterar quantidade, remover item e alterar status.
Essa escolha reduz ambiguidade e protege melhor as invariantes do domínio.

#### Persistência
A persistência é feita com Entity Framework Core e SQL Server.
O EF é responsável por criar o BD baseado nas entidades e nas configurations.
As configurações do EF Core estão separadas por entidade, seguindo o padrão de `IEntityTypeConfiguration<T>`.
As migrations ficam na infraestrutura, mantendo o domínio livre de detalhes de implementação.

#### Frontend
A UI consome a API via HTTP e não chama Application diretamente.
Os contracts usados pelo client ficam no próprio projeto Web para evitar acoplamento direto com camadas internas da API.
A segurança real permanece na API; a UI apenas esconde/mostra menus conforme a role.
A sessão atual é em memória, suficiente para o escopo do desafio. Persistência de token/refresh token ficou fora do escopo.
Para desenvolvimento de telas foi utilizado a biblioteca de componentes MudBlazor, que é leve e fácil de usar com Blazor Server.
Para mais detalhes sobre a UI, veja [UI (Blazor)](./ui.md).

#### Testes
Foram implementados testes unitários para todo o domínio, cobrindo regras de negócio e validações.
O Workflow de CI do projeto testa a aplicação e gera um summary de cobertura no GitHub Actions.
A cobertura do `GoodHamburger.Domain` é tratada como gate de qualidade, com threshold configurado em 100%.

