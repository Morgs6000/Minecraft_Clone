namespace GameEngine.Mathematics;

/// <summary>
/// Representação de vetores e pontos em 4D.
/// </summary>
public struct Vector3
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

    // --------------------------------------------------

    public static Vector3 Zero => new (0.0f, 0.0f, 0.0f);

    public static Vector3 Positive => new (1.0f, 1.0f, 1.0f);
    public static Vector3 Negative => new (-1.0f, -1.0f, -1.0f);

    public static Vector3 PositiveX => new (1.0f, 0.0f, 0.0f);
    public static Vector3 NegativeX => new (-1.0f, 0.0f, 0.0f);

    public static Vector3 PositiveY => new (0.0f, 1.0f, 0.0f);
    public static Vector3 NegativeY => new (0.0f, -1.0f, 0.0f);

    public static Vector3 PositiveZ => new (0.0f, 0.0f, 1.0f);
    public static Vector3 NegativeZ => new (0.0f, 0.0f, -1.0f);

    // Normalize
    // --------------------------------------------------

    public void Normalize()
    {
        this = System.Numerics.Vector3.Normalize(this);
    }

    public static Vector3 Normalize(Vector3 value)
    {
        return System.Numerics.Vector3.Normalize(value);
    }

    public static Vector3 Cross(Vector3 a, Vector3 b)
    {
        return System.Numerics.Vector3.Cross(a, b);
    }

    // Construtor
    // --------------------------------------------------

    /// <summary>
    /// Cria um novo vetor com as componentes x, y e z fornecidas.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Vector3(float x, float y, float z = 0.0f)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public Vector3(float value)
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
    public Vector3(Vector2 vector, float z = 0.0f)
    {
        X = vector.X;
        Y = vector.Y;
        Z = z;
    }

    // Conversão Implicita
    // --------------------------------------------------

    public static implicit operator System.Numerics.Vector3(Vector3 vector)
    {
        return new System.Numerics.Vector3(
            vector.X,
            vector.Y,
            vector.Z
        );
    }

    public static implicit operator Vector3(System.Numerics.Vector3 vector)
    {
        return new Vector3(
            vector.X,
            vector.Y,
            vector.Z
        );
    }

    // Multiplicação
    // --------------------------------------------------

    public static Vector3 operator *(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.X * b.X,
            a.Y * b.Y,
            a.Z * b.Z
        );
    }

    public static Vector3 operator *(Vector3 a, float b)
    {
        return new Vector3(
            a.X * b,
            a.Y * b,
            a.Z * b
        );
    }

    public static Vector3 operator *(float a, Vector3 b)
    {
        return new Vector3(
            a * b.X,
            a * b.Y,
            a * b.Z
        );
    }

    // Divisão
    // --------------------------------------------------

    public static Vector3 operator /(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.X / b.X,
            a.Y / b.Y,
            a.Z / b.Z
        );
    }

    public static Vector3 operator /(Vector3 a, float b)
    {
        return new Vector3(
            a.X / b,
            a.Y / b,
            a.Z / b
        );
    }

    public static Vector3 operator /(float a, Vector3 b)
    {
        return new Vector3(
            a / b.X,
            a / b.Y,
            a / b.Z
        );
    }

    // Adição
    // --------------------------------------------------

    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z
        );
    }

    public static Vector3 operator +(Vector3 a, float b)
    {
        return new Vector3(
            a.X + b,
            a.Y + b,
            a.Z + b
        );
    }

    public static Vector3 operator +(float a, Vector3 b)
    {
        return new Vector3(
            a + b.X,
            a + b.Y,
            a + b.Z
        );
    }

    // Subtração
    // --------------------------------------------------

    public static Vector3 operator -(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.X - b.X,
            a.Y - b.Y,
            a.Z - b.Z
        );
    }

    public static Vector3 operator -(Vector3 a, float b)
    {
        return new Vector3(
            a.X - b,
            a.Y - b,
            a.Z - b
        );
    }

    public static Vector3 operator -(float a, Vector3 b)
    {
        return new Vector3(
            a - b.X,
            a - b.Y,
            a - b.Z
        );
    }
}
