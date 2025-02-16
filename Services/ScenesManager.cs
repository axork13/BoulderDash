public interface IScenesManager
{
    public void Load<T>(object[]? args) where T : Scene, new();
}

public class ScenesManager : IScenesManager
{
    private Scene? _currentScene;
    public ScenesManager()
    {
        // On ajoute la scène créée directement dans le manager
        Services.Register<IScenesManager>(this);
    }

    public void Load<T>(object[]? args) where T : Scene, new()
    {
        // Si la scène courante n'est pas null on unload la scènee
        _currentScene?.Unload ();
        _currentScene = new T();
        _currentScene.Load(args);
    }
    public void Update() => _currentScene?.Update();

    public void Draw() => _currentScene?.Draw();

}