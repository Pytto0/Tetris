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
    int score, level;
    float blockWaitTime = 1f, blockTimeCounter, downWaitTime = 0.5f, downTimeCounter;

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
        int[][] currentBlock = new int[1][]; //lijst van alle grote blokken die momenteel stil staan.
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

    protected override void Update(GameTime gameTime)
    {
        downWaitTime = 0.5f;
        if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
             Exit();
        if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
            if(currentBlock != null) // && currentBlock.CanTurn()) //CanTurn() is not working...
                currentBlock.Turn();
        if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
            if (currentBlock != null)
                currentBlock.MoveLeft();
        if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
            if (currentBlock != null)
                currentBlock.MoveRight();
        if (inputHelper.KeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            if (currentBlock != null)
                downWaitTime = 0;
        if (downWaitTime <= downTimeCounter){
            downTimeCounter = 0;
            if (currentBlock != null)
            {
                if (currentBlock.CanFallDown())
                {
                    currentBlock.BlockFallDown();
                }
                else
                {
                    currentBlock.AddToFallenBlocks();
                    //Debug.WriteLine("test");
                    currentBlock = null;
                }
            }
        }
        if(blockWaitTime <= blockTimeCounter)
        {
            blockTimeCounter = 0;
            Random rnd = new Random();
            int form = rnd.Next(0, 6);
            if (currentBlock == null)
                currentBlock = new Block(5, -1, form);

        }
        downTimeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
        blockTimeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

        inputHelper.Update(gameTime);
        gameWorld.HandleInput(gameTime, inputHelper);
        gameWorld.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        spriteBatch.Begin();
        for (int x = 0; x < tetrisGrid.Width; x++)
        {
            for (int y = 0; y < tetrisGrid.Height; y++)
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
        string passedTime = gameTime.TotalGameTime.Seconds.ToString();
        spriteBatch.DrawString(font, passedTime, new Vector2(500, 500), Color.Blue);
        spriteBatch.End(); 
        gameWorld.Draw(gameTime, spriteBatch);
    }
}

