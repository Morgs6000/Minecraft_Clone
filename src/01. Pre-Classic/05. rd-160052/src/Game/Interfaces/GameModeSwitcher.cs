using GameEngine.Interfaces;
using GameEngine.Mathematics;
using GameEngine.Meshing;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung.Blocks;
using RubyDung.Level;

namespace RubyDung.Interfaces;

public class GameModeSwitcher
{
    private Texture2D _texture = null!;
    private World _world = null!;
    private Font _font = null!;

    private Image _backSprite = null!;
    private Image _highlight = null!;

    private Image[] _slot = new Image[4];
    private Image[] _icon = new Image[4];

    private MeshRenderer _meshRenderer = null!;

    public GameModeSwitcher(World world)
    {
        _world = world;

        BackSprite();
        Slots();
        Icons();
        Highlight();

        // font
        // --------------------------------------------------

        _font = new Font();
    }

    public void Update()
    {
        // _highlight
        // --------------------------------------------------

        switch (Game.Mode)
        {
            case GameMode.Creative:
                _highlight.Position = _slot[0].Position;
                break;
            case GameMode.Survival:
                _highlight.Position = _slot[1].Position;
                break;
            case GameMode.Adventure:
                _highlight.Position = _slot[2].Position;
                break;
            case GameMode.Spectator:
                _highlight.Position = _slot[3].Position;
                break;
        }

        // font
        // --------------------------------------------------

        float w = Interface.ScreenWidth / 2;
        float h = Interface.ScreenHeight / 2;

        // float largura_0 = 125.0f;
        float altura_0 = 75.0f;

        string str0 = $"{Game.Mode} Mode";
        string str1 = "[ F4 ]";
        string str2 = "Next";

        // Debug.Log(Color.FromHex("#54FCFC").ToDecimalString());

        _font.Clear();

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

        _font.FontRenderer();
    }

    public void Draw(ShaderProgram shader)
    {
        // mesh
        // --------------------------------------------------

        _backSprite.Draw(shader);

        _slot[0].Draw(shader);
        _slot[1].Draw(shader);
        _slot[2].Draw(shader);
        _slot[3].Draw(shader);

        // _icon[0].Draw(shader);
        _icon[1].Draw(shader);
        _icon[2].Draw(shader);
        _icon[3].Draw(shader);

        _highlight.Draw(shader);

        // 
        // --------------------------------------------------

        Matrix4x4 model = Matrix4x4.Identity;

        // Centraliza o bloco no próprio eixo antes de rodar (se necessário)
        model *= Matrix4x4.Translate(1.5f, -0.5f, -0.5f);

        // Converte Z‑up → Y‑up (rotação de -90° em torno de X)
        model *= Matrix4x4.Rotate(Vector3.PositiveX, Mathf.Radians(-90.0f));
        
        // Visualização isométrica (opcional)
        model *= Matrix4x4.Rotate(Vector3.PositiveY, Mathf.Radians(-45.0f));
        model *= Matrix4x4.Rotate(Vector3.PositiveX, Mathf.Radians(30.0f));

        // Escala (Tamanho do ícone na UI)
        model *= Matrix4x4.Scale(10.0f, 10.0f);

        float w = Interface.ScreenWidth / 2;
        float h = Interface.ScreenHeight / 2;

        // Posiciona na tela (Coordenadas de Pixel, já que a projeção é ortográfica)
        model *= Matrix4x4.Translate(w - 45, h);

        shader.SetUniform("model", model);

        _texture.Bind();
        _meshRenderer.Draw(shader);

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

    private void BackSprite()
    {
        // _backSprite
        // --------------------------------------------------

        _backSprite = new Image();

        _backSprite.Width = 125.0f;
        _backSprite.Height = 75.0f;

        _backSprite.Texture = new Texture2D("gamemode_switcher");
        _backSprite.UVRect = new Vector4(0.0f, 0.0f, 125.0f, 75.0f);

        _backSprite.Load();
    }

    private void Slots()
    {
        // _slot[0]
        // --------------------------------------------------

        _slot[0] = new Image();

        _slot[0].Position = new Vector3(-45.0f, 0.0f, 0.0f);

        _slot[0].Width = 25.0f;
        _slot[0].Height = 25.0f;

        _slot[0].Texture = new Texture2D("gamemode_switcher");
        _slot[0].UVRect = new Vector4(0.0f, 75.0f, 25.0f, 25.0f);

        _slot[0].Load();

        // _slot[1]
        // --------------------------------------------------

        _slot[1] = new Image();

        _slot[1].Position = new Vector3(-15.0f, 0.0f, 0.0f);

        _slot[1].Width = 25.0f;
        _slot[1].Height = 25.0f;

        _slot[1].Texture = new Texture2D("gamemode_switcher");
        _slot[1].UVRect = new Vector4(0.0f, 75.0f, 25.0f, 25.0f);

        _slot[1].Load();

        // _slot[2]
        // --------------------------------------------------

        _slot[2] = new Image();

        _slot[2].Position = new Vector3(15.0f, 0.0f, 0.0f);

        _slot[2].Width = 25.0f;
        _slot[2].Height = 25.0f;

        _slot[2].Texture = new Texture2D("gamemode_switcher");
        _slot[2].UVRect = new Vector4(0.0f, 75.0f, 25.0f, 25.0f);

        _slot[2].Load();

        // _slot[3]
        // --------------------------------------------------

        _slot[3] = new Image();

        _slot[3].Position = new Vector3(45.0f, 0.0f, 0.0f);

        _slot[3].Width = 25.0f;
        _slot[3].Height = 25.0f;

        _slot[3].Texture = new Texture2D("gamemode_switcher");
        _slot[3].UVRect = new Vector4(0.0f, 75.0f, 25.0f, 25.0f);

        _slot[3].Load();
    }

    private void Icons()
    {
        // _icon[0]
        // --------------------------------------------------

        /*
        _icon[0] = new Image();

        _icon[0].Position = _slot[0].Position;

        _icon[0].Width = 16.0f;
        _icon[0].Height = 16.0f;

        _icon[0].Texture = new Texture2D("items");
        _icon[0].UVRect = new Vector4(0.0f, 0.0f, 16.0f, 16.0f);

        _icon[0].Load();
        */

        _texture = new Texture2D("terrain");

        MeshCube mesh = new MeshCube();

        mesh.Clear();

        Block.Grass.SetupBlock(mesh, _world, -2, 0, 0);

        _meshRenderer = new MeshRenderer();
        _meshRenderer.Mesh = mesh;

        // _icon[2]
        // --------------------------------------------------

        _icon[1] = new Image();

        _icon[1].Position = _slot[1].Position;

        _icon[1].Width = 16.0f;
        _icon[1].Height = 16.0f;

        _icon[1].Texture = new Texture2D("items");
        _icon[1].UVRect = new Vector4(32.0f, 64.0f, 16.0f, 16.0f);

        _icon[1].Load();

        // _icon[2]
        // --------------------------------------------------

        _icon[2] = new Image();

        _icon[2].Position = _slot[2].Position;

        _icon[2].Width = 16.0f;
        _icon[2].Height = 16.0f;

        _icon[2].Texture = new Texture2D("items");
        _icon[2].UVRect = new Vector4(208.0f, 192.0f, 16.0f, 16.0f);

        _icon[2].Load();

        // _icon[3]
        // --------------------------------------------------

        _icon[3] = new Image();

        _icon[3].Position = _slot[3].Position;

        _icon[3].Width = 16.0f;
        _icon[3].Height = 16.0f;

        _icon[3].Texture = new Texture2D("items");
        _icon[3].UVRect = new Vector4(176.0f, 144.0f, 16.0f, 16.0f);

        _icon[3].Load();
    }
    
    private void Highlight()
    {
        // _highlight
        // --------------------------------------------------

        _highlight = new Image();

        _highlight.Position = _slot[0].Position;

        _highlight.Width = 25.0f;
        _highlight.Height = 25.0f;

        _highlight.Texture = new Texture2D("gamemode_switcher");
        _highlight.UVRect = new Vector4(25.0f, 75.0f, 25.0f, 25.0f);

        _highlight.Load();
    }
}
