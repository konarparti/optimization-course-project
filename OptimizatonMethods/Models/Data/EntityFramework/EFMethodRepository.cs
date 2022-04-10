using OptimizatonMethods.Models.Data.Abstract;
using System.Collections.Generic;
using System.Linq;

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
            return _context.Method.ToList();
        }

        public Method GetMethod(int id)
        {
            return _context.Method.First(m => m.Id == id);
        }

        public void SaveMethod(Method method)
        {
            if (method.Id == 0)
                _context.Method.Add(method);
            else
            {
                var dbEntry = _context.Method.FirstOrDefault(m => m.Id == method.Id);
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
            var value = _context.Method.Find(id);
            if (value != null)
                _context.Method.Remove(value);
            _context.SaveChanges();
        }
    }
}
