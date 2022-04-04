using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizatonMethods.Models.Data.Abstract;

namespace OptimizatonMethods.Models.Data.EntityFramework
{
    public class EFUserRepository : IUserRepository
    {
        private readonly MO_courseContext _context;

        public EFUserRepository(MO_courseContext context)
        {
            _context = context;
        }
        public IEnumerable<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
