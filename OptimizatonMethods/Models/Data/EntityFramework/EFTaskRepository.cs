using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizatonMethods.Models.Data.Abstract;

namespace OptimizatonMethods.Models.Data.EntityFramework
{
    public class EFTaskRepository : ITaskRepository
    {
        private readonly MO_courseContext _context;

        public EFTaskRepository(MO_courseContext context)
        {
            _context = context;
        }
        public IEnumerable<Task> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public Task GetTask(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveTast(Task task)
        {
            throw new NotImplementedException();
        }

        public void DeleteTask(int id)
        {
            throw new NotImplementedException();
        }
    }
}
