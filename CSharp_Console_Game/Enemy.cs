using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Console_Game
{
    // Фабрика (порождающий паттерн)
    public struct Point
    {
        public int X;
        public int Y;
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    abstract class Enemy
    {
        public string Name { get; set; }
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentProtection { get; set; }
        public int MaxProtection { get; set; }
        public int BaseAttack { get; set; }
        public int DamageModifier { get; set; }
        public int CurrentDamage { get; set; }
        public Point CurrentPos { get; set; }
        public Enemy(string name, int maxhealth, int protection, int baseAttack, int damageModifier)
        {
            Name = name;
            MaxHealth = maxhealth;
            MaxProtection = protection;
            BaseAttack = baseAttack;
            DamageModifier = damageModifier;

            CurrentHealth = maxhealth;
            CurrentProtection = protection;
            CurrentDamage = BaseAttack + CurrentProtection / 10 + DamageModifier;
        }
        public abstract Fight Attack();
        public void GetDamage(Player player)
        {
            if (this.CurrentProtection > 0)
            {
                if (this.CurrentProtection >= player.BaseAttack)
                    this.CurrentProtection -= player.BaseAttack;
                else
                {
                    int temp = player.BaseAttack - this.CurrentProtection;
                    this.CurrentProtection = 0;
                    this.CurrentHealth -= temp;
                }
            }
            else
                this.CurrentHealth -= player.BaseAttack;
        }

    }
    // Враги люди
    class HumanEnemy : Enemy
    {
        public HumanEnemy(string name, int maxhealth, int protection, int baseAttack, int damageModifier) : base(name, maxhealth, protection, baseAttack, damageModifier) { }

        public override Fight Attack()
        {
            Random rnd = new Random();
            if (rnd.Next(1, 10) % 2 == 0)
                return new WeaponAttack(this);
            else 
                return new ArmAttack(this);
        }
    }
    // Враги животные
    class AnimalEnemy : Enemy
    {
        public AnimalEnemy(string name, int maxhealth, int protection, int baseAttack, int damageModifier) : base(name, maxhealth, protection, baseAttack, damageModifier) { }

        public override Fight Attack()
        {
            return new ArmAttack(this);
        }
    }

    abstract class Fight
    { }

    // Атака оружием
    class WeaponAttack : Fight 
    {
        public WeaponAttack(Enemy enemy)
        {
            Random rnd = new Random();
            enemy.CurrentDamage += rnd.Next(-1, 1);
        }
    }
    // Атака голыми руками
    class ArmAttack : Fight
    {
        public ArmAttack(Enemy enemy)
        {
            Random rnd = new Random();
            enemy.CurrentDamage += rnd.Next(-1, 1);
        }
    }
}