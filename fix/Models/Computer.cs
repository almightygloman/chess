/*
    Using 
*/

public class Computer : Player{
    public Computer() {
    }

    public Move GenerateMove(Game game){
        Move[] moves = game.LegalMoves().ToArray();
        Random rand = new ();

        return moves[rand.Next(0, moves.Length)];        
    }
    

}
