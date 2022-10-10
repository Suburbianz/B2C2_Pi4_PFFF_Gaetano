using System.Security.Cryptography.X509Certificates;
using B2C2_Pi4_PFFF_Gaetano.Models;

namespace B2C2_Pi4_PFFF_Gaetano.ViewModels
{
    public class CameraReportsViewModel
    {
        public CameraReport? CameraReport { get; set; }
        public CameraLocation? CameraLocation { get; set; }
        public Camera? Camera { get; set; }
    }
}
