using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace B2C2_Pi4_PFFF_Gaetano.Models
{
    public class Camera
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public CameraInformation CameraInformation { get; set; }

        public int CameraInformationId { get; set; }

        public CameraLocation CameraLocation { get; set; }

        public int CameraLocationId { get; set; }

        public virtual ICollection<CameraReport> CameraReports { get; set; }
    }
}
