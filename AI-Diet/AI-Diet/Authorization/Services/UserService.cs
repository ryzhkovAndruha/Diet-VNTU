using AI_Diet.Authorization.Models;
using AI_Diet.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace AI_Diet.Authorization.Services
{
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;

        private const string LOWER_CASE = "abcdefghijklmnopqrstuvwxyz";
        private const string UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string DIGITS = "1234567890";

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AddUserResult> AddAsync(string email, string name)
        {
            var user = new User
            {
                Email = email,
                UserName = email,
                Name = name,
                EmailConfirmed = true
            };

            var password = CreatePassword();

            var result = await _userManager.CreateAsync(user, password);

            return new AddUserResult
            {
                IsSuccess = result.Succeeded,
                Password = result.Succeeded ? password : string.Empty,
                Errors = result.Errors?.Select(x => x.Description).ToList()
            };
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
                .Include(x => x.CalorieCalculatorData)
                .Include(x => x.DietData)
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

        private string CreatePassword()
        {
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();

            res.Append(GetNextSymbol(rnd, UPPER_CASE));
            res.Append(GetNextSymbol(rnd, LOWER_CASE));
            res.Append(GetNextSymbol(rnd, LOWER_CASE));
            res.Append(GetNextSymbol(rnd, UPPER_CASE));
            res.Append(GetNextSymbol(rnd, LOWER_CASE));
            res.Append(GetNextSymbol(rnd, LOWER_CASE));
            res.Append(GetNextSymbol(rnd, DIGITS));
            res.Append(GetNextSymbol(rnd, DIGITS));

            return res.ToString();
        }

        private char GetNextSymbol(Random rnd, string collection)
        {
            return collection[rnd.Next(collection.Length)];
        }
    }
}
