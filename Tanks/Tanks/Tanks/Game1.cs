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


        private int screenHeight;
        private int screenWidth;

        private Texture2D bgTexture;
        private Texture2D tankTexture;


        private float imagesRatio; //images size
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


            //Initialize data structure

            gameMatrix = new TerrainCell[20,20];
            imagesRatio = 30;

            InitializePlayers();

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
            DrawPlayers();

            spriteBatch.End();

            base.Draw(gameTime);
        }


        #region players methods

        private void InitializePlayers()
        {
            Random ran = new Random();
            players = new Player[2];
            players [0] = new Player()
                              {
                                  Row = 0,
                                  Column = 0,
                                  Direction = DataTypes.Direction.Down,
                                  Color = new Color(
                                      ran.Next(0,255),
                                      ran.Next(0, 255),
                                      ran.Next(0, 255))
                              };
            players[1] = new Player()
                             {
                                 Row = 19,
                                 Column = 19,
                                 Direction = DataTypes.Direction.Up,
                                 Color = new Color(
                                     ran.Next(0, 255),
                                     ran.Next(0, 255),
                                     ran.Next(0, 255))
                             };
        }


        private void DrawPlayers()
        {
            foreach (var player in players)
            {
                PrintCell(tankTexture, player.Row, player.Column, player.Color, (int)player.Direction);
            }

            
        }

        private void ReadKeyboard()
        {
            // Get current state of the keyboard
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Left))
            {
                players[0].Direction = DataTypes.Direction.Left;                
            }
            if (state.IsKeyDown(Keys.Up))
            {
                players[0].Direction = DataTypes.Direction.Up;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                players[0].Direction = DataTypes.Direction.Right;                
            }
            if (state.IsKeyDown(Keys.Down))
            {
                players[0].Direction = DataTypes.Direction.Down;
            }



            if (state.IsKeyDown(Keys.A))
            {
                players[1].Direction = DataTypes.Direction.Left;
            }
            if (state.IsKeyDown(Keys.W))
            {
                players[1].Direction = DataTypes.Direction.Up;
            }
            if (state.IsKeyDown(Keys.D))
            {
                players[1].Direction = DataTypes.Direction.Right;                
            }
            if (state.IsKeyDown(Keys.S))
            {
                players[1].Direction = DataTypes.Direction.Down;
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
    }
}
