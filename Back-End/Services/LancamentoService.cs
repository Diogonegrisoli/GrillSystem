using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Validacao;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services;

public class LancamentoService
{
    private readonly AppDbContext _context;

    public LancamentoService(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<Lancamento>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.Lancamentos.AsNoTracking()
            .OrderByDescending(x => x.Data).ThenByDescending(x => x.Id)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<Lancamento> GetId(int id, CancellationToken cancellationToken = default) =>
        await _context.Lancamentos.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"O lançamento com o id {id} não foi localizado.");

    public async Task<Lancamento> Create(
        LancamentoDto data,
        CancellationToken cancellationToken = default)
    {
        await Validar(data, cancellationToken);
        var lancamento = new Lancamento(
            data.Valor,
            data.Data,
            data.Descricao.Trim(),
            data.CategoriaFinanceiraId,
            data.FuncionarioId);
        _context.Lancamentos.Add(lancamento);
        await _context.SaveChangesAsync(cancellationToken);
        return lancamento;
    }

    public async Task<Lancamento> Update(
        int id,
        LancamentoDto data,
        CancellationToken cancellationToken = default)
    {
        var lancamento = await _context.Lancamentos
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O lançamento com o id {id} não foi localizado.");
        await Validar(data, cancellationToken);
        lancamento.Valor = data.Valor;
        lancamento.Data = data.Data;
        lancamento.Descricao = data.Descricao.Trim();
        lancamento.CategoriaFinanceiraId = data.CategoriaFinanceiraId;
        lancamento.FuncionarioId = data.FuncionarioId;
        await _context.SaveChangesAsync(cancellationToken);
        return lancamento;
    }

    public async Task<Lancamento> Delete(int id, CancellationToken cancellationToken = default)
    {
        var lancamento = await _context.Lancamentos
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O lançamento com o id {id} não foi localizado.");
        _context.Lancamentos.Remove(lancamento);
        await _context.SaveChangesAsync(cancellationToken);
        return lancamento;
    }

    private async Task Validar(LancamentoDto data, CancellationToken cancellationToken)
    {
        Validacoes.DataNaoFutura(data.Data, "A data do lançamento");
        if (!await _context.CategoriasFinanceiras.AnyAsync(
                x => x.Id == data.CategoriaFinanceiraId,
                cancellationToken))
        {
            throw new KeyNotFoundException(
                $"A categoria financeira com o id {data.CategoriaFinanceiraId} não foi localizada.");
        }
        if (!await _context.Funcionarios.AnyAsync(
                x => x.Id == data.FuncionarioId && x.Status == StatusFuncionario.Ativo,
                cancellationToken))
        {
            throw new KeyNotFoundException(
                $"O funcionário com o id {data.FuncionarioId} não existe ou está inativo.");
        }
    }
}
