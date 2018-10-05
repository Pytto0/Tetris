using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections;

class TetrisGame : Game
{
    SpriteBatch spriteBatch;
    InputHelper inputHelper;
    GameWorld gameWorld;
    Texture2D emptyCell;
    TetrisGrid tetrisGrid;
    int score, level;

    /// <summary>
    /// A static reference to the ContentManager object, used for loading assets.
    /// </summary>
    public static ContentManager ContentManager { get; private set; }
    test
    

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
        gameWorld = new GameWorld();
        gameWorld.Reset();
    }

    public static BitArray generateBlock(short blockCode)
    {
        byte[] bytes = BitConverter.GetBytes(blockCode);
        System.Collections.BitArray arrBit = new BitArray(bytes);
        return arrBit;
    }

    public static bool getBit(BitArray bitArr, int pos)
    {
        return bitArr.Get(pos);
    }

    public static int[,] ConvertBAToDrawableArray(int startx, int starty, BitArray BA, Texture2D emptyCell)
    {
        int[,] arr = new int[12, 12];
        int i = 0;
        int j = 0;
        for(int x = startx; x < startx + 3*emptyCell.Width; startx += emptyCell.Width)
        {
            i++;
            for(int y = starty; y < starty + 4*emptyCell.Height; starty += emptyCell.Height)
            {
                j++;
                if(getBit(BA, (i*3 + j)))
                {
                    //code
                }
            }
        }
        return null;
    }

    protected override void Update(GameTime gameTime)
    {
        if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
        {
            Exit();
        }
        inputHelper.Update(gameTime);
        gameWorld.HandleInput(gameTime, inputHelper);
        gameWorld.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        spriteBatch.Begin();
        for (int x = 0; x < tetrisGrid.Width * emptyCell.Width; x += emptyCell.Width)
        {
            for (int y = 0; y < tetrisGrid.Height * emptyCell.Height; y += emptyCell.Height)
            {
             spriteBatch.Draw(emptyCell, new Vector2(x, y), Color.White);
            }
        }
        spriteBatch.End(); 
        gameWorld.Draw(gameTime, spriteBatch);
    }
}

