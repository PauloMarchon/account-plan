using System;

namespace uAccountPlan.Application.DTOs
{
    public record AccountPlanDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool AcceptsLaunches { get; set; }
    }
}
