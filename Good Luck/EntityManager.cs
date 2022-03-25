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
    }
}
