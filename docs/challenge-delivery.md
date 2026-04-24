# desafio GoodHamburger STgenetics

Resumo do que foi implementado para o desafio técnico descrito em `docs/challenge_dotnet_stgen.pdf`.

## Escopo solicitado

O desafio pede uma API REST em C#/.NET para registrar pedidos da GoodHamburger, contemplando:

- CRUD completo de pedidos. [V]
- Cálculo de subtotal, desconto e total final. [V]
- Regras de desconto por combinação de itens. [V]
- Validação de itens duplicados e pedidos inválidos. [V]
- Respostas claras para erros. [V]
- Endpoint para consulta do cardápio. [V]
- Diferenciais opcionais: frontend em Blazor e testes automatizados das regras de negócio. [V]

## Como o escopo foi atendido

- O CRUD de pedidos foi implementado com criação, listagem, consulta, alteração de status, adição de item, alteração de quantidade, remoção de item e exclusão do pedido.
- O cálculo de subtotal, desconto e total final fica concentrado no domínio do pedido.
- As regras de desconto implementadas seguem o enunciado:
  - Sanduíche + batata + refrigerante: 20%.
  - Sanduíche + refrigerante: 15%.
  - Sanduíche + batata: 10%.
- A validação de itens duplicados foi interpretada da seguinte forma:
  - o pedido aceita quantidade maior que 1 no mesmo item;
  - o pedido não aceita duas linhas para o mesmo produto;
  - o pedido não aceita dois produtos da mesma categoria.
- Respostas de erro são retornadas de forma clara, com `ProblemDetails` e mensagens de domínio.
- O cardápio foi modelado como produtos ativos expostos pela API.
- O frontend foi implementado em Blazor com a utilização da biblioteca de componentes MudBlazor.
- Testes automatizados cobrem o domínio e validam as principais regras de negócio.

Para mais detalhes sobre essas decisões, veja [Decisões de Arquitetura](./architecture.md).

## O que foi feito além do escopo

- Autenticação e autorização com JWT e ASP.NET Core Identity, com perfis de acesso para diferentes operações.
- Documentação automática da API com Swagger e XML comments.
- Novas regras de negócio, como proibição de produtos inativos e atualização granular de pedidos.
- CRUDs completos para produtos e usuários, além de endpoints administrativos.

## O que ficou de fora

Para o escopo do teste técnico, poderiam ter sido feitos mas foram deixados como evolução opcional:

- Testes unitários de controllers, serviços e outros componentes além do domínio;
- Testes de integração HTTP e testes end-to-end não foram adicionados.
- Telemetria com Serilog, OpenTelemetry, tracing distribuído e CorrelationId;
- Refresh token, recuperação de senha, confirmação real de e-mail e provedores externos de login(OpenID).
