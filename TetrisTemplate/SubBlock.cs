using Microsoft.Xna.Framework;

class SubBlock
{
    public int X { get; set; }
    public int Y { get; set; }
    public Color Color { get; set; }

    public SubBlock(int x, int y, Color c)
    {
        X = x;
        Y = y;
        Color = c;
    }
    public bool IsInBounds()
    {
        if (X < 0 || X >= TetrisGrid.Width || Y >= TetrisGrid.Height)
        { return false; }
        return true;
    }
    public bool IsInSubBlock(SubBlock subBlock)
    {
        if (X == subBlock.X && Y == subBlock.Y)
        {
            return true;
        }
        return false;
    }
    /*public bool CanMoveTo(int x, int y)
    {
        return null;
    }*/
}

