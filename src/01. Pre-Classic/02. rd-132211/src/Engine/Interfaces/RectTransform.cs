using GameEngine.Mathematics;

namespace GameEngine.Interfaces;

/// <summary>
/// Informações de posição, tamanho, âncora e pivô para um retângulo.
/// </summary>
public class RectTransform : Tranform
{
    /// <summary>
    /// Largura do retângulo.
    /// </summary>
    public float Width = 100.0f;

    /// <summary>
    /// Altura do retângulo.
    /// </summary>
    public float Height = 100.0f;

    // Anchors
    // --------------------------------------------------

    /// <summary>
    /// A posição normalizada no RectTransform pai à qual o canto inferior esquerdo está ancorado.
    /// </summary>
    public Vector2 AnchorMin = new(0.5f, 0.5f);

    /// <summary>
    /// A posição normalizada no RectTransform pai à qual o canto superior direito está ancorado.
    /// </summary>
    public Vector2 AnchorMax = new(0.5f, 0.5f);

    // Pivot
    // --------------------------------------------------

    /// <summary>
    /// A posição normalizada neste RectTransform em torno da qual ocorre a rotação.
    /// </summary>
    public Vector2 Pivot = new(0.5f, 0.5f);
}
