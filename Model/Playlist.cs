namespace MVC.Models;

public sealed class Playlist
{
    public string Name { get; set; } = string.Empty;
    public List<string> TrackPaths { get; set; } = new List<string>();
}