using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pasquinelli.Martina._5H.SecondaWeb.DTO;
using Pasquinelli.Martina._5H.SecondaWeb.Models;

namespace Pasquinelli.Martina._5H.SecondaWeb.Controllers
{
  public class AccountController : Controller
  {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
    }

    public IActionResult LogIn()
    {
      return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO user)
    {
      if (ModelState.IsValid)
      {
        var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

        if (result.Succeeded)
        {
          // Se l'utente fa login correttamente, entra.
          return RedirectToAction("Lista", "Account");
        }
        else
        {

          // ...altrimenti meglio non dare troppo info a chi ci prova
          // meglio un generico errore,
          ModelState.AddModelError(string.Empty, "Login error");
        }
      }
      return View(user);
    }


    public IActionResult SignIn()
    {
      return View();
    }
      [HttpGet]
      public IActionResult Lista()
      {
        if( User.Identity.IsAuthenticated )
        {
          var db=new DBContext();
          return View("~/Views/Home/Lista.cshtml",db);
        }
            return RedirectToAction("LogIn", "Account");        
    }
    [HttpGet]
        public IActionResult Prenota()
        {
            if( User.Identity.IsAuthenticated )
            {
                return View("~/Views/Home/Prenota.cshtml");
            }
            return RedirectToAction("LogIn", "Account");
            
        }
    [HttpGet]
        public IActionResult Modifica()
        {
            if( User.Identity.IsAuthenticated )
            {
               return RedirectToAction("~/Views/Home/Modifica.cshtml");
            }
            return RedirectToAction("LogIn", "Account");
            
        }

    [HttpPost]
    public async Task<IActionResult> SignIn(SigInDTO model)
    {
      if (ModelState.IsValid)
      {
        var user = new IdentityUser
        {
          UserName = model.Email,
          Email = model.Email,
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
          var db= new DBContext();
          //await _signInManager.SignInAsync(user, isPersistent: false);
          return RedirectToAction("Lista", "Account");
        }
        foreach (var error in result.Errors)
          ModelState.AddModelError("", error.Description);

        ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
      }
      return View(model);
    }
    public async Task<IActionResult> Logout()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index", "Home");
    }

  }
}
