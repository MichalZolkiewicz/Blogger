using Infrastructure.Identity;
using Xunit;

namespace UnitTests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public void IsUserEmailConfirmedShouldReturnTrueWhenInputIsTrue()
        {
            //Arrange
            ApplicationUser user = new ApplicationUser();
            user.UserName = "Michał";
            user.EmailConfirmed = true;

            UserService userService = new UserService();
           
            //Act
            bool isEmailConfirmed = userService.IsUserEmailConfirmed(user);

            //Assert
            Assert.True(isEmailConfirmed);
        }

        [Fact]
        public void IsUserEmailConfirmedShouldReturnFalseWhenInputIsFalse()
        {
            //Arrange
            ApplicationUser user = new ApplicationUser();
            user.UserName = "Michał";
            user.EmailConfirmed = false;

            UserService userService = new UserService();

            //Act
            bool isEmailConfirmed = userService.IsUserEmailConfirmed(user);

            //Assert
            Assert.False(isEmailConfirmed);
        }
    }
}
