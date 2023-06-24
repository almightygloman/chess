public enum PieceColor
{
    White,
    Black
}

public abstract class Piece
{
    public string ID{get; protected set;}
    public string ImagePath { get; protected set; }
    public PieceColor Color { get; protected set; }
    public bool IsCaptured { get; set; }
    public (int Row, int Column) Position { get; set; }

    // Constructor
    public Piece(string id, string imagePath, PieceColor color, (int Row, int Column) position)
    {
        ID = id;
        ImagePath = imagePath;
        Color = color;
        IsCaptured = false;
        Position = position;
    }

    // Method to check if the piece can move to the specified position
    public abstract bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessBoard);
    public abstract List<(int Row, int Column)> CalculateLegalMoves(Chessboard chessBoard);

    // Other methods and properties specific to each type of piece
}