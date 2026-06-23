using GameEngine.Mathematics;
using GameEngine.Rendering;

namespace RubyDung.Level;

public class World : LevelListener
{
    public static readonly int ChunkSize = 16;

    public readonly int Width;
    public readonly int Depth;
    public readonly int Height;

    private int[] _lightDepths = [];

    public List<LevelListener> _levelListerners = new List<LevelListener>();

    // 
    // --------------------------------------------------
    
    private Chunk[] _chunks;

    private int _xChunks;
    private int _yChunks;
    private int _zChunks; // altura do mundo

    // Construtor
    // --------------------------------------------------

    public World(int w, int d, int h)
    {
        Width = w;
        Depth = d;
        Height = h;

        _lightDepths = new int[w * d];

        _xChunks = w / ChunkSize;
        _yChunks = d / ChunkSize;
        _zChunks = h / ChunkSize;

        _chunks = new Chunk[_xChunks * _yChunks * _zChunks];

        for (int x = 0; x < _xChunks; x++)
        {
            for (int y = 0; y < _yChunks; y++)
            {
                for (int z = 0; z < _zChunks; z++)
                {
                    Vector3Int worldPos = new Vector3Int(
                        x * ChunkSize,
                        y * ChunkSize,
                        z * ChunkSize
                    );

                    int i = (x + y * _xChunks) * _zChunks + z;
                    _chunks[i] = new Chunk(this, worldPos);
                }
            }
        }

        CalcLightDepths(0, 0, Width, Depth);
    }

    public void SetupWorld()
    {
        foreach (Chunk chunk in _chunks)
        {
            chunk.SetupChunk();
        }
    }

    public void Draw(ShaderProgram shader)
    {
        foreach (Chunk chunk in _chunks)
        {
            chunk.Draw(shader);
        }
    }

    // 
    // --------------------------------------------------

    public void SetChunk(int x0, int y0, int z0, int x1, int y1, int z1)
    {
        x0 /= 16;
        y0 /= 16;
        z0 /= 16;

        x1 /= 16;
        y1 /= 16;
        z1 /= 16;

        if (x0 < 0)
        {
            x0 = 0;
        }
        if (y0 < 0)
        {
            y0 = 0;
        }
        if (z0 < 0)
        {
            z0 = 0;
        }
        if (x1 >= _xChunks)
        {
            x1 = _xChunks - 1;
        }
        if (y1 >= _yChunks)
        {
            y1 = _yChunks - 1;
        }
        if (z1 >= _zChunks)
        {
            z1 = _zChunks - 1;
        }

        for (int x = x0; x < x1; x++)
        {
            for (int y = y0; y < y1; y++)
            {
                for (int z = z0; z < z1; z++)
                {
                    int i = (x + y * _xChunks) * _zChunks + z;
                    _chunks[i].SetupChunk();
                }
            }
        }
    }

    // 
    // --------------------------------------------------

    public void BlockChanged(int x, int y, int z)
    {
        SetChunk(
            x - 1, y - 1, z - 1,
            x + 1, y + 1, z + 1
        );
    }

    public void LightColumnChanged(int x, int y, int z0, int z1)
    {
        SetChunk(
            x - 1, y - 1, z0 - 1,
            x + 1, y + 1, z1 + 1
        );
    }

    public void AllChanged()
    {
        SetChunk(
            0, 0, 0,
            Width, Depth, Height
        );
    }

    // 
    // --------------------------------------------------

    public bool IsSolidBlock(int x, int y, int z)
    {
        // Converter coordenada global para índice do chunk
        int cx = (int)Math.Floor((float)x / ChunkSize);
        int cy = (int)Math.Floor((float)y / ChunkSize);
        int cz = (int)Math.Floor((float)z / ChunkSize);

        // Verificar se o índice está dentro dos limites do mundo
        if (cx < 0 || cx >= _xChunks ||
            cy < 0 || cy >= _yChunks ||
            cz < 0 || cz >= _zChunks)
            return false;

        // Obter o chunk correspondente
        int index = (cx + cy * _xChunks) * _zChunks + cz;
        Chunk chunk = _chunks[index];

        // Converter coordenada global para local do chunk
        int localX = x - (cx * ChunkSize);
        int localY = y - (cy * ChunkSize);
        int localZ = z - (cz * ChunkSize);

        // Chamar o método IsBlock do chunk (que verifica localmente)
        return chunk.IsBlock(localX, localY, localZ);
    }   

    public void CalcLightDepths(int x0, int y0, int x1, int y1)
    {
        for (int x = x0; x < x0 + x1; x++)
        {
            for (int y = y0; y < y0 + y1; y++)
            {
                int i = y + y * Width;
                int oldDepth = _lightDepths[i];

                int z;

                for (z = Height - 1; z > 0 && !IsSolidBlock(x, y, z); z--)
                {

                }

                _lightDepths[i] = z;

                if (oldDepth != z)
                {
                    int zl0 = oldDepth < z ? oldDepth : z;
                    int zl1 = oldDepth > z ? oldDepth : z;

                    for (int j = 0; j < _levelListerners.Count; j++)
                    {
                        _levelListerners[j].LightColumnChanged(x, y, zl0, zl1);
                    }
                }
            }
        }
    } 

    public float GetBrightness(int x, int y, int z)
    {
        float light = 1.0f;
        float dark = 0.8f;

        if (x >= 0 && x < Width &&
            y >= 0 && y < Depth &&
            z >= 0 && z < Height)
        {
            int i = x + y * Width;
            return z < _lightDepths[i] ? dark : light;
        }
        
        return light;
    }
}
