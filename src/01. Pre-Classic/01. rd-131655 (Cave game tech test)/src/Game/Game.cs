using GameEngine.Core;
using GameEngine.Inputs;
using Silk.NET.Maths;

namespace RubyDung;

public class Game : Engine
{
    protected override void OnLoad()
    {
        
    }

    protected override void OnResize(Vector2D<int> newSize)
    {
        
    }

    protected override void OnUpdate(double deltaTime)
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Close();
        }
    }

    protected override void OnRender(double deltaTime)
    {
        
    }

    protected override void OnClosing()
    {
        
    }
}
