using MVC.Models;
namespace MVC.Services;

/// <summary>
/// Internal enum helper class
/// </summary>
public enum TrackSortMode
{
    TitleAscending = 1,
    FolderAscending = 2,
    LastWriteDescription = 3
}

public sealed class LibraryIndex
{
    private readonly IReadOnlyList<Track> _allTracks;

    public LibraryIndex(IReadOnlyList<Track> allTracks)
    {
        _allTracks = allTracks;
    }

    public IReadOnlyList<Track> Query(string searchTerm, TrackSortMode sortMode)
    {
        // use an IEnumerable to query through the track collection
        IEnumerable<Track> query = _allTracks;

        // check if the search term is valid
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim();
            query = query.Where(track =>
                track.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                track.Path.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        query = sortMode switch
        {
            TrackSortMode.TitleAscending => query.OrderBy(track => track.Title),
            TrackSortMode.FolderAscending => query.OrderBy(track => track.Path),
            TrackSortMode.LastWriteDescription => query.OrderByDescending(track => SafeLastWriteUtc(track.Path)),
            _ => query
        };

        return query.ToList();
    }

    // helper method
    private static DateTime SafeLastWriteUtc(string path)
    {
        try
        {
            return File.GetLastWriteTimeUtc(path);
        }
        catch
        {
            return DateTime.MinValue;
        }
    }
}
