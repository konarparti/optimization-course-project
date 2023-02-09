using Microsoft.EntityFrameworkCore;
using OptimizatonMethods;
using OptimizatonMethods.Models.Data.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace OptimizatonMethods.Models.Data.EntityFramework
{
    public class EFUserRepository : IUserRepository
    {
        private readonly MO_courseContext _context;

        public EFUserRepository(MO_courseContext context)
        {
            _context = context;
        }
        public IEnumerable<User> GetAllUsers() => _context.Users.ToList();

        public async System.Threading.Tasks.Task<bool> VerifyUserAsync(string username, string password)
        {
            var value = await _context.Users.FirstOrDefaultAsync(x => (x.Username == username && x.Password == password));
            if (value != null)
                return true;
            else
                return false;
        }


        public async System.Threading.Tasks.Task SaveUserAsync(User user)
        {
            if (user.Id == 0)
                await _context.Users.AddAsync(user);
            else
            {
                var dbEntry = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
                if (dbEntry != null)
                {
                    dbEntry.Username = user.Username;
                    dbEntry.Password = user.Password;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(long id)
        {
            var value = await _context.Users.FindAsync(id);
            if (value != null)
                _context.Users.Remove(value);
            await _context.SaveChangesAsync();
        }
    }
}
