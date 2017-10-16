using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;

namespace snake
{
    public class SnakeGame : Game
    {
        const float UpdateRateInMilliseconds = 100;

        public const int spriteSize = 16;
        public const int gridLength = 10;
        public const int viewPortSize = spriteSize * gridLength;
        public static Vector2 viewPort = new Vector2(viewPortSize, viewPortSize);
        public static Vector2 screenSize = new Vector2(viewPortSize * 3, viewPortSize * 3);

        SpriteBatch spriteBatch;
        readonly GraphicsDeviceManager graphics;
        ScalingViewportAdapter viewPortAdapter;

        int timeSinceLastUpdate;
        Level level;
        SpriteFont font;

        GameState gameState = GameState.Started;

        public SnakeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = (int)screenSize.X;
            graphics.PreferredBackBufferHeight = (int)screenSize.Y;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            level = new Level(Services, graphics);
            base.Initialize();
            level.Initialize();
            viewPortAdapter = new ScalingViewportAdapter(GraphicsDevice, (int)viewPort.X, (int)viewPort.Y);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("fuente");
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Started:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.Playing;
                    }
                    break;
                case GameState.Playing:
                    // For Mobile devices, this logic will close the Game when the Back button is pressed
                    // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                        || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        Exit();
                    }
#endif
                    base.Update(gameTime);
                    timeSinceLastUpdate += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (timeSinceLastUpdate >= UpdateRateInMilliseconds)
                    {
                        timeSinceLastUpdate = 0;
                        level.Update();
                    }
                    if (level.snake.IsDead)
                    {
                        gameState = GameState.Died;
                    }
                    break;
                case GameState.Died:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        level.Initialize();
                        gameState = GameState.Playing;
                    }
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(transformMatrix: viewPortAdapter.GetScaleMatrix(),
                              samplerState: SamplerState.PointClamp);
            level.Draw(spriteBatch);
            if (gameState == GameState.Started)
            {
                spriteBatch.DrawString(font, "Pulsa <ENTER> para comenzar",
                                       new Vector2(8, 40), Color.White);
            }
            if (gameState == GameState.Died)
            {
                spriteBatch.DrawString(font, "Game Over",
                                       new Vector2(52, 40), Color.White);
                spriteBatch.DrawString(font, "Pulsa Enter para volver a jugar",
                                       new Vector2(8, 50), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
