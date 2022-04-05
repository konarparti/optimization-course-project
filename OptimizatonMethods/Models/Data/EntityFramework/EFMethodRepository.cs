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
            return _context.Methods.ToList();
        }

        public Method GetMethod(int id)
        {
            return _context.Methods.First(m => m.Id == id);
        }

        public void SaveMethod(Method method)
        {
            if (method.Id == 0)
                _context.Methods.Add(method);
            else
            {
                var dbEntry = _context.Methods.FirstOrDefault(m => m.Id == method.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = method.Name;
                    dbEntry.Activated = method.Activated;
                }
            }
            _context.SaveChanges();

        }

        public void DeleteMethod(int id)
        {
            var value = _context.Methods.Find(id);
            if (value != null)
                _context.Methods.Remove(value);
            _context.SaveChanges();
        }
    }
}
