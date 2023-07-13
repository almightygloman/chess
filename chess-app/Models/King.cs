public class King : Piece
{
    public King(string id, PieceColor color, (int Row, int Column) position)
            : base(id, color == PieceColor.White ? "white_king.png" : "black_king.png", color, position)
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessBoard)
    {
        if(newPosition.Row > Position.Row + 1 || newPosition.Row < Position.Row - 1 || newPosition.Column > Position.Column + 1 || newPosition.Column < Position.Column - 1){
            return false;
        }
        return true;
        
    }
    
    public override List<(int Row, int Column)> CalculateLegalMoves(Chessboard chessBoard)
    {
        throw new NotImplementedException();
    }
}