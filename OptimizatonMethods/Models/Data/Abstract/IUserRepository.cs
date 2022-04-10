using System.Collections.Generic;

namespace OptimizatonMethods.Models.Data.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(int id);
        void SaveUser(User user);
        
        //TODO: возможно стоит возвращать удаленного user 
        void DeleteUser(int id);

    }
}
