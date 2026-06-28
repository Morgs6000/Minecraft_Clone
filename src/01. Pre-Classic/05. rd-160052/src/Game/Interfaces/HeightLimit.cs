using GameEngine.Mathematics;
using GameEngine.Rendering;
using GameEngine.Utilities;
using RubyDung.Level;

namespace RubyDung.Interfaces;

public class HeightLimit
{
    private Font _font = null!;

    private const float TotalDuration = 6.0f;    // tempo total em segundos
    private const float FadeStart = 3.5f;        // quando começa a desaparecer
    private const float FadeDuration = 2.5f;     // duração do fade

    // 
    // --------------------------------------------------  

    public HeightLimit()
    {
        _font = new Font();
    }

    // 
    // --------------------------------------------------  

    public void Update()
    {
        // Se a flag não estiver ativa, não faz nada
        if (!World.OnHeightLimit)
        {
            return;
        }

        float elapsed = Time.ElapsedTime - World.HeightLimitStartTime;

        // Verifica se já passaram 3 segundos desde que a flag foi ativada
        if (elapsed > TotalDuration)
        {
            World.OnHeightLimit = false;

            return;
        }

         // Cor fixa sem alfa (o shader controlará a opacidade via uColor)
        int colorInt = 16536660; // vermelho sólido

        // Debug.Log(Color.FromHex("#FC5454").ToDecimalString());
        // Debug.Log(Color.FromHex("#FC5454").ToString());

        string str = $"O limite de altura para construcao eh de {World.HeightLimit} blocos.";

        float w = Interface.ScreenWidth / 2;
        // float h = Interface.ScreenHeight / 2;

        _font.Clear();

        _font.DrawShadow(
            str,
            w - (MeasureString(str) / 2.0f),
            65,
            colorInt
        );

        _font.FontRenderer();
    }
    
    public void Draw(ShaderProgram shader)
    {
        // Calcula a opacidade atual (mesmo cálculo do Update)
        float opacity = 1.0f;

        if (World.OnHeightLimit)
        {
            float elapsed = Time.ElapsedTime - World.HeightLimitStartTime;
            if (elapsed > FadeStart)
            {
                opacity = 1.0f - (elapsed - FadeStart) / FadeDuration;
                opacity = Mathf.Clamp(opacity, 0.0f, 1.0f);
            }
        }

        // Cria a cor vermelha com alfa = opacity
        Color h = Color.FromHex("#FC5454");

        Color color = new Color(h.R, h.G, h.B, opacity); // #FC5454 em floats
        shader.SetUniform("uColor", (Vector4)color);

        // Desenha a fonte normalmente
        Matrix4x4 model = Matrix4x4.Identity;
        shader.SetUniform("model", model);

        _font.Texture.Bind();
        _font.MeshRenderer.Draw(shader);

        shader.SetUniform("uColor", Vector4.Positive);
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
}
