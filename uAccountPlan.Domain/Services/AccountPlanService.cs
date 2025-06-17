using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uAccountPlan.Domain.Entities;
using uAccountPlan.Domain.Interfaces;

namespace uAccountPlan.Domain.Services
{
    public class AccountPlanService : IAccountPlanService
    {
        private readonly IAccountPlanRepository _accountPlanRepository;

        public AccountPlanService(IAccountPlanRepository accountPlanRepository)
        {
            _accountPlanRepository = accountPlanRepository;
        }

        public async Task AddAccountPlanAsync(AccountPlan accountPlan)
        {
            if (accountPlan.ParentId != null)
            {
                var parentAccountPlan = await _accountPlanRepository.GetByIdAsync(accountPlan.ParentId.Value);

                if (parentAccountPlan == null)
                    throw new ArgumentException("Parent account plan does not exist.");

                if (parentAccountPlan.Type != accountPlan.Type)
                    throw new ArgumentException("Child account plan type must match parent account plan type.");
            }

            var existingAccountPlan = (await _accountPlanRepository.GetAllAsync())
                .FirstOrDefault(ap => ap.Code == accountPlan.Code);

            if (existingAccountPlan != null)
                throw new ArgumentException("An account plan with code already exists.");

            await _accountPlanRepository.AddAsync(accountPlan);
        }

        public async Task DeleteAccountPlanAsync(Guid id)
        {
            await _accountPlanRepository.DeleteAsync(id);
        }

        public async Task DeleteAccountPlanAsync(AccountPlan accountPlan)
        {
            await _accountPlanRepository.DeleteAsync(accountPlan);
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
