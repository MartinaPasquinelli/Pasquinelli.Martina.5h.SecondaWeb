using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Pasquinelli.Martina._5H.SecondaWeb.Models
{
    public class CreatePost
    {
        public IFormFile MyCsv { get;set;}
    }
}