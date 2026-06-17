using GameEngine.Core;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung;
using Silk.NET.OpenGL;

namespace GameEngine.Interfaces;

public class Interface
{
    private GL _gl = Engine.GL;

    private ShaderProgram _shader = null!;
    private Camera _camera = null!;
    private DebugScreen _debugScreen = null!;

    public static float ScreenWidth;
    public static float ScreenHeight;

    private float _scaleFactor = 2.0f;

    // Construtor
    // --------------------------------------------------

    public Interface()
    {
        // shader
        // --------------------------------------------------

        _shader = new ShaderProgram("interface");

        // debug screen
        // --------------------------------------------------

        _debugScreen = new DebugScreen();
    }

    // 
    // --------------------------------------------------

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    // 
    // --------------------------------------------------

    public void Update()
    {
        // screen size
        // --------------------------------------------------

        ScreenWidth = Screen.Width / _scaleFactor;
        ScreenHeight = Screen.Height / _scaleFactor;

        // debug screen
        // --------------------------------------------------

        _debugScreen.SetCamera(_camera);
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

        // debug screen
        // --------------------------------------------------

        _gl.Disable(EnableCap.DepthTest);

        _debugScreen.Draw(_shader);

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
