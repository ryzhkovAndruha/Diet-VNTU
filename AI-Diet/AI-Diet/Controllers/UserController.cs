using AI_Diet.Authorization.Services.Interfaces;
using AI_Diet.Models.RequestModels;
using AI_Diet.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AI_Diet.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IAuthService _authService;
        private IUserService _userService;

        public UserController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUserAsync([FromBody]RegisterUserRequest registerUserRequestModel)
        {
            if (registerUserRequestModel.Password != registerUserRequestModel.RepeatPassword)
            {
                return BadRequest("Passwords are not matching");
            }

            var registerResponse = await _authService.RegisterAsync(registerUserRequestModel);

            if (registerResponse is RegisterUserResponseErrors registerResponseErrors)
            {
                return BadRequest(registerResponseErrors.RegisterErrors);
            }

            return Ok(registerResponse);
        }

        [Authorize]
        [HttpPost("add-diet-data")]
        public async Task<ActionResult> AddDietData([FromBody] AddDietDataRequest addDietDataRequest)
        {
            var result = _userService.AddDietData(new Models.UserModels.DietData(addDietDataRequest));

            return result ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("add-food-details")]
        public async Task<ActionResult> AddFoodDetails([FromBody] AddFoodDetailsRequest addFoodDetailsRequest)
        {
            var result = _userService.AddFoodDetails(new Models.UserModels.FoodDetails(addFoodDetailsRequest));

            return result ? Ok() : BadRequest();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteUserAsync(string userId)
        {
            var deleteResponse = await _authService.DeleteUserAsync(userId);

            Response.Cookies.Delete("Refresh-Token", new CookieOptions() { HttpOnly = true });
            return deleteResponse.IsDeleted ? Ok() : BadRequest(deleteResponse.Errors);
        }
    }
}
