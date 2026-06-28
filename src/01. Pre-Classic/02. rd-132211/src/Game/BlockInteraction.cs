using GameEngine.Core;
using GameEngine.Inputs;
using GameEngine.Mathematics;
using GameEngine.Meshing;
using GameEngine.Rendering;
using RubyDung.Blocks;
using RubyDung.Level;
using RubyDung.Physics;
using Silk.NET.OpenGL;

namespace RubyDung;

public class BlockInteraction
{
    public static HitResult HitResult { get; private set; } = null!;  
    public static string BlockName { get; private set; } = null!; 
    public static bool Target { get; private set; } = false; 

    // 
    // --------------------------------------------------

    private GL _gl = Engine.GL;

    private World _world;
    private Player _player;
    private HitResult _hitResult = null!;

    private MeshCube _mesh = null!;
    private MeshRenderer _meshRenderer = null!;

    private Color _color;

    // Construtor
    // --------------------------------------------------

    public BlockInteraction(World world, Player player)
    {
        _world = world;
        _player = player;

        _mesh = new MeshCube();
        _meshRenderer = new MeshRenderer();
    }
    
    public void Update()
    {
        if (RayCast())
        {
            Target = true;
            HitResult = _hitResult;

            int x = _hitResult.X;
            int y = _hitResult.Y;
            int z = _hitResult.Z;

            int blockID = _world.GetBlock(x, y, z);

            BlockName = Block.Blocks[blockID].TextualID;

            RenderHit(_hitResult);

            if (Game.Mode == GameMode.Survival || Game.Mode == GameMode.Creative)
            {
                if (Input.GetKeyDown(KeyCode.MouseRight))
                {
                    _world.SetBlock(x, y, z, 0);
                }
                if (Input.GetKeyDown(KeyCode.MouseLeft))
                {
                    if (_hitResult.F == (int)MeshQuadFace.Negative_X)
                    {
                        x--;
                    }
                    if (_hitResult.F == (int)MeshQuadFace.Positive_X)
                    {
                        x++;
                    }
                    if (_hitResult.F == (int)MeshQuadFace.Negative_Y)
                    {
                        y--;
                    }
                    if (_hitResult.F == (int)MeshQuadFace.Positive_Y)
                    {
                        y++;
                    }
                    if (_hitResult.F == (int)MeshQuadFace.Negative_Z)
                    {
                        z--;
                    }
                    if (_hitResult.F == (int)MeshQuadFace.Positive_Z)
                    {
                        z++;
                    }

                    int b = Block.Rock.ID;

                    if (z == _world.Height * 2 / 3)
                    {
                        b = Block.Grass.ID;
                    }

                    AABB bounds = Block.Blocks[b].GetBounds(x, y, z);

                    if (bounds == null || IsFree(bounds))
                    {
                        _world.SetBlock(x, y, z, b);
                    }
                }
            }
        }
        else
        {
            Target = false;

            _mesh.Clear();
            _meshRenderer.Mesh = _mesh;
        }
    }

    public void Draw(ShaderProgram shader)
    {
        shader.SetUniform("uColor", (Vector4)_color);

        _gl.Enable(EnableCap.Blend);
        _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        _gl.Enable(EnableCap.PolygonOffsetFill);
        _gl.PolygonOffset(-1.0f, -1.0f);

        _meshRenderer.Draw(shader);

        _gl.Disable(EnableCap.PolygonOffsetFill);
        _gl.Disable(EnableCap.Blend);

        shader.SetUniform("uColor", 1.0f, 1.0f, 1.0f, 1.0f);
    }

    private bool RayCast()
    {
        Vector3 start = _player.Position;
        Vector3 dir = _player.Front;

        float maxDist = 5.0f;

        // Verifica se o vetor direção é zero
        if (dir.X == 0 && dir.Y == 0 && dir.Z == 0)
            return false;

        dir = Vector3.Normalize(dir);

        // Posição inicial do voxel (arredondando para baixo)
        Vector3Int pos = new Vector3Int(
            Mathf.FloorToInt(start.X),
            Mathf.FloorToInt(start.Y),
            Mathf.FloorToInt(start.Z)
        );

        // Direção dos passos (1 ou -1)
        Vector3Int step = new Vector3Int(
            Mathf.SignToInt(dir.X),
            Mathf.SignToInt(dir.Y),
            Mathf.SignToInt(dir.Z)
        );

        // Distâncias até a próxima fronteira do voxel (em unidades do ray)
        Vector3 tMax = new Vector3(
            dir.X != 0 ? (dir.X > 0 ? (pos.X + 1 - start.X) / dir.X : (start.X - pos.X) / -dir.X) : float.MaxValue,
            dir.Y != 0 ? (dir.Y > 0 ? (pos.Y + 1 - start.Y) / dir.Y : (start.Y - pos.Y) / -dir.Y) : float.MaxValue,
            dir.Z != 0 ? (dir.Z > 0 ? (pos.Z + 1 - start.Z) / dir.Z : (start.Z - pos.Z) / -dir.Z) : float.MaxValue
        );

        Vector3 tDelta = new Vector3(
            dir.X != 0 ? 1f / Mathf.Abs(dir.X) : float.MaxValue,
            dir.Y != 0 ? 1f / Mathf.Abs(dir.Y) : float.MaxValue,
            dir.Z != 0 ? 1f / Mathf.Abs(dir.Z) : float.MaxValue
        );

        float dist = 0;
        int face = -1;

        while (dist < maxDist)
        {
            // Verifica se o bloco atual é sólido usando o World
            if (_world.IsSolidBlock(pos.X, pos.Y, pos.Z))
            {
                _hitResult = new HitResult(pos.X, pos.Y, pos.Z, face);
                return true;
            }

            // Avança para o próximo voxel (menor tMax)
            if (tMax.X < tMax.Y && tMax.X < tMax.Z)
            {
                pos.X += step.X;
                dist = tMax.X;
                tMax.X += tDelta.X;
                face = step.X > 0 ?
                    (int)MeshQuadFace.Negative_X : 
                    (int)MeshQuadFace.Positive_X;  
                    // 0: +X, 1: -X
            }
            else if (tMax.Y < tMax.Z)
            {
                pos.Y += step.Y;
                dist = tMax.Y;
                tMax.Y += tDelta.Y;
                face = step.Y > 0 ?
                    (int)MeshQuadFace.Negative_Y :
                    (int)MeshQuadFace.Positive_Y;  
                    // 2: +Y (frente), 3: -Y (trás)
            }
            else
            {
                pos.Z += step.Z;
                dist = tMax.Z;
                tMax.Z += tDelta.Z;
                face = step.Z > 0 ?
                    (int)MeshQuadFace.Negative_Z :
                    (int)MeshQuadFace.Positive_Z;  
                    // 4: +Z (cima), 5: -Z (baixo)
            }
        }

        return false;
    }

    private void RenderHit(HitResult h)
    {
        float alpha = Mathf.Sin(Environment.TickCount / 100.0f) * 0.2f + 0.4f;
        _color = new Color(1.0f, 1.0f, 1.0f, alpha);

        _mesh.Clear();

        Block.Rock.AddFace(_mesh, h.X, h.Y, h.Z, (MeshQuadFace)h.F);

        // _meshRenderer = new MeshRenderer();
        _meshRenderer.Mesh = _mesh;
    }
    
    private bool IsFree(AABB bounds)
    {
        if (_player.Bounds.Intersects(bounds))
        {
            return false;
        }
        
        return true;
    }
}
