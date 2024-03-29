using System.Collections.Generic;
public class Rook : Piece
{
    public Rook(PieceColor color, (int Row, int Column) position)
        : base(PieceType.Rook, color == PieceColor.White ? "white_rook.png" : "black_rook.png", color, position)
    { }


    public override bool CanMoveTo((int Row, int Column) newPosition, Piece?[][] board)
    {
        // Check if the new position is within the bounds of the chessboard
        if (newPosition.Row < 0 || newPosition.Row > 7 || newPosition.Column < 0 || newPosition.Column > 7)
        {
            return false;
        }


        // Check if the move is either horizontal or vertical
        bool isHorizontalMove = newPosition.Row == Position.Row;
        bool isVerticalMove = newPosition.Column == Position.Column;

        if (!isHorizontalMove && !isVerticalMove)
        {
            // Rook can only move horizontally or vertically
            return false;
        }

        // Check if there are pieces in the way
        int rowDirection = 0;
        int colDirection = 0;

        if (isHorizontalMove)
        {
            colDirection = newPosition.Column > Position.Column ? 1 : -1;
        }
        else
        {
            rowDirection = newPosition.Row > Position.Row ? 1 : -1;
        }

        int currentRow = Position.Row + rowDirection;
        int currentCol = Position.Column + colDirection;

        while (currentRow != newPosition.Row || currentCol != newPosition.Column)
        {
            // Check if there is a piece in the current square
            if (board[currentRow][currentCol] != null)
            {
                // There is a piece blocking the path
                return false;
            }

            currentRow += rowDirection;
            currentCol += colDirection;
        }
        // Check if there is a piece in the destination square
        Piece? destinationPiece = board[newPosition.Row][newPosition.Column];
        if (destinationPiece != null && destinationPiece.Color == Color)
        {
            // There is a piece of the same color in the destination square
            return false;
        }


        // No pieces are in the way, the move is valid
        return true;
    }

    public override bool CanAttack((int Row, int Column) position, Piece?[][] board)
    {
        if (!CanMoveTo(position, board)) return false;
        return true;
    }
    public override List<(int Row, int Column)> CalculateLegalMoves(Game game, bool checkKingSafety = true)
    {
        var legalMoves = new List<(int Row, int Column)>();

        // The directions that the rook can move. Up, down, left, right.
        (int, int)[] directions = new (int, int)[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

        foreach (var direction in directions)
        {
            int step = 1;
            while (true)
            {
                int newRow = Position.Row + direction.Item1 * step;
                int newCol = Position.Column + direction.Item2 * step;

                // Check that the position is inside the board
                if (newRow < 0 || newRow >= 8 || newCol < 0 || newCol >= 8)
                {
                    break;
                }

                // Get the piece that is in the new position
                var pieceAtNewPos = game.Chessboard.GetPieceAtPosition(newRow, newCol);

                // If the square is empty, add it to the legal moves
                if (pieceAtNewPos == null)
                {
                    legalMoves.Add((newRow, newCol));
                }
                else
                {
                    // If there is a piece in the new position and it is of a different color, 
                    // add the position to the legal moves and stop searching in this direction
                    if (pieceAtNewPos.Color != this.Color)
                    {
                        legalMoves.Add((newRow, newCol));
                    }
                    break;
                }
                step++;
            }
        }

        return legalMoves;
    }
}
