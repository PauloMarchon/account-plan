using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uAccountPlan.Domain.Entities;

namespace uAccountPlan.Domain.Interfaces
{
    public interface IAccountPlanService
    {
        Task<IEnumerable<AccountPlan>> GetAllAccountPlansAsync();
        Task<AccountPlan?> GetAccountPlanByIdAsync(Guid id);
        Task AddAccountPlanAsync(AccountPlan accountPlan);
        Task DeleteAccountPlanAsync(Guid id);
        Task DeleteAccountPlanAsync(AccountPlan accountPlan);
    }
}
