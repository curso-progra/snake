using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace snake
{
    class Sprite
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Texture2D SpriteSheet { get; set; }
        public Dictionary<int, Rectangle> Tiles { get; set; }

        public Sprite(Texture2D spriteSheet, int width, int height)
        {
            SpriteSheet = spriteSheet;
            Width = width;
            Height = height;
        }
    }
}
