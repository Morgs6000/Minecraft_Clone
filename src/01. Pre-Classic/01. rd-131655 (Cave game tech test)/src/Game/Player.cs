using GameEngine.Inputs;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung.Level;

namespace RubyDung;

public class Player : Camera
{
    private World _world;

    // private float _walking       = 4.317f;
    // private float _sprinting     = 5.612f;
    // private float _sneaking      = 1.295f;
    // private float _flying        = 10.79f;
    private float _sprint_flying = 21.58f;
    // private float _falling       = 77.71f;
    // private float _jumping       = 1.2522f;

    public Player(World world)
    {
        _world = world;

        MovementSpeed = _sprint_flying;

        // ResetPos();
    }

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

    protected override void ProcessKeyboard()
    {
        ProcessKeyboardMovement();

        if (Input.GetKey(KeyCode.R))
        {
            ResetPos();
        }
    }

    private void ProcessKeyboardMovement()
    {
        float velocity = MovementSpeed * Time.DeltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            Position += velocity * Vector3.Normalize(new Vector3(Front.X, Front.Y, 0.0f));
        }
        if (Input.GetKey(KeyCode.S))
        {
            Position -= velocity * Vector3.Normalize(new Vector3(Front.X, Front.Y, 0.0f));
        }
        if (Input.GetKey(KeyCode.A))
        {
            Position -= velocity * Vector3.Normalize(Vector3.Cross(Front, Up));
        }
        if (Input.GetKey(KeyCode.D))
        {
            Position += velocity * Vector3.Normalize(Vector3.Cross(Front, Up));
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Position += velocity * Up;
        }
        if (Input.GetKey(KeyCode.ShiftLeft))
        {
            Position -= velocity * Up;
        }
    }
}
