using accounting_cards.Controllers;
using accounting_cards.Model;

namespace accounting_cards_tests
{
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
}