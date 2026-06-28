namespace RubyDung.Level;

public interface LevelListener
{
    void BlockChanged(int x, int y, int z);

    void LightColumnChanged(int x, int z, int y0, int y1);

    void AllChanged();
}
