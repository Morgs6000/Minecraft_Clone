namespace GameEngine.Mathematics;

/// <summary>
/// Representação de vetores e pontos em 4D.
/// </summary>
public struct Vector4Int
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

    /// <summary>
    /// Componente W do vetor.
    /// </summary>
    public int W;

    // --------------------------------------------------

    public static Vector4Int Zero => new (0, 0, 0, 0);

    public static Vector4Int Positive => new (1, 1, 1, 1);
    public static Vector4Int Negative => new (-1, -1, -1, -1);

    public static Vector4Int PositiveX => new (1, 0, 0, 0);
    public static Vector4Int NegativeX => new (-1, 0, 0, 0);

    public static Vector4Int PositiveY => new (0, 1, 0, 0);
    public static Vector4Int NegativeY => new (0, -1, 0, 0);

    public static Vector4Int PositiveZ => new (0, 0, 1, 0);
    public static Vector4Int NegativeZ => new (0, 0, -1, 0);

    public static Vector4Int PositiveW => new (0, 0, 0, 1);
    public static Vector4Int NegativeW => new (0, 0, 0, -1);

    // Construtor
    // --------------------------------------------------

    /// <summary>
    /// Cria um novo vetor com as componentes x, y, z e w fornecidas.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    public Vector4Int(int x, int y, int z = 0, int w = 0)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public Vector4Int(int value)
    {
        X = value;
        Y = value;
        Z = value;
        W = value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="w"></param>
    public Vector4Int(Vector3Int vector, int w = 0)
    {
        X = vector.X;
        Y = vector.Y;
        Z = vector.Z;
        W = w;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    public Vector4Int(Vector2Int vector, int z = 0, int w = 0)
    {
        X = vector.X;
        Y = vector.Y;
        Z = z;
        W = w;
    }

    // To String
    // --------------------------------------------------

    public override string ToString()
    {
        return $"{X}, {Y}, {Z}, {W}";
    }

    // Conversão Implicita
    // --------------------------------------------------

    public static implicit operator System.Numerics.Vector4(Vector4Int vector)
    {
        return new System.Numerics.Vector4(
            vector.X,
            vector.Y,
            vector.Z,
            vector.W
        );
    }

    public static implicit operator Vector4Int(System.Numerics.Vector4 vector)
    {
        return new Vector4Int(
            (int)vector.X,
            (int)vector.Y,
            (int)vector.Z,
            (int)vector.W
        );
    }

    // Multiplicação
    // --------------------------------------------------

    public static Vector4Int operator *(Vector4Int a, Vector4Int b)
    {
        return new Vector4Int(
            a.X * b.X,
            a.Y * b.Y,
            a.Z * b.Z,
            a.W * b.W
        );
    }

    public static Vector4Int operator *(Vector4Int a, int b)
    {
        return new Vector4Int(
            a.X * b,
            a.Y * b,
            a.Z * b,
            a.W * b
        );
    }

    public static Vector4Int operator *(int a, Vector4Int b)
    {
        return new Vector4Int(
            a * b.X,
            a * b.Y,
            a * b.Z,
            a * b.W
        );
    }

    // Divisão
    // --------------------------------------------------

    public static Vector4Int operator /(Vector4Int a, Vector4Int b)
    {
        return new Vector4Int(
            a.X / b.X,
            a.Y / b.Y,
            a.Z / b.Z,
            a.W / b.W
        );
    }

    public static Vector4Int operator /(Vector4Int a, int b)
    {
        return new Vector4Int(
            a.X / b,
            a.Y / b,
            a.Z / b,
            a.W / b
        );
    }

    public static Vector4Int operator /(int a, Vector4Int b)
    {
        return new Vector4Int(
            a / b.X,
            a / b.Y,
            a / b.Z,
            a / b.W
        );
    }

    // Adição
    // --------------------------------------------------

    public static Vector4Int operator +(Vector4Int a, Vector4Int b)
    {
        return new Vector4Int(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z,
            a.W + b.W
        );
    }

    public static Vector4Int operator +(Vector4Int a, int b)
    {
        return new Vector4Int(
            a.X + b,
            a.Y + b,
            a.Z + b,
            a.W + b
        );
    }

    public static Vector4Int operator +(int a, Vector4Int b)
    {
        return new Vector4Int(
            a + b.X,
            a + b.Y,
            a + b.Z,
            a + b.W
        );
    }

    // Subtração
    // --------------------------------------------------

    public static Vector4Int operator -(Vector4Int a, Vector4Int b)
    {
        return new Vector4Int(
            a.X - b.X,
            a.Y - b.Y,
            a.Z - b.Z,
            a.W - b.W
        );
    }

    public static Vector4Int operator -(Vector4Int a, int b)
    {
        return new Vector4Int(
            a.X - b,
            a.Y - b,
            a.Z - b,
            a.W - b
        );
    }

    public static Vector4Int operator -(int a, Vector4Int b)
    {
        return new Vector4Int(
            a - b.X,
            a - b.Y,
            a - b.Z,
            a - b.W
        );
    }
}
