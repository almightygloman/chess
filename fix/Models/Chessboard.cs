using System.Collections;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Routing.Tree;
public class Chessboard
{
    private Piece?[][] Board { get; set; }

    public bool WhiteToMove { get; set; }

    public (int row, int col)? EnPassantTarget { get; set; }

    //castle[0] represents K(white king side), [1] : Q, [2] : k, [3] : q
    public bool[] castle { get; set; } = [true, true, true, true];

    public King[] Kings = new King[2];

    public Move LastMove { get; set; }

    public int halfMoves;
    public int fullMoves=1;

    // public Time   

    public Chessboard()
    {
        Board = new Piece?[8][];
        for (int i = 0; i < 8; i++)
        {
            Board[i] = new Piece?[8];
        }
        WhiteToMove = true;
    }

    public void AddInitialPieces()
    {
        // Add black pieces
        AddPiece(new Rook(PieceColor.White, (0, 0)));
        AddPiece(new Knight(PieceColor.White, (0, 1)));
        AddPiece(new Bishop(PieceColor.White, (0, 2)));
        AddPiece(new Queen(PieceColor.White, (0, 3)));
        AddPiece(new King(PieceColor.White, (0, 4)));
        AddPiece(new Bishop(PieceColor.White, (0, 5)));
        AddPiece(new Knight(PieceColor.White, (0, 6)));
        AddPiece(new Rook(PieceColor.White, (0, 7)));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(new Pawn(PieceColor.White, (1, i)));
        }
        // Add white pieces
        AddPiece(new Rook(PieceColor.Black, (7, 0)));
        AddPiece(new Knight(PieceColor.Black, (7, 1)));
        AddPiece(new Bishop(PieceColor.Black, (7, 2)));
        AddPiece(new Queen(PieceColor.Black, (7, 3)));
        AddPiece(new King(PieceColor.Black, (7, 4)));
        AddPiece(new Bishop(PieceColor.Black, (7, 5)));
        AddPiece(new Knight(PieceColor.Black, (7, 6)));
        AddPiece(new Rook(PieceColor.Black, (7, 7)));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(new Pawn(PieceColor.Black, (6, i)));
        }
    }


    public String ToRankFile((int row,int col)pos){
        String res = "";
        switch(pos.row){
            case 0:
                res += "a";
            break;
            case 1:
                res += "b";
            break;
            case 2:
                res += "c";
            break;
            case 3:
                res += "d";
            break;
            case 4:
                res += "e";
            break;
            case 5:
                res += "f";
            break;
            case 6:
                res += "g";
            break;
            case 7:
                res += "h";
            break;
        }
        return res+pos.col;
    }


    public void AddPiece(Piece piece)
    {
        var (row, col) = piece.Position;
        Board[row][col] = piece;
        if (piece.Type == PieceType.King)
        {
            if (piece.Color == PieceColor.White) Kings[0] = (King)piece;
            else Kings[1] = (King)piece;
        }
    }

    public void MovePiece(Move move)
    {
        // Check if the position is within the bounds of the board
        if (move.TargetPosition.row < 0 || move.TargetPosition.row >= 8 || move.TargetPosition.col < 0 || move.TargetPosition.col >= 8)
        {
            throw new ArgumentException("Invalid target position");
        }

        Piece? piece = GetFromPosition(move.SourcePosition) ?? throw new Exception(move.SourcePosition + " is empty");

        if (piece.Type == PieceType.Pawn && move.TargetPosition == EnPassantTarget)
        {
            if (piece.Color == PieceColor.White)
                SetPieceAtPosition((4, move.TargetPosition.col), null);
            else
                SetPieceAtPosition((3, move.TargetPosition.col), null);

            Console.WriteLine("en crossiant");
        }


        if (piece.Type == PieceType.Rook)
        {
            if (move.SourcePosition == (0, 7)) castle[0] = false;
            if (move.SourcePosition == (0, 0)) castle[1] = false;
            if (move.SourcePosition == (7, 7)) castle[2] = false;
            if (move.SourcePosition == (7, 0)) castle[3] = false;
        }

        if (piece.Type == PieceType.King)
        {
            (int x, int y) = piece.Color == PieceColor.White ? (0, 1) : (2, 3);
            King king = Kings[piece.Color == PieceColor.White ? 0 : 1];
            if (king.CanCastle(move.TargetPosition, this)) Castle(move.TargetPosition);
            castle[x] = castle[y] = false;
        }


        if (!(piece.Type == PieceType.Pawn && Math.Abs(move.TargetPosition.row - move.SourcePosition.row) == 2))
            EnPassantTarget = null;
        else
        {
            int deez = piece.Color == PieceColor.White ? 1 : -1;
            EnPassantTarget = (move.TargetPosition.row - (1 * deez), move.TargetPosition.col);
        }

        if (piece.Type == PieceType.Pawn)
        {
            if (move.TargetPosition.row == 7 || move.TargetPosition.row == 0)
            {
                piece = new Queen(piece.Color, piece.Position);
            }
        }

        if (piece.Type != PieceType.Pawn && GetFromPosition(move.TargetPosition) == null)
        {
            halfMoves++;
        }else halfMoves = 0;

        if ( piece.Color == PieceColor.Black) fullMoves++;

        // Place the piece at the new position
        SetPieceAtPosition(move.TargetPosition, piece);

        // Remove the piece from its current position
        SetPieceAtPosition(move.SourcePosition, null);

        WhiteToMove = !WhiteToMove;

        LastMove = move;

        Console.WriteLine(GenerateFEN());
    }


    public bool TestMove(Move move)
    {
        Piece?[][] board = GetBoardState();
        (int, int)? pt = EnPassantTarget;

        var sourcePiece = GetFromPosition(move.SourcePosition);

        if (sourcePiece == null) return false;

        if (sourcePiece.Type == PieceType.King && Math.Abs(move.TargetPosition.col - move.SourcePosition.col) > 1)
        {
            King king = (King)sourcePiece;
            if (king.CanCastle(move.TargetPosition, this))
                Castle(move.TargetPosition);
        }
        SetPieceAtPosition(move.TargetPosition, sourcePiece);
        SetPieceAtPosition(move.SourcePosition, null);

        int ndx = sourcePiece.Color == PieceColor.White ? 0 : 1;
        bool result = !IsKingInCheck(ndx);

        SetBoard(board);
        EnPassantTarget = pt;

        return result;
    }

    public bool IsKingInCheck(int ndx)
    {
        var king = Kings[ndx];
        bool result = false;
        foreach (Piece piece in GetPieces(ndx == 0 ? PieceColor.Black : PieceColor.White))
        {
            if (piece.CanAttack(king.Position, this)) result = true;
        }
        return result;
    }


    public Piece?[][] GetBoardState()
    {
        Piece?[][] boardState = new Piece?[8][];
        for (int i = 0; i < 8; i++)
        {
            boardState[i] = new Piece?[8];
            for (int j = 0; j < 8; j++)
            {
                boardState[i][j] = Board[i][j];
            }
        }

        return boardState;
    }

    public Stack<Piece> GetPieces()
    {
        Stack<Piece> pieces = new();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece? piece = Board[i][j];
                if (piece != null) pieces.Push(piece);
            }
        }

        return pieces;
    }

    public Stack<Piece> GetPieces(PieceColor color)
    {
        Stack<Piece> result = new();
        Stack<Piece> pieces = GetPieces();
        while (pieces.Count > 0)
        {
            if (pieces.Peek().Color == color) result.Push(pieces.Pop());
            else pieces.Pop();
        }
        return result;
    }

    public Piece? GetFromPosition((int row, int col) Position)
    {
        Piece? piece = Board[Position.row][Position.col];
        return piece;
    }

    private void SetBoard(Piece?[][] board)
    {
        this.Board = board;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece? piece = Board[i][j];
                if (piece != null) piece.Position = (i, j);
            }
        }
    }


    public void SetPieceAtPosition((int row, int col) position, Piece? piece)
    {
        if (position.row < 0 || position.row >= 8 || position.col < 0 || position.col >= 8)
        {
            throw new ArgumentOutOfRangeException("Position is out of board bounds.");
        }

        this.Board[position.row][position.col] = piece;

        if (piece != null)
        {
            piece.Position = (position.row, position.col);
        }
    }

    public bool IsPositionUnderAttack((int row, int col) pos, PieceColor color)
    {
        PieceColor c = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        Stack<Piece> pieces = GetPieces(c);
        bool res = false;
        foreach (Piece piece in pieces)
        {
            if (piece.CanAttack(pos, this) && piece.Color != color) res = true;
        }
        return res;

    }

    public void Castle((int row, int col) newPosition)
    {
        switch (newPosition)
        {
            case (0, 6):
                Piece rook = GetFromPosition((0, 7)) ?? throw new Exception("issues with castling");
                SetPieceAtPosition((0, 5), rook);
                SetPieceAtPosition((0, 7), null);
                break;
            case (0, 2):
                Piece rook1 = GetFromPosition((0, 0)) ?? throw new Exception("issues with castling");
                SetPieceAtPosition((0, 3), rook1);
                SetPieceAtPosition((0, 0), null);
                break;
            case (7, 6):
                Piece rook2 = GetFromPosition((7, 7)) ?? throw new Exception("issues with castling");
                SetPieceAtPosition((7, 5), rook2);
                SetPieceAtPosition((7, 7), null);
                break;
            case (7, 2):
                Piece rook3 = GetFromPosition((7, 0)) ?? throw new Exception("issues with castling");
                SetPieceAtPosition((7, 3), rook3);
                SetPieceAtPosition((7, 0), null);
                break;
            default:
                break;
        }
    }




    public String GenerateFEN()
    {
        String result = "";
        for (int i = 7; i >= 0; i--)
        {
            if (isNullRow(Board[i])) result += "8/";
            else result += getRowString(Board[i]);
        }

         result=  result.Substring(0,  result.Length - 1);

            string c  = WhiteToMove ? " w " : " b ";
            
             result+= c;



            if(!castle[0]&& !castle[1]&& !castle[2]&& !castle[3])
                 result+= "-";

            if(castle[0])  result+= "K";
            if(castle[1])  result+= "Q";
            if(castle[2])  result+= "k";
            if(castle[3])  result+= "q";


            if(EnPassantTarget!=null)  result+= " " + ToRankFile(((int row, int col))EnPassantTarget)+ " ";
            else  result+= " - ";

            result += halfMoves + " ";
            result += fullMoves;


        return result;

        bool isNullRow(Piece?[] row)
        {
            foreach (Piece? piece in row)
            {
                if (piece != null) return false;
            }
            return true;
        }

        String getRowString(Piece?[] row)
        {
            string res = "";

            //keep an array of counters for # of empty positions in a line
            //if a position isnt empty, stop the counter
            int counter = 0;
            for (int i = 0; i < 8; i++)
            {
                Piece? piece = row[i];
                if (piece == null)
                {
                    counter++;
                    if (i + 1 < 8 && row[i + 1] != null)
                    {
                        res += counter;
                        counter = 0;
                    }
                }
                else
                {
                    res += piece.Initial;
                    counter = 0;
                }
            }
            if (counter != 0) res += counter;
            res += "/";

            return res;
        }
    }



}