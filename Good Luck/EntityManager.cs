using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Good_Luck
{
    class EntityManager
    {
        public List<Bullet> bullets { get; set; }
        public List<Enemy> enemies { get; set; }
        public Player player { get; private set; }

        public EntityManager(Player player)
        {
            this.player = player;
            bullets = new List<Bullet>();
            enemies = new List<Enemy>();
        }

        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < bullets.Count; ++i)
            {
                bullets[i].Draw(sb);
            }
            for (int i = 0; i < enemies.Count; ++i)
            {
                enemies[i].Draw(sb);
            }
            player.Draw(sb);
        }

        public void CheckBulletCollision(GraphicsDeviceManager _graphics)
        {
            for (int i = 0; i < bullets.Count;)
            {
                //When the bullet is off the screen
                //May want to replace with wall collision
                if (bullets[i].Rect.X <= 0 - bullets[i].Rect.Width || bullets[i].Rect.X >= _graphics.PreferredBackBufferWidth
                    || bullets[i].Rect.Y <= 0 - bullets[i].Rect.Height || bullets[i].Rect.Y >= _graphics.PreferredBackBufferHeight + bullets[i].Rect.Height)
                {
                    //Delete the button
                    bullets.RemoveAt(i);
                    return;
                }

                //When the bullet hits an enemy, delete the bullet and make the enemy take damage
                for(int e = 0; e < enemies.Count; ++e)
                {
                    if (enemies[e].IsActive && enemies[e].IsColliding(bullets[i]))
                    {
                        bullets.RemoveAt(i);
                        enemies[e].TakeDamage(player.Damage);
                        return;
                    }
                }
                //If this bullet hits nothing, move on to the next bullet
                bullets[i].Move();
                i++;
            }
        }
    }
}
