using accounting_cards.Model;

namespace accounting_cards.Controllers
{
    public interface IResultService
    {
        UserCheckResponseBindingModel Get(UserCheckRequestBindingModel account, string salt, User? user);
    }
}