using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using GrillSystem.Validacao;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services;

public class FornecedorService
{
    private readonly AppDbContext _context;

    public FornecedorService(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<Fornecedor>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.Fornecedores.AsNoTracking().OrderBy(x => x.RazaoSocial)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<Fornecedor> GetId(int id, CancellationToken cancellationToken = default) =>
        await _context.Fornecedores.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"O fornecedor com o id {id} não foi localizado.");

    public async Task<Fornecedor> Create(
        FornecedorDto data,
        CancellationToken cancellationToken = default)
    {
        string cnpj = Validacoes.ValidarCnpj(data.Cnpj);
        if (await _context.Fornecedores.AnyAsync(x => x.Cnpj == cnpj, cancellationToken))
        {
            throw new ConflitoNegocioException("O CNPJ informado já está cadastrado.");
        }
        var fornecedor = new Fornecedor(
            data.RazaoSocial.Trim(),
            data.NomeFantasia.Trim(),
            cnpj,
            data.Email.Trim(),
            data.Endereco.Trim())
        {
            Contrato = data.Contrato.Trim(),
            Celular = data.Celular.Trim(),
            Situacao = data.Situacao
        };
        _context.Fornecedores.Add(fornecedor);
        await _context.SaveChangesAsync(cancellationToken);
        return fornecedor;
    }

    public async Task<Fornecedor> Update(
        int id,
        FornecedorUpdateDto data,
        CancellationToken cancellationToken = default)
    {
        var fornecedor = await _context.Fornecedores
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O fornecedor com o id {id} não foi localizado.");
        fornecedor.RazaoSocial = data.RazaoSocial.Trim();
        fornecedor.NomeFantasia = data.NomeFantasia.Trim();
        fornecedor.Email = data.Email.Trim();
        fornecedor.Endereco = data.Endereco.Trim();
        fornecedor.Contrato = data.Contrato.Trim();
        fornecedor.Celular = data.Celular.Trim();
        fornecedor.Situacao = data.Situacao;
        await _context.SaveChangesAsync(cancellationToken);
        return fornecedor;
    }

    public async Task<Fornecedor> Delete(int id, CancellationToken cancellationToken = default)
    {
        var fornecedor = await _context.Fornecedores
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O fornecedor com o id {id} não foi localizado.");
        if (await _context.PedidosCompra.AnyAsync(x => x.FornecedorId == id, cancellationToken))
        {
            throw new ConflitoNegocioException("O fornecedor possui pedidos e não pode ser excluído.");
        }
        _context.Fornecedores.Remove(fornecedor);
        await _context.SaveChangesAsync(cancellationToken);
        return fornecedor;
    }
}
