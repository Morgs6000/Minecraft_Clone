using GameEngine.Mathematics;

namespace GameEngine.Meshing;

/// <summary>
/// Uma classe que permite criar ou modificar malhas a partir de scripts.
/// </summary>
public class Mesh
{
    /// <summary>
    /// Retorna uma cópia das posições dos vértices ou atribui uma nova matriz de posições dos vértices.
    /// </summary>
    public Vector3[] Positions = null!;

    /// <summary>
    /// Cores dos vértices da malha.
    /// </summary>
    public Color[] Colors = null!;

    /// <summary>
    /// As coordenadas de textura base da malha.
    /// </summary>
    public TexCoord[] TexCoords = null!;

    /// <summary>
    /// Uma matriz contendo todos os triângulos da malha.
    /// </summary>
    public int[] Indices = null!;

    /// <summary>
    /// Retorna o número de vértices na malha (somente leitura).
    /// </summary>
    public int VertexCount => _vertexCount != 0 ? _vertexCount : Positions.Length;
    
    // --------------------------------------------------

    protected int _vertexCount;
    protected int _indexCount;

    protected bool _hasColor = false;
    protected Color _color;

    protected bool _hasTexture = false;
    protected TexCoord _texCoord;

    // clear
    // --------------------------------------------------

    public void Clear()
    {
        Positions = Array.Empty<Vector3>();
        Colors = Array.Empty<Color>();
        TexCoords = Array.Empty<TexCoord>();
        Indices = Array.Empty<int>();

        _vertexCount = 0;
        _indexCount = 0;

        _hasColor = false;
        _hasTexture = false;
    }

    // positions
    // --------------------------------------------------

    /// <summary>
    /// Atribui um novo vetor de posições de vértices.
    /// </summary>
    /// <param name="inPositions">Posição por vértice.</param>
    public void SetPositions(Vector3[] inPositions)
    {
        Positions = inPositions;
    }

    /// <summary>
    /// Atribui um novo vetor de posições de vértices.
    /// </summary>
    /// <param name="inPositions">Posição por vértice.</param>
    public void SetPositions(List<Vector3> inPositions)
    {
        Positions = inPositions.ToArray();
    }

    // colors
    // --------------------------------------------------

    /// <summary>
    /// Cores dos vértices da malha.
    /// </summary>
    /// <param name="inColors">Cores por vértice.</param>
    public void SetColors(Color[] inColors)
    {
        Colors = inColors;
    }

    /// <summary>
    /// Cores dos vértices da malha.
    /// </summary>
    /// <param name="inColors">Cores por vértice.</param>
    public void SetColors(List<Color> inColors)
    {
        Colors = inColors.ToArray();
    }

    // texture coords
    // --------------------------------------------------

    /// <summary>
    /// Defina os valores UV.
    /// </summary>
    /// <param name="inTexCoords">Lista de coordenadas UV a serem definidas para o índice fornecido.</param>
    public void SetTexCoords(TexCoord[] inTexCoords)
    {
        TexCoords = inTexCoords;
    }

    /// <summary>
    /// Defina os valores UV.
    /// </summary>
    /// <param name="inTexCoords">Lista de coordenadas UV a serem definidas para o índice fornecido.</param>
    public void SetTexCoords(List<TexCoord> inTexCoords)
    {
        TexCoords = inTexCoords.ToArray();
    }

    // indices
    // --------------------------------------------------

    /// <summary>
    /// Define a lista de triângulos para a submalha.
    /// </summary>
    /// <param name="inIndices"></param>
    public void SetIndices(int[] inIndices)
    {
        Indices = inIndices;
    }

    /// <summary>
    /// Define a lista de triângulos para a submalha.
    /// </summary>
    /// <param name="inIndices"></param>
    public void SetIndices(List<int> inIndices)
    {
        Indices = inIndices.ToArray();
    }
}
