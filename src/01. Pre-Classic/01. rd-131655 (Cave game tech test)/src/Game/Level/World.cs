using GameEngine.Mathematics;
using GameEngine.Rendering;

namespace RubyDung.Level;

public class World
{
    public static readonly int ChunkSize = 16;

    public static Vector3Int Size = new Vector3Int(256, 256, 64);

    // 
    // --------------------------------------------------
    
    private Chunk[] _chunks;

    private int _xChunks;
    private int _yChunks;
    private int _zChunks; // altura do mundo

    // Construtor
    // --------------------------------------------------

    public World()
    {
        _xChunks = Size.X / ChunkSize;
        _yChunks = Size.Y / ChunkSize;
        _zChunks = Size.Z / ChunkSize;

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
}
