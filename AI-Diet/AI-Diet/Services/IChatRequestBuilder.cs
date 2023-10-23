using AI_Diet.Models.UserModels;

namespace AI_Diet.Services
{
    public interface IChatRequestBuilder
    {
        string BuildDietRequest(DietRequestModel user);
        string BuildTrainingRequest(DietData calloriesData);
    }
}
