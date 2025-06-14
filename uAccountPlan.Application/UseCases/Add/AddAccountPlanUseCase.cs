using System;
using System.Drawing;
using uAccountPlan.Domain.Entities;
using uAccountPlan.Domain.Interfaces;

namespace uAccountPlan.Application.UseCases.Add
{
    public class AddAccountPlanUseCase
    {
        private readonly IAccountPlanService _accountPlanService;
        public AddAccountPlanUseCase(IAccountPlanService accountPlanService)
        {
            _accountPlanService = accountPlanService ?? throw new ArgumentNullException(nameof(accountPlanService));
        }
        public async Task ExecuteAsync(AddAccountPlanRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            var accountPlan = new AccountPlan
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                Name = request.Name,
                Type = Enum.Parse<AccountPlanType>(request.Type, true),
                AcceptsLaunches = request.AcceptsLaunches
            };

            await _accountPlanService.AddAccountPlanAsync(accountPlan);
        }
    }
}
