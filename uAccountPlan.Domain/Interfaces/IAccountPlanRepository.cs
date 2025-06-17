using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uAccountPlan.Domain.Entities;

namespace uAccountPlan.Domain.Interfaces
{
    public interface IAccountPlanRepository
    {
        Task<IEnumerable<AccountPlan>> GetAllAsync();
        Task<AccountPlan?> GetByIdAsync(Guid id);
        Task AddAsync(AccountPlan accountPlan);
        Task DeleteAsync(Guid id);
        Task DeleteAsync(AccountPlan accountPlan);
        Task<List<AccountPlan>> GetChildrenAsync(Guid? parentId);
    }
}
