using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// A class for representing the Tetris playing grid.
/// </summary>
class TetrisGrid
{
    /// The sprite of a single empty cell in the grid.
    Texture2D emptyCell;

    /// The position at which this TetrisGrid should be drawn.
    Vector2 position;

    SpriteBatch spriteBatch;

    /// The number of grid elements in the x-direction.
    public int Width { get { return 10; } }
   
    /// The number of grid elements in the y-direction.
    public int Height { get { return 20; } }

    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    /// <param name="b"></param>
    public TetrisGrid()
    {
        //emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        position = Vector2.Zero;
        Clear();
    }
    public static int[][] generateForm(int form, int x, int y)
    {
        switch (form)
        {
            case 0:
                return new int[][] { new int[] { x, y, form }, new int[] { x, 1 + y, form }, new int[] { x, 2 + y, form }, new int[] { x, 3 + y, form } }; //vierblokkige staaf
            case 1:
                return new int[][] { new int[] { 1 + x, y, form }, new int[] { 2 + x, y, form }, new int[] { x, 1 + y, form }, new int[] { 1 + x, 1 + y, form } }; //omgekeerde "Z"
            case 2:
                return new int[][] { new int[] { x, y, form }, new int[] { 1 + x, y, form }, new int[] { 1 + x, 1 + y, form }, new int[] { 2 + x, 1 + y, form } }; //"Z"
            case 3:
                return new int[][] { new int[] { 2 + x, y, form }, new int[] { x, 1 + y, form }, new int[] { 1 + x, 1 + y, form }, new int[] { 2 + x, 1 + y, form } }; //"L"
            case 4:
                return new int[][] { new int[] { x, y, form }, new int[] { x, 1 + y, form }, new int[] { 1 + x, 1 + y, form }, new int[] { 2 + x, 1 + y, form } }; //omgekeerde "L"
            case 5:
                return new int[][] { new int[] { 1 + x, y, form }, new int[] { x, 1 + y, form }, new int[] { 1 + x, 1 + y, form }, new int[] { 2 + x, 1 + y, form } }; //"T"
            default:
                return new int[][] { new int[] { x, y, form }, new int[] { 1 + x, y, form }, new int[] { x, 1 + y, form }, new int[] { 1 + x, 1 + y, form } }; //vierkant blok
        }

    }
    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        /*spriteBatch.Begin();
        //emptyCell = tetrisGrid;
        for (int x = 0; x < this.Width * emptyCell.Width; x += emptyCell.Width)
        {
            for (int y = 0; y < this.Height * emptyCell.Height; y += emptyCell.Height)
            {
                spriteBatch.Draw(emptyCell, new Vector2(x, y), Color.White);
            }
        }
        spriteBatch.End(); */
    }

    /// <summary>
    /// Clears the grid.
    /// </summary>
    public void Clear()
    {
    }
}

