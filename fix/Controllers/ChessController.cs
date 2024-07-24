using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace fix.Controllers
{
    public class ChessController : Controller
    {
        private readonly ILogger<ChessController> _logger;
        public ChessController(ILogger<ChessController> logger)
        {
            _logger = logger;
        }

        public IActionResult Chessboard([FromServices] Game game)
        {

            // Pass the board state to the view
            return View("Chessboard", game);

        }

        [HttpGet]
        public IActionResult InitialBoardState([FromServices] Game game)
        {
            try
            {
                int legalMoves = game.LegalMoves(game.Chessboard.WhiteToMove ? PieceColor.White : PieceColor.Black).Count;
                Piece?[][] boardState = game.Chessboard.GetBoardState();
                string currentTurn = game.Chessboard.WhiteToMove ? "white" : "black";
                return Json(new { boardState, currentTurn, legalMoves });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the initial board state.");
                return Json(new { success = false, responseText = "An error occurred while fetching the initial board state." });
            }
        }



        [HttpPost]
        public IActionResult MovePiece(int sourceRow, int sourceColumn, int targetRow, int targetColumn, [FromServices] Game game)
        {
            try
            {
                Move move = new((sourceRow, sourceColumn), (targetRow, targetColumn));
                // Delegate the move operation to the Game class
                bool moveSuccess = game.MovePiece(move);

                if (moveSuccess)
                {
                    // Get the updated chessboard state
                    int halfMoves = game.Chessboard.halfMoves;
                    int legalMoves = game.LegalMoves(game.Chessboard.WhiteToMove ? PieceColor.White : PieceColor.Black).Count;
                    Piece?[][] updatedBoardState = game.Chessboard.GetBoardState();
                    string currentTurn = game.Chessboard.WhiteToMove ? "white" : "black";
                    return Json(new { success = true, boardState = updatedBoardState, currentTurn, legalMoves, halfMoves });
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

        [HttpGet]
        public IActionResult GetLegalMoves(int row, int col, [FromServices] Game game)
        {
            try
            {
                Piece? piece = game.Chessboard.GetFromPosition((row, col));
                if (piece == null)
                {
                    return Json(new { success = false, responseText = "No piece at the selected position." });
                }
                (int row, int col)[] LegalMoves = game.LegalMoves(piece);

                return Json(new { success = true, LegalMoves });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the legal moves.");
                return Json(new { success = false, responseText = "An error occurred while fetching the legal moves." });
            }
        }

        [HttpPost]
        public IActionResult MakeComputerMove([FromServices] Game game)
        {
            try
            {
                // Instantiate the computer player
                Computer computerPlayer = new Computer();

                // Generate a move for the computer
                Move computerMove = computerPlayer.GenerateMove(game);

                // Make the move on the board
                bool moveSuccess = game.MovePiece(computerMove);

                if (moveSuccess)
                {
                    // Get the updated chessboard state
                    int halfMoves = game.Chessboard.halfMoves;
                    int legalMoves = game.LegalMoves(game.Chessboard.WhiteToMove ? PieceColor.White : PieceColor.Black).Count;
                    Piece?[][] updatedBoardState = game.Chessboard.GetBoardState();
                    string currentTurn = game.Chessboard.WhiteToMove ? "white" : "black";
                    return Json(new { success = true, boardState = updatedBoardState, currentTurn, legalMoves});
                }
                else
                {
                    return Json(new { success = false, responseText = "Invalid move. Please try again." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while making the computer move.");
                return Json(new { success = false, responseText = "An error occurred while making the computer move. Please try again." });
            }
        }

    }
}