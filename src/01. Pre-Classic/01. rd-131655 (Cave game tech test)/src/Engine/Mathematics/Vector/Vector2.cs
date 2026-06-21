namespace GameEngine.Mathematics;

/// <summary>
/// Representação de vetores e pontos em 4D.
/// </summary>
public struct Vector2
{
    /// <summary>
    /// Componente X do vetor.
    /// </summary>
    public float X;

    /// <summary>
    /// Componente Y do vetor.
    /// </summary>
    public float Y;

    // --------------------------------------------------

    public static Vector2 Zero => new (0.0f, 0.0f);

    public static Vector2 Positive => new (1.0f, 1.0f);
    public static Vector2 Negative => new (-1.0f, -1.0f);

    public static Vector2 PositiveX => new (1.0f, 0.0f);
    public static Vector2 NegativeX => new (-1.0f, 0.0f);

    public static Vector2 PositiveY => new (0.0f, 1.0f);
    public static Vector2 NegativeY => new (0.0f, -1.0f);

    // Normalize
    // --------------------------------------------------

    public void Normalize()
    {
        this = System.Numerics.Vector2.Normalize(this);
    }

    public static Vector2 Normalize(Vector2 value)
    {
        return System.Numerics.Vector2.Normalize(value);
    }

    // Construtor
    // --------------------------------------------------

    /// <summary>
    /// Cria um novo vetor com as componentes x e y fornecidas.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public Vector2(float value)
    {
        X = value;
        Y = value;
    }

    // To String
    // --------------------------------------------------

    public override string ToString()
    {
        return $"{X}, {Y}";
    }

    // Conversão Implicita
    // --------------------------------------------------

    public static implicit operator System.Numerics.Vector2(Vector2 vector)
    {
        return new System.Numerics.Vector2(
            vector.X,
            vector.Y
        );
    }

    public static implicit operator Vector2(System.Numerics.Vector2 vector)
    {
        return new Vector2(
            vector.X,
            vector.Y
        );
    }

    // Multiplicação
    // --------------------------------------------------

    public static Vector2 operator *(Vector2 a, Vector2 b)
    {
        return new Vector2(
            a.X * b.X,
            a.Y * b.Y
        );
    }

    public static Vector2 operator *(Vector2 a, float b)
    {
        return new Vector2(
            a.X * b,
            a.Y * b
        );
    }

    public static Vector2 operator *(float a, Vector2 b)
    {
        return new Vector2(
            a * b.X,
            a * b.Y
        );
    }

    // Divisão
    // --------------------------------------------------

    public static Vector2 operator /(Vector2 a, Vector2 b)
    {
        return new Vector2(
            a.X / b.X,
            a.Y / b.Y
        );
    }

    public static Vector2 operator /(Vector2 a, float b)
    {
        return new Vector2(
            a.X / b,
            a.Y / b
        );
    }

    public static Vector2 operator /(float a, Vector2 b)
    {
        return new Vector2(
            a / b.X,
            a / b.Y
        );
    }

    // Adição
    // --------------------------------------------------

    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(
            a.X + b.X,
            a.Y + b.Y
        );
    }

    public static Vector2 operator +(Vector2 a, float b)
    {
        return new Vector2(
            a.X + b,
            a.Y + b
        );
    }

    public static Vector2 operator +(float a, Vector2 b)
    {
        return new Vector2(
            a + b.X,
            a + b.Y
        );
    }

    // Subtração
    // --------------------------------------------------

    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new Vector2(
            a.X - b.X,
            a.Y - b.Y
        );
    }

    public static Vector2 operator -(Vector2 a, float b)
    {
        return new Vector2(
            a.X - b,
            a.Y - b
        );
    }

    public static Vector2 operator -(float a, Vector2 b)
    {
        return new Vector2(
            a - b.X,
            a - b.Y
        );
    }
}
