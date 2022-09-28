using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace B2C2_Pi4_PFFF_Gaetano.Models
{
    public class CameraLocation
    {
        [Key]
        public int id { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DisplayName("Straatnaam")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string StreetName { get; set; }

        [DisplayName("Huisnummer")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public int HouseNumber { get; set; }

        [DisplayName("Toevoeging")]
        public string? HouseNumberAddition { get; set; }

        [DisplayName("Postcode")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string ZipCode { get; set; }

        [DisplayName("Stad")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string City { get; set; }

        [DisplayName("Provincie")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string Region { get; set; }

        public Camera Camera { get; set; }
    }
}
