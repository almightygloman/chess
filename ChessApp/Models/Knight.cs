using System.Collections.Generic;
public class Knight : Piece
{
    public Knight(PieceColor color, (int Row, int Column) position)
            : base(PieceType.Knight, color == PieceColor.White ? "white_knight.png" : "black_knight.png", color, position)
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition, Piece?[][] board)
    {
        // Check if the position is within the bounds of the chessboard
        if (newPosition.Row < 0 || newPosition.Row >= 8 || newPosition.Column < 0 || newPosition.Column >= 8)
        {
            return false;
        }

        int rowDif = Math.Abs(newPosition.Row - Position.Row);
        int colDif = Math.Abs(newPosition.Column - Position.Column);

        // Check if the move is an L-shape
        if (colDif == 1 && rowDif == 2 || colDif == 2 && rowDif == 1)
        {
            // Check if the new position is occupied by a piece of the same color
            Piece? pieceAtNewPosition = board[newPosition.Row][newPosition.Column];
            if (pieceAtNewPosition == null || pieceAtNewPosition.Color != this.Color)
            {
                return true;
            }
        }

        return false;
    }
    public override bool CanAttack((int Row, int Column) position, Piece?[][] board)
    {
        if (!CanMoveTo(position, board)) return false;
        return true;
    }
}