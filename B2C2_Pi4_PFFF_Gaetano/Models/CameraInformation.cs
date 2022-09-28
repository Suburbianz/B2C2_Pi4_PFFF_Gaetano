using Microsoft.CodeAnalysis.Options;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace B2C2_Pi4_PFFF_Gaetano.Models
{
    public class CameraInformation
    {
        [Key]
        public int id { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DisplayName("Beschrijving / Opmerking")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string DescriptionRemark { get; set; }

        [DisplayName("Modelnummer")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string ModelNumber { get; set; }

        [DisplayName("Serienummer")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string SerialNumber { get; set; }

        public Camera Camera { get; set; }
    }
}
