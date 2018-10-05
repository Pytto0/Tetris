using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections;
using System.Diagnostics;
using System.Linq;

class TetrisGame : Game
{
    SpriteBatch spriteBatch;
    InputHelper inputHelper;
    GameWorld gameWorld;
    Texture2D emptyCell;
    TetrisGrid tetrisGrid;
    SpriteFont font;
    int[][] allBlocks = new int[0][];
    int score, level;
    float blockWaitTime = 3f, blockTimeCounter, downWaitTime = 1f, downTimeCounter;

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
        ScreenSize = new Point(800, 600);
        graphics.PreferredBackBufferWidth = ScreenSize.X;
        graphics.PreferredBackBufferHeight = ScreenSize.Y;

        // create the input helper object
        inputHelper = new InputHelper();

        tetrisGrid = new TetrisGrid();
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

    public void FallDown()
    {
        foreach (int[] block in allBlocks) // Loop voor alle (grote) arrays in allBlocks.
        {
            int[] blockBelow = (int[]) block.Clone(); //block is de oorspronkelijke positie en blockBelow is de positie eronder.
            int y = block[1]; // Declaratie van y, waarin het gelijk wordt gesteld aan de y-coördinaat.
            Debug.WriteLine("y1 " + y);
            blockBelow[1] = y + 1; // Verandering van blockBelow[1] (de y-coördinaat).
            Debug.WriteLine("y2 " + y);
            if (!(allBlocks.Contains(blockBelow)) && y <= 20) // Als allBlocks geen "blockBelow" heeft, of y+1 is niet 20...
            {
                block[1] = y + 1;
                Debug.WriteLine("TEST " + block[1]);
            } // ...dan gaan de originele y-coördinaat (block) omlaag, en zo niet dan gebeurt er niks.
        }
    }

    public static int[][] AddJaggedArrayToJaggedArray(int[][] arr1, int[][] arr2)
    {
        int[][] temporaryArr = new int[arr1.Length + arr2.Length][];
        for (int i = 0; i < arr1.Length; i++)
        {
            if (arr1[i] != null)
                temporaryArr[i] = arr1[i];
        }
        for (int j = 0; j < arr2.Length; j++)
        {
            if (arr2[j] != null)
                temporaryArr[j + arr1.Length] = arr2[j];
        }
        temporaryArr = temporaryArr.Where(c => c != null).ToArray();
        return temporaryArr;
    }
    
    protected override void Update(GameTime gameTime)
    {
        if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
        { Exit(); }
        if(downWaitTime <= downTimeCounter){
            downTimeCounter = 0;
            FallDown();
        }
        if(blockWaitTime <= blockTimeCounter)
        {
            blockTimeCounter = 0;
            Random rnd = new Random();
            int generate = rnd.Next(0, 6);
            int[][] form = TetrisGrid.generateForm(generate, 5, 0);
            allBlocks = AddJaggedArrayToJaggedArray(form, allBlocks);
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
        //bool[,] arr = ConvertBAToDrawableArray(0, 0, GenerateBlock(3456), emptyCell);
        for (int x = 0; x < tetrisGrid.Width; x++)
        {
            for (int y = 0; y < tetrisGrid.Height; y++)
            {
                spriteBatch.Draw(emptyCell, new Vector2(x * emptyCell.Width, y * emptyCell.Height), Color.White);
            }
        }

        foreach (int[] coord in allBlocks)
        {
            int colorCode = coord[2];
            Color col;
            switch(colorCode){
                case 0:
                    col = Color.Yellow;
                    break;
                case 1:
                    col = Color.Red;
                    break;
                case 2:
                    col = Color.Blue;
                    break;
                case 3:
                    col = Color.Brown;
                    break;
                case 4:
                    col = Color.Green;
                    break;
                case 5:
                    col = Color.Purple;
                    break;
                default:
                    col = Color.Aqua;
                    break;
            }
            spriteBatch.Draw(emptyCell, new Vector2(coord[0] * emptyCell.Width, coord[1] * emptyCell.Height), col);
        }
        string passedTime = gameTime.TotalGameTime.Seconds.ToString();
        spriteBatch.DrawString(font, passedTime, new Vector2(500, 500), Color.Blue);

        //spriteBatch.Draw(emptyCell, new Vector2(0, 0), Color.Yellow);
        spriteBatch.End(); 
        gameWorld.Draw(gameTime, spriteBatch);
    }
}

