public class King : Piece
{
    public King(PieceColor color, (int Row, int Column) position)
            : base(PieceType.King, color == PieceColor.White ? "white_king.png" : "black_king.png", color, position)
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessboard)
    {
        Piece?[][] board = chessboard.GetBoardState();
        if (newPosition.Row < 0 || newPosition.Row >= 8 || newPosition.Column < 0 || newPosition.Column >= 8)
        {
            return false;
        }

        if (Math.Abs(newPosition.Row - Position.Row) > 1 || Math.Abs(newPosition.Column - Position.Column) > 1)
        {
            return false;
        }

        if(newPosition == Position) return false;

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
                if (piece.CanAttack(newPosition, chessboard))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public override bool CanAttack((int Row, int Column) position, Chessboard chessboard)
    {
        // Check if the King can move to the position
        if (!CanMoveTo(position, chessboard)) return false;
        return true;
    }


}