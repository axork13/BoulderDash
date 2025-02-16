using System.Numerics;

public struct Coordinates
{
    public readonly int column;
    public readonly int row;

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinates"/> struct.
    /// </summary>
    /// <param name="column">The column value (X-axis).</param>
    /// <param name="row">The row value (Y-axis).</param>
    public Coordinates(int column, int row)
    {
        this.column = column;
        this.row = row;
    }

    public static Coordinates zero => new Coordinates(0, 0);
    public static Coordinates left => new Coordinates(-1, 0);
    public static Coordinates right => new Coordinates(1, 0);
    public static Coordinates up => new Coordinates(0, -1);
    public static Coordinates down => new Coordinates(0, 1);

    public static Coordinates[] mooreNeighborhood = {
        new Coordinates(-1, -1),
        new Coordinates(0, -1),
        new Coordinates(1, -1),
        new Coordinates(-1, 0),
        new Coordinates(1, 0),
        new Coordinates(-1, 1),
        new Coordinates(0, 1),
        new Coordinates(1, 1),
    };

    public static Coordinates[] vonNeumannNeighborhood = {
        new Coordinates(0, -1),
        new Coordinates(-1, 0),
        new Coordinates(1, 0),
        new Coordinates(0, 1),
    };

    /// <summary>
    /// Converts a <see cref="Vector2"/> to a <see cref="Coordinates"/>.
    /// </summary>
    /// <param name="vector">The Vector2 to convert.</param>
    /// <returns>A new instance of <see cref="Coordinates"/>.</returns>
    public static Coordinates FromVector2(Vector2 vector) => new Coordinates((int)Math.Round(vector.X), (int)Math.Round(vector.Y));


    /// <summary>
    /// Converts the coordinates to a <see cref="Vector2"/>.
    /// </summary>
    public Vector2 ToVector2 => new Vector2(column, row);

    /// <summary>
    /// Multiplies the coordinates by a scalar.
    /// </summary>
    public static Coordinates operator *(Coordinates coord, int scalar)
        => new Coordinates(coord.column * scalar, coord.row * scalar);

    /// <summary>
    /// Multiplies the coordinates by a scalar (commutative).
    /// </summary>
    public static Coordinates operator *(int scalar, Coordinates coord)
        => new Coordinates(coord.column * scalar, coord.row * scalar);

    /// <summary>
    /// Adds two coordinates together.
    /// </summary>
    public static Coordinates operator +(Coordinates a, Coordinates b)
        => new Coordinates(a.column + b.column, a.row + b.row);

    /// <summary>
    /// Subtracts one coordinate from another.
    /// </summary>
    public static Coordinates operator -(Coordinates a, Coordinates b)
        => new Coordinates(a.column - b.column, a.row - b.row);

    /// <summary>
    /// Overloads the unary '-' operator to invert the coordinates.
    /// </summary>
    /// <param name="coord">The coordinates to invert.</param>
    /// <returns>A new instance of <see cref="Coordinates"/> with inverted values.</returns>
    public static Coordinates operator -(Coordinates coord)
        => new Coordinates(-coord.column, -coord.row);

    /// <summary>
    /// Calculates the Euclidean distance between two coordinates.
    /// </summary>
    public static double Distance(Coordinates a, Coordinates b)
    {
        int deltaX = a.column - b.column;
        int deltaY = a.row - b.row;
        return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    /// <summary>
    /// Checks if two coordinates are equal.
    /// </summary>
    public override bool Equals(object obj)
        => obj is Coordinates other && column == other.column && row == other.row;

    /// <summary>
    /// Compares two coordinates for equality.
    /// </summary>
    public static bool operator ==(Coordinates a, Coordinates b)
        => a.Equals(b);

    /// <summary>
    /// Compares two coordinates for inequality.
    /// </summary>
    public static bool operator !=(Coordinates a, Coordinates b)
        => !a.Equals(b);

    /// <summary>
    /// Creates a random coordinate within the given limits.
    /// </summary>
    /// <param name="maxColumn">The maximum value for the column (exclusive).</param>
    /// <param name="maxRow">The maximum value for the row (exclusive).</param>
    /// <returns>A random instance of <see cref="Coordinates"/>.</returns>
    public static Coordinates Random(int maxColumn, int maxRow)
    {
        Random random = new Random();
        int column = random.Next(0, maxColumn);
        int row = random.Next(0, maxRow);
        return new Coordinates(column, row);
    }

    public override int GetHashCode() => HashCode.Combine(column, row);
    public override string ToString() => $"({column}, {row})";
}