using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

/// <summary>
/// A class for representing the Tetris playing grid.
/// </summary>
class TetrisGrid
{
    /// The sprite of a single empty cell in the grid.
    Texture2D emptyCell;

    /// The position at which this TetrisGrid should be drawn.
    Vector2 position;

    List<SubBlock> grid;

    /// The number of grid elements in the x-direction.
    public static int Width { get { return 10; } }
   
    /// The number of grid elements in the y-direction.
    public static int Height { get { return 20; } }

    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    /// <param name="b"></param>
    public TetrisGrid(Texture2D cell)
    {
        emptyCell = cell; 
        position = Vector2.Zero;
        grid = new List<SubBlock>();
        
    }
    
    

    public void UpdateGrid()
    {
        Clear();
        for (int x = (int) position.X; x < Width; x++)
        {
            for (int y = (int)position.Y; y < Height; y++)
            {
                SubBlock gridBlock = new SubBlock(x, y, Color.White);
                if (SubBlock.GetSubBlockAtPosition(x, y) == null)
                    grid.Add(gridBlock);
            }
        }
    }

    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        UpdateGrid();
        grid.AddRange(TetrisGame.allSubBlocks);
        foreach(SubBlock subBlock in grid)
        {
            spriteBatch.Draw(emptyCell, new Vector2(subBlock.X * emptyCell.Width, subBlock.Y * emptyCell.Height), subBlock.Color);
        } 
    }
    /// <summary>
    /// Clears the grid.
    /// </summary>
    public void Clear()
    {
        grid.Clear();
    }
}

