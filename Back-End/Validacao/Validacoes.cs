using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Validacao;

public static class Validacoes
{
    public static DateTime DataEmissao(DateTime dataInserida)
    {
        if (dataInserida.Date > DateTime.Today)
        {
            throw new ValidationException("A data não pode ser maior que a data atual!");
        }

        return dataInserida;
    }

    public static string ValidarCpf(string cpf)
    {
        cpf = SomenteDigitos(cpf);
        if (cpf.Length != 11)
        {
            throw new ValidationException("CPF deve possuir 11 dígitos.");
        }

        if (cpf.All(c => c == cpf[0]))
        {
            throw new ValidationException("CPF inválido.");
        }

        int soma = 0;
        for (int i = 0; i < 9; i++)
        {
            soma += (cpf[i] - '0') * (10 - i);
        }

        int resto = soma % 11;
        int primeiroDigito = resto < 2 ? 0 : 11 - resto;
        soma = 0;
        for (int i = 0; i < 10; i++)
        {
            soma += (cpf[i] - '0') * (11 - i);
        }

        resto = soma % 11;
        int segundoDigito = resto < 2 ? 0 : 11 - resto;
        if (cpf[9] - '0' != primeiroDigito || cpf[10] - '0' != segundoDigito)
        {
            throw new ValidationException("CPF inválido.");
        }

        return cpf;
    }

    public static string ValidarCnpj(string cnpj)
    {
        cnpj = SomenteDigitos(cnpj);
        if (cnpj.Length != 14)
        {
            throw new ValidationException("CNPJ deve possuir 14 dígitos.");
        }

        if (cnpj.All(c => c == cnpj[0]))
        {
            throw new ValidationException("CNPJ inválido.");
        }

        int[] multiplicador1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicador2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int soma = 0;
        for (int i = 0; i < 12; i++)
        {
            soma += (cnpj[i] - '0') * multiplicador1[i];
        }

        int resto = soma % 11;
        int primeiroDigito = resto < 2 ? 0 : 11 - resto;
        soma = 0;
        for (int i = 0; i < 13; i++)
        {
            soma += (cnpj[i] - '0') * multiplicador2[i];
        }

        resto = soma % 11;
        int segundoDigito = resto < 2 ? 0 : 11 - resto;
        if (cnpj[12] - '0' != primeiroDigito || cnpj[13] - '0' != segundoDigito)
        {
            throw new ValidationException("CNPJ inválido.");
        }

        return cnpj;
    }

    public static string ValidarCpfCnpj(string documento)
    {
        documento = SomenteDigitos(documento);
        return documento.Length switch
        {
            11 => ValidarCpf(documento),
            14 => ValidarCnpj(documento),
            _ => throw new ValidationException("O CPF/CNPJ é inválido.")
        };
    }

    private static string SomenteDigitos(string valor) =>
        new((valor ?? string.Empty).Where(char.IsDigit).ToArray());
}
