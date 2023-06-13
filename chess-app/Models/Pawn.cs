public class Pawn : Piece
{
    public Pawn(string color, (int Row, int Column) position)
        : base(color == "White" ? "white_pawn.png" : "black_pawn.png", color, position)
    {
    }

    public override bool CanMoveTo((int Row, int Column) newPosition)
    {
        // Check if the new position is within the bounds of the chessboard
        if (newPosition.Row < 0 || newPosition.Row >= 8 || newPosition.Column < 0 || newPosition.Column >= 8)
        {
            return false;
        }

        // Check the movement rules for the pawn based on its color
        if (Color == "White")
        {
            // Pawn can move forward one row (unless it's the first move, then it can move two rows)
            if (newPosition.Row == Position.Row - 1 && newPosition.Column == Position.Column)
            {
                return true;
            }

            if (Position.Row == 6 && newPosition.Row == 4 && newPosition.Column == Position.Column)
            {
                return true;
            }
        }
        else if (Color == "Black")
        {
            // Pawn can move forward one row (unless it's the first move, then it can move two rows)
            if (newPosition.Row == Position.Row + 1 && newPosition.Column == Position.Column)
            {
                return true;
            }

            if (Position.Row == 1 && newPosition.Row == 3 && newPosition.Column == Position.Column)
            {
                return true;
            }
        }

        // If none of the conditions are met, the move is not valid for the pawn
        return false;
    }
}
