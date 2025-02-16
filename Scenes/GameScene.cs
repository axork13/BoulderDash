using Raylib_cs;

public class GameScene : Scene
{
    enum GameState
    {
        Playing,
        Paused,
        GameOver,
        GameFinished
    }

    Label pauseLabel;
    Label gameOverLabel;
    Label gameFinishedLabel;
    Label infoDiamondsLabel;
    Label infoPickedUpLabel;
    Label infoGameOverLabel;

    GameState gameState = GameState.Playing;

    public static int columns = 40;
    public static int rows = 30;
    public static int tileSize = 16;

    public int diamonds = 0;
    public int totalDiamonds = 0;


    Tilemap tilemap = new Tilemap(columns, rows, tileSize);
    Timer gameOverTimer;
    Timer gamePlayTimer;
    Timer gameFinishedTimer;

    Rockford rockford;

    protected IAssetsManager assets = Services.Get<IAssetsManager>();


    public List<Entity> entities = new List<Entity>();


    public GameScene()
    {
        Raylib.PlaySound(assets.Get<Sound>("Start_level"));

        // Création des différents labels pour l'affichage de texte
        pauseLabel = new Label("--- Paused ---", new(0, 0, Game.width, Game.height));
        gameOverLabel = new Label("--- Game Over ---", new(0, 0, Game.width, Game.height));
        infoGameOverLabel = new Label("", new(0, 0, Game.width, Game.height + 50));
        gameFinishedLabel = new Label("--- Game Over ---", new(0, 0, Game.width, Game.height));

        // Création des différents timer utilisés dans le jeu
        gameOverTimer = AddTimer(GameOver, 4f, false);
        gameOverTimer.Stop();
        gameFinishedTimer = AddTimer(GameFinished, 2.5f, false);
        gameFinishedTimer.Stop();
        gamePlayTimer = AddTimer(GamePlayTriggered, 0.5f);

        InitGrid();

        infoPickedUpLabel = new Label($"--- You picked up {totalDiamonds} diamonds ---", new(0, 0, Game.width, Game.height + 50));
        gameFinishedLabel = new Label("--- Game Over ---", new(0, 0, Game.width, Game.height));
        infoDiamondsLabel = new Label("0", new(0, 0, Game.width, 30), 16);
    }

    private void UpdatePaused()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.P))
        {
            gameState = GameState.Playing;
            gamePlayTimer.Start();
        }
    }

    public void UpdateGrid()
    {
        // On scanne la grille de bas en haut 
        for (int row = tilemap.rows - 1; row >= 0; row--)
        {
            // On scan de gauche à droite
            for (int column = 0; column < tilemap.columns; column++)
            {
                // On récuperer la position courant et l'id de la tuile associé
                var position = new Coordinates(column, row);
                int tileId = tilemap.GetTextureIDFromCoord(position);

                // On applique la mise à jour uniquement aux Boulders
                if (tileId == (int)IdTile.Boulder || tileId == (int)IdTile.BoulderFalling)
                {
                    Boulder boulder = GetBoulderAt(position);
                    boulder?.Update(); // Mettre à jour le Boulder s'il existe
                    // On gère la mort de Rockford
                    if (boulder != null && boulder.isTouchingRockford)
                        rockford.isDead = true;
                }

                // Appliquer la mise à jour uniquement aux Diamonds
                if (tileId == (int)IdTile.Diamond || tileId == (int)IdTile.DiamondFalling)
                {
                    Diamond diamond = GetDiamondAt(position);
                    diamond?.Update(); // Mettre à jour le Diamond s'il existe
                    // On gère la mort de Rockford
                    if (diamond != null && diamond.isTouchingRockford)
                        rockford.isDead = true;
                }
            }
        }
    }

    private void UpdatePlaying()
    {
        // On parcours la liste des entitées à l'envers pour pouvoir supprimer des éléments sans bug
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            var entity = entities[i];

            // On gère la mise à jour si c'est Rockford
            if (entity.GetType() == typeof(Rockford))
            {
                entity.Update();
                rockford = (Rockford)entity;
            }

            // On supprime l'entité seulement si elle marquée comme devant être supprimée
            if (entity.shouldBeRemoved)
            {
                entities.RemoveAt(i);
                if (entity.GetType() == typeof(Diamond))
                {
                    // On décompte le diamants comme ramassé
                    Raylib.PlaySound(assets.Get<Sound>("Pick_diamond"));
                    diamonds--;
                }
            }
        }

        UpdateGrid();

        // On gère la pause
        if (Raylib.IsKeyPressed(KeyboardKey.P))
        {
            gameState = GameState.Paused;
            gamePlayTimer.Stop();
        }

        infoDiamondsLabel.text = $"Nombre de diamants restants : {diamonds}";
    }

    public override void Update()
    {
        base.Update();
        switch (gameState)
        {
            case GameState.Playing:
                UpdatePlaying();
                break;
            case GameState.Paused:
                UpdatePaused();
                break;
        }
        // Juste pour permette de montrer une fin de partie rapidement à la soutenance
        if (Raylib.IsKeyPressed(KeyboardKey.R)) diamonds = 0;
    }
    public override void Draw()
    {
        switch (gameState)
        {
            case GameState.Playing:
                // On dessine toutes les entités du jeu
                foreach (var entity in entities) entity.Draw();
                infoDiamondsLabel.Draw();
                break;
            case GameState.Paused:
                pauseLabel.Draw();
                break;
            case GameState.GameOver:
                gameOverLabel.Draw();
                infoGameOverLabel.text = $"{totalDiamonds - diamonds} diamants collectés sur {totalDiamonds}";
                infoGameOverLabel.Draw();
                break;
            case GameState.GameFinished:
                gameFinishedLabel.Draw();
                infoPickedUpLabel.Draw();
                break;
        }
    }

    private void InitGrid()
    {
        // On crée Rockford
        entities.Add(new Rockford(new Coordinates(2, 2), tilemap, this));
        
        // On initialise la grille avec des données aléatoires
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                var position = new Coordinates(column, row);
                // Si on est sur la position de Rockford on passe à la colonne suivante
                if (position == new Coordinates(2, 2)) break;

                // On génère une valeur entre 0 et 100 et on crée des entités en fonction de valeur
                int rand = Raylib.GetRandomValue(0, 100);
                switch (rand)
                {
                    case < 5:
                        entities.Add(new Diamond(position, tilemap));
                        diamonds++;
                        break;
                    case < 25:
                        tilemap.SetTile(new Coordinates(column, row), 0, false);
                        break;

                    case < 80:
                        entities.Add(new Dirt(position, tilemap));
                        break;
                    default:
                        entities.Add(new Boulder(position, tilemap));
                        break;
                }
            }
        }
        totalDiamonds = diamonds;
    }

    #region Getters pour chaque Entity
    private Diamond GetDiamondAt(Coordinates position)
    {
        return entities.OfType<Diamond>().FirstOrDefault(b => b.position.Equals(position));
    }
    private Boulder GetBoulderAt(Coordinates position)
    {
        return entities.OfType<Boulder>().FirstOrDefault(b => b.position.Equals(position));
    }
    #endregion


    #region Callback liées aux timers
    public void GameOver()
    {
        Services.Get<IScenesManager>().Load<MenuScene>(null);
    }

    public void GameFinished()
    {
        Services.Get<IScenesManager>().Load<MenuScene>(null);
    }

    public void GamePlayTriggered()
    {
        if (rockford.isDead)
        {
            Raylib.PlaySound(assets.Get<Sound>("Rockford_dead"));

            Raylib.PlaySound(assets.Get<Sound>("Game_over"));

            gameState = GameState.GameOver;

            gameOverTimer.Start();
            gamePlayTimer.Stop();

        }

        if (diamonds == 0)
        {
            gameState = GameState.GameFinished;
            gameFinishedTimer.Start();
            gamePlayTimer.Stop();
        }
    }
    #endregion
}