using System.Security.Cryptography.X509Certificates;

namespace B2C2_Pi4_PFFF_Gaetano.Models
{
    public class CameraReportsViewModel
    {
        public CameraReport CameraReport { get; set; } = new CameraReport();
        public CameraLocation CameraLocation { get; set; } = new CameraLocation();
        public Camera Camera { get; set; } = new Camera();
    }
}
