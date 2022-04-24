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

        public bool VerifyUser(string username, string password)
        {
            var value =_context.Users.FirstOrDefault(x => (x.Username == username && x.Password == password));
            if (value != null)
                return true;
            else
                return false;
        }


        public void SaveUser(User user)
        {
            if (user.Id == 0)
                _context.Users.Add(user);
            else
            {
                var dbEntry = _context.Users.FirstOrDefault(u => u.Id == user.Id);
                if (dbEntry != null)
                {
                    dbEntry.Username = user.Username;
                    dbEntry.Password = user.Password;
                }
            }
            _context.SaveChanges();
        }

        public void DeleteUser(long id)
        {
            var value = _context.Users.Find(id);
            if (value != null)
                _context.Users.Remove(value);
            _context.SaveChanges();
        }
    }
}
