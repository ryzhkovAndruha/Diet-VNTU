using Microsoft.AspNetCore.Identity;

namespace AI_Diet.Models.UserModels
{
    public class User: IdentityUser
    {
        public string Name { get; set; }
        public string SecondName { get; set; }

        public CalorieCalculatorData CalorieCalculatorData { get; set; }
        public DietData DietData { get; set; }
    }
}
