using Raylib_cs;

public class Boulder : Entity
{
    bool isFalling = false;
    bool hasLanded = false;
    public bool isTouchingRockford { get; private set; }

    public Boulder(Coordinates position, Tilemap tilemap) : base(position, tilemap)
    {
        this.tilemap.SetTile(position, (int)IdTile.Boulder, true);
        shouldBeRemoved = false;
    }

    public override void Update()
    {
        var under = position + Coordinates.down;
        var idUnder = tilemap.GetTextureID(under.column, under.row);

        // si le boulder tombe et que rockford et dessous
        if (isFalling && idUnder == (int)IdTile.Rockford) isTouchingRockford = true;

        // si le boulder tombe et que c'est vide dessous
        if (isFalling && idUnder != (int)IdTile.Void)
        {
            tilemap.SetTile(position, (int)IdTile.Boulder, true);
            isFalling = false;
        }

        // Si le Boulder est en train de tomber et qu'il touche le sol ou un bloc
        if (isFalling && idUnder != (int)IdTile.Void && !hasLanded)
        {
            tilemap.SetTile(position, (int)IdTile.Boulder, true);
            // On marque qu'il arrête de chuter
            hasLanded = true;
            isFalling = false;
        }

        // si c'est vide dessous
        if (idUnder == (int)IdTile.Void)
        {
            tilemap.MoveID(position.column, position.row, under.column, under.row, (int)IdTile.BoulderFalling, true);
            isFalling = true;
            position = under;
        }
        // Sinon si on peut rouler sur l'entité du dessous
        else if (!isFalling && tilemap.IsRound(idUnder))
        {
            var left = position + Coordinates.left;
            var idLeft = tilemap.GetTextureIDFromCoord(left);
            var idLeftUnder = tilemap.GetTextureIDFromCoord(left + Coordinates.down);

            var right = position + Coordinates.right;
            var idRight = tilemap.GetTextureIDFromCoord(right);
            var idRightUnder = tilemap.GetTextureIDFromCoord(right + Coordinates.down);

            // On gére quand ça roule à gauche
            if (idLeft == (int)IdTile.Void && idLeftUnder == (int)IdTile.Void)
            {
                tilemap.MoveID(position.column, position.row, position.column - 1, position.row, (int)IdTile.Boulder, true);
                position = left;
            }
            // On gére quand ça roule à droite
            else if (idRight == (int)IdTile.Void && idRightUnder == (int)IdTile.Void)
            {
                tilemap.MoveID(position.column, position.row, position.column + 1, position.row, (int)IdTile.Boulder, true);
                position = right;
            }
        }
    }

    public override void Draw()
    {
        var posInWorld = tilemap.MapToWorld(position);
        Raylib.DrawTexture(assets.GetTextureFromSet("Entity", (int)IdTile.Boulder), (int)posInWorld.X, (int)posInWorld.Y, Color.White);
    }
}