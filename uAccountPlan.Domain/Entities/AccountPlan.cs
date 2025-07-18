﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private AccountPlan(string code, string name, AccountPlanType type, bool acceptsLaunches, Guid? parentId)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Code cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.");

            if (type != AccountPlanType.REVENUE && type != AccountPlanType.EXPENSE)
                throw new ArgumentException("Type must be either REVENUE or EXPENSE.");

            Id = Guid.NewGuid();
            Code = code;
            Name = name;
            Type = type;
            AcceptsLaunches = acceptsLaunches;
            ParentId = parentId;
        }

        public static AccountPlan Create(string code, string name, AccountPlanType type, bool acceptsLaunches, Guid? parentId = null)
        {
            if (parentId.HasValue && parentId.Value == Guid.Empty)
                throw new ArgumentException("ParentId cannot be empty if provided.");

            return new AccountPlan(code, name, type, acceptsLaunches, parentId);
        }
    }
}
