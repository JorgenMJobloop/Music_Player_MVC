namespace MVC.Models;

public sealed class AppState
{
    public string? LibraryFolder { get; set; }
    public string? LastPlayedPath { get; set; }
    public float Volume { get; set; } = 1.0f;
    public List<string> RecentlyPlayed { get; set; } = new List<string>();
    public Dictionary<string, Playlist> Playlists { get; set; } = new Dictionary<string, Playlist>();
}
