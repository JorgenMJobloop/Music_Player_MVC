using MVC.Models;
using MVC.Views;

namespace MVC.Contollers;

public sealed class PlayerController
{
    private readonly ConsoleView _view;
    private readonly IPlaybackEngine _engine;
    private readonly PlayerState _state = new PlayerState();
    private readonly IReadOnlyList<Track> _tracks;

    private int _selectedSongIndex;

    public PlayerController(ConsoleView view, IPlaybackEngine engine, IReadOnlyList<Track> tracks)
    {
        _view = view;
        _engine = engine;
        _tracks = tracks;
    }

    public async Task RunAsync(CancellationToken token)
    {
        if (_tracks.Count == 0)
        {
            _view.Render(_tracks, _state, 0);
            return;
        }

        while (!token.IsCancellationRequested)
        {
            _view.Render(_tracks, _state, _selectedSongIndex);

            // get keyboard input
            var key = Console.ReadKey(intercept: true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    _selectedSongIndex = Math.Max(0, _selectedSongIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    _selectedSongIndex = Math.Min(_tracks.Count - 1, _selectedSongIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    await PlaySelectedAsync(token);
                    break;
                case ConsoleKey.S:
                    await StopAsync();
                    break;
                case ConsoleKey.Q:
                    await StopAsync();
                    break;
            }
        }
    }


    private void PlaySelected()
    {
        var track = _tracks[_selectedSongIndex];
        _state.CurrentTrack = track;
        _state.Status = PlaybackStatus.Playing;
        _engine.Play(track);
    }

    private async Task PlaySelectedAsync(CancellationToken token)
    {
        var track = _tracks[_selectedSongIndex];
        _state.CurrentTrack = track;
        _state.Status = PlaybackStatus.Playing;
        await _engine.PlayAsync(track, token);
    }

    private async Task StopAsync()
    {
        await _engine.StopAsync();
        _state.Status = PlaybackStatus.Stopped;
    }
}