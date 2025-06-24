using Moq;
using uAccountPlan.Domain.Entities;
using uAccountPlan.Domain.Interfaces;
using uAccountPlan.Domain.Services;

namespace uAccountPlan.Domain.Tests.Services
{
    public class AccountPlanServiceTests
    {
        private readonly Mock<IAccountPlanRepository> _mockAccountPlanRepository;
        private readonly IAccountPlanService _service;

        public AccountPlanServiceTests()
        {
            _mockAccountPlanRepository = new Mock<IAccountPlanRepository>();
            _service = new AccountPlanService(_mockAccountPlanRepository.Object);
        }

        [Fact]
        public async Task GetAllAccountPlansAsync_ShouldReturnAllAccountPlans()
        {
            // Arrange
            var parentId = Guid.NewGuid();

            var accountPlans = new List<AccountPlan>
            {
                AccountPlan.Create("2.0", "Parent Account Plan", AccountPlanType.REVENUE, false ),
                AccountPlan.Create("3.0", "Parent Account Plan", AccountPlanType.REVENUE, false ),
                AccountPlan.Create("3.1", "Child Account Plan 1", AccountPlanType.REVENUE, true, parentId),
            };
            
            _mockAccountPlanRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(accountPlans);
            
            // Act
            var result = await _service.GetAllAccountPlansAsync();
            
            // Assert
            Assert.Equal(3, result.Count());
            Assert.Contains(result, ap => ap.Code == "2.0");
            Assert.Contains(result, ap => ap.Code == "3.0");
            Assert.Contains(result, ap => ap.Code == "3.1");

            _mockAccountPlanRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAccountPlanByIdAsync_ShouldReturnAccountPlan_WhenExists()
        {
            // Arrange
            var accountPlan = AccountPlan
                .Create("2.0", "Parent Account Plan", AccountPlanType.REVENUE, false);
            
            _mockAccountPlanRepository.Setup(repo => repo.GetByIdAsync(accountPlan.Id)).ReturnsAsync(accountPlan);
            
            // Act
            var result = await _service.GetAccountPlanByIdAsync(accountPlan.Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(accountPlan.Code, result.Code);
            Assert.Equal(accountPlan.Name, result.Name);
            Assert.Equal(accountPlan.Type, result.Type);
            Assert.Equal(accountPlan.AcceptsLaunches, result.AcceptsLaunches);

            _mockAccountPlanRepository.Verify(repo => repo.GetByIdAsync(accountPlan.Id), Times.Once);
        }

        [Fact]
        public async Task GetAccountPlanByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            
            _mockAccountPlanRepository.Setup(repo => repo.GetByIdAsync(nonExistentId)).ReturnsAsync((AccountPlan?)null);
            
            // Act
            var result = await _service.GetAccountPlanByIdAsync(nonExistentId);
            
            // Assert
            Assert.Null(result);

            _mockAccountPlanRepository.Verify(repo => repo.GetByIdAsync(nonExistentId), Times.Once);
        }

        [Fact]
        public async Task AddAccountPlanAsync_ShouldAddAccountPlan_WhenValid()
        {
            // Arrange
            var accountPlan = AccountPlan
                .Create("2.0", "Parent Account Plan", AccountPlanType.REVENUE, false);
            
            _mockAccountPlanRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<AccountPlan>());
            
            _mockAccountPlanRepository.Setup(repo => repo.AddAsync(It.IsAny<AccountPlan>())).Returns(Task.CompletedTask);

            // Act
            await _service.AddAccountPlanAsync(accountPlan);
            
            // Assert
            _mockAccountPlanRepository.Verify(repo => repo.AddAsync(accountPlan), Times.Once);
        }

        [Fact]
        public async Task AddAccountPlanAsync_ShouldThrow_WhenCodeAlreadyExists()
        {
            // Arrange
            var newAccountPlanCode = "2.0";
            
            var newAccountPlan = AccountPlan
                .Create(newAccountPlanCode, "Parent Account Plan", AccountPlanType.REVENUE, false);
            
            var existingAccountPlan = AccountPlan
                .Create(newAccountPlanCode, "Parent Account Plan", AccountPlanType.REVENUE, false);

            _mockAccountPlanRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<AccountPlan> { existingAccountPlan });
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAccountPlanAsync(newAccountPlan));
            
            Assert.Equal("An account plan with code already exists.", exception.Message);

            _mockAccountPlanRepository.Verify(repo => repo.AddAsync(It.IsAny<AccountPlan>()), Times.Never);
        }

        [Fact]
        public async Task AddAccountPlanAsync_ShouldThrowArgumentException_WhenParentAccountPlanDoesNotExist()
        {
            // Arrange
            var nonExistingParentId = Guid.NewGuid();
            var childAccountPlan = AccountPlan
                .Create("3.1", "Child Account Plan", AccountPlanType.REVENUE, true, nonExistingParentId);
            
            _mockAccountPlanRepository.Setup(repo => repo.GetByIdAsync(nonExistingParentId)).ReturnsAsync((AccountPlan?)null);

            _mockAccountPlanRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<AccountPlan>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAccountPlanAsync(childAccountPlan));
            
            Assert.Equal("Parent account plan does not exist.", exception.Message);
           
            _mockAccountPlanRepository.Verify(repo => repo.AddAsync(It.IsAny<AccountPlan>()), Times.Never);
        }

        [Fact]
        public async Task AddAccountPlanAsync_ShouldThrowArgumentException_WhenChildTypeDoesNotMatchParentType()
        {
            // Arrange
            var parentAccountPlan = AccountPlan
                .Create("2.0", "Parent Account Plan", AccountPlanType.REVENUE, false);
            
            var childAccountPlan = AccountPlan
                .Create("2.1", "Child Account Plan", AccountPlanType.EXPENSE, true, parentAccountPlan.Id);
            
            _mockAccountPlanRepository.Setup(repo => repo.GetByIdAsync(parentAccountPlan.Id)).ReturnsAsync(parentAccountPlan);
            
            _mockAccountPlanRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<AccountPlan>());
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAccountPlanAsync(childAccountPlan));
            
            Assert.Equal("Child account plan type must match parent account plan type.", exception.Message);
            
            _mockAccountPlanRepository.Verify(repo => repo.AddAsync(It.IsAny<AccountPlan>()), Times.Never);
        }

        [Fact]
        public async Task AddAccountPlanAsync_ShouldThrowArgumentException_WhenParentAccountPlanAcceptsLaunches()
        {
            // Arrange
            var parentAccountPlan = AccountPlan
                .Create("2.0", "Parent Account Plan", AccountPlanType.REVENUE, true);
            
            var childAccountPlan = AccountPlan
                .Create("2.1", "Child Account Plan", AccountPlanType.REVENUE, true, parentAccountPlan.Id);
            
            _mockAccountPlanRepository.Setup(repo => repo.GetByIdAsync(parentAccountPlan.Id)).ReturnsAsync(parentAccountPlan);
            
            _mockAccountPlanRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<AccountPlan>());
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAccountPlanAsync(childAccountPlan));
            
            Assert.Equal("The account that accepts entries cannot have child accounts.", exception.Message);
            
            _mockAccountPlanRepository.Verify(repo => repo.AddAsync(It.IsAny<AccountPlan>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAccountPlanAsync_ShouldDeleteById_WhenExists()
        {
            // Arrange
            var accountPlan = AccountPlan
                .Create("2.0", "Parent Account Plan", AccountPlanType.REVENUE, false);
            
            _mockAccountPlanRepository.Setup(repo => repo.DeleteAsync(accountPlan.Id)).Returns(Task.CompletedTask);
            
            // Act
            await _service.DeleteAccountPlanAsync(accountPlan.Id);
            
            // Assert
            _mockAccountPlanRepository.Verify(repo => repo.DeleteAsync(accountPlan.Id), Times.Once);
        }

        [Fact]
        public async Task DeleteAccountPlanAsync_ShouldDelete_WhenAccountPlanExists()
        {
            // Arrange
            var accountPlan = AccountPlan
                .Create("2.0", "Parent Account Plan", AccountPlanType.REVENUE, false);
            
            _mockAccountPlanRepository.Setup(repo => repo.DeleteAsync(accountPlan)).Returns(Task.CompletedTask);
            
            // Act
            await _service.DeleteAccountPlanAsync(accountPlan);
            
            // Assert
            _mockAccountPlanRepository.Verify(repo => repo.DeleteAsync(accountPlan), Times.Once);
        }
    }
}
