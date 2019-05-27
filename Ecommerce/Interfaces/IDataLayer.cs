﻿using Ecommerce.Models.DB;
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
        List<Corso> RicercaConFiltri(int idcategoria, decimal prezzomin, decimal prezzomax, string testo);
        List<Corso> Ricerca(string testo);
        
    }
}
