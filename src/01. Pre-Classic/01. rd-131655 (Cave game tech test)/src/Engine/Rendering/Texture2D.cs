using GameEngine.Core;
using GameEngine.Utilities;
using Silk.NET.OpenGL;
using StbImageSharp;

namespace GameEngine.Rendering;

public class Texture2D
{
    public int Widht;
    public int Height;
    public byte[] Data = [];
    
    // 
    // --------------------------------------------------

    private GL _gl = Engine.GL;

    private uint _texture;
    
    // Construtor
    // --------------------------------------------------

    public Texture2D(string file)
    {
        string path = GetValidFormat($"res/Textures/{file}");

        SetupTexture(path);
    }

    //
    // --------------------------------------------------

    public void Bind(TextureUnit texture = TextureUnit.Texture0)
    {
        _gl.ActiveTexture(texture);
        _gl.BindTexture(TextureTarget.Texture2D, _texture);
    }
    
    //
    // --------------------------------------------------

    private void SetupTexture(string path)
    {
        _texture = _gl.GenTexture();
        _gl.BindTexture(TextureTarget.Texture2D, _texture);

        // Defina os parâmetros de envolvimento da textura.
        // --------------------------------------------------

        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        // definir parâmetros de filtragem de textura
        // --------------------------------------------------

        // StbImage.stbi_set_flip_vertically_on_load(1);

        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapLinear);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        byte[] buffer = File.ReadAllBytes(path);
        ImageResult image = ImageResult.FromMemory(buffer, ColorComponents.RedGreenBlueAlpha);

        Widht = image.Width;
        Height = image.Height;
        Data = image.Data;

        try
        {
            unsafe
            {
                fixed (byte* ptr = Data)
                {
                    _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)Widht, (uint)Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
                }
            }

            _gl.GenerateMipmap(TextureTarget.Texture2D);
        }
        catch (Exception ex)
        {
            Debug.LogError(
                "Falha ao carregar a textura."
                + "\n\n" + ex
                + "\n\n" + " -- --------------------------------------------------- -- "
            );
        }
    }
    
    //
    // --------------------------------------------------
    
    private string GetValidFormat(string path)
    {
        string[] externsions =
        {
            ".jpg", ".png", ".bmp", ".tga", ".psd", ".gif", ".hdr"
        };

        foreach (string? ext in externsions)
        {
            string fullPath = path + ext;

            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        return path;
    }
}
