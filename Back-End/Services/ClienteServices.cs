using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using GrillSystem.Validacao;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Services;

public class ClienteServices
{
    private readonly AppDbContext _context;

    public ClienteServices(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<Cliente>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.Clientes.AsNoTracking().OrderBy(x => x.Nome)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<Cliente> GetId(int id, CancellationToken cancellationToken = default) =>
        await _context.Clientes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"O cliente com o id {id} não foi localizado.");

    public async Task<Cliente> Create(ClienteDto data, CancellationToken cancellationToken = default)
    {
        string documento = Validacoes.ValidarCpfCnpj(data.CpfCnpj);
        TipoCliente tipo = data.Tipo
            ?? throw new ValidationException("O tipo do cliente deve ser informado.");
        ValidarTipoDocumento(tipo, documento);
        if (await _context.Clientes.AnyAsync(x => x.CpfCnpj == documento, cancellationToken))
        {
            throw new ConflitoNegocioException("O CPF/CNPJ informado já está cadastrado.");
        }

        var cliente = new Cliente(
            data.Nome.Trim(),
            documento,
            tipo,
            SomenteDigitos(data.Telefone),
            data.Endereco.Trim());
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync(cancellationToken);
        return cliente;
    }

    public async Task<Cliente> Update(
        int id,
        ClienteUpdateDto data,
        CancellationToken cancellationToken = default)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O cliente com o id {id} não foi localizado.");
        cliente.Nome = data.Nome.Trim();
        cliente.Endereco = data.Endereco.Trim();
        cliente.Telefone = SomenteDigitos(data.Telefone);
        await _context.SaveChangesAsync(cancellationToken);
        return cliente;
    }

    public async Task<Cliente> Delete(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O cliente com o id {id} não foi localizado.");
        if (await _context.PedidosVenda.AnyAsync(x => x.ClienteId == id, cancellationToken))
        {
            throw new ConflitoNegocioException("O cliente possui pedidos e não pode ser excluído.");
        }
        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync(cancellationToken);
        return cliente;
    }

    private static void ValidarTipoDocumento(TipoCliente tipo, string documento)
    {
        if (tipo == TipoCliente.Fisica && documento.Length != 11 ||
            tipo == TipoCliente.Juridica && documento.Length != 14)
        {
            throw new ValidationException("O documento não corresponde ao tipo de cliente.");
        }
    }

    private static string SomenteDigitos(string valor) => new(valor.Where(char.IsDigit).ToArray());
}
