using Mafiator.Invitement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Mafiator.Invitement.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]/{gameId}")]
        public IActionResult JoinGame(string gameId)
        {
            return View(model:gameId);
        }

        [Route("[action]/{roomId}")]
        public IActionResult JoinRoom(string roomId)
        {
            return View(model:roomId);
        }

        [Route("[action]/{gameId}")]
        public IActionResult Game(string gameId)
        {
            return View();
        }

        [Route("[action]/{roomId}")]
        public IActionResult Room(string roomId)
        {
            return View();
        }

        [Route("[action]")]

        public IActionResult Play()
        {
            return View();
        }
        [Route("[action]")]

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("[action]")]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
