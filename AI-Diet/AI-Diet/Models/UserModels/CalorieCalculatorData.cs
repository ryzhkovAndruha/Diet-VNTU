﻿using static AI_Diet.Models.Enums;

namespace AI_Diet.Models.UserModels
{
    public class CalorieCalculatorData
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public Gender Gender { get; set; }
        public DietGoal Goal { get; set; }
        public string PhysicalActivity { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
