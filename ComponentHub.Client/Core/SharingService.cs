namespace ComponentHub.Client.Core;

/// <summary>
/// Service class for sharing arbitrary state between components.
/// </summary>
/// <typeparam name="TItem"></typeparam>
internal sealed class SharingService<TItem>
{
    private readonly Dictionary<string, TItem?> _store = new();

    /// <summary>
    /// Adds an item to storage and returns the id to access it.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public string AddItemToStorage(TItem? item)
    {
        var id = Guid.NewGuid().ToString();
        _store.Add(id, item);
        return id;
    }

    
    /// <summary>
    /// Gets an item by it's id from the service and removes it from storage.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TItem? GetItem(string id)
    {
        return _store.Remove(id, out var item) ? item : default;
    }
}