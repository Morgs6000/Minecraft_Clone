using GameEngine.Mathematics;

namespace GameEngine;

/// <summary>
/// Posição, rotação e escala de um objeto.
/// </summary>
public class Tranform
{
    /// <summary>
    /// A posição da transformação no espaço mundial.
    /// </summary>
    public Vector3 Position = new(0.0f, 0.0f, 0.0f);

    /// <summary>
    /// A rotação da transformação no espaço mundial, armazenada como um quaternião.
    /// </summary>
    public Vector3 Rotation = new(0.0f, 0.0f, 0.0f);

    /// <summary>
    /// A escala da transformação em relação ao pai.
    /// </summary>
    public Vector3 Scale = new(1.0f, 1.0f, 1.0f);
}
