using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface ITaskRepository
    {
        void Add(Task item);
        void Edit(Task item);
        void Remove(string id);
        IEnumerable<Task> GetTasks();
        Task FindByID(string id);
    }
}
