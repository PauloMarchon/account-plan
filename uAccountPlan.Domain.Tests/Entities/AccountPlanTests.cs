using System;
using System.Security.Principal;
using Microsoft.Win32;
using uAccountPlan.Domain.Entities;

namespace uAccountPlan.Domain.Tests.Entities
{
    public class AccountPlanTests
    {   

        [Fact]
        public void AccountPlan_ShouldBeCreatedSuccessfully_WithValidDataAndNullParent()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedCode = "1.0";
            var expectedName = "Receita Pai";
            var expectedType = AccountPlanType.REVENUE;
            var expectedAcceptsLaunches = false;

            // Act
            var accountPlan = new AccountPlan
            {
                Id = expectedId,
                Code = expectedCode,
                Name = expectedName,
                Type = expectedType,
                AcceptsLaunches = expectedAcceptsLaunches
            };
            
            // Assert
            Assert.NotNull(accountPlan);
            Assert.Equal(expectedId, accountPlan.Id);
            Assert.Equal(expectedName, accountPlan.Name);
            Assert.Equal(expectedCode, accountPlan.Code);
            Assert.Equal(expectedType, accountPlan.Type);
            Assert.Equal(expectedAcceptsLaunches, accountPlan.AcceptsLaunches);
            Assert.Null(accountPlan.ParentId); 
        }

        [Fact]
        public void AccountPlan_ShouldBeCreatedSuccessfully_WithValidDataAndParentId()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedCode = "1.1";
            var expectedName = "Receita Filha";
            var expectedType = AccountPlanType.REVENUE;
            var expectedAcceptsLaunches = true;
            var expectedParentId = Guid.NewGuid();

            // Act
            var accountPlan = new AccountPlan
            {
                Id = expectedId,
                Code = expectedCode,
                Name = expectedName,
                Type = expectedType,
                AcceptsLaunches = expectedAcceptsLaunches,
                ParentId = expectedParentId
            };

            // Assert
            Assert.NotNull(accountPlan);
            Assert.Equal(expectedId, accountPlan.Id);
            Assert.Equal(expectedName, accountPlan.Name);
            Assert.Equal(expectedCode, accountPlan.Code);
            Assert.Equal(expectedType, accountPlan.Type);
            Assert.Equal(expectedAcceptsLaunches, accountPlan.AcceptsLaunches);
            Assert.Equal(expectedParentId, accountPlan.ParentId);
        }
    }
}
