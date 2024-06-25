public enum PieceColor
{
    White,
    Black
}

public enum PieceType
{
    King, Queen, Bishop, Knight, Rook, Pawn,
}

public abstract class Piece
{
    public PieceType Type { get; set; }
    public string ImagePath { get; set; }
    public PieceColor Color { get; set; }
    public (int Row, int Column) Position { get; set; }


    // Constructor
    public Piece(PieceType type, string imagePath, PieceColor color, (int Row, int Column) position)
    {
        Type = type;
        ImagePath = imagePath;
        Color = color;
        Position = position;
    }

    // Method to check if the piece can move to the specified position
    public abstract bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessboard);

    public abstract bool CanAttack((int Row, int Column) position, Chessboard chessboard);

}