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
            return false;
        return true;
    }
       
    public bool CanMoveTo(int x, int y)
    {
        foreach (SubBlock subBlock in TetrisGame.allSubBlocks)
        {
            if (IsSubBlockAtPosition(x, y))
                return false;
        }
        return true;
    }

    public void MoveTo(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool IsSubBlockAtPosition(int x, int y)
    {
        foreach (SubBlock subBlock in TetrisGame.allSubBlocks)
            if (subBlock.X == x && subBlock.Y == y)
                return true;
        return false;
    }
}

