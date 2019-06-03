using Ecommerce.Models;
using Ecommerce.Models.DB;
using System.Collections.Generic;

namespace Ecommerce.Interfaces
{
    public interface IDataLayer
    {
        User GetUserByID(int id);
        User GetUserByEmail(string mail);
        User GetUserByEmailAndPassword(string mail, string password);

        List<User> GetAdmins();
        int AddAdmin(int uid);
        int RevokeAdmin(int uid);

        int RegisterUser(User user);

        List<Categoria> GetCategories();

        int InsertCorso(Corso corso);
        Corso GetCorsoByID(int id);
        List<Corso> GetMiglioriCorsi();

        List<StatCorso> GetProdottiPiuVenduti(int limit, int page, out int tot);
        List<StatUtenti> GetUtentiPiuAttivi(int limit, int page, out int tot);
        
        int AggiungiCarrello(int idcorso, int idutente);

        List<ItemCarrello> GetCarrello(int uid);
        int AggiornaQuantitaCarrello(int idcarrello, int q);

        int CreaOrdine(int uid);
        List<Models.DB.Ordine> GetOrdini(int uid);
        Models.DB.Ordine GetOrdineByID(int id);
        int UpdateStatoOrdine(int id, string stato);

        CartaCredito GetCartaCredito(int userid);
        int InsertCartaCredito(CartaCredito cc, int userid);

        List<Corso> RicercaConFiltri(int idcategoria, decimal prezzomin, decimal prezzomax, string testo);
        List<Corso> Ricerca(string testo);
        
        List<Recensione> GetRecensioni(int idcorso, int idutente);
        int InsertRecensione(int v, int idc, string recensione, int valutazione);
        Recensione GetRecensioneUtente(int idcorso, int idutente);
        int UpdateRecensione(string id, string recensione, int valutazione);
    }
}
