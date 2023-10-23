namespace AI_Diet.Models.UserModels
{
    public class DietRequestModel
    {
        public DietData DietData { get; set; }
        public FoodDetails FoodDetails { get; set; }

        public DietRequestModel(DietData dietData, FoodDetails foodDetails)
        {
            DietData = dietData;
            FoodDetails = foodDetails;
        }
    }
}
