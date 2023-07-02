using Microsoft.AspNetCore.Mvc;

namespace chess_app.Controllers
{
    public class ChessController : Controller
    {
        private Chessboard chessboard; // Declare chessboard as a field
        private readonly ILogger<ChessController> _logger;

        public ChessController(Chessboard chessboard, ILogger<ChessController> logger)
        {
            this.chessboard = chessboard;
            _logger = logger;
        }

        public IActionResult Chessboard()
        {
            Piece?[][] boardState = chessboard.GetBoardState();

            // Pass the board state to the view
            return View("Chessboard", boardState);

        }

        [HttpGet]
        public IActionResult InitialBoardState()
        {
            try
            {
                Piece?[][] boardState = chessboard.GetBoardState();
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
                _logger.LogInformation("Moving " + chessboard.GetPieceAtPosition(sourceRow, sourceColumn).ID + " from ({SourceRow}, {SourceColumn}) to ({TargetRow}, {TargetColumn})", sourceRow, sourceColumn, targetRow, targetColumn);
                // Check if the source and target positions are within the bounds of the chessboard
                if (sourceRow < 0 || sourceRow >= 8 || sourceColumn < 0 || sourceColumn >= 8 || targetRow < 0 || targetRow >= 8 || targetColumn < 0 || targetColumn >= 8)
                {
                    return Json(new { success = false, responseText = "Invalid move. Source or target position is out of bounds." });
                }

                Piece? sourcePiece = chessboard.GetPieceAtPosition(sourceRow, sourceColumn);

                // Check if the source position is valid and contains a piece
                if (sourcePiece == null)
                {
                    // Invalid move, source position does not contain a piece
                    return Json(new { success = false, responseText = "Invalid move. Source position does not contain a piece." });
                }

                if (sourceRow == targetRow && sourceColumn == targetColumn)
                {
                    var piece = chessboard.GetPieceAtPosition(sourceRow, sourceColumn);
                    if (piece != null)
                    {
                        var legalMoves = piece.CalculateLegalMoves(chessboard);
                        return Json(new { success = true, legalMoves = legalMoves });
                    }
                }

                // Check if the move is valid for the selected piece
                if (!(sourcePiece?.CanMoveTo((targetRow, targetColumn), chessboard) ?? false))
                {
                    // Invalid move, the selected piece cannot move to the target position
                    return Json(new { success = false, responseText = "Invalid move. The selected piece cannot move to the target position." });
                }

                // Get the piece at the target position
                Piece? targetPiece = chessboard.GetPieceAtPosition(targetRow, targetColumn);

                // Check if the target position is occupied by a piece of the same color
                if (targetPiece != null && targetPiece.Color == sourcePiece.Color)
                {
                    // Invalid move, the target position is occupied by a piece of the same color
                    return Json(new { success = false, responseText = "Invalid move. The target position is occupied by a piece of the same color." });
                }

                // Move the piece on the chessboard
                chessboard.MovePiece(sourcePiece, (targetRow, targetColumn));

                // Get the updated chessboard state
                Piece?[][] updatedBoardState = chessboard.GetBoardState();

                // Pass the updated board state to the view
                return Json(new { success = true, boardState = updatedBoardState });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while moving a piece.");
                return Json(new { success = false, responseText = "An error occurred while moving the piece. Please try again." });
            }
        }
        /*
        [HttpGet]
        public IActionResult GetPieceInfo(int row, int col)
        {
            try
            {
                // Query your game state to find out which piece is on the square
                // Assume you have a method GetPieceNameAtPosition that returns the name of the piece
                string pieceName;
                if(chessboard.GetPieceAtPosition(row, col)!= null){
                    pieceName = chessboard.GetPieceAtPosition(row, col).ID;
                }
                
                return Json(new { pieceName = pieceName });
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return Json(new { error = "Error retrieving piece information" });
            }
        }
        */
    }
}