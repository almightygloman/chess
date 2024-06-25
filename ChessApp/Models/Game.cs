using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class Game
{
    public Chessboard Chessboard { get; set; }
    public readonly struct Move
    {
        public readonly (int row, int col) SourcePosition;
        public readonly (int row, int col) TargetPosition;

        public Move((int Row, int Column) source, (int Row, int Col) target) : this()
        {
            this.SourcePosition = source;
            this.TargetPosition = target;
        }
    }

    public int totalMoves;
    public Game()
    {
        Chessboard = new Chessboard();
        Chessboard.AddInitialPieces();
        totalMoves = 0;
    }


    public bool IsValidMove(Move move)
    {
        Piece? sourcePiece = Chessboard.GetFromPosition(move.SourcePosition);

        if (sourcePiece == null)
        {
            Console.WriteLine("Source piece is null.");
            return false;
        }
        
        //checking psuedo legal moves
        if(!sourcePiece.CanMoveTo(move.TargetPosition, Chessboard) && !sourcePiece.CanAttack(move.TargetPosition, Chessboard)) return false;

        //only the moving color can move
        if(!sourcePiece.Color.Equals(PieceColor.White) && Chessboard.WhiteToMove == true 
         || sourcePiece.Color.Equals(PieceColor.White) && Chessboard.WhiteToMove == false) return false;


        //if king is in check before and after the move is made, the move is invalid 

        PieceColor colorToMove = Chessboard.WhiteToMove ? PieceColor.White : PieceColor.Black;

        Chessboard.TempMove(move);

        if(IsKinginCheck(colorToMove, Chessboard)){
            Console.WriteLine("Move does not resolve check");
            Chessboard.UndoTempMove(move);
            return false;
        }else{
            Chessboard.UndoTempMove(move);
        }

        Console.WriteLine("The move is valid.");
        return true;
    }




    private static (int row, int col) GetKingPosition(PieceColor kingColor, Piece?[][] board)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece? piece = board[i][j];
                if (piece != null && piece.Type == PieceType.King && piece.Color == kingColor)
                {
                    return (i, j);
                }
            }
        }
        throw new InvalidOperationException("King not found");
    }

    //use this determine legal moves... if the king is in check after a move, it is not a legal move
    public bool IsKinginCheck(PieceColor color, Chessboard chessboard){
        (int row, int col) kingPos = GetKingPosition(color, chessboard.GetBoardState());
        Piece? king = chessboard.GetFromPosition(kingPos) ?? throw new Exception("error in GetKingPosition");

        //for all pieces on board of the moving color
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var sourcePiece = Chessboard.GetFromPosition((i, j));
                if (sourcePiece == null) continue;
                //if any piece of the non moving color can move to our king
                if (sourcePiece.Color != color && sourcePiece.CanAttack(king.Position, chessboard))
                {
                    return true;
                }
            }
        }
        return false;
    }


    public bool MovePiece(Move move)
    {

        // Get the piece at the source position
        Piece? sourcePiece = Chessboard.GetFromPosition(move.SourcePosition);

        if (!IsValidMove(move)) return false;

        // Move the piece on the chessboard
        Chessboard.MovePiece(move);
        return true;
    }

}
