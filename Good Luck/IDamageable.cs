using System;
using System.Collections.Generic;
using System.Text;

namespace Good_Luck
{
    interface IDamageable
    {
        /// <summary>
        /// The health of this <see cref="IDamageable"/> Object
        /// </summary>
        public int Health { get; set; }
        /// <summary>
        /// How much defense this <see cref="IDamageable"/> Object
        /// </summary>
        public int DefenseStat { get; set; }
        /// <summary>
        /// What's he highest amount of health this <see cref="IDamageable"/> Object can have
        /// </summary>
        public int MaxHealth { get; }
        /// <summary>
        /// Reduces the health of this <see cref="IDamageable"/> Object
        /// by given amount
        /// </summary>
        /// <param name="amount">How much to decrease the health by</param>
        public void TakeDamage(int amount);
        /// <summary>
        /// Increases the health of this <see cref="IDamageable"/> Object
        /// by given amount
        /// </summary>
        /// <param name="amount">How much to increase the health by</param>
        public void Heal(int amount);
    }
}
