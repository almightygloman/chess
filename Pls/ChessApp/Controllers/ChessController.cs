using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace ChessApp.Controllers
{
    public class ChessController : Controller
    {
        private Game game;
        private readonly ILogger<ChessController> _logger;

        public ChessController(Game game, ILogger<ChessController> logger)
        {
            this.game = game;
            _logger = logger;
        }

        public IActionResult Chessboard()
        {
            Piece?[][] boardState = game.Chessboard.GetBoardState();

            // Pass the board state to the view
            return View("Chessboard", boardState);

        }

        [HttpGet]
        public IActionResult InitialBoardState()
        {
            try
            {
                Piece?[][] boardState = game.Chessboard.GetBoardState();
                return Json(new { boardState = boardState });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the initial board state.");
                return Json(new { success = false, responseText = "An error occurred while fetching the initial board state." });
            }
        }



        [HttpPost]
        public IActionResult MovePiece(int sourceRow, int sourceColumn, int targetRow, int targetColumn)
        {
            try
            {
                // Delegate the move operation to the Game class
                bool moveSuccess = game.MovePiece(sourceRow, sourceColumn, targetRow, targetColumn, game.Chessboard.GetBoardState());

                if (moveSuccess)
                {
                    // Get the updated chessboard state
                    Piece?[][] updatedBoardState = game.Chessboard.GetBoardState();
                    string currentTurn = game.isWhiteTurn ? "white" : "black";
                    return Json(new { success = true, boardState = updatedBoardState, currentTurn = currentTurn });
                }
                else
                {
                    return Json(new { success = false, responseText = "Invalid move. Please try again." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while moving a piece.");
                return Json(new { success = false, responseText = "An error occurred while moving the piece. Please try again." });
            }
        }

    }
}