using Microsoft.Xna.Framework;

class Block
{
    public SubBlock[] subBlockArray { get; }
    public int Form { get; }

    public Block(int startGridX, int startGridY, int form)
    {
        subBlockArray = GenerateBlock(form, startGridX, startGridY);
        Form = form;
    }

    public bool CanTurn()
    {
        foreach (SubBlock subBlock1 in subBlockArray)
        {
                    SubBlock turnBlock = GetCurrentTurnBlock();
                    int tx = turnBlock.X;
                    int ty = turnBlock.Y;
                    SubBlock temporarySubBlock = new SubBlock(subBlock1.Y + tx - ty, -subBlock1.X + tx + ty, subBlock1.Color);

                    if (SubBlock.GetSubBlockAtPosition(temporarySubBlock.X, temporarySubBlock.Y) != null || !temporarySubBlock.IsInBounds())
                    { return false; }
                    temporarySubBlock = null;
            
        }
        return true;
    }

    public void Turn()
    {
        foreach (SubBlock subBlock in subBlockArray)
        {
            SubBlock turnBlock = GetCurrentTurnBlock();
            int x = subBlock.X;
            int y = subBlock.Y;
            int tx = turnBlock.X;
            int ty = turnBlock.Y;

            subBlock.X = (y + tx - ty);
            subBlock.Y = (tx + ty - x);
        }
    }

    public bool IsInBounds()
    {
        foreach (SubBlock subBlock in subBlockArray)
        {
            if (!subBlock.IsInBounds())
                return false;
        }
        return true;
    }

    public bool CanMoveRelativeTo(int xChange, int yChange)
    {
        foreach (SubBlock subBlock in subBlockArray)
        {
            SubBlock nextSubBlock = new SubBlock(subBlock.X + xChange, subBlock.Y + yChange, subBlock.Color);

            if(SubBlock.GetSubBlockAtPosition(nextSubBlock.X, nextSubBlock.Y) != null || !nextSubBlock.IsInBounds()){
                return false;
            }
        }
        return true;
    }

    public void MoveRelativeTo(int xChange, int yChange)
    {
        foreach (SubBlock subBlock in subBlockArray)
        {
            subBlock.X += xChange;
            subBlock.Y += yChange;
        }
    }

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
            ////Debug.WriteLine("TOTALLENGTH: " + TetrisGame.allSubBlocks.ToArray().Length + " x length: " + subBlock.X + " y length: " + subBlock.Y);
            TetrisGame.allSubBlocks.Add(subBlock);

        }
    }

    public SubBlock[] GenerateBlock(int form, int gridX, int gridY) //de eerste subarragrid.Y is ALTIJD de vorm (en dat is dan weer de kleurcode).
    {
        Color c = Color.White;
        switch (form)
        {
            case 0:
                c = Color.Yellow;
                // turnBlock = new SubBlock(gridX, 1 + gridY, c);
                return new SubBlock[] { new SubBlock(gridX, gridY, c), new SubBlock(gridX, 1 + gridY, c), new SubBlock(gridX, 2 + gridY, c), new SubBlock(gridX, 3 + gridY, c) }; //vierblokkige staaf
            case 1:
                c = Color.Blue;
                // turnBlock = new SubBlock(1 + gridX, 1 + gridY, c);
                return new SubBlock[] { new SubBlock(1 + gridX, gridY, c), new SubBlock(2 + gridX, gridY, c), new SubBlock(gridX, 1 + gridY, c), new SubBlock(1 + gridX, 1 + gridY, c) }; //omgekeerde "Z"
            case 2:
                c = Color.Red;
                //turnBlock = new SubBlock(1 + gridX, gridY, c);
                return new SubBlock[] { new SubBlock(gridX, gridY, c), new SubBlock(1 + gridX, gridY, c), new SubBlock(1 + gridX, 1 + gridY, c), new SubBlock(2 + gridX, 1 + gridY, c) }; //"Z"
            case 3:
                c = Color.Brown;
                //turnBlock = new SubBlock(gridX, 1 + gridY, c);
                return new SubBlock[] { new SubBlock(2 + gridX, gridY, c), new SubBlock(gridX, 1 + gridY, c), new SubBlock(1 + gridX, 1 + gridY, c), new SubBlock(2 + gridX, 1 + gridY, c) }; //"L"
            case 4:
                c = Color.Green;
                //turnBlock = new SubBlock(gridX, 1 + gridY, c);
                return new SubBlock[] { new SubBlock(gridX, gridY, c), new SubBlock(gridX, 1 + gridY, c), new SubBlock(1 + gridX, 1 + gridY, c), new SubBlock(2 + gridX, 1 + gridY, c) }; //omgekeerde "L"
            case 5:
                c = Color.Purple;
                //turnBlock = new SubBlock(1 + gridX, 1 + gridY, c);
                return new SubBlock[] { new SubBlock(1 + gridX, gridY, c), new SubBlock(gridX, 1 + gridY, c), new SubBlock(1 + gridX, 1 + gridY, c), new SubBlock(2 + gridX, 1 + gridY, c) }; //"T"
            default:
                c = Color.Aqua;
                //turnBlock = new SubBlock(gridX, gridY, c);
                return new SubBlock[] { new SubBlock(gridX, gridY, c), new SubBlock(1 + gridX, gridY, c), new SubBlock(gridX, 1 + gridY, c), new SubBlock(1 + gridX, 1 + gridY, c) }; //vierkant blok
        }
    }
}


