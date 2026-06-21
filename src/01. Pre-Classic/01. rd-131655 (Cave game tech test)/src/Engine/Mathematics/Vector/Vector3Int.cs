namespace GameEngine.Mathematics;

/// <summary>
/// Representação de vetores e pontos em 4D.
/// </summary>
public struct Vector3Int
{
    /// <summary>
    /// Componente X do vetor.
    /// </summary>
    public int X;

    /// <summary>
    /// Componente Y do vetor.
    /// </summary>
    public int Y;

    /// <summary>
    /// Componente Z do vetor.
    /// </summary>
    public int Z;

    // --------------------------------------------------

    public static Vector3Int Zero => new (0, 0, 0);

    public static Vector3Int Positive => new (1, 1, 1);
    public static Vector3Int Negative => new (-1, -1, -1);

    public static Vector3Int PositiveX => new (1, 0, 0);
    public static Vector3Int NegativeX => new (-1, 0, 0);

    public static Vector3Int PositiveY => new (0, 1, 0);
    public static Vector3Int NegativeY => new (0, -1, 0);

    public static Vector3Int PositiveZ => new (0, 0, 1);
    public static Vector3Int NegativeZ => new (0, 0, -1);

    // Construtor
    // --------------------------------------------------

    /// <summary>
    /// Cria um novo vetor com as componentes x, y e z fornecidas.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Vector3Int(int x, int y, int z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public Vector3Int(int value)
    {
        X = value;
        Y = value;
        Z = value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="z"></param>
    public Vector3Int(Vector2Int vector, int z = 0)
    {
        X = vector.X;
        Y = vector.Y;
        Z = z;
    }

    // To String
    // --------------------------------------------------

    public override string ToString()
    {
        return $"{X}, {Y}, {Z}";
    }

    // Conversão Implicita
    // --------------------------------------------------

    public static implicit operator System.Numerics.Vector3(Vector3Int vector)
    {
        return new System.Numerics.Vector3(
            vector.X,
            vector.Y,
            vector.Z
        );
    }

    public static implicit operator Vector3Int(System.Numerics.Vector3 vector)
    {
        return new Vector3Int(
            (int)vector.X,
            (int)vector.Y,
            (int)vector.Z
        );
    }

    // Multiplicação
    // --------------------------------------------------

    public static Vector3Int operator *(Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(
            a.X * b.X,
            a.Y * b.Y,
            a.Z * b.Z
        );
    }

    public static Vector3Int operator *(Vector3Int a, int b)
    {
        return new Vector3Int(
            a.X * b,
            a.Y * b,
            a.Z * b
        );
    }

    public static Vector3Int operator *(int a, Vector3Int b)
    {
        return new Vector3Int(
            a * b.X,
            a * b.Y,
            a * b.Z
        );
    }

    // Divisão
    // --------------------------------------------------

    public static Vector3Int operator /(Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(
            a.X / b.X,
            a.Y / b.Y,
            a.Z / b.Z
        );
    }

    public static Vector3Int operator /(Vector3Int a, int b)
    {
        return new Vector3Int(
            a.X / b,
            a.Y / b,
            a.Z / b
        );
    }

    public static Vector3Int operator /(int a, Vector3Int b)
    {
        return new Vector3Int(
            a / b.X,
            a / b.Y,
            a / b.Z
        );
    }

    // Adição
    // --------------------------------------------------

    public static Vector3Int operator +(Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z
        );
    }

    public static Vector3Int operator +(Vector3Int a, int b)
    {
        return new Vector3Int(
            a.X + b,
            a.Y + b,
            a.Z + b
        );
    }

    public static Vector3Int operator +(int a, Vector3Int b)
    {
        return new Vector3Int(
            a + b.X,
            a + b.Y,
            a + b.Z
        );
    }

    // Subtração
    // --------------------------------------------------

    public static Vector3Int operator -(Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(
            a.X - b.X,
            a.Y - b.Y,
            a.Z - b.Z
        );
    }

    public static Vector3Int operator -(Vector3Int a, int b)
    {
        return new Vector3Int(
            a.X - b,
            a.Y - b,
            a.Z - b
        );
    }

    public static Vector3Int operator -(int a, Vector3Int b)
    {
        return new Vector3Int(
            a - b.X,
            a - b.Y,
            a - b.Z
        );
    }
}
