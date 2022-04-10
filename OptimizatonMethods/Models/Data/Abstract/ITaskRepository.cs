using System.Collections.Generic;

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
