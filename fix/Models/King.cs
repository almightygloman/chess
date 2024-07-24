using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
public class King : Piece
{
    public King(PieceColor color, (int Row, int Column) position)
            : base(PieceType.King, color == PieceColor.White ? "white_king.png" : "black_king.png", color, position, color == PieceColor.White ? 'K' : 'k')
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition, Chessboard chessboard)
    {

        Piece?[][] board = chessboard.GetBoardState();
        if (newPosition.Row < 0 || newPosition.Row >= 8 || newPosition.Column < 0 || newPosition.Column >= 8)
        {
            return false;
        }

        if (Math.Abs(newPosition.Row - Position.Row) > 1)
        {
            return false;
        }
        if (Math.Abs(newPosition.Column - Position.Column) > 1){
            return CanCastle(newPosition, chessboard);
        }

        if (newPosition == Position) return false;

        Piece? pieceAtNewPosition = board[newPosition.Row][newPosition.Column];
        if (pieceAtNewPosition != null && pieceAtNewPosition.Color == this.Color)
        {
            return false;
        }

        return true;
    }

    public override bool CanAttack((int Row, int Column) position, Chessboard chessboard)
    {
        return CanMoveTo(position, chessboard);
    }

    public bool CanCastle((int row, int col)newPosition, Chessboard chessboard)
        {
            int ndx = Color == PieceColor.White ? 0 : 1;
            var result = newPosition switch
            {
                (0, 6) => Position == (0, 4) && areSquaresPlayable([(0,5),(0,6)]) && chessboard.castle[0],
                (0, 2) => Position == (0, 4) && areSquaresPlayable([(0,3),(0,2),(0,1)]) && chessboard.castle[1],
                (7, 6) => Position == (7, 4) && areSquaresPlayable([(7,5),(7,6)]) && chessboard.castle[2],
                (7, 2) => Position == (7, 4) && areSquaresPlayable([(7,3),(7,2),(7,1)]) && chessboard.castle[3],
                _ => false,
            };

            bool areSquaresPlayable((int row, int col)[]positions){
                foreach((int row, int col) in positions){
                    if(chessboard.GetFromPosition((row,col)) != null  || chessboard.IsPositionUnderAttack((row, col), Color)) return false;
                }
                return true;
            }
            return result && !chessboard.IsKingInCheck(ndx) ;
        }


}