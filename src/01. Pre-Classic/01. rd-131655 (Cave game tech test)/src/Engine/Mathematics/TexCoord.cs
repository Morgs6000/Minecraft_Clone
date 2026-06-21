namespace GameEngine.Mathematics;

public struct TexCoord
{
    /// <summary>
    /// Componente U do vetor.
    /// </summary>
    public float U;

    /// <summary>
    /// Componente V do vetor.
    /// </summary>
    public float V;

    // --------------------------------------------------

    public static TexCoord Zero => new (0.0f, 0.0f);

    public static TexCoord Positive => new (1.0f, 1.0f);
    public static TexCoord Negative => new (-1.0f, -1.0f);

    public static TexCoord PositiveX => new (1.0f, 0.0f);
    public static TexCoord NegativeX => new (-1.0f, 0.0f);

    public static TexCoord PositiveY => new (0.0f, 1.0f);
    public static TexCoord NegativeY => new (0.0f, -1.0f);

    // Normalize
    // --------------------------------------------------

    public void Normalize()
    {
        this = System.Numerics.Vector2.Normalize(this);
    }

    public static TexCoord Normalize(TexCoord value)
    {
        return System.Numerics.Vector2.Normalize(value);
    }

    // Construtor
    // --------------------------------------------------

    /// <summary>
    /// Cria um novo vetor com as componentes x e y fornecidas.
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    public TexCoord(float u, float v)
    {
        U = u;
        V = v;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public TexCoord(float value)
    {
        U = value;
        V = value;
    }

    // To String
    // --------------------------------------------------

    public override string ToString()
    {
        return $"{U}, {V}";
    }

    // Conversão Implicita
    // --------------------------------------------------

    public static implicit operator System.Numerics.Vector2(TexCoord vector)
    {
        return new System.Numerics.Vector2(
            vector.U,
            vector.V
        );
    }

    public static implicit operator TexCoord(System.Numerics.Vector2 vector)
    {
        return new TexCoord(
            vector.X,
            vector.Y
        );
    }

    // Multiplicação
    // --------------------------------------------------

    public static TexCoord operator *(TexCoord a, TexCoord b)
    {
        return new TexCoord(
            a.U * b.U,
            a.V * b.V
        );
    }

    public static TexCoord operator *(TexCoord a, float b)
    {
        return new TexCoord(
            a.U * b,
            a.V * b
        );
    }

    public static TexCoord operator *(float a, TexCoord b)
    {
        return new TexCoord(
            a * b.U,
            a * b.V
        );
    }

    // Divisão
    // --------------------------------------------------

    public static TexCoord operator /(TexCoord a, TexCoord b)
    {
        return new TexCoord(
            a.U / b.U,
            a.V / b.V
        );
    }

    public static TexCoord operator /(TexCoord a, float b)
    {
        return new TexCoord(
            a.U / b,
            a.V / b
        );
    }

    public static TexCoord operator /(float a, TexCoord b)
    {
        return new TexCoord(
            a / b.U,
            a / b.V
        );
    }

    // Adição
    // --------------------------------------------------

    public static TexCoord operator +(TexCoord a, TexCoord b)
    {
        return new TexCoord(
            a.U + b.U,
            a.V + b.V
        );
    }

    public static TexCoord operator +(TexCoord a, float b)
    {
        return new TexCoord(
            a.U + b,
            a.V + b
        );
    }

    public static TexCoord operator +(float a, TexCoord b)
    {
        return new TexCoord(
            a + b.U,
            a + b.V
        );
    }

    // Subtração
    // --------------------------------------------------

    public static TexCoord operator -(TexCoord a, TexCoord b)
    {
        return new TexCoord(
            a.U - b.U,
            a.V - b.V
        );
    }

    public static TexCoord operator -(TexCoord a, float b)
    {
        return new TexCoord(
            a.U - b,
            a.V - b
        );
    }

    public static TexCoord operator -(float a, TexCoord b)
    {
        return new TexCoord(
            a - b.U,
            a - b.V
        );
    }
}
