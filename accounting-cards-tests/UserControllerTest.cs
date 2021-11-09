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
        private readonly UserController _userController = new(_db, new UserTestService(), new ResultTestService());

        [Test]
        public void Return_Ok_Response_When_Account_Is_Not_Null_Or_Empty()
        {
            // Arrange
            var account = new UserCheckRequestBindingModel()
            {
                Account = "unit-test-test"
            };
            
            // Act
            var result = _userController.Check(account);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void Return_Bad_Request_When_Account_Is_Null()
        {
            // Arrange
            var account = new UserCheckRequestBindingModel();
            
            // Act
            var result = _userController.Check(account);
            
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void Return_Bad_Request_When_Account_Is_Empty()
        {
            // Arrange
            var account = new UserCheckRequestBindingModel()
            {
                Account = ""
            };
            
            // Act
            var result = _userController.Check(account);
            
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        
    }

    class ResultTestService : IResultService
    {
        public UserCheckResponseBindingModel Get(UserCheckRequestBindingModel account, string salt, User? user)
        {
            return new UserCheckResponseBindingModel()
            {
                Account = account.Account,
                Step = 0,
                Salt = ""
            };
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