using Raylib_cs;

public class Dirt : Entity
{
    public Dirt(Coordinates position, Tilemap tilemap) : base(position, tilemap)
    {
        this.tilemap.SetTile(position, (int)IdTile.Dirt, false);
        shouldBeRemoved = false;
    }

    public override void Draw()
    {
        var posInWorld = tilemap.MapToWorld(position);
        Raylib.DrawTexture(assets.GetTextureFromSet("Entity", (int)IdTile.Dirt), (int)posInWorld.X, (int)posInWorld.Y, Color.White);
    }
}