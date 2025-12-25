namespace CargoSystem.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        public string PlateNumber { get; set; } // Plaka veya Araç Adı (Örn: Araç 1)
        public double MaxCapacityKg { get; set; } // 500, 750, 1000 kg
        
        // Kiralık araç mı? (Başlangıçtaki 3 araç kiralık değil, sonradan eklenenler kiralık)
        public bool IsRented { get; set; } 
        public decimal RentalCost { get; set; } 
        public decimal CostPerKm { get; set; } = 1.0m;

        // O anki doluluk oranı (Hesaplamalar için kullanılabilir)
        public double CurrentLoadKg { get; set; }
    }
}