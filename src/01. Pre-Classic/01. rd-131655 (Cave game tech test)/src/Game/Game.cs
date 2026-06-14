using GameEngine.Core;
using GameEngine.Inputs;
using GameEngine.Utils;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace RubyDung;

public class Game : Engine
{
    private uint _shaderProgram;

    private float[] _vertices =
    {
        -0.5f, -0.5f,  0.0f,
         0.5f, -0.5f,  0.0f,
         0.0f,  0.5f,  0.0f
    };

    private uint _vertexArrayObject;
    private uint _vertexBufferObject;

    protected override void OnLoad()
    {
        // shader
        // --------------------------------------------------

        string vertexShaderSource = @"
            #version 330 core
            layout (location = 0) in vec3 aPos;

            void main()
            {
                gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
            }
        ";

        string fragmentShaderSource = @"
            #version 330 core
            out vec4 FragColor;

            void main()
            {
                FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
            }
        ";

        uint vertexShader;
        vertexShader = _gl.CreateShader(ShaderType.VertexShader);
        _gl.ShaderSource(vertexShader, vertexShaderSource);
        _gl.CompileShader(vertexShader);

        int success;
        string infoLog;

        _gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out success);
        if (success == 0)
        {
            infoLog = _gl.GetShaderInfoLog(vertexShader);
            Debug.LogError("ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" + infoLog);
        }

        uint fragmentShader;
        fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
        _gl.ShaderSource(fragmentShader, fragmentShaderSource);
        _gl.CompileShader(fragmentShader);

        _gl.GetShader(fragmentShader, ShaderParameterName.CompileStatus, out success);
        if (success == 0)
        {
            infoLog = _gl.GetShaderInfoLog(fragmentShader);
            Debug.LogError("ERROR::SHADER::FRAGMENT::COMPILATION_FAILED\n" + infoLog);
        }

        _shaderProgram = _gl.CreateProgram();
        _gl.AttachShader(_shaderProgram, vertexShader);
        _gl.AttachShader(_shaderProgram, fragmentShader);
        _gl.LinkProgram(_shaderProgram);

        _gl.GetProgram(_shaderProgram, ProgramPropertyARB.LinkStatus, out success);
        if (success == 0)
        {
            infoLog = _gl.GetProgramInfoLog(_shaderProgram);
            Debug.LogError("ERROR::SHADER::PROGRAM::LINKING_FAILED\n" + infoLog);
        }

        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);

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

        unsafe
        {
            _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), (void*)0);
        }
        _gl.EnableVertexAttribArray(0);
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

        _gl.UseProgram(_shaderProgram);

        // 
        // --------------------------------------------------

        _gl.BindVertexArray(_vertexArrayObject);
        _gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    protected override void OnClosing()
    {
        
    }
}
