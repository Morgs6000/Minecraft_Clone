using GameEngine.Inputs;
using GameEngine.Mathematics;
using GameEngine.Utilities;

namespace GameEngine.Rendering;

public class Camera
{
    // camera Attributes
    public Vector3 Position = new Vector3(0.0f, -3.0f, 0.0f);
    public Vector3 Front = new Vector3(0.0f, 1.0f, 0.0f);
    public Vector3 Up = new Vector3(0.0f, 0.0f, 1.0f);

    // euler Angles
    public float Yaw = 90.0f;
    public float Pitch = 0.0f;

    // camera options
    public float MovementSpeed = 2.5f;
    public float MouseSensitivity = 0.1f;
    public float Zoom = 60.0f;

    private bool _firstMouse = true;
    private Vector2 _lastPos;

    // private float _walking       = 4.317f;
    // private float _sprinting     = 5.612f;
    // private float _sneaking      = 1.295f;
    // private float _flying        = 10.79f;
    private float _sprint_flying = 21.58f;
    // private float _falling       = 77.71f;
    // private float _jumping       = 1.2522f;

    // Construtor
    // --------------------------------------------------

    public Camera()
    {
        MovementSpeed = _sprint_flying;

        Input.CursorLockMode = CursorLockMode.Raw;
    }

    // 
    // --------------------------------------------------

    public void Update()
    {
        ProcessKeyboard();
        ProcessMouseMovement();
        ProcessMouseScroll();
    }

    // Retorna a matriz de visualização calculada usando os ângulos de Euler e a matriz LookAt.
    // --------------------------------------------------
    
    public Matrix4x4 GetViewMatrix()
    {
        return Matrix4x4.LookAt(
            position: Position,
            target:   Position + Front,
            up:       Up
        );
    }

    // Retorna a matriz de projeção.
    // --------------------------------------------------
    
    public Matrix4x4 GetProjectionMatrix()
    {
        return Matrix4x4.Perspective(
            fov: Mathf.Radians(Zoom),
            aspect: (float)Screen.Width / (float)Screen.Height,
            zNear:  0.3f,
            zFar:   1000.0f
        );
    }

    // Processa a entrada recebida de qualquer sistema de entrada semelhante a um teclado. Aceita parâmetros de entrada na forma de um ENUM definido pela câmera (para abstraí-lo dos sistemas de janelas).
    // --------------------------------------------------
    
    private void ProcessKeyboard()
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

    // Processa a entrada recebida de um sistema de entrada de mouse. Espera o valor de deslocamento nas direções x e y.
    // --------------------------------------------------
    
    private void ProcessMouseMovement(bool constrainPitch = true)
    {
        if (_firstMouse)
        {
            _lastPos = Input.MousePosition;
            _firstMouse = false;
        }

        float xoffset = _lastPos.X - Input.MousePosition.X;
        float yoffset = _lastPos.Y - Input.MousePosition.Y;
        _lastPos = Input.MousePosition;

        xoffset *= MouseSensitivity;
        yoffset *= MouseSensitivity;

        Yaw   += xoffset;
        Pitch += yoffset;

        // Certifique-se de que, quando o campo estiver fora dos limites, a tela não seja invertida.
        // --------------------------------------------------

        if (constrainPitch)
        {
            Pitch = Mathf.Clamp(Pitch, -89.0f, 89.0f);
        }

        // Atualizar vetores Frontal, Direito e Superior usando os ângulos de Euler atualizados
        // --------------------------------------------------

        UpdateCameraVectors();
    }

    // Processa a entrada recebida de um evento de rolagem do mouse. Requer entrada apenas no eixo vertical da roda.
    // --------------------------------------------------
    
    private void ProcessMouseScroll()
    {
        Zoom -= Input.MouseScrollDelta.Y;
        Zoom = Mathf.Clamp(Zoom, 1.0f, 180.0f);
    }

    // Calcula o vetor frontal a partir dos ângulos de Euler (atualizados) da câmera.
    // --------------------------------------------------
    
    private void UpdateCameraVectors()
    {
        // calcular o novo vetor Front
        // --------------------------------------------------

        Vector3 front;

        front.X = Mathf.Cos(Mathf.Radians(Pitch)) * Mathf.Cos(Mathf.Radians(Yaw));
        front.Y = Mathf.Cos(Mathf.Radians(Pitch)) * Mathf.Sin(Mathf.Radians(Yaw));
        front.Z = Mathf.Sin(Mathf.Radians(Pitch));

        Front = Vector3.Normalize(front);
    }
}
