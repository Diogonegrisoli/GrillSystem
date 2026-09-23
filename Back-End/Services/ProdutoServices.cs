using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services;

public class ProdutoServices
{
    private readonly AppDbContext _context;

    public ProdutoServices(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<Produto>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.Produtos.AsNoTracking().OrderBy(x => x.Codigo)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<Produto> GetId(int id, CancellationToken cancellationToken = default) =>
        await _context.Produtos.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"O produto com o id {id} não foi localizado.");

    public async Task<Produto> Create(ProdutoDto data, CancellationToken cancellationToken = default)
    {
        string codigo = data.Codigo.Trim();
        await ValidarCodigo(codigo, null, cancellationToken);
        var produto = new Produto(codigo, data.Descricao.Trim(), data.Preco, 0);
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync(cancellationToken);
        return produto;
    }

    public async Task<Produto> Update(
        int id,
        ProdutoDto data,
        CancellationToken cancellationToken = default)
    {
        var produto = await _context.Produtos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O produto com o id {id} não foi localizado.");
        string codigo = data.Codigo.Trim();
        await ValidarCodigo(codigo, id, cancellationToken);
        produto.Codigo = codigo;
        produto.Descricao = data.Descricao.Trim();
        produto.Preco = data.Preco;
        await _context.SaveChangesAsync(cancellationToken);
        return produto;
    }

    public async Task<Produto> Delete(int id, CancellationToken cancellationToken = default)
    {
        var produto = await _context.Produtos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O produto com o id {id} não foi localizado.");
        bool utilizado = await _context.ProdutosMateriasPrimas.AnyAsync(x => x.ProdutoId == id, cancellationToken)
            || await _context.ProdutosPedidosVenda.AnyAsync(x => x.ProdutoId == id, cancellationToken)
            || await _context.ProdutosOrdensProducao.AnyAsync(x => x.ProdutoId == id, cancellationToken);
        if (utilizado)
        {
            throw new ConflitoNegocioException("O produto possui histórico e não pode ser excluído.");
        }
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync(cancellationToken);
        return produto;
    }

    private async Task ValidarCodigo(
        string codigo,
        int? ignorarId,
        CancellationToken cancellationToken)
    {
        if (await _context.Produtos.AnyAsync(
                x => x.Codigo == codigo && (!ignorarId.HasValue || x.Id != ignorarId),
                cancellationToken))
        {
            throw new ConflitoNegocioException("O código do produto já está cadastrado.");
        }
    }
}
