using GameEngine.Core;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung;

namespace GameEngine.Interfaces;

public class DebugScreen
{
    private Font _font = null!;
    private CpuUsageTracker _cpuTracker = new CpuUsageTracker();
    private Camera _camera = null!;

    private int _frames = 0;
    private float _timeAccumulator = 0.0f;
    private string _fpsString = "0 fps";

    private string _memory = "Memory: 0 B / 0 B , MaxUsed: 0 B";
    private long _maxMemoryUsed = 0;

    private string _cpuString = "CPU: 0%, MaxUsed: 0%";
    private float _maxCpuUsege = 0.0f;

    public DebugScreen()
    {
        _font = new Font();
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    public void Update()
    {
        _frames++;
        _timeAccumulator += Time.DeltaTime;

        if (_timeAccumulator >= 1.0f)
        {
            // --- FPS ---
            _fpsString = $"{_frames} fps";
            _frames = 0;

            // --- Memory ---
            long used = SystemInfo.UsedMemoryBytes;
            _maxMemoryUsed = Mathf.Max(_maxMemoryUsed, used);

            _memory =
                "Memory: " +
                SystemInfo.FormatBytes(SystemInfo.UsedMemoryBytes) + " / " +
                SystemInfo.FormatBytes(SystemInfo.TotalMemoryBytes) + ", " +
                "MaxUsed: " + SystemInfo.FormatBytes(_maxMemoryUsed);

            // --- CPU ---
            float cpu = _cpuTracker.GetUsage();
            _maxCpuUsege = Mathf.Max(_maxCpuUsege, cpu);

            _cpuString =
                "CPU: " + $"{cpu:F0}%" + ", " +
                "MaxUsed: " + $"{_maxCpuUsege:F0}%";

            // --- reset? ---
            _timeAccumulator %= 1.0f;
        }

        float h = Interface.ScreenHeight - 8;

        _font.Mesh.Clear();

        string str = Game.Version + " - by Morgana Stradivarius";

        _font.DrawShadow(str       , 2, h -  2, 16777215);
        _font.DrawShadow(_memory   , 2, h - 12, 16777215);
        _font.DrawShadow(_cpuString, 2, h - 22, 16777215);
        _font.DrawShadow(_fpsString, 2, h - 32, 16777215);

        string camPos =
            "X: " + $"{_camera.Position.X:F3}" + ", " +
            "Y: " + $"{_camera.Position.Y:F3}" + ", " +
            "Z: " + $"{_camera.Position.Z:F3}";

        string shadedMode = "Shaded Mode (F3 + W): " + Engine.ShadingMode;

        _font.DrawShadow(camPos    , 2, h - 52, 16777215);
        _font.DrawShadow(shadedMode, 2, h - 62, 16777215);

        _font.MeshRenderer.Mesh = _font.Mesh;
    }
    
    public void Draw(ShaderProgram shader)
    {
        Matrix4x4 model = Matrix4x4.Identity;
        shader.SetUniform("model", model);

        _font.Texture.Bind();
        _font.MeshRenderer.Draw(shader, false);
    }
}
