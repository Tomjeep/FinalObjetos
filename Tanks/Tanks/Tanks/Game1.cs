using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Tanks.Model;

namespace Tanks
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random randomizer;
        private int screenHeight;
        private int screenWidth;

        private Texture2D bgTexture;
        private Texture2D tankTexture;
        private Texture2D treeTexture;
        private Texture2D stoneTexture;
        private Texture2D cannonBallTexture;


        private SoundEffect DestruirSnd;
        private SoundEffect DispararSnd;
        private SoundEffect CaminarSnd;


        private float imagesRatio; //images size
        private int matrixLastCell;
        private TerrainCell [,] gameMatrix;
        private Player[] players;
        private int numberOfPlayers;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferMultiSampling = false;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Tanks";

            randomizer = new Random();


            //Initialize data structure
            matrixLastCell = 19;
            gameMatrix = new TerrainCell[matrixLastCell+1,matrixLastCell+1];
            imagesRatio = 30;

            numberOfPlayers = 2;
            InitializePlayers();
            GenerateTerrain();
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            bgTexture = Content.Load<Texture2D>("BACKGROUND");
            tankTexture = Content.Load<Texture2D>("Tank");
            treeTexture = Content.Load<Texture2D>("TREE");
            stoneTexture = Content.Load<Texture2D>("STONE");
            cannonBallTexture = Content.Load<Texture2D>("CANONBALL");


            DestruirSnd = Content.Load<SoundEffect>("DESTRUIR");
            DispararSnd = Content.Load<SoundEffect>("DISPARAR");
            CaminarSnd = Content.Load<SoundEffect>("CAMINAR");

            screenHeight = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            screenWidth = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            ReadKeyboard();

            BulletHits();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            DrawScenery();
            DrawTerrain();
            DrawPlayers();

            spriteBatch.End();

            base.Draw(gameTime);
        }


        #region players methods

        private void InitializePlayers()
        {
            if (numberOfPlayers < 2 || numberOfPlayers > 4)
                numberOfPlayers = 2;

            players = new Player[numberOfPlayers];
            players [0] = new Player()
                              {
                                  Id = 0,
                                  IsAlive = true,
                                  Row = 0,
                                  Column = 0,
                                  Direction = DataTypes.Direction.Down,                                  
                                  Color = new Color(
                                      randomizer.Next(0,255),
                                      randomizer.Next(0, 255),
                                      randomizer.Next(0, 255))
                              };
            players[1] = new Player()
                             {
                                 Id = 1,
                                 IsAlive = true,
                                 Row = matrixLastCell,
                                 Column = matrixLastCell,
                                 Direction = DataTypes.Direction.Up,
                                 Color = new Color(
                                     randomizer.Next(0, 255),
                                     randomizer.Next(0, 255),
                                     randomizer.Next(0, 255))
                             };
            if (numberOfPlayers > 2)
            {
                players[2] = new Player()
                                 {
                                     Id = 2,
                                     IsAlive = true,
                                     Row = matrixLastCell,
                                     Column = 0,
                                     Direction = DataTypes.Direction.Up,
                                     Color = new Color(
                                         randomizer.Next(0, 255),
                                         randomizer.Next(0, 255),
                                         randomizer.Next(0, 255))
                                 };
                if (numberOfPlayers > 3)
                {
                    players[3] = new Player()
                                     {
                                         Id = 3,
                                         IsAlive = true,
                                         Row = 0,
                                         Column = matrixLastCell,
                                         Direction = DataTypes.Direction.Down,
                                         Color = new Color(
                                             randomizer.Next(0, 255),
                                             randomizer.Next(0, 255),
                                             randomizer.Next(0, 255))
                                     };
                }
            }
        }


        private void DrawPlayers()
        {
            foreach (var player in players)
            {
                if (player.IsAlive)
                {
                    float xMovement = 0;
                    float yMovement = 0;

                    switch (player.Direction)
                    {
                        case DataTypes.Direction.Left:
                            xMovement = player.MovingTime;
                            break;
                        case DataTypes.Direction.Right:
                            xMovement = player.MovingTime * -1;
                            break;
                        case DataTypes.Direction.Up:
                            yMovement = player.MovingTime;
                            break;
                        case DataTypes.Direction.Down:
                            yMovement = player.MovingTime * -1;
                            break;
                    }

                    PrintCell(tankTexture, player.Row, player.Column, player.Color, (int) player.Direction, xMovement, yMovement);
                    //draws cannon ball
                    if (player.CannonBall != null)
                        spriteBatch.Draw(cannonBallTexture, player.CannonBall.Position, null, Color.White, 0,
                                         new Vector2(0, 0), imagesRatio/100, SpriteEffects.None, 1);
                }
            }


        }

        private void ReadKeyboard()
        {
            // Get current state of the keyboard
            KeyboardState state = Keyboard.GetState();

            player1Movement(state);
            player2Movement(state);
            if (numberOfPlayers > 2)
            {
                player3Movement(state);
                if (numberOfPlayers > 3)
                {
                    player4Movement(state);
                }
            }
            
            
        }        

        private void player1Movement(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.A))
            {
                EvaulateMovement(players[0], DataTypes.Direction.Left);
            }
            else if (state.IsKeyDown(Keys.W))
            {
                EvaulateMovement(players[0], DataTypes.Direction.Up);
            }
            else if (state.IsKeyDown(Keys.D))
            {
                EvaulateMovement(players[0], DataTypes.Direction.Right);
            }
            else if (state.IsKeyDown(Keys.S))
            {
                EvaulateMovement(players[0], DataTypes.Direction.Down);
            }
            else if (state.IsKeyDown(Keys.Space))
            {
                Fire(players[0]);
            }
        }


        private void player2Movement(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Left))
            {
                EvaulateMovement(players[1], DataTypes.Direction.Left);
            }
            else if (state.IsKeyDown(Keys.Up))
            {
                EvaulateMovement(players[1], DataTypes.Direction.Up);
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                EvaulateMovement(players[1], DataTypes.Direction.Right);
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                EvaulateMovement(players[1], DataTypes.Direction.Down);
            }
            else if (state.IsKeyDown(Keys.Enter))
            {
                Fire(players[1]);
            }
        }

        private void player3Movement(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.F))
            {
                EvaulateMovement(players[2], DataTypes.Direction.Left);
            }
            else if (state.IsKeyDown(Keys.T))
            {
                EvaulateMovement(players[2], DataTypes.Direction.Up);
            }
            else if (state.IsKeyDown(Keys.H))
            {
                EvaulateMovement(players[2], DataTypes.Direction.Right);
            }
            else if (state.IsKeyDown(Keys.G))
            {
                EvaulateMovement(players[2], DataTypes.Direction.Down);
            }
            else if (state.IsKeyDown(Keys.Y))
            {
                Fire(players[2]);
            }
        }

        private void player4Movement(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.J))
            {
                EvaulateMovement(players[3], DataTypes.Direction.Left);
            }
            else if (state.IsKeyDown(Keys.I))
            {
                EvaulateMovement(players[3], DataTypes.Direction.Up);
            }
            else if (state.IsKeyDown(Keys.L))
            {
                EvaulateMovement(players[3], DataTypes.Direction.Right);
            }
            else if (state.IsKeyDown(Keys.K))
            {
                EvaulateMovement(players[3], DataTypes.Direction.Down);
            }
            else if (state.IsKeyDown(Keys.O))
            {
                Fire(players[3]);
            }
        }

        private void EvaulateMovement(Player player, DataTypes.Direction direction)
        {
            if (player.MovingTime <= 0)
            {
                int col = player.Column;
                int row = player.Row;
                CaminarSnd.Play();
                switch (direction)
                {
                    case DataTypes.Direction.Left:
                        player.Direction = DataTypes.Direction.Left;
                        col--;
                        break;
                    case DataTypes.Direction.Right:
                        player.Direction = DataTypes.Direction.Right;
                        col++;
                        break;
                    case DataTypes.Direction.Up:
                        player.Direction = DataTypes.Direction.Up;
                        row--;
                        break;
                    case DataTypes.Direction.Down:
                        player.Direction = DataTypes.Direction.Down;
                        row++;
                        break;
                }

                if (col >= 0 && col <= matrixLastCell && row >= 0 && row <= matrixLastCell)
                {
                    if (IsCellAvailable(row, col))
                    {
                        player.Row = row;
                        player.Column = col;
                        player.MovingTime = 25;
                    }
                }
            }
        }

        private bool IsCellAvailable(int row, int col)
        {
            return gameMatrix[row, col] == null;
        }

        private void Fire(Player player)
        {
            if (player.IsAlive && player.ReloadTime <= 0 && player.CannonBall == null)
            {
                player.ReloadTime = 100;
                DispararSnd.Play();
                float adjustment = (imagesRatio - (cannonBallTexture.Height*(imagesRatio/100)))/2;
                player.Shoot(new Vector2(player.Column*imagesRatio + adjustment, player.Row*imagesRatio + adjustment));
            }
        }

        #endregion


        /// <summary>
        /// Draws game background
        /// </summary>
        private void DrawScenery()
        {
            var screenRectangle = new Rectangle(0, 0, screenHeight, screenHeight);

            spriteBatch.Draw(bgTexture, screenRectangle, Color.White);            
        }


        #region general methods


        private void PrintCell(Texture2D texture, int row, int column, Color color, int angle = 0, float xMovement = 0, float yMovement = 0)
        {            
            float rotation = MathHelper.ToRadians(angle);
            float imageCenter = texture.Height/2;            
            spriteBatch.Draw(texture, new Vector2(column * imagesRatio + imagesRatio /2 + xMovement, row * imagesRatio + imagesRatio /2 + yMovement), null, color, rotation, new Vector2(imageCenter, imageCenter), imagesRatio / 100, SpriteEffects.None, 1);
        }

        #endregion


        #region Terrain

        /// <summary>
        /// adds the game terrain to the matrix
        /// </summary>
        private void GenerateTerrain ()
        {
            
            generateCell(DataTypes.CellType.Trees, gameMatrix.Length * 0.6);
            generateCell(DataTypes.CellType.Stone, gameMatrix.Length * 0.1);
            
            
        }

        private void generateCell(DataTypes.CellType type, double obstaclesCellsTotal)
        {
            for (int i = 0; i < obstaclesCellsTotal; i++)
            {
                int row = randomizer.Next(0, matrixLastCell + 1);
                int column = randomizer.Next(0, matrixLastCell + 1);

                TerrainCell cell = gameMatrix[row, column];

                while (cell != null || IsPlayerStartArea(row, column))
                {
                    row = randomizer.Next(0, matrixLastCell + 1);
                    column = randomizer.Next(0, matrixLastCell + 1);
                    cell = gameMatrix[row, column];
                }
                gameMatrix[row, column] = new TerrainCell()
                {
                    Type = type
                };
            }
        }

        private bool IsPlayerStartArea(int row, int column)
        {
            foreach (var player in players)
            {
                for (int r = player.Row - 1; r <= player.Row + 1; r++)
                    for (int c = player.Column - 1; c <= player.Column + 1; c++)
                        if (row == r && column == c)
                            return true;
            }
            return false;
        }


        private void DrawTerrain()
        {
            for (int i = 0; i <= matrixLastCell; i++)
                for (int j = 0; j  <= matrixLastCell; j++)
                {
                    TerrainCell cell = gameMatrix[i, j];
                    if (cell != null && cell.Type.Equals(DataTypes.CellType.Trees))
                        PrintCell(treeTexture, i, j, Color.White, 0);
                    if (cell != null && cell.Type.Equals(DataTypes.CellType.Stone))
                        PrintCell(stoneTexture, i, j, Color.White, 0);
                }
        }
        #endregion


        #region bullets

        private void BulletHits()
        {
            foreach (var player in players)
            {
                if(player.MovingTime > 0)
                {
                    player.MovingTime--;
                }
                if (player.ReloadTime > 0)
                {
                    player.ReloadTime--;
                }
                if (player.CannonBall != null)
                {
                    float x = player.CannonBall.Position.X;
                    float y = player.CannonBall.Position.Y;
                    //Out of field bullet
                    float max = (matrixLastCell + 1)*imagesRatio; //total game area size
                    max -= cannonBallTexture.Height*(imagesRatio/100);
                    if (x < 0 || x > max || y < 0 || y > max)
                        player.CannonBall = null;
                    else
                    {
                        int row = Convert.ToInt32(Math.Round(y/imagesRatio));
                        int col = Convert.ToInt32(Math.Round(x/imagesRatio));

                        if (row <= matrixLastCell && col <= matrixLastCell && gameMatrix[row,col] != null)
                        {
                            if(gameMatrix[row,col].Type.Equals(DataTypes.CellType.Trees))
                            {
                                DestruirSnd.Play();
                                gameMatrix[row, col] = null;
                            }
                            player.CannonBall = null;
                        }

                        foreach (var player1 in players)
                        {
                            if(player.Id != player1.Id && player1.Row == row && player1.Column == col)
                            {
                                DestruirSnd.Play();
                                player1.IsAlive = false;
                                player.CannonBall = null;
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
