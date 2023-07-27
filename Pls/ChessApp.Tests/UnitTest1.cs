using NUnit.Framework;

namespace ChessApp.Tests;

public class Tests
{
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
            Piece king = new King(PieceColor.White, (4, 0));
            Piece queen = new Queen(PieceColor.Black, (4, 7));
            game.Chessboard.AddPiece(king);
            game.Chessboard.AddPiece(queen);

            bool isCheck = game.IsKingInCheck(PieceColor.White, game.Chessboard.GetBoardState());
            Assert.IsTrue(isCheck);
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
    }
}