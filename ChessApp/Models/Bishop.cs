using System.Collections.Generic;
public class Bishop : Piece
{
    public Bishop(PieceColor color, (int Row, int Column) position)
        : base(PieceType.Bishop, color == PieceColor.White ? "white_bishop.png" : "black_bishop.png", color, position)
    { }

    public override bool CanMoveTo((int Row, int Column) newPosition, Piece?[][] board)
    {
        var (newRow, newCol) = newPosition;
        var (currentRow, currentCol) = this.Position;

        // Check if the move is diagonal
        if (Math.Abs(newRow - currentRow) != Math.Abs(newCol - currentCol))
        {
            return false;
        }

        // Determine the direction of the movement
        int rowDirection = (newRow > currentRow) ? 1 : -1;
        int colDirection = (newCol > currentCol) ? 1 : -1;

        // Check for pieces blocking the path
        int pathLength = Math.Abs(newRow - currentRow);
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

        return true;
    }
    public override bool CanAttack((int Row, int Column) position, Piece?[][] board)
    {
        if (!CanMoveTo(position, board)) return false;
        return true;
    }


    public override List<(int Row, int Column)> CalculateLegalMoves(Game game, bool checkKingSafety = true)
    {
        List<(int Row, int Column)> legalMoves = new List<(int Row, int Column)>();
        var (currentRow, currentCol) = this.Position;

        // Check diagonal directions: top-left, top-right, bottom-right, bottom-left
        int[][] directions = new int[][] {
        new int[] {-1, -1},
        new int[] {-1, 1},
        new int[] {1, 1},
        new int[] {1, -1}
    };

        foreach (int[] direction in directions)
        {
            int rowDirection = direction[0];
            int colDirection = direction[1];

            for (int i = 1; i < 8; i++)
            {
                int checkRow = currentRow + i * rowDirection;
                int checkCol = currentCol + i * colDirection;

                // Check if the position is within the board
                if (checkRow < 0 || checkRow >= 8 || checkCol < 0 || checkCol >= 8)
                {
                    break;
                }

                // Check if there is a piece in the path
                var pieceAtPosition = game.GetPieceAtPosition(checkRow, checkCol);
                if (pieceAtPosition != null)
                {
                    // If the piece is of opposite color, the bishop can capture it
                    if (pieceAtPosition.Color != this.Color)
                    {
                        // The piece can be captured, but let's check if this would put our king in check
                        if (checkKingSafety)
                        {
                            var tempBoard = game.Chessboard.GetBoardState();
                            tempBoard[checkRow][checkCol] = tempBoard[currentRow][currentCol];  // Move our piece
                            tempBoard[currentRow][currentCol] = null;  // Remove it from its original position

                            if (!game.IsKingInCheck(this.Color, tempBoard))
                            {
                                legalMoves.Add((checkRow, checkCol));
                            }
                        }
                        else
                        {
                            legalMoves.Add((checkRow, checkCol));
                        }
                    }

                    // If the piece is of the same color or of opposite color, the bishop cannot move further in this direction
                    break;
                }
                else
                {
                    // The square is empty, but let's check if this would put our king in check
                    if (checkKingSafety)
                    {
                        var tempBoard = game.Chessboard.GetBoardState();
                        tempBoard[checkRow][checkCol] = tempBoard[currentRow][currentCol];  // Move our piece
                        tempBoard[currentRow][currentCol] = null;  // Remove it from its original position

                        if (!game.IsKingInCheck(this.Color, tempBoard))
                        {
                            legalMoves.Add((checkRow, checkCol));
                        }
                    }
                    else
                    {
                        legalMoves.Add((checkRow, checkCol));
                    }
                }
            }
        }

        return legalMoves;
    }



}
