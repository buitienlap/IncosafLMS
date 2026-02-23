using IncoSafCMS.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncoSafCMS.Core;

namespace IncoSafCMS.Infrastructure
{
    public class TaskRepository : ITaskRepository
    {
        public void Add(Core.Task item)
        {
            throw new NotImplementedException();
        }

        public void Edit(Core.Task item)
        {
            throw new NotImplementedException();
        }

        public Core.Task FindByID(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Core.Task> GetTasks()
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }
    }
}
