using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Parasale.WebUI.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Error500()
        {
            return View();
        }

        public IActionResult Error404()
        {
            return View();
        }


        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return RedirectToAction("Error404");
                case 500:
                    return RedirectToAction("Error500");
            }

            return View();
        }
    }
}