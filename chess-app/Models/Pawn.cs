public class Pawn : Piece
{
    public Pawn(PieceColor color, (int Row, int Column) position)
        : base(color == PieceColor.White ? "white_pawn.png" : "black_pawn.png", color, position)
    {
    }

    public override bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessBoard)
    {
        // Check if the position is within the bounds of the chessboard
        if (newPosition.Row < 0 || newPosition.Row >= 8 || newPosition.Column < 0 || newPosition.Column >= 8)
        {
            return false;
        }

        // Calculate the difference between the current and new positions
        int rowDifference = newPosition.Row - Position.Row;
        int columnDifference = Math.Abs(newPosition.Column - Position.Column);

        // Pawns can only move forward
        if (Color == PieceColor.White && rowDifference < 0 || Color == PieceColor.Black && rowDifference > 0)
        {
            return false;
        }

        // Pawns can move forward one or two squares on their first move
        if (Math.Abs(rowDifference) > 2 || columnDifference > 1)
        {
            return false;
        }

        // Pawns can move forward two squares only on their first move
        if (Math.Abs(rowDifference) == 2)
        {
            if (Color == PieceColor.White && Position.Row != 1 || Color == PieceColor.Black && Position.Row != 6)
            {
                return false;
            }
        }

        // Pawns can move diagonally only if they are capturing a piece
        if (columnDifference == 1)
        {
            Piece? pieceAtNewPosition = chessBoard.GetPieceAtPosition(newPosition.Row, newPosition.Column);
            if (pieceAtNewPosition == null || pieceAtNewPosition.Color == Color)
            {
                return false;
            }
        }

        // Pawns cannot capture pieces directly in front of them
        if (columnDifference == 0)
        {
            Piece? pieceAtNewPosition = chessBoard.GetPieceAtPosition(newPosition.Row, newPosition.Column);
            if (pieceAtNewPosition != null)
            {
                return false;
            }
        }

        return true;
    }
    public override List<(int Row, int Column)> CalculateLegalMoves(Chessboard chessBoard)
    {
        var legalMoves = new List<(int Row, int Column)>();

        // Check if the new position is within the bounds of the chessboard
        if (Color == PieceColor.White)
        {
            // Pawn can move forward one row (unless it's the first move, then it can move two rows)
            if (Position.Row - 1 >= 0)
            {
                legalMoves.Add((Position.Row - 1, Position.Column));

                if (Position.Row == 6)
                {
                    legalMoves.Add((Position.Row - 2, Position.Column));
                }
            }
        }
        else if (Color == PieceColor.Black)
        {
            // Pawn can move forward one row (unless it's the first move, then it can move two rows)
            if (Position.Row + 1 < 8)
            {
                legalMoves.Add((Position.Row + 1, Position.Column));

                if (Position.Row == 1)
                {
                    legalMoves.Add((Position.Row + 2, Position.Column));
                }
            }
        }

        // Check for possible captures

        return legalMoves;
    }
}
