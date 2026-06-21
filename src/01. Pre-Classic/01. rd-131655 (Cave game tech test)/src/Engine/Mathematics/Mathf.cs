namespace GameEngine.Mathematics;

/// <summary>
/// Uma coleção de funções matemáticas comuns.
/// </summary>
public struct Mathf
{
    public static float PI => (float)Math.PI;

    public static float DegToRad => (PI * 2.0f) / 360.0f;

    //
    // --------------------------------------------------

    /// <summary>
    /// Retorna o valor absoluto de f.
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static float Abs(float f)
    {
        return (float)System.Math.Abs(f);
    }

    /// <summary>
    /// Retorna o seno do ângulo f em radianos.
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static float Sin(float f)
    {
        return (float)System.Math.Sin(f);
    }

    /// <summary>
    /// Retorna o cosseno do ângulo f em radianos.
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static float Cos(float f)
    {
        return (float)System.Math.Cos(f);
    }

    public static float Radians(float degress)
    {
        return DegToRad * degress;
    }

    // Clamp
    // --------------------------------------------------

    /// <summary>
    /// Limita um valor entre um valor mínimo e um valor máximo de ponto flutuante.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float Clamp(float value, float min, float max)
    {
        return (float)System.Math.Clamp(value, min, max);
    }

    /// <summary>
    /// Limita o valor entre o mínimo e o máximo e retorna o valor.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public static float Clamp(int value, float min, float max) // ARRUMA ISSO AQUI
    {
        return (float)System.Math.Clamp(value, min, max);
    }

    // Max
    // --------------------------------------------------

    /// <summary>
    /// Retorna o maior de dois ou mais valores.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Max(float a, float b)
    {
        return (float)System.Math.Max(a, b);
    }

    /// <summary>
    /// Retorna o maior de dois ou mais valores.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int Max(int a, int b)
    {
        return System.Math.Max(a, b);
    }

    /// <summary>
    /// Retorna o maior de dois ou mais valores.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static long Max(long a, long b)
    {
        return System.Math.Max(a, b);
    }

    // Min
    // --------------------------------------------------

    /// <summary>
    /// Retorna o menor de dois ou mais valores.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Min(float a, float b)
    {
        return (float)System.Math.Min(a, b);
    }

    /// <summary>
    /// Retorna o menor de dois ou mais valores.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int Min(int a, int b)
    {
        return System.Math.Min(a, b);
    }

    // Flor
    // --------------------------------------------------

    /// <summary>
    /// Retorna o maior número inteiro menor ou igual a f.
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static float Floor(float f)
    {
        return (float)System.Math.Floor(f);
    }

    /// <summary>
    /// Retorna o maior número inteiro menor ou igual a f.
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static int FloorToInt(float f)
    {
        return (int)System.Math.Floor(f);
    }
}
