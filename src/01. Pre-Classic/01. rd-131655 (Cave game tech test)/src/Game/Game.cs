using GameEngine.Core;
using GameEngine.Inputs;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utils;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace RubyDung;

public class Game : Engine
{
    private ShaderProgram _shader = null!;

    private float[] _vertices =
    {
        // positions           // colors
        -0.5f, -0.5f,  0.0f,   1.0f, 0.0f, 0.0f, // 0
         0.5f, -0.5f,  0.0f,   0.0f, 1.0f, 0.0f, // 1
         0.5f,  0.5f,  0.0f,   0.0f, 0.0f, 1.0f, // 2
        -0.5f,  0.5f,  0.0f,   1.0f, 1.0f, 0.0f  // 3
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
            _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), (void*)0);
        }
        _gl.EnableVertexAttribArray(0);

        // color attribute
        unsafe
        {
            _gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), (void*)(3 * sizeof(float)));
        }
        _gl.EnableVertexAttribArray(1);

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
