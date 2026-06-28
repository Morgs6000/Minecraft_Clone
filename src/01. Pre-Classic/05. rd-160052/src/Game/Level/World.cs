using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung.Blocks;

namespace RubyDung.Level;

public class World : LevelListener
{
    public static readonly int ChunkSize = 16;

    public static int HeightLimit { get; private set; }
    public static bool OnHeightLimit { get; set; } = false;
    public static float HeightLimitStartTime { get; private set; } = 0.0f;

    // 
    // --------------------------------------------------

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
        HeightLimit = h;

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
        // Converte para índices de chunk: inferior com Floor, superior com Ceil
        int cx0 = Mathf.FloorToInt((float)x0 / ChunkSize);
        int cy0 = Mathf.FloorToInt((float)y0 / ChunkSize);
        int cz0 = Mathf.FloorToInt((float)z0 / ChunkSize);

        int cx1 = Mathf.CeilToInt((float)x1 / ChunkSize);
        int cy1 = Mathf.CeilToInt((float)y1 / ChunkSize);
        int cz1 = Mathf.CeilToInt((float)z1 / ChunkSize);

        // Limita os índices ao intervalo válido
        cx0 = Math.Max(cx0, 0);
        cy0 = Math.Max(cy0, 0);
        cz0 = Math.Max(cz0, 0);

        cx1 = Math.Min(cx1, _xChunks);
        cy1 = Math.Min(cy1, _yChunks);
        cz1 = Math.Min(cz1, _zChunks);

        // Reconstroi os chunks no intervalo [cx0, cx1) etc.
        for (int x = cx0; x < cx1; x++)
        {
            for (int y = cy0; y < cy1; y++)
            {
                for (int z = cz0; z < cz1; z++)
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
            x - ChunkSize, y - ChunkSize, z - ChunkSize,
            x + ChunkSize, y + ChunkSize, z + ChunkSize
        );
    }

    public void LightColumnChanged(int x, int y, int z0, int z1)
    {
        SetChunk(
            x - ChunkSize, y - ChunkSize, z0 - ChunkSize,
            x + ChunkSize, y + ChunkSize, z1 + ChunkSize
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

    public void Save()
    {
        foreach (Chunk chunk in _chunks)
        {
            chunk.Save();
        }
    }

    public int GetBlock(int x, int y, int z)
    {
        int cx = Mathf.FloorToInt((float)x / ChunkSize);
        int cy = Mathf.FloorToInt((float)y / ChunkSize);
        int cz = Mathf.FloorToInt((float)z / ChunkSize);

        if (cx >= 0 && cx < _xChunks &&
            cy >= 0 && cy < _yChunks &&
            cz >= 0 && cz < _zChunks)
        {
            int index = (cx + cy * _xChunks) * _zChunks + cz;
            Chunk chunk = _chunks[index];

            int localX = x - (cx * ChunkSize);
            int localY = y - (cy * ChunkSize);
            int localZ = z - (cz * ChunkSize);

            return chunk.GetBlock(localX, localY, localZ);
        }

        return 0;
    }

    public bool IsSolidBlock(int x, int y, int z)
    {
        // Converter coordenada global para índice do chunk
        int cx = Mathf.FloorToInt((float)x / ChunkSize);
        int cy = Mathf.FloorToInt((float)y / ChunkSize);
        int cz = Mathf.FloorToInt((float)z / ChunkSize);

        // Verificar se o índice está dentro dos limites do mundo
        if (cx < 0 || cx >= _xChunks ||
            cy < 0 || cy >= _yChunks ||
            cz < 0 || cz >= _zChunks)
        {
            return false;
        }

        // Obter o chunk correspondente
        int index = (cx + cy * _xChunks) * _zChunks + cz;
        Chunk chunk = _chunks[index];

        // Converter coordenada global para local do chunk
        int localX = x - (cx * ChunkSize);
        int localY = y - (cy * ChunkSize);
        int localZ = z - (cz * ChunkSize);

        // Chamar o método IsBlock do chunk (que verifica localmente)
        Block block = Block.Blocks[chunk.GetBlock(localX, localY, localZ)];
        
        return block != null ? block.IsSolid() : false;
    }   

    public void CalcLightDepths(int x0, int y0, int x1, int y1)
    {
        for (int x = x0; x < x0 + x1; x++)
        {
            for (int y = y0; y < y0 + y1; y++)
            {
                int i = x + y * Width;
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

        // float dark = 0.8f;
        float dark = 0.4f;

        if (x >= 0 && x < Width &&
            y >= 0 && y < Depth &&
            z >= 0 && z < Height)
        {
            int i = x + y * Width;
            return z < _lightDepths[i] ? dark : light;
        }

        return light;
    }
    
    public void SetBlock(int x, int y, int z, int blockID)
    {
        int cx = Mathf.FloorToInt((float)x / ChunkSize);
        int cy = Mathf.FloorToInt((float)y / ChunkSize);
        int cz = Mathf.FloorToInt((float)z / ChunkSize);

        if (cx >= 0 && cx < _xChunks &&
            cy >= 0 && cy < _yChunks &&
            cz >= 0 && cz < _zChunks)
        {
            int index = (cx + cy * _xChunks) * _zChunks + cz;
            Chunk chunk = _chunks[index];

            int localX = x - (cx * ChunkSize);
            int localY = y - (cy * ChunkSize);
            int localZ = z - (cz * ChunkSize);

            chunk.SetBlock(localX, localY, localZ, blockID);
            
            // Notifica os listeners para atualizar chunks vizinhos
            BlockChanged(x, y, z);
            // Também recalcula a luz na coluna
            CalcLightDepths(x, y, 1, 1);
        }
        if (z >= Height)
        {
            // Height limit for building is 64 blocks.
            // O limite de altura para construção é de 64 blocos.
            // Debug.LogError($"O limite de altura para construção é de {Height} blocos.");

            OnHeightLimit = true;

            HeightLimitStartTime = Time.ElapsedTime;

            return;
        }
    }
}
