// Brian Egan
// 3-18-2022
// Class for buttons that can be clicked and hold text
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using System.IO;

namespace Good_Luck
{
    class Button
    {
        //Fields
        private Texture2D defaultImage;
        private Texture2D hoverImage;
        private Rectangle rect;
        private bool isHovered;
        private GameState gameState;

        //Constructor
        /// <summary>
        /// creates a button with a position, default, hover, and click images
        /// </summary>
        /// /// <param name="gameState">The GameState the button returns</param>
        /// <param name="rect">The position and size of the button</param>
        /// <param name="defaultImage">The button's standard image</param>
        /// <param name="hoverImage">The image of the button when it is hovered</param>
        public Button(GameState gameState, Rectangle rect, Texture2D defaultImage, Texture2D hoverImage)
        {
            this.defaultImage = defaultImage;
            this.hoverImage = hoverImage;
            this.rect = rect;
            this.gameState = gameState;
            isHovered = false;
        }
        /// <summary>
        /// Constructor for a button with no hover image
        /// </summary>
        /// <param name="gameState">The GameState the button returns</param>
        /// <param name="rect">The position and size of the button</param>
        /// <param name="defaultImage">The button's standard image</param>
        public Button(GameState gameState, Rectangle rect, Texture2D defaultImage)
            : this(gameState, rect, defaultImage, defaultImage) { }


        //Methods
        /// <summary>
        /// Draws the button depending on if it is hovered or not
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            if (isHovered)
            {
                sb.Draw(hoverImage, rect, Color.White);
            }
            else
            {
                sb.Draw(defaultImage, rect, Color.White);
            }
        }
        /// <summary>
        /// Checks if the current mouse position
        /// is contained in the button's position rectangle
        /// </summary>
        /// <param name="ms">The current mouseState</param>
        /// <returns>True if the mouse's position is within the coin</returns>
        public bool Collision(MouseState ms)
        {
            isHovered = rect.Contains(ms.Position);
            return isHovered;
        }
        /// <summary>
        /// When the button is clicked, return its corresponding GameState value
        /// </summary>
        /// <returns>The button's GameState Value</returns>
        public GameState ClickedGetState()
        {
            return gameState;
        }
    }
}
