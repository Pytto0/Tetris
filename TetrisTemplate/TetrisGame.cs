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
    public static int maxLevel { get; private set; } = normalModeMaxLevel;
    public static float currentGameTime { get; private set; } = 0f;
    private const int amountOfBlockForms = 7, normalModeMaxLevel = 10, hardModeMaxLevel = 15;
    private const float constBlockWaitTime = 1f, constDownWaitTime = 0.5f;
    private float blockWaitTime = constBlockWaitTime, blockTimeCounter, downWaitTime = constDownWaitTime, downTimeCounter;
    public static int difficulty = 0;

    /// <summary>
    /// A static reference to the ContentManager object, used for loading assets.
    /// </summary>
    public static ContentManager ContentManager { get; private set; }
    public static List<SubBlock> allSubBlocks { get; private set; } //we willen dit alleen maar vanuit deze klasse kunnen aanpassen (opvragen mag altijd).
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

        allSubBlocks = new List<SubBlock> { };         //Lijst van alle blokjes die momenteel stil staan en niet meer kunnen bewegen
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

    public bool CheckIfGameOver()
    {
        foreach (SubBlock subBlock in allSubBlocks)
        {
            if (subBlock.Y <= 0)
                return true; 
        }
        return false;
    }

    private void Reset()
    {
        score = 0;
        level = 1;
        currentGameTime = 0f;
        allSubBlocks.Clear();
        blockWaitTime = constBlockWaitTime;
        downWaitTime = constDownWaitTime;
        currentBlock = null;
       // gameWorld.gameState = GameWorld.GameState.NotStarted; 

    }

    /*private void ExecuteKeyOrders()
    {
       
    } */
    private void RemoveFullRows()
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

    /*public void SetLevelLimitTo(int x)
    {
        if (level < x)
            downTimeCounter = level * 0.03f;
        else
            downTimeCounter = x * 0.03f;
    } */

  /*  public bool isPlaying()
    {
        if(gameWorld.gameState == GameWorld.GameState.Playing)
        {
            return true;
        }
        return false;
    } */

    private void BlockFallDown()
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
        downTimeCounter = 0;
    }

    private void CreateNewBlock()
    {
        currentBlock = new Block(4, -3, nextBlock.Form);
        nextBlock = new Block(15, 3, GameWorld.Random.Next(0, amountOfBlockForms));
        blockTimeCounter = 0;
    }

    private int GetCorrectLevel()
    {
        int levelHolder = (int)(Math.Floor((double)(score / 100)) + 1);
        //Debug.WriteLine(levelHolder);
        if (levelHolder > maxLevel)
            return maxLevel;
        else
        {
            return levelHolder;
        }
    }

    /*public void SetGameMode()
    {
        if (inputHelper.KeyPressed(Keys.D1) && gameWorld.gameState == GameWorld.GameState.Playing)
            SetLevelLimitTo(11);
        else if (inputHelper.KeyPressed(Keys.D2) && gameWorld.gameState == GameWorld.GameState.Playing)
            SetLevelLimitTo(16);
    } */

    public void HandleInput(GameTime gameTime)
    {
        //KeyboardState kbs = Keyboard.GetState();
        inputHelper.Update(gameTime);

        List<Keys> keys = inputHelper.GetPressedKeys();
        if (keys != null)
        {
            GameWorld.GameState gs = gameWorld.gameState;
            ////Debug.WriteLine(keys.Count);
            foreach (Keys key in keys)
            {
                switch (key)
                {
                    case Keys.Up:
                        //Debug.WriteLine("test3");
                        if (currentBlock != null && currentBlock.CanTurn())
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
                    case Keys.Space:
                        //Debug.WriteLine("test325");
                        if (gs != GameWorld.GameState.Playing)
                        {
                            gameWorld.gameState = GameWorld.GameState.Playing;
                            Reset();
                        }
                        break;
                    case Keys.Escape:
                        Exit();
                        break;
                    case Keys.D1:
                        //Debug.WriteLine("test2");
                        if (gs == GameWorld.GameState.NotStarted)
                            maxLevel = normalModeMaxLevel;
                        break;
                    case Keys.D2:
                        if (gs == GameWorld.GameState.NotStarted)
                            maxLevel = hardModeMaxLevel;
                        break;

                }
            }
        }

       
    }


    protected override void Update(GameTime gameTime)
    {
       // //Debug.WriteLine("Update TetrisGame");
        if (gameWorld.gameState == GameWorld.GameState.Playing)
        {
            level = GetCorrectLevel();
            //Debug.WriteLine("a: " + GetCorrectLevel());
            //Debug.WriteLine("b: " + level);
            if (currentBlock != null)
                {
                ////Debug.WriteLine(downTimeCounter);
                    if (downWaitTime <= downTimeCounter)
                        BlockFallDown();
                    if (inputHelper.KeyDown(Keys.Down))
                        downTimeCounter += 4 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (blockWaitTime <= blockTimeCounter)
                    CreateNewBlock();
            if (CheckIfGameOver())
            {
                gameWorld.gameState = GameWorld.GameState.GameOver;
            }
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            downTimeCounter += (float) (0.25 * (level-1) * elapsedTime) + elapsedTime; //nu heb je bij level 5 twee keer zo snel, bij level 9 drie keer zo snel en bij level 13 vier keer zo snel.
            blockTimeCounter += elapsedTime;
            currentGameTime += elapsedTime;

        }


        //gameWorld.HandleInput(gameTime, inputHelper);
        HandleInput(gameTime);
        //gameWorld.Update(gameTime);
       // Update(gameTime); 
    }

    protected override void Draw(GameTime gameTime)
    {
        spriteBatch.Begin();
        GraphicsDevice.Clear(Color.White);
        gameWorld.Draw(gameTime, spriteBatch, currentBlock, nextBlock, emptyCell);
        spriteBatch.End();
    }
}

