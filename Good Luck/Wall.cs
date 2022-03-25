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
        public Rectangle Rect { get => rect; set => rect = value; }
        public Texture2D Texture { get => texture; set => texture = value; }

        //Constructor
        public Wall(Rectangle rect, Texture2D texture)
        {
            this.rect = rect;
            this.texture = texture;
        }
    }
}
