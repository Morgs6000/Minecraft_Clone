using GameEngine.Core;
using GameEngine.Inputs;
using GameEngine.Interfaces;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung;
using RubyDung.Level;
using Silk.NET.OpenGL;

namespace RubyDung.Interfaces;

public class Interface
{
    public static float ScreenWidth { get; private set; }
    public static float ScreenHeight { get; private set; }

    public static bool DebugScreen = false;

    // 
    // --------------------------------------------------

    private GL _gl = Engine.GL;

    private ShaderProgram _shader = null!;
    private World _world = null!;
    private Player _player = null!;

    private GameModeSwitcher _gameModeSwitcher = null!;
    private HeightLimit _heightLimit = null!;
    private Saving _saving = null!;
    private SelectedBlock _selectedBlock = null!;

    private DebugScreen _debugScreen = null!;

    private float _scaleFactor = 2.0f;

    // Construtor
    // --------------------------------------------------

    public Interface(World world)
    {
        _world = world;

        // screen size
        // --------------------------------------------------

        ScreenWidth = Screen.Width / _scaleFactor;
        ScreenHeight = Screen.Height / _scaleFactor;

        // shader
        // --------------------------------------------------

        _shader = new("interface");

        // 
        // --------------------------------------------------

        _gameModeSwitcher = new GameModeSwitcher(_world);
        _heightLimit = new HeightLimit();
        _saving = new Saving();
        _selectedBlock = new SelectedBlock(world);

        _debugScreen = new DebugScreen();
    }

    // 
    // --------------------------------------------------

    public void SetPlayer(Player player)
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

        // 
        // --------------------------------------------------

        if (DebugHotkeys.ShowGameModeSwitcher)
        {
            _gameModeSwitcher.Update();
        }
        if (World.OnHeightLimit)
        {
            _heightLimit.Update();
        }
        if (Input.GetKeyDown(KeyCode.Enter))
        {
            _saving.Start();
        }

        _saving.Update();
        _selectedBlock.Update();

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

        _gl.Enable(EnableCap.Blend);
        _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        {
            if (DebugHotkeys.ShowGameModeSwitcher)
            {
                _gameModeSwitcher.Draw(_shader);
            }
            if (World.OnHeightLimit)
            {
                _heightLimit.Draw(_shader);
            }

            _saving.Draw(_shader);
            _selectedBlock.Draw(_shader);

            if (DebugScreen) 
            {
                _debugScreen.Draw(_shader);
            }
        }

        _gl.Disable(EnableCap.Blend);
        _gl.Enable(EnableCap.DepthTest);
    }

    // 
    // --------------------------------------------------

    private Matrix4x4 GetProjectionMatrix()
    {
        return Matrix4x4.Orthographic(
            left: 0.0f,
            right: ScreenWidth,
            bottom: 0.0f,
            top: ScreenHeight,
            zNear: 0.3f,
            zFar: 1000.0f
        );
    }
}
