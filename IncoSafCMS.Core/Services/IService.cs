using System;
using IncosafCMS.Core.Data;

namespace IncosafCMS.Core.Services
{
    public interface IService : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
