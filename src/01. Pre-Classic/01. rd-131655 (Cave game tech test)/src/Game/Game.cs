using GameEngine.Core;
using GameEngine.Inputs;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utils;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using StbImageSharp;

namespace RubyDung;

public class Game : Engine
{
    private ShaderProgram _shader = null!;

    private uint _texture;

    private float[] _vertices =
    {
        // positions           // colors           // texture coords
        -0.5f, -0.5f,  0.0f,   1.0f, 0.0f, 0.0f,   0.0f, 1.0f, // 0
         0.5f, -0.5f,  0.0f,   0.0f, 1.0f, 0.0f,   1.0f, 1.0f, // 1
         0.5f,  0.5f,  0.0f,   0.0f, 0.0f, 1.0f,   1.0f, 0.0f, // 2
        -0.5f,  0.5f,  0.0f,   1.0f, 1.0f, 0.0f,   0.0f, 0.0f  // 3
    };

    private uint[] _indices =
    {
        0, 1, 2, // primeiro triangulo
        0, 2, 3  // segundo triangulo
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
    }

    protected override void OnRender(double deltaTime)
    {
        // shader
        // --------------------------------------------------

        _shader.Use();

        Matrix4x4 model = Matrix4x4.Identity;
        model *= Matrix4x4.Rotate(new Vector3(1.0f, 0.0f, 0.0f), Mathf.Radians(-55.0f));

        Matrix4x4 view = Matrix4x4.Identity;
        view *= Matrix4x4.Translate(new Vector3(0.0f, 0.0f, -3.0f));

        Matrix4x4 projection = Matrix4x4.Identity;
        projection *= Matrix4x4.Perspective(
            fov: Mathf.Radians(60.0f),
            aspect: (float)Screen.Width / (float)Screen.Height,
            zNear:  0.3f,
            zFar:   1000.0f
        );

        _shader.SetUniform("model", model);
        _shader.SetUniform("view", view);
        _shader.SetUniform("projection", projection);

        // texture
        // --------------------------------------------------
        
        _gl.BindTexture(TextureTarget.Texture2D, _texture);

        // 
        // --------------------------------------------------

        _gl.BindVertexArray(_vertexArrayObject);

        unsafe
        {
            _gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*)0);
        }

        _gl.BindVertexArray(0);
    }

    protected override void OnClosing()
    {
        
    }
}
