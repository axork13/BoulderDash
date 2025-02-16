using System.Numerics;
using Raylib_cs;

public class Rockford : Entity
{
    // Réference à la gameScene
    private GameScene gameScene;
    public bool isDead = false;
    public bool isBoulderMoving = false;

    public Rockford(Coordinates position, Tilemap tilemap, GameScene gameScene) : base(position, tilemap)
    {
        this.tilemap.SetTile(position, (int)IdTile.Rockford, false);
        this.gameScene = gameScene;
        shouldBeRemoved = false;
    }

    public override void Update()
    {
        // On gère le déplacement
        if (Raylib.IsKeyDown(KeyboardKey.Left)) Move(Coordinates.left);
        if (Raylib.IsKeyDown(KeyboardKey.Up)) Move(Coordinates.up);
        if (Raylib.IsKeyDown(KeyboardKey.Right)) Move(Coordinates.right);
        if (Raylib.IsKeyDown(KeyboardKey.Down)) Move(Coordinates.down);
    }

    public void Move(Coordinates direction)
    {
        // Ne rien faire si aucune direction
        if (direction == Coordinates.zero) return;

        Coordinates targetPosition = position + direction;

        // Ne faire si aucune texture trouvée, permet de ne pas dépasser les bords de l'écran
        if (tilemap.GetTextureIDFromCoord(targetPosition) == -1) return;

        int currentId = tilemap.GetTextureID(position.column, position.row);
        int targetId = tilemap.GetTextureID(targetPosition.column, targetPosition.row);

        // Si la case cible est vide ou du dirt
        if (targetId == (int)IdTile.Void || targetId == (int)IdTile.Dirt)
        {
            tilemap.MoveID(position.column, position.row, targetPosition.column, targetPosition.row, currentId, false);
            Dirt dirt = GetDirtAt(targetPosition);
            if (dirt != null)
            {
                // On rend le dirt supprimable de la liste des entités
                Raylib.PlaySound(assets.Get<Sound>("Pick_dirt"));
                dirt.shouldBeRemoved = true;
            }
        }
        // Si la case cible est un diamand
        else if (targetId == (int)IdTile.Diamond)
        {
            tilemap.MoveID(position.column, position.row, targetPosition.column, targetPosition.row, currentId, false);
            Diamond diamond = GetDiamondAt(targetPosition);
            if (diamond != null)
            {
                // On rend le dirt supprimable de la liste des entités
                diamond.shouldBeRemoved = true;
            }
        }
        // Si la case cible est un boulder
        else if (targetId == (int)IdTile.Boulder && !isBoulderMoving)
        {
            // Si on va à gauche
            if (direction == Coordinates.left)
            {
                var boulderNewPosition = targetPosition + Coordinates.left;
                var leftBoulder = boulderNewPosition + Coordinates.left;
                var idLeftBoulder = tilemap.GetTextureIDFromCoord(leftBoulder);
                // Si c'est vide à gauche du boulder
                if (idLeftBoulder == (int)IdTile.Void)
                {
                    isBoulderMoving = true;
                    tilemap.MoveID(targetPosition.column, targetPosition.row, boulderNewPosition.column, boulderNewPosition.row, (int)IdTile.Boulder, true);
                    // On ajoute le nouveau boulder à la liste des entités
                    gameScene.entities.Add(new Boulder(boulderNewPosition, tilemap));
                    tilemap.MoveID(position.column, position.row, targetPosition.column, targetPosition.row, (int)IdTile.Rockford, false);
                    Boulder boulder = GetBoulderAt(targetPosition);
                    if (boulder != null)
                    {
                        // On supprime l'ancien boulder de la liste des entités
                        boulder.shouldBeRemoved = true;
                    }
                }
                else
                {
                    // Sinon on ne fait rien
                    return;
                }
            }
            // Si on va à droite
            else if (direction == Coordinates.right)
            {
                var boulderNewPosition = targetPosition + Coordinates.right;
                var rightBoulder = targetPosition + Coordinates.right;
                var idRightBoulder = tilemap.GetTextureIDFromCoord(rightBoulder);
                // Si c'est vide à droite du boulder
                if (idRightBoulder == (int)IdTile.Void)
                {
                    isBoulderMoving = true;
                    tilemap.MoveID(targetPosition.column, targetPosition.row, boulderNewPosition.column, boulderNewPosition.row, (int)IdTile.Boulder, true);
                    // On ajoute le nouveau boulder à la liste des entités
                    gameScene.entities.Add(new Boulder(boulderNewPosition, tilemap));
                    tilemap.MoveID(position.column, position.row, targetPosition.column, targetPosition.row, (int)IdTile.Rockford, false);

                    Boulder boulder = GetBoulderAt(targetPosition);
                    if (boulder != null)
                    {
                        // On supprime l'ancien boulder de la liste des entités
                        boulder.shouldBeRemoved = true;
                    }
                }
                else
                {
                    // Sinon on ne fait rien
                    return;
                }
            }
            else
            {
                // On ne fait rien si on est au dessus ou sous un boulder
                return;
            }
        }

        isBoulderMoving = false;
        // On met à jour la position de Rockford
        position = targetPosition;
    }

    public override void Draw()
    {
        var posInWorld = tilemap.MapToWorld(position);

        Raylib.DrawTexture(assets.GetTextureFromSet("Entity", (int)IdTile.Rockford), (int)posInWorld.X, (int)posInWorld.Y, Color.White);
    }

    #region Getters
    private Boulder GetBoulderAt(Coordinates position)
    {
        return gameScene.entities.OfType<Boulder>().FirstOrDefault(d => d.position.Equals(position));
    }

    private Diamond GetDiamondAt(Coordinates position)
    {
        return gameScene.entities.OfType<Diamond>().FirstOrDefault(d => d.position.Equals(position));
    }

    private Dirt GetDirtAt(Coordinates position)
    {
        return gameScene.entities.OfType<Dirt>().FirstOrDefault(d => d.position.Equals(position));
    }
    #endregion
}