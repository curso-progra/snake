using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace snake
{
    class Grass
    {
        Vector2 Position { get; set; }

        readonly Sprite sprite;
        Texture2D spriteSheet;

        public Grass(Texture2D spriteSheet, Vector2 position)
        {
            this.spriteSheet = spriteSheet;
            sprite = new Sprite(spriteSheet, 16, 16);
            sprite.Tiles = new System.Collections.Generic.Dictionary<int, Rectangle>
            {
                { 1, new Rectangle(48, 48, sprite.Width, sprite.Height) }
            };
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite.SpriteSheet, Position, sprite.Tiles[1], Color.White, 0f,
                             new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
        }
    }
}