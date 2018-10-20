using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

class SubBlockRow
{
    int y;
    //SubBlock[,] allSubBlocks;
    SubBlock[] rowBlocks { get; set; }
    TetrisGrid grid;
    public SubBlockRow(int Y, TetrisGrid Grid)
    {
        y = Y;
        grid = Grid;
        rowBlocks = new SubBlock[grid.width];
        // allSubBlocks = gameWorld.grid.gridArr;
        for (int x = 0; x < grid.width; x++)
        {
            //Debug.WriteLine("X: " + x + " Y: " + y);
            rowBlocks[x] = grid.gridArr[x, y];
        }
    }
    /*public List<SubBlock> GetRowSubBlocks(int y, List<SubBlock> allSubBlocks)
    {
        List<SubBlock> subBlockList = new List<SubBlock>();
        for (int x = 0; x < TetrisGrid.Width; x++)
        {
            if (SubBlock.GetSubBlockAtPosition(x, y, allSubBlocks) != null)
                subBlockList.Add(SubBlock.GetSubBlockAtPosition(x, y, allSubBlocks));
        }
        return subBlockList;
    } */

    public bool IsRowFull()
    {
        for(int x = 0; x < grid.width; x++)
        {
            if(rowBlocks[x] == null)
            {
                return false;
            }
        }
        return true;
    }
    /*public static List<int> GetAllRowsYCoordinates(TetrisGrid grid)
    {
        List<int> rows = new List<int>();
        for(int y = 0; y < grid.correctHeight; y++)
        {
            SubBlockRow row = new SubBlockRow(y, grid);
        }
    } */
    private bool IsRowEmpty()
    {
        for(int x = 0; x < grid.width; x++)
        {
            if(rowBlocks[x] != null)
            {
                return false;
            }
        }
        return true;
    }
    public static List<int> GetAllRowsYCoordinates(TetrisGrid grid, bool isFullOnly)
    {
            List<int> rows = new List<int>();
            for (int y = 0; y < grid.height; y++)
            {
                SubBlockRow row = new SubBlockRow(y, grid);
            if (isFullOnly)
            {
                if (row.IsRowFull())
                    rows.Add(y);
            }
            else
            {
                if (!row.IsRowEmpty())
                {
                    rows.Add(y);
                }
            }
            }
            return rows;
    }

    public void ClearRow()
    {
        for(int x = 0; x < grid.width; x++)
        {
            rowBlocks[x] = null;
            ApplyChanges();
        }
    }

    public void ApplyChanges()
    {
        for(int x = 0; x < grid.width; x++)
        {
            grid.gridArr[x, y] = rowBlocks[x];
        }
    }

    public void Fall()
    {
        for(int x = 0; x < grid.width; x++)
        {
            if (rowBlocks[x] != null)
            {
                //SubBlock subBlock = rowBlocks[x];
                SubBlock subBlock = new SubBlock(x, y, Color.Green, grid);
                if (subBlock.CanMoveTo(x, y + 1))
                {
                    grid.gridArr[x, y] = grid.gridArr[x, y + 1];
                    grid.gridArr[x, y] = null;
                }
            }
            
        }
        y += 1; //verschuift de rij eentje omlaag.
        ApplyChanges();
    }

}

