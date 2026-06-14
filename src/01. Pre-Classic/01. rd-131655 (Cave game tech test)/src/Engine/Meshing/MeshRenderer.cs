using GameEngine.Core;
using GameEngine.Rendering;
using Silk.NET.OpenGL;

namespace GameEngine.Meshing;

public class MeshRenderer
{
    public Mesh Mesh
    {
        get => _mesh;
        set
        {
            _mesh = value;
            BuildMesh();
        }
    }

    // 
    // --------------------------------------------------

    private GL _gl = Engine.GL;
    private Mesh _mesh = null!;

    // 
    // --------------------------------------------------

    private float[] _vertices = [];
    private uint[] _indices = [];

    private uint _vertexArrayObject;
    private uint _vertexBufferObject;
    private uint _elementBufferObject;

    // 
    // --------------------------------------------------

    private uint _vertexStride;
    private uint _vertexPointer;

    private const int _sizePos = 3;
    private const int _sizeColor = 4;
    private const int _sizeTexCoord = 2;

    private bool _hasColor = false;
    private bool _hasTexture = false;

    private uint _vertexCount;
    private uint _indexCount;

    // renderizar a malha
    // --------------------------------------------------

    public void Draw(ShaderProgram shader)
    {
        shader.SetUniform("hasColor", _hasColor);
        shader.SetUniform("hasTexture", _hasTexture);

        _gl.BindVertexArray(_vertexArrayObject);

        DrawTrianglesFill(shader);
        DrawTrianglesLine(shader);

        _gl.BindVertexArray(0);
    }

    // Inicializa todos os objetos/arrays de buffer
    // --------------------------------------------------

    private void BuildMesh()
    {
        // Define o tamanho do Stride
        // --------------------------------------------------

        _vertexStride = _sizePos;

        _hasColor = _mesh.Colors?.Length > 0;
        if (_hasColor)
        {
            _vertexStride += _sizeColor;
        }

        _hasTexture = _mesh.TexCoords?.Length > 0;
        if (_hasTexture)
        {
            _vertexStride += _sizeTexCoord;
        }

        // Define os vertices
        // --------------------------------------------------

        _vertexPointer = 0;

        _vertexCount = (uint)_mesh.VertexCount;
        _vertices = new float[_vertexCount * _vertexStride];

        for (int i = 0; i < _vertexCount; i++)
        {
            _vertices[_vertexPointer++] = _mesh.Positions[i].X;
            _vertices[_vertexPointer++] = _mesh.Positions[i].Y;
            _vertices[_vertexPointer++] = _mesh.Positions[i].Z;

            if (_hasColor)
            {
                _vertices[_vertexPointer++] = _mesh.Colors![i].R;
                _vertices[_vertexPointer++] = _mesh.Colors![i].G;
                _vertices[_vertexPointer++] = _mesh.Colors![i].B;
                _vertices[_vertexPointer++] = _mesh.Colors![i].A;
            }
            if (_hasTexture)
            {
                _vertices[_vertexPointer++] = _mesh.TexCoords![i].U;
                _vertices[_vertexPointer++] = _mesh.TexCoords![i].V;
            }
        }

        // Define os indices
        // --------------------------------------------------

        _indexCount = (uint)_mesh.Indices.Length;
        _indices = new uint[_indexCount];

        for (int i = 0; i < _indexCount; i++)
        {
            _indices[i] = (uint)_mesh.Indices[i];
        }

        // 
        // --------------------------------------------------

        SetupMesh();
    }

    private void SetupMesh()
    {
        DeleteOldBuffers();

        // criar buffers/arrays
        // --------------------------------------------------

        _vertexArrayObject = _gl.GenVertexArray();
        _vertexBufferObject = _gl.GenBuffer();
        _elementBufferObject = _gl.GenBuffer();

        //
        // --------------------------------------------------

        _gl.BindVertexArray(_vertexArrayObject);

        // Carregar dados nos buffers de vértices
        // --------------------------------------------------

        // --- Vertex Buffer Object ---
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vertexBufferObject);
        unsafe
        {
            fixed (float* buf = _vertices)
            {
                _gl.BufferData(BufferTargetARB.ArrayBuffer, _vertexPointer * sizeof(float), buf, BufferUsageARB.StaticDraw);
            }
        }

        // --- Elemente Buffer Object ---
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _elementBufferObject);
        unsafe
        {
            fixed (uint* buf = _indices)
            {
                _gl.BufferData(BufferTargetARB.ElementArrayBuffer, _indexCount * sizeof(uint), buf, BufferUsageARB.StaticDraw);
            }
        }

        // Defina os ponteiros de atributos de vértice
        // --------------------------------------------------

        VertexAttribPointerType type = VertexAttribPointerType.Float;
        bool normalized = false;
        uint stride = _vertexStride * sizeof(float);
        int pointer = 0;

        // --- vertex positions ---
        const uint indexPos = 0; 

        _gl.EnableVertexAttribArray(indexPos);     

        unsafe
        {
            _gl.VertexAttribPointer(indexPos, _sizePos, type, normalized, stride, (void*)pointer);
        }

        pointer += _sizePos * sizeof(float);

        // --- vertex colors ---
        if (_hasColor)
        {
            const uint indexColor = 1;

            _gl.EnableVertexAttribArray(indexColor);

            unsafe
            {
                _gl.VertexAttribPointer(indexColor, _sizeColor, type, normalized, stride, (void*)pointer);
            }

            pointer += _sizeColor * sizeof(float);
        }

        // --- vertex texcoords ---
        if (_hasTexture)
        {
            const uint indexTexCoord = 2;

            _gl.EnableVertexAttribArray(indexTexCoord);

            unsafe
            {
                _gl.VertexAttribPointer(indexTexCoord, _sizeTexCoord, type, normalized, stride, (void*)pointer);
            }
        }

        //
        // --------------------------------------------------

        _gl.BindVertexArray(0);
    }
    
    //
    // --------------------------------------------------

    private void DrawTrianglesFill(ShaderProgram shader)
    {
        if (Engine.ShadingMode == ShadingMode.Shaded_Wireframe || 
            Engine.ShadingMode == ShadingMode.Shaded)
        {
            shader.SetUniform("hasWireframe", false);
            _gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);

            DrawTriangles();
        }
    }

    private void DrawTrianglesLine(ShaderProgram shader)
    {
        if (Engine.ShadingMode == ShadingMode.Shaded_Wireframe || 
            Engine.ShadingMode == ShadingMode.Wireframe)
        {
            shader.SetUniform("hasWireframe", true);
            _gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);

            _gl.Enable(EnableCap.PolygonOffsetLine);
            _gl.PolygonOffset(-1.0f, -1.0f);

            DrawTriangles();
        }
    }

    private void DrawTriangles()
    {
        unsafe
        {
            _gl.DrawElements(PrimitiveType.Triangles, _indexCount, DrawElementsType.UnsignedInt, (void*)0);
        }
    }
    
    //
    // --------------------------------------------------

    private void DeleteOldBuffers()
    {
        if (_vertexArrayObject != 0)
        {
            _gl.DeleteVertexArray(_vertexArrayObject);
            _gl.DeleteBuffer(_vertexBufferObject);
            _gl.DeleteBuffer(_elementBufferObject);

            _vertexArrayObject = 0;
            _vertexBufferObject = 0;
            _elementBufferObject = 0;
        }
    }
}
