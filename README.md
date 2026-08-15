# 🔐 Autenticação em Minimal API

Projeto desenvolvido para demonstrar a criação de uma **Minimal API com ASP.NET Core 8** e a implementação de **autenticação e autorização utilizando JWT (JSON Web Token)**.

A aplicação possui um endpoint de login que gera um token JWT. Esse token é utilizado para acessar endpoints protegidos da API.

O projeto também possui integração com **Swagger**, permitindo informar o Bearer Token diretamente pela interface e testar os endpoints protegidos.

---

## 📚 Sobre o projeto

Neste projeto são abordados:

* Criação de uma Minimal API com ASP.NET Core 8
* Autenticação utilizando JWT Bearer
* Geração de tokens JWT
* Validação de tokens
* Claims de usuário
* Roles
* Proteção de endpoints com `RequireAuthorization()`
* Configuração de autenticação e autorização
* Personalização da resposta `401 Unauthorized`
* Integração do JWT com Swagger
* Utilização do Entity Framework Core
* Banco de dados em memória para testes

---

## 🚀 Tecnologias utilizadas

* **.NET 8**
* **ASP.NET Core**
* **Minimal APIs**
* **C#**
* **JWT Bearer**
* **ASP.NET Core Authentication**
* **ASP.NET Core Authorization**
* **Entity Framework Core**
* **In-Memory Database**
* **Swagger / OpenAPI**

---

## 📁 Estrutura do projeto

Uma possível estrutura do projeto:

```text
AuthMinimalApi/
│
├── Models/
│   ├── LoginRequest.cs
│   ├── Produto.cs
│   └── ProdutoContext.cs
│
├── Program.cs
│
├── appsettings.json
│
└── AuthMinimalApi.csproj
```

A estrutura pode ser adaptada conforme a evolução do projeto.

---

# 🔑 Autenticação com JWT

JWT significa **JSON Web Token** e é um padrão utilizado para transmitir informações de autenticação entre cliente e servidor.

Neste projeto, o usuário realiza o login através do endpoint:

```http
POST /login
```

Após validar usuário e senha, a API gera um token JWT.

O fluxo é:

```text
Cliente
   │
   │ POST /login
   │
   ▼
API
   │
   │ Validação usuário/senha
   │
   ▼
JWT Token
   │
   │
   ▼
Cliente armazena o token
   │
   │ Authorization: Bearer TOKEN
   │
   ▼
Endpoint protegido
   │
   ▼
JWT Middleware
   │
   ├── Token inválido → 401
   │
   └── Token válido
          │
          ▼
       Endpoint
```

---

# 👤 Login

O projeto possui um usuário de teste:

```text
Usuário: usuario_teste
Senha: 12345
```

O endpoint utilizado é:

```http
POST /login
```

Exemplo de requisição:

```json
{
  "username": "usuario_teste",
  "password": "12345"
}
```

Quando as credenciais são válidas, a API retorna um JWT:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

Esse token deverá ser utilizado nas requisições aos endpoints protegidos.

---

# 🛡️ Proteção de endpoints

No ASP.NET Core, um endpoint pode ser protegido utilizando:

```csharp
.RequireAuthorization();
```

Exemplo:

```csharp
app.MapGet("/produtos", async (ProdutoContext db) =>
{
    var produtos = await db.Produtos.ToListAsync();

    return Results.Ok(produtos);
})
.RequireAuthorization();
```

Nesse caso, o endpoint `/produtos` exige que o usuário esteja autenticado.

---

# 🔒 Bearer Token

Para acessar um endpoint protegido, o cliente deve enviar o token através do header HTTP:

```http
Authorization: Bearer SEU_TOKEN
```

Exemplo:

```http
GET /produtos
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

O middleware do ASP.NET Core valida automaticamente:

* Assinatura do token
* Issuer
* Audience
* Data de expiração
* Chave utilizada na assinatura

---

# ⚙️ Configuração do JWT

As configurações do JWT estão no arquivo `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "MinhaChaveJWTSuperSecreta2026@API#123456789",
    "Issuer": "AuthMinimalApi",
    "Audience": "AuthMinimalApiClient"
  }
}
```

### Key

Chave utilizada para assinar e validar o JWT.

Para o algoritmo `HS256`, a chave deve possuir tamanho suficiente para o algoritmo utilizado.

### Issuer

Identifica quem emitiu o token.

### Audience

Identifica para quem o token foi emitido.

---

# 📋 Claims

Durante a criação do token são adicionadas algumas informações sobre o usuário.

Exemplo:

```csharp
var claims = new[]
{
    new Claim(
        JwtRegisteredClaimNames.Sub,
        loginRequest.Username),

    new Claim(
        JwtRegisteredClaimNames.Jti,
        Guid.NewGuid().ToString()),

    new Claim(
        ClaimTypes.Role,
        "Admin")
};
```

Neste projeto temos:

### `Sub`

Identifica o usuário.

```text
usuario_teste
```

### `Jti`

Identificador único do token.

### `Role`

Define a função/permissão do usuário.

Neste exemplo:

```text
Admin
```

---

# 👑 Roles

O JWT também possui uma `Role`:

```csharp
new Claim(ClaimTypes.Role, "Admin")
```

Isso permite posteriormente restringir endpoints de acordo com o perfil do usuário.

Por exemplo:

```csharp
.RequireAuthorization(policy =>
    policy.RequireRole("Admin"));
```

Dessa forma, apenas usuários com a role `Admin` poderão acessar determinado recurso.

---

# 🚫 Resposta 401 personalizada

Quando o usuário tenta acessar um endpoint protegido sem um token válido, a API retorna:

```http
401 Unauthorized
```

Neste projeto a resposta foi personalizada:

```json
{
  "mensagem": "Token não informado ou inválido."
}
```

A personalização é feita através do evento:

```csharp
options.Events = new JwtBearerEvents
{
    OnChallenge = async context =>
    {
        context.HandleResponse();

        context.Response.StatusCode =
            StatusCodes.Status401Unauthorized;

        context.Response.ContentType =
            "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            mensagem = "Token não informado ou inválido."
        });
    }
};
```

Isso permite que a API mantenha um padrão de resposta mais amigável para aplicações frontend e clientes externos.

---

# 📖 Swagger

O projeto possui Swagger configurado para trabalhar com JWT Bearer.

A configuração adiciona um esquema de segurança:

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
```

Com isso, o Swagger disponibiliza o botão:

```text
Authorize 🔒
```

---

# 🧪 Testando pelo Swagger

A forma mais simples de testar a aplicação é através do Swagger.

## 1. Execute a aplicação

```bash
dotnet run
```

Acesse o Swagger:

```text
https://localhost:xxxx/swagger
```

---

## 2. Faça login

No Swagger, execute:

```http
POST /login
```

Utilize:

```json
{
  "username": "usuario_teste",
  "password": "12345"
}
```

A API retornará:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

---

## 3. Autorize o Swagger

Clique no botão:

```text
Authorize 🔒
```

Informe apenas o JWT:

```text
eyJhbGciOiJIUzI1NiIs...
```

Não é necessário informar:

```text
Bearer eyJhbGciOiJIUzI1NiIs...
```

O Swagger adicionará automaticamente o prefixo:

```http
Authorization: Bearer SEU_TOKEN
```

---

## 4. Acesse `/produtos`

Depois de autorizar o Swagger, execute:

```http
GET /produtos
```

Como o endpoint possui:

```csharp
.RequireAuthorization();
```

o token será validado antes da execução da requisição.

Com um token válido:

```http
200 OK
```

Sem token ou com token inválido:

```http
401 Unauthorized
```

---

# 📡 Endpoints

| Método | Endpoint    | Autenticação | Descrição                |
| ------ | ----------- | ------------ | ------------------------ |
| POST   | `/login`    | ❌ Não        | Realiza login e gera JWT |
| GET    | `/produtos` | ✅ Sim        | Lista os produtos        |

---

# 🧪 Testando com cURL

### Login

```bash
curl -X POST "https://localhost:xxxx/login" \
  -H "Content-Type: application/json" \
  -d "{\"username\":\"usuario_teste\",\"password\":\"12345\"}"
```

A resposta será semelhante a:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

### Endpoint protegido

```bash
curl -X GET "https://localhost:xxxx/produtos" \
  -H "Authorization: Bearer SEU_TOKEN"
```

---

# 🔐 Autenticação x Autorização

É importante diferenciar os dois conceitos.

## Autenticação

Responde:

> Quem é o usuário?

Exemplo:

```text
Usuário
   │
   ▼
Login
   │
   ▼
JWT
```

## Autorização

Responde:

> O usuário pode acessar esse recurso?

Exemplo:

```text
Usuário autenticado
        │
        ▼
Possui permissão?
     │       │
    SIM     NÃO
     │       │
     ▼       ▼
   200      403
```

### Códigos utilizados

**401 Unauthorized**

O usuário não está autenticado ou o token é inválido/expirado.

**403 Forbidden**

O usuário está autenticado, mas não possui permissão para acessar o recurso.

---

# 🗄️ Banco de dados

Para simplificar o projeto, foi utilizado o banco de dados em memória do Entity Framework Core:

```csharp
builder.Services.AddDbContext<ProdutoContext>(options =>
    options.UseInMemoryDatabase("ProdutosDb"));
```

Isso permite executar o projeto sem a necessidade de configurar MySQL, SQL Server ou outro banco de dados externo.

Os dados são mantidos apenas enquanto a aplicação estiver em execução.

---

# 🎯 Objetivo

O principal objetivo deste projeto é demonstrar, de maneira simples e prática, como implementar **autenticação e autorização com JWT em uma ASP.NET Core Minimal API**.

O projeto demonstra o fluxo completo:

```text
Login
  ↓
Validação das credenciais
  ↓
Geração do JWT
  ↓
Bearer Token
  ↓
Swagger / Cliente HTTP
  ↓
Middleware de autenticação
  ↓
Autorização
  ↓
Endpoint protegido
```

A partir dessa estrutura é possível evoluir a aplicação para cenários mais completos, incluindo:

* Cadastro de usuários
* Banco de dados real
* Hash de senhas
* Refresh Token
* Controle de permissões
* Roles
* Policies
* Expiração e renovação de tokens
* Controle de acesso por usuário
* Integração com aplicações frontend

---

## 👨‍💻 Autor

Projeto desenvolvido para estudos e demonstração de conceitos de:

* **ASP.NET Core 8**
* **Minimal APIs**
* **JWT**
* **Authentication**
* **Authorization**
* **Swagger / OpenAPI**
* **Entity Framework Core**
