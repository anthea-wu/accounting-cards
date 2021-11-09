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
            var account = CreateNewAccount("unit-test-test");
            var result = ApiPostSession(account);
            ResultEqualsToOkObjectResult(result);
        }

        [Test]
        public void Return_Bad_Request_When_Account_Is_Null()
        {
            var account = new UserCheckRequestBindingModel();
            var result = ApiPostSession(account);
            ResultEqualsToBadRequestObjectResult(result);
        }

        [Test]
        public void Return_Bad_Request_When_Account_Is_Empty()
        {
            var account = CreateNewAccount("");
            var result = ApiPostSession(account);
            ResultEqualsToBadRequestObjectResult(result);
        }

        private static UserCheckRequestBindingModel CreateNewAccount(string account)
        {
            var result = new UserCheckRequestBindingModel()
            {
                Account = account
            };
            return result;
        }

        private IActionResult ApiPostSession(UserCheckRequestBindingModel account)
        {
            return _userController.Check(account);
        }

        private static void ResultEqualsToOkObjectResult(IActionResult result)
        {
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private static void ResultEqualsToBadRequestObjectResult(IActionResult result)
        {
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}