using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Models
{
    public class ModCorso
    {
        public ModCorso()
        {
            Categorie = new List<SelectListItem>();
        }

        public bool IsModifica { get; set; }

        public List<SelectListItem> Categorie { get; set; }

        public string Immagine { get; set; }

        [Display(Name = "Foto")]
        public HttpPostedFileBase File { get; set; }

        [Display(Name = "Categoria")]
        public int IDCategoria { get; set; }
        
        [Required]
        [Display(Name = "Titolo")]
        public string Titolo { get; set; }

        [Required]
        [Display(Name = "Autore")]
        public string Autore { get; set; }

        private string _prezzo;

        [Required]
        [Display(Name = "Prezzo")]
        public string Prezzo
        {
            get { return _prezzo?.Replace(',', '.'); }
            set { _prezzo = value; }
        }


        [Required]
        [Display(Name = "Descrizione")]
        public string Descrizione { get; set; }
        public int ID { get; set; }
    }
}