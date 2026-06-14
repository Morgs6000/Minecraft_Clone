using GameEngine.Utils;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace GameEngine.Core;

public class Engine
{
    private IWindow _window = null!;
    private GL _gl = null!;
    
    // Construtor
    // --------------------------------------------------

    public Engine()
    {
        WindowOptions options = WindowOptions.Default;

        options.Size = new Vector2D<int>(800, 600);
        options.Title = "Game";
        options.IsVisible = false;

        _window = Window.Create(options);
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

            Screen.Initialize(_window);

            OnLoad();
        };

        _window.Resize += newSize =>
        {
            _gl.Viewport(0, 0, (uint)Screen.Width, (uint)Screen.Height);

            OnResize(newSize);
        };

        _window.Update += deltaTime =>
        {
            Time.Update(deltaTime);

            OnUpdate(deltaTime);
        };

        _window.Render += deltaTime =>
        {
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
                "Falha ao criar a janela Silk.NET" 
                + "\n\n" + ex
                + "\n\n" + " -- --------------------------------------------------- -- "
            );
        }
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
