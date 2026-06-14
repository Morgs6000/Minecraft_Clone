namespace GameEngine.Mathematics;

/// <summary>
/// Representação de vetores e pontos em 4D.
/// </summary>
public struct Vector2Int
{
    /// <summary>
    /// Componente X do vetor.
    /// </summary>
    public int X;

    /// <summary>
    /// Componente Y do vetor.
    /// </summary>
    public int Y;

    // --------------------------------------------------

    public static Vector2Int Zero => new (0, 0);

    public static Vector2Int Positive => new (1, 1);
    public static Vector2Int Negative => new (-1, -1);

    public static Vector2Int PositiveX => new (1, 0);
    public static Vector2Int NegativeX => new (-1, 0);

    public static Vector2Int PositiveY => new (0, 1);
    public static Vector2Int NegativeY => new (0, -1);

    // Construtor
    // --------------------------------------------------

    /// <summary>
    /// Cria um novo vetor com as componentes x e y fornecidas.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public Vector2Int(int value)
    {
        X = value;
        Y = value;
    }

    // Conversão Implicita
    // --------------------------------------------------

    public static implicit operator System.Numerics.Vector2(Vector2Int vector)
    {
        return new System.Numerics.Vector2(
            vector.X,
            vector.Y
        );
    }

    public static implicit operator Vector2Int(System.Numerics.Vector2 vector)
    {
        return new Vector2Int(
            (int)vector.X,
            (int)vector.Y
        );
    }

    // Multiplicação
    // --------------------------------------------------

    public static Vector2Int operator *(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(
            a.X * b.X,
            a.Y * b.Y
        );
    }

    public static Vector2Int operator *(Vector2Int a, int b)
    {
        return new Vector2Int(
            a.X * b,
            a.Y * b
        );
    }

    public static Vector2Int operator *(int a, Vector2Int b)
    {
        return new Vector2Int(
            a * b.X,
            a * b.Y
        );
    }

    // Divisão
    // --------------------------------------------------

    public static Vector2Int operator /(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(
            a.X / b.X,
            a.Y / b.Y
        );
    }

    public static Vector2Int operator /(Vector2Int a, int b)
    {
        return new Vector2Int(
            a.X / b,
            a.Y / b
        );
    }

    public static Vector2Int operator /(int a, Vector2Int b)
    {
        return new Vector2Int(
            a / b.X,
            a / b.Y
        );
    }

    // Adição
    // --------------------------------------------------

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(
            a.X + b.X,
            a.Y + b.Y
        );
    }

    public static Vector2Int operator +(Vector2Int a, int b)
    {
        return new Vector2Int(
            a.X + b,
            a.Y + b
        );
    }

    public static Vector2Int operator +(int a, Vector2Int b)
    {
        return new Vector2Int(
            a + b.X,
            a + b.Y
        );
    }

    // Subtração
    // --------------------------------------------------

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(
            a.X - b.X,
            a.Y - b.Y
        );
    }

    public static Vector2Int operator -(Vector2Int a, int b)
    {
        return new Vector2Int(
            a.X - b,
            a.Y - b
        );
    }

    public static Vector2Int operator -(int a, Vector2Int b)
    {
        return new Vector2Int(
            a - b.X,
            a - b.Y
        );
    }
}
