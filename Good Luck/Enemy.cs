using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Timers;
using System.Diagnostics;

namespace Good_Luck
{
    public class Enemy : Entity, IDamageable
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
        private Texture2D bulletTexture;
        private int direction;

        // Properties
        public int MaxHealth { get { return maxHealth; } }
        public int Health { get { return health; } set { health = value; } }
        public int Score { get { return score; } set { score = value; } }

        public int DefenseStat { get; set; }
        public int Damage { get { return damage; } }

        private Color drawColor;

        /// <summary>
        /// Creates a new <see cref="Enemy"/>
        /// </summary>
        /// <param name="enemyRect">The rect of this <see cref="Enemy"/></param>
        /// <param name="enemyTexture">The texture of this <see cref="Enemy"/></param>
        /// <param name="speed">The speed of this <see cref="Enemy"/></param>
        /// <param name="maxhealth">The maximum amount of health for this <see cref="Enemy"/></param>
        /// <param name="bulletSpeed">The bullet speed of this <see cref="Enemy"/></param>
        /// <param name="score">How much this <see cref="Enemy"/> is worth</param>
        public Enemy(Rectangle enemyRect, Texture2D enemyTexture, float speed, int maxhealth, int bulletSpeed, int score, Texture2D crying, Color drawColor)
        : base(enemyTexture, enemyRect, speed)
        {
            this.maxHealth = maxhealth;
            this.health = maxhealth;
            this.bulletSpeed = bulletSpeed;
            this.score = score;
            reloadSpeed = 70;
            damage = 3;
            pause = true;
            this.crying = crying;
            this.drawColor = drawColor;
            bulletTexture = Game1.carrotTexture;
            direction = 1;
            moveTime = 50;
        }
        /// <summary>
        /// Draws the <see cref="Enemy"/>
        /// </summary>ss
        /// <param name="sb">The <see cref="SpriteBatch"/> to draw with</param>
        public override void Draw(SpriteBatch sb)
        {
            if (isActive)
            {
                if(maxHealth == health)
                {
                    sb.Draw(texture, rect, drawColor);
                }
                else
                {
                    sb.Draw(crying, rect, drawColor);

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
        public int Attack()
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
                    reloadSpeed = Game1.rng.Next(30, 50);
                    EntityManager.Instance.Bullets.Add(Shoot());
                }
                else
                {
                    --reloadSpeed;
                }
            }
            return -1;
        }

        private Bullet Shoot()
        {
            Player p = EntityManager.Instance.Player;

            int halfWidth = rect.Width / 2;
            int halfHeight = rect.Height / 2;

            Rectangle bulletRect = new Rectangle(rect.X + halfWidth, rect.Y + halfHeight, 12 * Game1.screenScale, (int)(12 * 3.25f * Game1.screenScale));
            Vector2 mouseBetweenPlayer = new Vector2(p.Rect.X - rect.X, p.Rect.Y - rect.Y);
            
            return new Bullet(bulletRect, bulletTexture, bulletSpeed, this, mouseBetweenPlayer.GetAngle());
        }

        /// <summary>
        /// Moves the <see cref="Enemy"/>
        /// </summary>
        public void Move()
        {
            
            if (bulletSpeed <= -1) //Code for bunny bomb.
            {
                if (moveTime <= 0)
                {
                    moveTime = 20;
                    if (Game1.rng.Next(1,20) == 1)
                    {
                        pause = !pause;
                    }
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
            else // Code for Bunny Shooter
            {
                Player p = EntityManager.Instance.Player;

                if (moveTime <= 0)
                {
                    if(Vector2.Distance(new Vector2(p.Rect.X, p.Rect.Y), new Vector2(Rect.X, Rect.Y)) > 250)
                    {
                        direction = 1;
                    }
                    else
                    {
                        direction = -1;
                    }
                    moveTime = 20;
                    pause = !pause;
                }
                else
                {
                    if (!pause)
                    {
                        float enemyAndPlayerAngle = new Vector2(p.Rect.X - rect.X, p.Rect.Y - rect.Y).GetAngle();
                        int xOffset = (int)(Math.Sin(enemyAndPlayerAngle) * speed);
                        int yOffset = (int)(-Math.Cos(enemyAndPlayerAngle) * speed);

                        rect.X += xOffset * direction;
                        rect.Y += yOffset * direction;

                    }
                    --moveTime;
                }
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
