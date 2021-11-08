using accounting_cards.Controllers;
using accounting_cards.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace accounting_cards_tests
{
    [TestFixture]
    public class UserControllerTest
    {
        static AccountingContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AccountingContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            return new AccountingContext(options);
        }
        private static readonly AccountingContext _context = GetDbContext();
        private static readonly UserTestService _userService = new UserTestService();

        private readonly UserController _userController = new(_context, _userService, new ResultService());

        [Test]
        public void Return_Step_One_When_Account_Is_New()
        {
            // Arrange
            var account = new UserCheckRequestBindingModel();
            
            // Act
            var okResult = _userController.Check(account) as OkObjectResult;
            var result = okResult.Value as UserCheckResponseBindingModel;

            // Assert
            Assert.AreEqual(0, result.Step);
        }
        
    }

    class UserTestService : IDbService
    {
        public User? GetExistOrCreateNew(UserCheckRequestBindingModel account, string salt)
        {
            return new User()
            {
                account = account.Account,
                temp_key = salt
            };
        }
    } 
}