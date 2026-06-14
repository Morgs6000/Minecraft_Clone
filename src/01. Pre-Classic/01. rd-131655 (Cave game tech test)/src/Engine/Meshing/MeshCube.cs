using GameEngine.Mathematics;

namespace GameEngine.Meshing;

public class MeshCube : MeshQuad
{
    public override MeshCube Default
    {
        get
        {
            Clear();

            float x0 = -0.5f;
            float y0 = -0.5f;
            float z0 = -0.5f;

            float x1 = 0.5f;
            float y1 = 0.5f;
            float z1 = 0.5f;

            AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y1, z0),
                    new Vector3(x0, y0, z0),
                    new Vector3(x0, y0, z1),
                    new Vector3(x0, y1, z1)
                }
            );

            AddQuad(
                new Vector3[]
                {
                    new Vector3(x1, y0, z0),
                    new Vector3(x1, y1, z0),
                    new Vector3(x1, y1, z1),
                    new Vector3(x1, y0, z1)
                }
            );

            AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y0, z0),
                    new Vector3(x1, y0, z0),
                    new Vector3(x1, y0, z1),
                    new Vector3(x0, y0, z1)
                }
            );

            AddQuad(
                new Vector3[]
                {
                    new Vector3(x1, y1, z0),
                    new Vector3(x0, y1, z0),
                    new Vector3(x0, y1, z1),
                    new Vector3(x1, y1, z1)
                }
            );

            AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y1, z0),
                    new Vector3(x1, y1, z0),
                    new Vector3(x1, y0, z0),
                    new Vector3(x0, y0, z0)
                }
            );

            AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y0, z1),
                    new Vector3(x1, y0, z1),
                    new Vector3(x1, y1, z1),
                    new Vector3(x0, y1, z1)
                }
            );

            return this;
        }
    }
    
    public override MeshCube DefaultWithUV
    {
        get
        {                   
            Clear();

            float x0 = -0.5f;
            float y0 = -0.5f;
            float z0 = -0.5f;

            float x1 = 0.5f;
            float y1 = 0.5f;
            float z1 = 0.5f;

            float u0 = 0.0f;
            float v0 = 0.0f;

            float u1 = 1.0f;
            float v1 = 1.0f;

            TexCoord[] texCoords = [
                new TexCoord(u0, v1),
                new TexCoord(u1, v1),
                new TexCoord(u1, v0),
                new TexCoord(u0, v0)
            ];

            AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y1, z0),
                    new Vector3(x0, y0, z0),
                    new Vector3(x0, y0, z1),
                    new Vector3(x0, y1, z1)
                },

                texCoords
            );

            AddQuad(
                new Vector3[]
                {
                    new Vector3(x1, y0, z0),
                    new Vector3(x1, y1, z0),
                    new Vector3(x1, y1, z1),
                    new Vector3(x1, y0, z1)
                },

                texCoords
            );

            AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y0, z0),
                    new Vector3(x1, y0, z0),
                    new Vector3(x1, y0, z1),
                    new Vector3(x0, y0, z1)
                },

                texCoords
            );

            AddQuad(
                new Vector3[]
                {
                    new Vector3(x1, y1, z0),
                    new Vector3(x0, y1, z0),
                    new Vector3(x0, y1, z1),
                    new Vector3(x1, y1, z1)
                },

                texCoords
            );

            AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y1, z0),
                    new Vector3(x1, y1, z0),
                    new Vector3(x1, y0, z0),
                    new Vector3(x0, y0, z0)
                },

                texCoords
            );

            AddQuad(
                new Vector3[]
                {
                    new Vector3(x0, y0, z1),
                    new Vector3(x1, y0, z1),
                    new Vector3(x1, y1, z1),
                    new Vector3(x0, y1, z1)
                },

                texCoords
            );   

            return this;
        }
    }
}
