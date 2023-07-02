public class Queen : Piece
{
    public Queen(string id, PieceColor color, (int Row, int Column) position)
            : base(id, color == PieceColor.White ? "white_queen.png" : "black_queen.png", color, position)
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessBoard)
    {
        var (newRow, newCol) = newPosition;
        var (currentRow, currentCol) = this.Position;
        bool isDiagonal;
        // Check if the move is diagonal
        if (Math.Abs(newRow - currentRow) != Math.Abs(newCol - currentCol))
        {
            isDiagonal = false;
        }
        else { isDiagonal = true; }
        // Check if the move is either horizontal or vertical
        bool isHorizontalMove = newRow == currentRow;
        bool isVerticalMove = newCol == currentCol;

        if (!isHorizontalMove && !isVerticalMove && !isDiagonal)
        {
            // Rook can only move horizontally or vertically
            return false;
        }
        // Determine the direction of the movement
        int rowDirection = (newRow > currentRow) ? 1 : (newRow < currentRow) ? -1 : 0;
        int colDirection = (newCol > currentCol) ? 1 : (newCol < currentCol) ? -1 : 0;

        // Check for pieces blocking the path
        int pathLength = Math.Max(Math.Abs(newRow - currentRow), Math.Abs(newCol - currentCol));
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