using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using GrillSystem.Validacao;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Services;

public class MovimentacaoEstoqueService
{
    private readonly AppDbContext _context;

    public MovimentacaoEstoqueService(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<MovimentacaoEstoque>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.MovimentacoesEstoque
            .AsNoTracking()
            .OrderByDescending(x => x.Data)
            .ThenByDescending(x => x.Id)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<MovimentacaoEstoque> GetId(
        int id,
        CancellationToken cancellationToken = default) =>
        await _context.MovimentacoesEstoque
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException(
            $"A movimentação de estoque com o id {id} não foi localizada.");

    public async Task<MovimentacaoEstoque> Create(
        MovimentacaoEstoqueDto data,
        CancellationToken cancellationToken = default)
    {
        Validacoes.DataNaoFutura(data.Data, "A data da movimentação");
        TipoMovimentacao tipo = data.Tipo
            ?? throw new ValidationException("O tipo da movimentação deve ser informado.");

        await using var transaction = await _context.Database
            .BeginTransactionAsync(cancellationToken);

        int registrosAtualizados;
        if (tipo == TipoMovimentacao.Entrada)
        {
            registrosAtualizados = await _context.MateriasPrimas
                .Where(x => x.Id == data.MateriaPrimaId)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(
                        x => x.Quantidade,
                        x => x.Quantidade + data.Quantidade),
                    cancellationToken);
        }
        else
        {
            registrosAtualizados = await _context.MateriasPrimas
                .Where(x => x.Id == data.MateriaPrimaId && x.Quantidade >= data.Quantidade)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(
                        x => x.Quantidade,
                        x => x.Quantidade - data.Quantidade),
                    cancellationToken);
        }

        if (registrosAtualizados == 0)
        {
            bool materiaPrimaExiste = await _context.MateriasPrimas
                .AnyAsync(x => x.Id == data.MateriaPrimaId, cancellationToken);

            if (!materiaPrimaExiste)
            {
                throw new KeyNotFoundException(
                    $"A matéria-prima com o id {data.MateriaPrimaId} não foi localizada.");
            }

            throw new RegraNegocioException(
                "Estoque insuficiente para realizar a movimentação de saída.");
        }

        var movimentacao = new MovimentacaoEstoque(
            tipo,
            data.Quantidade,
            data.CustoUnitario,
            data.Data,
            data.Referencia.Trim(),
            data.MateriaPrimaId);

        _context.MovimentacoesEstoque.Add(movimentacao);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return movimentacao;
    }

    public async Task<MovimentacaoEstoque> Update(
        int id,
        MovimentacaoEstoqueDto data,
        CancellationToken cancellationToken = default)
    {
        _ = data;
        await GetId(id, cancellationToken);
        throw new RegraNegocioException(
            "Movimentações efetivadas são imutáveis. Registre uma movimentação inversa para estornar o saldo.");
    }

    public async Task<MovimentacaoEstoque> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        await GetId(id, cancellationToken);
        throw new RegraNegocioException(
            "Movimentações efetivadas não podem ser excluídas. Registre uma movimentação inversa para estornar o saldo.");
    }
}
