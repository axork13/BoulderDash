public static class Services
{
    /* Dictionnaire qui contient tout nos services */
    private static Dictionary<Type, object> _services = new Dictionary<Type, object>();

    /* On enregistre notre service dans le dictionnaire */
    public static void Register<T>(T service)
    {
        if (_services.ContainsKey(typeof(T))) throw new InvalidOperationException($"Service of type {typeof(T)} is already registered.");

        if (service == null) throw new Exception($"Parameter service is null");
        _services.Add(typeof(T), service);
    }

    /* On recupère un service en fonction de son type */
    public static T Get<T>()
    {
        if (!_services.ContainsKey(typeof(T))) throw new InvalidOperationException($"Service of type {typeof(T)} is not registered.");
        return (T)_services[typeof(T)];
    }
}