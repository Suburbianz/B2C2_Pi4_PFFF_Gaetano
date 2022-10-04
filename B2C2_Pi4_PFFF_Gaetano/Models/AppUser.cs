using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2C2_Pi4_PFFF_Gaetano.Models
{
    public class AppUser : IdentityUser
    {
        // Individual Properties
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DisplayName("Voornaam")]
        [StringLength(50, ErrorMessage = "Voornaam kan niet meer dan 50 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld")]
        [Column("FirstName")]
        // [RegularExpression(@"^[A-Z]+[a-z]*$")]
        public string FirstMidName { get; set; }

        [DisplayName("Achternaam")]
        [StringLength(50, ErrorMessage = "Achternaam kan niet meer dan 50 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld")]
        public string LastName { get; set; }

        // public string ProfilePictureUrl { get; set; }

        // public string PrivacySetting { get; set; }

        public int TotalScore { get; set; } = 0;

        // Lists
        public virtual ICollection<CameraReport> CameraReports { get; set; }

        public virtual ICollection<AppUserAchievement> AppUserAchievements { get; set; }
    }
}
