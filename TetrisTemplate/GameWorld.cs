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
    InputHelper inputHelper;
    TetrisGame tetrisGame;
    public int score, level;
    public float currentGameTime = 0f;
    /// <summary>
    /// An enum for the different game states that the game can have.
    /// </summary>
    public enum GameState
    {
        GameOver,
        Playing,
        HardMode
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

    public GameWorld()
    {
        Random random = new Random();
        gameState = GameState.Playing;
        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");
        //grid = new TetrisGrid();
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
    }

    public void Update(GameTime gameTime)
    {
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        /*if (state == 1){
        spriteBatch.Begin();
        grid.Draw(gameTime, spriteBatch);
        spriteBatch.DrawString(font, "Hello!", new Vector2(30*TetrisGrid.Width + 20, 5*TetrisGrid.Height), Color.Blue);
        spriteBatch.End(); 
        }*/
        //Debug.WriteLine("Test One");

    }

    public void Reset()
    {
    }
}
