using GameEngine.Inputs;
using GameEngine.Mathematics;
using GameEngine.Meshing;
using GameEngine.Rendering;
using RubyDung.Blocks;
using RubyDung.Level;

namespace RubyDung.Interfaces;

public class SelectedBlock
{
    public static int PaintTextue = Block.Rock.ID;

    private Texture2D _texture = null!;
    private World _world = null!;
    private MeshRenderer _meshRenderer = null!;

    private int[] _selectableBlocks;

    public SelectedBlock(World world)
    {
        _world = world;

        _selectableBlocks = Block.Blocks
            .Where(b => b != null && b.ID != Block.Grass.ID)
            .Select(b => b.ID)
            .ToArray();

        _texture = new Texture2D("terrain");

        Selected();
    }

    public void Update()
    {
        int previous = PaintTextue;

        if (Input.GetKeyDown(KeyCode.Number1))
        {
            PaintTextue = Block.Rock.ID;
        }
        if (Input.GetKeyDown(KeyCode.Number2))
        {
            PaintTextue = Block.Dirt.ID;
        }
        if (Input.GetKeyDown(KeyCode.Number3))
        {
            PaintTextue = Block.StoneBrick.ID;
        }
        if (Input.GetKeyDown(KeyCode.Number4))
        {
            PaintTextue = Block.Wood.ID;
        }

        int scroll = Mathf.FloorToInt(Input.MouseScrollDelta.Y);

        if (scroll != 0)
        {
            int currentIndex = Array.IndexOf(_selectableBlocks, PaintTextue);
            if (currentIndex == -1)
            {
                currentIndex = 0;
            }

            currentIndex = (currentIndex - scroll) % _selectableBlocks.Length;
            if (currentIndex < 0) currentIndex += _selectableBlocks.Length;

            PaintTextue = _selectableBlocks[currentIndex];
        }
        if (previous != PaintTextue)
        {
            Selected();
        }
    }

    public void Draw(ShaderProgram shader)
    {
        Matrix4x4 model = Matrix4x4.Identity;

        // Centraliza o bloco no próprio eixo antes de rodar (se necessário)
        model *= Matrix4x4.Translate(1.5f, -0.5f, -0.5f);

        // Converte Z‑up → Y‑up (rotação de -90° em torno de X)
        model *= Matrix4x4.Rotate(Vector3.PositiveX, Mathf.Radians(-90.0f));
        
        // Visualização isométrica (opcional)
        model *= Matrix4x4.Rotate(Vector3.PositiveY, Mathf.Radians(-45.0f));
        model *= Matrix4x4.Rotate(Vector3.PositiveX, Mathf.Radians(30.0f));

        // Escala (Tamanho do ícone na UI)
        model *= Matrix4x4.Scale(32.0f, 32.0f);

        float w = Interface.ScreenWidth;
        float h = Interface.ScreenHeight;

        // Posiciona na tela (Coordenadas de Pixel, já que a projeção é ortográfica)
        model *= Matrix4x4.Translate(w - 32.0f, h - 32.0f);

        shader.SetUniform("model", model);

        _texture.Bind();
        _meshRenderer.Draw(shader);
    }

    private void Selected()
    {
        MeshCube mesh = new MeshCube();

        mesh.Clear();

        Block.Blocks[PaintTextue].SetupBlock(mesh, _world, -2, 0, 0);

        _meshRenderer = new MeshRenderer();
        _meshRenderer.Mesh = mesh;
    }
}
