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
        private int score;

        // Properties
        public int MaxHealth { get { return health; } }
        public int Health { get { return health; } set { health = value; } }
        public int Score { get { return score; } set { score = value; } }

        public Enemy(Rectangle enemyRect, Texture2D enemyTexture, float speed, int maxhealth, int bulletSpeed, int score)
        : base(enemyTexture, enemyRect, speed)
        {
            this.maxhealth = maxhealth;
            this.health = maxhealth;
            this.bulletSpeed = bulletSpeed;
            this.score = score;
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

            if (Rect.Intersects(collidedRect) && other is Bullet && other.IsActive)
            {
                Bullet shot = (Bullet)other;
                if (shot.BulletOwner != this)
                {
                    return true;
                }
            }

            return false;
        }


        public void Move()
        {

        }

        public void TakeDamage(int amount)
        {
            health-=amount;

            if (health <= 0)
            {
                isActive = false;
            }
        }
    }
}
