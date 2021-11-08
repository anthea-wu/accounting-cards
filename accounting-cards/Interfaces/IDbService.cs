using accounting_cards.Model;

namespace accounting_cards.Controllers
{
    public interface IDbService
    {
        User? GetExistOrCreateNew(UserCheckRequestBindingModel account, string salt);
    }
}