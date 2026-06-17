using GameEngine.Core;
using GameEngine.Inputs;
using GameEngine.Interfaces;
using GameEngine.Mathematics;
using GameEngine.Meshing;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung.Level;
using Silk.NET.Maths;

namespace RubyDung;

public class Game : Engine
{
    public static string Version = GetRootFolderName.NomeSemPrefixo;

    public static bool DebugScreen = true;

    // 
    // --------------------------------------------------

    private ShaderProgram _shader = null!;
    private Texture2D _texture = null!;

    // private MeshRenderer _meshRenderer = null!;
    private Chunk _chunk = null!;

    private Camera _camera = null!;
    private Interface _interface = null!;

    // Construtor
    // --------------------------------------------------

    public Game(EngineOptions options) : base(options)
    {
        
    }

    // 
    // --------------------------------------------------

    protected override void OnLoad()
    {
        // shader
        // --------------------------------------------------

        _shader = new ShaderProgram("base");

        // texture
        // --------------------------------------------------

        _texture = new Texture2D("container");

        // mesh
        // --------------------------------------------------

        // MeshCube mesh = new MeshCube().DefaultWithUV;

        // _meshRenderer = new MeshRenderer();
        // _meshRenderer.Mesh = mesh;

        _chunk = new Chunk();
        _chunk.SetupChunk();

        // camera
        // --------------------------------------------------

        _camera = new Camera();

        // interface
        // --------------------------------------------------

        _interface = new Interface();
        _interface.SetCamera(_camera);

        // 
        // --------------------------------------------------

        // _gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
    }

    protected override void OnResize(Vector2D<int> newSize)
    {
        
    }

    protected override void OnUpdate(double deltaTime)
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Close();
        }

        // 
        // --------------------------------------------------

        DebugHotkeys.Update();
        
        if (!DebugHotkeys.Pressed)
        {
            _camera.Update();
        }

        _interface.Update();
    }

    protected override void OnRender(double deltaTime)
    {
        // shader
        // --------------------------------------------------

        _shader.Use();

        Matrix4x4 model = Matrix4x4.Identity;
        _shader.SetUniform("model", model);

        Matrix4x4 view = _camera.GetViewMatrix();
        _shader.SetUniform("view", view);

        Matrix4x4 projection = _camera.GetProjectionMatrix();
        _shader.SetUniform("projection", projection);

        // texture
        // --------------------------------------------------

        _texture.Bind();

        // mesh
        // --------------------------------------------------

        // _meshRenderer.Draw(_shader);

        _chunk.Draw(_shader);

        // interface
        // --------------------------------------------------

        if (DebugScreen)
        {
            _interface.Draw();
        }
    }

    protected override void OnClosing()
    {
        
    }
}
