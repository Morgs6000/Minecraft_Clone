using GameEngine.Core;
using GameEngine.Inputs;
using GameEngine.Rendering;

namespace RubyDung;

public class DebugHotkeys
{
    public static void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            switch (Engine.ShadingMode)
            {
                case ShadingMode.Shaded:
                    Engine.ShadingMode = ShadingMode.Shaded_Wireframe;
                    break;
                case ShadingMode.Shaded_Wireframe:
                    Engine.ShadingMode = ShadingMode.Wireframe;
                    break;
                case ShadingMode.Wireframe:
                    Engine.ShadingMode = ShadingMode.Shaded;
                    break;
            }
        }
    }
}
