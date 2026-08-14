using AuthMinimalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProdutoContext>(opt =>
   opt.UseInMemoryDatabase("ProdutosDb")); // Para simplificar, usando banco de dados em memória

builder.Services.AddAuthorization();

builder.Services.AddAuthentication("Bearer")
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

app.MapPost("/login", (LoginRequest loginRequest) =>
{
    if (loginRequest.Username == "usuario_teste" && loginRequest.Password == "12345")
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key not configured."));
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        // Adicionar claims (informações sobre o usuário)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, loginRequest.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "Admin") // Exemplo de claim de role
        };
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30), // Token expira em 30 minutos
            signingCredentials: credentials);
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(new { Token = jwtToken });
    }
    else
    {
        return Results.Unauthorized(); // 401 Unauthorized
    }
});

app.MapGet("/produtos", async (ProdutoContext db) =>
     await db.Produtos.ToListAsync())
     .RequireAuthorization();

app.UseAuthentication();
app.UseAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();