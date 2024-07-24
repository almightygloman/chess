public class Bishop : Piece
{
    public Bishop(PieceColor color, (int Row, int Column) position)
        : base(PieceType.Bishop, color == PieceColor.White ? "white_bishop.png" : "black_bishop.png", color, position, color == PieceColor.White ? 'B' : 'b')
    {
    }

    public override bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessboard)
    {
        Piece?[][] board = chessboard.GetBoardState();

        var (currentRow, currentCol) = this.Position;

        if (newPosition == Position) return false;

        // Check if the move is diagonal
        if (Math.Abs(newPosition.Row - currentRow) != Math.Abs(newPosition.Column - currentCol))
        {
            return false;
        }

        // Determine the direction of the movement
        int rowDirection = (newPosition.Row > currentRow) ? 1 : -1;
        int colDirection = (newPosition.Column > currentCol) ? 1 : -1;

        // Check for pieces blocking the path
        int pathLength = Math.Abs(newPosition.Row - currentRow);
        for (int i = 1; i < pathLength; i++)
        {
            int checkRow = currentRow + i * rowDirection;
            int checkCol = currentCol + i * colDirection;

            // If there is a piece in the path, the move is not valid
            if (board[checkRow][checkCol] != null)
            {
                return false;
            }
        }
        Piece? destinationPiece = board[newPosition.Row][newPosition.Column];
        if (destinationPiece != null && destinationPiece.Color == Color)
        {
            // There is a piece of the same color in the destination square
            return false;
        }
        return true;
    }
    public override bool CanAttack((int Row, int Column) position, Chessboard chessboard)
    {
        if (!CanMoveTo(position, chessboard)) return false;
        return true;
    }
}
