using JaredStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JaredStore.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}

//The HomeController manages what happens when a user visits the website's homepage.
//The Index() method uses return View() to tell ASP.NET MVC to display the homepage, typically located at Views/Home/Index.cshtml.
//The Error() method handles errors by displaying an error page with a request ID to help identify and troubleshoot problems.