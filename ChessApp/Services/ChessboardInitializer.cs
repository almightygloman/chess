using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

public class ChessboardInitializer : IHostedService
{
    private readonly Chessboard _chessboard;

    public ChessboardInitializer(Chessboard chessboard)
    {
        _chessboard = chessboard;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Add pieces to the chessboard
        _chessboard.AddInitialPieces();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}