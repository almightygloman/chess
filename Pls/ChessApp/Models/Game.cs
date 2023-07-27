using System;
using System.Collections.Generic;
public enum GameState
{
    Active,
    Checkmate,
    Draw
}

public class Game
{
    public Chessboard Chessboard { get; set; }
    public GameState State { get; private set; } = GameState.Active;
    public bool isWhiteTurn { get; set; }
    public Game()
    {
        Chessboard = new Chessboard();
        isWhiteTurn = true;
        Chessboard.AddInitialPieces();
    }

    public bool IsValidMove(Piece? sourcePiece, int targetRow, int targetColumn)
    {
        if (sourcePiece == null || !IsPieceTurn(sourcePiece) || IsMoveBlockedBySameColorPiece(sourcePiece, targetRow, targetColumn))
        {
            return false;
        }

        var boardState = Chessboard.GetBoardState();
        if (!sourcePiece.CanMoveTo((targetRow, targetColumn), boardState) || IsPinned(sourcePiece,boardState) && !CanMoveToUnpin(sourcePiece, targetRow, targetColumn))
        {
            return false;
        }

        var checkingPieces = GetCheckingPieces(sourcePiece.Color, boardState);

        if (checkingPieces.Count > 0)
        {
            return CanResolveCheck(sourcePiece, (targetRow, targetColumn), checkingPieces);
        }

        return true;
    }

    private bool IsPieceTurn(Piece piece)
    {
        return piece.Color == PieceColor.White && isWhiteTurn || piece.Color == PieceColor.Black && !isWhiteTurn;
    }

    private bool IsMoveBlockedBySameColorPiece(Piece sourcePiece, int targetRow, int targetColumn)
    {
        Piece? targetPiece = this.Chessboard.GetPieceAtPosition(targetRow, targetColumn);
        return targetPiece != null && targetPiece.Color == sourcePiece.Color;
    }

    private bool CanMoveToUnpin(Piece piece, int targetRow, int targetColumn)
    {
        var tempBoard = Chessboard.GetBoardState();
        tempBoard[piece.Position.Row][piece.Position.Column] = null;
        tempBoard[targetRow][targetColumn] = piece;
        return !IsPinned(piece, tempBoard);
    }

    private bool CanResolveCheck(Piece sourcePiece, (int row, int column) targetPosition, List<Piece> checkingPieces)
    {
        if (sourcePiece.Type == PieceType.King && IsPositionUnderAttack(targetPosition, sourcePiece.Color == PieceColor.White ? PieceColor.Black : PieceColor.White, Chessboard.GetBoardState()))
        {
            return false;
        }

        if (checkingPieces.Count > 1 && sourcePiece.Type != PieceType.King)
        {
            return false;
        }

        var checkingPiece = checkingPieces[0];
        if (checkingPiece.Type == PieceType.Knight)
        {
            return sourcePiece.Type == PieceType.King;
        }

        return sourcePiece.CanMoveTo(checkingPiece.Position, Chessboard.GetBoardState()) || IsMoveResolvingCheck(sourcePiece, targetPosition);
    }

    public Piece? GetPieceAtPosition(int row, int col)
    {
        return Chessboard.GetPieceAtPosition(row, col);
    }

    public void SwitchTurn()
    {
        isWhiteTurn = !isWhiteTurn;
    }


    public bool MovePiece(int sourceRow, int sourceColumn, int targetRow, int targetColumn, Piece?[][] boardState)
    {
        // Get the piece at the source position
        Piece? sourcePiece = boardState[sourceRow][sourceColumn];

        if (sourcePiece == null)
        {
            return false;
        }

        // Check if the move is valid
        if (!IsValidMove(sourcePiece, targetRow, targetColumn))
        {
            // Invalid move
            return false;
        }

        // Move the piece on the chessboard
        Chessboard.MovePiece(sourcePiece, (targetRow, targetColumn));

        // Update the turn color
        SwitchTurn();

        return true;
    }

    public List<Piece> GetCheckingPieces(PieceColor kingColor, Piece?[][] board)
    {
        List<Piece> checkingPieces = new List<Piece>();

        (int row, int col) kingPosition = GetKingPosition(kingColor, board);

        // Checking every position on the board for an enemy piece that can reach the king
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece? piece = board[row][col];
                if (piece != null && piece.Color != kingColor)
                {
                    var possibleMoves = piece.CalculateLegalMoves(this, false);
                    if (possibleMoves.Any(move => move.Row == kingPosition.row && move.Column == kingPosition.col))
                    {
                        if (piece.Type != PieceType.Knight && !IsPathClear((row, col), kingPosition, board))
                        {
                            // There's a piece blocking the check, so continue with the next piece
                            continue;
                        }
                        checkingPieces.Add(piece);
                    }
                }
            }
        }

        return checkingPieces;
    }




    private (int row, int col) GetKingPosition(PieceColor kingColor, Piece?[][] board)
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


    public bool IsKingInCheck(PieceColor kingColor, Piece?[][] boardState)
    {
        var kingPosition = GetKingPosition(kingColor, boardState);
        return IsKingInCheck(kingPosition, kingColor, boardState);
    }

    public bool IsKingInCheck((int row, int col) kingPosition, PieceColor kingColor, Piece?[][] boardState)
    {
        var checkingPieces = GetCheckingPieces(kingColor, boardState);
        foreach (var checkingPiece in checkingPieces)
        {
            if (IsPathClear(checkingPiece.Position, kingPosition, boardState))
            {
                return true; // The king is in check
            }
        }
        return false; // No pieces are checking the king
    }

    public bool IsPathClear((int row, int col) position1, (int row, int col) position2, Piece?[][] boardState)
    {
        // calculate the deltas and direction between two positions
        var dx = position2.col - position1.col;
        var dy = position2.row - position1.row;

        // calculate direction, considering the zero case
        var dirX = dx == 0 ? 0 : dx / Math.Abs(dx);
        var dirY = dy == 0 ? 0 : dy / Math.Abs(dy);

        // start from the next position
        var currentX = position1.col + dirX;
        var currentY = position1.row + dirY;

        // we need to stop one cell before the target, as the target cell is the destination of the piece
        while (currentX != position2.col || currentY != position2.row)
        {
            Console.WriteLine($"Current X: {currentX}, Current Y: {currentY}"); // Log current values
            if (boardState[currentY][currentX] != null)
            {
                Console.WriteLine("Path blocked"); // Log when path is blocked
                return false; // there's a piece blocking the path
            }

            // when moving diagonally, change both x and y
            if (dirX != 0) currentX += dirX;
            if (dirY != 0) currentY += dirY;
        }

        return true;
    }


    public bool IsPinned(Piece piece, Piece?[][] board)
    {
        var kingPosition = GetKingPosition(piece.Color, board);

        var (deltaRow, deltaCol) = (piece.Position.Row - kingPosition.row, piece.Position.Column - kingPosition.col);

        // Return false if the piece is not in line with the King either diagonally, horizontally or vertically
        if (Math.Abs(deltaRow) != Math.Abs(deltaCol) && (piece.Position.Row != kingPosition.row && piece.Position.Column != kingPosition.col))
        {
            return false;
        }
        if ((Math.Abs(deltaRow) != Math.Abs(deltaCol) && deltaRow != 0 && deltaCol != 0) || (deltaRow == 0 && deltaCol == 0))
        {
            return false;
        }


        int stepRow = (deltaRow != 0) ? deltaRow / Math.Abs(deltaRow) : 0;
        int stepCol = (deltaCol != 0) ? deltaCol / Math.Abs(deltaCol) : 0;

        var checkPosition = (Row: kingPosition.row + stepRow, Column: kingPosition.col + stepCol);

        while (checkPosition != piece.Position)
        {
            if (board[checkPosition.Row][checkPosition.Column] != null)
            {
                return false;
            }

            checkPosition = (checkPosition.Row + stepRow, checkPosition.Column + stepCol);
        }

        checkPosition = (piece.Position.Row + stepRow, piece.Position.Column + stepCol);

        while (checkPosition.Row >= 0 && checkPosition.Row < 8 && checkPosition.Column >= 0 && checkPosition.Column < 8)
        {
            var checkingPiece = board[checkPosition.Row][checkPosition.Column];

            if (checkingPiece != null)
            {
                if (checkingPiece.Color != piece.Color && (checkingPiece.Type == PieceType.Queen ||
                    (checkingPiece.Type == PieceType.Rook && (stepRow == 0 || stepCol == 0)) ||
                    (checkingPiece.Type == PieceType.Bishop && stepRow != 0 && stepCol != 0) ||
                    (checkingPiece.Type == PieceType.Knight)))
                {
                    return true;
                }

                else
                {
                    break;
                }
            }

            checkPosition = (checkPosition.Row + stepRow, checkPosition.Column + stepCol);
        }


        return false;
    }

    public bool IsPositionUnderAttack((int Row, int Column) position, PieceColor attackingColor, Piece?[][] board)
    {
        // Go through every piece on the board
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece? piece = board[row][col];

                // If there is a piece of the attacking color
                if (piece != null && piece.Color == attackingColor)
                {
                    // Check if the piece can move to the target position
                    if (piece.CanMoveTo(position, board))
                    {
                        // If it can, then the position is under attack
                        return true;
                    }
                }
            }
        }

        // If no piece of the attacking color can move to the position, it's not under attack
        return false;
    }

    public bool IsMoveResolvingCheck(Piece sourcePiece, (int row, int col) targetPosition)
    {
        // Create a copy of the current board state
        var tempBoardState = Chessboard.GetBoardState();

        // Move the piece in the copied board state
        tempBoardState[targetPosition.row][targetPosition.col] = sourcePiece;

        bool isKingInCheck = IsKingInCheck(sourcePiece.Color, tempBoardState);
        if (isKingInCheck)
        {
            Console.WriteLine($"Move from {sourcePiece.Position} to {targetPosition} does not resolve check.");
        }
        else
        {
            Console.WriteLine($"Move from {sourcePiece.Position} to {targetPosition} resolves check.");
        }

        // Check if the king is still in check in the copied board state
        return !isKingInCheck;
    }




    private List<(int row, int col)> GetPath((int row, int col) from, (int row, int col) to)
    {
        var path = new List<(int row, int col)>();
        var (fromRow, fromCol) = from;
        var (toRow, toCol) = to;

        int dRow = toRow - fromRow;
        int dCol = toCol - fromCol;

        int rowStep = dRow != 0 ? dRow / Math.Abs(dRow) : 0; // Determine the step for row (could be -1, 0, or 1)
        int colStep = dCol != 0 ? dCol / Math.Abs(dCol) : 0; // Determine the step for column (could be -1, 0, or 1)

        int currentRow = fromRow + rowStep;
        int currentCol = fromCol + colStep;

        // We loop until we reach the destination coordinates
        while (currentRow != toRow || currentCol != toCol)
        {
            path.Add((currentRow, currentCol));

            currentRow += rowStep;
            currentCol += colStep;
        }

        return path;
    }


    public List<Piece> GetAllPieces()
    {
        var board = Chessboard.GetBoardState();
        List<Piece> pieces = new List<Piece>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece? piece = board[i][j];
                if (piece != null)
                {
                    pieces.Add(piece);
                }
            }
        }
        return pieces;
    }





    //startGame
    //Stalemate function
    //(mate) functions
    //endGame
}