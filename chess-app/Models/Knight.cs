public class Knight : Piece
{
    public Knight(string id, PieceColor color, (int Row, int Column) position)
            : base(id, color == PieceColor.White ? "white_knight.png" : "black_knight.png", color, position)
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessBoard)
    {
        if (newPosition.Row < 0 || newPosition.Row >= 8 || newPosition.Column < 0 || newPosition.Column >= 8)
        {
            return false;
        }
        int rowDif = Math.Abs(newPosition.Row - Position.Row);
        int colDif = Math.Abs(newPosition.Column - Position.Column);
        if (colDif == 1 && rowDif  == 2 || colDif == 2 && rowDif == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override List<(int Row, int Column)> CalculateLegalMoves(Chessboard chessBoard)
    {
        throw new NotImplementedException();
    }
}