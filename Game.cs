using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

public class Game
{
    static ScenesManager scenesManager = new();
    static AssetsManager assetsManager = new();

    public static readonly int width = 640;
    public static readonly int height = 480;
    public static float scale => Math.Min(GetScreenWidth() / (float)width, GetScreenHeight() / (float)height);
    static float offSetX = 1f;
    static float offSetY = 1f;
    static void LoadAssets()
    {
        assetsManager.AddTextureSet("Entity", new List<(int id, string name, string path)>
        {
            (1, "Dirt", "Assets/Images/Dirt.png"),
            (2, "Boulder", "Assets/Images/Boulder.png"),
            (3, "BoulderFalling", "Assets/Images/Boulder.png"),
            (4, "Rockford", "Assets/Images/Rockford.png"),
            (5, "Diamond", "Assets/Images/Diamond.png"),
            (6, "DiamondFalling", "Assets/Images/Diamond.png"),
        });

        assetsManager.AddTextureSet("GUI", new List<(int id, string name, string path)>
        {
            (1, "Menu_background", "Assets/Images/Menu_background.png"),
        });

        assetsManager.Load<Music>("Menu", "Assets/Musics/menu.wav");
        assetsManager.Load<Sound>("Pick_dirt", "Assets/Sounds/Pick_dirt.wav");
        assetsManager.Load<Sound>("Pick_diamond", "Assets/Sounds/Pick_diamond.wav");
        assetsManager.Load<Sound>("Boulder", "Assets/Sounds/Boulder.wav");
        assetsManager.Load<Sound>("Game_over", "Assets/Sounds/Game_over.wav");
        assetsManager.Load<Sound>("Rockford_dead", "Assets/Sounds/Rockford_dead.wav");
        assetsManager.Load<Sound>("Start_level", "Assets/Sounds/Start_level.wav");

    }
    static void Main()
    {
        SetConfigFlags(ConfigFlags.ResizableWindow | ConfigFlags.VSyncHint);
        InitWindow(1080, 720, "Boulder Dash");
        SetTargetFPS(10);
        InitAudioDevice();
        // Pour éviter un effet de grésillement sur la musique du menu
        SetAudioStreamBufferSizeDefault(4096);

        LoadAssets();
        scenesManager.Load<MenuScene>(null);

        RenderTexture2D canvas = LoadRenderTexture(width, height);
        SetTextureFilter(canvas.Texture, TextureFilter.Point);

        while (!WindowShouldClose())
        {
            float scaleW = width * scale;
            float scaleH = height * scale;
            offSetX = (GetScreenWidth() - scaleW) * .5f;
            offSetY = (GetScreenHeight() - scaleH) * .5f;

            BeginTextureMode(canvas);
            ClearBackground(Color.Black);
            scenesManager.Update();
            scenesManager.Draw();
            EndTextureMode();

            BeginDrawing();
            ClearBackground(Color.SkyBlue);
            DrawTexturePro(canvas.Texture, new Rectangle(0, 0, width, -height), new Rectangle(offSetX, offSetY, scaleW, scaleH), Vector2.Zero, 0, Color.White);
            EndDrawing();
        }

        CloseAudioDevice();
        CloseWindow();
    }
}
