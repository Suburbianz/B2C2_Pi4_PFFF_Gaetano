using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2C2_Pi4_PFFF_Gaetano.Models
{
    public class Camera
    {
        //Individual Properties
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DisplayName("Naam")]
        [StringLength(50, ErrorMessage = "Naam kan niet meer dan 50 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string Name { get; set; }

        [DisplayName("Modelnummer")]
        [StringLength(100, ErrorMessage = "Modelnummer kan niet meer dan 100 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string ModelNumber { get; set; }

        [DisplayName("Serienummer")]
        [StringLength(100, ErrorMessage = "Serienummer kan niet meer dan 100 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld.")]
        public string SerialNumber { get; set; }

        public virtual CameraLocation CameraLocation { get; set; }

        public int CameraLocationId { get; set; }

        //Lists
        public virtual ICollection<CameraReport> CameraReports { get; set; }
    }
}
