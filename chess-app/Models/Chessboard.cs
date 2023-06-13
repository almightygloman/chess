public class Chessboard
{
    private Piece?[,] board;

    public Chessboard()
    {
        board = new Piece?[8, 8];
    }

    public void AddInitialPieces()
    {
        // Add white pieces
        AddPiece(new Rook("White", (0, 0)));
        AddPiece(new Pawn("White", (0, 1)));
        AddPiece(new Bishop("White", (0, 2)));
        AddPiece(new Pawn("White", (0, 3)));
        AddPiece(new Pawn("White", (0, 4)));
        AddPiece(new Bishop("White", (0, 5)));
        AddPiece(new Pawn("White", (0, 6)));
        AddPiece(new Rook("White", (0, 7)));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(new Pawn("White", (1, i)));
        }

        // Add black pieces
        AddPiece(new Rook("Black", (7, 0)));
        AddPiece(new Pawn("Black", (7, 1)));
        AddPiece(new Bishop("Black", (7, 2)));
        AddPiece(new Pawn("Black", (7, 3)));
        AddPiece(new Pawn("Black", (7, 4)));
        AddPiece(new Bishop("Black", (7, 5)));
        AddPiece(new Pawn("Black", (7, 6)));
        AddPiece(new Rook("Black", (7, 7)));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(new Pawn("Black", (6, i)));
        }
    }

    public void AddPiece(Piece piece)
    {
        (int row, int column) = piece.Position;
        board[row, column] = piece;
    }

    public void RemovePiece(Piece piece)
    {
        (int row, int column) = piece.Position;
        board[row, column] = null;
    }

    public Piece? GetPieceAtPosition((int Row, int Column) position)
    {
        return board[position.Row, position.Column];
    }

    public void MovePiece(Piece piece, (int Row, int Column) newPosition)
    {
        RemovePiece(piece);
        piece.Position = newPosition;
        AddPiece(piece);
    }

    public Piece?[,] GetBoardState()
    {
        Piece?[,] boardState = new Piece?[8, 8];

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                boardState[row, col] = board[row, col];
            }
        }

        return boardState;
    }
}