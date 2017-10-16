using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace snake
{
    class SnakeBodyPart
    {
        public Vector2 Position { get; set; }
        public int CurrentTile { get; set; }
        public int PreviousDirection { get; set; }
        public SnakeBodyPart Next { get; set; }

        internal Sprite Sprite { get; set; }

        public SnakeBodyPart(Sprite sprite)
        {
            Sprite = sprite;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.SpriteSheet, Position, Sprite.Tiles[CurrentTile], Color.White, 0f,
                             new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
        }
    }
}
