using System.Numerics;
using Raylib_cs;

public class MenuScene : Scene
{
    private Timer blinkTimer;
    private bool isTextShowed = true;

    private Label menuLabel;

    IAssetsManager assets = Services.Get<IAssetsManager>();

    Music menuMusic;
    

    public MenuScene()
    {
        // Timer pour faire clignoter le texte du menu
        blinkTimer = AddTimer(() => isTextShowed = !isTextShowed, .5f, true);
        menuLabel = new Label("Press space to play", new(0, 0, Game.width, Game.height + 50));

        // On gère la musique du menu
        menuMusic = assets.Get<Music>("Menu");
        Raylib.PlayMusicStream(menuMusic);
        Raylib.SetMusicVolume(menuMusic, .4f);
    }

    public override void Update()
    {
        base.Update();

        Raylib.UpdateMusicStream(menuMusic);

        if(Raylib.IsKeyDown(KeyboardKey.Space))
            StartGame();
    }

    public override void Draw()
    {
        Raylib.DrawTexture(assets.GetTextureFromSet("GUI", 1), Game.width/2-160, 40, Color.White);

        if (isTextShowed)
            menuLabel.Draw();
    }

    private void StartGame()
    {        
        blinkTimer.Stop();
        Services.Get<IScenesManager>().Load<GameScene>(null);
    }
}