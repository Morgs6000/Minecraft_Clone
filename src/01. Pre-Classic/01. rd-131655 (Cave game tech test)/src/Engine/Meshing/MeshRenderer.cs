using GameEngine.Core;
using GameEngine.Rendering;
using Silk.NET.OpenGL;

namespace GameEngine.Meshing;

/// <summary>
/// Responsável por configurar e renderizar uma malha (<see cref="Mesh"/>) usando OpenGL.
/// Gerencia buffers de vértices, índices e estados de atributos.
/// </summary>
public class MeshRenderer
{
    /// <summary>
    /// Malha associada a este renderizador.
    /// Ao definir, automaticamente reconstrói os buffers internos.
    /// </summary>
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

    private GL _gl = Engine.GL;           // Contexto OpenGL
    private Mesh _mesh = null!;           // Malha atual

    // 
    // --------------------------------------------------

    // Arrays intercalados de vértices e índices enviados para a GPU
    private float[] _vertices = [];
    private uint[] _indices = [];

    // Handles dos objetos OpenGL
    private uint _vertexArrayObject;      // VAO
    private uint _vertexBufferObject;     // VBO
    private uint _elementBufferObject;    // EBO (índices)

    // 
    // --------------------------------------------------

    private uint _vertexStride;           // Tamanho total de um vértice em floats
    private uint _vertexPointer;          // Contador auxiliar para preencher _vertices

    // Tamanhos fixos (em floats) de cada componente do vértice
    private const int _sizePos = 3;       // x, y, z
    private const int _sizeColor = 4;     // r, g, b, a
    private const int _sizeTexCoord = 2;  // u, v

    // Flags que indicam se a malha possui cores e coordenadas de textura
    private bool _hasColor = false;
    private bool _hasTexture = false;

    private uint _vertexCount;            // Número de vértices na malha
    private uint _indexCount;             // Número de índices

    // Renderizar a malha
    // --------------------------------------------------

    /// <summary>
    /// Desenha a malha utilizando o shader fornecido.
    /// Aplica preenchimento sólido e/ou wireframe conforme o modo de sombreamento atual.
    /// </summary>
    /// <param name="shader">Programa shader utilizado para o desenho</param>
    public void Draw(ShaderProgram shader, bool applyWireframe = true)
    {
        // Informa ao shader se existem atributos de cor e textura
        shader.SetUniform("hasColor", _hasColor);
        shader.SetUniform("hasTexture", _hasTexture);

        // Ativa o VAO
        _gl.BindVertexArray(_vertexArrayObject);

        // Desenha triângulos preenchidos e/ou linhas de wireframe
        DrawTrianglesFill(shader, applyWireframe);
        DrawTrianglesLine(shader, applyWireframe);

        // Desvincula o VAO
        _gl.BindVertexArray(0);
    }

    // Inicializa todos os objetos/arrays de buffer
    // --------------------------------------------------

    /// <summary>
    /// Reconstrói os buffers de vértices e índices a partir da malha atual.
    /// Calcula o stride, intercala os dados e configura o VAO/VBO/EBO.
    /// </summary>
    private void BuildMesh()
    {
        // Define o tamanho do Stride (quantos floats por vértice)
        // --------------------------------------------------

        _vertexStride = _sizePos; // Posição é obrigatória

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

        // Define os vértices (intercala posições, cores e coordenadas UV)
        // --------------------------------------------------

        _vertexPointer = 0;

        _vertexCount = (uint)_mesh.VertexCount;
        _vertices = new float[_vertexCount * _vertexStride];

        for (int i = 0; i < _vertexCount; i++)
        {
            // Posição
            _vertices[_vertexPointer++] = _mesh.Positions[i].X;
            _vertices[_vertexPointer++] = _mesh.Positions[i].Y;
            _vertices[_vertexPointer++] = _mesh.Positions[i].Z;

            // Cor (se disponível)
            if (_hasColor)
            {
                _vertices[_vertexPointer++] = _mesh.Colors![i].R;
                _vertices[_vertexPointer++] = _mesh.Colors![i].G;
                _vertices[_vertexPointer++] = _mesh.Colors![i].B;
                _vertices[_vertexPointer++] = _mesh.Colors![i].A;
            }

            // Coordenadas de textura (se disponível)
            if (_hasTexture)
            {
                _vertices[_vertexPointer++] = _mesh.TexCoords![i].U;
                _vertices[_vertexPointer++] = _mesh.TexCoords![i].V;
            }
        }

        // Define os índices (convertendo para uint)
        // --------------------------------------------------

        _indexCount = (uint)_mesh.Indices.Length;
        _indices = new uint[_indexCount];

        for (int i = 0; i < _indexCount; i++)
        {
            _indices[i] = (uint)_mesh.Indices[i];
        }

        // Configura os objetos OpenGL (VAO, VBO, EBO)
        // --------------------------------------------------

        SetupMesh();
    }

    /// <summary>
    /// Cria e configura o VAO, VBO e EBO, enviando os dados para a GPU
    /// e definindo os ponteiros de atributos dos vértices.
    /// </summary>
    private void SetupMesh()
    {
        // Libera recursos OpenGL de buffers/VBOS anteriores antes de criar novos,
        // evitando vazamento de memória na GPU quando a malha é reconstruída.
        DeleteOldBuffers();

        // Criar buffers/arrays OpenGL
        // --------------------------------------------------

        _vertexArrayObject = _gl.GenVertexArray();
        _vertexBufferObject = _gl.GenBuffer();
        _elementBufferObject = _gl.GenBuffer();

        //
        // --------------------------------------------------

        _gl.BindVertexArray(_vertexArrayObject);

        // Carregar dados nos buffers de vértices
        // --------------------------------------------------

        // --- Vertex Buffer Object (VBO) ---
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vertexBufferObject);
        unsafe
        {
            fixed (float* buf = _vertices)
            {
                // Envia os dados intercalados para o VBO
                _gl.BufferData(BufferTargetARB.ArrayBuffer, _vertexPointer * sizeof(float), buf, BufferUsageARB.StaticDraw);
            }
        }

        // --- Element Buffer Object (EBO) ---
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _elementBufferObject);
        unsafe
        {
            fixed (uint* buf = _indices)
            {
                // Envia os índices dos triângulos
                _gl.BufferData(BufferTargetARB.ElementArrayBuffer, _indexCount * sizeof(uint), buf, BufferUsageARB.StaticDraw);
            }
        }

        // Defina os ponteiros de atributos de vértice
        // --------------------------------------------------

        VertexAttribPointerType type = VertexAttribPointerType.Float;
        bool normalized = false;
        uint stride = _vertexStride * sizeof(float); // Distância entre vértices consecutivos (em bytes)
        int pointer = 0; // Offset inicial dentro do vértice (em bytes)

        // --- Atributo 0: posições dos vértices ---
        const uint indexPos = 0;

        _gl.EnableVertexAttribArray(indexPos);

        unsafe
        {
            _gl.VertexAttribPointer(indexPos, _sizePos, type, normalized, stride, (void*)pointer);
        }

        pointer += _sizePos * sizeof(float); // Avança para o próximo componente

        // --- Atributo 1: cores dos vértices ---
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

        // --- Atributo 2: coordenadas de textura ---
        if (_hasTexture)
        {
            const uint indexTexCoord = 2;

            _gl.EnableVertexAttribArray(indexTexCoord);

            unsafe
            {
                _gl.VertexAttribPointer(indexTexCoord, _sizeTexCoord, type, normalized, stride, (void*)pointer);
            }
        }

        // Desvincula o VAO (bom hábito)
        // --------------------------------------------------

        _gl.BindVertexArray(0);
    }

    //
    // --------------------------------------------------

    /// <summary>
    /// Desenha os triângulos preenchidos caso o modo de sombreamento
    /// seja <see cref="ShadingMode.Shaded_Wireframe"/> ou <see cref="ShadingMode.Shaded"/>.
    /// </summary>
    private void DrawTrianglesFill(ShaderProgram shader, bool applyWireframe)
    {
        if (Engine.ShadingMode == ShadingMode.Shaded_Wireframe ||
            Engine.ShadingMode == ShadingMode.Shaded ||
            !applyWireframe)
        {
            shader.SetUniform("hasWireframe", false);
            _gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);

            DrawTriangles();
        }
    }

    /// <summary>
    /// Desenha as linhas de wireframe sobrepostas (ou sozinhas)
    /// caso o modo de sombreamento seja <see cref="ShadingMode.Shaded_Wireframe"/> ou <see cref="ShadingMode.Wireframe"/>.
    /// </summary>
    private void DrawTrianglesLine(ShaderProgram shader, bool applyWireframe)
    {
        if (!applyWireframe)
        {
            return;
        }
        if (Engine.ShadingMode == ShadingMode.Shaded_Wireframe ||
            Engine.ShadingMode == ShadingMode.Wireframe)
        {
            shader.SetUniform("hasWireframe", true);
            _gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);

            // Aplica um deslocamento de polígono para evitar z-fighting entre preenchimento e linha
            _gl.Enable(EnableCap.PolygonOffsetLine);
            _gl.PolygonOffset(-1.0f, -1.0f);

            DrawTriangles();
        }
    }

    /// <summary>
    /// Executa a chamada de desenho indexado (glDrawElements).
    /// </summary>
    private void DrawTriangles()
    {
        unsafe
        {
            _gl.DrawElements(PrimitiveType.Triangles, _indexCount, DrawElementsType.UnsignedInt, (void*)0);
        }
    }
    
    //
    // --------------------------------------------------

    /// <summary>
    /// Remove os buffers OpenGL (VAO, VBO, EBO) associados a esta instância,
    /// liberando os recursos de GPU. É chamado antes de criar novos buffers
    /// para evitar acúmulo de objetos não utilizados.
    /// </summary>
    private void DeleteOldBuffers()
    {
        // Só exclui se o VAO foi realmente criado (handle diferente de zero)
        if (_vertexArrayObject != 0)
        {
            // Exclui o Vertex Array Object e os dois buffers
            _gl.DeleteVertexArray(_vertexArrayObject);
            _gl.DeleteBuffer(_vertexBufferObject);
            _gl.DeleteBuffer(_elementBufferObject);

            // Reseta os handles para zero, indicando que não há mais recursos alocados
            _vertexArrayObject = 0;
            _vertexBufferObject = 0;
            _elementBufferObject = 0;
        }
    }
}
