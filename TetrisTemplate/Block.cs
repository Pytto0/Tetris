﻿
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Linq;

class Block
{

    public SubBlock[] subBlockArray { get; }
    public int Form {get;}

    public Block(int startGridX, int startGridY, int form)
    {
        subBlockArray = GenerateBlock(form, startGridX, startGridY);
        Form = form;
    }

    public void Turn()
    {
        foreach(SubBlock subBlock in subBlockArray)
        {
            int x = subBlock.X;
            int y = subBlock.Y;
            SubBlock turnBlock = getCurrentTurnBlock();
            int tx = turnBlock.X;
            int ty = turnBlock.Y;

            subBlock.X = (y + tx - ty);
            subBlock.Y = (tx + ty - x);
        }
    }
    public SubBlock getCurrentTurnBlock()
    {
        switch (Form)
        {
            case 0:
                return subBlockArray[1];
            case 1:
                return subBlockArray[3];
            case 2:
                return subBlockArray[1];
            case 3:
                return subBlockArray[1];
            case 4:
                return subBlockArray[1];
            case 5:
                return subBlockArray[2];
            default:
                return subBlockArray[0];
        }
    }


    public bool CanFallDown()
    {
        foreach (SubBlock subBlock1 in subBlockArray)
        {
            //Debug.WriteLine("test2: " + TetrisGame.allSubBlocks.ToArray().Length);
            if (TetrisGame.allSubBlocks.ToArray().Length > 0)
            {
                foreach (SubBlock subBlock2 in TetrisGame.allSubBlocks)
                {
                    //Debug.WriteLine("(" + subBlock1.X + "," + (subBlock1.Y + 1) + ")" + " (" + subBlock2.X + "," + subBlock2.Y + ")");
                    if (subBlock1.X == subBlock2.X && (subBlock1.Y + 1) == subBlock2.Y)
                        return false;
                }
            }
            if (subBlock1.Y >= 14)
                return false;
        }
        
        return true;

    }

    public void AddToFallenBlocks()
    {
        foreach(SubBlock subBlock in subBlockArray)
        {
            //Debug.WriteLine("TOTALLENGTH: " + TetrisGame.allSubBlocks.ToArray().Length + " x length: " + subBlock.X + " y length: " + subBlock.Y);
            TetrisGame.allSubBlocks.Add(subBlock);
            
        }
    }

    public void BlockFallDown() //allFallenSubBlocks: een lijst van 1 bij 1 blokjes die of op andere blokjes staan of op de grond staan
      {
         foreach (SubBlock subBlock in subBlockArray)
            {
                subBlock.Y += 1;           
            }
      }

    public SubBlock[] GenerateBlock(int form, int gridX, int gridY) //de eerste subarragridY is ALTIJD de vorm (en dat is dan weer de kleurcode).
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

