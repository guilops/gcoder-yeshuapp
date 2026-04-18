namespace ManagerTruck.Web.Dtos
{
    public class CaminhaoViewModel
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int AnoFabricacao { get; set; }
        public int AnoModelo { get; set; }
        public string FotoUrl { get; set; }
        public List<TruckEntry> Historico { get; set; } = new();
        public int KmAtual { get; set; }
        public bool PrecisaManutencao { get; set; }
        public bool ManutencaoProxima { get; set; }
        public int MontadoraId { get; set; }
        public string Montadora { get; set; }
    }

    public class TruckEntry
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int KmRodado { get; set; }
    }

    public class ManutencaoInput
    {
        public int CaminhaoId { get; set; }
        public int KmNaManutencao { get; set; }
        public string Tipo { get; set; }
    }
}
