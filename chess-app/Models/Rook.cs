public class Rook : Piece
{
    public Rook(string color, (int Row, int Column) position)
        : base(color == "White" ? "white_rook.png" : "black_rook.png", color, position)
    { }


    public override bool CanMoveTo((int Row, int Column) newPosition)
    {
        // Check if the new position is within the bounds of the chessboard
        if (newPosition.Row < 0 || newPosition.Row >= 8 || newPosition.Column < 0 || newPosition.Column >= 8)
        {
            return false;
        }

        // Rook can move horizontally or vertically
        return newPosition.Row == Position.Row || newPosition.Column == Position.Column;
    }
}