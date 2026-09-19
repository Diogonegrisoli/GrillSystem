using DotNetEnv;
using GrillSystem.Authorization;
using GrillSystem.Data;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

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
    throw new InvalidOperationException("A chave JWT deve ter pelo menos 32 bytes.");
}

string jwtIssuer = builder.Configuration["JWT_ISSUER"] ?? "GrillSystem";
string jwtAudience = builder.Configuration["JWT_AUDIENCE"] ?? "GrillSystem.Api";
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Configure a connection string DefaultConnection.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services
    .AddIdentityCore<Usuario>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 3;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    })
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<Usuario>>();
                var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
                string? userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                string? tokenStamp = context.Principal?.FindFirstValue("security_stamp");
                var usuario = userId is null ? null : await userManager.FindByIdAsync(userId);

                bool funcionarioAtivo = usuario is not null && await dbContext.Funcionarios
                    .AnyAsync(x => x.Id == usuario.FuncionarioId && x.Status == StatusFuncionario.Ativo);
                bool bloqueado = usuario?.LockoutEnd is not null && usuario.LockoutEnd > DateTimeOffset.UtcNow;

                if (usuario is null || bloqueado || !funcionarioAtivo ||
                    !string.Equals(usuario.SecurityStamp, tokenStamp, StringComparison.Ordinal))
                {
                    context.Fail("Token revogado ou usuário inexistente.");
                }
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Politicas.GerenciarUsuarios, policy =>
        policy.RequireRole(Perfis.Administrador));
    options.AddPolicy(Politicas.GerenciarSistema, policy =>
        policy.RequireRole(Perfis.Administrador, Perfis.Gerente));
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
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

var app = builder.Build();

app.UseExceptionHandler();
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

public partial class Program;
