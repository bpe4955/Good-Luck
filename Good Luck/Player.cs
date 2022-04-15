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
        private int maxHealth;
        private int health;
        private int damage;
        private int defenseStat;
        private int bulletSpeed;
        private int totalScore;
        private float angle;
        private Vector2 origin;

        // Properties
        public int MaxHealth { get { return maxHealth; } }
        public int Health { get { return health; } set { health = value; } }
        public int DefenseStat { get { return defenseStat; } set { defenseStat = value; } }
        public int Damage { get { return damage; } set { damage = value; } }
        public int TotalScore { get { return totalScore; } set { totalScore = value; } }

        /// <summary>
        /// Creates the Player Character that will be controlled in Gameplay
        /// </summary>
        /// <param name="playerRect"> Bounding Box of the Player </param>
        /// <param name="playerTexture"> Texture of the Player and how it looks </param>
        /// <param name="speed"> Speed of the Player's Movement </param>
        /// <param name="maxhealth"> Maximum Health of the Player </param>
        /// <param name="defense"> Defense against Damage caused to the Player </param>
        public Player(Rectangle playerRect, Texture2D playerTexture, float speed, int maxhealth, int defense, int bulletSpeed, int damage) 
            : base(playerTexture, playerRect, speed)
        {
            this.maxHealth = maxhealth;
            this.health = maxhealth;
            this.defenseStat = defense;
            this.damage = damage;
            this.bulletSpeed = bulletSpeed * Game1.screenScale;
            origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
        }

        public void Move(KeyboardState kb, MouseState mb)
        {
            if (kb.IsKeyDown(Game1.bindings[0])) { rect.Y -= (int)Math.Floor(speed); }
            if (kb.IsKeyDown(Game1.bindings[1])) { rect.X -= (int)Math.Floor(speed); }
            if (kb.IsKeyDown(Game1.bindings[2])) { rect.Y += (int)Math.Floor(speed); }
            if (kb.IsKeyDown(Game1.bindings[3])) { rect.X += (int)Math.Floor(speed); }
            angle = new Vector2(mb.Position.X - rect.X, mb.Position.Y - rect.Y).GetAngle();
        }
        public Bullet Shoot(MouseState mb, Texture2D bulletTexture)
        {
            int halfWidth = rect.Width / 2;
            int halfHeight = rect.Height / 2;

            Rectangle bulletRect = new Rectangle(rect.X + halfHeight, rect.Y+halfHeight, 25*Game1.screenScale, 25 * Game1.screenScale);

            Bullet playerBullet = new Bullet(bulletRect, bulletTexture, bulletSpeed, this, angle);

            return playerBullet;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (isActive)
            {
                //sb.Draw(texture, new Rectangle(rect.X,rect.Y,rect.Width*2,rect.Height*2), Color.White);
                sb.Draw(texture, new Rectangle(Rect.X + (Rect.Width / 2), Rect.Y + (Rect.Height / 2), rect.Width, rect.Height),
                    null, Color.White, angle, origin, SpriteEffects.None, 0);
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
