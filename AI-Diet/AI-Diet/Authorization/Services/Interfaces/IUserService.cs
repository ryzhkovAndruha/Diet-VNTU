using AI_Diet.Models.RequestModels;
using AI_Diet.Models.UserModels;

namespace AI_Diet.Authorization.Services.Interfaces
{
    public interface IUserService
    {
        bool AddDietData(DietData dietDataRequest);
        bool AddFoodDetails(FoodDetails foodDetailsRequest);
    }
}
