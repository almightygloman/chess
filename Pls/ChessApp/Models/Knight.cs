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

    public override List<(int Row, int Column)> CalculateLegalMoves(Game game, bool checkKingSafety = true)
    {
        // Create a list to hold the legal moves
        List<(int Row, int Column)> legalMoves = new List<(int Row, int Column)>();

        // Define the potential moves for a knight
        int[] moveRows = new int[] { -2, -1, 1, 2, -2, -1, 1, 2 };
        int[] moveCols = new int[] { -1, -2, -2, -1, 1, 2, 2, 1 };

        // Check each potential move to see if it is a valid move
        for (int i = 0; i < 8; i++)
        {
            int newRow = Position.Row + moveRows[i];
            int newCol = Position.Column + moveCols[i];

            // If the move is valid, add it to the list of legal moves
            if (CanMoveTo((newRow, newCol), game.Chessboard.GetBoardState()))
            {
                legalMoves.Add((newRow, newCol));
            }
        }

        // Return the list of legal moves
        return legalMoves;
    }
}