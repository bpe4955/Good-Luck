using System;
using System.Collections.Generic;
using System.Text;

namespace Good_Luck
{
    interface IDamageable
    {
        public int Health { get; set; }
        public int DefenseStat { get; set; }
        public int MaxHealth { get; protected set; }
        public void TakeDamage(int amount);
        public void Heal(int amount);
    }
}
