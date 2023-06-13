using Microsoft.AspNetCore.Mvc;

namespace chess_app.Controllers
{
    public class ChessController : Controller
    {
        private Chessboard chessboard; // Declare chessboard as a field

        public ChessController()
        {
            // Create an instance of the Chessboard model
            chessboard = new Chessboard();

            // Add pieces to the chessboard
            chessboard.AddInitialPieces();
        }

        public IActionResult Chessboard()
        {
            // Pass the board state to the view
            Piece?[,] boardState = chessboard.GetBoardState();
            return View("Chessboard", boardState);
        }

        [HttpPost]
        public IActionResult MovePiece(int sourceRow, int sourceCol, int targetRow, int targetCol)
        {
            // Create tuples for source and target positions
            var sourcePosition = (sourceRow, sourceCol);
            var targetPosition = (targetRow, targetCol);

            // Get the piece at the source position
            Piece? sourcePiece = chessboard.GetPieceAtPosition(sourcePosition);

            // Check if the source position is valid and contains a piece
            if (sourcePiece == null)
            {
                // Invalid move, source position does not contain a piece
                return Json(new { success = false, responseText = "Invalid move. Source position does not contain a piece." });
            }

            // Check if the move is valid for the selected piece
            if (!sourcePiece.CanMoveTo(targetPosition))
            {
                // Invalid move, the selected piece cannot move to the target position
                return Json(new { success = false, responseText = "Invalid move. The selected piece cannot move to the target position." });
            }

            // Get the piece at the target position
            Piece? targetPiece = chessboard.GetPieceAtPosition(targetPosition);

            // Check if the target position is occupied by a piece of the same color
            if (targetPiece != null && targetPiece.Color == sourcePiece.Color)
            {
                // Invalid move, the target position is occupied by a piece of the same color
                return Json(new { success = false, responseText = "Invalid move. The target position is occupied by a piece of the same color." });
            }

            // Move the piece on the chessboard
            chessboard.MovePiece(sourcePiece, targetPosition);

            // Get the updated chessboard state
            Piece?[,] updatedBoardState = chessboard.GetBoardState();

            // Pass the updated board state to the view
            return Json(new { success = true, boardState = updatedBoardState });
        }
    }
}