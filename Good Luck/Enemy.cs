using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Good_Luck
{
    class Enemy
    {
        public int Health { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int DefenseStat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int MaxHealth => throw new NotImplementedException();

        public override void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }

        public void Heal(int amount)
        {
            throw new NotImplementedException();
        }

        public override bool IsColliding(Entity other)
        {
            throw new NotImplementedException();
        }

        public void TakeDamage(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
