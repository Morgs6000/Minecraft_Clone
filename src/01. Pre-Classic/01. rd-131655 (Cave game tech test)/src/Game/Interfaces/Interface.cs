using GameEngine.Core;
using GameEngine.Interfaces;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung;
using Silk.NET.OpenGL;

namespace RubyDung.Interfaces;

public class Interface
{
    public static float ScreenWidth { get; private set; }
    public static float ScreenHeight { get; private set; }

    // 
    // --------------------------------------------------

    private GL _gl = Engine.GL;

    private ShaderProgram _shader = null!;
    private Player _player = null!;

    private GameModeSwitcher _gameModeSwitcher = null!;
    private DebugScreen _debugScreen = null!;

    private float _scaleFactor = 2.0f;

    // Construtor
    // --------------------------------------------------

    public Interface()
    {
        // screen size
        // --------------------------------------------------

        ScreenWidth = Screen.Width / _scaleFactor;
        ScreenHeight = Screen.Height / _scaleFactor;

        // shader
        // --------------------------------------------------

        _shader = new("interface");

        // game mode switcher
        // --------------------------------------------------

        _gameModeSwitcher = new GameModeSwitcher();

        // debug screen
        // --------------------------------------------------

        _debugScreen = new DebugScreen();
    }

    // 
    // --------------------------------------------------

    public void SetCamera(Player player)
    {
        _player = player;
    }

    // 
    // --------------------------------------------------

    public void Update()
    {
        // screen size
        // --------------------------------------------------

        ScreenWidth = Screen.Width / _scaleFactor;
        ScreenHeight = Screen.Height / _scaleFactor;

        // game mode switcher
        // --------------------------------------------------

        if (DebugHotkeys.ShowGameModeSwitcher)
        {
            _gameModeSwitcher.Update();
        }

        // debug screen
        // --------------------------------------------------

        _debugScreen.SetCamera(_player);
        _debugScreen.Update();
    }

    // 
    // --------------------------------------------------

    public void Draw()
    {
        // shader
        // --------------------------------------------------

        _shader.Use();

        Matrix4x4 model = Matrix4x4.Identity;
        _shader.SetUniform("model", model);

        Matrix4x4 view = Matrix4x4.Identity;
        _shader.SetUniform("view", view);

        Matrix4x4 projection = GetProjectionMatrix();
        _shader.SetUniform("projection", projection);

        _gl.Disable(EnableCap.DepthTest);

        {
            // game mode switcher
            // --------------------------------------------------

            if (DebugHotkeys.ShowGameModeSwitcher)
            {
                _gameModeSwitcher.Draw(_shader);
            }

            // debug screen
            // --------------------------------------------------

            _debugScreen.Draw(_shader);
        }

        _gl.Enable(EnableCap.DepthTest);
    }

    // 
    // --------------------------------------------------

    private Matrix4x4 GetProjectionMatrix()
    {
        return Matrix4x4.Orthographic(
            left:   0.0f,
            right:  ScreenWidth,
            bottom: 0.0f,
            top:    ScreenHeight,
            zNear:  0.3f,
            zFar:   1000.0f
        );
    }
}
