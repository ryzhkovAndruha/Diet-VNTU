using AI_Diet.Models.RequestModels;
using Microsoft.AspNetCore.Identity;

namespace AI_Diet.Models.UserModels
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string RefreshToken { get; set; }

        public DietData DietData { get; set; }
        public FoodDetails FoodDetails { get; set; }

        public User()
        {}

        public User(RegisterUserRequest registerUserRequestModel)
        {
            Name = registerUserRequestModel.Name;
            SecondName = registerUserRequestModel.SecondName;
            Email = registerUserRequestModel.Email;
            UserName = registerUserRequestModel.Email;
        }
    }
}
