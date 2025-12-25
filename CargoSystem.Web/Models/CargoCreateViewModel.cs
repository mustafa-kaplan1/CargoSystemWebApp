using CargoSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CargoSystem.Web.Models
{
	public class CargoCreateViewModel
	{
		[Display(Name = "Gönderici Adı Soyadı")]
		[Required(ErrorMessage = "Lütfen adınızı giriniz.")]
		public string SenderName { get; set; }

		[Display(Name = "Kargo Ağırlığı (kg)")]
		[Required(ErrorMessage = "Ağırlık boş bırakılamaz.")]
		[Range(1, 1000, ErrorMessage = "Kargo ağırlığı 1 ile 1000 kg arasında olmalıdır.")]
		public double WeightKg { get; set; }

		[Display(Name = "Hedef İstasyon (İlçe)")]
		[Required(ErrorMessage = "Lütfen bir istasyon seçiniz.")]
		public int StationId { get; set; }

		// Dropdown'ı doldurmak için istasyon listesi
		public List<Station>? Stations { get; set; }
	}
}