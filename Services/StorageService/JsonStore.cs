using System.Text.Json;

namespace MVC.Services;

public sealed class JsonStore
{
    // Create a private readonly JsonSerializerOption field
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    /// <summary>
    /// Save data to a database asynchronously 
    /// </summary>
    /// <typeparam name="T">whichever datatype is required when called as a paramater argument</typeparam>
    /// <param name="filePath">specified path to save the file on</param>
    /// <param name="data">data to save</param>
    /// <param name="token">cancellation token</param>
    /// <returns>Task</returns>
    public async Task SaveAsync<T>(string filePath, T data, CancellationToken token)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory); // create a new directory if Database/, db/, Context/ etc does not exists.
        }

        await using var stream = File.Create(filePath); // create and await an open file stream, where we can create a database file, if one does not already exist.
        await JsonSerializer.SerializeAsync(stream, data, _options, token); // use the JsonSerializer to write to a database json file, and fill in the content from the filestream that is opened on the line above this one.
    }

    /// <summary>
    /// Load data from the database
    /// </summary>
    /// <typeparam name="T">whichever datatype is required when called as a paramater argument</typeparam>
    /// <param name="filePath">The path to the file we load from</param>
    /// <param name="fallBack">fallback data</param>
    /// <param name="token">cancellation token</param>
    /// <returns>Task</returns>
    public async Task<T> LoadAsync<T>(string filePath, T fallBack, CancellationToken token)
    {
        if (!File.Exists(filePath))
        {
            return fallBack;
        }

        await using var stream = File.OpenRead(filePath);
        var loaded = await JsonSerializer.DeserializeAsync<T>(stream, _options, token);
        return loaded ?? fallBack;
    }
}