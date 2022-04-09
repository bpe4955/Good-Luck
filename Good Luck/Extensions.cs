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
        /// <summary>
        /// Gets the angle of a vector (+ 90 degrees)
        /// </summary>
        /// <param name="vector">The vector to get the angle of</param>
        /// <returns>The angle</returns>
        public static float GetAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X) + (float)(90 * Math.PI/180);
        }
        /// <summary>
        /// Removes inactive entities from given list
        /// </summary>
        /// <typeparam name="T">What type of entity</typeparam>
        /// <param name="entities">The list of entities to search</param>
        /// <returns>The new list with only active entities</returns>
        public static List<T> RemoveInactive<T>(this List<T> entities)
        {
            if(entities.Count > 0)
            {
                if(entities[0] is Entity)
                {
                    for (int i = entities.Count - 1; i >= 0; --i)
                    {
                        if (!(entities[i] as Entity).IsActive)
                        {
                            entities.RemoveAt(i);
                        }
                    }
                }
            }
            return entities;
        }

        public static Enemy CreateBunny(this Rectangle rect)
        {
            return new Enemy(rect, Game1.enemyTexture, 5, 10, -5, 20, Game1.sadEnemy);
        }
    }
}
