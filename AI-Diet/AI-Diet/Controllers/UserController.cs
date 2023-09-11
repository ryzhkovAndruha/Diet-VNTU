using AI_Diet.Context;
using AI_Diet.Models.UserModels;
using AI_Diet.Services;
using Microsoft.AspNetCore.Mvc;

namespace AI_Diet.Controllers
{
    [Route("/api")]
    public class UserController : Controller
    {
        private UserProvider _userProvider;

        public UserController(UserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var users = _userProvider.GetAll();

            if (users == null)
            {
                return BadRequest("Bad request");
            }

            return Ok();
        }

        [HttpGet("Get")]
        public IActionResult Get(string id)
        {
            var user = _userProvider.GetById(id);

            if (user == null)
            {
                return BadRequest("Bad request");
            }

            return Ok();
        }

        [HttpPost("Create")]
        public IActionResult Create(User user)
        {
            try
            {
                _userProvider.Create(user);
            }
            catch (Exception ex)
            {
                return BadRequest("Bad request");
            }

            return Ok();
        }

        [HttpPut ("Update")]
        public IActionResult Update(User user)
        {
            try
            {
                _userProvider.Update(user);
            }
            catch(Exception ex)
            {
                return BadRequest("Bad request");
            }

            return Ok();
        }

        [HttpDelete ("Delete")]
        public IActionResult Delete(string id)
        {
            try
            {
                _userProvider.Delete(id);
            }
            catch (Exception ex)
            {
                return BadRequest("Bad request");
            }

            return Ok();
        }
    }
}
