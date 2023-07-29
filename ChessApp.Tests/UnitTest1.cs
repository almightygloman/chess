using NUnit.Framework;

namespace ChessApp.Tests;

public class Tests
{
    //testing Game class methods
    [SetUp]
    public void Setup()
    {

    }

    [TestFixture]
    public class ChessTests
    {
        [Test]
        public void TestIsPinned()
        {
            Game game = new Game();
            Piece king = new King(PieceColor.White, (4, 0));
            Piece queen = new Queen(PieceColor.Black, (4, 7));
            Piece bishop = new Bishop(PieceColor.White, (4, 1));
            game.Chessboard.AddPiece(king);
            game.Chessboard.AddPiece(queen);
            game.Chessboard.AddPiece(bishop);

            bool isPinned = game.IsPinned(bishop,game.Chessboard.GetBoardState());
            Assert.IsTrue(isPinned);
        }

        [Test]
        public void TestIsKingInCheck()
        {
            Game game = new Game();
            Piece king = new King(PieceColor.White, (0, 3));
            Piece pawn = new Pawn(PieceColor.Black, (1, 4));
            Piece queen = new Queen(PieceColor.White, (0,4));
            game.Chessboard.AddPiece(king);
            game.Chessboard.AddPiece(pawn);

            bool isCheck = game.IsKingInCheck(PieceColor.White, game.Chessboard.GetBoardState());
            bool canmove = game.MovePiece(0, 4, 1, 4, game.Chessboard.GetBoardState());
            Assert.IsTrue(isCheck);
            Assert.IsFalse(canmove);
        }

        [Test]
        public void TestGetCheckingPieces()
        {
            Game game = new Game();
            Piece king = new King(PieceColor.White, (4, 0));
            Piece queen = new Queen(PieceColor.Black, (4, 7));
            game.Chessboard.AddPiece(king);
            game.Chessboard.AddPiece(queen);

            var checkingPieces = game.GetCheckingPieces(PieceColor.White, game.Chessboard.GetBoardState());
            Assert.AreEqual(1, checkingPieces.Count);
            Assert.AreEqual(PieceType.Queen, checkingPieces[0].Type);
        }

        [Test]
        public void TestIsPathClear()
        {
            Game game = new Game();
            Piece bishop = new Bishop(PieceColor.White, (0, 0));
            Piece rook = new Rook(PieceColor.Black, (7, 7));
            game.Chessboard.AddPiece(bishop);
            game.Chessboard.AddPiece(rook);

            bool isPathClear = game.IsPathClear(bishop.Position, rook.Position, game.Chessboard.GetBoardState());
            Assert.IsTrue(isPathClear);
        }
        [Test]
        public void TestIsUnderAttack(){
            Game game = new Game();
            Piece wRook = new Rook(PieceColor.White, (0,0));
            Piece bRook = new Rook(PieceColor.Black, (0,1));
            game.Chessboard.AddPiece(bRook);
            game.Chessboard.AddPiece(wRook);

            bool wIsPositionUnderAttack = game.IsPositionUnderAttack((wRook.Position.Row, wRook.Position.Column), PieceColor.Black, game.Chessboard.GetBoardState());
            bool bIsPositionUnderAttack = game.IsPositionUnderAttack((bRook.Position.Row, bRook.Position.Column), PieceColor.White, game.Chessboard.GetBoardState());

            
            Assert.IsTrue(wIsPositionUnderAttack);
        }
        [Test]
        public void TestMovePiece(){
            Game game = new Game();
            game.isWhiteTurn = true;
            Pawn pawn = new Pawn(PieceColor.Black, (2,3));
            King king = new King(PieceColor.White, (0,3));
            game.Chessboard.AddPiece(pawn);
            game.Chessboard.AddPiece(king);
            bool canKMove = king.CanMoveTo((1,3), game.Chessboard.GetBoardState());
            Assert.IsTrue(canKMove);
        }
        [Test]
        public void TestIsMoveResolvingCheck(){
            Game game = new Game();
            game.isWhiteTurn = true;
            Piece wking = new King(PieceColor.White, (0,4));
            Piece bbishop = new Bishop(PieceColor.Black, (3,1));
            Piece bqueen = new Queen(PieceColor.Black, (1,3));
            Piece wpawn = new Pawn(PieceColor.White, (1,4));
            game.Chessboard.AddPiece(wking);
            game.Chessboard.AddPiece(bbishop);
            game.Chessboard.AddPiece(bqueen);
            game.Chessboard.AddPiece(wpawn);

            bool resolve = game.IsValidMove(wpawn, 2, 4);
            Assert.IsFalse(resolve);

        }
    }
}