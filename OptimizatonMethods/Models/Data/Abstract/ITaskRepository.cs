using System.Collections.Generic;

namespace OptimizatonMethods.Models.Data.Abstract
{
    public interface ITaskRepository
    {
        IEnumerable<Task> GetAllTasks();
        Task GetTask(long id);
        void SaveTask(Task task);
        void DeleteTask(long id);

    }
}
