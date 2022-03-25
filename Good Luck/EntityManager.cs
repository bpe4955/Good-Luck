using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Good_Luck
{
    class EntityManager
    {
        /// <summary>
        /// All the <see cref="Bullet"/>s on screen
        /// </summary>
        public List<Bullet> Bullets { get; set; }
        /// <summary>
        /// All the <see cref="Enemy"/>s on screen
        /// </summary>
        public List<Enemy> Enemies { get; set; }
        /// <summary>
        /// The main <see cref="Good_Luck.Player"/>
        /// </summary>
        public Player Player { get; private set; }
        /// <summary>
        /// All the <see cref="Wall"/>s in the level
        /// </summary>
        public List<Wall> Walls { get; set; }
        /// <summary>
        /// Creates a new <see cref="EntityManager"/>
        /// </summary>
        /// <param name="player">reference to the <see cref="Good_Luck.Player"/></param>
        public EntityManager(Player player)
        {
            Player = player;
            Bullets = new List<Bullet>();
            Enemies = new List<Enemy>();
            Walls = new List<Wall>();
        }
        /// <summary>
        /// Draws all the <see cref="Entity"/>s and <see cref="Wall"/>s
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch"/> to draw to</param>
        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < Bullets.Count; ++i)
            {
                Bullets[i].Draw(sb);
            }
            for (int i = 0; i < Enemies.Count; ++i)
            {
                Enemies[i].Draw(sb);
            }
            Player.Draw(sb);

            for(int i = 0; i < Walls.Count; ++i)
            {
                Walls[i].Draw(sb);
            }
        }
        /// <summary>
        /// Updates all the <see cref="Entity"/>s and checks for collisions
        /// </summary>
        /// <param name="_graphics">The main <see cref="GraphicsDeviceManager"/></param>
        /// <param name="kb">The current <see cref="KeyboardState"/></param>
        public void UpdateEntities(GraphicsDeviceManager _graphics, KeyboardState kb)
        {
            Player.Move(kb);
            if (Walls.Exists(x => x.Rect.Intersects(Player.Rect)))
            {
                Wall w = Walls.Find(x => x.Rect.Intersects(Player.Rect));
                Rectangle rect = Rectangle.Intersect(w.Rect, Player.Rect);
                Vector2 pos = new Vector2(Player.Rect.X, Player.Rect.Y);
                if (rect.Width <= rect.Height)
                {
                    if (w.Rect.X > Player.Rect.X)
                    {
                        pos.X -= rect.Width;
                    }
                    else
                    {
                        pos.X += rect.Width;
                    }
                }
                else
                {
                    if (w.Rect.Y > Player.Rect.Y)
                    {
                        pos.Y -= rect.Height;
                    }
                    else
                    {
                        pos.Y += rect.Height;
                    }
                }
                Player.Rect = new Rectangle((int)pos.X, (int)pos.Y, Player.Rect.Width, Player.Rect.Height);
            }
            for (int i = 0; i < Bullets.Count;)
            {
                //When the bullet is off the screen
                //May want to replace with wall collision
                if (Bullets[i].Rect.X <= 0 - Bullets[i].Rect.Width || Bullets[i].Rect.X >= _graphics.PreferredBackBufferWidth
                    || Bullets[i].Rect.Y <= 0 - Bullets[i].Rect.Height || Bullets[i].Rect.Y >= _graphics.PreferredBackBufferHeight + Bullets[i].Rect.Height)
                {
                    //Delete the button
                    Bullets.RemoveAt(i);
                    return;
                }

                //Wall collisions
                for(int w = 0; w < Walls.Count; ++w)
                {
                    if (Walls[w].Rect.Intersects(Bullets[i].Rect))
                    {
                        Bullets.RemoveAt(i);
                        return;
                    }
                }

                //When the bullet hits an enemy, delete the bullet and make the enemy take damage
                for(int e = 0; e < Enemies.Count; ++e)
                {
                    if (Enemies[e].IsActive && Enemies[e].IsColliding(Bullets[i]))
                    {
                        Bullets.RemoveAt(i);
                        Enemies[e].TakeDamage(Player.Damage);
                        if (!Enemies[e].IsActive)
                        {
                            Player.TotalScore += Enemies[e].Score;
                            Enemies.RemoveAt(e);
                        }
                        return;
                    }
                }
                //If this bullet hits nothing, move on to the next bullet
                Bullets[i].Move();
                ++i;
            }
        }
    }
}
