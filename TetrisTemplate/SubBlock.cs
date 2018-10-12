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

    public Vector2 Position
    {
        get { return new Vector2(X, Y); }
        set { Position = value; }
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
            if (GetSubBlockAtPosition(x, y) != null)
                return false;
        }
        return true;
    }

    public void MoveTo(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static SubBlock GetSubBlockAtPosition(int x, int y)
    {
        foreach (SubBlock subBlock in TetrisGame.allSubBlocks)
            if (subBlock.X == x && subBlock.Y == y)
                return subBlock;
        return null;
    }
}

