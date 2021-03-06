using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace accounting_cards.Model
{
    public class AccountingContext : DbContext
    {
        public AccountingContext(DbContextOptions<AccountingContext> options)
            : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }

        // 使用 new 的簡單 DbContext 初始化時需要做的設定
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     // optionsBuilder.UseSqlServer(
        //     //     @"Data Source=EMCT-KING\SQLEXPRESS; Initial Catalog=accounting_card; User Id=sa; Password=emct;");
        //     // optionsBuilder.UseSqlServer(
        //     //     @"Persist Security Info=False;Integrated Security=true;  
        //     // Initial Catalog=accounting_card;Server=LAPTOP-EIP37RBS");
        // }
    }

    public class User
    {
        [Key]
        public Guid guid { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string temp_key { get; set; }
        public DateTimeOffset create_time { get; set; }
    }
}