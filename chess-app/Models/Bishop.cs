public class Bishop : Piece
{
    public Bishop(string id, PieceColor color, (int Row, int Column) position)
        : base(id, color == PieceColor.White ? "white_bishop.png" : "black_bishop.png", color, position)
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessBoard)
    {
        var (newRow, newCol) = newPosition;
        var (currentRow, currentCol) = this.Position;

        // Check if the move is diagonal
        if (Math.Abs(newRow - currentRow) != Math.Abs(newCol - currentCol))
        {
            return false;
        }

        // Determine the direction of the movement
        int rowDirection = (newRow > currentRow) ? 1 : -1;
        int colDirection = (newCol > currentCol) ? 1 : -1;

        // Check for pieces blocking the path
        int pathLength = Math.Abs(newRow - currentRow);
        for (int i = 1; i < pathLength; i++)
        {
            int checkRow = currentRow + i * rowDirection;
            int checkCol = currentCol + i * colDirection;

            // If there is a piece in the path, the move is not valid
            if (chessBoard.GetPieceAtPosition(checkRow, checkCol) != null)
            {
                return false;
            }
        }

        return true;
    }

    public override List<(int Row, int Column)> CalculateLegalMoves(Chessboard chessBoard)
    {
        throw new NotImplementedException();
    }
}
