using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Good_Luck
{
    class KeybindButton
    {
        public Texture2D DefaultImage { get; private set; }
        public Color SelectedTint { get; private set; }
        public Color TextColor { get; private set; }
        public Keys Key { get; private set; }
        public bool Selected { get; set; }
        private Vector2 fontSize;
        private SpriteFont font;
        public Rectangle Rect { get; private set; }

        public KeybindButton(Texture2D defaultImage, Color selectedTint, Keys key, Color textColor, SpriteFont font, Point pos)
        {
            DefaultImage = defaultImage;
            SelectedTint = selectedTint;
            Key = key;
            TextColor = textColor;
            this.font = font;
            fontSize = font.MeasureString(Key.ToString());
            Rect = new Rectangle(pos.X, pos.Y, 50*Game1.screenScale, 50 * Game1.screenScale);
        }

        public void Rebind()
        {
            if (Selected)
            {
                Keys key = Keyboard.GetState().GetPressedKeys()[0];
                if (!Game1.bindings.Contains(key))
                {
                    Key = key;
                    fontSize = font.MeasureString(Key.ToString());
                }
                Selected = false;
            }
        }

        /// <summary>
        /// Checks if the current <see cref="Mouse"/> position
        /// is contained in the <see cref="KeybindButton"/>'s position rectangle
        /// </summary>
        /// <param name="ms">The current <see cref="MouseState"/></param>
        /// <returns>True if the <see cref="Mouse"/>'s position is within the <see cref="KeybindButton"/></returns>
        public bool Collision(MouseState ms)
        {
            return Rect.Contains(ms.Position);
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            if (Selected)
            {
                _spriteBatch.Draw(DefaultImage, Rect, SelectedTint);
            }
            else
            {
                _spriteBatch.Draw(DefaultImage, Rect, Color.White);
            }
            _spriteBatch.DrawString(font, Key.ToString(),
                new Vector2(Rect.X + (int)(25 * Game1.screenScale - (fontSize.X / 2)),
                            Rect.Y + (int)(25 * Game1.screenScale - (fontSize.Y / 2))), TextColor);
        }

    }
}
