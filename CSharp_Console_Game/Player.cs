using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Console_Game
{
    // Одиночка (порождающий паттерн)
    class Player
    {
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentProtection { get; set; }
        public int MaxProtection { get; set; }
        public int BaseAttack { get; set; }
        public SP SP { get; set; }
        public void SetPlayer(string SPName, int maxhealth, int protection, int baseAttack)
        {
            SP = SP.getInstance(SPName);
            MaxHealth = maxhealth;
            MaxProtection = protection;
            BaseAttack = baseAttack;

            CurrentHealth = maxhealth;
            CurrentProtection = protection;
        }
        public void Attack(Player player, Enemy enemy)
        {
            enemy.GetDamage(player);
            if (player.CurrentProtection > 0)
            {
                if (player.CurrentProtection >= enemy.CurrentDamage)
                    player.CurrentProtection -= enemy.CurrentDamage;
                else
                {
                    int temp = enemy.CurrentDamage - player.CurrentProtection;
                    player.CurrentProtection = 0;
                    player.CurrentHealth -= temp;
                }
            }
            else
                player.CurrentHealth -= enemy.CurrentDamage;
        }
    }
    class SP
    {
        private static SP instance;

        public string Name { get; private set; }

        protected SP(string name)
        {
            this.Name = name;
        }

        public static SP getInstance(string name)
        {
            if (instance == null)
                instance = new SP(name);
            return instance;
        }
    }
}
