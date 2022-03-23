using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Good_Luck
{
    class Player : Entity
    {
        public Player(Rectangle playerRect, Texture2D playerCharacter, float speed)
        {
            Rect = playerRect;
            Texture = playerCharacter;
            Speed = speed;
            IsActive = true;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Rect, Color.White);
        }

        public override bool IsColliding(Entity other)
        {
            Rectangle collidedRect = new Rectangle(other.Rect.X, other.Rect.Y, other.Rect.Width, other.Rect.Height);

            if (Rect.Intersects(collidedRect))
            {
                return true;
            }
            return false;
        }
    }
}
