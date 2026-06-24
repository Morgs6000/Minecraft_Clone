using GameEngine.Mathematics;
using GameEngine.Meshing;
using GameEngine.Rendering;
using GameEngine.Utilities;

namespace RubyDung.Interfaces;

public class GameModeSwitcher
{
    private Texture2D[] _texture = new Texture2D[2];
    private MeshRenderer[] _meshRenderer = new MeshRenderer[3];
    private Font _font = null!;

    private Vector3[] positions = [
        new Vector3(211.0f, 192.0f, 0.0f),
        new Vector3(241.0f, 192.0f, 0.0f),
        new Vector3(271.0f, 192.0f, 0.0f),
        new Vector3(301.0f, 192.0f, 0.0f)
    ];

    public GameModeSwitcher()
    {
        // texture
        // --------------------------------------------------

        _texture[0] = new Texture2D("gamemode_switcher");
        _texture[1] = new Texture2D("items");

        // mesh
        // --------------------------------------------------

        float w = Interface.ScreenWidth / 2;
        float h = Interface.ScreenHeight / 2;

        MeshQuad[] mesh = new MeshQuad[3];

        mesh[0] = new MeshQuad();
        mesh[0].Clear();

        // _textures[0].Bind();

        float largura_0 = 125.0f;
        float altura_0 = 75.0f;

        float x0_0 = w - (largura_0 / 2.0f);
        float y0_0 = h - (altura_0 / 2.0f);

        float x1_0 = w + (largura_0 / 2.0f);
        float y1_0 = h + (altura_0 / 2.0f);

        float u0_0 = 0.0f;
        float v0_0 = 0.0f;

        float u1_0 = u0_0 + (largura_0 / 128.0f);
        float v1_0 = v0_0 + (altura_0 / 128.0f);

        mesh[0].AddVertexWithUV(x0_0, y0_0, 0.0f, u0_0, v1_0);
        mesh[0].AddVertexWithUV(x1_0, y0_0, 0.0f, u1_0, v1_0);
        mesh[0].AddVertexWithUV(x1_0, y1_0, 0.0f, u1_0, v0_0);
        mesh[0].AddVertexWithUV(x0_0, y1_0, 0.0f, u0_0, v0_0);

        float largura_1 = 25.0f;
        float altura_1 = 25.0f;

        float x0_1 = w - (largura_1 / 2.0f);
        float y0_1 = h - (altura_1 / 2.0f);

        float x1_1 = w + (largura_1 / 2.0f);
        float y1_1 = h + (altura_1 / 2.0f);

        float u0_1 = 0.0f;
        float v0_1 = altura_0 / 128.0f;

        float u1_1 = u0_1 + (largura_1 / 128.0f);
        float v1_1 = v0_1 + (altura_1 / 128.0f);

        mesh[0].AddVertexWithUV(x0_1 - (largura_0 / 2) + 17.5f, y0_1, 0.0f, u0_1, v1_1);
        mesh[0].AddVertexWithUV(x1_1 - (largura_0 / 2) + 17.5f, y0_1, 0.0f, u1_1, v1_1);
        mesh[0].AddVertexWithUV(x1_1 - (largura_0 / 2) + 17.5f, y1_1, 0.0f, u1_1, v0_1);
        mesh[0].AddVertexWithUV(x0_1 - (largura_0 / 2) + 17.5f, y1_1, 0.0f, u0_1, v0_1);

        mesh[0].AddVertexWithUV(x0_1 - (largura_0 / 2) + 47.5f, y0_1, 0.0f, u0_1, v1_1);
        mesh[0].AddVertexWithUV(x1_1 - (largura_0 / 2) + 47.5f, y0_1, 0.0f, u1_1, v1_1);
        mesh[0].AddVertexWithUV(x1_1 - (largura_0 / 2) + 47.5f, y1_1, 0.0f, u1_1, v0_1);
        mesh[0].AddVertexWithUV(x0_1 - (largura_0 / 2) + 47.5f, y1_1, 0.0f, u0_1, v0_1);

        mesh[0].AddVertexWithUV(x0_1 + (largura_0 / 2) - 47.5f, y0_1, 0.0f, u0_1, v1_1);
        mesh[0].AddVertexWithUV(x1_1 + (largura_0 / 2) - 47.5f, y0_1, 0.0f, u1_1, v1_1);
        mesh[0].AddVertexWithUV(x1_1 + (largura_0 / 2) - 47.5f, y1_1, 0.0f, u1_1, v0_1);
        mesh[0].AddVertexWithUV(x0_1 + (largura_0 / 2) - 47.5f, y1_1, 0.0f, u0_1, v0_1);

        mesh[0].AddVertexWithUV(x0_1 + (largura_0 / 2) - 17.5f, y0_1, 0.0f, u0_1, v1_1);
        mesh[0].AddVertexWithUV(x1_1 + (largura_0 / 2) - 17.5f, y0_1, 0.0f, u1_1, v1_1);
        mesh[0].AddVertexWithUV(x1_1 + (largura_0 / 2) - 17.5f, y1_1, 0.0f, u1_1, v0_1);
        mesh[0].AddVertexWithUV(x0_1 + (largura_0 / 2) - 17.5f, y1_1, 0.0f, u0_1, v0_1);

        // Debug.Log(x0_1 + (largura_0 / 2) - 17.5f + (25.0f / 2.0f));

        _meshRenderer[0] = new MeshRenderer();
        _meshRenderer[0].Mesh = mesh[0];

        // _textures[1].Bind();

        mesh[1] = new MeshQuad();
        mesh[1].Clear();

        float x0_2 = w - (16.0f / 2.0f);
        float y0_2 = h - (16.0f / 2.0f);

        float x1_2 = w + (16.0f / 2.0f);
        float y1_2 = h + (16.0f / 2.0f);

        float u0_2 = 0.0f % 16.0f;
        float v0_2 = 0.0f / 16.0f;

        float u1_2 = u0_2 + (1.0f / 16.0f);
        float v1_2 = v0_2 + (1.0f / 16.0f);

        mesh[1].AddVertexWithUV(x0_2 - (largura_0 / 2) + 17.5f, y0_2, 0.0f, u0_2, v1_2);
        mesh[1].AddVertexWithUV(x1_2 - (largura_0 / 2) + 17.5f, y0_2, 0.0f, u1_2, v1_2);
        mesh[1].AddVertexWithUV(x1_2 - (largura_0 / 2) + 17.5f, y1_2, 0.0f, u1_2, v0_2);
        mesh[1].AddVertexWithUV(x0_2 - (largura_0 / 2) + 17.5f, y1_2, 0.0f, u0_2, v0_2);

        mesh[1].AddVertexWithUV(x0_2 - (largura_0 / 2) + 47.5f, y0_2, 0.0f, u0_2, v1_2);
        mesh[1].AddVertexWithUV(x1_2 - (largura_0 / 2) + 47.5f, y0_2, 0.0f, u1_2, v1_2);
        mesh[1].AddVertexWithUV(x1_2 - (largura_0 / 2) + 47.5f, y1_2, 0.0f, u1_2, v0_2);
        mesh[1].AddVertexWithUV(x0_2 - (largura_0 / 2) + 47.5f, y1_2, 0.0f, u0_2, v0_2);

        mesh[1].AddVertexWithUV(x0_2 + (largura_0 / 2) - 47.5f, y0_2, 0.0f, u0_2, v1_2);
        mesh[1].AddVertexWithUV(x1_2 + (largura_0 / 2) - 47.5f, y0_2, 0.0f, u1_2, v1_2);
        mesh[1].AddVertexWithUV(x1_2 + (largura_0 / 2) - 47.5f, y1_2, 0.0f, u1_2, v0_2);
        mesh[1].AddVertexWithUV(x0_2 + (largura_0 / 2) - 47.5f, y1_2, 0.0f, u0_2, v0_2);

        mesh[1].AddVertexWithUV(x0_2 + (largura_0 / 2) - 17.5f, y0_2, 0.0f, u0_2, v1_2);
        mesh[1].AddVertexWithUV(x1_2 + (largura_0 / 2) - 17.5f, y0_2, 0.0f, u1_2, v1_2);
        mesh[1].AddVertexWithUV(x1_2 + (largura_0 / 2) - 17.5f, y1_2, 0.0f, u1_2, v0_2);
        mesh[1].AddVertexWithUV(x0_2 + (largura_0 / 2) - 17.5f, y1_2, 0.0f, u0_2, v0_2);

        _meshRenderer[1] = new MeshRenderer();
        _meshRenderer[1].Mesh = mesh[1];

        // _textures[0].Bind();

        mesh[2] = new MeshQuad();
        mesh[2].Clear();

        float x0_3 = -largura_1 / 2.0f;
        float y0_3 = -altura_1 / 2.0f;

        float x1_3 = largura_1 / 2.0f;
        float y1_3 = altura_1 / 2.0f;

        float u0_3 = largura_1 / 128.0f;
        float v0_3 = altura_0 / 128.0f;

        float u1_3 = u0_3 + (largura_1 / 128.0f);
        float v1_3 = v0_3 + (altura_1 / 128.0f);

        mesh[2].AddVertexWithUV(x0_3, y0_3, 0.0f, u0_3, v1_3);
        mesh[2].AddVertexWithUV(x1_3, y0_3, 0.0f, u1_3, v1_3);
        mesh[2].AddVertexWithUV(x1_3, y1_3, 0.0f, u1_3, v0_3);
        mesh[2].AddVertexWithUV(x0_3, y1_3, 0.0f, u0_3, v0_3);

        _meshRenderer[2] = new MeshRenderer();
        _meshRenderer[2].Mesh = mesh[2];

        // font
        // --------------------------------------------------

        _font = new Font();

        _font.Mesh.Clear();

        string str0 = "Creative Mode";
        string str1 = "[ F4 ]";
        string str2 = "Next";

        // Debug.Log(Color.FromHex("#54FCFC").ToDecimalString());

        _font.DrawShadow(
            str0,
            w - (MeasureString(str0) / 2.0f),
            h + (altura_0 / 2.0f) - 8.0f - 7.0f,
            16777215
        );

        _font.DrawShadow(
            str1,
            w - (MeasureString($"{str1} {str2}") / 2),
            h - (altura_0 / 2.0f) + 5.0f,
            5569788
        );

        _font.DrawShadow(
            str2,
            (w - (MeasureString($"{str1} {str2}") / 2)) + MeasureString($"{str1} "),
            h - (altura_0 / 2.0f) + 5.0f,
            16777215
        );

        _font.MeshRenderer.Mesh = _font.Mesh;
    }

    public void Draw(ShaderProgram shader)
    {
        // mesh
        // --------------------------------------------------

        Matrix4x4 model = Matrix4x4.Identity;
        shader.SetUniform("model", model);

        _texture[0].Bind();
        _meshRenderer[0].Draw(shader);

        _texture[1].Bind();
        _meshRenderer[1].Draw(shader);

        model = Matrix4x4.Identity;
        model *= Matrix4x4.Translate(positions[0]);
        shader.SetUniform("model", model);

        _texture[0].Bind();
        _meshRenderer[2].Draw(shader);

        // font
        // --------------------------------------------------

        model = Matrix4x4.Identity;
        shader.SetUniform("model", model);

        _font.Texture.Bind();
        _font.MeshRenderer.Draw(shader);
    }
    
    // 
    // --------------------------------------------------    
    
    private int MeasureString(string str)
    {
        int width = 0;

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '&')
            {
                i++; // pula o próximo caractere (código de cor)
                
                continue;
            }
            width += _font.CharWidths[str[i]];   // torna o campo acessível ou use um método
        }

        return width;
    }
}
