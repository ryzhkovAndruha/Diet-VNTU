using AI_Diet.Models.UserModels;
using static AI_Diet.Models.Enums;

namespace AI_Diet.Models.ResponseModels
{
    public class GetUserResponseModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public Gender Gender { get; set; }
        public DietGoal Goal { get; set; }
        public string PhysicalActivity { get; set; }

        public string Diet { get; set; }
        public string Training { get; set; }

        public GetUserResponseModel(User user)
        {
            Name = user.Name;
            Age = user.DietData.Age;
            Height = user.DietData.Height;
            Weight = user.DietData.Weight;
            Gender = user.DietData.Gender;
            PhysicalActivity = user.DietData.PhysicalActivity;

            Diet = user.Diet;
            Training = user.Training;
        }
    }
}
