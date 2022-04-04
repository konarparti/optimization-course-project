using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizatonMethods.Models.Data.Abstract
{
    public interface IMethodRepository
    {
        IEnumerable<Method> GetAllMethods();
        Method GetMethod(int id);
        void SaveMethod(Method method);
        void DeleteMethod(int id);
    }
}
