using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services;

public sealed class EnderecoService(AppDbContext context)
{
    public Task<ResultadoPaginadoDto<Endereco>> Listar(PaginacaoDto pagina, CancellationToken ct) =>
        context.Enderecos.AsNoTracking().OrderBy(x => x.Id).PaginarAsync(pagina, ct);

    public async Task<Endereco> Obter(int id, CancellationToken ct) =>
        await context.Enderecos.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct)
        ?? throw new KeyNotFoundException($"Endereço {id} não encontrado.");

    public async Task<Endereco> Criar(EnderecoDto dto, CancellationToken ct)
    {
        var endereco = new Endereco();
        Aplicar(endereco, dto);
        context.Enderecos.Add(endereco);
        await context.SaveChangesAsync(ct);
        return endereco;
    }

    public async Task<Endereco> Alterar(int id, EnderecoDto dto, CancellationToken ct)
    {
        var endereco = await context.Enderecos.SingleOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new KeyNotFoundException($"Endereço {id} não encontrado.");
        Aplicar(endereco, dto);
        await context.SaveChangesAsync(ct);
        return endereco;
    }

    public async Task Vincular(int id, VinculoEnderecoDto dto, CancellationToken ct)
    {
        var endereco = await context.Enderecos.SingleOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new KeyNotFoundException($"Endereço {id} não encontrado.");
        switch (dto.TipoTitular)
        {
            case TipoTitularEndereco.Cliente:
                var cliente = await context.Clientes.Include(x => x.Enderecos)
                    .SingleOrDefaultAsync(x => x.Id == dto.TitularId, ct)
                    ?? throw new KeyNotFoundException("Cliente não encontrado.");
                if (!cliente.Enderecos.Any(x => x.Id == id)) cliente.Enderecos.Add(endereco);
                break;
            case TipoTitularEndereco.Fornecedor:
                var fornecedor = await context.Fornecedores.Include(x => x.Enderecos)
                    .SingleOrDefaultAsync(x => x.Id == dto.TitularId, ct)
                    ?? throw new KeyNotFoundException("Fornecedor não encontrado.");
                if (!fornecedor.Enderecos.Any(x => x.Id == id)) fornecedor.Enderecos.Add(endereco);
                break;
            case TipoTitularEndereco.Funcionario:
                var funcionario = await context.Funcionarios.Include(x => x.Enderecos)
                    .SingleOrDefaultAsync(x => x.Id == dto.TitularId, ct)
                    ?? throw new KeyNotFoundException("Funcionário não encontrado.");
                if (!funcionario.Enderecos.Any(x => x.Id == id)) funcionario.Enderecos.Add(endereco);
                break;
            default: throw new ArgumentException("Tipo de titular inválido.");
        }
        await context.SaveChangesAsync(ct);
    }

    public async Task Desvincular(int id, VinculoEnderecoDto dto, CancellationToken ct)
    {
        switch (dto.TipoTitular)
        {
            case TipoTitularEndereco.Cliente:
                var cliente = await context.Clientes.Include(x => x.Enderecos)
                    .SingleOrDefaultAsync(x => x.Id == dto.TitularId, ct)
                    ?? throw new KeyNotFoundException("Cliente não encontrado.");
                cliente.Enderecos.Remove(cliente.Enderecos.FirstOrDefault(x => x.Id == id)
                    ?? throw new KeyNotFoundException("Vínculo não encontrado."));
                break;
            case TipoTitularEndereco.Fornecedor:
                var fornecedor = await context.Fornecedores.Include(x => x.Enderecos)
                    .SingleOrDefaultAsync(x => x.Id == dto.TitularId, ct)
                    ?? throw new KeyNotFoundException("Fornecedor não encontrado.");
                fornecedor.Enderecos.Remove(fornecedor.Enderecos.FirstOrDefault(x => x.Id == id)
                    ?? throw new KeyNotFoundException("Vínculo não encontrado."));
                break;
            case TipoTitularEndereco.Funcionario:
                var funcionario = await context.Funcionarios.Include(x => x.Enderecos)
                    .SingleOrDefaultAsync(x => x.Id == dto.TitularId, ct)
                    ?? throw new KeyNotFoundException("Funcionário não encontrado.");
                funcionario.Enderecos.Remove(funcionario.Enderecos.FirstOrDefault(x => x.Id == id)
                    ?? throw new KeyNotFoundException("Vínculo não encontrado."));
                break;
            default: throw new ArgumentException("Tipo de titular inválido.");
        }
        await context.SaveChangesAsync(ct);
    }

    public async Task Excluir(int id, CancellationToken ct)
    {
        var endereco = await context.Enderecos.SingleOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new KeyNotFoundException($"Endereço {id} não encontrado.");
        bool vinculado = await context.Clientes.AnyAsync(x => x.Enderecos.Any(e => e.Id == id), ct)
            || await context.Fornecedores.AnyAsync(x => x.Enderecos.Any(e => e.Id == id), ct)
            || await context.Funcionarios.AnyAsync(x => x.Enderecos.Any(e => e.Id == id), ct);
        if (vinculado) throw new ConflitoNegocioException("Desvincule o endereço antes de excluí-lo.");
        context.Enderecos.Remove(endereco);
        await context.SaveChangesAsync(ct);
    }

    private static void Aplicar(Endereco e, EnderecoDto dto)
    {
        e.Logradouro = dto.Logradouro.Trim();
        e.Numero = dto.Numero.Trim();
        e.Bairro = dto.Bairro.Trim();
        e.Cidade = dto.Cidade.Trim();
        e.Estado = dto.Estado.Trim().ToUpperInvariant();
        e.Cep = dto.Cep.Trim();
    }
}
