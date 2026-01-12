using MVC.Models;

public sealed class LibraryScanner
{
    private static readonly string[] FileExtensions = [".mp3", ".wav", ".flac", ".m4a", ".ogg", ".aac"];

    public IReadOnlyList<Track> Scan(string folder)
    {
        if (!Directory.Exists(folder))
        {
            return [];
        }

        return Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories)
            .Where(f => FileExtensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase))
            .Select(p => new Track(p, Path.GetFileNameWithoutExtension(p)))
            .OrderBy(t => t.Title)
            .ToList();
    }
}