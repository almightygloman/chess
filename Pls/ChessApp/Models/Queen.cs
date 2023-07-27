using System.Collections.Generic;
public class Queen : Piece
{
    public Queen(PieceColor color, (int Row, int Column) position)
            : base(PieceType.Queen, color == PieceColor.White ? "white_queen.png" : "black_queen.png", color, position)
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition, Piece?[][] board)
    {
        var (newRow, newCol) = newPosition;
        var (currentRow, currentCol) = this.Position;
        bool isDiagonal;
        // Check if the move is diagonal
        if (Math.Abs(newRow - currentRow) != Math.Abs(newCol - currentCol))
        {
            isDiagonal = false;
        }
        else { isDiagonal = true; }
        // Check if the move is either horizontal or vertical
        bool isHorizontalMove = newRow == currentRow;
        bool isVerticalMove = newCol == currentCol;

        if (!isHorizontalMove && !isVerticalMove && !isDiagonal)
        {
            // Rook can only move horizontally or vertically
            return false;
        }
        // Determine the direction of the movement
        int rowDirection = (newRow > currentRow) ? 1 : (newRow < currentRow) ? -1 : 0;
        int colDirection = (newCol > currentCol) ? 1 : (newCol < currentCol) ? -1 : 0;

        // Check for pieces blocking the path
        int pathLength = Math.Max(Math.Abs(newRow - currentRow), Math.Abs(newCol - currentCol));
        for (int i = 1; i < pathLength; i++)
        {
            int checkRow = currentRow + i * rowDirection;
            int checkCol = currentCol + i * colDirection;

            // If there is a piece in the path, the move is not valid
            if (board[checkRow][checkCol] != null)
            {
                return false;
            }
        }
        // Check if there is a piece in the destination square
        Piece? destinationPiece = board[newPosition.Row][newPosition.Column];
        if (destinationPiece != null && destinationPiece.Color == Color)
        {
            // There is a piece of the same color in the destination square
            return false;
        }


        return true;
    }

    public override List<(int Row, int Column)> CalculateLegalMoves(Game game, bool checkKingSafety = true)
    {
        List<(int Row, int Column)> legalMoves = new List<(int Row, int Column)>();
        var (currentRow, currentCol) = this.Position;

        // Vertical and Horizontal moves
        for (int dir = 0; dir < 8; dir++)
        {
            for (int distance = 1; distance < 8; distance++)
            {
                int checkRow, checkCol;
                switch (dir)
                {
                    case 0: // north
                        checkRow = currentRow - distance;
                        checkCol = currentCol;
                        break;
                    case 1: // northeast
                        checkRow = currentRow - distance;
                        checkCol = currentCol + distance;
                        break;
                    case 2: // east
                        checkRow = currentRow;
                        checkCol = currentCol + distance;
                        break;
                    case 3: // southeast
                        checkRow = currentRow + distance;
                        checkCol = currentCol + distance;
                        break;
                    case 4: // south
                        checkRow = currentRow + distance;
                        checkCol = currentCol;
                        break;
                    case 5: // southwest
                        checkRow = currentRow + distance;
                        checkCol = currentCol - distance;
                        break;
                    case 6: // west
                        checkRow = currentRow;
                        checkCol = currentCol - distance;
                        break;
                    default: // northwest
                        checkRow = currentRow - distance;
                        checkCol = currentCol - distance;
                        break;
                }

                // Skip if the position is outside the board
                if (checkRow < 0 || checkRow >= 8 || checkCol < 0 || checkCol >= 8)
                {
                    break;
                }

                // Check if there is a piece in the position
                var pieceAtPosition = game.Chessboard.GetPieceAtPosition(checkRow, checkCol);
                if (pieceAtPosition != null)
                {
                    if (pieceAtPosition.Color != this.Color)
                    {
                        // If there is an enemy piece in the position, add it as a legal move
                        legalMoves.Add((checkRow, checkCol));
                    }

                    // Break as we cannot jump over a piece
                    break;
                }

                // Add the position as a legal move
                legalMoves.Add((checkRow, checkCol));
            }
        }

        return legalMoves;
    }


}