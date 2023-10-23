namespace AI_Diet.Models.UserModels
{
    public class FoodDetails
    {
        public int Id { get; set; }
        public string Allergies { get; set; }
        public string FoodRestrictions { get; set; }
        public string FoodPreferences { get; set; }
        public string DislikeFood { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
