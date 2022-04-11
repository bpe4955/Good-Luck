using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Timers;

namespace Good_Luck
{
    class Enemy : Entity, IDamageable
    {
        // Fields
        private int maxHealth;
        private int health;
        private int bulletSpeed;
        private int score;
        private int reloadSpeed;
        private int damage;
        private int moveTime;
        private bool pause;
        private Texture2D crying;

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
        public Enemy(Rectangle enemyRect, Texture2D enemyTexture, float speed, int maxhealth, int bulletSpeed, int score, Texture2D crying)
        : base(enemyTexture, enemyRect, speed)
        {
            this.maxHealth = maxhealth;
            this.health = maxhealth;
            this.bulletSpeed = bulletSpeed;
            this.score = score;
            reloadSpeed = 20;
            damage = 3;
            pause = false;
            this.crying = crying;
        }
        /// <summary>
        /// Draws the <see cref="Enemy"/>
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch"/> to draw with</param>
        public override void Draw(SpriteBatch sb)
        {
            if (isActive)
            {
                if (health != maxHealth)
                {
                    sb.Draw(crying, rect, Color.White);
                }
                else
                {
                    sb.Draw(texture, rect, Color.White);
                }
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
            else if(Rect.Intersects(collidedRect) && other is Player)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Attacks the player when called
        /// </summary>
        /// <returns>The damage amount for mele attacks</returns>
        public int Attack(Texture2D bullectTexture)
        {
            //Code for bunny attack. Attacks by running into player
            if(bulletSpeed <= -1)
            {
                if (IsColliding(EntityManager.Instance.Player))
                {
                    IsActive = false;
                    return damage;
                }
            }
            //Code for shooting. Creates a bullet
            else
            {
                if (reloadSpeed <= 0)
                {
                    reloadSpeed = 20;
                    EntityManager.Instance.Bullets.Add(Shoot(bullectTexture));
                }
                else
                {
                    --reloadSpeed;
                }
            }
            return -1;
        }

        private Bullet Shoot(Texture2D bulletTexture)
        {
            Player p = EntityManager.Instance.Player;
            Vector2 mouseBetweenPlayer = new Vector2(p.Rect.X - rect.X, p.Rect.Y - rect.Y);
            Rectangle bulletRect = new Rectangle(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2), 25, 25);

            return new Bullet(bulletRect, bulletTexture, bulletSpeed, this, mouseBetweenPlayer.GetAngle());
        }

        /// <summary>
        /// Moves the <see cref="Enemy"/>
        /// </summary>
        public void Move()
        {
            if (moveTime <= 0)
            {
                moveTime = 20;
                pause = !pause;
            }
            else
            {
                if (!pause)
                {
                    Player p = EntityManager.Instance.Player;
                    float enemyAndPlayerAngle = new Vector2(p.Rect.X - rect.X, p.Rect.Y - rect.Y).GetAngle();
                    int xOffset = (int)(Math.Sin(enemyAndPlayerAngle) * speed);
                    int yOffset = (int)(-Math.Cos(enemyAndPlayerAngle) * speed);



                    rect.X += xOffset;
                    rect.Y += yOffset;
                }

                --moveTime;
            }
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
