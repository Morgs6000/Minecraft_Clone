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
    private Font _font = null!;

    private int _screenWidht;
    private int _screenHeight;

    private int _frames = 0;
    private float _timeAccumulator = 0.0f;
    private string _fpsString = "0 fps";

    private string _memory = "Memory: ";

    private CpuUsageTracker _cpuTracker = new CpuUsageTracker();
    private string _cpuString = "CPU: 0%";

    private Camera _camera = null!;

    // Construtor
    // --------------------------------------------------

    public Interface()
    {
        // shader
        // --------------------------------------------------

        _shader = new ShaderProgram("interface");

        _screenWidht = Screen.Width * 240 / Screen.Height;
        _screenHeight = Screen.Height * 240 / Screen.Height;

        // font
        // --------------------------------------------------

        _font = new Font();
    }

    // 
    // --------------------------------------------------

    public void Update()
    {
        // font
        // --------------------------------------------------

        _frames++;
        _timeAccumulator += Time.DeltaTime;

        if (_timeAccumulator >= 1.0f)
        {
            _fpsString = $"{_frames} fps";
            _frames = 0;

            _memory = "Memory: "
                + SystemInfo.FormatBytes(SystemInfo.UsedMemoryBytes) + " / "
                + SystemInfo.FormatBytes(SystemInfo.TotalMemoryBytes);

            float cpu = _cpuTracker.GetUsage();
            _cpuString = $"CPU: {cpu:F0}%";

            _timeAccumulator %= 1.0f;
        }

        int h = _screenHeight - 8;

        _font.Mesh.Clear();

        _font.DrawShadow(Game.Version, 2, h - 2, 16777215);
        _font.DrawShadow(_fpsString, 2, h - 12, 16777215);

        _font.DrawShadow(_memory, 2, h - 32, 16777215);
        _font.DrawShadow(_cpuString, 2, h - 42, 16777215);

        string camPos = "Pos: "
            + $"{_camera.Position.X:F3}" + ", "
            + $"{_camera.Position.Y:F3}" + ", "
            + $"{_camera.Position.Z:F3}";

        _font.DrawShadow(camPos, 2, h - 62, 16777215);

        _font.MeshRenderer.Mesh = _font.Mesh;
    }

    // 
    // --------------------------------------------------
    
    public void SetCamera(Camera camera)
    {
        _camera = camera;
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

        // font
        // --------------------------------------------------

        _gl.Disable(EnableCap.DepthTest);

        model = Matrix4x4.Identity;
        _shader.SetUniform("model", model);

        _font.Texture.Bind();
        _font.MeshRenderer.Draw(_shader);

        _gl.Enable(EnableCap.DepthTest);
    }

    // 
    // --------------------------------------------------

    private Matrix4x4 GetProjectionMatrix()
    {
        return Matrix4x4.Orthographic(
            left:   0.0f,
            right:  (float)_screenWidht,
            bottom: 0.0f,
            top:    (float)_screenHeight,
            zNear:  0.3f,
            zFar:   1000.0f
        );
    }
}
