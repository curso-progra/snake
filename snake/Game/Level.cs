using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace snake
{
    class Level
    {
        const int PlayerStartingDirection = 2;
        const int PlayerStartingLength = 2;
		
        public Snake snake;

        IServiceProvider serviceProvider;
        GraphicsDeviceManager graphics;
        Grass[,] grass;
        Apple apple;

        static readonly Random random = new Random();
        static readonly object syncLock = new object();

        public Level(IServiceProvider serviceProvider, GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            this.serviceProvider = serviceProvider;
        }

        public void Initialize()
        {
            ContentManager contentManager = new ContentManager(serviceProvider, "Content");
            Texture2D snakeSpriteSheet = contentManager.Load<Texture2D>("snake");
            snake = new Snake(snakeSpriteSheet);
            grass = new Grass[SnakeGame.gridLength, SnakeGame.gridLength];
            for (int x = 0; x < grass.GetLength(0); x++)
            {
                for (int y = 0; y < grass.GetLength(1); y++)
                {
                    grass[x, y] = new Grass(snakeSpriteSheet, new Vector2(x * SnakeGame.spriteSize,
                                                                          y * SnakeGame.spriteSize));
                }
            }
            apple = new Apple(snakeSpriteSheet);
            LoadContent();
        }

        public void LoadContent()
        {
            int xCenter = (int)SnakeGame.viewPort.X / 2;
            int yCenter = (int)SnakeGame.viewPort.Y / 2;
            Vector2 playerStartingPosition = new Vector2(xCenter, yCenter);
            snake.Initialize(playerStartingPosition, PlayerStartingDirection, PlayerStartingLength);
            apple.Position = GetRandomFreeLocation();
        }

        public void Update()
        {
            snake.Update();
            if (Helpers.CheckCollision(snake.snakeHead.Position, apple.Position))
            {
                snake.AteApple = true;
                apple.Position = GetRandomFreeLocation();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Grass item in grass)
            {
                item.Draw(spriteBatch);
            }
            snake.Draw(spriteBatch);
            apple.Draw(spriteBatch);
        }

        Vector2 GetRandomFreeLocation()
        {
            while (true)
            {
                lock (syncLock)
                {
                    int x = random.Next(1, SnakeGame.gridLength) * SnakeGame.spriteSize;
                    int y = random.Next(1, SnakeGame.gridLength) * SnakeGame.spriteSize;
                    if (LocationIsFree(x, y))
                    {
                        return new Vector2(x, y);
                    }
                }
            }
        }

        bool LocationIsFree(int x, int y)
        {
            Vector2 wantedPosition = new Vector2(x, y);
            if (wantedPosition == snake.snakeHead.Position)
            {
                return false;
            }
            foreach (var bodyPart in snake.snakeBody.bodyParts)
            {
                if (wantedPosition == bodyPart.Position)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
