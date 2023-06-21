public class Chessboard
{
    private Piece?[][] board;

    public Chessboard()
    {
        board = new Piece?[8][];
        for (int i = 0; i < 8; i++)
        {
            board[i] = new Piece?[8];
        }
    }

    public void AddInitialPieces()
    {
        // Add white pieces
        AddPiece(new Rook(PieceColor.White, (0, 0)));
        AddPiece(new Pawn(PieceColor.White, (0, 1)));
        AddPiece(new Bishop(PieceColor.White, (0, 2)));
        AddPiece(new Pawn(PieceColor.White, (0, 3)));
        AddPiece(new Pawn(PieceColor.White, (0, 4)));
        AddPiece(new Bishop(PieceColor.White, (0, 5)));
        AddPiece(new Pawn(PieceColor.White, (0, 6)));
        AddPiece(new Rook(PieceColor.White, (0, 7)));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(new Rook(PieceColor.White, (1, i)));
        }

        // Add black pieces
        AddPiece(new Rook(PieceColor.Black, (7, 0)));
        AddPiece(new Pawn(PieceColor.Black, (7, 1)));
        AddPiece(new Bishop(PieceColor.Black, (7, 2)));
        AddPiece(new Pawn(PieceColor.Black, (7, 3)));
        AddPiece(new Pawn(PieceColor.Black, (7, 4)));
        AddPiece(new Bishop(PieceColor.Black, (7, 5)));
        AddPiece(new Pawn(PieceColor.Black, (7, 6)));
        AddPiece(new Rook(PieceColor.Black, (7, 7)));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(new Bishop(PieceColor.Black, (6, i)));
        }
    }


    public void AddPiece(Piece piece)
    {
        var (row, col) = piece.Position;
        board[row][col] = piece;
    }

    public void RemovePiece(Piece piece)
    {
        var (row, col) = piece.Position;
        board[row][col] = null;
    }

    public Piece? GetPieceAtPosition(int row, int col)
    {
        // check if the position is valid
        if (row < 0 || row >= 8 || col < 0 || col >= 8)
        {
            throw new ArgumentException("Invalid board position");
        }

        // get the piece at the given position
        var piece = board[row][col];

        // return the piece (which may be null)
        return piece;
    }

    public void MovePiece(Piece piece, (int row, int col) newPosition)
    {
        // Check if the position is within the bounds of the board
        if (newPosition.row < 0 || newPosition.row >= 8 || newPosition.col < 0 || newPosition.col >= 8)
        {
            throw new ArgumentException("Invalid target position");
        }

        // Get the piece's current position
        var currentPosition = piece.Position;

        // Remove the piece from its current position
        board[currentPosition.Row][currentPosition.Column] = null;

        // Update the piece's position
        piece.Position = newPosition;

        // Place the piece at the new position
        board[newPosition.row][newPosition.col] = piece;
    }
    public Piece?[][] GetBoardState()
    {
        Piece?[][] boardState = new Piece?[8][];
        for (int i = 0; i < 8; i++)
        {
            boardState[i] = new Piece?[8];
            for (int j = 0; j < 8; j++)
            {
                boardState[i][j] = board[i][j];
            }
        }

        return boardState;
    }
}