namespace B2C2_Pi4_PFFF_Gaetano.Models
{
    public class AppUserAchievement
    {
        public string? AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int? AchievementId { get; set; }
        public Achievement Achievement { get; set; }
    }
}
