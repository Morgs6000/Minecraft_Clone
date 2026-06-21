using System.Runtime.InteropServices;
using GameEngine.Core;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung;
using RubyDung.Level;

namespace GameEngine.Interfaces;

public class DebugScreen
{
    private Font _font = null!;
    private CpuUsageTracker _cpuTracker = new CpuUsageTracker();
    private Camera _camera = null!;

    private string _version = $"{Game.Version}";

    private int _frames = 0;
    private float _timeAccumulator = 0.0f;
    private string _fpsString = "";

    private string _cameraPos = "";
    private string _cameraDir = "";
    private string _chunkPos = "";

    private string _shadedMode = "";

    private string _author = $"by Morgana Stradivarius";

    private string _memory = "";
    private string _memoryMax = "";
    private long _maxMemoryUsed = 0;

    private string _cpu1 = "";
    private string _cpu0 = "";
    private float _maxCpuUsege = 0.0f;

    private string _display = "";

    private string _openGLVersion = $"OpenGL {SystemInfo.OpenGLVersion}";

    private string _dotnetVersion = $"{RuntimeInformation.FrameworkDescription}";

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
        CalculateFPSs();
        CalculateCameraPos();
        CalculateCameraDir();

        _shadedMode = $"Shaded Mode [F3 + W]: {Engine.ShadingMode}";
        _display = $"Display: {Screen.Width}x{Screen.Height}";

        float w = Interface.ScreenWidth - 2;
        float h = Interface.ScreenHeight - 8;

        _font.Mesh.Clear();

        // --- Texto a esquerda ---

        _font.DrawShadow(_version   , 2, h -  2, 16777215);
        _font.DrawShadow(_fpsString , 2, h - 12, 16777215);

        _font.DrawShadow(_cameraPos , 2, h - 32, 16777215);
        _font.DrawShadow(_chunkPos  , 2, h - 42, 16777215);
        _font.DrawShadow(_cameraDir , 2, h - 52, 16777215);
        
        _font.DrawShadow(_shadedMode, 2, h - 72, 16777215);

        _font.DrawShadow(_author    , 2,      2, 16777215);

        // --- Texto a direita ---

        _font.DrawShadow(_dotnetVersion, w - MeasureString(_dotnetVersion), h - 2, 16777215);

        _font.DrawShadow(_memory, w - MeasureString(_memory)   , h - 22, 16777215);
        _font.DrawShadow(_memoryMax, w - MeasureString(_memoryMax), h - 32, 16777215);

        _font.DrawShadow(_cpu0, w - MeasureString(_cpu0), h - 52, 16777215);
        _font.DrawShadow(_cpu1, w - MeasureString(_cpu1), h - 62, 16777215);

        _font.DrawShadow(_display, w - MeasureString(_display), h - 82, 16777215);

        _font.DrawShadow(_openGLVersion, w - MeasureString(_openGLVersion), h - 102, 16777215);

        _font.MeshRenderer.Mesh = _font.Mesh;
    }

    public void Draw(ShaderProgram shader)
    {
        Matrix4x4 model = Matrix4x4.Identity;
        shader.SetUniform("model", model);

        _font.Texture.Bind();
        _font.MeshRenderer.Draw(shader, false);
    }

    private void CalculateFPSs()
    {
        _frames++;
        _timeAccumulator += Time.DeltaTime;

        if (_timeAccumulator >= 1.0f)
        {
            _fpsString = $"{_frames} fps, {Chunk.Updates} chunk updates";            
            _frames = 0;
            Chunk.Updates = 0;

            CalculateMemory();
            CalculateCPU();

            _timeAccumulator %= 1.0f;
        }
    }

    private void CalculateMemory()
    {
        long used = SystemInfo.UsedMemoryBytes;
        long total = SystemInfo.TotalMemoryBytes;

        _maxMemoryUsed = Mathf.Max(_maxMemoryUsed, used);

        string percent = (total > 0) ? $"{((float)used / total * 100.0f):F0}%" : "N/A";

        // Trocar de :
        //      Memory: 0 B / 0 B
        // para:
        //      Memory: 0 / 0 B
        _memory =
            $"Memory: {percent} " +
            $"{SystemInfo.FormatBytes(used)} / " +
            $"{SystemInfo.FormatBytes(total)}";
        
        _memoryMax =
            $"(MaxUsed: {SystemInfo.FormatBytes(_maxMemoryUsed)})";
    }

    private void CalculateCPU()
    {
        float cpu = _cpuTracker.GetUsage();
        _maxCpuUsege = Mathf.Max(_maxCpuUsege, cpu);

        _cpu1 = $"{SystemInfo.ProcessorName}";

        _cpu0 = 
            $"CPU: {cpu:F0}%" +
            $" (MaxUsed: {_maxCpuUsege:F0}%)";
    }

    private void CalculateCameraPos()
    {
        _cameraPos = $"XYZ: " + 
            $"{_camera.Position.X:F3} / " +
            $"{_camera.Position.Y:F3} / " +
            $"{_camera.Position.Z:F3}";

        _chunkPos = $"Chunk: " + 
            $"{Mathf.FloorToInt(_camera.Position.X / World.ChunkSize)} " +
            $"{Mathf.FloorToInt(_camera.Position.Y / World.ChunkSize)} " +
            $"{Mathf.FloorToInt(_camera.Position.Z / World.ChunkSize)}";
    }

    private void CalculateCameraDir()
    {
        float yaw = _camera.Yaw % 360.0f;

        if (yaw < 0)
        {
            yaw += 360;
        }

        string dir = "";

        if (yaw > 22.5f && yaw < 67.5f)
        {
            dir = "Northeast (Positive X / Positive Y) ";
        }
        // if (yaw > 45.0f && yaw < 135.0f)
        else if (yaw > 67.5f && yaw < 112.5f)
        {
            dir = "North (Positive Y) ";
        }
        else if (yaw > 112.5f && yaw < 157.5f)
        {
            dir = "Northwest (Negative X / Positive Y) ";
        }
        // else if (yaw > 135.0f && yaw < 225.0f)
        else if (yaw > 157.5f && yaw < 202.5f)
        {
            dir = "West (Negative X) ";
        }
        else if (yaw > 202.5f && yaw < 247.5f)
        {
            dir = "Southwest (Negative X / Negative Y) ";
        }
        // else if (yaw > 225.0f && yaw < 315.0f)
        else if (yaw > 247.5f && yaw < 292.5f)
        {
            dir = "South (Negative Y) ";
        }
        else if (yaw > 292.5f && yaw < 337.5f)
        {
            dir = "Southeast (Positive X / Negative Y) ";
        }
        else
        {
            dir = "East (Positive X) ";
        }

        _cameraDir = $"Facing: {dir} {yaw:F0}";
    }
    
    private int MeasureString(string str)
    {
        int width = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '&')
            {
                i++; // pula o próximo caractere (código de cor)
                continue;
            }
            width += _font.CharWidths[str[i]];   // torna o campo acessível ou use um método
        }
        return width;
    }
}
