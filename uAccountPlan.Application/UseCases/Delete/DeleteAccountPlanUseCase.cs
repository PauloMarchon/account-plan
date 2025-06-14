using System;
using uAccountPlan.Domain.Interfaces;

namespace uAccountPlan.Application.UseCases.Delete
{
    public class DeleteAccountPlanUseCase
    {
        private readonly IAccountPlanService _accountPlanService;
        public DeleteAccountPlanUseCase(IAccountPlanService accountPlanService)
        {
            _accountPlanService = accountPlanService ?? throw new ArgumentNullException(nameof(accountPlanService));
        }
        public async Task ExecuteAsync(DeleteAccountPlanRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            await _accountPlanService.DeleteAccountPlanAsync(request.Id);
        }
    }
}
