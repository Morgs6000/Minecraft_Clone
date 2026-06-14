using GameEngine.Core;
using GameEngine.Inputs;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utils;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using StbImageSharp;

namespace RubyDung;

public class Game : Engine
{
    private ShaderProgram _shader = null!;
    private Camera _camera = null!;

    private uint _texture;

    private float[] _vertices =
    {
        // positions           // colors           // texture coords
        -0.5f, -0.5f, -0.5f,   1.0f, 0.0f, 0.0f,   0.0f, 1.0f,         //  0
        -0.5f, -0.5f,  0.5f,   0.0f, 1.0f, 0.0f,   1.0f, 1.0f,         //  1
        -0.5f,  0.5f,  0.5f,   0.0f, 0.0f, 1.0f,   1.0f, 0.0f,         //  2
        -0.5f,  0.5f, -0.5f,   1.0f, 1.0f, 0.0f,   0.0f, 0.0f,         //  3
        
         0.5f, -0.5f,  0.5f,   1.0f, 0.0f, 0.0f,   0.0f, 1.0f,         //  4
         0.5f, -0.5f, -0.5f,   0.0f, 1.0f, 0.0f,   1.0f, 1.0f,         //  5
         0.5f,  0.5f, -0.5f,   0.0f, 0.0f, 1.0f,   1.0f, 0.0f,         //  6
         0.5f,  0.5f,  0.5f,   1.0f, 1.0f, 0.0f,   0.0f, 0.0f,         //  7
        
        -0.5f, -0.5f, -0.5f,   1.0f, 0.0f, 0.0f,   0.0f, 1.0f,         //  8
         0.5f, -0.5f, -0.5f,   0.0f, 1.0f, 0.0f,   1.0f, 1.0f,         //  9
         0.5f, -0.5f,  0.5f,   0.0f, 0.0f, 1.0f,   1.0f, 0.0f,         // 10
        -0.5f, -0.5f,  0.5f,   1.0f, 1.0f, 0.0f,   0.0f, 0.0f,         // 11
        
        -0.5f,  0.5f,  0.5f,   1.0f, 0.0f, 0.0f,   0.0f, 1.0f,         // 12
         0.5f,  0.5f,  0.5f,   0.0f, 1.0f, 0.0f,   1.0f, 1.0f,         // 13
         0.5f,  0.5f, -0.5f,   0.0f, 0.0f, 1.0f,   1.0f, 0.0f,         // 14
        -0.5f,  0.5f, -0.5f,   1.0f, 1.0f, 0.0f,   0.0f, 0.0f,         // 15
    
         0.5f, -0.5f, -0.5f,   1.0f, 0.0f, 0.0f,   0.0f, 1.0f,         // 16
        -0.5f, -0.5f, -0.5f,   0.0f, 1.0f, 0.0f,   1.0f, 1.0f,         // 17
        -0.5f,  0.5f, -0.5f,   0.0f, 0.0f, 1.0f,   1.0f, 0.0f,         // 18
         0.5f,  0.5f, -0.5f,   1.0f, 1.0f, 0.0f,   0.0f, 0.0f,         // 19
        
        -0.5f, -0.5f,  0.5f,   1.0f, 0.0f, 0.0f,   0.0f, 1.0f,         // 20
         0.5f, -0.5f,  0.5f,   0.0f, 1.0f, 0.0f,   1.0f, 1.0f,         // 21
         0.5f,  0.5f,  0.5f,   0.0f, 0.0f, 1.0f,   1.0f, 0.0f,         // 22
        -0.5f,  0.5f,  0.5f,   1.0f, 1.0f, 0.0f,   0.0f, 0.0f          // 23
    };

    private uint[] _indices =
    {
         0,  1,  2, // primeiro triangulo
         0,  2,  3, // segundo triangulo

         4,  5,  6, // primeiro triangulo
         4,  6,  7, // segundo triangulo

         8,  9, 10, // primeiro triangulo
         8, 10, 11, // segundo triangulo

        12, 13, 14, // primeiro triangulo
        12, 14, 15, // segundo triangulo

        16, 17, 18, // primeiro triangulo
        16, 18, 19, // segundo triangulo

        20, 21, 22, // primeiro triangulo
        20, 22, 23  // segundo triangulo
    };

    private uint _vertexArrayObject;
    private uint _vertexBufferObject;
    private uint _elementBufferObject;

    protected override void OnLoad()
    {
        // shader
        // --------------------------------------------------

        _shader = new ShaderProgram("base");

        // texture
        // --------------------------------------------------

        _texture = _gl.GenTexture();
        _gl.BindTexture(TextureTarget.Texture2D, _texture);

        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        byte[] buffer = File.ReadAllBytes("res/Textures/container.jpg");
        ImageResult image = ImageResult.FromMemory(buffer, ColorComponents.RedGreenBlueAlpha);

        try
        {
            unsafe
            {
                fixed (byte* ptr = image.Data)
                {
                    _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)image.Width, (uint)image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
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

        // 
        // --------------------------------------------------

        _vertexArrayObject = _gl.GenVertexArray();
        _gl.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vertexBufferObject);
        unsafe
        {
            fixed (float* buf = _vertices)
            {
                _gl.BufferData(BufferTargetARB.ArrayBuffer, (uint)(_vertices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);
            }
        }

        _elementBufferObject = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _elementBufferObject);
        unsafe
        {
            fixed (uint* buf = _indices)
            {
                _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (uint)(_indices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);
            }
        }

        // position attribute
        unsafe
        {
            _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*)0);
        }
        _gl.EnableVertexAttribArray(0);

        // color attribute
        unsafe
        {
            _gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*)(3 * sizeof(float)));
        }
        _gl.EnableVertexAttribArray(1);

        // texture attribute
        unsafe
        {
            _gl.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*)(6 * sizeof(float)));
        }
        _gl.EnableVertexAttribArray(2);

        // camera
        // --------------------------------------------------

        _camera = new Camera();

        // 
        // --------------------------------------------------

        // _gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
    }

    protected override void OnResize(Vector2D<int> newSize)
    {
        
    }

    protected override void OnUpdate(double deltaTime)
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Close();
        }

        // 
        // --------------------------------------------------

        _camera.Update();
    }

    protected override void OnRender(double deltaTime)
    {
        // shader
        // --------------------------------------------------

        _shader.Use();

        Matrix4x4 model = Matrix4x4.Identity;
        _shader.SetUniform("model", model);

        Matrix4x4 view = _camera.GetViewMatrix();
        _shader.SetUniform("view", view);

        Matrix4x4 projection = _camera.GetProjectionMatrix();
        _shader.SetUniform("projection", projection);

        // texture
        // --------------------------------------------------
        
        _gl.BindTexture(TextureTarget.Texture2D, _texture);

        // 
        // --------------------------------------------------

        _gl.BindVertexArray(_vertexArrayObject);

        unsafe
        {
            _gl.DrawElements(PrimitiveType.Triangles, (uint)_indices.Length, DrawElementsType.UnsignedInt, (void*)0);
        }

        _gl.BindVertexArray(0);
    }

    protected override void OnClosing()
    {
        
    }
}
