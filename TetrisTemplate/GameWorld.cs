using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Collections.Generic;

/// <summary>
/// A class for representing the game world.
/// This contains the grid, the falling block, and everything else that the player can see/do.
/// </summary>
class GameWorld
{
    Texture2D emptyCell;
    SpriteFont font;
    public int score { get; private set; }
    public int level { get; private set; }
    
    public float currentGameTime { get; private set; } = 0f;
    private const int amountOfBlockForms = 7;
    private const float constBlockWaitTime = 1f, constDownWaitTime = 0.5f;
    private float blockWaitTime = constBlockWaitTime, blockTimeCounter, downWaitTime = constDownWaitTime, downTimeCounter;
    //public  SubBlock[,] gameWorld.grid.gridArr { get; set; } 
    public Block currentBlock, nextBlock;
    InputHelper inputHelper;
    /// <summary>
    /// An enum for the different game states that the game can have.
    /// </summary>


    /// <summary>
    /// The random-number generator of the game.
    /// </summary>
    public static Random Random { get { return new Random(); } }
    /// <summary>
    /// The main font of the game.
    /// </summary>

    /// <summary>
    /// The current game state.
    /// </summary>


    /// <summary>
    /// The main grid of the game.
    /// </summary>
    public TetrisGrid grid { get; }
    public Vector2 position { get; }
    public Keys[] keyList;
    private bool lostGame;

    public GameWorld(Texture2D EmptyCell, SpriteFont Font, Vector2 Position, int GridWidth, int GridHeight, Keys[] KeyList)
    {
        emptyCell = EmptyCell;
        font = Font;
        Random random = new Random();
        position = Position;
        keyList = KeyList;
        grid = new TetrisGrid(emptyCell, position, this, GridWidth, GridHeight);
        //allSubBlocks = grid.gridArr; //Lijst van alle blokjes die momenteel stil staan en niet meer kunnen bewegen
        nextBlock = new Block(15, 3, Random.Next(0, amountOfBlockForms), grid);
        //grid = new TetrisGrid();
        inputHelper = new InputHelper();
    }
    public bool CheckIfGameOver()
    {
        for(int x = 0; x < grid.width; x++)
        {
            if(grid.gridArr[x, (int) Math.Abs(grid.position.Y)] != null) //de absolute waarde van de positie van de grid geeft aan of we het nulpunt bereikt hebben. We kunnen namelijk niet een array maken met index [0, -1] en daarom moeten we alles een paar blokken hoger tekenen.
            {
                return true;
            }
        }
        return false;
    }

    public void Reset(GameTime gameTime)
    {
        score = 0;
        level = 1;
        currentGameTime = 0f;
        grid.Clear();
        blockWaitTime = constBlockWaitTime;
        downWaitTime = constDownWaitTime;
        currentBlock = null;
        lostGame = false;
        Update(gameTime);
        // gameWorld.gameState = GameWorld.GameState.NotStarted; 

    }

    /*private void ExecuteKeyOrders()
    {
       
    } */

    private void MakeRowsFall(int minimumY)
    {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = minimumY; y > 0; y--)
            {
                if (grid.gridArr[x, y] != null)
                {
                        if (grid.gridArr[x, y+1] == null)
                        {
                            grid.gridArr[x, y + 1] = grid.gridArr[x, y];
                            grid.gridArr[x, y] = null;
                        }
                    
                }
            }
        }
    }
        /*List<int> yCoordinates = SubBlockRow.GetAllRowsYCoordinates(grid, false);
        foreach (int y in yCoordinates)
        {
            //Debug.WriteLine("test56");
            if (y < minimumY)
            {
                SubBlockRow subBlockRow = new SubBlockRow(y, grid);

                Debug.WriteLine("test56");
                subBlockRow.Fall();
                Debug.WriteLine("test57");
            }
        } */
    
    private void RemoveFullRows()
    {
        List<int> yCoordinates = SubBlockRow.GetAllRowsYCoordinates(grid, true);

        foreach (int y in yCoordinates)
        {
            SubBlockRow subBlockRow = new SubBlockRow(y, grid);
            if (subBlockRow.IsRowFull())
            {
                Debug.WriteLine("test1");
                subBlockRow.ClearRow();
                MakeRowsFall(y);
                
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
            Debug.WriteLine("ttest234");
            currentBlock.AddToSubBlocks();
            score += 10;
            currentBlock = null;
            RemoveFullRows();
        }
        downTimeCounter = 0;
    }

    private void CreateNewBlock()
    {
        currentBlock = new Block((int) (grid.width/2), 0, nextBlock.Form, grid);
        nextBlock = new Block((grid.correctWidth) + 5, (int) (grid.position.Y + 5), Random.Next(0, amountOfBlockForms), grid);
        blockTimeCounter = 0;
    }

    private int GetCorrectLevel()
    {
        int levelHolder = (int)(Math.Floor((double)(score / 100)) + 1);
        //Debug.WriteLine(levelHolder);
        if (levelHolder > TetrisGame.maxLevel)
            return TetrisGame.maxLevel;
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
        //Debug.WriteLine("test1");

        List<Keys> keys = inputHelper.GetPressedKeys();
        if (keys != null)
        {
            TetrisGame.GameState gs = TetrisGame.gameState;
            ////Debug.WriteLine(keys.Count);
            foreach (Keys key in keys)
            {
                if(key == keyList[0])
                {
                    if (currentBlock != null && currentBlock.CanTurn())
                        currentBlock.Turn();
                }
                else if(key == keyList[1])
                {
                    if (currentBlock != null && currentBlock.CanMoveRelativeTo(1, 0)) //move right
                        currentBlock.MoveRelativeTo(1, 0);
                }
                /*else if (key == keyList[2]) //move down; dat doen we in update method
                {
                    downTimeCounter += 4 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                } */
                else if(key == keyList[3])
                {
                    if (currentBlock != null && currentBlock.CanMoveRelativeTo(-1, 0)) //move left
                        currentBlock.MoveRelativeTo(-1, 0);
                }

                /*switch (key)
                {
                    case keyList[0]: //
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

                } */
            }
        }

    }

    public void Update(GameTime gameTime)
    {
        //grid.gridArr = grid.gridArr;
        //Debug.WriteLine("Update TetrisGame");
        if (TetrisGame.gameState == TetrisGame.GameState.Playing)
        {
            level = GetCorrectLevel();
            //Debug.WriteLine("a: " + GetCorrectLevel());
            //Debug.WriteLine("b: " + level);
            if (currentBlock != null)
            {
                ////Debug.WriteLine(downTimeCounter);
                if (downWaitTime <= downTimeCounter)
                    BlockFallDown();
                if (inputHelper.KeyDown(keyList[2]))
                    downTimeCounter += 4 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (blockWaitTime <= blockTimeCounter)
                CreateNewBlock();
            if (CheckIfGameOver())
            {
                TetrisGame.gameState = TetrisGame.GameState.GameOver; // nu weten alle classes dat er iemand verloren heeft. Het spel stopt dus.
                lostGame = true; //Maar alleen deze class hoeft te weten dat die verloren heeft, omdat de andere speler dan automatisch gewonnen heeft.
            }
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            downTimeCounter += (float)(0.25 * (level - 1) * elapsedTime) + elapsedTime; //nu heb je bij level 5 twee keer zo snel, bij level 9 drie keer zo snel en bij level 13 vier keer zo snel.
            blockTimeCounter += elapsedTime;
            currentGameTime += elapsedTime;

        }


        //gameWorld.HandleInput(gameTime, inputHelper);
        HandleInput(gameTime);
        //gameWorld.Update(gameTime);
        //Update(gameTime); 
    }

    public void DrawCurrentAndNextBlock(SpriteBatch spriteBatch)
    {
        foreach (SubBlock subBlock in currentBlock.subBlockArray)
        {
            int correctedX = (int)(subBlock.x + grid.position.X);
            int correctedY = (int)(subBlock.y + grid.position.Y);
            spriteBatch.Draw(emptyCell, new Vector2(correctedX * emptyCell.Width, correctedY * emptyCell.Height), subBlock.color);
        }
        foreach (SubBlock subBlock in nextBlock.subBlockArray)
        {
            //int correctedX = (int)(subBlock.x + grid.position.X);
           //int correctedY = (int)(subBlock.y + grid.position.Y);
            spriteBatch.Draw(emptyCell, new Vector2(subBlock.x * emptyCell.Width, subBlock.y * emptyCell.Height), subBlock.color);
        }
    }

    private void DrawEndGameText(SpriteBatch spriteBatch)
    {
        String text1, text2;
        int startDrawX = (int)((grid.width + position.X + 3) * emptyCell.Width);
        if (!TetrisGame.multiplayer)
        {
            text1 = "Je hebt verloren. Jouw score is: " + score;
            text2 = "Druk op de spatiebalk om opnieuw te beginnen.";
            
        }
        else
        {
            if (lostGame)
            {
                text1 = "Je hebt verloren. Jouw score is: " + score;
                text2 = "Druk op de spatiebalk om opnieuw te beginnen.";
            }
            else
            {
                text1 = "Je hebt gewonnen. Jouw score is: " + score;
                text2 = "Druk op de spatiebalk om opnieuw te beginnen.";
            }
        }
        spriteBatch.DrawString(font, text1, new Vector2(startDrawX, 260), Color.Black);
        spriteBatch.DrawString(font, text2, new Vector2(startDrawX, 300), Color.Black);
    }

    public void DrawScore(SpriteBatch spriteBatch)
    {
        int startDrawX = (int)((grid.width + position.X + 3) * emptyCell.Width);
        string currentlevel = "Level: " + level.ToString();
        string scorecount = "Score: " + score.ToString();
        string passedTime = "Time: " + Math.Floor(currentGameTime).ToString();
        spriteBatch.DrawString(font, currentlevel, new Vector2(startDrawX, 460), Color.Blue);
        spriteBatch.DrawString(font, scorecount, new Vector2(startDrawX, 480), Color.Blue);
        spriteBatch.DrawString(font, passedTime, new Vector2(startDrawX, 500), Color.Blue);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (TetrisGame.gameState == TetrisGame.GameState.GameOver)
            DrawEndGameText(spriteBatch);

        grid.Draw(gameTime, spriteBatch);
        DrawScore(spriteBatch);
        if (currentBlock != null)
        {
            DrawCurrentAndNextBlock(spriteBatch);
        }
       /* if (gameWorld.grid.gridArr.Length > 0)
        {
            for(int x = 0; x < TetrisGrid.Width; x++)
            {
                for(int y = 0; y < TetrisGrid.Height; y++)
                {
                    spriteBatch.Draw(emptyCell, new Vector2(subBlock.x * emptyCell.Width, subBlock.y * emptyCell.Height), subBlock.color);
                }
                
            }
                
        } */

    }

    //public void Reset()
    //{
    //}
}
