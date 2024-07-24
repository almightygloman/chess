using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
//what the hell is signalR?
namespace Controllers
{
    public class PlayController : Controller
    {
        public Game game;
        public readonly ILogger<PlayController> _logger;
        public PlayController(Game game, ILogger<PlayController> logger)
        {
            _logger = logger;
            this.game = game;
        }

        public IActionResult Play()
        {

            return View();
        }

        //figure out what async ans Tasks are??? 


        //methods with put post get put and delete

        public IActionResult Computer()
        {


            return View("Computer", game);
        }

        // [HttpPost]

        // public async Task<IActionResult> ()



    }
}