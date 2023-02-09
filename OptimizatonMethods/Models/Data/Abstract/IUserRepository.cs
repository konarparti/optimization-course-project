using System.Collections.Generic;

namespace OptimizatonMethods.Models.Data.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        System.Threading.Tasks.Task<bool> VerifyUserAsync(string username, string password);
        System.Threading.Tasks.Task SaveUserAsync(User user);

        //TODO: возможно стоит возвращать удаленного user 
        System.Threading.Tasks.Task DeleteUserAsync(long id);

    }
}
