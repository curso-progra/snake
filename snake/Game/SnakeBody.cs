using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace snake
{
    class SnakeBody
    {
        public int HeadDirection { get; set; }
        public int Length { get; set; }
        public readonly List<SnakeBodyPart> bodyParts;

        Sprite snakeBodySprite;
        Texture2D spriteSheet;

        static readonly Dictionary<int, Rectangle> BodySprites = new Dictionary<int, Rectangle>()
        {
            { 1, new Rectangle(0, 48, 16, 16) }, // BODY
            { 2, new Rectangle(16, 48, 16, 16) },
            { 3, new Rectangle(0, 48, 16, 16) },
            { 4, new Rectangle(16, 48, 16, 16) },
            { 5, new Rectangle(0, 32, 16, 16) }, // CURVES
            { 6, new Rectangle(16, 32, 16, 16) },
            { 7, new Rectangle(32, 32, 16, 16) },
            { 8, new Rectangle(48, 32, 16, 16) },
            { 9, new Rectangle(0, 16, 16, 16) }, // TAIL
            { 10, new Rectangle(16, 16, 16, 16) },
            { 11, new Rectangle(32, 16, 16, 16) },
            { 12, new Rectangle(48, 16, 16, 16) }
        };

        public SnakeBody(Texture2D spriteSheet)
        {
            bodyParts = new List<SnakeBodyPart>();
            this.spriteSheet = spriteSheet;
            snakeBodySprite = new Sprite(spriteSheet, 16, 16)
            {
                Tiles = BodySprites
            };
        }

        public void Initialize(Vector2 startingPosition, int length)
        {
            Length = length;
            for (int i = 0; i < Length; i++)
            {
                bodyParts.Add(new SnakeBodyPart(snakeBodySprite)
                {
                    Position = new Vector2(startingPosition.X - 16 * i, startingPosition.Y),
                    CurrentTile = HeadDirection,
                    PreviousDirection = HeadDirection
                });
                if (i > 0)
                {
                    bodyParts[i].Next = bodyParts[i - 1];
                }
            }
            bodyParts.First().Next = bodyParts.Last();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            bodyParts.ForEach((bodyPart) =>
            {
                bodyPart.Draw(spriteBatch);
            });
        }

        internal void Move(Vector2 newPosition, int previousHeadDirection)
        {
            SnakeBodyPart newFirstPart = bodyParts.Last();
            SnakeBodyPart newTail = newFirstPart.Next;
            newFirstPart.Position = newPosition;
            newFirstPart.PreviousDirection = HeadDirection;
            newFirstPart.CurrentTile = GetTileForFirstBodyPart(previousHeadDirection);
            newTail.CurrentTile = GetTileForTail(newTail);
            bodyParts.Insert(0, newFirstPart);
            bodyParts.RemoveAt(Length);
        }

        internal void Grow(Vector2 newPosition, int previousHeadDirection)
        {
            SnakeBodyPart firstPart = bodyParts.First();
            SnakeBodyPart tail = bodyParts.Last();
            SnakeBodyPart newPart = new SnakeBodyPart(snakeBodySprite)
            {
                Position = newPosition,
                CurrentTile = GetTileForFirstBodyPart(previousHeadDirection),
                PreviousDirection = HeadDirection,
                Next = tail
            };
            firstPart.Next = newPart;
            bodyParts.Insert(0, newPart);
        }

        int GetTileForFirstBodyPart(int previousHeadDirection)
        {
            int newTile = previousHeadDirection;
            if (previousHeadDirection != HeadDirection)
            {
                if ((previousHeadDirection == Snake.Left && HeadDirection == Snake.Up)
                    || previousHeadDirection == Snake.Down && HeadDirection == Snake.Right)
                {
                    newTile = 5;
                }
                if ((previousHeadDirection == Snake.Up && HeadDirection == Snake.Right)
                    || previousHeadDirection == Snake.Left && HeadDirection == Snake.Down)
                {
                    newTile = 6;
                }
                if ((previousHeadDirection == Snake.Right && HeadDirection == Snake.Down)
                   || previousHeadDirection == Snake.Up && HeadDirection == Snake.Left)
                {
                    newTile = 7;
                }
                if ((previousHeadDirection == Snake.Right && HeadDirection == Snake.Up)
                   || previousHeadDirection == Snake.Down && HeadDirection == Snake.Left)
                {
                    newTile = 8;
                }
            }
            return newTile;
        }

        int GetTileForTail(SnakeBodyPart newTail)
        {
            if (newTail.CurrentTile > 4)
            {
                return newTail.PreviousDirection + 8;
            }
            return newTail.CurrentTile + 8;
        }
    }
}
