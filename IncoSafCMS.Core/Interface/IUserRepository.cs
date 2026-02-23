using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface IUserRepository
    {
        void Add(User item);
        void Edit(User item);
        void Remove(string id);
        IEnumerable<User> GetUsers();
        User FindByID(string id);
    }
}
