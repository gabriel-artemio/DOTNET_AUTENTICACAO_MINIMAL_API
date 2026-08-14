# Autenticação em Minimal API

Projeto desenvolvido para demonstrar a criação de uma **Minimal API com ASP.NET Core** e a implementação de autenticação e proteção de rotas utilizando um mecanismo de **AuthGuard**.

A proposta é apresentar, de forma prática, como criar uma Minimal API, implementar a lógica de autenticação e restringir o acesso a determinados endpoints.

## 📚 Sobre o projeto

Neste projeto vamos abordar:

* Criação de uma Minimal API com ASP.NET Core
* Configuração da autenticação
* Implementação de um `AuthGuard`
* Proteção de endpoints
* Validação da autenticação do usuário
* Organização da lógica de autorização
* Utilização de trechos de código para demonstrar o funcionamento do Guard

A ideia é mostrar como podemos impedir que determinados endpoints sejam acessados por usuários que não estejam autenticados.

---

## 🚀 Tecnologias utilizadas

* **.NET / ASP.NET Core**
* **Minimal APIs**
* **C#**
* **HTTP / REST**
* **Autenticação e autorização**

---

## 📁 Estrutura do projeto

Uma possível organização do projeto:

```text
MinimalApiAuth/
│
├── Guards/
│   └── AuthGuard.cs
│
├── Models/
│   └── Usuario.cs
│
├── Services/
│   └── AuthService.cs
│
├── Program.cs
│
└── appsettings.json
```

A estrutura pode ser adaptada conforme a evolução do projeto.

---

## 🔐 O que é um AuthGuard?

O **AuthGuard** é responsável por verificar se o usuário possui autorização para acessar determinado recurso da API.

A ideia é criar uma camada de proteção antes que a requisição chegue à lógica principal do endpoint.

De forma simplificada:

```text
Cliente
   │
   ▼
Requisição HTTP
   │
   ▼
AuthGuard
   │
   ├── Usuário autenticado?
   │       │
   │       ├── Não → 401 Unauthorized
   │       │
   │       └── Sim
   │
   ▼
Endpoint
   │
   ▼
Resposta
```

---

## 🛡️ Protegendo uma rota

Uma rota pública pode ser acessada normalmente:

```csharp
app.MapGet("/publico", () =>
{
    return Results.Ok("Endpoint público");
});
```

Enquanto uma rota protegida deverá passar pela validação do Guard:

```csharp
app.MapGet("/privado", () =>
{
    return Results.Ok("Endpoint protegido");
});
```

O `AuthGuard` será responsável por verificar se a requisição possui uma autenticação válida antes de permitir o acesso ao recurso.

---

## ⚙️ Funcionamento do Guard

O Guard pode analisar informações presentes na requisição, como:

* Token de autenticação
* Headers HTTP
* Claims do usuário
* Informações da sessão
* Permissões do usuário

Exemplo conceitual:

```csharp
public static bool IsAuthenticated(HttpContext context)
{
    return context.User?.Identity?.IsAuthenticated ?? false;
}
```

A partir dessa validação podemos decidir se a requisição continuará ou será interrompida.

---

## 🔑 Autenticação x Autorização

É importante diferenciar os dois conceitos.

### Autenticação

Responde à pergunta:

> "Quem é o usuário?"

Exemplo:

```text
Usuário → Login → Token
```

### Autorização

Responde à pergunta:

> "Esse usuário pode acessar esse recurso?"

Exemplo:

```text
Usuário autenticado
        │
        ▼
Possui permissão?
   │           │
  SIM         NÃO
   │           │
   ▼           ▼
Acesso       403
```

---

## 📡 Exemplos de endpoints

### Endpoint público

```http
GET /publico
```

Pode ser acessado sem autenticação.

### Login

```http
POST /login
```

Responsável por autenticar o usuário.

### Endpoint protegido

```http
GET /usuarios
Authorization: Bearer {token}
```

Nesse caso, o acesso dependerá da autenticação fornecida na requisição.

---

## 🧪 Testando a API

A API pode ser testada utilizando ferramentas como:

* Swagger
* Postman
* Insomnia
* curl

Exemplo:

```bash
curl https://localhost:5001/publico
```

Para um endpoint protegido:

```bash
curl https://localhost:5001/privado \
  -H "Authorization: Bearer SEU_TOKEN"
```

---

## 🎯 Objetivo

O principal objetivo deste projeto é demonstrar, de maneira simples e prática, como implementar uma camada de proteção para endpoints de uma **Minimal API**, permitindo compreender os conceitos envolvidos antes de utilizar soluções mais completas de autenticação e autorização.

## 👨‍💻 Autor

Projeto desenvolvido para estudos e demonstração de conceitos de **ASP.NET Core Minimal APIs, autenticação e autorização**.
