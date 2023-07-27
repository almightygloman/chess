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

                // If the piece can move to the new position, return false
                if (piece.CanMoveTo(newPosition, board))
                {
                    return false;
                }
            }
        }
        return true;
    }


    public override List<(int Row, int Column)> CalculateLegalMoves(Game game, bool checkKingSafety = true)
    {
        List<(int Row, int Column)> legalMoves = new List<(int Row, int Column)>();
        var (currentRow, currentCol) = this.Position;

        // Check all surrounding squares
        for (int row = -1; row <= 1; row++)
        {
            for (int col = -1; col <= 1; col++)
            {
                int checkRow = currentRow + row;
                int checkCol = currentCol + col;

                // Skip if the position is outside the board
                if (checkRow < 0 || checkRow >= 8 || checkCol < 0 || checkCol >= 8)
                {
                    continue;
                }

                // Skip if the position is the current position
                if (row == 0 && col == 0)
                {
                    continue;
                }

                // Check if there is a piece in the position
                var pieceAtPosition = game.Chessboard.GetPieceAtPosition(checkRow, checkCol);
                if (pieceAtPosition != null && pieceAtPosition.Color == this.Color)
                {
                    // Skip if the piece is of the same color
                    continue;
                }

                // Add the position as a legal move
                legalMoves.Add((checkRow, checkCol));
            }
        }

        return legalMoves;
    }

}