using GameEngine.Core;
using GameEngine.Inputs;
using GameEngine.Rendering;

namespace RubyDung;

public class DebugHotkeys
{
    public static bool Pressed = false;
    public static bool ShowGameModeSwitcher = false;

    // 
    // --------------------------------------------------

    private static bool _anyComboHandled;
    private static bool _f3Alone;

    // 
    // --------------------------------------------------

    public static void Update()
    {
        _anyComboHandled = false;

        if (Input.GetKeyDown(KeyCode.F3))
        {
            _f3Alone = true;

            Pressed = true;
        }
        if (Input.GetKey(KeyCode.F3))
        {
            ShadedModeSwitcher();
            GameModeSwitcher();

            if (_anyComboHandled)
            {
                _f3Alone = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.F3))
        {
            if (_f3Alone)
            {
                Game.DebugScreen = !Game.DebugScreen;
            }

            _f3Alone = false;

            Pressed = false;

            ShowGameModeSwitcher = false;
        }
    }

    private static void ShadedModeSwitcher()
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

            _anyComboHandled = true;
        }
    }
    
    private static void GameModeSwitcher()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            ShowGameModeSwitcher = true;

            switch (Game.Mode)
            {
                case GameMode.Creative:
                    Game.Mode = GameMode.Survival;
                    break;
                case GameMode.Survival:
                    Game.Mode = GameMode.Adventure;
                    break;
                case GameMode.Adventure:
                    Game.Mode = GameMode.Spectator;
                    break;
                case GameMode.Spectator:
                    Game.Mode = GameMode.Creative;
                    break;
            }

            _anyComboHandled = true;
        }
    }
}
