﻿namespace AI_Diet.Models.RequestModels
{
    public class AddFoodDetailsRequest
    {
        public string Allergies { get; set; }
        public string FoodRestrictions { get; set; }
        public string FoodPreferences { get; set; }
        public string DislikeFood { get; set; }
        public string UserId { get; set; }
    }
}
