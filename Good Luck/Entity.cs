using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Good_Luck
{
    public abstract class Entity
    {
        // Fields
        protected Texture2D texture;
        protected Rectangle rect;
        protected bool isActive;
        protected float speed;


        /// <summary>
        /// The <see cref="Texture2D"/> of this <see cref="Entity"/>
        /// </summary>
        public Texture2D Texture { get { return texture; } set { texture = value; } }
        /// <summary>
        /// The <see cref="Rectangle"/> and bounding box of this <see cref="Entity"/>
        /// </summary>
        public Rectangle Rect { get { return rect; } set { rect = value; } }
        /// <summary>
        /// Whether or not this <see cref="Entity"/> is active
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        /// <summary>
        /// How fast this <see cref="Entity"/> moves
        /// </summary>
        public float Speed { get { return speed; } }
        /// <summary>
        /// Determines if this <see cref="Entity"/> is colliding
        /// with anything else
        /// </summary>
        /// <returns>Whether or not this <see cref="Entity"/> is colliding</returns>
        public abstract bool IsColliding(Entity other);
        /// <summary>
        /// Draws this <see cref="Entity"/> to he screen
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch"/> drawing the <see cref="Entity"/></param>
        public abstract void Draw(SpriteBatch sb);

        /// <summary>
        /// Base Constructor for Entities
        /// </summary>
        /// <param name="texture"> Entitie's Texture </param>
        /// <param name="rect"> Rectangle for the Entity which helps with collision </param>
        /// <param name="speed"> Speed of the Entity </param>
        public Entity(Texture2D texture, Rectangle rect, float speed)
        {
            isActive = true;

            this.texture = texture;
            this.rect = rect;
            this.speed = speed;
        }
    }
}
