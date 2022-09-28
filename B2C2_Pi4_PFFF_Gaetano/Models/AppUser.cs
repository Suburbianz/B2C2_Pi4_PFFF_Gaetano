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
    }
}
