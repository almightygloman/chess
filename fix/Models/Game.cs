using System.ComponentModel;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.ModelBinding;
public class Game
{
    public Player? White;
    public Player? Black;
    public Chessboard Chessboard { get; set; }

    public Dictionary<String, Chessboard> History = [];

    public int halfMoves;
    //for analysis
    public Game()
    {
        Chessboard = new Chessboard();
        Chessboard.AddInitialPieces();
        halfMoves = 0;
        this.White = null;
        this.Black = null;
        Console.WriteLine(Chessboard.GenerateFEN());
    }
    //for games
    public Game(Player white, Player black){
        Chessboard = new Chessboard();
        Chessboard.AddInitialPieces();
        this.White = white;
        this.Black = black;
    }


    public bool IsValidMove(Move move)
    {
        Piece? sourcePiece = Chessboard.GetFromPosition(move.SourcePosition);

        if (sourcePiece == null)
        {
            Console.WriteLine("Source piece is null.");
            return false;
        }

        //only the moving color can move
        if (!sourcePiece.Color.Equals(PieceColor.White) && Chessboard.WhiteToMove == true
         || sourcePiece.Color.Equals(PieceColor.White) && Chessboard.WhiteToMove == false) return false;

        //checking psuedo legal moves
        if (!sourcePiece.CanMoveTo(move.TargetPosition, Chessboard) && !sourcePiece.CanAttack(move.TargetPosition, Chessboard)) return false;

        return Chessboard.TestMove(move);
    }



    public bool MovePiece(Move move)
    {
        if (!IsValidMove(move)) return false;
        // Move the piece on the chessboard
        Chessboard.MovePiece(move);
        History.Add(Chessboard.GenerateFEN(), Chessboard);
        return true;
    }

    //returns all legal moves on the board
    public Stack<Move> LegalMoves(){
        Stack<Move> moves = new();
        Stack<Piece> pieces = Chessboard.GetPieces();

        foreach(Piece piece in pieces){
            for(int i = 0; i<8; i++){
                for(int j = 0; j < 8; j++){
                    Move move = new Move(piece.Position, (i,j));
                    if(IsValidMove(move)) moves.Push(move);
                }
            }
        }

        return moves;
    }
    
    //returns all legal moves for a specific piece
    public Stack<Move> LegalMoves(PieceColor pieceColor){
        Stack<Move> moves = new();
        var pieces = Chessboard.GetPieces(pieceColor);
        foreach(Piece piece in pieces){
            for(int i = 0; i<8; i++){
                for(int j = 0; j < 8; j++){
                    Move move = new Move(piece.Position, (i,j));
                    if(IsValidMove(move)) moves.Push(move);
                }
            }
        }
        return moves;
    }

    public (int row,int col)[] LegalMoves(Piece? piece) {
        Stack<(int row, int col)> targets = new();
        if(piece == null) return [.. targets];
        foreach(Move move in LegalMoves(piece.Color)){
            if(move.SourcePosition == piece.Position) targets.Push(move.TargetPosition);
        }
        return [.. targets];
    }


    // int MoveGeneration(int depth){
    //     if(depth == 0) return 1;

    //     Stack<Move> moves = LegalMoves();
    //     int numPositions = 0;

    //     foreach(Move move in moves){
            
    //         MovePiece(move);
    //         numPositions += MoveGeneration(depth-1);
    //         UnMovePiece(move);
    //     }

    //     return numPositions;
    // }    

    // UnMovePiece(Move move){

    // }

    
}
