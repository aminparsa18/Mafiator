using Mafiator.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;

namespace Mafiator.Web.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        [Route("")]
        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("[action]")]

        public IActionResult GoogleLogin()
        {
            return Challenge(new AuthenticationProperties()
            {
                RedirectUri = "/home/GoogleRedirect"
            },"Google");
        }
        [Route("[action]")]

        public IActionResult FacebookLogin()
        {
            return Challenge(new AuthenticationProperties()
            {
                
                RedirectUri = "/home/FaceBookRedirect"
            }, "Facebook");
        }
        [Route("[action]")]

        public async Task<IActionResult> GoogleRedirect()
        {
            var auth = await Request.HttpContext.AuthenticateAsync("Google");
            string success;
            if (!auth.Succeeded
                || auth?.Principal == null
                || !auth.Principal.Identities.Any(id => id.IsAuthenticated)
                || string.IsNullOrEmpty(auth.Properties.GetTokenValue("access_token")))
            {
                success = "0";
            }
            else
            {
                success = "1";
            }

            return Redirect("app://callback.mftor?success=" + success);
        }
        [Route("[action]")]

        public async Task<IActionResult> FaceBookRedirect()
        {
            var auth = await Request.HttpContext.AuthenticateAsync("Facebook");
            string success;
            if (!auth.Succeeded
                || auth?.Principal == null
                || !auth.Principal.Identities.Any(id => id.IsAuthenticated)
                || string.IsNullOrEmpty(auth.Properties.GetTokenValue("access_token")))
            {
                success = "0";
            }
            else
            {
                success = "1";
            }

            return Redirect("app://callback.mftor?success=" + success);
        }
        [Route("[action]")]

        public IActionResult Privacy()
        {
            return View();
        }


        [Route("[action]")]
        public FileResult Help(string lang)
        {
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, "game-en.pdf");
            var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
            return File(fileStream, "application/pdf");
        }


        [Route("[action]")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
