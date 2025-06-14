using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uAccountPlan.Application.DTOs;
using uAccountPlan.Domain.Interfaces;

namespace uAccountPlan.Application.UseCases.Retrieve
{
    public class RetrieveAllAccountPlanUseCase
    {
        private readonly IAccountPlanService _accountPlanService;
        public RetrieveAllAccountPlanUseCase(IAccountPlanService accountPlanService)
        {
            _accountPlanService = accountPlanService ?? throw new ArgumentNullException(nameof(accountPlanService));
        }
        public async Task<RetrieveAllAccountPlanResponse> ExecuteAsync()
        {
            var accountPlans = await _accountPlanService.GetAllAccountPlansAsync();

            if (accountPlans == null || !accountPlans.Any())
            {
                return new RetrieveAllAccountPlanResponse(new List<AccountPlanDto>());
            }

            var accountPlanDtos = 
                    accountPlans.Select(ap => new AccountPlanDto
                    {
                        Id = ap.Id,
                        Code = ap.Code,
                        Name = ap.Name,
                        Type = ap.Type.ToString(),
                        AcceptsLaunches = ap.AcceptsLaunches
                    });

            return new RetrieveAllAccountPlanResponse(
                accountPlanDtos
            );
        }
    }
}
