using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2C2_Pi4_PFFF_Gaetano.Models
{
    public class CameraReport
    {
        // Individual Properties
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DisplayName("Beschrijving / Opmerking")]
        [StringLength(150, ErrorMessage = "Beschrijving kan niet meer dan 150 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string DescriptionRemark { get; set; }

        /*[Required]
        public string MediaUrl { get; set; } */

        // public string MediaType { get; set; }

        public Camera Camera { get; set; }

        public int CameraId { get; set; }

        public AppUser AppUser { get; set; }

        public string AppUserId { get; set; }
    }
}
