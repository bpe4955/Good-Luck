// Brian Egan
// 3-25-2022
// Basic wall class with a rectangle and texture
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Good_Luck
{
    class Wall 
    {
        //fields
        private Rectangle rect;
        private Texture2D texture;

        //Properties
        /// <summary>
        /// Get and set the wall's rectangle
        /// </summary>
        public Rectangle Rect { get => rect; set => rect = value; }
        /// <summary>
        /// Get and set the wall's texture
        /// </summary>
        public Texture2D Texture { get => texture; set => texture = value; }

        //Constructor
        /// <summary>
        /// Constructor for a wall with a rectangle and texture
        /// </summary>
        /// <param name="rect">The position and size of the wall</param>
        /// <param name="texture">The texture of the wall</param>
        public Wall(Rectangle rect, Texture2D texture)
        {
            this.rect = rect;
            this.texture = texture;
        }
    }
}
