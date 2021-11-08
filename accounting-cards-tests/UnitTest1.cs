using System;
using System.Linq;
using accounting_cards;
using accounting_cards.Controllers;
using accounting_cards.Model;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace accounting_cards_tests
{
    public class Tests
    {
        static AccountingContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AccountingContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            return new AccountingContext(options);
        }
        private static readonly AccountingContext _context = GetDbContext();
        
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test_Db_In_Memory_Sample()
        {
            var ctx = GetDbContext();
            var data = new User()
            {
                guid = Guid.NewGuid(),
                name = "測試"
            };
            ctx.Users.Add(data);
            ctx.SaveChanges();
            var rec = ctx.Users.FirstOrDefault();
            Assert.IsNotNull(rec);
        }
    }
}