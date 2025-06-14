using System;
using System.Reflection.Metadata;
using Moq;
using uAccountPlan.Domain.Entities;
using uAccountPlan.Domain.Interfaces;
using uAccountPlan.Domain.Services;

namespace uAccountPlan.Domain.Tests.Services
{
    public class AccountPlanServiceTests
    {

        [Fact]
        public async Task GetAccountPlanByIdAsync_ShouldReturnAccountPlan_WhenFound()
        {
            // Arrange
            var accountPlanId = Guid.NewGuid();
            var accountPlanParentId = Guid.NewGuid();
            var expectedAccountPlan = new AccountPlan {
                Id = accountPlanId,
                Code = "1.1",
                Name = "Despesa Filha Teste",
                Type = AccountPlanType.EXPENSE,
                AcceptsLaunches = true,
                ParentId = accountPlanParentId
            };

            var mockAccountPlanRepository = new Mock<IAccountPlanRepository>();

            mockAccountPlanRepository.Setup(repo => repo.GetByIdAsync(accountPlanId)).ReturnsAsync(expectedAccountPlan);

            var service = new AccountPlanService(mockAccountPlanRepository.Object);

            // Act 
            var result = await service.GetAccountPlanByIdAsync(accountPlanId);

            // Assert 
            Assert.NotNull(result);
            Assert.Equal(expectedAccountPlan.Id, result.Id);
            Assert.Equal(expectedAccountPlan.Code, result.Code);
            Assert.Equal(expectedAccountPlan.Name, result.Name);
            Assert.Equal(expectedAccountPlan.Type, result.Type);
            Assert.Equal(expectedAccountPlan.AcceptsLaunches, result.AcceptsLaunches);
            Assert.Equal(expectedAccountPlan.ParentId, result.ParentId);

            mockAccountPlanRepository.Verify(repo => repo.GetByIdAsync(accountPlanId), Times.Once);
        }

        [Fact]
        public async Task GetAccountPlanByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
   
            var mockAccountPlanRepository = new Mock<IAccountPlanRepository>();

            mockAccountPlanRepository.Setup(repo => repo.GetByIdAsync(nonExistentId)).ReturnsAsync((AccountPlan?)null);

            var service = new AccountPlanService(mockAccountPlanRepository.Object);

            // Act
            var result = await service.GetAccountPlanByIdAsync(nonExistentId);

            // Assert
            Assert.Null(result);

            mockAccountPlanRepository.Verify(repo => repo.GetByIdAsync(nonExistentId), Times.Once);
        }

    }
}
