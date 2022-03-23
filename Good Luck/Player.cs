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

        // Properties
        public int MaxHealth { get { return health; } }
        public int Health { get { return health; } set { health = value; } }
        public int DefenseStat { get { return defenseStat; } set { defenseStat = value; } }

        public Player(Rectangle playerRect, Texture2D playerCharacter, float speed, int maxhealth, int defense) : base()
        {
            Rect = playerRect;
            Texture = playerCharacter;
            Speed = speed;
            IsActive = true;

            this.maxhealth = maxhealth;
            this.health = maxhealth;
            this.defenseStat = defense;
        }

        public void Move(KeyboardState kb)
        {
            /*
            if (kb.IsKeyDown(Keys.W)) { rect.X -= Speed; }
            if (kb.IsKeyDown(Keys.A)) { rect.X -= Speed; }
            if (kb.IsKeyDown(Keys.S)) { rect.Y += Speed; }
            if (kb.IsKeyDown(Keys.D)) { rect.X += Speed; }
            */
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

        /// <summary>
        /// Heals the Player's Total Health by the amount specified
        /// </summary>
        /// <param name="amount"></param>
        public void Heal(int amount)
        {
            if (amount + health > maxhealth)
            {
                health = maxhealth;
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
            if (amount >= health)
            {
                health = 0;
                IsActive = false;
            }
            else
            {
                health -= amount;
            }
            
        }
    }
}
