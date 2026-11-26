namespace LogHub.Processing;
public static class PluginCatalog
{
    public static IReadOnlyList<T> Discover<T>() where T : class
    {
        var result = new List<T>();
        foreach (var type in typeof(T).Assembly.GetTypes())
        {
            if (!type.IsClass || type.IsAbstract || !typeof(T).IsAssignableFrom(type)) continue;
            if (type.GetConstructor(Type.EmptyTypes) is null) continue;
            if (Activator.CreateInstance(type) is T instance) result.Add(instance);
        }
        return result;
    }
}