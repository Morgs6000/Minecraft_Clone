using GameEngine.Mathematics;

namespace GameEngine.Meshing;

public class MeshQuad : Mesh
{
    public virtual MeshQuad Default
    {
        get
        {
            Clear();

            AddQuad(
                [
                    new Vector3(-0.5f,  0.0f, -0.5f),
                    new Vector3( 0.5f,  0.0f, -0.5f),
                    new Vector3( 0.5f,  0.0f,  0.5f),
                    new Vector3(-0.5f,  0.0f,  0.5f)
                ]
            );            

            return this;
        }
    }
    
    public virtual MeshQuad DefaultWithUV
    {
        get
        {
            Clear();

            AddQuad(
                [
                    new Vector3(-0.5f,  0.0f, -0.5f),
                    new Vector3( 0.5f,  0.0f, -0.5f),
                    new Vector3( 0.5f,  0.0f,  0.5f),
                    new Vector3(-0.5f,  0.0f,  0.5f)
                ],

                [
                    new TexCoord(0.0f, 1.0f),
                    new TexCoord(1.0f, 1.0f),
                    new TexCoord(1.0f, 0.0f),
                    new TexCoord(0.0f, 0.0f)
                ]
            );

            return this;
        }
    }

    // Add Quad
    // --------------------------------------------------

    public void AddQuad(Vector3[] positions)
    {
        for (int i = 0; i < 4; i++)
        {
            AddVertex(positions[i]);
        }
    }

    public void AddQuad(Vector3[] positions, Color[] colors)
    {
        for (int i = 0; i < 4; i++)
        {
            SetColors(colors[i]);
            AddVertex(positions[i]);
        }
    }

    public void AddQuad(Vector3[] positions, TexCoord[] texCoords)
    {
        for (int i = 0; i < 4; i++)
        {
            SetTexCoords(texCoords[i]);
            AddVertex(positions[i]);
        }
    }

    public void AddQuad(Vector3[] positions, Color[] colors, TexCoord[] texCoords)
    {
        for (int i = 0; i < 4; i++)
        {
            SetColors(colors[i]);
            SetTexCoords(texCoords[i]);
            AddVertex(positions[i]);
        }
    }

    // Add Vertex With UV
    // --------------------------------------------------

    public void AddVertexWithUV(float x, float y, float z, float u, float v)
    {
        SetTexCoords(u, v);
        AddVertex(x, y, z);
    }

    public void AddVertexWithUV(Vector3 positions, TexCoord texCoord)
    {
        SetTexCoords(texCoord);
        AddVertex(positions);
    }

    // Add Vertex
    // --------------------------------------------------

    public void AddVertex(float x, float y, float z)
    {
        AddVertex(new Vector3(x, y, z));
    }

    public void AddVertex(Vector3 positions)
    {
        // Define o tamanho do Array
        // --------------------------------------------------

        if (VertexCount >= Positions.Length)
        {
            int newSize = Positions.Length == 0 ? 4 : Positions.Length * 2;
            Array.Resize(ref Positions, newSize);

            if (_hasColor)
            {
                Array.Resize(ref Colors, newSize);
            }
            if (_hasTexture)
            {
                Array.Resize(ref TexCoords, newSize);
            }
        }

        // 
        // --------------------------------------------------

        Positions[_vertexCount] = positions;

        if (_hasColor)
        {
            Colors[_vertexCount] = _color;
        }
        if (_hasTexture)
        {
            TexCoords[_vertexCount] = _texCoord;
        }

        _vertexCount++;

        if (_vertexCount % 4 == 0)
        {
            SetIndices();
        }
    }

    // Set Colors
    // --------------------------------------------------

    public void SetColors(float r, float g, float b, float a = 1.0f)
    {
        SetColors(new Color(r, g, b, a));
    }

    public void SetColors(int r, int g, int b, int a = 255)
    {
        SetColors(new Color(r, g, b, a));
    }

    public void SetColors(Color color)
    {
        _hasColor = true;

        _color = color;
    }

    public void SetColors(int color)
    {
        float r = (float)(color >> 16 & 255) / 255.0f;
        float g = (float)(color >> 8 & 255) / 255.0f;
        float b = (float)(color & 255) / 255.0f;

        SetColors(new Color(r, g, b));
    }

    // Set TexCoords
    // --------------------------------------------------

    public void SetTexCoords(float u, float v)
    {
        SetTexCoords(new TexCoord(u, v));
    }

    public void SetTexCoords(TexCoord texCoord)
    {
        _hasTexture = true;

        _texCoord = texCoord;
    }

    // SetIndices
    // --------------------------------------------------
    
    private void SetIndices()
    {
        if (_indexCount + 6 > Indices.Length)
        {
            int newSize = Indices.Length == 0 ? 6 : Indices.Length * 2;
            Array.Resize(ref Indices, newSize);
        }

        int index = _vertexCount - 4;

        // primeiro triangulo
        Indices[_indexCount++] = 0 + index;
        Indices[_indexCount++] = 1 + index;
        Indices[_indexCount++] = 2 + index;

        // segundo triangulo
        Indices[_indexCount++] = 0 + index;
        Indices[_indexCount++] = 2 + index;
        Indices[_indexCount++] = 3 + index;
    }
}
