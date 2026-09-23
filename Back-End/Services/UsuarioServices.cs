using GrillSystem.Authorization;
using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Services;

public class UsuarioServices
{
    private readonly AppDbContext _context;
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;

    public UsuarioServices(
        AppDbContext context,
        UserManager<Usuario> userManager,
        SignInManager<Usuario> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public Task<bool> HasUsers() => _userManager.Users.AnyAsync();

    public async Task<Usuario?> Authenticate(LoginDto data)
    {
        var usuario = await _userManager.FindByEmailAsync(data.Email.Trim());
        if (usuario is null)
        {
            return null;
        }

        await _context.Entry(usuario).Reference(x => x.Funcionario).LoadAsync();
        if (usuario.Funcionario.Status != StatusFuncionario.Ativo)
        {
            return null;
        }

        var resultado = await _signInManager.CheckPasswordSignInAsync(
            usuario,
            data.Senha,
            lockoutOnFailure: true);

        return resultado.Succeeded ? usuario : null;
    }

    public async Task<ResultadoPaginadoDto<UsuarioResponseDto>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default)
    {
        var consulta = _userManager.Users
            .AsNoTracking()
            .Include(x => x.Funcionario)
            .OrderBy(x => x.Email);
        int totalItens = await consulta.CountAsync(cancellationToken);
        var usuarios = await consulta
            .Skip((paginacao.Pagina - 1) * paginacao.TamanhoPagina)
            .Take(paginacao.TamanhoPagina)
            .ToListAsync(cancellationToken);

        var response = new List<UsuarioResponseDto>(usuarios.Count);
        foreach (var usuario in usuarios)
        {
            response.Add(await ToResponse(usuario));
        }

        return new ResultadoPaginadoDto<UsuarioResponseDto>(
            response,
            paginacao.Pagina,
            paginacao.TamanhoPagina,
            totalItens,
            (int)Math.Ceiling(totalItens / (double)paginacao.TamanhoPagina));
    }

    public async Task<UsuarioResponseDto> GetId(int id)
    {
        var usuario = await _userManager.Users
            .AsNoTracking()
            .Include(x => x.Funcionario)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new KeyNotFoundException($"O usuário com o id {id} não foi localizado.");

        return await ToResponse(usuario);
    }

    public async Task<UsuarioResponseDto> Create(UsuarioDto data)
    {
        string perfil = (await HasUsers()) ? NormalizarPerfil(data.Perfil) : Perfis.Administrador;
        await ValidarFuncionario(data.FuncionarioId);

        var usuario = new Usuario
        {
            Email = data.Email.Trim(),
            UserName = data.Email.Trim(),
            FuncionarioId = data.FuncionarioId,
            EmailConfirmed = true,
            LockoutEnabled = true
        };

        await using var transaction = await _context.Database.BeginTransactionAsync();
        EnsureSuccess(await _userManager.CreateAsync(usuario, data.Senha));
        EnsureSuccess(await _userManager.AddToRoleAsync(usuario, perfil));
        await transaction.CommitAsync();

        await _context.Entry(usuario).Reference(x => x.Funcionario).LoadAsync();
        return await ToResponse(usuario);
    }

    public async Task<UsuarioResponseDto> Update(int id, UsuarioUpdateDto data)
    {
        var usuario = await _userManager.Users
            .Include(x => x.Funcionario)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new KeyNotFoundException($"O usuário com o id {id} não foi localizado.");

        string perfil = NormalizarPerfil(data.Perfil);
        await GarantirAdministradorRemanescente(usuario, perfil, removendo: data.Bloqueado);

        await using var transaction = await _context.Database.BeginTransactionAsync();

        string email = data.Email.Trim();
        EnsureSuccess(await _userManager.SetEmailAsync(usuario, email));
        EnsureSuccess(await _userManager.SetUserNameAsync(usuario, email));

        if (!string.IsNullOrWhiteSpace(data.Senha))
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
            EnsureSuccess(await _userManager.ResetPasswordAsync(usuario, token, data.Senha));
        }

        if (data.Bloqueado)
        {
            EnsureSuccess(await _userManager.SetLockoutEndDateAsync(usuario, DateTimeOffset.MaxValue));
        }
        else
        {
            EnsureSuccess(await _userManager.SetLockoutEndDateAsync(usuario, null));
            await _userManager.ResetAccessFailedCountAsync(usuario);
        }

        var perfisAtuais = await _userManager.GetRolesAsync(usuario);
        if (perfisAtuais.Count > 0)
        {
            EnsureSuccess(await _userManager.RemoveFromRolesAsync(usuario, perfisAtuais));
        }
        EnsureSuccess(await _userManager.AddToRoleAsync(usuario, perfil));
        EnsureSuccess(await _userManager.UpdateSecurityStampAsync(usuario));

        await transaction.CommitAsync();
        return await ToResponse(usuario);
    }

    public async Task Delete(int id, int? usuarioAtualId)
    {
        var usuario = await _userManager.FindByIdAsync(id.ToString())
            ?? throw new KeyNotFoundException($"O usuário com o id {id} não foi localizado.");

        if (usuarioAtualId == id)
        {
            throw new ValidationException("Não é permitido excluir o próprio usuário autenticado.");
        }

        await GarantirAdministradorRemanescente(usuario, null, removendo: true);
        EnsureSuccess(await _userManager.DeleteAsync(usuario));
    }

    private async Task ValidarFuncionario(int funcionarioId)
    {
        bool existe = await _context.Funcionarios.AnyAsync(
            x => x.Id == funcionarioId && x.Status == StatusFuncionario.Ativo);
        if (!existe)
        {
            throw new ValidationException("O funcionário informado não existe ou está inativo.");
        }

        bool associado = await _userManager.Users.AnyAsync(x => x.FuncionarioId == funcionarioId);
        if (associado)
        {
            throw new ValidationException("O funcionário informado já possui um usuário.");
        }
    }

    private async Task GarantirAdministradorRemanescente(Usuario usuario, string? novoPerfil, bool removendo)
    {
        if (!await _userManager.IsInRoleAsync(usuario, Perfis.Administrador))
        {
            return;
        }

        if (!removendo && novoPerfil == Perfis.Administrador)
        {
            return;
        }

        var administradores = await _userManager.GetUsersInRoleAsync(Perfis.Administrador);
        if (administradores.Count <= 1)
        {
            throw new ValidationException("O sistema deve manter pelo menos um administrador.");
        }
    }

    private static string NormalizarPerfil(string perfil)
    {
        string? encontrado = Perfis.Todos.FirstOrDefault(x =>
            string.Equals(x, perfil?.Trim(), StringComparison.OrdinalIgnoreCase));

        return encontrado ?? throw new ValidationException(
            $"Perfil inválido. Valores permitidos: {string.Join(", ", Perfis.Todos)}.");
    }

    private async Task<UsuarioResponseDto> ToResponse(Usuario usuario)
    {
        var perfis = await _userManager.GetRolesAsync(usuario);
        bool bloqueado = usuario.LockoutEnd.HasValue && usuario.LockoutEnd > DateTimeOffset.UtcNow;
        return new UsuarioResponseDto(
            usuario.Id,
            usuario.Email ?? string.Empty,
            usuario.FuncionarioId,
            usuario.Funcionario?.Nome ?? string.Empty,
            bloqueado,
            perfis.ToArray());
    }

    private static void EnsureSuccess(IdentityResult result)
    {
        if (result.Succeeded)
        {
            return;
        }

        throw new ValidationException(string.Join(" ", result.Errors.Select(x => x.Description)));
    }
}
