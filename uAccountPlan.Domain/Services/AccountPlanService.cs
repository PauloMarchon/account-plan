using System;
using uAccountPlan.Domain.Entities;
using uAccountPlan.Domain.Interfaces;

namespace uAccountPlan.Domain.Services
{
    public class AccountPlanService : IAccountPlanService
    {
        private readonly IAccountPlanInterface _accountPlanRepository;
        
        public AccountPlanService(IAccountPlanInterface accountPlanRepository)
        {
            _accountPlanRepository = accountPlanRepository ?? throw new ArgumentNullException(nameof(accountPlanRepository));
        }

        public async Task CreateAccountPlanAsync(AccountPlan accountPlan)
        {
            await _accountPlanRepository.AddAsync(accountPlan);
        }

        public async Task DeleteAccountPlanAsync(Guid id)
        {
            await _accountPlanRepository.DeleteAsync(id);
        }

        public async Task<AccountPlan?> GetAccountPlanByIdAsync(Guid id)
        {
            return await _accountPlanRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<AccountPlan>> GetAllAccountPlansAsync()
        {
            return await _accountPlanRepository.GetAllAsync();
        }
    }
}
