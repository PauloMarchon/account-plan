using System;
using uAccountPlan.Domain.Entities;
using uAccountPlan.Domain.Interfaces;

namespace uAccountPlan.Domain.Services
{
    public class AccountPlanService : IAccountPlanService
    {
        private readonly IAccountPlanRepository _accountPlanRepository;
        
        public AccountPlanService(IAccountPlanRepository accountPlanRepository)
        {
            _accountPlanRepository = accountPlanRepository ?? throw new ArgumentNullException(nameof(accountPlanRepository));
        }

        public async Task AddAccountPlanAsync(AccountPlan accountPlan)
        {
            if(accountPlan.ParentId != null)
            {
                var parentAccountPlan = await _accountPlanRepository.GetByIdAsync(accountPlan.ParentId.Value);
                if (parentAccountPlan == null)
                {
                    throw new ArgumentException($"Parent account plan with ID {accountPlan.ParentId} does not exist.");
                }
            }

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
