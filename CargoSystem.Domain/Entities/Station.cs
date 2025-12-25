namespace CargoSystem.Domain.Entities
{
    public class Station : BaseEntity
    {
        public string Name { get; set; } // Örn: Başiskele, Gebze
        public double Latitude { get; set; }  // Enlem
        public double Longitude { get; set; } // Boylam
        
        // Bu istasyondaki kargoların listesi (Navigation Property)
        public ICollection<Cargo> Cargos { get; set; }
    }
}