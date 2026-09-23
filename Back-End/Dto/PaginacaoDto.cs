using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto;

public sealed class PaginacaoDto
{
    [Range(1, int.MaxValue)]
    public int Pagina { get; set; } = 1;

    [Range(1, 100)]
    public int TamanhoPagina { get; set; } = 20;
}

public sealed record ResultadoPaginadoDto<T>(
    IReadOnlyCollection<T> Itens,
    int Pagina,
    int TamanhoPagina,
    int TotalItens,
    int TotalPaginas);

public static class PaginacaoExtensions
{
    public static async Task<ResultadoPaginadoDto<T>> PaginarAsync<T>(
        this IQueryable<T> consulta,
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default)
    {
        int totalItens = await consulta.CountAsync(cancellationToken);
        var itens = await consulta
            .Skip((paginacao.Pagina - 1) * paginacao.TamanhoPagina)
            .Take(paginacao.TamanhoPagina)
            .ToListAsync(cancellationToken);
        int totalPaginas = (int)Math.Ceiling(totalItens / (double)paginacao.TamanhoPagina);

        return new ResultadoPaginadoDto<T>(
            itens,
            paginacao.Pagina,
            paginacao.TamanhoPagina,
            totalItens,
            totalPaginas);
    }
}
