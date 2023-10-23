using AI_Diet.Authorization.Models;
using AI_Diet.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AI_Diet.Authorization.Services
{
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> ChangePasswordAsync(string email, string passworrd, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                throw new Exception("User not exists");
            }

            var result = await _userManager.ChangePasswordAsync(user, passworrd, newPassword);

            return result.Succeeded;
        }

        public async Task<IEnumerable<GetUserModel>> GetAsync()
        {
            return await _userManager.Users
                .Include(x => x.DietData)
                .Include(x => x.FoodDetails)
                .Select(x => new GetUserModel
                {
                    Email = x.Email,
                    Name = x.Name,
                })
                .ToListAsync();
        }

        public async Task<bool> RemoveAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                throw new Exception("User not exists");
            }
            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }
    }
}
