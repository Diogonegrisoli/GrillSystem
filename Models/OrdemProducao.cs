using Microsoft.EntityFrameworkCore.Storage.Json;

namespace GrillSystem.Models
{
    public class OrdemProducao
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public StatusOrdem Status { get; set; }
        public DateOnly DataInicio { get; set; }
        public DateOnly? DataFim { get; set; }
        public TipoOrdem Tipo { get; set; }

        public OrdemProducao(int quantidade, DateOnly dataInicio, DateOnly dataFim, TipoOrdem tipo)
        {
            Quantidade = quantidade;
            Status = StatusOrdem.Pendente;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Tipo = tipo;
        }

        public OrdemProducao() { }

    }

    public enum StatusOrdem
    {
        Pendente,
        EmAndamento,
        Finalizado
    }

    public enum TipoOrdem
    {
        Puxada,
        Empurrada
    }
}
