using GameEngine.Mathematics;

namespace GameEngine.Core;

public class EngineOptions
{
    public static readonly EngineOptions Default = new EngineOptions();

    public Vector2Int Size { get; set; } = new Vector2Int(1280, 720);

    public Vector2Int MinimumSize { get; set; }

    public string Title { get; set; } = "Silk.NET Window";

    public int Samples;

    public bool IsVisible { get; set; } = true;
}
