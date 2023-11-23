﻿using AI_Diet.Authorization.Services.Interfaces;
using AI_Diet.Context;
using AI_Diet.Models.RequestModels;
using AI_Diet.Models.ResponseModels;
using AI_Diet.Models.UserModels;

namespace AI_Diet.Authorization.Services
{
    public class UserService : IUserService
    {
        ApplicationContext _dbContext;

        public UserService(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddDietData(DietData dietData)
        {
            try
            {
                _dbContext.DietData.Add(dietData);
                _dbContext.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddDietToUser(AddDietToUserRequestModel dietModel)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(user => user.Id == dietModel.UserId);
                user.Diet = dietModel.Diet;
                _dbContext.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddTrainingToUser(AddTrainingToUserRequestModel trainingModel)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(user => user.Id == trainingModel.UserId);
                user.Training = trainingModel.TrainingProgramm;
                _dbContext.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public GetUserResponseModel GetUser(string userId)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(user => user.Id == userId);
                return new GetUserResponseModel(user);
            }
            catch 
            { 
                return null;
            }
        }

        public bool AddFoodDetails(FoodDetails foodDetails)
        {
            try
            {
                _dbContext.FoodDetails.Add(foodDetails);
                _dbContext.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
