# FIAP Cloud Games - Fase 1

API REST em .NET 8 para cadastro de usuários, autenticação JWT e biblioteca de jogos adquiridos.

## Requisitos implementados

- Cadastro de usuários com nome, e-mail e senha.
- Validação de e-mail e senha forte: mínimo de 8 caracteres, letras, números e caracteres especiais.
- Autenticação com JWT.
- Autorização por perfil:
  - `User`: consulta jogos e gerencia sua biblioteca.
  - `Admin`: cadastra jogos e administra usuários.
- Monolito em .NET 8 usando Minimal APIs.
- Entity Framework Core com SQLite.
- Migration inicial incluída.
- Middleware para tratamento de erros e logs estruturados via console logging.
- Swagger habilitado.
- Testes unitários com xUnit.

## O que foi ignorado por ser opcional

- MongoDB.
- Dapper.
- GraphQL.
- Domain Storytelling.

## Como rodar

Pré-requisitos:

- .NET SDK 8 instalado.

Comandos:

```bash
cd src/Fcg.Api
dotnet restore
dotnet run
```

A API abrirá o Swagger em:

```text
https://localhost:5001/swagger
http://localhost:5000/swagger
```

Dependendo do perfil local do .NET, a porta pode mudar. Confira o terminal após executar `dotnet run`.

## Usuário administrador inicial

Ao iniciar, a aplicação aplica migrations e cria um usuário admin se ainda não existir:

```text
E-mail: admin@fcg.com
Senha: Admin@123
```

## Fluxo básico de teste

1. Fazer login em `POST /api/auth/login` com o admin.
2. Copiar o token retornado.
3. No Swagger, clicar em `Authorize` e informar: `Bearer {token}`.
4. Cadastrar jogos em `POST /api/games`.
5. Criar usuário comum em `POST /api/auth/register` ou pelo admin em `POST /api/users`.
6. Login como usuário comum.
7. Comprar/adicionar jogo à biblioteca em `POST /api/games/purchase`.
8. Consultar biblioteca em `GET /api/users/me/library`.

## Endpoints principais

### Auth

- `POST /api/auth/register`
- `POST /api/auth/login`

### Games

- `GET /api/games`
- `POST /api/games` - somente Admin
- `POST /api/games/purchase` - User ou Admin autenticado

### Users

- `GET /api/users/me/library`
- `GET /api/users` - somente Admin
- `POST /api/users` - somente Admin

## Testes

```bash
dotnet test
```

## Observações sobre DDD

O projeto separa domínio, aplicação, infraestrutura, contratos, endpoints e middleware. As entidades `User`, `Game` e `UserGame` concentram regras essenciais de domínio.

Sugestão para documentação DDD no Miro:

### Event Storming - Criação de usuários

- Comando: `Cadastrar usuário`.
- Evento: `Usuário cadastrado`.
- Regras: e-mail válido, senha forte, e-mail único.
- Agregado: `User`.

### Event Storming - Criação de jogos

- Comando: `Cadastrar jogo`.
- Evento: `Jogo cadastrado`.
- Regras: apenas admin, título obrigatório, preço não negativo.
- Agregado: `Game`.

### Event Storming - Aquisição de jogo

- Comando: `Adicionar jogo à biblioteca`.
- Evento: `Jogo adquirido`.
- Regras: usuário autenticado, jogo existente, jogo ainda não adquirido.
- Agregado: `UserGame`.
