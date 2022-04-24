using System.Collections.Generic;

namespace OptimizatonMethods.Models.Data.Abstract
{
    public interface IMethodRepository
    {
        IEnumerable<Method> GetAllMethods();
        Method GetMethod(long id);
        void SaveMethod(Method method);
        void DeleteMethod(long id);
    }
}
