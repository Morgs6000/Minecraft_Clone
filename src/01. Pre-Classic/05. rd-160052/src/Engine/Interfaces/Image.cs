using GameEngine.Mathematics;
using GameEngine.Meshing;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung.Interfaces;

namespace GameEngine.Interfaces;

/// <summary>
/// Exibe uma Texture2D para o sistema de interface do usuário (UI).
/// </summary>
public class Image : RectTransform
{
    /// <summary>
    /// A textura do RawImage.
    /// </summary>
    public Texture2D? Texture;

    /// <summary>
    /// 
    /// </summary>
    public Color color = Color.White;

    /// <summary>
    /// As coordenadas de textura da RawImage.
    /// </summary>
    public Vector4 UVRect = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);

    // 
    // --------------------------------------------------

    private MeshRenderer? _meshRenderer;

    // Construtor
    // --------------------------------------------------

    public Image()
    {
        Width = 100;
        Height = 100;
    }

    // 
    // --------------------------------------------------

    public void Load()
    {
        MeshQuad mesh = new MeshQuad();

        mesh.Clear();

        float x0 = -0.5f;
        float y0 = -0.5f;

        float x1 = 0.5f;
        float y1 = 0.5f;

        mesh.AddVertex(x0, y0, 0.0f);
        mesh.AddVertex(x1, y0, 0.0f);
        mesh.AddVertex(x1, y1, 0.0f);
        mesh.AddVertex(x0, y1, 0.0f);

        if (Texture != null)
        {
            float u0;
            float v0;

            float u1;
            float v1;

            if (UVRect == new Vector4(0.0f, 0.0f, 1.0f, 1.0f))
            {
                u0 = UVRect.X;
                v0 = UVRect.Y;

                u1 = UVRect.Z;
                v1 = UVRect.W;
            }
            else
            {
                u0 = UVRect.X / Texture.Widht;
                v0 = UVRect.Y / Texture.Height;

                u1 = u0 + (UVRect.Z / Texture.Widht);
                v1 = v0 + (UVRect.W / Texture.Height);
            }

            mesh.AddVertexWithUV(x0, y0, 0.0f, u0, v1);
            mesh.AddVertexWithUV(x1, y0, 0.0f, u1, v1);
            mesh.AddVertexWithUV(x1, y1, 0.0f, u1, v0);
            mesh.AddVertexWithUV(x0, y1, 0.0f, u0, v0);
        }

        _meshRenderer = new MeshRenderer();
        _meshRenderer.Mesh = mesh;
    }

    // 
    // --------------------------------------------------

    public void Draw(ShaderProgram shader)
    {
        shader.SetUniform("uColor", (Vector4)color);

        float w = Interface.ScreenWidth;
        float h = Interface.ScreenHeight;

        float posX = w * Pivot.X + Width * (0.5f - Pivot.X);
        float posY = h * Pivot.Y + Height * (0.5f - Pivot.Y);

        Matrix4x4 model = Matrix4x4.Identity;
        // model *= Matrix4x4.Scale(Scale);
        model *= Matrix4x4.Scale(Width, Height, 0.0f);
        model *= Matrix4x4.Translate(Position);
        model *= Matrix4x4.Translate(posX, posY, 0.0f);

        shader.SetUniform("model", model);

        if (Texture != null)
        {
            Texture.Bind();
        }

        _meshRenderer!.Draw(shader);
    }
}
