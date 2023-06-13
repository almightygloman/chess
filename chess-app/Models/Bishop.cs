public class Bishop : Piece
{
    public Bishop(string color, (int Row, int Column) position)
        : base(color == "White" ? "white_bishop.png" : "black_bishop.png", color, position)
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition)
    {
        // Check if the new position is within the bounds of the chessboard
        if (newPosition.Row < 0 || newPosition.Row >= 8 || newPosition.Column < 0 || newPosition.Column >= 8)
        {
            return false;
        }

        // Bishop can move diagonally
        int rowDifference = Math.Abs(newPosition.Row - Position.Row);
        int colDifference = Math.Abs(newPosition.Column - Position.Column);

        return rowDifference == colDifference;
    }
}