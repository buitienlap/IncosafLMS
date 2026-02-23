using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface IGroupRepository
    {
        void Add(UserGroup item);
        void Edit(UserGroup item);
        void Remove(string id);
        IEnumerable<UserGroup> GetUserGroups();
        UserGroup FindByID(string id);
    }
}
