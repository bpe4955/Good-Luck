using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Good_Luck
{
    public static class Extensions
    {
        /// <summary>
        /// Creates a texture out of a source texture and a bounding box
        /// </summary>
        /// <param name="source">The texture to take from</param>
        /// <param name="souceBox">The part of the texture to take</param>
        /// <param name="graphics">The current <see cref="GraphicsDevice"/></param>
        /// <returns></returns>
        public static Texture2D GetTexture(this Texture2D source, Rectangle souceBox, GraphicsDevice graphics)
        {
            //Get how big the part of the texture we want is
            int size = souceBox.Width * souceBox.Height;

            //Create a color array to hold all the data within that area
            Color[] textureData = new Color[size];

            //Get the color data from the source image
            source.GetData(0, souceBox, textureData, 0, size);

            //Create new texture to the correct size
            Texture2D newTexture = new Texture2D(graphics, souceBox.Width, souceBox.Height);

            //Put it into the newTexture variable
            newTexture.SetData(textureData);

            //Return the new texture
            return newTexture;
        }

        public static float GetAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }
    }
}
