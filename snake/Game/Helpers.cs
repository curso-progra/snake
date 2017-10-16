using Microsoft.Xna.Framework;

namespace snake
{
    static class Helpers
    {
        public static bool CheckCollision(Vector2 positionA, Vector2 positionB)
        {
            return (positionA == positionB);
        }
    }
}
