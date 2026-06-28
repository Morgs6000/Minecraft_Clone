namespace RubyDung.Blocks;

public class BlockDirt : Block
{
    public BlockDirt(int id) : base(id)
    {
        TextualID = "dirt";
        _tex = 2;
    }
}
