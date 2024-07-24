using Microsoft.AspNetCore.Mvc;

namespace fix.Controllers;

    public class ComputerController : Controller{

        private readonly Computer _computer;

        private readonly Game game;
        private readonly ILogger<ComputerController> _logger;

        public ComputerController(Game game, Computer computer, ILogger<ComputerController> logger){
            this.game = game;
            _computer = computer;
            _logger = logger;
        }

    }