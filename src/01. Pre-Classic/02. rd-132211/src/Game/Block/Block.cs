using GameEngine.Mathematics;
using GameEngine.Meshing;
using RubyDung.Level;

namespace RubyDung.Blocks;

public class Block
{
    public static readonly Block[] Blocks = new Block[256];

    public static readonly Block Rock = new BlockRock(id: 1);
    public static readonly Block Grass = new BlockGrass(id: 2);

    public int ID { get; protected set; }
    public string TextualID { get; protected set; } = null!;

    protected int _tex = 0;

    protected Block(int id)
    {
        Blocks[id] = this;
        ID = id;
    }

    protected Block(int id, int tex) : this(id)
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
        float c1 = 0.6f;
        float c2 = 0.8f;
        float c3 = 1.0f;

        if (!chunk.IsSolidBlock(x - 1, y, z))
        {
            float br = chunk.GetBrightness(x - 1, y, z) * c1;

            mesh.SetColors(br, br, br);

            AddFaceWithUV(mesh, x, y, z, MeshQuadFace.Negative_X);
        }
        if (!chunk.IsSolidBlock(x + 1, y, z))
        {
            float br = chunk.GetBrightness(x + 1, y, z) * c1;

            mesh.SetColors(br, br, br);

            AddFaceWithUV(mesh, x, y, z, MeshQuadFace.Positive_X);
        }
        if (!chunk.IsSolidBlock(x, y - 1, z))
        {
            float br = chunk.GetBrightness(x, y - 1, z) * c2;

            mesh.SetColors(br, br, br);

            AddFaceWithUV(mesh, x, y, z, MeshQuadFace.Negative_Y);
        }
        if (!chunk.IsSolidBlock(x, y + 1, z))
        {
            float br = chunk.GetBrightness(x, y + 1, z) * c2;

            mesh.SetColors(br, br, br);

            AddFaceWithUV(mesh, x, y, z, MeshQuadFace.Positive_Y);
        }
        if (!chunk.IsSolidBlock(x, y, z - 1))
        {
            float br = chunk.GetBrightness(x, y, z - 1) * c3;

            mesh.SetColors(br, br, br);

            AddFaceWithUV(mesh, x, y, z, MeshQuadFace.Negative_Z);
        }
        if (!chunk.IsSolidBlock(x, y, z + 1))
        {
            float br = chunk.GetBrightness(x, y, z + 1) * c3;

            mesh.SetColors(br, br, br);

            AddFaceWithUV(mesh, x, y, z, MeshQuadFace.Positive_Z);
        }
    }

    public void AddFace(MeshCube mesh, int x, int y, int z, MeshQuadFace face)
    {
        float x0 = x + 0.0f;
        float y0 = y + 0.0f;
        float z0 = z + 0.0f;

        float x1 = x + 1.0f;
        float y1 = y + 1.0f;
        float z1 = z + 1.0f;

        if (face == MeshQuadFace.Negative_X)
        {
            mesh.AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y1, z0),
                    new Vector3(x0, y0, z0),
                    new Vector3(x0, y0, z1),
                    new Vector3(x0, y1, z1)
                }
            );
        }
        if (face == MeshQuadFace.Positive_X)
        {
            mesh.AddQuad(
                new Vector3[]
                {
                    new Vector3(x1, y0, z0),
                    new Vector3(x1, y1, z0),
                    new Vector3(x1, y1, z1),
                    new Vector3(x1, y0, z1)
                }
            );
        }
        if (face == MeshQuadFace.Negative_Y)
        {
            mesh.AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y0, z0),
                    new Vector3(x1, y0, z0),
                    new Vector3(x1, y0, z1),
                    new Vector3(x0, y0, z1)
                }
            );
        }
        if (face == MeshQuadFace.Positive_Y)
        {
            mesh.AddQuad(
                new Vector3[]
                {
                    new Vector3(x1, y1, z0),
                    new Vector3(x0, y1, z0),
                    new Vector3(x0, y1, z1),
                    new Vector3(x1, y1, z1)
                }
            );
        }
        if (face == MeshQuadFace.Negative_Z)
        {
            mesh.AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y1, z0),
                    new Vector3(x1, y1, z0),
                    new Vector3(x1, y0, z0),
                    new Vector3(x0, y0, z0)
                }
            );
        }
        if (face == MeshQuadFace.Positive_Z)
        {
            mesh.AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y0, z1),
                    new Vector3(x1, y0, z1),
                    new Vector3(x1, y1, z1),
                    new Vector3(x0, y1, z1)
                }
            );
        }
    }

    public void AddFaceWithUV(MeshCube mesh, int x, int y, int z, MeshQuadFace face)
    {
        float x0 = x + 0.0f;
        float y0 = y + 0.0f;
        float z0 = z + 0.0f;

        float x1 = x + 1.0f;
        float y1 = y + 1.0f;
        float z1 = z + 1.0f;

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

        if (face == MeshQuadFace.Negative_X)
        {
            mesh.AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y1, z0),
                    new Vector3(x0, y0, z0),
                    new Vector3(x0, y0, z1),
                    new Vector3(x0, y1, z1)
                },

                texCoords
            );
        }
        if (face == MeshQuadFace.Positive_X)
        {
            mesh.AddQuad(
                new Vector3[]
                {
                    new Vector3(x1, y0, z0),
                    new Vector3(x1, y1, z0),
                    new Vector3(x1, y1, z1),
                    new Vector3(x1, y0, z1)
                },

                texCoords
            );
        }
        if (face == MeshQuadFace.Negative_Y)
        {
            mesh.AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y0, z0),
                    new Vector3(x1, y0, z0),
                    new Vector3(x1, y0, z1),
                    new Vector3(x0, y0, z1)
                },

                texCoords
            );
        }
        if (face == MeshQuadFace.Positive_Y)
        {
            mesh.AddQuad(
                new Vector3[]
                {
                    new Vector3(x1, y1, z0),
                    new Vector3(x0, y1, z0),
                    new Vector3(x0, y1, z1),
                    new Vector3(x1, y1, z1)
                },

                texCoords
            );
        }
        if (face == MeshQuadFace.Negative_Z)
        {
            mesh.AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y1, z0),
                    new Vector3(x1, y1, z0),
                    new Vector3(x1, y0, z0),
                    new Vector3(x0, y0, z0)
                },

                texCoords
            );
        }
        if (face == MeshQuadFace.Positive_Z)
        {
            mesh.AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y0, z1),
                    new Vector3(x1, y0, z1),
                    new Vector3(x1, y1, z1),
                    new Vector3(x0, y1, z1)
                },

                texCoords
            );
        }
    }
}
