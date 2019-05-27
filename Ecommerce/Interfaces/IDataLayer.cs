using Ecommerce.Models.DB;
using System.Collections.Generic;

namespace Ecommerce.Interfaces
{
    public interface IDataLayer
    {
        User GetUserByID(int id);
        User GetUserByEmail(string mail);
        User GetUserByEmailAndPassword(string mail, string password);
        
        int RegisterUser(User user);

        List<Categoria> GetCategories();

        int InsertCorso(Corso corso);

        Corso GetCorsoByID(int id);
        List<Corso> GetMiglioriCorsi();

        List<StatCorso> GetProdottiPiuVenduti(int limit, int page, out int tot);
        List<StatUtenti> GetUtentiPiuAttivi(int limit, int page, out int tot);

        List<ItemCarrello> GetCarrello(int uid);
    }
}
