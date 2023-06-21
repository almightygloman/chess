public class Rook : Piece
{
    public Rook(PieceColor color, (int Row, int Column) position)
        : base(color == PieceColor.White ? "white_rook.png" : "black_rook.png", color, position)
    { }


    public override bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessBoard)
    {
        // Check if the new position is within the bounds of the chessboard
        if (newPosition.Row < 0 || newPosition.Row >= 8 || newPosition.Column < 0 || newPosition.Column >= 8)
        {
            return false;
        }

        // Rook can move horizontally or vertically
        return newPosition.Row == Position.Row || newPosition.Column == Position.Column;
    }
    public override List<(int Row, int Column)> CalculateLegalMoves(Chessboard chessBoard)
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
                var pieceAtNewPos = chessBoard.GetPieceAtPosition(newRow, newCol);

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
