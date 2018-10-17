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
    GameWorld gameWorld;
    Texture2D emptyCell;
    TetrisGrid tetrisGrid;
    SpriteFont font;
    public static int score { get; private set; }
    public static int level { get; private set; }
    public static float currentGameTime { get; private set; } = 0f;
    private const int amountOfBlockForms = 7;
    private const float constBlockWaitTime = 1f, constDownWaitTime = 0.5f;
    private float blockWaitTime = constBlockWaitTime, blockTimeCounter, downWaitTime = constDownWaitTime, downTimeCounter;
    public static int difficulty = 0;

    /// <summary>
    /// A static reference to the ContentManager object, used for loading assets.
    /// </summary>
    public static ContentManager ContentManager { get; private set; }
    public static List<SubBlock> allSubBlocks { get; private set; } //we willen dit alleenmaar vanuit deze klasse kunnen aanpassen (opvragen mag altijd).
    public Block currentBlock, nextBlock;
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
        ScreenSize = new Point(800, 600);
        graphics.PreferredBackBufferWidth = ScreenSize.X;
        graphics.PreferredBackBufferHeight = ScreenSize.Y;

        // create the input helper object
        inputHelper = new InputHelper();

        allSubBlocks = new List<SubBlock> { };         //Lijst van alle blokjes die momenteel stil staan en niet meer kunnen bewgen
        nextBlock = new Block(15, 3, GameWorld.Random.Next(0, amountOfBlockForms));

    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        emptyCell = Content.Load<Texture2D>("Block");
        tetrisGrid = new TetrisGrid(emptyCell);
        // create and reset the game world
        font = ContentManager.Load<SpriteFont>("SpelFont");
        gameWorld = new GameWorld(emptyCell);
        Reset();
    }

    public bool IsGameOver()
    {
        foreach (SubBlock subBlock in allSubBlocks)
        {
            if (subBlock.Y <= 0)
                return true; 
        }
        return false;
    }

    public void Reset()
    {
        score = 0;
        level = 1;
        currentGameTime = 0f;
        allSubBlocks.Clear();
        blockWaitTime = constBlockWaitTime;
        downWaitTime = constDownWaitTime;
        currentBlock = null;
    }

    public void ExecuteKeyOrders()
    {
        KeyboardState kbs = Keyboard.GetState();
        Keys[] keys = kbs.GetPressedKeys();
        foreach (Keys key in inputHelper.GetPressedKeys())
        {
            switch (key)
            { 
                case Keys.Up:
                    if(currentBlock != null && currentBlock.CanTurn() )
                        currentBlock.Turn();
                    break;
                case Keys.Right:
                    if (currentBlock != null && currentBlock.CanMoveRelativeTo(1, 0))
                        currentBlock.MoveRelativeTo(1, 0);
                    break;
                case Keys.Left:
                    if (currentBlock != null && currentBlock.CanMoveRelativeTo(-1, 0))
                        currentBlock.MoveRelativeTo(-1, 0);
                    break;
            }
        }
    }
    public void RemoveFullRows()
    {
        
        List<int> YCoordinates = SubBlockRow.GetAllRowsYCoordinates();
        foreach (int y in YCoordinates)
        {
            if (SubBlockRow.IsRowFull(y))
            {
                SubBlockRow.ClearRow(y);
                SubBlockRow.Fall(y);
                score += 30;
            }
        }
    }

    public void SetLimitTo(int x)
    {
        if (level < x)
            downTimeCounter = level * 0.03f;
        else
            downTimeCounter = x * 0.03f;
    }

  /*  public bool isPlaying()
    {
        if(gameWorld.gameState == GameWorld.GameState.Playing)
        {
            return true;
        }
        return false;
    } */

    public void BlockFallDown()
    {
        if (currentBlock.CanMoveRelativeTo(0, 1))
            currentBlock.MoveRelativeTo(0, 1);
        else
        {
            currentBlock.AddToSubBlocks();
            score += 10;
            currentBlock = null;
            RemoveFullRows();
        }
    }

    public void CreateNewBlock()
    {
        currentBlock = new Block(4, -3, nextBlock.Form);
        nextBlock = new Block(15, 3, GameWorld.Random.Next(0, amountOfBlockForms));
        blockTimeCounter = 0;
    }

    public void SetGameMode(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.D1) && gameWorld.gameState == GameWorld.GameState.Playing)
            SetLimitTo(11);
        else if (inputHelper.KeyPressed(Keys.D2) && gameWorld.gameState == GameWorld.GameState.Playing)
            SetLimitTo(16);
    }

    public void HandleInput(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Space) && gameWorld.gameState == GameWorld.GameState.NotStarted)
        {
            gameWorld.gameState = GameWorld.GameState.Playing;
            Reset();
        }

        if (inputHelper.KeyPressed(Keys.Space) && (gameWorld.gameState != GameWorld.GameState.Playing))
        {
            gameWorld.gameState = GameWorld.GameState.Playing;
            Reset();
        }
        if (inputHelper.KeyPressed(Keys.Escape))
            Exit();
        if (IsGameOver())
            gameWorld.gameState = GameWorld.GameState.GameOver;
        if (inputHelper.KeyDown(Keys.LeftShift) && gameWorld.gameState == GameWorld.GameState.Playing)
            SetGameMode(inputHelper);
    }


    protected override void Update(GameTime gameTime)
    {
        HandleInput(gameTime);
        if (gameWorld.gameState == GameWorld.GameState.Playing)
        {
            Debug.WriteLine("Update TetrisGame");
            level = (int)(Math.Floor((double)(score / 100)) + 1);

                if (currentBlock != null)
                {
                    ExecuteKeyOrders();
                    if (downWaitTime <= downTimeCounter)
                        BlockFallDown();
                    if (inputHelper.KeyDown(Keys.Down))
                        downTimeCounter += 4 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (blockWaitTime <= blockTimeCounter)
                    CreateNewBlock();

            downTimeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            blockTimeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (gameWorld.gameState != GameWorld.GameState.GameOver || gameWorld.gameState != GameWorld.GameState.NotStarted)
                currentGameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            inputHelper.Update(gameTime);
            //gameWorld.HandleInput(gameTime, inputHelper);
            gameWorld.Update(gameTime);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        spriteBatch.Begin();
        GraphicsDevice.Clear(Color.White);
        gameWorld.Draw(gameTime, spriteBatch, currentBlock, nextBlock, emptyCell);
        spriteBatch.End();
    }
}

