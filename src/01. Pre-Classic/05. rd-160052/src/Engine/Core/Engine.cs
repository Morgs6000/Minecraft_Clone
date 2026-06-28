using GameEngine.Inputs;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utilities;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace GameEngine.Core;

public class Engine
{
    public static GL GL { get; private set; } = null!;
    public static ShadingMode ShadingMode { get; set; } = ShadingMode.Shaded;

    public bool IsVisible;
    
    // 
    // --------------------------------------------------

    protected IWindow _window = null!;
    protected GL _gl = null!;

    private Vector2Int _minimumSize;
    
    // Construtor
    // --------------------------------------------------

    public Engine(EngineOptions options)
    {
        WindowOptions windowOptions = WindowOptions.Default;

        windowOptions.Size = new Vector2D<int>(options.Size.X, options.Size.Y);
        _minimumSize = options.MinimumSize;
        windowOptions.Title = options.Title;
        windowOptions.Samples = options.Samples;

        windowOptions.IsVisible = false;

        _window = Window.Create(windowOptions);
    }

    // 
    // --------------------------------------------------

    public void Run()
    {
        _window.Load += () =>
        {
            _window.Center();
            _window.IsVisible = true;

            _gl = _window.CreateOpenGL();
            GL = _gl;

            Screen.Initialize(_window);
            Input.Initialize(_window);

            _gl.ClearColor(Color.LightSkyBlue);

            _gl.Enable(EnableCap.DepthTest);
            _gl.Enable(EnableCap.CullFace);

            // _gl.Enable(EnableCap.Blend);
            // _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            OnLoad();
        };

        _window.Resize += newSize =>
        {
            if (Screen.Width < _minimumSize.X)
            {
                _window.Size = new Vector2D<int>(_minimumSize.X, _window.Size.Y);
            }
            if (Screen.Height < _minimumSize.Y)
            {
                _window.Size = new Vector2D<int>(_window.Size.X, _minimumSize.Y);
            }

            _gl.Viewport(0, 0, (uint)Screen.Width, (uint)Screen.Height);

            OnResize(newSize);
        };

        _window.Update += deltaTime =>
        {
            Time.Update(deltaTime);
            Input.NewFrame();

            OnUpdate(deltaTime);
        };

        _window.Render += deltaTime =>
        {
            _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            OnRender(deltaTime);
        };

        _window.Closing += () =>
        {
            OnClosing();
        };

        // --------------------------------------------------

        try
        {
            _window.Run();
        }
        catch (Exception ex)
        {
            Debug.LogError(
                "Falha ao criar a janela Silk.NET" + "\n\n" +
                ex + "\n\n" +
                " -- --------------------------------------------------- -- "
            );
        }
    }

    // 
    // --------------------------------------------------

    public void Close()
    {
        _window.Close();
    }
    
    // 
    // --------------------------------------------------
    
    protected virtual void OnLoad()
    {
        
    }
    
    protected virtual void OnResize(Vector2D<int> newSize)
    {
        
    }
    
    protected virtual void OnUpdate(double deltaTime)
    {
        
    }
    
    protected virtual void OnRender(double deltaTime)
    {
        
    }
    
    protected virtual void OnClosing()
    {
        
    }
}
