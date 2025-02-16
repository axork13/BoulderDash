public abstract class Entity
{
    protected readonly IAssetsManager assets = Services.Get<IAssetsManager>();

    public bool shouldBeRemoved { get; set; }
    
    public Coordinates position { get; protected set; }
    protected Tilemap tilemap;

    public Entity(Coordinates position, Tilemap tilemap) 
    {
        this.position = position;
        this.tilemap = tilemap;
    }

    public virtual void Update() {}
    public abstract void Draw();
}