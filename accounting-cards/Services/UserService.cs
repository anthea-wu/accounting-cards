using System;
using System.Linq;
using accounting_cards.Model;

namespace accounting_cards.Controllers
{
    public class UserService : IDbService
    {
        private readonly AccountingContext _db;

        public UserService(AccountingContext db)
        {
            _db = db;
        }

        public User? GetExistOrCreateNew(UserCheckRequestBindingModel account, string salt)
        {
            var user = _db.Users.FirstOrDefault(u => u.account == account.Account);
            if (user == null)
            {
                user = new User()
                {
                    guid = Guid.NewGuid(),
                    account = account.Account,
                };
                _db.Users.Add(user);
            }

            user.temp_key = salt;
            _db.SaveChanges();
            
            return user;
        }
    }
}