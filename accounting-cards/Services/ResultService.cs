using accounting_cards.Model;

namespace accounting_cards.Controllers
{
    public class ResultService : IResultService
    {
        public UserCheckResponseBindingModel Get(UserCheckRequestBindingModel account, string salt, User? user)
        {
            var result = new UserCheckResponseBindingModel
            {
                Account = account.Account,
                Salt = salt
            };

            if (!string.IsNullOrEmpty(user.password))
            {
                result.Step = 1;
            }
            else
            {
                result.Step = 0;
            }

            return result;
        }
    }
}