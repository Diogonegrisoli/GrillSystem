using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Models
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public StatusFuncionario Status {  get; set; }

        public Funcionario() { }

        public Funcionario(string nome, string cpf)
        {
            Nome = nome;
            Cpf = cpf;
            Status = StatusFuncionario.Ativo;
        }
    }

    public enum StatusFuncionario
    {
        Ativo,
        Inativo
    }
}
