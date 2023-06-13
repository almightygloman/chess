public abstract class Piece
{
    public char Symbol { get; protected set; }
    public string Color { get; protected set; }
    public bool IsCaptured { get; set; }
    public (int Row, int Column) Position { get; set; }

    // Constructor
    public Piece(char symbol, string color, (int Row, int Column) position)
    {
        Symbol = symbol;
        Color = color;
        IsCaptured = false;
        Position = position;
    }

    // Method to check if the piece can move to the specified position
    public abstract bool CanMoveTo((int Row, int Column) newPosition);


    // Other methods and properties specific to each type of piece
}