public class Game
{
    public Chessboard Chessboard { get; set; }
    public PieceColor TurnColor { get; set; }

    public Game()
    {
        Chessboard = new Chessboard();
        TurnColor = PieceColor.White; // Game always starts with white's turn
        Chessboard.AddInitialPieces();
    }

    public Piece?[][] GetBoardState()
    {
        return Chessboard.GetBoardState();
    }

    public Piece? GetPieceAtPosition(int row, int col){
        return Chessboard.GetPieceAtPosition(row, col);
    }

    public bool MovePiece(int sourceRow, int sourceColumn, int targetRow, int targetColumn)
    {
        Piece? sourcePiece = Chessboard.GetPieceAtPosition(sourceRow, sourceColumn);

        // Check if the source position is valid and contains a piece
        if (sourcePiece == null)
        {
            // Invalid move, source position does not contain a piece
            return false;
        }

        // Check if it's the correct color's turn to move
        if (sourcePiece.Color != TurnColor)
        {
            // Invalid move, it's not the piece's color's turn to move
            return false;
        }

        // Check if the move is valid for the selected piece
        if (!sourcePiece.CanMoveTo((targetRow, targetColumn), Chessboard))
        {
            // Invalid move, the selected piece cannot move to the target position
            return false;
        }

        // Get the piece at the target position
        Piece? targetPiece = Chessboard.GetPieceAtPosition(targetRow, targetColumn);

        // Check if the target position is occupied by a piece of the same color
        if (targetPiece != null && targetPiece.Color == sourcePiece.Color)
        {
            // Invalid move, the target position is occupied by a piece of the same color
            return false;
        }

        // Move the piece on the chessboard
        Chessboard.MovePiece(sourcePiece, (targetRow, targetColumn));

        // Update the turn color
        TurnColor = TurnColor == PieceColor.White ? PieceColor.Black : PieceColor.White;

        return true;
    }
    // ... add any other game-related methods here
}
