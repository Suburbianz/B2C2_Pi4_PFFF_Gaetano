using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2C2_Pi4_PFFF_Gaetano.Models
{
    public class CameraLocation
    {
        // Individual Properties
        [Key]
        public int Id { get; set; }

        [DisplayName("Gemaakt op")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DisplayName("Straatnaam")]
        [StringLength(100, ErrorMessage = "Straatnaam kan niet meer dan 100 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string StreetName { get; set; }

        [DisplayName("Huisnummer")]
        [Range(1, 20000, ErrorMessage = "Huisnummer moet tussen 1 en 20000 zijn.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public int HouseNumber { get; set; }

        [DisplayName("Toevoeging")]
        [StringLength(20, ErrorMessage = "Toevoeging kan niet meer dan 20 karakters bevatten.")]
        public string? HouseNumberAddition { get; set; }

        [DisplayName("Postcode")]
        [StringLength(6, ErrorMessage = "Toevoeging kan niet meer dan 6 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string ZipCode { get; set; }

        [DisplayName("Stad")]
        [StringLength(20, ErrorMessage = "Stadnaam kan niet meer dan 20 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string City { get; set; }

        [DisplayName("Provincie")]
        [StringLength(20, ErrorMessage = "Toevoeging kan niet meer dan 20 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string Region { get; set; }

        // Lists
        public virtual ICollection<Camera> Cameras { get; set; }
    }
}
