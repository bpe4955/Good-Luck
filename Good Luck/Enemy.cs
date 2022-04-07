using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Good_Luck
{
    class Enemy : Entity, IDamageable
    {
        // Fields
        private int maxHealth;
        private int health;
        private int bulletSpeed;
        private int score;

        // Properties
        public int MaxHealth { get { return maxHealth; } }
        public int Health { get { return health; } set { health = value; } }
        public int Score { get { return score; } set { score = value; } }

        public int DefenseStat { get; set; }
        /// <summary>
        /// Creates a new <see cref="Enemy"/>
        /// </summary>
        /// <param name="enemyRect">The rect of this <see cref="Enemy"/></param>
        /// <param name="enemyTexture">The texture of this <see cref="Enemy"/></param>
        /// <param name="speed">The speed of this <see cref="Enemy"/></param>
        /// <param name="maxhealth">The maximum amount of health for this <see cref="Enemy"/></param>
        /// <param name="bulletSpeed">The bullet speed of this <see cref="Enemy"/></param>
        /// <param name="score">How much this <see cref="Enemy"/> is worth</param>
        public Enemy(Rectangle enemyRect, Texture2D enemyTexture, float speed, int maxhealth, int bulletSpeed, int score)
        : base(enemyTexture, enemyRect, speed)
        {
            this.maxHealth = maxhealth;
            this.health = maxhealth;
            this.bulletSpeed = bulletSpeed;
            this.score = score;
        }
        /// <summary>
        /// Draws the <see cref="Enemy"/>
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch"/> to draw with</param>
        public override void Draw(SpriteBatch sb)
        {
            if (isActive)
            {
                sb.Draw(texture, rect, Color.White);
            }
        }
        /// <summary>
        /// Is this <see cref="Enemy"/> colliding with another <see cref="Entity"/>
        /// </summary>
        /// <param name="other">The other <see cref="Entity"/></param>
        /// <returns>Is the <see cref="Enemy"/> is colliding</returns>
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

        /// <summary>
        /// Moves the <see cref="Enemy"/>
        /// </summary>
        public void Move()
        {
            Player p = EntityManager.Instance.Player;
            float enemyAndPlayerAngle = new Vector2(p.Rect.X - rect.X, p.Rect.Y - rect.Y).GetAngle();
            int xOffset = (int)(Math.Sin(enemyAndPlayerAngle) * speed);
            int yOffset = (int)(-Math.Cos(enemyAndPlayerAngle) * speed);

            rect.X += xOffset;
            rect.Y += yOffset;
        }
        /// <summary>
        /// Decreases the Health by the given amount
        /// </summary>
        /// <param name="amount">How much to decrease the health</param>
        public void TakeDamage(int amount)
        {
            health-=amount;

            if (health <= 0)
            {
                isActive = false;
            }
        }
        /// <summary>
        /// Increases the Health by the given amount
        /// </summary>
        /// <param name="amount">How much to increase the health</param>
        public void Heal(int amount)
        {
            if (amount + health > MaxHealth)
            {
                health = MaxHealth;
            }
            else
            {
                health += amount;
            }
        }
    }
}
