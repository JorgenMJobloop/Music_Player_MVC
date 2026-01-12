namespace MVC.Models;

public enum PlaybackStatus { Stopped = 0, Playing = 1 }

public sealed class PlayerState
{
    public PlaybackStatus Status { get; set; } = PlaybackStatus.Stopped;
    public Track? CurrentTrack { get; set; }
}