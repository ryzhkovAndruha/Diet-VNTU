﻿using AI_Diet.Authorization.Models;

namespace AI_Diet.Authorization.Services
{
    public interface IUserService
    {
        Task<bool> ChangePasswordAsync(string email, string passworrd, string newPassword);
        Task<bool> RemoveAsync(string email);
        Task<IEnumerable<GetUserModel>> GetAsync();
    }
}
