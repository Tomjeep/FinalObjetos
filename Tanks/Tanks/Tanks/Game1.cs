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
        private Texture2D cannonBallTexture;


        private float imagesRatio; //images size
        private int matrixLastCell;
        private TerrainCell [,] gameMatrix;
        private Player[] players;

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
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Tanks";

            randomizer = new Random();


            //Initialize data structure
            matrixLastCell = 19;
            gameMatrix = new TerrainCell[matrixLastCell+1,matrixLastCell+1];
            imagesRatio = 30;
            

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
            cannonBallTexture = Content.Load<Texture2D>("CANONBALL");

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
            players = new Player[2];
            players [0] = new Player()
                              {
                                  Id = 0,
                                  IsAlive = true,
                                  Row = 0,
                                  RowDisplacement = 0,
                                  Column = 0,
                                  ColumnDisplacement = 0,
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
                                 RowDisplacement = matrixLastCell,
                                 Column = matrixLastCell,
                                 ColumnDisplacement = matrixLastCell,
                                 Direction = DataTypes.Direction.Up,
                                 Color = new Color(
                                     randomizer.Next(0, 255),
                                     randomizer.Next(0, 255),
                                     randomizer.Next(0, 255))
                             };
        }


        private void DrawPlayers()
        {
            foreach (var player in players)
            {
                if (player.IsAlive)
                {
                    PrintCell(tankTexture, player.Row, player.Column, player.Color, (int) player.Direction);
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

        private void EvaulateMovement(Player player, DataTypes.Direction direction)
        {
            float columnDisplacement = player.ColumnDisplacement;
            float rowDisplacement = player.RowDisplacement;
            switch (direction)
            {
                case DataTypes.Direction.Left:
                    player.Direction = DataTypes.Direction.Left;
                    columnDisplacement -= 0.15f;
                    break;
                case DataTypes.Direction.Right:
                    player.Direction = DataTypes.Direction.Right;
                    columnDisplacement += 0.15f;
                    break;
                case DataTypes.Direction.Up:
                    player.Direction = DataTypes.Direction.Up;
                    rowDisplacement -= 0.15f;
                    break;
                case DataTypes.Direction.Down:
                    player.Direction = DataTypes.Direction.Down;
                    rowDisplacement += 0.15f;
                    break;
            }

            if (columnDisplacement >= 0 && columnDisplacement <= matrixLastCell && rowDisplacement >= 0 && rowDisplacement <= matrixLastCell)
            {
                int newRow = Convert.ToInt32(Math.Round(rowDisplacement));
                int newColumn = Convert.ToInt32(Math.Round(columnDisplacement));


                if (IsCellAvailable(newRow, newColumn))
                {
                    player.RowDisplacement = rowDisplacement;
                    player.Row = newRow;

                    player.ColumnDisplacement = columnDisplacement;
                    player.Column = newColumn;
                }
            }
        }

        private bool IsCellAvailable(int row, int col)
        {
            return gameMatrix[row, col] == null;
        }

        private void Fire(Player player)
        {
            if (player.IsAlive && player.CannonBall == null)
            {
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
            var screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            spriteBatch.Draw(bgTexture, screenRectangle, Color.White);            
        }


        #region general methods


        private void PrintCell(Texture2D texture, int row, int column, Color color, int angle = 0)
        {
            float rotation = MathHelper.ToRadians(angle);
            float imageCenter = texture.Height/2;            
            spriteBatch.Draw(texture, new Vector2(column * imagesRatio + imagesRatio /2 , row * imagesRatio + imagesRatio /2), null, color, rotation, new Vector2(imageCenter, imageCenter), imagesRatio / 100, SpriteEffects.None, 1);
        }

        #endregion


        #region Terrain

        /// <summary>
        /// adds the game terrain to the matrix
        /// </summary>
        private void GenerateTerrain ()
        {
            double obstaclesCellsTotal = gameMatrix.Length*0.6;
            
            for (int i = 0; i < obstaclesCellsTotal; i++)
            {
                int row = randomizer.Next(0, matrixLastCell+1);
                int column = randomizer.Next(0, matrixLastCell+1);

                TerrainCell cell = gameMatrix[row,column];
                
                while (cell != null || IsPlayerStartArea(row,column))
                {
                    row = randomizer.Next(0, matrixLastCell+1);
                    column = randomizer.Next(0, matrixLastCell+1);
                    cell = gameMatrix[row,column];
                }
                gameMatrix[row, column] = new TerrainCell()
                                              {
                                                  Type = DataTypes.CellType.Trees
                                              };
            }
        }

        private bool IsPlayerStartArea(int row, int column)
        {
            foreach (var player in players)
            {
                if (row == player.Row && column == player.Column)
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
                        PrintCell(treeTexture, i, j, Color.Turquoise, 0);
                }
        }
        #endregion


        #region bullets

        private void BulletHits()
        {
            foreach (var player in players)
            {
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
                            gameMatrix[row, col] = null;
                            player.CannonBall = null;
                        }

                        foreach (var player1 in players)
                        {
                            if(player.Id != player1.Id && player1.Row == row && player1.Column == col)
                            {
                                player1.IsAlive = false;
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
