public class Pawn : Piece
{
    public Pawn(PieceColor color, (int Row, int Column) position)
        : base(PieceType.Pawn, color == PieceColor.White ? "white_pawn.png" : "black_pawn.png", color, position)
    {
    }
    public override bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessboard)
    {
        Piece?[][] board = chessboard.GetBoardState();
        // Check if the position is within the bounds of the chessboard
        if (newPosition.Row < 0 || newPosition.Row >= 8 || newPosition.Column < 0 || newPosition.Column >= 8)
        {
            return false;
        }

        if (newPosition == Position) return false;

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
            if (Color == PieceColor.White && Position.Row != 1 || Color == PieceColor.Black && Position.Row != 6)
            {
                return false;
            }

            if (Color == PieceColor.Black)
            {
                chessboard.EnPassantTarget = (5, newPosition.Column);
            }
            else
            {
                chessboard.EnPassantTarget = (2, newPosition.Column);
            }
            return true;
        }

        // Pawns cannot capture pieces directly in front of them
        if (columnDifference == 0)
        {
            Piece? pieceAtNewPosition = board[newPosition.Row][newPosition.Column];
            if (pieceAtNewPosition != null)
            {
                return false;
            }

        }
        chessboard.EnPassantTarget = null;
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

        // Pawns can only capture in one column away
        if (columnDifference != 1)
        {
            return false;
        }

        if (columnDifference == 1 && Math.Abs(rowDifference) < 1) return false;

        //enPassant
        if (chessboard.GetFromPosition(position) == null && chessboard.EnPassantTarget == position)
        {
            if (Color == PieceColor.White)
                chessboard.SetPieceAtPosition((4, position.Column), null);
            else
                chessboard.SetPieceAtPosition((3, position.Column), null);

            Console.WriteLine("en crossiant");

            return true;
        }
        else
            if (chessboard.GetFromPosition(position) == null) return false;

        chessboard.EnPassantTarget = null;
        return true;
    }

}