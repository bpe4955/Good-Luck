﻿using Microsoft.Xna.Framework.Graphics;
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

        /// <summary>
        /// Creates the Player Character that will be controlled in Gameplay
        /// </summary>
        /// <param name="playerRect"> Bounding Box of the Player </param>
        /// <param name="playerTexture"> Texture of the Player and how it looks </param>
        /// <param name="speed"> Speed of the Player's Movement </param>
        /// <param name="maxhealth"> Maximum Health of the Player </param>
        /// <param name="defense"> Defense against Damage caused to the Player </param>
        public Player(Rectangle playerRect, Texture2D playerTexture, float speed, int maxhealth, int defense) 
            : base(playerTexture, playerRect, speed)
        {

            this.maxhealth = maxhealth;
            this.health = maxhealth;
            this.defenseStat = defense;
        }

        public void Move(KeyboardState kb)
        {
            if (kb.IsKeyDown(Keys.W)) { rect.Y -= (int)Math.Floor(speed); }
            if (kb.IsKeyDown(Keys.A)) { rect.X -= (int)Math.Floor(speed); }
            if (kb.IsKeyDown(Keys.S)) { rect.Y += (int)Math.Floor(speed); }
            if (kb.IsKeyDown(Keys.D)) { rect.X += (int)Math.Floor(speed); }
        }
        public void Shoot(KeyboardState kb)
        {
            if () 
            {
                System.Diagnostics.Debug.WriteLine("Shoot!");
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, rect, Color.White);
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
