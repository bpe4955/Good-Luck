using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Good_Luck
{
    abstract class Entity
    {
        private float speed;
        /// <summary>
        /// The <see cref="Texture2D"/> of this <see cref="Entity"/>
        /// </summary>
        public Texture2D Texture { get; set; }
        /// <summary>
        /// The <see cref="Rectangle"/> and bounding box of this <see cref="Entity"/>
        /// </summary>
        public Rectangle Rect { get; set; }
        /// <summary>
        /// Whether or not this <see cref="Entity"/> is active
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// How fast this <see cref="Entity"/> moves
        /// </summary>
        public float Speed { get { return speed; } }
        /// <summary>
        /// Determines if this <see cref="Entity"/> is colliding
        /// with anything else
        /// </summary>
        /// <returns>Whether or not this <see cref="Entity"/> is colliding</returns>
        public abstract bool IsColliding();
        /// <summary>
        /// Draws this <see cref="Entity"/> to he screen
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch"/> drawing the <see cref="Entity"/></param>
        public abstract void Draw(SpriteBatch sb);
    }
}
