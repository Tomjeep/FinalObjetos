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
using Rockets_Sesion_II.Model;

namespace Rockets_Sesion_II
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Global Variables & Properties
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Ulacit: Backgorund and Foreground Textures
        Texture2D bgTexture;
        Texture2D fgTexture; // Ulacit: Will represent level complexity!
        Texture2D cannonTexture; // Ulacit: base Texture
        Texture2D baseTexture; // Ulacit: cannon base texture
        Texture2D rocketTexture; // Ulacit : rocket bullet image
        Texture2D smokeTexture; // Ulacit: this will represent the rocket smoke


        // Ulacit: Game size constraints 
        int gameHeight;
        int gameWidth;

        // Ulait: Players information
        Player[] players;
        int numberOfPlayers = 2; // Initial number of players
        float playerScale; // spriteBatch image scaling
        int currentPlayer = 0;

        // Ulacit: Game Font Info
        SpriteFont fontTitle;
        SpriteFont fontData;

        // Ulacit: Rocket Information
        bool rocketIsFlying = false;
        Vector2 rocketPosition;
        Vector2 rocketDirection;
        float rocketAngle;

        // Ulacit: Smoke elements
        List<Vector2> smokeParticles = new List<Vector2>(); // Ulacit: store all existing smoke particles
        Random smokeRandomizer = new Random(); // generate a smoke particle at a random position

        // Ulacit: Cannon Sound Effect
        SoundEffect cannonSound;


        #endregion

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
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 500;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Ulacit 2D - Game";

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

            // Ulacit: init graphical scenary content
            bgTexture = Content.Load<Texture2D>("BACKGROUND");
            fgTexture = Content.Load<Texture2D>("FOREGROUND");
            cannonTexture = Content.Load<Texture2D>("CANNON");
            baseTexture = Content.Load<Texture2D>("BASE");
            fontTitle = Content.Load<SpriteFont>("ARIAL14");
            fontData = Content.Load<SpriteFont>("ARIAL12");
            rocketTexture = Content.Load<Texture2D>("ROCKET");
            smokeTexture = Content.Load<Texture2D>("SMOKE");

            // Ulacit: init Sound Elements
            cannonSound = Content.Load<SoundEffect>("CANNON-SOUND");

            // Ulacit: setup screen size contraints
            gameWidth = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            gameHeight = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;

            // Ulacit: scale player to 40px
            playerScale = 40.0f / (float)baseTexture.Width;

            // Ulacit: Initialize players Game State
            InitializePlayers();
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

            #region Ulacit - Game Logic
            // Ulacit: Our Custom Method to rotate the canon of the player in focus
            if (!rocketIsFlying)
            {
                ReadKeyboard();
            }
            // Ulacit: Update rcoket position.
            UpdateRocketAngle();


            // Ulacit: Add Smoke
            for (int i = 0; i < 2; i++)
            {
                Vector2 smokePos = rocketPosition; // Ulacit: Current Rocket Position
                smokePos.X += smokeRandomizer.Next(10) - 5;
                smokePos.Y += smokeRandomizer.Next(10) - 5;
                smokeParticles.Add(smokePos);
            }

            #endregion

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // Ulacit: lets draw the background of our game
            spriteBatch.Begin();

            // Ulacit: All Our Game Methods
            DrawGameScenery();
            DrawPlayers();
            DrawText();
            DrawRocket();
            DrawSmoke();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Ulacit Helper Methods
        /// <summary>
        /// This is method will help on setting the backgound on the whole screen
        /// </summary>
        void DrawGameScenery()
        {
            // ulacit: create a rectangle at position xy (0,0) and size gameWidth, gameHeight  
            Rectangle screenRectangle = new Rectangle(0, 0, gameWidth, gameHeight);

            // ulacit: this will draw the image within the rectangle constraints
            spriteBatch.Draw(bgTexture, screenRectangle, Color.White);
            spriteBatch.Draw(fgTexture, screenRectangle, Color.White);
        }

        /// <summary>
        /// Setup all players data
        /// </summary>
        void InitializePlayers()
        {
            // Ulacit: Random Player Colors
            Color[] playerColors = new Color[numberOfPlayers];

            // Ulacit: Generate Random Colors
            Random r = new Random();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                playerColors[i] = new Color(
                (byte)r.Next(0, 255),
                (byte)r.Next(0, 255),
                (byte)r.Next(0, 255));
            }

            // Ulacit: Initialize player array with the amound defined.
            players = new Player[numberOfPlayers];

            // Set all players information
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i] = new Player();
                players[i].IsAlive = true;
                players[i].Color = playerColors[i];
                // Ulacit: XNA manage angles in radians... 
                // so we pass the angle and Math Helper transform it to Radians.
                // cannon image is horizontal, so we hae to rotate it 90 degrees (just for start)
                players[i].Angle = MathHelper.ToRadians(90); // 0 to 180
                players[i].Power = 100; // defualt power
            }

            // Setup positions on initial Level
            LevelPlayerPositions(1);
        }

        /// <summary>
        /// Ulacit: this method needs to get smarter!
        /// An algorithm must be written to define positions agains scenery.
        /// </summary>
        /// <param name="level"></param>
        void LevelPlayerPositions(int level)
        {
            switch (level)
            {
                case 1:
                    players[0].Position = new Vector2(68, 420);
                    players[1].Position = new Vector2(745, 467);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Ulacit: Draw all players in the game canvas.. if they still alive
        /// </summary>
        void DrawPlayers()
        {
            foreach (var player in players)
            {
                if (player.IsAlive)
                {
                    // spriteBatch.Draw(baseTexture, player.Position, Color.White); /*DEFUALT DRAWING APPROACH*/
                    // spriteBatch.Draw(baseTexture, player.Position, null, Color.White, 0, new Vector2(0, baseTexture.Height) /*ORIGIN OF THE IMAGE*/, 1, SpriteEffects.None, 0);
                    spriteBatch.Draw(baseTexture, player.Position, null, player.Color, 0, new Vector2(0, baseTexture.Height) /*ORIGIN OF THE IMAGE*/, playerScale, SpriteEffects.None, 0);


                    // ulacit: lets draw the cannon!
                    int cX = (int)player.Position.X + 20;
                    int cY = (int)player.Position.Y - 10;
                    var cannonOrigin = new Vector2(11, 50);
                    spriteBatch.Draw(cannonTexture, new Vector2(cX, cY), null, player.Color, player.Angle, cannonOrigin /*ORIGIN OF THE CANON [Centered]*/, playerScale, SpriteEffects.None, 1);

                }
            }
        }

        /// <summary>
        /// Ulacit: Process commands from keyboard
        /// This method contains game logic, so it should be on the Update Method()
        /// </summary>
        void ReadKeyboard()
        {
            // Get current state of the keyboard
            KeyboardState state = Keyboard.GetState();

            // Modify current player cannon angle to the left
            if (state.IsKeyDown(Keys.Left))
            {
                players[currentPlayer].Angle -= 0.01f;
            }


            // Modify current player cannon angle to the right
            if (state.IsKeyDown(Keys.Right))
            {
                players[currentPlayer].Angle += 0.01f;
            }

            // Ulacit: Avoid current canon to hit the ground. 
            if (players[currentPlayer].Angle > MathHelper.PiOver2)
                players[currentPlayer].Angle = -MathHelper.PiOver2;
            if (players[currentPlayer].Angle < -MathHelper.PiOver2)
                players[currentPlayer].Angle = MathHelper.PiOver2;

            // Ulacit : User Key Down or Up to Add Shooting Power (x1)
            if (state.IsKeyDown(Keys.Down))
                players[currentPlayer].Power -= 1;
            if (state.IsKeyDown(Keys.Up))
                players[currentPlayer].Power += 1;

            // Ulacit : User Key PageDown or PageUp to Add Shooting Power (x20)
            if (state.IsKeyDown(Keys.PageDown))
                players[currentPlayer].Power -= 20;
            if (state.IsKeyDown(Keys.PageUp))
                players[currentPlayer].Power += 20;

            // Ulacit: Define power boundary (min: 0, max:1000) 
            if (players[currentPlayer].Power > 1000)
                players[currentPlayer].Power = 1000;
            if (players[currentPlayer].Power < 0)
                players[currentPlayer].Power = 0;

            // Ulacit: Rocket Area : Shoot on Enter or SpaceBar
            if (state.IsKeyDown(Keys.Enter) || state.IsKeyDown(Keys.Space))
            {
                rocketIsFlying = true;

                // Ulacit: Play Sound!
                cannonSound.Play();

                // Ulacit: Initial Position of the Rocket
                rocketPosition = players[currentPlayer].Position;
                rocketPosition.X += 20;
                rocketPosition.Y -= 10;
                rocketAngle = players[currentPlayer].Angle;

                // Ulacit: Define Rocket Direction
                Vector2 up = new Vector2(0, -1); // Initial Direction Vector
                Matrix rotMatrix = Matrix.CreateRotationZ(rocketAngle); // Transformation Matrix with z Axis
                rocketDirection = Vector2.Transform(up, rotMatrix); // Transformed value
                rocketDirection *= players[currentPlayer].Power / 50.0f; // Tranbsformed value affected by the selected power
            }

        }

        /// <summary>
        /// This method will help showing in-game info (power of canon and angle)
        /// </summary>
        void DrawText()
        {
            var data = players[currentPlayer];
            string angle = ((int)MathHelper.ToDegrees(data.Angle)).ToString();
            spriteBatch.DrawString(fontTitle, string.Format("Player: {0}", currentPlayer + 1), new Vector2(3, 25), data.Color);
            spriteBatch.DrawString(fontData, string.Format("Power: {0}/1000", data.Power), new Vector2(3, 45), data.Color);
            spriteBatch.DrawString(fontData, string.Format("Angle: {0}", angle), new Vector2(3, 60), data.Color);
        }


        /// <summary>
        /// Render rocket on the screen
        /// </summary>
        void DrawRocket()
        {
            if (rocketIsFlying)
            {
                spriteBatch.Draw(rocketTexture, rocketPosition, null, players[currentPlayer].Color, rocketAngle, new Vector2(42, 240), 0.1f, SpriteEffects.None, 1);
            }
        }

        /// <summary>
        /// Ulacit: Updates rocket angle while flying
        /// This method is called in upate method
        /// </summary>
        void UpdateRocketAngle()
        {
            if (rocketIsFlying)
            {
                // Ulacit: Remember rocketDirection is a result from our transfomed matrix! 
                // Stright Line Flight rocketPosition += rocketDirection;

                // Ulacit: Graity Emulation!
                Vector2 gravity = new Vector2(0, 1);
                rocketDirection += gravity / 10.0f;
                rocketPosition += rocketDirection;
                // Ulacit: lets change the angle when flying
                rocketAngle = (float)Math.Atan2(rocketDirection.X, -rocketDirection.Y);

                if (RocketOutOfBounds(rocketPosition))
                {
                    rocketIsFlying = false;
                    currentPlayer = (currentPlayer + 1) % numberOfPlayers; //Calculates the next player shooting
                }

            }
        }


        /// <summary>
        /// Checks if the rocket has gone out of the playing area
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool RocketOutOfBounds(Vector2 position)
        {
            if (position.X < 0 || position.X > gameWidth || position.Y > gameHeight)
                return true;
            return false;
        }


        /// <summary>
        /// Ulacit: This draw smoke from the soke list. Since everytime on the draw() method, clearDevice is swapping out the screen,
        /// then we have to help the game to remember when the smoke was placed
        /// </summary>
        void DrawSmoke()
        {
            foreach (var particle in smokeParticles)
                spriteBatch.Draw(smokeTexture, particle, null, Color.White, 0, new Vector2(40, 35), 0.2f, SpriteEffects.None, 1);
        }

        #endregion
    }
}
