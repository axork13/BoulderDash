using System.Numerics;
using Raylib_cs;

public enum IdTile
    {
        Void,
        Dirt,
        Boulder,
        BoulderFalling,
        Rockford,
        Diamond,
        DiamondFalling
    }

public class Tilemap
{
    public bool debug = true;

    public Vector2 position = Vector2.Zero;
    public int tileSize { get; private set; }
    public int columns { get; private set; }
    public int rows { get; private set; }
    public Tile[,] tiles;

    public Tilemap(int columns = 40, int rows = 30, int tileSize = 16)
    {
        this.columns = columns;
        this.rows = rows;
        this.tileSize = tileSize;
        tiles = new Tile[columns, rows];
    }

    public void SetTile(Coordinates coordinates, int textureId, bool isRound)
    {
        
        tiles[coordinates.column, coordinates.row] = new Tile
        {
            textureId = textureId,
            isRound = isRound
        };
    }

    #region Coordinates conversion      
    public Coordinates WorldToMap(Vector2 pos)
    {
        pos -= position;
        pos /= tileSize;
        return new Coordinates((int)pos.X, (int)pos.Y);
    }

    public Vector2 MapToWorld(Coordinates coordinates)
    {
        coordinates *= tileSize;

        return coordinates.ToVector2 + position;
    }
    #endregion

    public struct Tile
    {
        public int textureId;
        public bool isRound;
    }

    public bool IsRound(int pId)
    {
        return pId == (int)IdTile.Boulder || pId == (int)IdTile.BoulderFalling || pId == (int)IdTile.Diamond || pId == (int)IdTile.DiamondFalling;
    }

    public int GetTextureID(int column, int row)
    {
        if (row < rows && row >= 0 && column < columns && column >= 0)
            return tiles[column, row].textureId;
        return -1;
    }

    public int GetTextureIDFromCoord(Coordinates coordinates)
    {
        return GetTextureID(coordinates.column, coordinates.row);
    }

    public void MoveID(int fromColumn, int fromRow, int toColumn, int toRow, int id, bool isRound)
    {
        SetTile(new Coordinates(toColumn, toRow), id, isRound);
        SetTile(new Coordinates(fromColumn, fromRow), (int)IdTile.Void, false);
    }
}