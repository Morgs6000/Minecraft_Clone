using GameEngine.Mathematics;
using GameEngine.Meshing;
using GameEngine.Rendering;

namespace GameEngine.Interfaces;

/// <summary>
/// Representa uma fonte bitmap monocromática carregada a partir de uma textura.
/// Permite desenhar texto com suporte a cores via códigos de formatação (ex: "&c").
/// </summary>
public class Font
{
    // Textura que contém a atlas da fonte (128x128 pixels, com glifos 8x8 organizados em grid 16x8)
    public Texture2D Texture = null!;

    // Malha utilizada para construir os quads do texto dinamicamente
    public MeshQuad Mesh = null!;

    // Renderizador associado à malha
    public MeshRenderer MeshRenderer = null!;

    public int[] CharWidths;

    // Largura em pixels de cada caractere ASCII (0-127), calculada na construção
    private int[] _charWidths = new int[256]; // suporta até 256, mas somente 0-127 são preenchidos

    // Construtor
    // --------------------------------------------------

    public Font()
    {
        // Inicializa os componentes básicos
        Texture = new Texture2D("default");
        Mesh = new MeshQuad();
        MeshRenderer = new MeshRenderer();

        int w = Texture.Widht; // largura da textura (espera-se 128)
        int h = Texture.Height; // altura da textura (espera-se 128)
        byte[] rawPixels = Texture.Data; // dados RGBA brutos

        // Para cada caractere de 0 a 127 (ASCII básico)
        for (int i = 0; i < 128; i++)
        {
            // Posição do tile na grid: 16 colunas
            int xt = i % 16;
            int yt = i / 16;
            int x = 0;
            bool emptyColumn = false;

            // Varre as colunas do glifo (8 pixels de largura)
            // Procura a primeira coluna totalmente vazia para determinar a largura real
            for (x = 0; x < 8 && !emptyColumn; x++)
            {
                int xPixel = xt * 8 + x; // coordenada X do pixel na textura
                emptyColumn = true;

                // Verifica todos os pixels desta coluna (altura 8)
                for (int y = 0; y < 8 && emptyColumn; y++)
                {
                    int yPixel = (yt * 8 + y) * w; // deslocamento da linha
                    int pixelIndex = (xPixel + yPixel) * 4; // 4 bytes por pixel (RGBA)
                    int pixelValue = rawPixels[pixelIndex]; // canal R (ou alpha? aqui usa R > 128)

                    // Se houver algum pixel com valor > 128, a coluna não é vazia
                    if (pixelValue > 128)
                    {
                        emptyColumn = false;
                    }
                }
            }

            // Para o espaço (índice 32), força uma largura fixa de 4 pixels
            if (i == 32)
            {
                x = 4;
            }

            _charWidths[i] = x; // armazena a largura calculada
        }

        //
        // --------------------------------------------------

        CharWidths = _charWidths;
    }

    // Desenha o texto com uma sombra escura projetada para baixo e para a direita
    // --------------------------------------------------

    /// <summary>
    /// Desenha uma string com efeito de sombra (versão escura deslocada e depois o texto normal).
    /// </summary>
    /// <param name="str">Texto a ser desenhado (pode conter códigos de cor &x)</param>
    /// <param name="x">Posição X na tela</param>
    /// <param name="y">Posição Y na tela</param>
    /// <param name="color">Cor base (RGB empacotado em inteiro)</param>
    public void DrawShadow(string str, float x, float y, int color)
    {
        // Primeiro desenha a sombra: posição deslocada e cor escurecida
        Draw(str, x + 1.0f, y - 1.0f, color, true);

        // Depois desenha o texto principal por cima
        Draw(str, x, y, color);
    }

    /// <summary>
    /// Desenha uma string na posição especificada com a cor fornecida.
    /// </summary>
    public void Draw(string str, float x, float y, int color)
    {
        Draw(str, x, y, color, false);
    }

    /// <summary>
    /// Método interno de desenho que monta os quads na malha.
    /// </summary>
    /// <param name="str">Texto a ser desenhado</param>
    /// <param name="x">Posição X</param>
    /// <param name="y">Posição Y</param>
    /// <param name="color">Cor base</param>
    /// <param name="darken">Se verdadeiro, escurece a cor (usado para sombra)</param>
    public void Draw(string str, float x, float y, int color, bool darken)
    {
        char[] chars = str.ToCharArray();

        // Escurecimento da cor: aplica um shift nos bits para reduzir intensidade
        if (darken)
        {
            // 16579836 = 0xFCFCFC; (color & 0xFCFCFC) >> 2  reduz cada componente aproximadamente a 1/4
            color = (color & 16579836) >> 2;
        }
        
        // Mesh.Clear();
        
        // Prepara a malha com a cor atual
        Mesh.SetColors(color);

        int xo = 0; // offset horizontal acumulado

        for (int i = 0; i < chars.Length; i++)
        {
            // Detecta código de cor estilo '&c', onde 'c' é um dígito hexadecimal 0-f
            if (chars[i] == '&')
            {
                // Procura o caractere seguinte na string de dígitos hexa
                int cc = "0123456789abcdef".IndexOf(chars[i + 1]);

                // Calcula brilho extra: se bit 3 (8) estiver setado, br = 64, senão 0
                int br = (cc & 8) * 8;

                // Separa os bits R, G, B:
                // R: bit 2 (4) -> multiplica por 191 e adiciona br
                // G: bit 1 (2) -> multiplica por 191 e adiciona br
                // B: bit 0 (1) -> multiplica por 191 e adiciona br
                int r = ((cc & 4) >> 2) * 191 + br;
                int g = ((cc & 2) >> 1) * 191 + br;
                int b = (cc & 1) * 191 + br;

                // Monta cor no formato 0xRRGGBB
                color = r << 16 | g << 8 | b;

                i += 2; // pula o '&' e o código

                // Se for sombra, reaplica o escurecimento na nova cor
                if (darken)
                {
                    color = (color & 16579836) >> 2;
                }

                Mesh.SetColors(color);
            }

            // Calcula coordenadas UV do glifo na textura
            int ix = chars[i] % 16 * 8; // coluna do tile * 8
            int iy = chars[i] / 16 * 8; // linha do tile * 8

            // Coordenadas do quad na tela
            float x0 = (float)(x + xo);
            float y0 = (float)y;

            float x1 = (float)(x + xo + 8);
            float y1 = (float)(y + 8);

            // Coordenadas UV (a textura é considerada de 128x128)
            // Nota: v0 usa v1 (invertido?) para orientação da textura
            float u0 = (float)ix / 128.0f;
            float v0 = (float)iy / 128.0F;

            float u1 = (float)(ix + 8) / 128.0F;
            float v1 = (float)(iy + 8) / 128.0F;

            // Adiciona dois triângulos (quad) na malha
            Mesh.AddVertexWithUV(x0, y0, 0.0f, u0, v1);
            Mesh.AddVertexWithUV(x1, y0, 0.0f, u1, v1);
            Mesh.AddVertexWithUV(x1, y1, 0.0f, u1, v0);
            Mesh.AddVertexWithUV(x0, y1, 0.0f, u0, v0);

            // Avança o offset horizontal de acordo com a largura real do caractere
            xo += _charWidths[chars[i]];
        }

        // MeshRenderer.Mesh = Mesh;
    }
}
