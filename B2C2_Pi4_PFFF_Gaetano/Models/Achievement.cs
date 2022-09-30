using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2C2_Pi4_PFFF_Gaetano.Models
{
    public class Achievement
    {
         
        //Individual Properties
        [Key]
        public int Id { get; set; }

        [DisplayName("Naam")]
        [StringLength(50, ErrorMessage = "Achievementnaam kan niet meer dan 50 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld")]
        public string Name { get; set; }

        [DisplayName("Inhoud")]
        [StringLength(150, ErrorMessage = "Achievementnaam kan niet meer dan 150 karakters bevatten.")]
        [Required(ErrorMessage = "Dit veld moet worden ingevuld")]
        public string Content { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        // Lists
        public virtual ICollection<AppUserAchievement> AppUserAchievements { get; set; }
    }
}

