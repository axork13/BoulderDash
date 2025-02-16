using Raylib_cs;

public class Diamond : Entity
{
    bool isFalling = false;
    bool hasLanded = false;

    public bool isTouchingRockford { get; private set; }

    public Diamond(Coordinates position, Tilemap tilemap) : base(position, tilemap)
    {
        this.tilemap.SetTile(position, (int)IdTile.Diamond, true);
        shouldBeRemoved = false;
    }

    public override void Update()
    {
        var under = position + Coordinates.down;
        var idUnder = tilemap.GetTextureID(under.column, under.row);

        // Vérifie si le diamond tombe sur Rockford avant qu'il ne touche le sol
        if (isFalling && idUnder == (int)IdTile.Rockford) isTouchingRockford = true;

        //  si le diamant tombe et que c'est vide dessous
        if (isFalling && idUnder != (int)IdTile.Void)
        {
            tilemap.SetTile(position, (int)IdTile.Diamond, true);
            isFalling = false;
        }

        // Si le diamant est en train de tomber et qu'il touche le sol ou un bloc
        if (isFalling && idUnder != (int)IdTile.Void && !hasLanded)
        {
            tilemap.SetTile(position, (int)IdTile.Diamond, true);
            // On marque qu'il arrête de chuter
            hasLanded = true;
            isFalling = false;
        }

        // Si c'est vide en dessous, le Diamond tombe
        if (idUnder == (int)IdTile.Void)
        {
            tilemap.MoveID(position.column, position.row, under.column, under.row, (int)IdTile.DiamondFalling, true);
            isFalling = true;
            position = under;
        }
        // Vérification pour rouler sur les côtés (comme les boulders)
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
                tilemap.MoveID(position.column, position.row, left.column, left.row, (int)IdTile.Diamond, true);
                position = left;
            }
            // On gére quand ça roule à droite
            else if (idRight == (int)IdTile.Void && idRightUnder == (int)IdTile.Void)
            {
                tilemap.MoveID(position.column, position.row, right.column, right.row, (int)IdTile.Diamond, true);
                position = right;
            }
        }
    }

    public override void Draw()
    {
        var posInWorld = tilemap.MapToWorld(position);
        Raylib.DrawTexture(assets.GetTextureFromSet("Entity", (int)IdTile.Diamond), (int)posInWorld.X, (int)posInWorld.Y, Color.White);
    }
}