using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// A class for representing the Tetris playing grid.
/// </summary>
class TetrisGrid
{
    /// The sprite of a single empty cell in the grid.
    Texture2D emptyCell;

    /// The position at which this TetrisGrid should be drawn.

    public SubBlock[,] gridArr { get; set; }
    GameWorld gameWorld;

    /// The number of grid elements in the x-direction.
    public int width { get; }
   
    /// The number of grid elements in the y-direction.
    public int height { get; }

    public Vector2 position { get; }

    public int correctHeight { get; }
    public int correctWidth { get; }

    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    /// <param name="b"></param>
    public TetrisGrid(Texture2D cell, Vector2 Position, GameWorld G, int Height, int Width)
    {
        gameWorld = G;
        emptyCell = cell;
        position = Position;
        width = Width;
        height = Height;
        correctHeight = (int) (height + position.Y);
        correctWidth = (int) (width + position.X);
        // correctHeight = (int)(Math.Abs(position.Y) + Math.Abs(Height));
        // correctWidth = (int)(Math.Abs(position.X) + Math.Abs(Width));
        gridArr = new SubBlock[width, height];
        Clear();
        
    }

    
    

    /*private void UpdateGrid()
    {
        gridArr = gameWorld.allSubBlocks;
        /*Clear();
        for (int x = (int) position.X; x < Width; x++)
        {
            for (int y = (int)position.Y; y < Height; y++)
            {
                if(gameWorld.allSubBlocks[x, y])
                SubBlock gridBlock = new SubBlock(x, y, Color.White, gameWorld);
                if (SubBlock.GetSubBlockAtPosition(x, y) == null)
                    grid.Add(gridBlock);
            }
        } 
    } */

    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
       // UpdateGrid();
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                int correctedX = (int)(x +position.X);
                int correctedY = (int)(y +position.Y);
                //Debug.WriteLine("x: " + x + " Y ; " + y);
                if (gridArr[x, y] != null)
                {
                    SubBlock subBlock = gridArr[x, y];

                    spriteBatch.Draw(emptyCell, new Vector2(correctedX * emptyCell.Width, correctedY * emptyCell.Height), subBlock.color);
                }
                else
                {
                    spriteBatch.Draw(emptyCell, new Vector2(correctedX * emptyCell.Width, correctedY * emptyCell.Height), Color.White);
                }
            }
        }
       /* foreach(SubBlock subBlock in grid)
        {
            spriteBatch.Draw(emptyCell, new Vector2(subBlock.X * emptyCell.Width, subBlock.Y * emptyCell.Height), subBlock.Color);
        }*/ 
    }
    /// <summary>
    /// Clears the grid.
    /// </summary>
    public void Clear()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                gridArr[x, y] = null;
            }
        }
    }

    public static bool IsInBounds(int x, int y, TetrisGrid grid)
    {
        if (x < 0|| x >= grid.width|| y < 0 || y >= grid.height)
            return false;
        return true;
    }
}

