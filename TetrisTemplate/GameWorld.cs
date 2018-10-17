using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

/// <summary>
/// A class for representing the game world.
/// This contains the grid, the falling block, and everything else that the player can see/do.
/// </summary>
class GameWorld
{
    //InputHelper inputHelper;
    /// <summary>
    /// An enum for the different game states that the game can have.
    /// </summary>
    public enum GameState
    {
        NotStarted,
        GameOver,
        Playing

    }

    /// <summary>
    /// The random-number generator of the game.
    /// </summary>
    public static Random Random { get { return new Random(); } }
    /// <summary>
    /// The main font of the game.
    /// </summary>
    SpriteFont font;

    /// <summary>
    /// The current game state.
    /// </summary>
    public GameState gameState;

    /// <summary>
    /// The main grid of the game.
    /// </summary>
    TetrisGrid grid;

    public GameWorld(Texture2D emptyCell)
    {
        Random random = new Random();
        gameState = GameState.NotStarted;
        grid = new TetrisGrid(emptyCell);
        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");
        //grid = new TetrisGrid();
    }


    public void Update(GameTime gameTime)
    {
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Block currentBlock, Block nextBlock, Texture2D emptyCell)
    {
        if (gameState == GameState.NotStarted)
            spriteBatch.DrawString(font, "Druk op de spatiebalk om te beginnen.", new Vector2(400, 300), Color.Black);
        if (gameState == GameState.GameOver)
        {
            spriteBatch.DrawString(font, "Je hebt verloren. Jouw score is: " + TetrisGame.score, new Vector2(420, 260), Color.Black);
            spriteBatch.DrawString(font, "Druk op de spatiebalk om opnieuw te beginnen.", new Vector2(360, 300), Color.Black);
        }
        grid.Draw(gameTime, spriteBatch);

        if (currentBlock != null)
        {
            foreach (SubBlock subBlock in currentBlock.subBlockArray)
                spriteBatch.Draw(emptyCell, new Vector2(subBlock.X * emptyCell.Width, subBlock.Y * emptyCell.Height), subBlock.Color);
            foreach (SubBlock subBlock in nextBlock.subBlockArray)
                spriteBatch.Draw(emptyCell, new Vector2(subBlock.X * emptyCell.Width, subBlock.Y * emptyCell.Height), subBlock.Color);
        }
        if (TetrisGame.allSubBlocks.Count > 0)
        {
            foreach (SubBlock subBlock in TetrisGame.allSubBlocks)
                spriteBatch.Draw(emptyCell, new Vector2(subBlock.X * emptyCell.Width, subBlock.Y * emptyCell.Height), subBlock.Color);
        }
        string currentlevel = "Level: " + TetrisGame.level.ToString();
        string scorecount = "Score: " + TetrisGame.score.ToString();
        string passedTime = "Time: " + Math.Floor(TetrisGame.currentGameTime).ToString();
        spriteBatch.DrawString(font, currentlevel, new Vector2(500, 460), Color.Blue);
        spriteBatch.DrawString(font, scorecount, new Vector2(500, 480), Color.Blue);
        spriteBatch.DrawString(font, passedTime, new Vector2(500, 500), Color.Blue);
    }

    //public void Reset()
    //{
    //}
}
