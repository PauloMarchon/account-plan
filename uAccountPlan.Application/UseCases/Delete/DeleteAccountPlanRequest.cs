using System;

namespace uAccountPlan.Application.UseCases.Delete
{
    public record DeleteAccountPlanRequest
    {
        public Guid Id { get; set; }
    }
}
