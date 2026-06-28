using System.IO.Compression;
using GameEngine.Mathematics;
using GameEngine.Meshing;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung.Blocks;

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
                    Vector3Int position = new Vector3Int(
                        x + _position.X,
                        y + _position.Y,
                        z + _position.Z
                    );

                    int id = 0;

                    if (position.Z == _world.Height * 2 / 3)
                    {
                        id = Block.Grass.ID;
                    }
                    if (position.Z < _world.Height * 2 / 3)
                    {
                        id = Block.Rock.ID;
                    }

                    int i = (y * _size + z) * _size + x;
                    _blocks[i] = (byte)id;
                }
            }
        }

        if (File.Exists(GetChunkFilePath()))
        {
            Load();
        }
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
                    int blockID = GetBlock(x, y, z);

                    Vector3Int position = new Vector3Int(
                        x + _position.X,
                        y + _position.Y,
                        z + _position.Z
                    );

                    if (blockID > 0)
                    {
                        Block.Blocks[blockID].SetupBlock(_mesh, _world, position);
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

    public void Load()
    {
        string fileName = GetChunkFilePath();

        try
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            using (GZipStream gs = new GZipStream(fs, CompressionMode.Decompress))
            using (BinaryReader br = new BinaryReader(gs))
            {
                br.Read(_blocks, 0, _blocks.Length);

                // CalcLightDepths(0, 0, _size, _size);

                // foreach (LevelListener listener in _levelListerners)
                // {
                //     listener.AllChanged();
                // }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
    }

    public void Save()
    {
        string fileName = GetChunkFilePath();

        try
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            using (GZipStream gs = new GZipStream(fs, CompressionMode.Compress))
            using (BinaryWriter bw = new BinaryWriter(gs))
            {
                bw.Write(_blocks);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
    }
    
    private string GetChunkFilePath()
    {
        int x = _position.X / World.ChunkSize;
        int y = _position.Y / World.ChunkSize;
        int z = _position.Z / World.ChunkSize;

        // Cria o diretório: saves/X_Y/
        string saveDir = Path.Combine("saves", "New World", $"chunk_{x}_{y}");

        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir); // garante que a pasta existe
        }

        // Nome do arquivo: chunk_Z.dat
        return Path.Combine(saveDir, $"chunk_{z}.dat");
    }

    public int GetBlock(int x, int y, int z)
    {
        if (x >= 0 && x < _size &&
            y >= 0 && y < _size &&
            z >= 0 && z < _size)
        {
            int i = (y * _size + z) * _size + x;
            return _blocks[i];
        }

        return 0;
    }

    public bool IsSolidBlock(int x, int y, int z)
    {
        return _world.IsSolidBlock(x, y, z);
    }

    public void CalcLightDepths(int x0, int y0, int x1, int y1)
    {
        int globalX0 = x0 + _position.X;
        int globalY0 = y0 + _position.Y;

        _world.CalcLightDepths(globalX0, globalY0, x1, y1);
    }

    public float GetBrightness(int x, int y, int z)
    {
        return _world.GetBrightness(x, y, z);
    }

    public void SetBlock(int x, int y, int z, int type)
    {
        if (x >= 0 && x < _size &&
            y >= 0 && y < _size &&
            z >= 0 && z < _size)
        {
            int i = (y * _size + z) * _size + x;
            _blocks[i] = (byte)type;

            CalcLightDepths(x, y, 1, 1);

            int globalX = x + _position.X;
            int globalY = y + _position.Y;
            int globalZ = z + _position.Z;

            for (int j = 0; j < _levelListerners.Count; j++)
            {
                _levelListerners[j].BlockChanged(globalX, globalY, globalZ);
            }
        }
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
