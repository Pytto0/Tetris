using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

class TetrisGame : Game
{
    SpriteBatch spriteBatch;
    InputHelper inputHelper;
    GameWorld gameWorld;
    Texture2D emptyCell;
    TetrisGrid tetrisGrid;
    SpriteFont font;
    int[][][] allBlocks = new int[0][][];
    public int score, level;
    const float constBlockWaitTime = 1f, constDownWaitTime = 0.5f;
    float blockWaitTime = constBlockWaitTime, blockTimeCounter, downWaitTime = constDownWaitTime, downTimeCounter, currentGameTime = 0f;

    /// <summary>
    /// A static reference to the ContentManager object, used for loading assets.
    /// </summary>
    public static ContentManager ContentManager { get; private set; }
    public static List<SubBlock> allSubBlocks { get; private set; } //we willen dit alleenmaar vanuit deze klasse kunnen aanpassen (opvragen mag altijd).
    public Block currentBlock;
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

        tetrisGrid = new TetrisGrid();

        allSubBlocks = new List<SubBlock> { };
        List<int[][]> fallenSubBlockList = new List<int[][]>(); //lijst van alle blokjes die momenteel stil staan en niet meer kunnen bewgen
        //int[][] currentBlock = new int[1][]; //lijst van alle grote blokken die momenteel stil staan.
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        emptyCell = Content.Load<Texture2D>("Block");
        // create and reset the game world
        font = ContentManager.Load<SpriteFont>("SpelFont");
        gameWorld = new GameWorld();
        gameWorld.Reset();
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

    protected override void Update(GameTime gameTime)
    {
        level = (int) (Math.Floor((double) (score/100)) + 1);
        if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            Reset();
        if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
             Exit();
        if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
            if(currentBlock != null && currentBlock.CanTurn()) 
                currentBlock.Turn();
        if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
            if (currentBlock != null && currentBlock.CanMoveTo(-1, 0))
                currentBlock.MoveTo(-1, 0);
        if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
            if (currentBlock != null && currentBlock.CanMoveTo(1, 0))
                currentBlock.MoveTo(1, 0);
        if (inputHelper.KeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            if (currentBlock != null)
                downTimeCounter += 4*(float)gameTime.ElapsedGameTime.TotalSeconds;
        if (downWaitTime <= downTimeCounter)
        {
            downTimeCounter = level * 0.03f;
            if (currentBlock != null)
            {
                if (currentBlock.CanMoveTo(0, 1))
                {
                    currentBlock.MoveTo(0, 1);
                }
                else
                {
                    currentBlock.AddToSubBlocks();
                    score += 10; //Change this to 30 if you want to debug.
                    //Debug.WriteLine("The current falling block speed is: " + downTimeCounter);
                    currentBlock = null;
                }
            }
        }
        if (IsGameOver())
        {
            Reset();
        }
        if(blockWaitTime <= blockTimeCounter && currentBlock == null)
        {
            Random rnd = new Random();
            currentBlock = new Block(4, -3, rnd.Next(0, 7));
            blockTimeCounter = 0;
        }
        downTimeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
        blockTimeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
        currentGameTime += (float) gameTime.ElapsedGameTime.TotalSeconds;

        inputHelper.Update(gameTime);
        gameWorld.HandleInput(gameTime, inputHelper);
        gameWorld.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        spriteBatch.Begin();
        for (int x = 0; x < TetrisGrid.Width; x++)
        {
            for (int y = 0; y < TetrisGrid.Height; y++)
            {
                spriteBatch.Draw(emptyCell, new Vector2(x * emptyCell.Width, y * emptyCell.Height), Color.White);
            }
        }
        if(currentBlock != null)
        {
            foreach (SubBlock subBlock in currentBlock.subBlockArray)
            {
                spriteBatch.Draw(emptyCell, new Vector2(subBlock.X * emptyCell.Width, subBlock.Y * emptyCell.Height), subBlock.Color);
            }
        }
        if (allSubBlocks.ToArray().Length > 0)
        {
            foreach (SubBlock subBlock in allSubBlocks)
            {
                spriteBatch.Draw(emptyCell, new Vector2(subBlock.X * emptyCell.Width, subBlock.Y * emptyCell.Height), subBlock.Color);
            }
        }
        string currentlevel = "Level: " + level.ToString();
        string scorecount = "Score: " + score.ToString();
        string passedTime = "Time: " + Math.Floor(currentGameTime).ToString();
        spriteBatch.DrawString(font, currentlevel, new Vector2(500, 460), Color.Blue);
        spriteBatch.DrawString(font, scorecount, new Vector2(500, 480), Color.Blue);
        spriteBatch.DrawString(font, passedTime, new Vector2(500, 500), Color.Blue);
        spriteBatch.End(); 
        gameWorld.Draw(gameTime, spriteBatch);
    }
}

