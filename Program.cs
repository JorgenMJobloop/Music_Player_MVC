using System.Threading.Tasks;
using MVC.Contollers;
using MVC.Views;

namespace MVC;

class Program
{
    static async Task Main(string[] args)
    {
        var cancellationToken = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; cancellationToken.Cancel(); };

        var folder = args.Length > 0 ? args[0] : Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

        var scanner = new LibraryScanner();
        var tracks = scanner.Scan(folder);

        // implementing the MVC pattern
        var view = new ConsoleView();
        var engine = new PlaybackEngine();
        var controller = new PlayerController(view, engine, tracks);

        await controller.RunAsync(cancellationToken.Token);
    }
}