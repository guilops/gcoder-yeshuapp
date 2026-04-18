using ManagerTruck.API.Enums;
using ManagerTruck.Entities;

namespace ManagerTruck.Dtos
{
    public class CaminhaoDto
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int AnoFabricacao { get; set; }
        public int AnoModelo { get; set; }
        public string FotoUrl { get; set; }
        public List<CaminhaoHistoricoDto> Historico { get; set; } = new();
        public int KmAtual { get; set; } = 0;
        public int KmUltimaManutencao { get; set; } = 0;
        public string Montadora { get; set; }
        public int MontadoraId { get; set; }
        public bool PrecisaManutencao { get; set; }
        public bool ManutencaoProxima { get; set; }
    }

    public class CaminhaoHistoricoDto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public ETipoLancamento Tipo { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int KmRodado { get; set; }
    }
}
