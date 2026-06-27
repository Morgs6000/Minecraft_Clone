using GameEngine.Mathematics;
using GameEngine.Utilities;
using Silk.NET.Input;
using Silk.NET.Windowing;

namespace GameEngine.Inputs;

/// <summary>
/// Interface com o sistema de entrada.
/// </summary>
public class Input
{
    /// <summary>
    /// Alguma tecla ou botão do mouse está pressionado no momento? (Somente leitura)
    /// </summary>
    public static bool AnyKey => _keys.Count > 0;

    /// <summary>
    /// Retorna verdadeiro no primeiro frame em que o usuário pressionar qualquer tecla ou botão do mouse. (Somente leitura)
    /// </summary>
    public static bool AnyKeyDown => _keys.Count > 0 && _keysPrevious.Count == 0;

    /// <summary>
    /// Retorna a entrada do teclado inserida neste quadro. (Somente leitura)
    /// </summary>
    public static string InputString => _keys.Count > 0 ? _keys.First().ToString() : string.Empty;

    //
    // --------------------------------------------------

    /// <summary>
    /// 
    /// </summary>
    public static CursorLockMode CursorLockMode
    {
        get
        {
            CursorMode inputMode = _mouse.Cursor.CursorMode;

            switch (inputMode)
            {
                case CursorMode.Normal:
                    return CursorLockMode.Normal;
                case CursorMode.Hidden:
                    return CursorLockMode.Hidden;
                case CursorMode.Disabled:
                    return CursorLockMode.Disabled;
                case CursorMode.Raw:
                    return CursorLockMode.Raw;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        set
        {
            CursorMode inputMode;

            switch (value)
            {
                case CursorLockMode.Normal:
                    inputMode = CursorMode.Normal;
                    break;
                case CursorLockMode.Hidden:
                    inputMode = CursorMode.Hidden;
                    break;
                case CursorLockMode.Disabled:
                    inputMode = CursorMode.Disabled;
                    break;
                case CursorLockMode.Raw:
                    inputMode = CursorMode.Raw;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _mouse.Cursor.CursorMode = inputMode;
        }
    }

    //
    // --------------------------------------------------

    public static Vector2 MousePosition => _mouse.Position;

    public static Vector2 MouseScrollDelta
    {
        get
        {
            var delta = _mouseScrollDelta;
            _mouseScrollDelta = Vector2.Zero;

            return delta;
        }
    }

    private static Vector2 _mouseScrollDelta;

    //
    // --------------------------------------------------

    private static IKeyboard _keyboard = null!;
    private static IMouse _mouse = null!;

    private static HashSet<KeyCode> _keys = [];
    private static HashSet<KeyCode> _keysPrevious = [];

    private static readonly HashSet<KeyCode> _keysValid = Enum.GetValues<KeyCode>()
        .Where(key => key != KeyCode.Unknown)
        .ToHashSet();

    private readonly static Dictionary<KeyCode, float> _lastPressTime = [];
    private const float _doublePressedTime = 0.3f;

    //
    // --------------------------------------------------

    public static void Initialize(IWindow window)
    {
        IInputContext input = window.CreateInput();
        
        _keyboard = input.Keyboards[0];
        _mouse = input.Mice[0];

        _mouse.Scroll += (mouse, scrollWheel) =>
        {
            _mouseScrollDelta = new Vector2(scrollWheel.X, scrollWheel.Y);
        };
    }

    //
    // --------------------------------------------------

    public static void NewFrame()
    {
        _keysPrevious.Clear();

        foreach (KeyCode key in _keys)
        {
            _keysPrevious.Add(key);
        }

        // --------------------------------------------------

        _keys.Clear();

        if (_keyboard != null)
        {
            foreach (KeyCode key in _keysValid)
            {
                if (_keyboard.IsKeyPressed((Key)key))
                {
                    _keys.Add(key);
                }
            }
        }
    }

    //
    // --------------------------------------------------

    /// <summary>
    /// Retorna verdadeiro enquanto o usuário mantiver pressionada a tecla identificada pelo parâmetro de enumeração KeyCode.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKey(KeyCode key)
    {
        return _keys.Contains(key);
    }

    /// <summary>
    /// Retorna verdadeiro durante o frame em que o usuário começa a pressionar a tecla identificada pelo parâmetro de enumeração KeyCode.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKeyDown(KeyCode key)
    {
        return _keys.Contains(key) && !_keysPrevious.Contains(key);
    }

    /// <summary>
    /// Retorna verdadeiro durante o frame em que o usuário solta a tecla identificada pelo parâmetro de enumeração KeyCode.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKeyUp(KeyCode key)
    {
        return !_keys.Contains(key) && _keysPrevious.Contains(key);
    }
    
    /// <summary>
    /// Retorna verdadeiro durante o frame em que o usuário pressionar duas vezes a tecla identificada pelo parâmetro de enumeração KeyCode.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKeyDouble(KeyCode key)
    {
        if (GetKeyDown(key))
        {
            float now = Time.ElapsedTime;

            if (_lastPressTime.TryGetValue(key, out float last) && (now - last) < _doublePressedTime)
            {
                return true;
            }

            _lastPressTime[key] = now;
        }

        return false;
    }
}
