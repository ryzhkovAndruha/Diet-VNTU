using AI_Diet.Context;
using AI_Diet.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace AI_Diet.Services
{
    public class UserProvider
    {
        private ApplicationContext _aplicationContext;

        public UserProvider(ApplicationContext applicationContext)
        {
            _aplicationContext = applicationContext;
        }

        public IEnumerable<User> GetAll()
        {
            return _aplicationContext.Users.ToList();
        }

        public User GetById(string id)
        {
            return _aplicationContext.Users.Where(p => p.Id == id).FirstOrDefault();
        }

        public void Create(User user)
        {
            _aplicationContext.Users.Add(user);
        }

        public void Update(User user)
        {
            var userUpdate = _aplicationContext.Users.Where(p => p.Id == user.Id).FirstOrDefault();
            userUpdate.Name = user.Name;
            userUpdate.SecondName = user.SecondName;
            userUpdate.Email = user.Email; 
            _aplicationContext.SaveChanges();
        }

        public void Delete(string id)
        {
            var userDelete = _aplicationContext.Users.Where(p => p.Id == id).FirstOrDefault();
            _aplicationContext.Users.Remove(userDelete);
        }
    }
}
