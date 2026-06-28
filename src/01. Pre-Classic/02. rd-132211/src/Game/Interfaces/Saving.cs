using GameEngine.Interfaces;
using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utilities;

namespace RubyDung.Interfaces;

public class Saving
{
    private Image _image = null!;

    private bool _isActive = false;
    private float _startTime = 0f;

    private const float TotalDuration = 3.0f;   // tempo total em segundos
    private const float FadeStart = 2.0f;       // quando começa a desaparecer
    private const float FadeDuration = 1.0f;    // duração do fade

    public bool IsActive => _isActive;

    public Saving()
    {
        _image = new Image();

        _image.Width = 32.0f;
        _image.Height = 32.0f;

        _image.Position = new Vector3(-16.0f, 16.0f, 0.0f);
        _image.Pivot = new Vector2(1.0f, 0.0f);

        _image.Texture = new Texture2D("floppy_disk");

        _image.Load();
    }

    public void Start()
    {
        _isActive = true;
        _startTime = Time.ElapsedTime;
    }

    public void Update()
    {
        if (!_isActive)
            return;

        float elapsed = Time.ElapsedTime - _startTime;
        if (elapsed > TotalDuration)
        {
            _isActive = false;
            return;
        }
    }

    public void Draw(ShaderProgram shader)
    {
        if (!_isActive)
        return;

        // Calcula opacidade (fade out nos últimos 1 segundo)
        float elapsed = Time.ElapsedTime - _startTime;
        float opacity = 1.0f;
        if (elapsed > FadeStart)
        {
            opacity = 1.0f - (elapsed - FadeStart) / FadeDuration;
            opacity = Mathf.Clamp(opacity, 0.0f, 1.0f);
        }

        // Aplica opacidade diretamente na cor da imagem
        _image.color = new Color(1.0f, 1.0f, 1.0f, opacity);

        // Desenha a imagem (ela usará sua própria cor)
        _image.Draw(shader);

        // Restaura a cor da imagem para branco sólido (para uso futuro)
        _image.color = Color.White;

        // RESTAURA O UNIFORME uColor NO SHADER PARA BRANCO
        shader.SetUniform("uColor", (Vector4)Color.White);
    }
}
