using Raylib_cs;

public interface IAssetsManager
{
    public T Get<T>(string name);
    public Texture2D GetTextureFromSet(string textureSetName, int id);
}

public class AssetsManager : IAssetsManager
{
    private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
    private Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();
    private Dictionary<string, Music> musics = new Dictionary<string, Music>();
    private Dictionary<string, Font> fonts = new Dictionary<string, Font>();
    private Dictionary<string, TextureSet> textureSets = new Dictionary<string, TextureSet>();


    public AssetsManager()
    {
        Services.Register<IAssetsManager>(this);
    }

    public void Load<T>(string name, string path)
    {
        Type assetType = typeof(T);

        switch (assetType)
        {
            case Type _ when assetType == typeof(Texture2D):
                textures.Add(name, Raylib.LoadTexture(path));
                break;
            case Type _ when assetType == typeof(Sound):
                sounds.Add(name, Raylib.LoadSound(path));
                break;
            case Type _ when assetType == typeof(Music):
                musics.Add(name, Raylib.LoadMusicStream(path));
                break;
            case Type _ when assetType == typeof(Font):
                fonts.Add(name, Raylib.LoadFont(path));
                break;
            default:
                throw new Exception($"Type {assetType} not supported");
        }
    }

    public T Get<T>(string name)
    {
        Type assetType = typeof(T);
        return assetType switch
        {
            Type _ when assetType == typeof(Texture2D) => (T)(object)textures[name],
            Type _ when assetType == typeof(Sound) => (T)(object)sounds[name],
            Type _ when assetType == typeof(Music) => (T)(object)musics[name],
            Type _ when assetType == typeof(Font) => (T)(object)fonts[name],
            _ => throw new Exception($"Type {assetType} not supported")
        };
    }

    public void AddTextureSet(string textureSetName, List<(int id, string name, string path)> texturesInfos)
    {
        TextureSet textureSet = new TextureSet();
        textureSet.texturesInfos = new Dictionary<int, string>();
        foreach (var (id, name, path) in texturesInfos)
        {
            textures[name] = Raylib.LoadTexture(path);
            textureSet.texturesInfos.Add(id, name);
        }
        textureSets.Add(textureSetName, textureSet);
    }

    public Texture2D GetTextureFromSet(string textureSetName, int id)
    {
       string textureName = textureSets[textureSetName].texturesInfos[id];
       return textures[textureName]; 
    }

    private struct TextureSet
    {
        public Dictionary<int, string> texturesInfos;
    }
}