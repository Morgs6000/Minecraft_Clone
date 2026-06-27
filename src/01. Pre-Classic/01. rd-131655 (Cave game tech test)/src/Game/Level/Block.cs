using GameEngine.Mathematics;
using GameEngine.Meshing;

namespace RubyDung.Level;

public class Block
{
    public static Block Rock = new Block(tex: 0);
    public static Block Grass = new Block(tex: 1);

    private int _tex = 0;

    private Block(int tex)
    {
        _tex = tex;
    }

    public void SetupBlock(MeshCube mesh, Chunk chunk, Vector3Int position)
    {
        int x = position.X;
        int y = position.Y;
        int z = position.Z;

        SetupBlock(mesh, chunk, x, y, z);
    }

    public void SetupBlock(MeshCube mesh, Chunk chunk, int x, int y, int z)
    {
        float x0 = (float)x + 0.0f;
        float y0 = (float)y + 0.0f;
        float z0 = (float)z + 0.0f;

        float x1 = (float)x + 1.0f;
        float y1 = (float)y + 1.0f;
        float z1 = (float)z + 1.0f;

        float c1 = 0.6f;
        float c2 = 0.8f;
        float c3 = 1.0f;

        float u0 = (float)_tex / 16.0f;
        float v0 = 0.0f;

        float u1 = u0 + (1.0f / 16.0f);
        float v1 = v0 + (1.0f / 16.0f);

        TexCoord[] texCoords = [
            new TexCoord(u0, v1),
            new TexCoord(u1, v1),
            new TexCoord(u1, v0),
            new TexCoord(u0, v0)
        ];

        if (!chunk.IsSolidBlock(x - 1, y, z))
        {
            float br = chunk.GetBrightness(x - 1, y, z) * c1;

            mesh.SetColors(br, br, br);

            mesh.AddQuad(
                [
                    new Vector3(x0, y1, z0),
                    new Vector3(x0, y0, z0),
                    new Vector3(x0, y0, z1),
                    new Vector3(x0, y1, z1)
                ],

                texCoords
            );
        }
        if (!chunk.IsSolidBlock(x + 1, y, z))
        {
            float br = chunk.GetBrightness(x + 1, y, z) * c1;
            
            mesh.SetColors(br, br, br);

            mesh.AddQuad(
                [
                    new Vector3(x1, y0, z0),
                    new Vector3(x1, y1, z0),
                    new Vector3(x1, y1, z1),
                    new Vector3(x1, y0, z1)
                ],

                texCoords
            );
        }
        if (!chunk.IsSolidBlock(x, y - 1, z))
        {
            float br = chunk.GetBrightness(x, y - 1, z) * c2;
            
            mesh.SetColors(br, br, br);

            mesh.AddQuad(
                [
                    new Vector3(x0, y0, z0),
                    new Vector3(x1, y0, z0),
                    new Vector3(x1, y0, z1),
                    new Vector3(x0, y0, z1)
                ],

                texCoords
            );
        }
        if (!chunk.IsSolidBlock(x, y + 1, z))
        {
            float br = chunk.GetBrightness(x, y + 1, z) * c2;
            
            mesh.SetColors(br, br, br);

            mesh.AddQuad(
                [                
                    new Vector3(x1, y1, z0),
                    new Vector3(x0, y1, z0),
                    new Vector3(x0, y1, z1),
                    new Vector3(x1, y1, z1)
                ],

                texCoords
            );
        }
        if (!chunk.IsSolidBlock(x, y, z - 1))
        {
            float br = chunk.GetBrightness(x, y, z - 1) * c3;
            
            mesh.SetColors(br, br, br);

            mesh.AddQuad(
                [
                    new Vector3(x0, y1, z0),
                    new Vector3(x1, y1, z0),
                    new Vector3(x1, y0, z0),
                    new Vector3(x0, y0, z0)
                ],

                texCoords
            );
        }
        if (!chunk.IsSolidBlock(x, y, z + 1))
        {
            float br = chunk.GetBrightness(x, y, z + 1) * c3;
            
            mesh.SetColors(br, br, br);

            mesh.AddQuad(
                [
                    new Vector3(x0, y0, z1),
                    new Vector3(x1, y0, z1),
                    new Vector3(x1, y1, z1),
                    new Vector3(x0, y1, z1)
                ],

                texCoords
            );
        }
    }
}
