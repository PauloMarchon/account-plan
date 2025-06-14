using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uAccountPlan.Application.DTOs;

namespace uAccountPlan.Application.UseCases.Retrieve
{
    public class RetrieveAllAccountPlanResponse
    {
        public IEnumerable<AccountPlanDto> AccountPlans { get; set; }
        public RetrieveAllAccountPlanResponse(IEnumerable<AccountPlanDto> accountPlans)
        {
            AccountPlans = accountPlans ?? throw new ArgumentNullException(nameof(accountPlans));
        }
    }
}
