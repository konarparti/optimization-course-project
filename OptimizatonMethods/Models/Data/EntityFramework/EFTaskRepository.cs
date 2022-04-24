using OptimizatonMethods.Models.Data.Abstract;
using System.Collections.Generic;
using System.Linq;

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
            return _context.Tasks.ToList();
        }

        public Task GetTask(long id)
        {
            return _context.Tasks.First(t => t.Id == id);
        }

        public void SaveTask(Task task)
        {
            if (task.Id == 0)
                _context.Tasks.Add(task);
            else
            {
                var dbEntry = _context.Tasks.FirstOrDefault(t => t.Id == task.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = task.Name;
                    dbEntry.Alpha = task.Alpha;
                    dbEntry.Beta = task.Beta;
                    dbEntry.Delta = task.Delta;
                    dbEntry.Mu = task.Mu;
                    dbEntry.A = task.A;
                    dbEntry.G = task.G;
                    dbEntry.N = task.N;
                    dbEntry.Price = task.Price;
                    dbEntry.Step = task.Step;
                    dbEntry.T1min = task.T1min;
                    dbEntry.T1max = task.T1max;
                    dbEntry.T2min = task.T2min;
                    dbEntry.T2max = task.T2max;
                }
            }
            _context.SaveChanges();
        }

        public void DeleteTask(long id)
        {
            var value = _context.Tasks.Find(id);
            if (value != null)
                _context.Tasks.Remove(value);
            _context.SaveChanges();
        }
    }
}
