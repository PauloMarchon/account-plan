using System;

namespace uAccountPlan.Domain.Entities
{
    public class AccountPlan
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public AccountPlanType Type { get; set; }
        public bool AcceptsLaunches { get; set; }
        public Guid? ParentId { get; set; }
    }
}
