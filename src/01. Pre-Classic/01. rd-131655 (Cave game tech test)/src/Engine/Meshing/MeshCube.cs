using GameEngine.Mathematics;

namespace GameEngine.Meshing;

public class MeshCube : MeshQuad
{
    public override MeshCube Default
    {
        get
        {
            Clear();

            float x = -0.5f;
            float y = -0.5f;
            float z = -0.5f;

            RenderFace(x, y, z, MeshQuadFace.Negative_X);
            RenderFace(x, y, z, MeshQuadFace.Positive_X);
            RenderFace(x, y, z, MeshQuadFace.Negative_Y);
            RenderFace(x, y, z, MeshQuadFace.Positive_Y);
            RenderFace(x, y, z, MeshQuadFace.Negative_Z);
            RenderFace(x, y, z, MeshQuadFace.Positive_Z);

            return this;
        }
    }

    public override MeshCube DefaultWithUV
    {
        get
        {
            Clear();

            float x = -0.5f;
            float y = -0.5f;
            float z = -0.5f;

            RenderFaceWithUV(x, y, z, MeshQuadFace.Negative_X);
            RenderFaceWithUV(x, y, z, MeshQuadFace.Positive_X);
            RenderFaceWithUV(x, y, z, MeshQuadFace.Negative_Y);
            RenderFaceWithUV(x, y, z, MeshQuadFace.Positive_Y);
            RenderFaceWithUV(x, y, z, MeshQuadFace.Negative_Z);
            RenderFaceWithUV(x, y, z, MeshQuadFace.Positive_Z);

            return this;
        }
    }

    public void RenderFace(float x, float y, float z, MeshQuadFace face)
    {
        
    }
    
    public void RenderFaceWithUV(float x, float y, float z, MeshQuadFace face)
    {
        float x0 = x + 0.0f;
        float y0 = y + 0.0f;
        float z0 = z + 0.0f;

        float x1 = x + 1.0f;
        float y1 = y + 1.0f;
        float z1 = z + 1.0f;

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

        if (face == MeshQuadFace.Negative_X)
        {
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
        }
        if (face == MeshQuadFace.Positive_X)
        {
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
        }
        if (face == MeshQuadFace.Negative_Y)
        {
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
        }
        if (face == MeshQuadFace.Positive_Y)
        {
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
        }
        if (face == MeshQuadFace.Negative_Z)
        {
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
        }
        if (face == MeshQuadFace.Positive_Z)
        {
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
        }
    }
}

public enum MeshQuadFace
{
    Negative_X,
    Positive_X,
    Negative_Y,
    Positive_Y,
    Negative_Z,
    Positive_Z
}
