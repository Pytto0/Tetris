using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

class Block
{
    public SubBlock[] subBlockArray { get; }
    public int Form { get; }
    public TetrisGrid grid { get; }

    public Block(int startGridX, int startGridY, int form, TetrisGrid Grid)
    {
        subBlockArray = GenerateBlock(form, startGridX, startGridY);
        Form = form;
        grid = Grid;
    }

    public bool CanTurn()
    {
        foreach (SubBlock subBlock in subBlockArray)
        {
            SubBlock turnBlock = GetCurrentTurnBlock();
            int tx = turnBlock.x;
            int ty = turnBlock.y;
            SubBlock temporarySubBlock = new SubBlock(subBlock.y + tx - ty, -subBlock.x + tx + ty, subBlock.color, grid);
            if (TetrisGrid.IsInBounds(temporarySubBlock.x, temporarySubBlock.y, grid))
            {
                if (grid.gridArr[temporarySubBlock.x, temporarySubBlock.y] != null) 
                    return false;
            }
            else
            {
                return false;
            }
            temporarySubBlock = null;

        }
        return true;
    }

    public void Turn()
    {
        foreach (SubBlock subBlock in subBlockArray)
        {
            SubBlock turnBlock = GetCurrentTurnBlock();
            int x = subBlock.x;
            int y = subBlock.y;
            int tx = turnBlock.x;
            int ty = turnBlock.y;

            subBlock.x = (y + tx - ty);
            subBlock.y = (tx + ty - x);
        }
    }

    public bool IsBlockInBounds() //Let op: deze functie is anders dan de TetrisGrid.IsInBounds functie. TetrisGrid.IsInBounds kijkt alleen naar of een blokje in 
    {
        foreach (SubBlock subBlock in subBlockArray)
        {
            //if (subBlock.x < (int) grid.position.X || subBlock.x > (int)(grid.position.X + grid.width)|| subBlock.y < (int)grid.position.Y || subBlock.y > (int) (grid.position.Y + grid.height)) 
            if(TetrisGrid.IsInBounds(subBlock.x, subBlock.y, grid))
                return false;
        }
        return true;
    }

    public bool CanMoveRelativeTo(int xChange, int yChange)
    {
        foreach (SubBlock subBlock in subBlockArray)
        {
            int newX = subBlock.x + xChange;
            int newY = subBlock.y + yChange;
            
            if (TetrisGrid.IsInBounds(newX, newY, grid))
            {
                Debug.WriteLine("x: " + newX + " y: " + newY + "test1");
                if (grid.gridArr[newX, newY] != null)
                {
                    Debug.WriteLine("af1");
                    //Debug.WriteLine("x-as width: " + grid.gridArr.GetLength(0) + " y-as height: " + grid.gridArr.GetLength(1));
                    return false;
                }
            }
            else
            {
                Debug.WriteLine("af2");
                return false;
            }
        }
        return true;
    }

    public void MoveRelativeTo(int xChange, int yChange)
    {
        foreach (SubBlock subBlock in subBlockArray)
        {
            //grid.gridArr[subBlock.x + xChange, subBlock.y + yChange] = grid.gridArr[subBlock.x, subBlock.y];
            //grid.gridArr[subBlock.x, subBlock.y] = null;
            subBlock.x += xChange;
            subBlock.y += yChange;
            
        }
        //ApplyChanges();
    }

    /*private void ApplyChanges()
    {
        foreach(SubBlock subBlock in subBlockArray)
        {
            grid.gridArr[subBlock.x, subBlock.y] = subBlock;
        }
    }*/

    public SubBlock GetCurrentTurnBlock()
    {
        switch (Form)
        {
            case 0:
                return subBlockArray[1]; //Vier blokken onder elkaar
            case 1:
                return subBlockArray[3]; //Omgekeerde Z
            case 2:
                return subBlockArray[1]; //Z
            case 3:
                return subBlockArray[2]; //L
            case 4:
                return subBlockArray[1]; //omgekeerde L
            case 5:
                return subBlockArray[2]; //T
            default:
                return subBlockArray[1]; //Vierkant blok
        }
    }

    public void AddToSubBlocks()
    {
        foreach (SubBlock subBlock in subBlockArray)
        {
            if (TetrisGrid.IsInBounds(subBlock.x, subBlock.y, grid))
            {
                grid.gridArr[subBlock.x, subBlock.y] = subBlock;
            }
        }
    }

    public SubBlock[] GenerateBlock(int form, int gridX, int gridY) //de eerste subarragrid.Y is ALTIJD de vorm (en dat is dan weer de kleurcode).
    {
        Color c = Color.White;
        switch (form)
        {
            case 0:
                c = Color.Yellow;
                // turnBlock = new SubBlock(gridX, 1 + gridY, c, grid);
                return new SubBlock[] { new SubBlock(gridX, gridY, c, grid), new SubBlock(gridX, 1 + gridY, c, grid), new SubBlock(gridX, 2 + gridY, c, grid), new SubBlock(gridX, 3 + gridY, c, grid) }; //vierblokkige staaf
            case 1:
                c = Color.Blue;
                // turnBlock = new SubBlock(1 + gridX, 1 + gridY, c, grid);
                return new SubBlock[] { new SubBlock(1 + gridX, gridY, c, grid), new SubBlock(2 + gridX, gridY, c, grid), new SubBlock(gridX, 1 + gridY, c, grid), new SubBlock(1 + gridX, 1 + gridY, c, grid) }; //omgekeerde "Z"
            case 2:
                c = Color.Red;
                //turnBlock = new SubBlock(1 + gridX, gridY, c, grid);
                return new SubBlock[] { new SubBlock(gridX, gridY, c, grid), new SubBlock(1 + gridX, gridY, c, grid), new SubBlock(1 + gridX, 1 + gridY, c, grid), new SubBlock(2 + gridX, 1 + gridY, c, grid) }; //"Z"
            case 3:
                c = Color.Brown;
                //turnBlock = new SubBlock(gridX, 1 + gridY, c, grid);
                return new SubBlock[] { new SubBlock(2 + gridX, gridY, c, grid), new SubBlock(gridX, 1 + gridY, c, grid), new SubBlock(1 + gridX, 1 + gridY, c, grid), new SubBlock(2 + gridX, 1 + gridY, c, grid) }; //"L"
            case 4:
                c = Color.Green;
                //turnBlock = new SubBlock(gridX, 1 + gridY, c, grid);
                return new SubBlock[] { new SubBlock(gridX, gridY, c, grid), new SubBlock(gridX, 1 + gridY, c, grid), new SubBlock(1 + gridX, 1 + gridY, c, grid), new SubBlock(2 + gridX, 1 + gridY, c, grid) }; //omgekeerde "L"
            case 5:
                c = Color.Purple;
                //turnBlock = new SubBlock(1 + gridX, 1 + gridY, c, grid);
                return new SubBlock[] { new SubBlock(1 + gridX, gridY, c, grid), new SubBlock(gridX, 1 + gridY, c, grid), new SubBlock(1 + gridX, 1 + gridY, c, grid), new SubBlock(2 + gridX, 1 + gridY, c, grid) }; //"T"
            default:
                c = Color.Aqua;
                //turnBlock = new SubBlock(gridX, gridY, c, grid);
                return new SubBlock[] { new SubBlock(gridX, gridY, c, grid), new SubBlock(1 + gridX, gridY, c, grid), new SubBlock(gridX, 1 + gridY, c, grid), new SubBlock(1 + gridX, 1 + gridY, c, grid) }; //vierkant blok
        }
    }
}


