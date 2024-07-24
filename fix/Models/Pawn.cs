public class Pawn : Piece
{
    public Pawn(PieceColor color, (int Row, int Column) position)
        : base(PieceType.Pawn, color == PieceColor.White ? "white_pawn.png" : "black_pawn.png", color, position, color == PieceColor.White ? 'P' : 'p')
    {

    }
    public override bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessboard)
    {
        // Check if the position is within the bounds of the chessboard
        if (newPosition.Row < 0 || newPosition.Row >= 8 || newPosition.Column < 0 || newPosition.Column >= 8)
        {
            return false;
        }

        if (newPosition == Position) return false;

        if(chessboard.GetFromPosition(newPosition) != null) return false;

        // Calculate the difference between the current and new positions
        int rowDifference = newPosition.Row - Position.Row;
        int columnDifference = Math.Abs(newPosition.Column - Position.Column);

        // Pawns can only move forward
        if (Color == PieceColor.White && rowDifference < 0 || Color == PieceColor.Black && rowDifference > 0)
        {
            return false;
        }

        // Pawns can move forward one or two squares on their first move
        if (Math.Abs(rowDifference) > 2 || columnDifference > 1) return false;

        if (columnDifference > 0) return false;

        // Pawns can move forward two squares only on their first move
        if (Math.Abs(rowDifference) == 2)
        {
            (int x, int y) = Color == PieceColor.White ? (1, 1) : (-1,6);
            return Position.Row == y && chessboard.GetFromPosition((newPosition.Row-x, newPosition.Column))==null;
        }

        return true;
    }
    public override bool CanAttack((int Row, int Column) position, Chessboard chessboard)
    {
        if (position == Position) return false;

        // Calculate the difference between the current and new positions
        int rowDifference = position.Row - Position.Row;
        int columnDifference = Math.Abs(position.Column - Position.Column);

        // Pawns can only capture diagonally
        if (Color == PieceColor.Black && rowDifference != -1 || Color == PieceColor.White && rowDifference != 1)
        {
            return false;
        }

        Piece? piece = chessboard.GetFromPosition(position);

        // Pawns can only capture in one column away
        if (columnDifference != 1)
        {
            return false;
        }

        if (columnDifference == 1 && Math.Abs(rowDifference) < 1) return false;

        //enPassant
        if (piece == null && chessboard.EnPassantTarget == position)
        {
            if (Color == PieceColor.White && position.Row != 5
            || Color == PieceColor.Black && position.Row != 2)
                return false;

            return true;
        }
        
        if (piece == null) return false;
        else if (piece.Color == Color) return false;


        return true;
    }

    //void Promote()

}