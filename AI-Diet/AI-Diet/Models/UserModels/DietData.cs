using static AI_Diet.Models.Enums;

namespace AI_Diet.Models.UserModels
{
    public class DietData
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public Gender Gender { get; set; }
        public DietGoal Goal { get; set; }
        public string PhysicalActivity { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
