using GameEngine.Mathematics;
using GameEngine.Meshing;
using GameEngine.Rendering;

namespace GameEngine.Interfaces;

public class Font
{
    public Texture2D Texture = null!;

    public MeshQuad Mesh = null!;
    public MeshRenderer MeshRenderer = null!;

    private int[] _charWidths = new int[256];

    // Construtor
    // --------------------------------------------------

    public Font()
    {
        Texture = new Texture2D("default");
        Mesh = new MeshQuad();
        MeshRenderer = new MeshRenderer();

        int w = Texture.Widht;
        int h = Texture.Height;
        byte[] rawPixels = Texture.Data;

        for (int i = 0; i < 128; i++)
        {
            int xt = i % 16;
            int yt = i / 16;
            int x = 0;
            bool emptyColumn = false;

            for (x = 0; x < 8 && !emptyColumn; x++)
            {
                int xPixel = xt * 8 + x;
                emptyColumn = true;

                for (int y = 0; y < 8 && emptyColumn; y++)
                {
                    int yPixel = (yt * 8 + y) * w;
                    int pixelIndex = (xPixel + yPixel) * 4;
                    int pixelValue = rawPixels[pixelIndex];

                    if (pixelValue > 128)
                    {
                        emptyColumn = false;
                    }
                }
            }

            if (i == 32)
            {
                x = 4;
            }

            _charWidths[i] = x;
        }
    }
    
    // 
    // --------------------------------------------------

    public void DrawShadow(string str, int x, int y, int color)
    {
        Draw(str, x + 1, y - 1, color, true);
        Draw(str, x, y, color);
    }

    public void Draw(string str, int x, int y, int color)
    {
        Draw(str, x, y, color, false);
    }
    
    public void Draw(string str, int x, int y, int color, bool darken)
    {
        char[] chars = str.ToCharArray();

        if (darken)
        {
            color = (color & 16579836) >> 2;
        }

        // Mesh.Clear();
        Mesh.SetColors(color);

        int xo = 0;

        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == '&')
            {
                int cc = "0123456789abcdef".IndexOf(chars[i + 1]);
                int br = (cc & 8) * 8;

                int r = ((cc & 4) >> 2) * 191 + br;
                int g = ((cc & 2) >> 1) * 191 + br;
                int b = (cc & 1) * 191 + br;

                color = r << 16 | g << 8 | b;

                i += 2;

                if (darken)
                {
                    color = (color & 16579836) >> 2;
                }

                Mesh.SetColors(color);
            }

            int ix = chars[i] % 16 * 8;
            int iy = chars[i] / 16 * 8;

            float x0 = (float)(x + xo);
            float y0 = (float)y;

            float x1 = (float)(x + xo + 8);
            float y1 = (float)(y + 8);

            float u0 = (float)ix / 128.0f;
            float v0 = (float)iy / 128.0F;

            float u1 = (float)(ix + 8) / 128.0F;
            float v1 = (float)(iy + 8) / 128.0F;

            Mesh.AddVertexWithUV(x0, y0, 0.0f, u0, v1);
            Mesh.AddVertexWithUV(x1, y0, 0.0f, u1, v1);
            Mesh.AddVertexWithUV(x1, y1, 0.0f, u1, v0);
            Mesh.AddVertexWithUV(x0, y1, 0.0f, u0, v0);

            xo += _charWidths[chars[i]];
        }

        // MeshRenderer.Mesh = Mesh;
    }
}
