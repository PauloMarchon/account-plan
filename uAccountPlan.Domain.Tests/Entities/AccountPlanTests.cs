using uAccountPlan.Domain.Entities;

namespace uAccountPlan.Domain.Tests.Entities
{
    public class AccountPlanTests
    {

        public static IEnumerable<object[]> InvalidParentAccountPlanData()
        {
            yield return new object[] { "", "Parent Account Plan", AccountPlanType.EXPENSE, false, "Code cannot be null or empty." };
            yield return new object[] { "2.0", "", AccountPlanType.EXPENSE, false, "Name cannot be null or empty." };
            yield return new object[] { "2.0", " ", AccountPlanType.EXPENSE, false, "Name cannot be null or empty." };
            yield return new object[] { "2.0", "Parent Account Plan", (AccountPlanType)999, false, "Type must be either REVENUE or EXPENSE." };         
        }

        public static IEnumerable<object[]> InvalidChildAccountPlanData()
        {
            yield return new object[] { "2.1", "Child Account Plan", AccountPlanType.EXPENSE, true, Guid.Empty, "ParentId cannot be empty if provided." };
        }

        [Fact]
        public void CreateParentAccountPlan_ShouldCreateParentAccountPlanSuccessfully_WhenValidParametersAreProvided()
        {
            // Arrange
            string code = "2.0";
            string name = "Parent Account Plan";
            var type = AccountPlanType.REVENUE;
            bool acceptsLaunches = false;

            // Act
            var accountPlan = AccountPlan.Create(code, name, type, acceptsLaunches);

            // Assert
            Assert.NotNull(accountPlan);
            Assert.NotEqual(Guid.Empty, accountPlan.Id);
            Assert.Equal(code, accountPlan.Code);
            Assert.Equal(name, accountPlan.Name);
            Assert.Equal(type, accountPlan.Type);
            Assert.Equal(acceptsLaunches, accountPlan.AcceptsLaunches);
            Assert.Null(accountPlan.ParentId);
        }

        [Fact]
        public void CreateChildAccountPlan_ShouldCreateChildAccountPlanSuccessfully_WhenValidParametersAreProvided()
        {
            // Arrange
            string code = "2.1";
            string name = "Child Account Plan";
            var type = AccountPlanType.REVENUE;
            bool acceptsLaunches = true;
            Guid parentId = Guid.NewGuid();

            // Act
            var accountPlan = AccountPlan.Create(code, name, type, acceptsLaunches, parentId);

            // Assert
            Assert.NotNull(accountPlan);
            Assert.NotEqual(Guid.Empty, accountPlan.Id);
            Assert.Equal(code, accountPlan.Code);
            Assert.Equal(name, accountPlan.Name);
            Assert.Equal(type, accountPlan.Type);
            Assert.Equal(acceptsLaunches, accountPlan.AcceptsLaunches);
            Assert.Equal(parentId, accountPlan.ParentId);
        }

        [Theory]
        [MemberData(nameof(InvalidParentAccountPlanData))]
        public void CreateParentAccountPlan_ShouldThrowArgumentException_WhenInvalidParametersAreProvided(string code, string name, AccountPlanType type, bool acceptsLaunches, string expectedMessage)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => AccountPlan.Create(code, name, type, acceptsLaunches));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [MemberData(nameof(InvalidChildAccountPlanData))]
        public void CreateChildAccountPlan_ShouldThrowArgumentException_WhenInvalidParametersAreProvided(string code, string name, AccountPlanType type, bool acceptsLaunches, Guid parentId, string expectedMessage)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => AccountPlan.Create(code, name, type, acceptsLaunches, parentId));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}

    
        
    
