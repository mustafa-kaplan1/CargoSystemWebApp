using CargoSystem.Domain.Entities;
using System.Collections.Generic;

namespace CargoSystem.Application.Services
{
    public interface ICargoService
    {
        // Kullanıcının seçebilmesi için istasyonları getir
        List<Station> GetStations();
        
        // Yeni kargo talebi oluştur
        void CreateCargo(Cargo cargo);
    }
}