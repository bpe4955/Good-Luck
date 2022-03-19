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
        private Texture2D clickImage;
        private Rectangle rect;
        private bool isHovered;
        private bool isClicked;
        private GameState gameState;

        //Constructor
        /// <summary>
        /// creates a <see cref="Button"/> with a position, default, hover, and click images
        /// </summary>
        /// /// <param name="gameState">The <see cref="GameState"/> the <see cref="Button"/> returns</param>
        /// <param name="rect">The position and size of the <see cref="Button"/></param>
        /// <param name="defaultImage">The <see cref="Button"/>'s standard image</param>
        /// <param name="hoverImage">The image of the <see cref="Button"/> when it is hovered</param>
        /// <param name="clickImage">The image of the <see cref="Button"/> when clicked</param>
        public Button(GameState gameState, Rectangle rect, Texture2D defaultImage, Texture2D hoverImage, Texture2D clickImage)
        {
            this.defaultImage = defaultImage;
            this.hoverImage = hoverImage;
            this.rect = rect;
            this.gameState = gameState;
            this.clickImage = clickImage;
            isHovered = false;
            isClicked = false;
        }
        /// <summary>
        /// Constructor for a <see cref="Button"/> with no hover image
        /// </summary>
        /// <param name="gameState">The <see cref="GameState"/> the <see cref="Button"/> returns</param>
        /// <param name="rect">The position and size of the <see cref="Button"/></param>
        /// <param name="defaultImage">The <see cref="Button"/>'s standard image</param>
        public Button(GameState gameState, Rectangle rect, Texture2D defaultImage)
            : this(gameState, rect, defaultImage, defaultImage, defaultImage) { }


        //Methods
        /// <summary>
        /// Draws the <see cref="Button"/> depending on if it is hovered or not
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            if (isClicked)
            {
                sb.Draw(clickImage, rect, Color.White);
            }
            else if (isHovered)
            {
                sb.Draw(hoverImage, rect, Color.White);
            }
            else
            {
                sb.Draw(defaultImage, rect, Color.White);
            }
        }
        /// <summary>
        /// Checks if the current <see cref="Mouse"/> position
        /// is contained in the <see cref="Button"/>'s position rectangle
        /// </summary>
        /// <param name="ms">The current <see cref="MouseState"/></param>
        /// <returns>True if the <see cref="Mouse"/>'s position is within the <see cref="Button"/></returns>
        public bool Collision(MouseState ms)
        {
            isHovered = rect.Contains(ms.Position);
            return isHovered;
        }
        /// <summary>
        /// When the <see cref="Button"/> is clicked, return its corresponding <see cref="GameState"/> value
        /// </summary>
        /// <returns>The <see cref="Button"/>'s <see cref="GameState"/> Value</returns>
        public GameState ClickedGetState()
        {
            isClicked = true;
            return gameState;
        }
    }
}
