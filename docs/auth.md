# Autenticação e autorização

A Autenticação é feita via JWT, usando ASP.NET Core Identity para gerenciamento de usuários e roles.
JWT pois é a padrão do mercado e é uma autenticação stateless, ideal para APIs REST, e facilita a integração com clientes web.
Também é uma escolha mais simples e leve do que OAuth2, que seria mais indicado para cenários de SSO ou delegação de acesso.

A Autorização é feita via ASP.NET Core Identity usado para usuários, roles e senha. 
Foi configurado perfis de acesso separados por roles: Admin, Manager e Attendant.
Assim, foi implementado políticas de autorização que protegem operações administrativas e operações de pedido.

Em desenvolvimento, o projeto já traz uma chave JWT local pronta para uso.
Os detalhes de configuração local com variáveis de ambiente, `user-secrets` e uso com Docker estão em [Configuração e execução local](./configuration.md).

#### Login

Endpoint:

- `POST /api/v1/auth/login`

Exemplo de request:

```json
{
  "email": "admin@goodhamburger.com",
  "password": "S3cr3tP@ssw0rd"
}
```

#### Usar o token

Envie o header:

- `Authorization: Bearer <token>`

Na UI Web, esse token é mantido em memória para consumo da API.
Na API, as permissões são validadas pelas policies configuradas para cada operação.

#### Perfis de acesso

Usuários:

- `admin@goodhamburger.com`
- `manager@goodhamburger.com`
- `attendant@goodhamburger.com`

Senha (para os 3):

- `S3cr3tP@ssw0rd`

#### Roles

- `Admin`
- `Manager`
- `Attendant`

#### Policies

- `OrderManagement`: `Attendant`, `Manager`, `Admin` 
Essa policy protege endpoints de criação, atualização e consulta de pedidos.
No caso desse sistema, todos os perfis podem gerenciar pedidos.

- `ProductManagement`: `Manager`, `Admin`
Sobre essa policy, apenas Manager e Admin podem gerenciar produtos do cardápio.

- `CreateAttendantManagement`: `Manager`, `Admin`
No gerenciamento de usuários, Manager também pode criar atendentes, mas não pode criar outros Managers ou Admins.

- `UserManagement`: `Admin`
Admin pode gerenciar todos os usuários, incluindo outros Admins e Managers.
