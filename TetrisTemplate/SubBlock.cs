using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

class SubBlock
{
    public int x { get; set; }
    public int y { get; set; }
    public Color color { get; set; }
    public TetrisGrid grid { get; }

    public SubBlock(int X, int Y, Color C, TetrisGrid Grid)
    {
        x = X;
        y = Y;
        color = C;
        grid = Grid;
    }

    public Vector2 Position
    {
        get { return new Vector2(x, y); }
        set { Position = value; }
    }


       
    public bool CanMoveTo(int x, int y)
    {
            if (TetrisGrid.IsInBounds(x, y, grid))
            {
                if (grid.gridArr[x, y] != null)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
    }

    public void MoveTo(int X, int Y)
    {
        x = X;
        y = Y;
    }

    /*public static SubBlock GetSubBlockAtPosition(int x, int y, List<SubBlock> allSubBlocks)
    {
        foreach (SubBlock subBlock in allSubBlocks)
            if (subBlock.X == x && subBlock.Y == y)
                return subBlock;
        return null;
    }*/
}

