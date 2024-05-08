using System.Collections.Generic;
public class Bishop : Piece
{
    public Bishop(PieceColor color, (int Row, int Column) position)
        : base(PieceType.Bishop, color == PieceColor.White ? "white_bishop.png" : "black_bishop.png", color, position)
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition, Piece?[][] board)
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
            if (board[checkRow][checkCol] != null)
            {
                return false;
            }
        }

        return true;
    }
    public override bool CanAttack((int Row, int Column) position, Piece?[][] board)
    {
        if (!CanMoveTo(position, board)) return false;
        return true;
    }
}
