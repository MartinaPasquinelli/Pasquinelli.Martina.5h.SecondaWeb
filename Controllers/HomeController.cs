using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pasquinelli.Martina._5H.SecondaWeb.Models;

namespace Pasquinelli.Martina._5H.SecondaWeb.Controllers
{
  public class HomeController : Controller
  {
  private readonly ILogger<HomeController> _logger;

      public HomeController(ILogger<HomeController> logger)
      {
          _logger = logger;
      }
      public IActionResult Index()
      {
          return View();
      }
      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
          return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }
      [Authorize]
      public IActionResult Lista()
      {
          var db= new DBContext();
          return View(db);
      }
      [Authorize]
      [HttpGet]
      public IActionResult Prenota()
      {
          return View();
      }
      [Authorize]
      [HttpPost]
       public IActionResult Prenota(Prenotazione p)
      {
        var db = new DBContext();
            db.Prenotazioni.Add(p);
            db.SaveChanges();
          return View("Lista",db);
      }
      [Authorize]
      public IActionResult Cancella(int Id)
      {
          var db = new DBContext();
          Prenotazione prenotazione=db.Prenotazioni.Find(Id);
              db.Prenotazioni.Remove(prenotazione);
              db.SaveChanges();
          return View("Lista",db);
      }
      [Authorize]
      public IActionResult Modifica(int Id)
      {
          var db = new DBContext();
          Prenotazione prenotazione=db.Prenotazioni.Find(Id);
          return View("Modifica",prenotazione);
      
      }
      [Authorize]
      [HttpPost]
      //public IActionResult Modifica(int id, [Bind("Nome,Email")] Prenotazione nuovo)
      //{
      public IActionResult Modifica(int id,Prenotazione nuovo)
      {
          var db = new DBContext();
          var vecchio=db.Prenotazioni.Find(id);
          if(vecchio!=null){
                  vecchio.Nome=nuovo.Nome;
                  vecchio.Email=nuovo.Email;
                  vecchio.VideoGioco=nuovo.VideoGioco;
              db.Prenotazioni.Update(vecchio);
              db.SaveChanges();
              return View("Lista",db);
          }
          return NotFound();
      }
    [Authorize]
    public IActionResult Prova()
    {
      if (User.Identity.IsAuthenticated)
        return View();

      return RedirectToAction("Login", "Account");
    }

    public IActionResult Upload(){
            return View("Upload");
        }

        [HttpPost]
        public IActionResult Upload(CreatePost c){
            MemoryStream stream =new MemoryStream();
            c.MyCsv.CopyTo(stream);
            stream.Seek(0,0);
            StreamReader fn=new StreamReader(stream);

            if(!fn.EndOfStream)
            {
                DBContext db =new DBContext();
                string riga=fn.ReadLine();
                while(!fn.EndOfStream){

                    riga=fn.ReadLine();
                    string[] colonne=riga.Split(';');
                   
                    Prenotazione p =new Prenotazione{Nome=colonne[0],Email=colonne[1],VideoGioco=colonne[2]};
                    db.Prenotazioni.Add(p);
                }
                db.SaveChanges();
                return View("Lista",db);
            }
            return View();
        }
  }
}
