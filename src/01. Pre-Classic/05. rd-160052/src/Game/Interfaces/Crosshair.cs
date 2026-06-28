#region 
/*

wiki: https://minecraft.wiki/w/Icons.png

*/
#endregion

using GameEngine.Core;
using GameEngine.Interfaces;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using Silk.NET.OpenGL;

namespace RubyDung.Interfaces;

public class Crosshair
{
    private GL _gl = Engine.GL;

    private Image _image = null!;

    public Crosshair()
    {
        _image = new Image();

        _image.Width = 16.0f;
        _image.Height = 16.0f;

        _image.Texture = new Texture2D("icons");
        _image.UVRect = new Vector4(0.0f, 0.0f, 16.0f, 16.0f);

        _image.Load();
    }

    public void Draw(ShaderProgram shader)
    {
        // Ativa inversão de cores para o crosshair
        _gl.Enable(EnableCap.Blend);
        _gl.BlendFunc(BlendingFactor.OneMinusDstColor, BlendingFactor.Zero);

        _image.Draw(shader);

        // Restaura o padrão (sem inversão) para outros elementos da interface
        _gl.Disable(EnableCap.Blend);
    }
}
