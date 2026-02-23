using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface IPermissionRepository
    {
        void Add(Permission item);
        void Edit(Permission item);
        void Remove(string id);
        IEnumerable<Permission> GetPermission();
        Permission FindByID(string id);
    }
}
