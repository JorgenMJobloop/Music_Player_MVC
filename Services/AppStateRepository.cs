using MVC.Models;

namespace MVC.Services;

public sealed class AppStateRepository
{
    private readonly JsonStore _storageService;
    private readonly string _path;

    public AppStateRepository(JsonStore storageService, string appName = "KH Music Player CLI")
    {
        _storageService = storageService;
        var baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        _path = Path.Combine(baseDirectory, appName, "db.json");
    }

    public Task<AppState> LoadAsync(CancellationToken token)
    {
        return _storageService.LoadAsync(_path, new AppState(), token);
    }

    public Task SaveAsync(AppState state, CancellationToken token)
    {
        return _storageService.SaveAsync(_path, state, token);
    }
}