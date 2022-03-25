using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Good_Luck
{
    class Enemy : Entity
    {
        // Fields
        private int maxhealth;
        private int health;
        private int bulletSpeed;

        // Properties
        public int MaxHealth { get { return health; } }
        public int Health { get { return health; } set { health = value; } }

        public Enemy(Rectangle enemyRect, Texture2D enemyTexture, float speed, int maxhealth, int bulletSpeed)
        : base(enemyTexture, enemyRect, speed)
        {
            this.maxhealth = maxhealth;
            this.health = maxhealth;
            this.bulletSpeed = bulletSpeed;
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

            if (Rect.Intersects(collidedRect) && other is Bullet)
            {
                Bullet shot = (Bullet)other;
                if (shot.BulletOwner != this)
                {
                    return true;
                }
            }

            return false;
        }

        public void TakeDamage(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
