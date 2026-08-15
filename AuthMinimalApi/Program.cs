using AuthMinimalApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Banco
builder.Services.AddDbContext<ProdutoContext>(options =>
    options.UseInMemoryDatabase("ProdutosDb"));

// JWT
builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]
                    ?? throw new InvalidOperationException(
                        "JWT Key not configured.")
                )
            )
        };

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
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();


// Swagger
builder.Services.AddEndpointsApiExplorer();

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


var app = builder.Build();


// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


// Login
app.MapPost("/login", (LoginRequest loginRequest) =>
{
    if (loginRequest.Username != "usuario_teste" ||
        loginRequest.Password != "12345")
    {
        return Results.Unauthorized();
    }

    var issuer = builder.Configuration["Jwt:Issuer"];
    var audience = builder.Configuration["Jwt:Audience"];

    var jwtKey = builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException(
            "JWT Key not configured.");

    var key = Encoding.UTF8.GetBytes(jwtKey);

    var securityKey = new SymmetricSecurityKey(key);

    var credentials = new SigningCredentials(
        securityKey,
        SecurityAlgorithms.HmacSha256);

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

    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(30),
        signingCredentials: credentials
    );

    var jwtToken =
        new JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(new
    {
        token = jwtToken
    });
});


// Produtos
app.MapGet("/produtos", async (ProdutoContext db) =>
{
    var produtos = await db.Produtos.ToListAsync();

    return Results.Ok(produtos);
})
.RequireAuthorization();


app.MapControllers();

app.Run();