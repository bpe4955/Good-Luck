using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Good_Luck
{
    /// <summary>
    /// Delegate for a void method with a wall input
    /// </summary>
    /// <param name="wall">A wall object</param>
    delegate void RoomDelegate(Wall wall);
    /// <summary>
    /// Delegate for a void method with a player input
    /// </summary>
    /// <param name="player">The player object</param>
    delegate void PlayerDelegate(Player player);

    class EntityManager
    {
        //Events
        public event RoomDelegate DoorCollided;
        public event PlayerDelegate PlayerInWall;

        //Fields
        public int roomIndex;

        //Auto-Properties
        /// <summary>
        /// All the <see cref="Bullet"/>s on screen
        /// </summary>
        public List<Bullet> Bullets { get; set; }
        /// <summary>
        /// All the <see cref="Enemy"/>s on screen
        /// </summary>
        public List<List<Enemy>> Enemies { get; set; }
        /// <summary>
        /// The main <see cref="Good_Luck.Player"/>
        /// </summary>
        public Player Player { get; private set; }
        /// <summary>
        /// All the <see cref="Wall"/>s in the level
        /// </summary>
        public List<Wall> Walls { get; set; }
        /// <summary>
        /// Static instance of the <see cref="EntityManager"/> class
        /// </summary>
        public static EntityManager Instance { get; private set; }

        /// <summary>
        /// Creates a new <see cref="EntityManager"/>
        /// </summary>
        /// <param name="player">reference to the <see cref="Good_Luck.Player"/></param>
        public EntityManager(Player player)
        {
            Player = player;
            Bullets = new List<Bullet>();
            Enemies = new List<List<Enemy>>();
            Walls = new List<Wall>();
            Instance = this;
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
            for (int i = 0; i < Enemies[roomIndex].Count; ++i)
            {
                Enemies[roomIndex][i].Draw(sb);
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
        public void UpdateEntities(GraphicsDeviceManager _graphics, KeyboardState kb, Texture2D bulletTexture, GameTime gameTime, MouseState mb)
        {
            //Move Player
            Player.Move(kb, mb);
            //Enemies attacking player
            int damage;
            for (int i = 0; i < Enemies[roomIndex].Count; ++i)
            {
                if (Enemies[roomIndex][i].IsActive)
                {
                    Enemies[roomIndex][i].Move();
                    WallCollisionWithEnemies(Enemies[roomIndex][i]);
                    if((damage = Enemies[roomIndex][i].Attack(bulletTexture)) > -1)
                    {
                        Player.TakeDamage(damage);
                    }
                }
            }
            //Wall Collision
            WallCollision();
            //Bullet Collisions
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

                //Wall/Bullet collisions
                for(int w = 0; w < Walls.Count; ++w)
                {
                    if (Walls[w].Rect.Intersects(Bullets[i].Rect))
                    {
                        Bullets.RemoveAt(i);
                        return;
                    }
                }

                //When the bullet hits an enemy, delete the bullet and make the enemy take damage
                for(int e = 0; e < Enemies[roomIndex].Count; ++e)
                {
                    if (Enemies[roomIndex][e].IsActive && Enemies[roomIndex][e].IsColliding(Bullets[i]))
                    {
                        Bullets.RemoveAt(i);
                        Enemies[roomIndex][e].TakeDamage(Player.Damage);
                        if (!Enemies[roomIndex][e].IsActive)
                        {
                            Player.TotalScore += Enemies[roomIndex][e].Score;
                            Enemies[roomIndex].RemoveAt(e);
                        }
                        return;
                    }
                }
                //If this bullet hits nothing, move on to the next bullet
                Bullets[i].Move();
                ++i;
            }

            //Remove inactive objects
            Enemies[roomIndex] = Enemies[roomIndex].RemoveInactive();
            Bullets = Bullets.RemoveInactive();
        }

        //Helper Methods
        /// <summary>
        /// Deal with player overlapping with a wall
        /// </summary>
        private void WallCollision()
        {
            if (Walls.Exists(x => x.Rect.Intersects(Player.Rect)))
            {
                //loop through every wall
                foreach (Wall wall in Walls)
                {
                    //Get the overlapping rectangle
                    Rectangle rect = Rectangle.Intersect(wall.Rect, Player.Rect);
                    Vector2 pos = new Vector2(Player.Rect.X, Player.Rect.Y);
                    //If there is a collision and the wall is a door
                    if(rect.Height != 0 && wall.IsDoor && Enemies[roomIndex].Count == 0)
                    {
                        DoorCollided(wall);
                        PlayerInWall(Player);
                        break;
                    }
                    RepositionCollision(Player, wall, rect, pos);
                }
            }
        }
        /// <summary>
        /// Checks collision of enemies with walls
        /// </summary>
        /// <param name="enemy">The enemy to check</param>
        private void WallCollisionWithEnemies(Enemy enemy)
        {
            if (Walls.Exists(x => x.Rect.Intersects(enemy.Rect)))
            {
                foreach (Wall wall in Walls)
                {
                    //Get the overlapping rectangle
                    Rectangle rect = Rectangle.Intersect(wall.Rect, enemy.Rect);
                    Vector2 pos = new Vector2(enemy.Rect.X, enemy.Rect.Y);
                    RepositionCollision(enemy, wall, rect, pos);
                }
            }
        }
        /// <summary>
        /// repositions an entity after wall collision
        /// </summary>
        /// <param name="entity">The entity to reposition</param>
        /// <param name="wall">The wall to reposition</param>
        /// <param name="rect">The intersection rect</param>
        /// <param name="pos">The current pos of the entity</param>
        private void RepositionCollision(Entity entity, Wall wall, Rectangle rect, Vector2 pos)
        {
            //If Rectangle has height and width of Zero, the player would not be moved at all
            //So we can exit early
            if (rect.Size == Point.Zero)
                return;

            //If the overlap is taller than it is wide
            if (rect.Width < rect.Height)
            {
                if (wall.Rect.X > entity.Rect.X)
                {
                    pos.X -= rect.Width;
                }
                else
                {
                    pos.X += rect.Width;
                }
            }
            //If the overlap is wider than it is tall
            else if(rect.Height < rect.Width)
            {
                if (wall.Rect.Y > entity.Rect.Y)
                {
                    pos.Y -= rect.Height;
                }
                else
                {
                    pos.Y += rect.Height;
                }
            }

            //Update entity location
            entity.Rect = new Rectangle((int)pos.X, (int)pos.Y, entity.Rect.Width, entity.Rect.Height);
        }
    }
}
