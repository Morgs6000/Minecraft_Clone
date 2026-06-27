using GameEngine.Mathematics;
using GameEngine.Meshing;
using GameEngine.Rendering;

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
    private int[] _lightDepths = [];

    public List<LevelListener> _levelListerners = new List<LevelListener>();

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
        _blocks = new byte[_size * _size * _size];
        _lightDepths = new int[_world.Width * _world.Depth];

        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                for (int z = 0; z < _size; z++)
                {
                    int i = (y * _size + z) * _size + x;
                    _blocks[i] = (byte)(z + _position.Z <= _world.Height * 2 / 3 ? 1 : 0);
                }
            }
        }

        // CalcLightDepths(0, 0, _world.Width, _world.Depth);
    }

    // 
    // --------------------------------------------------

    public void SetupChunk()
    {
        Updates++;

        _mesh.Clear();

        // ApplyRandonColor();

        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                for (int z = 0; z < _size; z++)
                {
                    if (IsBlock(x, y, z))
                    {
                        int tex = z + _position.Z == _world.Height * 2 / 3 ? 0 : 1;

                        Vector3Int position = new Vector3Int(
                            x + _position.X,
                            y + _position.Y,
                            z + _position.Z
                        );

                        if (tex == 0)
                        {
                            Block.Rock.SetupBlock(_mesh, this, position);
                        }
                        else
                        {
                            Block.Grass.SetupBlock(_mesh, this, position);
                        }
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

    public bool IsSolidBlock(int x, int y, int z)
    {
        return _world.IsSolidBlock(x, y, z);
    }

    public void CalcLightDepths(int x0, int y0, int x1, int y1)
    {
        _world.CalcLightDepths(x0, y0, x1, y1);
    }

    public float GetBrightness(int x, int y, int z)
    {
        return _world.GetBrightness(x, y, z);
    }

    // 
    // --------------------------------------------------

    private void ApplyRandonColor()
    {
        Random random = new Random();

        _mesh.SetColors(
            (float)random.NextDouble(),
            (float)random.NextDouble(),
            (float)random.NextDouble()
        );
    }
}
