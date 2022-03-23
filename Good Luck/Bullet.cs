using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Good_Luck
{
    class Bullet : Entity
    {
        public Bullet(Rectangle bulletRect, Texture2D bulletTexture, float bulletSpeed)
            :base(bulletTexture, bulletRect, bulletSpeed)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }

        public override bool IsColliding(Entity other)
        {
            throw new NotImplementedException();
        }
    }
}
