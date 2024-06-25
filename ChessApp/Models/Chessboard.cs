using System.Security.Cryptography.X509Certificates;

public class Chessboard
{
    private Piece?[][] Board { get; set; }

    public bool WhiteToMove{get; set;}

    public int MoveCount = 0;

    public (int row, int col)? EnPassantTarget{get;set;} 

    public Piece? LastCaptured = null;

    public String fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    public Chessboard()
    {
        this.Board = new Piece?[8][];
        for (int i = 0; i < 8; i++)
        {
            Board[i] = new Piece?[8];
        }
        this.WhiteToMove = true;
    }



    public void AddInitialPieces()
    {
        // Add black pieces
        AddPiece(new Rook(PieceColor.White, (0, 0)));
        AddPiece(new Knight(PieceColor.White, (0, 1)));
        AddPiece(new Bishop(PieceColor.White, (0, 2)));
          AddPiece(new King(PieceColor.White, (0, 3)));
         AddPiece(new Queen(PieceColor.White, (0, 4)));
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
          AddPiece(new King(PieceColor.Black, (7, 3)));
         AddPiece(new Queen(PieceColor.Black, (7, 4)));
        AddPiece(new Bishop(PieceColor.Black, (7, 5)));
        AddPiece(new Knight(PieceColor.Black, (7, 6)));
        AddPiece(new Rook(PieceColor.Black, (7, 7)));

        for (int i = 0; i < 8; i++)
        {
           AddPiece(new Pawn(PieceColor.Black, (6, i)));
        }
    }


    public void AddPiece(Piece piece)
    {
        var (row, col) = piece.Position;
        Board[row][col] = piece;
    }

    public void MovePiece(Game.Move move)
    {
        // Check if the position is within the bounds of the board
        if (move.TargetPosition.row < 0 || move.TargetPosition.row >= 8 || move.TargetPosition.col < 0 || move.TargetPosition.col >= 8)
        {
            throw new ArgumentException("Invalid target position");
        }

        Piece? piece = GetFromPosition(move.SourcePosition);

        if(piece==null)
        {
            throw new Exception(move.SourcePosition + " is empty");
        }
        // Place the piece at the new position
        SetPieceAtPosition(move.TargetPosition, piece);

        // Remove the piece from its current position
        SetPieceAtPosition(move.SourcePosition, null);

        WhiteToMove = !WhiteToMove;

    }

    public void TempMove(Game.Move move){
        Piece? piece = GetFromPosition(move.SourcePosition);

        // Place the piece at the new position
        SetPieceAtPosition(move.TargetPosition, piece);

        // Remove the piece from its current position
        SetPieceAtPosition(move.SourcePosition, null);

    }

    public void UndoTempMove(Game.Move move){
        EnPassantTarget = null;

        Piece? piece = GetFromPosition(move.TargetPosition);

        SetPieceAtPosition(move.SourcePosition, piece);

        // Remove the piece from its current position
        SetPieceAtPosition(move.TargetPosition, null);

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

    public Piece? GetFromPosition((int row, int col) Position){
        Piece? piece = Board[Position.row][Position.col];
        return piece;
    }

    public Piece[] GetPiecesOnBoard(){
        Piece[] pieces = new Piece[32];
        
        return pieces;
    }


    public void SetPieceAtPosition((int row, int col)position, Piece? piece)
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

}