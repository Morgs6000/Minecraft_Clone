using GameEngine.Meshing;
using GameEngine.Rendering;

namespace RubyDung.Level;

public class Chunk
{
    private MeshCube _mesh = null!;
    private MeshRenderer _meshRenderer = null!;

    private readonly int _chunkSize = 16;

    private byte[] _blocks = [];

    // Construtor
    // --------------------------------------------------

    public Chunk()
    {
        _mesh = new MeshCube();
        _meshRenderer = new MeshRenderer();

        PopulateBlocks();
    }

    // 
    // --------------------------------------------------

    public void PopulateBlocks()
    {
        _blocks = new byte[_chunkSize * _chunkSize * _chunkSize];

        for (int x = 0; x < _chunkSize; x++)
        {
            for (int y = 0; y < _chunkSize; y++)
            {
                for (int z = 0; z < _chunkSize; z++)
                {
                    int i = (y * _chunkSize + z) * _chunkSize + x;
                    _blocks[i] = 1;
                }
            }
        }
    }

    // 
    // --------------------------------------------------

    public void SetupChunk()
    {
        _mesh.Clear();

        for (int x = 0; x < _chunkSize; x++)
        {
            for (int y = 0; y < _chunkSize; y++)
            {
                for (int z = 0; z < _chunkSize; z++)
                {
                    if (IsBlock(x, y, z))
                    {
                        SetupBlock(x, y, z);
                    }
                }
            }
        }

        _meshRenderer.Mesh = _mesh;
    }

    // 
    // --------------------------------------------------

    public void Draw(ShaderProgram shader)
    {
        _meshRenderer.Draw(shader);
    }

    // 
    // --------------------------------------------------

    private void SetupBlock(int x, int y, int z)
    {
        if (!IsSolidBlock(x - 1, y, z))
        {
            _mesh.RenderFaceWithUV(x, y, z, MeshQuadFace.Negative_X);
        }
        if (!IsSolidBlock(x + 1, y, z))
        {
            _mesh.RenderFaceWithUV(x, y, z, MeshQuadFace.Positive_X);
        }
        if (!IsSolidBlock(x, y - 1, z))
        {
            _mesh.RenderFaceWithUV(x, y, z, MeshQuadFace.Negative_Y);
        }
        if (!IsSolidBlock(x, y + 1, z))
        {
            _mesh.RenderFaceWithUV(x, y, z, MeshQuadFace.Positive_Y);
        }
        if (!IsSolidBlock(x, y, z - 1))
        {
            _mesh.RenderFaceWithUV(x, y, z, MeshQuadFace.Negative_Z);
        }
        if (!IsSolidBlock(x, y, z + 1))
        {
            _mesh.RenderFaceWithUV(x, y, z, MeshQuadFace.Positive_Z);
        }
    }

    private bool IsBlock(int x, int y, int z)
    {
        if (x >= 0 && x < _chunkSize &&
            y >= 0 && y < _chunkSize &&
            z >= 0 && z < _chunkSize)
        {
            int i = (y * _chunkSize + z) * _chunkSize + x;
            return _blocks[i] == 1;
        }

        return false;
    }

    private bool IsSolidBlock(int x, int y, int z)
    {
        return IsBlock(x, y, z);
    }
}
