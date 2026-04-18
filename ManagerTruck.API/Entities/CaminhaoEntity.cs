using System.ComponentModel.DataAnnotations;
using ManagerTruck.API.Enums;

namespace ManagerTruck.Entities
{
    public class CaminhaoEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int AnoFabricacao { get; set; }
        public int AnoModelo { get; set; }
        public string FotoUrl { get; set; }
        public List<CaminhaoHistoricoEntity> Historico { get; set; } = new();
        public int KmAtual { get; set; } 
        public int KmUltimaManutencao { get; set; }
        public int MontadoraId { get; set; }
        public MontadoraEntity Montadora { get; set; }
    }

    public class CaminhaoHistoricoEntity
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public ETipoLancamento Tipo { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int KmRodado { get; set; }
    }
}