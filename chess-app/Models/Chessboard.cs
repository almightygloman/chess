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
         // Add black pieces
        AddPiece(new Rook("Black Rook", PieceColor.Black, (0, 0)));
        AddPiece(new Knight("Black Knight", PieceColor.Black, (0, 1)));
        AddPiece(new Bishop("Black Bishop", PieceColor.Black, (0, 2)));
        AddPiece(new Queen("Black Queen", PieceColor.Black, (0, 3)));
        AddPiece(new King("Black King", PieceColor.Black, (0, 4)));
        AddPiece(new Bishop("Black Bishop", PieceColor.Black, (0, 5)));
        AddPiece(new Knight("Black Knight", PieceColor.Black, (0, 6)));
        AddPiece(new Rook("Black Rook", PieceColor.Black, (0, 7)));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(new Pawn("Black Pawn", PieceColor.Black, (1, i)));
        }
        // Add white pieces
        AddPiece(new Rook("White Rook", PieceColor.White, (7, 0)));
        AddPiece(new Knight("White Knight",PieceColor.White, (7, 1)));
        AddPiece(new Bishop("White Bishop",PieceColor.White, (7, 2)));
        AddPiece(new Queen("White Queen",PieceColor.White, (7, 3)));
        AddPiece(new King("White King", PieceColor.White, (7, 4)));
        AddPiece(new Bishop("White Bishop" ,PieceColor.White, (7, 5)));
        AddPiece(new Knight("White Knight", PieceColor.White, (7, 6)));
        AddPiece(new Rook("White Rook", PieceColor.White, (7, 7)));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(new Pawn("White Pawn", PieceColor.White, (6, i)));
        }
    }


    public void AddPiece(Piece piece)
    {
        var (row, col) = piece.Position;
        board[row][col] = piece;
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

        // Update the piece's position
        piece.Position = newPosition;

        // Place the piece at the new position
        board[newPosition.row][newPosition.col] = piece;

        // Remove the piece from its current position
        board[currentPosition.Row][currentPosition.Column] = null;

        
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