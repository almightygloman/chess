public abstract class Piece
{
    public string ImagePath { get; protected set; }
    public string Color { get; protected set; }
    public bool IsCaptured { get; set; }
    public (int Row, int Column) Position { get; set; }

    // Constructor
    public Piece(string imagePath, string color, (int Row, int Column) position)
    {
        ImagePath = imagePath;
        Color = color;
        IsCaptured = false;
        Position = position;
    }
    // Method to check if the piece can move to the specified position
    public abstract bool CanMoveTo((int Row, int Column) newPosition);


    // Other methods and properties specific to each type of piece
}