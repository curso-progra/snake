using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace snake
{
    class SnakeHead
    {
        public int Direction { get; set; }
        public Vector2 Position { get; set; }

        readonly Sprite sprite;

        public SnakeHead(Texture2D spriteSheet)
        {
            sprite = new Sprite(spriteSheet, 16, 16);
            sprite.Tiles = new System.Collections.Generic.Dictionary<int, Rectangle>
            {
                { 1, new Rectangle(0, 0, sprite.Width, sprite.Height) },
                { 2, new Rectangle(16, 0, sprite.Width, sprite.Height) },
                { 3, new Rectangle(32, 0, sprite.Width, sprite.Height) },
                { 4, new Rectangle(48, 0, sprite.Width, sprite.Height) }
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite.SpriteSheet, Position, sprite.Tiles[Direction], Color.White, 0f,
                             new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
        }
    }
}
