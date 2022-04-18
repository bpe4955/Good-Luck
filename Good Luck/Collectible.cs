using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Timers;

namespace Good_Luck
{
    public class Collectible : Entity
    {
        public int Score { get; private set; }
        public int HealthModifier { get; private set; }

        private Color drawColor;

        public Collectible(Rectangle rect, Texture2D texture, int score, int health, Color drawColor) :
            base(texture, rect, 0)
        {
            Score = score;
            HealthModifier = health;
            this.drawColor = drawColor;
        }
        public override void Draw(SpriteBatch sb)
        {
            if (IsActive)
            {
                sb.Draw(Texture, Rect, drawColor);
            }
        }

        public override bool IsColliding(Entity other) => Rect.Intersects(other.Rect) && other is Player;

        public int Collect(out bool isScore)
        {
            isScore = Score > 0;
            if (isScore)
            {
                return Score;
            }
            else
            {
                return HealthModifier;
            }
        }
    }
}
