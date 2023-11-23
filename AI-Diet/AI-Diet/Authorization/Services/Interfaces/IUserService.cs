using AI_Diet.Models.RequestModels;
using AI_Diet.Models.ResponseModels;
using AI_Diet.Models.UserModels;

namespace AI_Diet.Authorization.Services.Interfaces
{
    public interface IUserService
    {
        bool AddDietData(DietData dietDataRequest);
        bool AddFoodDetails(FoodDetails foodDetailsRequest);
        bool AddDietToUser(AddDietToUserRequestModel dietModel);
        bool AddTrainingToUser(AddTrainingToUserRequestModel trainingModel);
        GetUserResponseModel GetUser(string userId);
    }
}
