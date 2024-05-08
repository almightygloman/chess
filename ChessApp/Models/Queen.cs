using System.Collections.Generic;
public class Queen : Piece
{
    public Queen(PieceColor color, (int Row, int Column) position)
            : base(PieceType.Queen, color == PieceColor.White ? "white_queen.png" : "black_queen.png", color, position)
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition, Piece?[][] board)
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
            if (board[checkRow][checkCol] != null)
            {
                return false;
            }
        }
        // Check if there is a piece in the destination square
        Piece? destinationPiece = board[newPosition.Row][newPosition.Column];
        if (destinationPiece != null && destinationPiece.Color == Color)
        {
            // There is a piece of the same color in the destination square
            return false;
        }


        return true;
    }

    public override bool CanAttack((int Row, int Column) position, Piece?[][] board)
    {
        if (!CanMoveTo(position, board)) return false;
        return true;
    }



}