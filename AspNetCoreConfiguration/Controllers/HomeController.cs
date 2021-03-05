using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreIdentity.Extension;

namespace AspNetCoreIdentity.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            throw new Exception("Erro!!!");
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Secret()
        {
            return View();
        }

        //Claim
        [Authorize(Policy = "AuthorizedDelete")]
        public IActionResult SecretClaim()
        {
            return View();
        }

        //Claim
        [Authorize(Policy = "Write")]
        public IActionResult SecretClaimWrite()
        {
            return View();
        }

        //Curdston Claims Authentication
        [ClaimsAuthorizeAttribute("Product", "Read")]
        public IActionResult SecretClaimCustomWrite()
        {
            return View();
        }


        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelError = new ErrorViewModel();

            if(id == 500)
            {
                modelError.Message = "Error! Try again";
                modelError.Title = "Error!";
                modelError.ErroCode = id;
            }
            else if (id == 404)
            {
                modelError.Message = "Error! The page not exist.";
                modelError.Title = "Page not found!";
                modelError.ErroCode = id;
            }
            else if (id == 403)
            {
                modelError.Message = "Error! Access denied. You don't have permission.";
                modelError.Title = "Access denied";
                modelError.ErroCode = id;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelError);
        }
    }
}
