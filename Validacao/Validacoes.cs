using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GrillSystem.Validacao
{
    public class Validacoes
    {
        public static DateTime DataEmissao(DateTime dataInserida)
        {
            DateTime data = dataInserida;

            if (data.Date > DateTime.Today)
            {
                throw new Exception("A data não pode ser maior que a data atual!");
            }

            return data;
        }

        public static string ValidarCpf(string cpf)
        {
            // Remove caracteres não numéricos
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Verifica tamanho
            if (cpf.Length != 11)
                throw new Exception("CPF deve possuir 11 dígitos.");

            // Verifica sequência repetida
            if (cpf.All(c => c == cpf[0]))
                throw new Exception("CPF inválido.");

            // =========================
            // Primeiro dígito
            // =========================
            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += (cpf[i] - '0') * (10 - i);
            }

            int resto = soma % 11;

            int primeiroDigito = resto < 2 ? 0 : 11 - resto;

            // =========================
            // Segundo dígito
            // =========================
            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += (cpf[i] - '0') * (11 - i);
            }

            resto = soma % 11;

            int segundoDigito = resto < 2 ? 0 : 11 - resto;

            // Validação final
            if (cpf[9] - '0' != primeiroDigito ||
                cpf[10] - '0' != segundoDigito)
            {
                throw new Exception("CPF inválido.");
            }

            // Retorna CPF formatado
            return cpf.ToString();
        }

        public static string ValidarCnpj(string cnpj)
        {
            if (cnpj.All(c => c == cnpj[0]))
                throw new Exception("CNPJ inválido.");

            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

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
                int valor = (cnpj[i] - '0');

                if (i == 12)
                    valor = primeiroDigito;

                soma += valor * multiplicador2[i];
            }

            resto = soma % 11;

            int segundoDigito = resto < 2 ? 0 : 11 - resto;

            if (cnpj[12] - '0' != primeiroDigito ||
                cnpj[13] - '0' != segundoDigito)
            {
                throw new Exception("CNPJ inválido.");
            }

            return cnpj.ToString();
        }


        public static string ValidarCpfCnpj(string documento)
        {
            documento = new string(documento.Where(char.IsDigit).ToArray());

            if (documento.Length == 11)
            {
                return ValidarCpf(documento);
            }

            if (documento.Length == 14)
            {
                return ValidarCnpj(documento);
            }

            throw new Exception("O CPF/CNPJ é inválido");
        }
    }
}
