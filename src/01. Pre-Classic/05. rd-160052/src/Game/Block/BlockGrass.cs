using GameEngine.Meshing;

namespace RubyDung.Blocks;

public class BlockGrass : Block
{
    public BlockGrass(int id) : base(id)
    {
        TextualID = "grass_block";
        _tex = 0;
    }

    protected override int GetTexture(MeshQuadFace face)
    {
        if (face == MeshQuadFace.Positive_Z)
        {
            return 0;
        }
        else
        {
            return face == MeshQuadFace.Negative_Z ? 2 : 3;
        }
    }
}
