using GameEngine.Meshing;
using GameEngine.Rendering;

namespace RubyDung.Level;

public class Chunk
{
    private MeshCube _mesh = null!;
    private MeshRenderer _meshRenderer = null!;

    private readonly int _chunkSize = 16;

    // Construtor
    // --------------------------------------------------

    public Chunk()
    {
        _mesh = new MeshCube();
        _meshRenderer = new MeshRenderer();
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
                    _mesh.RenderFaceWithUV(x, y, z, MeshQuadFace.Negative_X);
                    _mesh.RenderFaceWithUV(x, y, z, MeshQuadFace.Positive_X);
                    _mesh.RenderFaceWithUV(x, y, z, MeshQuadFace.Negative_Y);
                    _mesh.RenderFaceWithUV(x, y, z, MeshQuadFace.Positive_Y);
                    _mesh.RenderFaceWithUV(x, y, z, MeshQuadFace.Negative_Z);
                    _mesh.RenderFaceWithUV(x, y, z, MeshQuadFace.Positive_Z);
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
}
