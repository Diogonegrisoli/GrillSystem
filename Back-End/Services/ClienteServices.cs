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
        _context.Clientes.AsNoTracking()
            .Include(x => x.PessoaFisica).Include(x => x.PessoaJuridica)
            .OrderBy(x => x.Nome)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<Cliente> GetId(int id, CancellationToken cancellationToken = default) =>
        await _context.Clientes.AsNoTracking()
            .Include(x => x.PessoaFisica).Include(x => x.PessoaJuridica)
            .Include(x => x.Enderecos)
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

        string nome = tipo == TipoCliente.Juridica
            ? (data.RazaoSocial ?? data.Nome).Trim()
            : data.Nome.Trim();
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ValidationException("Informe o nome ou a razão social do cliente.");
        }
        if (tipo == TipoCliente.Fisica && nome.Length > 100)
        {
            throw new ValidationException("O nome da pessoa física deve ter no máximo 100 caracteres.");
        }
        var cliente = new Cliente(
            nome,
            documento,
            tipo,
            SomenteDigitos(data.Telefone),
            data.Endereco.Trim())
        {
            Email = data.Email.Trim(),
            Celular = SomenteDigitos(data.Celular),
            Observacoes = data.Observacoes.Trim()
        };
        if (tipo == TipoCliente.Fisica)
        {
            cliente.PessoaFisica = new PessoaFisica
            {
                Nome = nome,
                Cpf = documento,
                DataNascimento = data.DataNascimento
            };
        }
        else
        {
            cliente.PessoaJuridica = new PessoaJuridica
            {
                RazaoSocial = nome,
                NomeFantasia = string.IsNullOrWhiteSpace(data.NomeFantasia)
                    ? nome : data.NomeFantasia.Trim(),
                Cnpj = documento
            };
        }
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync(cancellationToken);
        return cliente;
    }

    public async Task<Cliente> Update(
        int id,
        ClienteUpdateDto data,
        CancellationToken cancellationToken = default)
    {
        var cliente = await _context.Clientes
            .Include(x => x.PessoaFisica).Include(x => x.PessoaJuridica)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O cliente com o id {id} não foi localizado.");
        cliente.Nome = data.Nome.Trim();
        if (cliente.Tipo == TipoCliente.Fisica && cliente.Nome.Length > 100)
        {
            throw new ValidationException("O nome da pessoa física deve ter no máximo 100 caracteres.");
        }
        cliente.Endereco = data.Endereco.Trim();
        cliente.Telefone = SomenteDigitos(data.Telefone);
        cliente.Email = data.Email.Trim();
        cliente.Celular = SomenteDigitos(data.Celular);
        cliente.Observacoes = data.Observacoes.Trim();
        cliente.Situacao = data.Situacao;
        if (cliente.Tipo == TipoCliente.Fisica)
        {
            if (cliente.PessoaFisica is null)
            {
                throw new ConflitoNegocioException("O cliente não possui os dados de pessoa física.");
            }
            cliente.PessoaFisica.Nome = cliente.Nome;
            cliente.PessoaFisica.DataNascimento = data.DataNascimento;
        }
        else
        {
            if (cliente.PessoaJuridica is null)
            {
                throw new ConflitoNegocioException("O cliente não possui os dados de pessoa jurídica.");
            }
            cliente.PessoaJuridica.RazaoSocial = cliente.Nome;
            cliente.PessoaJuridica.NomeFantasia = string.IsNullOrWhiteSpace(data.NomeFantasia)
                ? cliente.Nome : data.NomeFantasia.Trim();
        }
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
