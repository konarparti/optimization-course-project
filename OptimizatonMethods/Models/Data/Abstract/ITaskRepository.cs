using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizatonMethods.Models.Data.Abstract
{
    public interface ITaskRepository
    {
        IEnumerable<Task> GetAllTasks();
        Task GetTask(int id);
        void SaveTask(Task task);
        void DeleteTask(int id);

    }
}
