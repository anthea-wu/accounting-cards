using System;
using System.Linq;
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
        private static readonly AccountingContext _db = GetDbContext();
        private readonly UserController _userController = new(_db, new UserTestService(), new ResultService());

        [Test]
        public void Return_Step_Zero_When_Account_Is_New()
        {
            // Arrange
            var account = new UserCheckRequestBindingModel()
            {
                Account = "unit-test-test"
            };
            
            // Act
            var okResult = _userController.Check(account) as OkObjectResult;
            var result = okResult.Value as UserCheckResponseBindingModel;

            // Assert
            Assert.AreEqual(0, result.Step);
        }

        [Test]
        public void Return_Step_One_When_Account_Is_Exist()
        {
            // Arrange
            var account = new UserCheckRequestBindingModel()
            {
                Account = "unit-test-test",
                Password = "123"
            };
            
            // Act
            var okResult = _userController.Check(account) as OkObjectResult;
            var result = okResult.Value as UserCheckResponseBindingModel;
            
            // Assert
            Assert.AreEqual(1, result.Step);
        }
        
    }

    class UserTestService : IDbService
    {
        public User? GetExistOrCreateNew(UserCheckRequestBindingModel account, string salt)
        {
            return new User()
            {
                account = account.Account,
                password = account.Password,
                temp_key = salt
            };
        }
    } 
    
    
}