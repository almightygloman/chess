using System.Collections.Generic;
public class King : Piece
{
    public King(PieceColor color, (int Row, int Column) position)
            : base(PieceType.King, color == PieceColor.White ? "white_king.png" : "black_king.png", color, position)
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition, Piece?[][] board)
    {
        if (newPosition.Row < 0 || newPosition.Row >= 8 || newPosition.Column < 0 || newPosition.Column >= 8)
        {
            return false;
        }

        if (Math.Abs(newPosition.Row - Position.Row) > 1 || Math.Abs(newPosition.Column - Position.Column) > 1)
        {
            return false;
        }

        Piece? pieceAtNewPosition = board[newPosition.Row][newPosition.Column];
        if (pieceAtNewPosition != null && pieceAtNewPosition.Color == this.Color)
        {
            return false;
        }

        // If the new position is occupied by an enemy piece, return true immediately
        if (pieceAtNewPosition != null && pieceAtNewPosition.Color != this.Color)
        {
            return true;
        }

        // Check if the new position is under attack by any enemy piece
        foreach (var row in board)
        {
            foreach (var piece in row)
            {
                // If the piece is null or is the same color, skip
                if (piece == null || piece.Color == this.Color)
                {
                    continue;
                }
                if (piece is King)
                {
                    if (Math.Abs(piece.Position.Row - newPosition.Row) <= 1 && Math.Abs(piece.Position.Column - newPosition.Column) <= 1)
                    {
                        return false;
                    }
                    continue;
                }

                // If the piece can move to the new position, return false
                if (piece.CanAttack(newPosition, board))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public override bool CanAttack((int Row, int Column) position, Piece?[][] board)
    {
        // Check if the King can move to the position
        if (!CanMoveTo(position, board)) return false;

        // Check if there is an enemy piece at the position
        Piece? pieceAtPosition = board[position.Row][position.Column];
        if (pieceAtPosition == null || pieceAtPosition.Color == this.Color) return false;

        // Check if the enemy piece is protected
        PieceColor enemyColor = this.Color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece? piece = board[row][col];
                if (piece != null && piece.Color == enemyColor)
                {
                    // Use CanMoveTo instead of CanAttack to avoid infinite recursion
                    if (piece.CanMoveTo(position, board)) return false;
                }
            }
        }

        // The enemy piece is not protected, so the King can capture it
        return true;
    }


}