using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace B2C2_Pi4_PFFF_Gaetano.Models
{
    public class CameraReport
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Naam")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string Name { get; set; }

        /*[Required]
        public string MediaUrl { get; set; } */

        // public string MediaType { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public Camera? Camera { get; set; }

        public AppUser User { get; set; }

        public int AppUserId { get; set; }
    }
}
