using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizatonMethods.Models.Data.Abstract;

namespace OptimizatonMethods.Models.Data.EntityFramework
{
    public class EFMethodRepository : IMethodRepository
    {
        private readonly MO_courseContext _context;

        public EFMethodRepository(MO_courseContext context)
        {
            _context = context;
        }
        public IEnumerable<Method> GetAllMethods()
        {
            throw new NotImplementedException();
        }

        public Method GetMethod(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveMethod(Method method)
        {
            throw new NotImplementedException();
        }

        public void DeleteMethod(int id)
        {
            throw new NotImplementedException();
        }
    }
}
