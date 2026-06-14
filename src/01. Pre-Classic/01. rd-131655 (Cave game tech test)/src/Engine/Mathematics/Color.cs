using System.Globalization;

namespace GameEngine.Mathematics;

/// <summary>
/// Representação das cores RGBA.
/// </summary>
public struct Color
{
    /// <summary>
    /// Componente vermelho da cor.
    /// </summary>
    public float R;

    /// <summary>
    /// Componente verde da cor.
    /// </summary>
    public float G;

    /// <summary>
    /// Componente azul da cor.
    /// </summary>
    public float B;

    /// <summary>
    /// Componente alfa da cor.
    /// </summary>
    public float A;

    // --------------------------------------------------

    /// <summary>
    /// Completamente transparente. RGBA é (0, 0, 0, 0).
    /// </summary>
    public static Color Clear => new(0.0f, 0.0f, 0.0f, 0.0f);

    /// <summary>
    /// Preto sólido. RGBA é (0, 0, 0, 1).
    /// </summary>
    public static Color Black => new(0.0f, 0.0f, 0.0f, 1.0f);

    /// <summary>
    /// Branco sólido. RGBA é (1, 1, 1, 1).
    /// </summary>
    public static Color White => new(1.0f, 1.0f, 1.0f, 1.0f);

    /// <summary>
    /// Cinza. RGBA é (0,5, 0,5, 0,5, 1).
    /// </summary>
    public static Color Gray => new(0.5f, 0.5f, 0.5f, 1.0f);

    /// <summary>
    /// Vermelho sólido. RGBA é (1, 0, 0, 1).
    /// </summary>
    public static Color Red => new(1.0f, 0.0f, 0.0f, 1.0f);

    /// <summary>
    /// 
    /// </summary>
    public static Color DarkRed => new(0.5f, 0.0f, 0.0f, 1.0f);

    /// <summary>
    /// Verde sólido. RGBA é (0, 1, 0, 1).
    /// </summary>
    public static Color Green => new(0.0f, 1.0f, 0.0f, 1.0f);

    /// <summary>
    /// 
    /// </summary>
    public static Color DarkGreen => new(0.0f, 0.5f, 0.0f, 1.0f);

    /// <summary>
    /// Azul sólido. RGBA é (0, 0, 1, 1).
    /// </summary>
    public static Color Blue => new(0.0f, 0.0f, 1.0f, 1.0f);

    /// <summary>
    /// 
    /// </summary>
    public static Color DarkBlue => new(0.0f, 0.0f, 0.5f, 1.0f);

    /// <summary>
    /// Amarelo. O valor RGBA é (1, 0,92, 0,016, 1), mas a cor é agradável aos olhos!
    /// </summary>
    public static Color Yellow => new(1.0f, 0.92f, 0.016f, 1.0f);

    /// <summary>
    /// Magenta. RGBA é (1, 0, 1, 1).
    /// </summary>
    public static Color Magenta => new(1.0f, 0.0f, 1.0f, 1.0f);

    /// <summary>
    /// Ciano. RGBA é (0, 1, 1, 1).
    /// </summary>
    public static Color Cyan => new(0.0f, 1.0f, 1.0f, 1.0f);

    /// <summary>
    /// 
    /// </summary>
    public static Color DarkGreenBlueish => new(0.2f, 0.3f, 0.3f, 1.0f);

    /// <summary>
    /// 
    /// </summary>
    public static Color OrangeIsh => new(1.0f, 0.5f, 0.2f, 1.0f);

    /// <summary>
    /// 
    /// </summary>
    public static Color CornflowerBlue => new(100, 149, 237, 255);

    /// <summary>
    /// 
    /// </summary>
    public static Color Manhattan => new(49, 77, 121, 255);

    /// <summary>
    /// 
    /// </summary>
    public static Color LightSkyBlue => new Color(0.5f, 0.8f, 1.0f, 1.0f);

    // Construtor
    // --------------------------------------------------

    /// <summary>
    /// Constrói uma nova cor com os componentes r, g, b e a fornecidos.
    /// </summary>
    /// <param name="red">Componente vermelho.</param>
    /// <param name="green">Componente verde.</param>
    /// <param name="blue">Componente azul.</param>
    /// <param name="alpha">Componente alfa.</param>
    public Color(float red, float green, float blue, float alpha = 1.0f)
    {
        R = red;
        G = green;
        B = blue;
        A = alpha;
    }

    /// <summary>
    /// Constrói uma nova cor com os componentes r, g, b e a fornecidos.
    /// </summary>
    /// <param name="red">Componente vermelho.</param>
    /// <param name="green">Componente verde.</param>
    /// <param name="blue">Componente azul.</param>
    /// <param name="alpha">Componente alfa.</param>
    public Color(int red, int green, int blue, int alpha = 255)
    {
        R = red   / 255.0f;
        G = green / 255.0f;
        B = blue  / 255.0f;
        A = alpha / 255.0f;
    }

    // Hexadecimal
    // --------------------------------------------------

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Color FromHex(string hex)
    {
        hex = hex.TrimStart('#');

        if (hex.Length != 6 && hex.Length != 8)
        {
            throw new ArgumentException("Hex deve ter 6 ou 8 caracteres.");
        }

        float r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber) / 255.0f;
        float g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber) / 255.0f;
        float b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber) / 255.0f;
        float a;

        if (hex.Length == 8)
        {
            a = int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber) / 255.0f;
        }
        else
        {
            a = 1.0f;
        }

        return new Color(r, g, b, a);
    }

    // HSV
    // --------------------------------------------------

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hue"></param>
    /// <param name="saturation"></param>
    /// <param name="value"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    public static Color FromHsv(float hue, float saturation, float value, float alpha = 1.0f)
    {
        float h = hue * 360.0f;
        float s = saturation;
        float v = value;
        float a = alpha;

        float c = v * s;

        float H = h / 60.0f;
        float x = c * (1.0f - Mathf.Abs((H % 2.0f) - 1.0f));

        float r, g, b;

        if (H >= 0.0f && H < 1.0f)
        {
            r = c;
            g = x;
            b = 0.0f;
        }
        else if (H >= 1.0f && H < 2.0f)
        {
            r = x;
            g = c;
            b = 0.0f;
        }
        else if (H >= 2.0f && H < 3.0f)
        {
            r = 0.0f;
            g = c;
            b = x;
        }
        else if (H >= 3.0f && H < 4.0f)
        {
            r = 0.0f;
            g = x;
            b = c;
        }
        else if (H >= 4.0f && H < 5.0f)
        {
            r = x;
            g = 0.0f;
            b = c;
        }
        else if (H >= 5.0f && H <= 6.0f)
        {
            r = c;
            g = 0.0f;
            b = x;
        }
        else
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
        }

        float m = v - c;

        return new Color(r + m, g + m, b + m, a);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hue"></param>
    /// <param name="saturation"></param>
    /// <param name="value"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    public static Color FromHsv(int hue, int saturation, int value, int alpha = 255)
    {
        return FromHsv(
            hue / 360.0f,
            saturation / 100.0f,
            value / 100.0f,
            alpha / 255.0f
        );
    }

    // To String
    // --------------------------------------------------

    public string ToFloatString()
    {
        return $"RGBA: {R:F3}, {G:F3}, {B:F3}, {A:F3}";
    }

    public string ToIntString()
    {
        return $"RGBA: {(int)(R * 255)}, {(int)(G * 255)}, {(int)(B * 255)}, {(int)(A * 255)}";
    }

    public string ToHexString(bool incluedAlpha = false)
    {
        int r = (int)(R * 255);
        int g = (int)(G * 255);
        int b = (int)(B * 255);
        int a = (int)(A * 255);

        return incluedAlpha
            ? $"Hex: #{r:X2}{g:X2}{b:X2}{a:X2}"
            : $"Hex: #{r:X2}{g:X2}{b:X2}";
    }

    public string ToHsvString()
    {
        float max = Mathf.Max(R, Mathf.Max(G, B));
        float min = Mathf.Min(R, Mathf.Min(G, B));
        float delta = max - min;

        float h = 0.0f;
        float s = (max == 0.0f) ? 0.0f : delta / max;
        float v = max;

        if (delta != 0.0f)
        {
            if (max == R)
            {
                h = (G - B) / delta + (G < B ? 6.0f : 0.0f);
            }
            else if (max == G)
            {
                h = (B - R) / delta + 2.0f;
            }
            else
            {
                h = (R - G) / delta + 4.0f;
            }

            h /= 6.0f;
        }

        return $"HSV: {h * 360.0f:F1}, {s:F3}, {v:F3}";
    }

    public string ToHsvIntString()
    {
        float max = Math.Max(R, Math.Max(G, B));
        float min = Math.Min(R, Math.Min(G, B));
        float delta = max - min;

        float h = 0f;
        float s = (max == 0f) ? 0f : delta / max;
        float v = max;

        if (delta != 0f)
        {
            if (max == R)
                h = (G - B) / delta + (G < B ? 6f : 0f);
            else if (max == G)
                h = (B - R) / delta + 2f;
            else // max == B
                h = (R - G) / delta + 4f;
            h /= 6f;
        }

        int hi = (int)(h * 360f);
        int si = (int)(s * 100f);
        int vi = (int)(v * 100f);

        return $"HSV: {hi}, {si}, {vi}";
    }
    
    // Conversão Implicita
    // --------------------------------------------------

    public static implicit operator System.Drawing.Color(Color color)
    {
        return System.Drawing.Color.FromArgb(
            (int)(color.A * 255.0f),
            (int)(color.R * 255.0f),
            (int)(color.G * 255.0f),
            (int)(color.B * 255.0f)
        );
    }
    
    public static implicit operator Color(System.Drawing.Color color)
    {
        return new Color(
            color.R / 255.0f,
            color.G / 255.0f,
            color.B / 255.0f,
            color.A / 255.0f
        );
    }

    public static implicit operator Vector4(Color color)
    {
        return new Vector4(
            color.R,
            color.G,
            color.B,
            color.A
        );
    }
    
    public static implicit operator Color(Vector4 vector)
    {
        return new Color(
            vector.X,
            vector.Y,
            vector.Z,
            vector.W
        );
    }

    public static implicit operator Vector3(Color color)
    {
        return new Vector3(
            color.R,
            color.G,
            color.B
        );
    }
    
    public static implicit operator Color(Vector3 vector)
    {
        return new Color(
            vector.X,
            vector.Y,
            vector.Z
        );
    }

    // Multiplicação
    // --------------------------------------------------

    public static Color operator *(Color a, Color b)
    {
        return new Color(
            a.R * b.R,
            a.G * b.G,
            a.B * b.B,
            a.A * b.A
        );
    }

    public static Color operator *(Color a, float b)
    {
        return new Color(
            a.R * b,
            a.G * b,
            a.B * b,
            a.A * b
        );
    }

    public static Color operator *(float a, Color b)
    {
        return new Color(
            a * b.R,
            a * b.G,
            a * b.B,
            a * b.A
        );
    }

    // Divisão
    // --------------------------------------------------

    public static Color operator /(Color a, Color b)
    {
        return new Color(
            a.R / b.R,
            a.G / b.G,
            a.B / b.B,
            a.A / b.A
        );
    }

    public static Color operator /(Color a, float b)
    {
        return new Color(
            a.R / b,
            a.G / b,
            a.B / b,
            a.A / b
        );
    }

    public static Color operator /(float a, Color b)
    {
        return new Color(
            a / b.R,
            a / b.G,
            a / b.B,
            a / b.A
        );
    }

    // Adição
    // --------------------------------------------------

    public static Color operator +(Color a, Color b)
    {
        return new Color(
            a.R + b.R,
            a.G + b.G,
            a.B + b.B,
            a.A + b.A
        );
    }

    public static Color operator +(Color a, float b)
    {
        return new Color(
            a.R + b,
            a.G + b,
            a.B + b,
            a.A + b
        );
    }

    public static Color operator +(float a, Color b)
    {
        return new Color(
            a + b.R,
            a + b.G,
            a + b.B,
            a + b.A
        );
    }

    // Subtração
    // --------------------------------------------------

    public static Color operator -(Color a, Color b)
    {
        return new Color(
            a.R - b.R,
            a.G - b.G,
            a.B - b.B,
            a.A - b.A
        );
    }

    public static Color operator -(Color a, float b)
    {
        return new Color(
            a.R - b,
            a.G - b,
            a.B - b,
            a.A - b
        );
    }

    public static Color operator -(float a, Color b)
    {
        return new Color(
            a - b.R,
            a - b.G,
            a - b.B,
            a - b.A
        );
    }
}
