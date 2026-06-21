using GameEngine.Mathematics;
using GameEngine.Meshing;
using GameEngine.Rendering;
using GameEngine.Utilities;

namespace RubyDung.Level;

public class Chunk
{
    public static int Updates;

    // 
    // --------------------------------------------------

    private World _world;

    private MeshCube _mesh = null!;
    private MeshRenderer _meshRenderer = null!;

    private readonly int _size = World.ChunkSize;

    private byte[] _blocks = [];

    private readonly Vector3Int _position;

    // Construtor
    // --------------------------------------------------

    public Chunk(World world, Vector3Int position)
    {
        _world = world;

        _mesh = new MeshCube();
        _meshRenderer = new MeshRenderer();

        _position = position;

        PopulateBlocks();
    }

    // 
    // --------------------------------------------------

    public void PopulateBlocks()
    {
        // Debug.Log($"PopulateBlock no chunk {_position}");

        _blocks = new byte[_size * _size * _size];

        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                for (int z = 0; z < _size; z++)
                {
                    int i = (y * _size + z) * _size + x;
                    _blocks[i] = (byte)(z + _position.Z <= World.Size.Z * 2 / 3 ? 1 : 0);
                }
            }
        }
    }

    // 
    // --------------------------------------------------

    public void SetupChunk()
    {
        Updates++;

        _mesh.Clear();

        //*
        Random random = new Random();

        _mesh.SetColors(
            (float)random.NextDouble(),
            (float)random.NextDouble(),
            (float)random.NextDouble()
        );
        //*/

        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                for (int z = 0; z < _size; z++)
                {
                    if (IsBlock(x, y, z))
                    {
                        SetupBlock(
                            x + _position.X,
                            y + _position.Y,
                            z + _position.Z
                        );
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

    public bool IsBlock(int x, int y, int z)
    {
        if (x >= 0 && x < _size &&
            y >= 0 && y < _size &&
            z >= 0 && z < _size)
        {
            int i = (y * _size + z) * _size + x;
            return _blocks[i] == 1;
        }

        return false;
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

    private bool IsSolidBlock(int x, int y, int z)
    {
        return _world.IsSolidBlock(x, y, z);
    }
}
