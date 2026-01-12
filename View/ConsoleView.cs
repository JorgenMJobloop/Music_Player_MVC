using MVC.Models;
using Spectre.Console;

namespace MVC.Views;

public sealed class ConsoleView
{
    public void Render(IReadOnlyList<Track> tracks, PlayerState state, int selectedSongIndex)
    {
        AnsiConsole.Clear();

        var header = new Panel($"[bold]KH Music Player CLI[/]\nStatus: [yellow]{state.Status}[/]\nNow: [green]{state.CurrentTrack?.Title ?? "-"}[/]")
            .Border(BoxBorder.Rounded)
            .Header("Now playing", Justify.Center);

        AnsiConsole.Write(header);
        AnsiConsole.WriteLine(); // empty newline

        var table = new Table().RoundedBorder();
        table.AddColumn("#");
        table.AddColumn("Title");

        for (int i = 0; i < Math.Min(tracks.Count, 25); i++)
        {
            var t = tracks[i];
            var title = i == selectedSongIndex ? $"[black on yellow]> {t.Title}[/]" : t.Title;
            table.AddRow((i + 1).ToString(), title);
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[dim]KeyUp/KeyDown: Choose song. Enter: Play S: stop Q: quit[/]");
    }
}