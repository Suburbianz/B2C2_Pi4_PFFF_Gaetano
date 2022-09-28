using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace B2C2_Pi4_PFFF_Gaetano.Models
{
    public class AppUser : IdentityUser
    {
        [DisplayName("Voornaam")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        // public string ProfilePictureUrl { get; set; }

        // public string PrivacySetting { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public virtual ICollection<CameraReport> CameraReports { get; set; }

        // public virtual ICollection<Achievement> Achievements { get; set; }
    }
}
