﻿// Brian Egan
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
        private bool isDoor;
        private bool isStair;

        //Properties
        /// <summary>
        /// Get and set the wall's rectangle
        /// </summary>
        public Rectangle Rect { get => rect; set => rect = value; }
        /// <summary>
        /// Get and set the wall's texture
        /// </summary>
        public Texture2D Texture { get => texture; set => texture = value; }
        /// <summary>
        /// Get and set whether the wall is a door or not
        /// </summary>
        public bool IsDoor { get => isDoor; set => isDoor = value; }
        public bool IsStair { get => isStair; set => isStair = value; }

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
            isDoor = false;
            isStair = false;
        }
        /// <summary>
        /// Constructor for an invisible wall with only a rectangle
        /// </summary>
        /// <param name="rect">The position and size of the wall</param>
        public Wall(Rectangle rect)
        {
            this.rect = rect;
            isDoor = false;
            isStair = false;
        }

        //Method
        /// <summary>
        /// Draws the wall to the screen
        /// </summary>
        /// <param name="sb">The spritebatch needed to draw</param>
        public void Draw(SpriteBatch sb)
        {
            if(texture != null) { sb.Draw(texture, rect, Color.White); }
        }
    }
}
