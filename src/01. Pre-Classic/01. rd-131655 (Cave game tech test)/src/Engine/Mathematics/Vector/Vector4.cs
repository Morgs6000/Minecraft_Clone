namespace GameEngine.Mathematics;

/// <summary>
/// Representação de vetores e pontos em 4D.
/// </summary>
public struct Vector4
{
    /// <summary>
    /// Componente X do vetor.
    /// </summary>
    public float X;

    /// <summary>
    /// Componente Y do vetor.
    /// </summary>
    public float Y;

    /// <summary>
    /// Componente Z do vetor.
    /// </summary>
    public float Z;

    /// <summary>
    /// Componente W do vetor.
    /// </summary>
    public float W;

    // --------------------------------------------------

    public static Vector4 Zero => new (0.0f, 0.0f, 0.0f, 0.0f);

    public static Vector4 Positive => new (1.0f, 1.0f, 1.0f, 1.0f);
    public static Vector4 Negative => new (-1.0f, -1.0f, -1.0f, -1.0f);

    public static Vector4 PositiveX => new (1.0f, 0.0f, 0.0f, 0.0f);
    public static Vector4 NegativeX => new (-1.0f, 0.0f, 0.0f, 0.0f);

    public static Vector4 PositiveY => new (0.0f, 1.0f, 0.0f, 0.0f);
    public static Vector4 NegativeY => new (0.0f, -1.0f, 0.0f, 0.0f);

    public static Vector4 PositiveZ => new (0.0f, 0.0f, 1.0f, 0.0f);
    public static Vector4 NegativeZ => new (0.0f, 0.0f, -1.0f, 0.0f);

    public static Vector4 PositiveW => new (0.0f, 0.0f, 0.0f, 1.0f);
    public static Vector4 NegativeW => new (0.0f, 0.0f, 0.0f, -1.0f);

    // Normalize
    // --------------------------------------------------

    public void Normalize()
    {
        this = System.Numerics.Vector4.Normalize(this);
    }

    public static Vector4 Normalize(Vector4 value)
    {
        return System.Numerics.Vector4.Normalize(value);
    }

    // Construtor
    // --------------------------------------------------

    /// <summary>
    /// Cria um novo vetor com as componentes x, y, z e w fornecidas.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    public Vector4(float x, float y, float z = 0.0f, float w = 0.0f)
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
    public Vector4(float value)
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
    public Vector4(Vector3 vector, float w = 0.0f)
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
    public Vector4(Vector2 vector, float z = 0.0f, float w = 0.0f)
    {
        X = vector.X;
        Y = vector.Y;
        Z = z;
        W = w;
    }

    // Conversão Implicita
    // --------------------------------------------------

    public static implicit operator System.Numerics.Vector4(Vector4 vector)
    {
        return new System.Numerics.Vector4(
            vector.X,
            vector.Y,
            vector.Z,
            vector.W
        );
    }

    public static implicit operator Vector4(System.Numerics.Vector4 vector)
    {
        return new Vector4(
            vector.X,
            vector.Y,
            vector.Z,
            vector.W
        );
    }

    // Multiplicação
    // --------------------------------------------------

    public static Vector4 operator *(Vector4 a, Vector4 b)
    {
        return new Vector4(
            a.X * b.X,
            a.Y * b.Y,
            a.Z * b.Z,
            a.W * b.W
        );
    }

    public static Vector4 operator *(Vector4 a, float b)
    {
        return new Vector4(
            a.X * b,
            a.Y * b,
            a.Z * b,
            a.W * b
        );
    }

    public static Vector4 operator *(float a, Vector4 b)
    {
        return new Vector4(
            a * b.X,
            a * b.Y,
            a * b.Z,
            a * b.W
        );
    }

    // Divisão
    // --------------------------------------------------

    public static Vector4 operator /(Vector4 a, Vector4 b)
    {
        return new Vector4(
            a.X / b.X,
            a.Y / b.Y,
            a.Z / b.Z,
            a.W / b.W
        );
    }

    public static Vector4 operator /(Vector4 a, float b)
    {
        return new Vector4(
            a.X / b,
            a.Y / b,
            a.Z / b,
            a.W / b
        );
    }

    public static Vector4 operator /(float a, Vector4 b)
    {
        return new Vector4(
            a / b.X,
            a / b.Y,
            a / b.Z,
            a / b.W
        );
    }

    // Adição
    // --------------------------------------------------

    public static Vector4 operator +(Vector4 a, Vector4 b)
    {
        return new Vector4(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z,
            a.W + b.W
        );
    }

    public static Vector4 operator +(Vector4 a, float b)
    {
        return new Vector4(
            a.X + b,
            a.Y + b,
            a.Z + b,
            a.W + b
        );
    }

    public static Vector4 operator +(float a, Vector4 b)
    {
        return new Vector4(
            a + b.X,
            a + b.Y,
            a + b.Z,
            a + b.W
        );
    }

    // Subtração
    // --------------------------------------------------

    public static Vector4 operator -(Vector4 a, Vector4 b)
    {
        return new Vector4(
            a.X - b.X,
            a.Y - b.Y,
            a.Z - b.Z,
            a.W - b.W
        );
    }

    public static Vector4 operator -(Vector4 a, float b)
    {
        return new Vector4(
            a.X - b,
            a.Y - b,
            a.Z - b,
            a.W - b
        );
    }

    public static Vector4 operator -(float a, Vector4 b)
    {
        return new Vector4(
            a - b.X,
            a - b.Y,
            a - b.Z,
            a - b.W
        );
    }
}
