using GrillSystem.Data;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

string envPath = Path.Combine(builder.Environment.ContentRootPath, ".env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
    builder.Configuration.AddEnvironmentVariables();
}

string jwtKey = builder.Configuration["JWT_KEY"]
    ?? builder.Configuration["JWT_SECRET"]
    ?? throw new InvalidOperationException("Configure JWT_KEY ou JWT_SECRET com pelo menos 32 caracteres.");

if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException("A variável JWT_KEY deve ter pelo menos 32 caracteres.");
}

string jwtIssuer = builder.Configuration["JWT_ISSUER"] ?? "GrillSystem";
string jwtAudience = builder.Configuration["JWT_AUDIENCE"] ?? "GrillSystem.Api";

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
        Description = "Informe somente o token JWT."
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

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddScoped<CategoriaFinanceiraService>();
builder.Services.AddScoped<ClienteServices>();
builder.Services.AddScoped<ContaPagarService>();
builder.Services.AddScoped<ContaReceberServices>();
builder.Services.AddScoped<FornecedorMateriaPrimaServices>();
builder.Services.AddScoped<FornecedorService>();
builder.Services.AddScoped<FuncionarioServices>();
builder.Services.AddScoped<LancamentoService>();
builder.Services.AddScoped<MateriaPrimaServices>();
builder.Services.AddScoped<MovimentacaoEstoqueService>();
builder.Services.AddScoped<OrdemProducaoServices>();
builder.Services.AddScoped<PedidoCompraMateriaPrimaService>();
builder.Services.AddScoped<PedidoCompraService>();
builder.Services.AddScoped<PedidoVendaServices>();
builder.Services.AddScoped<ProdutoMateriaPrimaServices>();
builder.Services.AddScoped<ProdutoOrdemProducaoServices>();
builder.Services.AddScoped<ProdutoPedidoVendaServices>();
builder.Services.AddScoped<ProdutoServices>();
builder.Services.AddScoped<UsuarioServices>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<PasswordHasher<Usuario>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
