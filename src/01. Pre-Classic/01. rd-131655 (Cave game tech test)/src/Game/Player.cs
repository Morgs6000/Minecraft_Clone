using System.Diagnostics;
using GameEngine.Inputs;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung.Level;
using RubyDung.Physics;

namespace RubyDung;

public class Player : Camera
{
    public bool OnFly { get; private set; } = false;
    public bool OnGround { get; private set; } = false;
    public bool OnSprint { get; private set; } = false;

    // 
    // --------------------------------------------------

    private World _world;
    private AABB _bounds = null!;

    private Vector3 _delta = Vector3.Zero;

    private float _walking       = 4.317f;
    private float _sprinting     = 5.612f;
    // private float _sneaking      = 1.295f;
    private float _flying        = 10.79f;
    private float _sprint_flying = 21.58f;
    private float _falling       = 77.71f;
    private float _jumping       = 1.2522f;
    
    private float _boundsWidht  = 0.6f;
    private float _boundsHeight = 1.8f;
    private float _offsetHeight = 1.62f;

    private Vector3 _velocity = Vector3.Zero;

    // Construtor
    // --------------------------------------------------

    public Player(World world)
    {
        _world = world;

        MovementSpeed = _walking;

        ResetPos();
    }

    // 
    // --------------------------------------------------

    protected override void ProcessKeyboard()
    {
        ProcessKeyboardMovement();

        if (Input.GetKey(KeyCode.R))
        {
            ResetPos();
        }
    }
    
    // 
    // --------------------------------------------------

    private void ResetPos()
    {
        Random random = new Random();

        float x = (float)(random.NextDouble() * _world.Width);
        float y = (float)(random.NextDouble() * _world.Depth);
        float z = (float)(_world.Height + 10);

        SetPos(x, y, z);
    }

    private void SetPos(float x, float y, float z)
    {
        Position = new Vector3(x, y, z);
    }

    private void ProcessKeyboardMovement()
    {
        _delta = Vector3.Zero;

        float velocity = MovementSpeed * Time.DeltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            _delta += velocity * Vector3.Normalize(new Vector3(Front.X, Front.Y, 0.0f));
        }
        if (Input.GetKey(KeyCode.S))
        {
            _delta -= velocity * Vector3.Normalize(new Vector3(Front.X, Front.Y, 0.0f));
        }
        if (Input.GetKey(KeyCode.A))
        {
            _delta -= velocity * Vector3.Normalize(Vector3.Cross(Front, Up));
        }
        if (Input.GetKey(KeyCode.D))
        {
            _delta += velocity * Vector3.Normalize(Vector3.Cross(Front, Up));
        }

        ProcessSpace(velocity);
        ProcessSprint();

        Move(_delta);
    }

    private void ProcessSpace(float velocity)
    {
        MovementSpeed = OnFly ? _flying : _walking;

        if (OnFly)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _delta += velocity * Up;
            }
            if (Input.GetKey(KeyCode.ShiftLeft))
            {
                _delta -= velocity * Up;
            }
        }
        else
        {
            // Aplicar gravidade
            _velocity.Z -= _falling * Time.DeltaTime;

            if (Input.GetKey(KeyCode.Space) && OnGround)
            {
                _velocity.Z = (float)Math.Sqrt(2 * _falling * _jumping);
            }

            _delta.Z = _velocity.Z * Time.DeltaTime;
        }

        if (Game.Mode == GameMode.Spectator)
        {
            OnFly = true;
        }
        if (Input.GetKeyDouble(KeyCode.Space) && Game.Mode == GameMode.Creative)
        {
            OnFly = !OnFly;
        }
        if (Game.Mode == GameMode.Survival ||
            Game.Mode == GameMode.Adventure ||
            OnGround)
        {
            OnFly = false;
        }
    }

    private void ProcessSprint()
    {
        if (OnFly)
        {
            MovementSpeed = OnSprint ? _sprint_flying : _flying;
        }
        else
        {
            MovementSpeed = OnSprint ? _sprinting : _walking;
        }

        if (Input.GetKeyDouble(KeyCode.W) ||
            Input.GetKey(KeyCode.ControlLeft))
        {
            OnSprint = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            OnSprint = false;
        }
    }

    // Verifica se a posição da câmera colide com algum bloco sólido
    private bool IsColliding(Vector3 position)
    {
        if (Game.Mode == GameMode.Spectator)
        {
            return false;
        }

        // Calcula as dimensões da caixa de colisão
        float _w = _boundsWidht / 2.0f;
        float _h = _boundsHeight / 2.0f;

        // Centro da caixa em X e Z, centro da caixa em Y ajustado pela altura dos olhos
        float _x = position.X;
        float _y = position.Y;
        float _z = position.Z - _offsetHeight + _h;

        // Vetores min e max da caixa de colisão
        _bounds = new AABB(
            new Vector3(_x - _w, _y - _w, _z - _h),
            new Vector3(_x + _w, _y + _w, _z + _h)
        );

        // Limites inteiros para verificar blocos
        Vector3Int min = new Vector3Int(
            Mathf.FloorToInt(_bounds.Min.X),
            Mathf.FloorToInt(_bounds.Min.Y),
            Mathf.FloorToInt(_bounds.Min.Z)
        );

        Vector3Int max = new Vector3Int(
            Mathf.CeilToInt(_bounds.Max.X),
            Mathf.CeilToInt(_bounds.Max.Y),
            Mathf.CeilToInt(_bounds.Max.Z)
        );

        // Verifica colisão com cada bloco possível
        for (int x = min.X; x <= max.X; x++)
        {
            for (int y = min.Y; y <= max.Y; y++)
            {
                for (int z = min.Z; z <= max.Z; z++)
                {
                    if (_world.IsSolidBlock(x, y, z))
                    {
                        // Vetores min e max do bloco
                        AABB bounds = new AABB(
                            new Vector3(x, y, z),
                            new Vector3(x + 1, y + 1, z + 1)
                        );

                        // Verifica se as caixas AABB se intersectam
                        if (_bounds.Intersects(bounds))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private void Move(Vector3 delta)
    {
        // Tenta mover na direção X (esquerda/direita)
        Vector3 newPosition = Position + new Vector3(delta.X, 0.0f, 0.0f);

        if (!IsColliding(newPosition))
        {
            Position = newPosition;
        }

        // Tenta mover na direção Y (frente/trás)
        newPosition = Position + new Vector3(0.0f, delta.Y, 0.0f);
        
        if (!IsColliding(newPosition))
        {
            Position = newPosition;
        }

        // Tenta mover na direção Z (cima/baixo)
        newPosition = Position + new Vector3(0.0f, 0.0f, delta.Z);

        if (!IsColliding(newPosition))
        {
            Position = newPosition;
            OnGround = false;
        }
        else
        {
            if (delta.Z < 0)
            {
                OnGround = true;
                _velocity.Z = 0;
            }

        }
    }
}
