using System.Diagnostics;
using MVC.Models;

public sealed class PlaybackEngine : IPlaybackEngine
{
    private Process? _process;

    public bool IsPlaying => _process is { HasExited: false };

    public void Play(Track track)
    {
        StopProcess();

        // "Wrap" around the FFMpeg process, by using C#'s ProcessStartInfo class
        var psi = new ProcessStartInfo
        {
            FileName = "ffplay",
            Arguments = $"-nodisp -autoexit -loglevel quiet \"{track.Path}\"",
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        _process = Process.Start(psi);
    }

    public void Stop()
    {
        StopProcess();
    }

    public Task PlayAsync(Track track, CancellationToken token)
    {
        // if a song is already playing, we can "override" the currently playing song with a new one, using the StopProcess() method.
        StopProcess();

        // "Wrap" around the FFMpeg process, by using C#'s ProcessStartInfo class
        var psi = new ProcessStartInfo
        {
            FileName = "ffplay",
            Arguments = $"-nodisp -autoexit -loglevel quiet \"{track.Path}\"",
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        _process = Process.Start(psi);

        token.Register(() => StopProcess());

        return Task.CompletedTask;

    }

    public Task StopAsync()
    {
        StopProcess();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Failsafe to stop the process immediately if something fails.
    /// </summary>
    private void StopProcess()
    {
        try
        {
            if (_process is null)
            {
                return;
            }
            if (!_process.HasExited)
            {
                _process.Kill(entireProcessTree: true);
                _process.WaitForExit(500);
            }
        }
        catch
        {
            // empty catch block, catch "anything"
        }
        finally
        {
            _process?.Dispose();
            _process = null;
        }
    }
}