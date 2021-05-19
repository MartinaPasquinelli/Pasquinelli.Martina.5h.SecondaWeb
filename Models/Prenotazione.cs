using System;
using System.ComponentModel.DataAnnotations;

namespace Pasquinelli.Martina._5H.SecondaWeb.Models
{
    public class Prenotazione 
    {
        public string Nome { get; set; }
        public int PrenotazioneId { get; set; }

        [Required(ErrorMessage="Inserisci una Email valida")]
        [EmailAddress]
        public string Email { get; set; }
        public string VideoGioco  {get; set; }
        public DateTime DataPrenotazione { get; set; }=DateTime.Now;

      
  }
}