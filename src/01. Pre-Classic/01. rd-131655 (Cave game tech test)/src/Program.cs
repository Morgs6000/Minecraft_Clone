using GameEngine.Core;
using GameEngine.Mathematics;

namespace RubyDung;

public class Program
{
    private static void Main(string[] args)
    {
        EngineOptions options = EngineOptions.Default;

        options.Size = new Vector2Int(1024, 768);
        options.MinimumSize = new Vector2Int(800, 600);
        options.Title = "Game";

        Game game = new Game(options);
        game.Run();
    }
}
