using MVC.Models;

public interface IPlaybackEngine
{
    void Play(Track track); // cannot be awaited, must be called in real-time
    void Stop();
    Task PlayAsync(Track track, CancellationToken token); // can be awaited, and called async
    Task StopAsync();
    bool IsPlaying { get; }
}