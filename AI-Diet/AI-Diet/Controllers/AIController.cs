using AI_Diet.Authorization.Services;
using AI_Diet.Context;
using AI_Diet.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AI_Diet.Controllers
{
    [Authorize]
    [Route("api/chat-gpt")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private IChatGptService _chatGptService;
        private IChatRequestBuilder _requestBuilder;

        private ApplicationContext _applicationContext;
        
        public AIController(IChatGptService chatGptService, IChatRequestBuilder requestBuilder,ApplicationContext applicationContext)
        {
            _chatGptService = chatGptService;
            _requestBuilder = requestBuilder;

            _applicationContext = applicationContext;
        }

        [HttpGet("ask-for-diet")]
        public IActionResult AskForDiet(string userId)
        {
            try
            {
                var dietData = _applicationContext.DietData.FirstOrDefault(c => c.UserId == userId);
                var foodDetails = _applicationContext.FoodDetails.FirstOrDefault(d => d.UserId == userId);

                if (dietData == null)
                {
                    return NotFound("User not found");
                }

                var dietRequest = _requestBuilder.BuildDietRequest(new DietRequestModel(dietData, foodDetails));
                var chatResponse = _chatGptService.Ask(dietRequest);

                return Ok(chatResponse);

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("ask-for-training")]
        public IActionResult AskForTraining(string userId)
        {
            try
            {
                var dietData = _applicationContext.DietData.FirstOrDefault(c => c.UserId == userId);

                if (dietData == null)
                {
                    return NotFound("User not found");
                }

                var dietRequest = _requestBuilder.BuildTrainingRequest(dietData);
                var chatResponse = _chatGptService.Ask(dietRequest);

                return Ok(chatResponse);

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
