using System.Collections.Generic;
public class Pawn : Piece
{
    public bool EnPassantTarget = false;
    public Pawn(PieceColor color, (int Row, int Column) position)
        : base(PieceType.Pawn, color == PieceColor.White ? "white_pawn.png" : "black_pawn.png", color, position)
    {
    }
    public override bool CanMoveTo((int Row, int Column) newPosition, Piece?[][] board)
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
            Piece? pieceAtNewPosition = board[newPosition.Row][newPosition.Column];
            if (pieceAtNewPosition == null || pieceAtNewPosition.Color == Color)
            {
                return false;
            }
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
        EnPassantTarget = false;
        return true;
    }
    public override bool CanAttack((int Row, int Column) position, Piece?[][] board)
    {
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

        return true;
    }


    public bool isPromoted(){
        if(this.Position.Row.Equals(7) && this.Color.Equals(PieceColor.White) || 
          this.Position.Row.Equals(0) && this.Color.Equals(PieceColor.Black)){
            
            return true;
        }
        return false;
    }

    public void setPosition((int row, int col)position){
        this.Position = position;
    }

}