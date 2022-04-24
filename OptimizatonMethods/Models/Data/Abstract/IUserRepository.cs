using System.Collections.Generic;

namespace OptimizatonMethods.Models.Data.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        bool VerifyUser(string username, string password);
        void SaveUser(User user);
        
        //TODO: возможно стоит возвращать удаленного user 
        void DeleteUser(long id);

    }
}
