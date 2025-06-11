using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using uAccountPlan.Domain.Entities;

namespace uAccountPlan.Domain.Interfaces
{
    public interface IAccountPlanInterface
    {
        Task<IEnumerable<AccountPlan>> GetAllAsync();
        Task<AccountPlan?> GetByIdAsync(Guid id);
        Task AddAsync(AccountPlan accountPlan);
        Task DeleteAsync(Guid id);
        Task<List<AccountPlan>> GetChildrenAsync(Guid? parentId);
    }
}
