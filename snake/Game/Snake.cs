using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace snake
{
    class Snake
    {
        public static int Step = 16;
        public static int Up = 1;
        public static int Right = 2;
        public static int Down = 3;
        public static int Left = 4;
        public bool AteApple { get; set; }
        public bool IsDead { get; set; }
        public readonly SnakeHead snakeHead;
        public SnakeBody snakeBody;

        Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                snakeHead.Position = value;
            }
        }
        Vector2 position;

        int Direction
        {
            get => direction;
            set
            {
                direction = value;
                snakeHead.Direction = value;
                snakeBody.HeadDirection = value;
            }
        }

        int direction;
        int previousDirection;

        public Snake(Texture2D spriteSheet)
        {
            snakeHead = new SnakeHead(spriteSheet);
            snakeBody = new SnakeBody(spriteSheet);
        }

        public void Initialize(Vector2 startingPosition, int direction, int length)
        {
            Direction = direction;
            Position = startingPosition;
            snakeBody.Initialize(new Vector2(startingPosition.X - 16, startingPosition.Y), length);
        }

        public void Update()
        {
            HandleInput();
            Move();
            foreach (var bodyPart in snakeBody.bodyParts)
            {
                IsDead |= Helpers.CheckCollision(snakeHead.Position, bodyPart.Position);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            snakeHead.Draw(spriteBatch);
            snakeBody.Draw(spriteBatch);
        }

        void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            previousDirection = Direction;
            if (keyboardState.IsKeyDown(Keys.Up) && Direction != Down)
            {
                Direction = Up;
            }
            else if
                (keyboardState.IsKeyDown(Keys.Right) && Direction != Left)
            {
                Direction = Right;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) && Direction != Up)
            {
                Direction = Down;
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && Direction != Right)
            {
                Direction = Left;
            }
        }

        public void Move()
        {
            Vector2 previousHeadPosition = position;
            switch (Direction)
            {
                case 1: // UP
                    position.Y -= Step;
                    break;
                case 2: // RIGHT
                    position.X += Step;
                    break;
                case 3: // DOWN
                    position.Y += Step;
                    break;
                case 4: // LEFT
                    position.X -= Step;
                    break;
            }
            CheckBounds();
            snakeHead.Position = position;
            MoveBody(previousHeadPosition);
        }

        void MoveBody(Vector2 previousHeadPosition)
        {
            if (AteApple)
            {
                snakeBody.Length++;
                snakeBody.Grow(previousHeadPosition, previousDirection);
                AteApple = false;
            }
            else
            {
                snakeBody.Move(previousHeadPosition, previousDirection);
            }
        }

        void CheckBounds()
        {
            if (position.X >= SnakeGame.viewPort.X)
            {
                position.X = 0;
            }
            if (position.Y >= SnakeGame.viewPort.Y)
            {
                position.Y = 0;
            }
            if (position.X < 0)
            {
                position.X = SnakeGame.viewPort.X;
            }
            if (position.Y < 0)
            {
                position.Y = SnakeGame.viewPort.Y;
            }
            snakeHead.Position = position;
        }
    }
}
