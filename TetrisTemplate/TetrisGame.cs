using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections;
using System.Diagnostics;

class TetrisGame : Game
{
    SpriteBatch spriteBatch;
    InputHelper inputHelper;
    GameWorld gameWorld;
    Texture2D emptyCell;
    TetrisGrid tetrisGrid;
    int[][] allBlocks = new int[10][];
    int score, level;

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
        gameWorld = new GameWorld();
        gameWorld.Reset();
    }
    public static int[][] generateForm(int form, int x, int y)
    {
        switch (form)
        {
            case 0:
                return new int[][] { new int[] { 0 + x, 0 + y }, new int[]{ 0 + x, 1 + y }, new int[]{ 0 + x, 2 + y }, new int[]{ 0 + x, 3 + y } }; //vierblokkige staaf
            default:
                return new int[][] { new int[] { 0 + x, 0 + y }, new int[] { 1 + x, 0 + y }, new int[] { 0 + x, 1 + y }, new int[] { 1 + x, 1 + y } }; //vierkant blok
        }
            
    }
   /* public static BitArray GenerateBlock(short blockCode)
    {
        byte[] bytes = BitConverter.GetBytes(blockCode);
        System.Collections.BitArray arrBit = new BitArray(bytes);
        return arrBit;
    }

    public static bool GetBit(BitArray bitArr, int pos)
    {
        return bitArr.Get(pos);
    } */

    /*public static int[][] ConvertBAToDrawableArray(int startx, int starty, BitArray BA, Texture2D emptyCell)
    {
        int[][] jaggedArr = new int[12][];
        int i = 0;
        for(int x = startx; x < startx + 3*emptyCell.Width; startx += emptyCell.Width)
        {
            for(int y = starty; y < starty + 4*emptyCell.Height; starty += emptyCell.Height)
            {
                if(GetBit(BA, (i)))
                {
                    jaggedArr[i] = new int[2] { x, y };
                }
                i++;
            }
        }
        return jaggedArr;
    } */
    /*public static bool[,] ConvertBAToDrawableArray(int startx, int starty, BitArray BA, Texture2D emptyCell)
{

    bool[,] jaggedArr = new bool[startx/emptyCell.Width + 3, starty/emptyCell.Height + 4];
    int i = 0;
    for(int x = startx; x < startx + 3*emptyCell.Width; x += emptyCell.Width)
    {
            
        for(int y = starty; y < starty + 4*emptyCell.Height; y += emptyCell.Height)
        {
                
            if(GetBit(BA, (i)))
            {
                    jaggedArr[x / emptyCell.Width, y / emptyCell.Height] = true;
            } 
            i++;
        }
    }
    return jaggedArr;  
}  */
    
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
        //bool[,] arr = ConvertBAToDrawableArray(0, 0, GenerateBlock(3456), emptyCell);
        for (int x = 0; x < tetrisGrid.Width; x++)
        {
            for (int y = 0; y < tetrisGrid.Height; y++)
            {
                {
                    spriteBatch.Draw(emptyCell, new Vector2(x * emptyCell.Width, y * emptyCell.Height), Color.White);
                }
            }
        }
        int[][] form = generateForm(0, 0, 0);

        foreach(int[] coord in form)
        {
            spriteBatch.Draw(emptyCell, new Vector2(coord[0] * emptyCell.Width, coord[1] * emptyCell.Height), Color.Yellow);
        }


        //spriteBatch.Draw(emptyCell, new Vector2(0, 0), Color.Yellow);
        spriteBatch.End(); 
        gameWorld.Draw(gameTime, spriteBatch);
    }
}

