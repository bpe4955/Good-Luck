using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Good_Luck
{
    class Bullet : Entity
    {
        private Entity bulletOwner;
        private float bulletAngle;

        public Bullet(Rectangle bulletRect, Texture2D bulletTexture, float bulletSpeed, Entity bulletOwner, float bulletAngle)
            :base(bulletTexture, bulletRect, bulletSpeed)
        {
            this.bulletOwner = bulletOwner;
            this.bulletAngle = bulletAngle;
        }

        public void Move()
        {
            int xOffset = (int)(Math.Sin(bulletAngle) * speed);
            int yOffset = -(int)(Math.Sin(bulletAngle) * speed);

            rect.X += xOffset;
            rect.Y += yOffset;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (isActive)
            {
                sb.Draw(texture, rect, Color.White);
            }
        }

        public override bool IsColliding(Entity other)
        {
            Rectangle collidedRect = new Rectangle(other.Rect.X, other.Rect.Y, other.Rect.Width, other.Rect.Height);
            if(other != bulletOwner)
            {
                if (Rect.Intersects(collidedRect))
                {
                    isActive = false;
                }
            }

            return false;
        }
    }
}
