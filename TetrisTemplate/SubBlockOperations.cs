
using Microsoft.Xna.Framework;
using System.Collections.Generic;

class SubBlockOperations
{

    public static List<SubBlock> GetRowSubBlocks(int y)
    {
        List<SubBlock> subBlockList = new List<SubBlock>();
        for (int x = 0; x < TetrisGrid.Width; x++)
        {
            if(GameWorld.GetSubBlockAtPosition(x, y) != null)
                subBlockList.Add(GameWorld.GetSubBlockAtPosition(x, y));
        }
        return subBlockList;
    }

    /*public static void RemoveSubBlockAtPosition(int x, int y)
    {
        foreach(SubBlock subBlock in TetrisGame.allSubBlocks)
        {
            if(subBlock.X == x && subBlock.Y == y)
            {
                TetrisGame.allSubBlocks.Remove(subBlock);
            }
        }
    } */


    public static bool IsRowFull(int y)
    {
        if (GetRowSubBlocks(y).Count == TetrisGrid.Width) //als er evenveel blokjes in deze rij zitten als de lengte van een rij, dan hebben we een volle rij
            return true;
        else
            return false;
    }

    public static List<int> GetAllRowsYCoordinates()
    {
        List<int> Rows = new List<int>();
        {
            for (int y = 0; y < TetrisGrid.Height; y++)
            {
                if (IsRowFull(y))
                { Rows.Add(y); }
            }
            return Rows;
        }
    }

    public static void ClearRow(int y)
    {
        List<SubBlock> rowSubBlocks = GetRowSubBlocks(y);
        foreach(SubBlock subBlock in rowSubBlocks)
        {
            TetrisGame.allSubBlocks.Remove(subBlock);
        }

    }

}

