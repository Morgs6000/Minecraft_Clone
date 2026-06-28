using System.Runtime.InteropServices;
using GameEngine.Core;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung.Level;

namespace RubyDung.Interfaces;

public class DebugScreen
{
    private Font _font = null!;
    private CpuUsageTracker _cpuTracker = new CpuUsageTracker();
    private Player _player = null!;

    private string _version = $"{Game.Version}";

    private int _frames = 0;
    private float _timeAccumulator = 0.0f;
    private string _fpsString = "";

    private string _playerPos = "";
    private string _cameraDir = "";
    private string _blockPos = "";
    private string _chunkPos = "";

    private string _author = $"by Morgana Stradivarius";

    private string _memory = "";
    private string _memoryMax = "";
    private long _maxMemoryUsed = 0;

    private string _cpu1 = "";
    private string _cpu0 = "";
    private float _maxCpuUsege = 0.0f;

    private string _openGLVersion = $"OpenGL {SystemInfo.OpenGLVersion}";

    private string _dotnetVersion = $"{RuntimeInformation.FrameworkDescription}";

    public DebugScreen()
    {
        _font = new Font();
    }

    public void SetCamera(Player player)
    {
        _player = player;
    }

    public void Update()
    {
        CalculateFPSs();
        CalculatePlayerPos();
        CalculateCameraDir();

        string gameMode = $"Game Mode: {Game.Mode}";
        string speed = $"Movement Speed: {_player.MovementSpeed}";
        string onFly = $"OnFly: {_player.OnFly}";
        string onGround = $"OnGround: {_player.OnGround}";
        string onSprint = $"OnSprint: {_player.OnSprint}";
        string onSneak = $"OnSneak: {_player.OnSneak}";

        string shadedMode = $"Shaded Mode [F3 + W]: {Engine.ShadingMode}";

        string display = $"Display: {Screen.Width}x{Screen.Height}";

        string targetBlock_0 = $"Targeted Block: {BlockInteraction.HitResult}";
        string targetBlock_1 = $"{BlockInteraction.BlockName}";

        float w = Interface.ScreenWidth - 2;
        float h = Interface.ScreenHeight - 8;

        _font.Clear();

        // --- Texto a esquerda ---

        _font.DrawShadow(_version   , 2, h -   2, 16777215);
        _font.DrawShadow(_fpsString , 2, h -  12, 16777215);

        _font.DrawShadow(_playerPos , 2, h -  32, 16777215);
        _font.DrawShadow(_blockPos  , 2, h -  42, 16777215);
        _font.DrawShadow(_chunkPos  , 2, h -  52, 16777215);
        _font.DrawShadow(_cameraDir , 2, h -  62, 16777215);

        _font.DrawShadow(gameMode   , 2, h -  82, 16777215);
        _font.DrawShadow(speed      , 2, h -  92, 16777215);
        _font.DrawShadow(onFly      , 2, h - 102, 16777215);
        _font.DrawShadow(onGround   , 2, h - 112, 16777215);
        _font.DrawShadow(onSprint   , 2, h - 122, 16777215);
        _font.DrawShadow(onSneak    , 2, h - 132, 16777215);
        
        _font.DrawShadow(shadedMode , 2, h - 152, 16777215);

        _font.DrawShadow(_author    , 2,       2, 16777215);

        // --- Texto a direita ---

        _font.DrawShadow(_dotnetVersion, w - MeasureString(_dotnetVersion), h - 2, 16777215);

        _font.DrawShadow(_memory, w - MeasureString(_memory)   , h - 22, 16777215);
        _font.DrawShadow(_memoryMax, w - MeasureString(_memoryMax), h - 32, 16777215);

        _font.DrawShadow(_cpu0, w - MeasureString(_cpu0), h - 52, 16777215);
        _font.DrawShadow(_cpu1, w - MeasureString(_cpu1), h - 62, 16777215);

        _font.DrawShadow(display, w - MeasureString(display), h - 82, 16777215);

        _font.DrawShadow(_openGLVersion, w - MeasureString(_openGLVersion), h - 102, 16777215);

        if (BlockInteraction.Target)
        {
            _font.DrawShadow(targetBlock_0, w - MeasureString(targetBlock_0), h - 122, 16777215);
            _font.DrawShadow(targetBlock_1, w - MeasureString(targetBlock_1), h - 132, 16777215);
        }

        _font.FontRenderer();
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

        _memory =
            $"Memory: {percent} " +
            $"{SystemInfo.FormatToMB(used, false)} / " +
            $"{SystemInfo.FormatToMB(total, false)} MB";

        string maxPercent = (total > 0) ? $"{((float)_maxMemoryUsed / total * 100.0f):F0}%" : "N/A";
        
        _memoryMax =
            $"(MaxUsed: {maxPercent} {SystemInfo.FormatToMB(_maxMemoryUsed)})";
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

    private void CalculatePlayerPos()
    {
        _playerPos = $"XYZ: " + 
            $"{_player.Position.X:F3} / " +
            $"{_player.Position.Y:F3} / " +
            $"{_player.Position.Z:F3}";

        _blockPos = $"Block: " + 
            $"{Mathf.FloorToInt(_player.Position.X)} " +
            $"{Mathf.FloorToInt(_player.Position.Y)} " +
            $"{Mathf.FloorToInt(_player.Position.Z)}";

        _chunkPos = $"Chunk: " + 
            $"{Mathf.FloorToInt(_player.Position.X / World.ChunkSize)} " +
            $"{Mathf.FloorToInt(_player.Position.Y / World.ChunkSize)} " +
            $"{Mathf.FloorToInt(_player.Position.Z / World.ChunkSize)}";
    }

    private void CalculateCameraDir()
    {
        float yaw = _player.Yaw % 360.0f;

        if (yaw < 0)
        {
            yaw += 360;
        }

        string dir = "";

        if (yaw > 22.5f && yaw < 67.5f)
        {
            dir = "Northeast (+X / +Y) ";
        }
        // if (yaw > 45.0f && yaw < 135.0f)
        else if (yaw > 67.5f && yaw < 112.5f)
        {
            dir = "North (+Y) ";
        }
        else if (yaw > 112.5f && yaw < 157.5f)
        {
            dir = "Northwest (-X / +Y) ";
        }
        // else if (yaw > 135.0f && yaw < 225.0f)
        else if (yaw > 157.5f && yaw < 202.5f)
        {
            dir = "West (-X) ";
        }
        else if (yaw > 202.5f && yaw < 247.5f)
        {
            dir = "Southwest (-X / -Y) ";
        }
        // else if (yaw > 225.0f && yaw < 315.0f)
        else if (yaw > 247.5f && yaw < 292.5f)
        {
            dir = "South (-Y) ";
        }
        else if (yaw > 292.5f && yaw < 337.5f)
        {
            dir = "Southeast (+X / -Y) ";
        }
        else
        {
            dir = "East (+X) ";
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
