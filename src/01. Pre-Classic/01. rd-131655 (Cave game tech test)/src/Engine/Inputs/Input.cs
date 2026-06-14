using GameEngine.Utils;
using Silk.NET.Input;
using Silk.NET.Windowing;

namespace GameEngine.Inputs;

/// <summary>
/// Interface com o sistema de entrada.
/// </summary>
public class Input
{
    private static IKeyboard _keyboard = null!;

    private static HashSet<KeyCode> _keys = [];
    private static HashSet<KeyCode> _keysPrevious = [];

    private static readonly HashSet<KeyCode> _keysValid = Enum.GetValues<KeyCode>()
        .Where(key => key != KeyCode.Unknown)
        .ToHashSet();

    private readonly static Dictionary<KeyCode, float> _lastPressTime = [];
    private const float _doublePressedTime = 0.2f;

    //
    // --------------------------------------------------

    public static void Initialize(IWindow window)
    {
        IInputContext input = window.CreateInput();
        _keyboard = input.Keyboards[0];
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
