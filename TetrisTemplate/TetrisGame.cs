using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

class TetrisGame : Game
{
    SpriteBatch spriteBatch;
    InputHelper inputHelper;
    GameWorld gameWorld, gameWorld2;

    public int players { get; private set; } = 1;
    Texture2D emptyCell;
    SpriteFont font;
    List<GameWorld> gameWorlds = new List<GameWorld>();
    public static int maxLevel { get; private set; } = normalModeMaxLevel;
    public static bool multiplayer = false;
    private const int normalModeMaxLevel = 10, hardModeMaxLevel = 15, gridWidth = 8, gridHeight = 25;
    Keys[] p1KeySet, p2KeySet;
    public enum GameState
    {
        NotStarted,
        GameOver,
        Playing

    }
    public static GameState gameState;

    //Dit hier is onze host class. Deze class gaan we direct aan het begin van het spel aanmaken. Afhankelijk van de hoeveelheid spelers, maken we een of meerdere gameWorlds aan.

    /// <summary>
    /// A static reference to the ContentManager object, used for loading assets.
    /// </summary>
    public static ContentManager ContentManager { get; private set; }
    /// <summary>
    /// A static reference to the width and height of the screen.
    /// </summary>
    public static Point ScreenSize { get; private set; }

    [STAThread]
    static void Main(string[] args)
    {
        TetrisGame game = new TetrisGame();
        game.Run();
    }
    
    public TetrisGame()
    {        
        // initialize the graphics device
        GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);

        // store a static reference to the content manager, so other objects can use it
        ContentManager = Content;
        
        // set the directory where game assets are located
        Content.RootDirectory = "Content";
        // set the desired window size
        ScreenSize = new Point(1280, 768);
        graphics.PreferredBackBufferWidth = ScreenSize.X;
        graphics.PreferredBackBufferHeight = ScreenSize.Y;

        // create the input helper object
        inputHelper = new InputHelper();
        gameState = GameState.NotStarted;
        p1KeySet = new Keys[4] { Keys.Up, Keys.Right, Keys.Down, Keys.Left };
        p2KeySet = new Keys[4] { Keys.W, Keys.D, Keys.S, Keys.A };


    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        emptyCell = Content.Load<Texture2D>("Block");
        // create and reset the game world
        font = ContentManager.Load<SpriteFont>("SpelFont");

        gameWorld = new GameWorld(emptyCell, font, new Vector2(0, -3), gridHeight, gridWidth, p1KeySet);
        gameWorlds.Add(gameWorld);
 
    }


    public void HandleInput(GameTime gameTime)
    {
        //KeyboardState kbs = Keyboard.GetState();
        inputHelper.Update(gameTime);
        //Debug.WriteLine("test2");
        List<Keys> keys = inputHelper.GetPressedKeys();
        if (keys != null)
        {
            //Debug.WriteLine(keys.Count);
            foreach (Keys key in keys)
            {
                switch (key)
                {
                    
                    case Keys.Space:
                        //Debug.WriteLine("test325");
                        if (gameState != GameState.Playing)
                        {
                            gameState = GameState.Playing;
                            foreach(GameWorld GW in gameWorlds) {
                                GW.Reset(gameTime);
                            }
                        }
                        break;
                    case Keys.D1:
                        //Debug.WriteLine("test2");
                        if (gameState == GameState.NotStarted)
                        //maxLevel = normalModeMaxLevel;
                        {
                            gameState = GameState.Playing;
                            foreach (GameWorld GW in gameWorlds)
                            {
                                GW.Reset(gameTime);
                            }
                        }
                        break;
                    case Keys.D2:
                        if (gameState == GameState.NotStarted)
                        {
                            //maxLevel = hardModeMaxLevel;
                            gameWorld2 = new GameWorld(emptyCell, font, new Vector2(25, -3), gridHeight, gridWidth, p2KeySet);
                            gameWorlds.Add(gameWorld2);
                            multiplayer = true;
                            gameState = GameState.Playing;
                            foreach (GameWorld GW in gameWorlds)
                            {
                                GW.Reset(gameTime);
                            }
                        }
                        break;
                    case Keys.Escape:
                        Exit();
                        break;

                }
            }
        }


    }

    private void DrawStartGameText(SpriteBatch spriteBatch)
    {
        if (gameState == GameState.NotStarted)
        {
            spriteBatch.DrawString(font, "Druk op de nummer 1 knop om in je eentje te spelen.", new Vector2((gameWorld.grid.width + 3) * emptyCell.Width, 300), Color.Black);
            spriteBatch.DrawString(font, "Druk op de nummer 2 knop om met zijn tweeen te spelen.", new Vector2((gameWorld.grid.width + 3) * emptyCell.Width, 400), Color.Black);
        }

    }

    protected override void Update(GameTime gameTime)
    {
        HandleInput(gameTime);
        foreach(GameWorld GW in gameWorlds)
        {
            GW.Update(gameTime);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        spriteBatch.Begin();
        GraphicsDevice.Clear(Color.White);
        if (gameState == GameState.NotStarted)
            DrawStartGameText(spriteBatch);
        foreach (GameWorld GW in gameWorlds)
        {
            GW.Draw(gameTime, spriteBatch);
        }
        //gameWorld.Draw(gameTime, spriteBatch);
        spriteBatch.End();
    }
}

