using accounting_cards.Controllers;
using accounting_cards.Model;

namespace accounting_cards_tests
{
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