using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Services;
using System.Collections.Generic;

namespace IncosafCMS.Services
{
    public class ActivityLogService : Service<ActivityLog>
    {
        public ActivityLogService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<ActivityLog> GetByUser(int userId)
        {
            return UnitOfWork.Repository<ActivityLog>().FindBy(x => x.UserId == userId);
        }
    }
}
