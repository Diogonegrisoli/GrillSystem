using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using GrillSystem.Validacao;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services;

public class FuncionarioServices
{
    private readonly AppDbContext _context;

    public FuncionarioServices(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<Funcionario>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.Funcionarios.AsNoTracking().OrderBy(x => x.Nome)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<Funcionario> GetId(int id, CancellationToken cancellationToken = default) =>
        await _context.Funcionarios.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"O funcionário com o id {id} não foi localizado.");

    public async Task<Funcionario> Create(
        FuncionarioDto data,
        CancellationToken cancellationToken = default)
    {
        string cpf = Validacoes.ValidarCpf(data.Cpf);
        await ValidarCpfDuplicado(cpf, null, cancellationToken);
        var funcionario = new Funcionario(data.Nome.Trim(), cpf);
        _context.Funcionarios.Add(funcionario);
        await _context.SaveChangesAsync(cancellationToken);
        return funcionario;
    }

    public async Task<Funcionario> Update(
        int id,
        FuncionarioDto data,
        CancellationToken cancellationToken = default)
    {
        var funcionario = await _context.Funcionarios
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O funcionário com o id {id} não foi localizado.");
        string cpf = Validacoes.ValidarCpf(data.Cpf);
        await ValidarCpfDuplicado(cpf, id, cancellationToken);
        funcionario.Nome = data.Nome.Trim();
        funcionario.Cpf = cpf;
        await _context.SaveChangesAsync(cancellationToken);
        return funcionario;
    }

    public async Task<Funcionario> Delete(int id, CancellationToken cancellationToken = default)
    {
        var funcionario = await _context.Funcionarios
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O funcionário com o id {id} não foi localizado.");
        funcionario.Status = StatusFuncionario.Inativo;
        var usuarios = await _context.Usuarios.Where(x => x.FuncionarioId == id).ToListAsync(cancellationToken);
        foreach (var usuario in usuarios)
        {
            usuario.LockoutEnd = DateTimeOffset.MaxValue;
            usuario.SecurityStamp = Guid.NewGuid().ToString();
        }
        await _context.SaveChangesAsync(cancellationToken);
        return funcionario;
    }

    private async Task ValidarCpfDuplicado(
        string cpf,
        int? ignorarId,
        CancellationToken cancellationToken)
    {
        if (await _context.Funcionarios.AnyAsync(
                x => x.Cpf == cpf && (!ignorarId.HasValue || x.Id != ignorarId),
                cancellationToken))
        {
            throw new ConflitoNegocioException("O CPF informado já está cadastrado.");
        }
    }
}
