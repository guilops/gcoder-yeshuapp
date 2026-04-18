namespace ManagerTruck.Entities
{
    public class MontadoraEntity
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<CaminhaoEntity> Caminhoes { get; set; }
    }
}