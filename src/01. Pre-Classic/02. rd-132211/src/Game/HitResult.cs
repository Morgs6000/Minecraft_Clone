namespace RubyDung;

public class HitResult
{
    public int X;
    public int Y;
    public int Z;
    public int F;

    // Construtor
    // --------------------------------------------------

    public HitResult(int x, int y, int z, int f)
    {
        X = x;
        Y = y;
        Z = z;
        F = f;
    }

    // To String
    // --------------------------------------------------

    public override string ToString()
    {
        return $"{X}, {Y}, {Z}";
    }
}
