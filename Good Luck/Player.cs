using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Good_Luck
{
    class Player : Entity, IDamageable
    {
        // Fields
        private int maxhealth;
        private int health;
        private int defenseStat;
        private int bulletSpeed;
        private int totalScore;

        // Properties
        public int MaxHealth { get { return health; } }
        public int Health { get { return health; } set { health = value; } }
        public int DefenseStat { get { return defenseStat; } set { defenseStat = value; } }
        public int TotalScore { get { return totalScore; } set { totalScore = value; } }

        /// <summary>
        /// Creates the Player Character that will be controlled in Gameplay
        /// </summary>
        /// <param name="playerRect"> Bounding Box of the Player </param>
        /// <param name="playerTexture"> Texture of the Player and how it looks </param>
        /// <param name="speed"> Speed of the Player's Movement </param>
        /// <param name="maxhealth"> Maximum Health of the Player </param>
        /// <param name="defense"> Defense against Damage caused to the Player </param>
        public Player(Rectangle playerRect, Texture2D playerTexture, float speed, int maxhealth, int defense, int bulletSpeed) 
            : base(playerTexture, playerRect, speed)
        {

            this.maxhealth = maxhealth;
            this.health = maxhealth;
            this.defenseStat = defense;
            this.bulletSpeed = bulletSpeed;
        }

        public void Move(KeyboardState kb)
        {
            if (kb.IsKeyDown(Keys.W)) { rect.Y -= (int)Math.Floor(speed); }
            if (kb.IsKeyDown(Keys.A)) { rect.X -= (int)Math.Floor(speed); }
            if (kb.IsKeyDown(Keys.S)) { rect.Y += (int)Math.Floor(speed); }
            if (kb.IsKeyDown(Keys.D)) { rect.X += (int)Math.Floor(speed); }
        }
        public Bullet Shoot(MouseState mb, Texture2D bulletTexture)
        {
            Vector2 mouseBetweenPlayer = new Vector2(mb.Position.X - rect.X, mb.Position.Y - rect.Y);
            Rectangle bulletRect = new Rectangle(rect.X, rect.Y, 25, 25);

            Bullet playerBullet = new Bullet(bulletRect, bulletTexture, bulletSpeed, this, Extensions.GetAngle(mouseBetweenPlayer));


            return playerBullet;
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

        /// <summary>
        /// Heals the Player's Total Health by the amount specified
        /// </summary>
        /// <param name="amount"></param>
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
        /// <summary>
        /// Damages Player by the amount specified
        /// </summary>
        /// <param name="amount"></param>
        public void TakeDamage(int amount)
        {
            if (amount - defenseStat >= health)
            {
                health = 0;
                isActive = false;
            }
            else
            {
                health -= amount - defenseStat;

            }
        }
    }
}
