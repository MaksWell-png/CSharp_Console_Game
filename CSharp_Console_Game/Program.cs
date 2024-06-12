using System;
using System.Numerics;
using System.Text;

namespace CSharp_Console_Game
{
    internal class Programm
    {
        static void Main()
        {
            // Отрисовка карты
            Console.CursorVisible = false;
            char[,] map =
            {
                { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
                { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', ' ', ' ', '#'},
                { '#', '#', '#', '#', '#', '#', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', ' ', ' ', '#'},
                { '#', ' ', ' ', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', ' ', ' ', '#'},
                { '#', ' ', ' ', '#', '#', '#', ' ', ' ', ' ', ' ', ' ', '#', '#', ' ', ' ', ' ', '#'},
                { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', ' ', '#', ' ', '#'},
                { '#', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', ' ', '#', ' ', '#'},
                { '#', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', ' ', ' ', '#', ' ', '#'},
                { '#', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', '#'},
                { '#', ' ', ' ', '#', '#', '#', ' ', ' ', '#', ' ', '#', '#', ' ', ' ', ' ', ' ', '#'},
                { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', ' ', '#', '#', '#', ' ', ' ', '#'},
                { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
                { '#', ' ', '#', '#', ' ', ' ', ' ', ' ', '#', '#', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
                { '#', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
                { '#', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
                { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
            };
            char[,] loose =
            {
                {'-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-','Y','o','u','-','l','o','o','s','e','!','-'},
                {'-','-','-','-','-','-','-','-','-','-','-','-'},

            };
            char[,] win =
            {
                {'-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-','Y','o','u','-','w','i','n','!','!','!','-'},
                {'-','-','-','-','-','-','-','-','-','-','-','-'},
            };

            // Объявление игрока и противников и настройка расположений противников
            Player player = new Player();
            Enemy[] enemyList = new Enemy[3];
            InitPlayerAndEnemy(ref player, ref enemyList, ref map);
            int playerX = 6, playerY = 6;
            Enemy thisEnemy = null;

            while (true)
            {
                DrawBarPlayer(player);
                if (map[playerX, playerY] == 'E')
                {
                    for (int i = 0; i < enemyList.Length; i++)
                    {
                        if (enemyList[i].CurrentPos.X == playerX && enemyList[i].CurrentPos.Y == playerY)
                        {
                            DrawBarEnemy(enemyList[i]);
                            thisEnemy = enemyList[i];
                        }
                    }
                }
                else 
                    thisEnemy = null;

                Console.SetCursorPosition(19, 0);
                Console.Write("F1 - Exit the game");

                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        Console.Write(map[i, j]);
                    }
                    Console.WriteLine();
                }
                Console.SetCursorPosition(playerY, playerX);
                Console.Write('P');


                // Обработка проигрыша
                if (player.CurrentHealth <= 0)
                {
                    Console.SetCursorPosition(0, 18);
                    for (int i = 0; i < loose.GetLength(0); i++)
                    {
                        for (int j = 0; j < loose.GetLength(1); j++)
                        {
                            Console.Write(loose[i, j]);
                        }
                        Console.WriteLine();
                    }
                }

                // Обарботка выигрыша
                if (enemyList[0].CurrentHealth <= 0 && enemyList[1].CurrentHealth <= 0 && enemyList[2].CurrentHealth <= 0)
                {
                    Console.SetCursorPosition(0, 18);
                    for (int i = 0; i < win.GetLength(0); i++)
                    {
                        for (int j = 0; j < win.GetLength(1); j++)
                        {
                            Console.Write(win[i, j]);
                        }
                        Console.WriteLine();
                    }
                }

                ConsoleKeyInfo charKey = Console.ReadKey();
                switch (charKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (map[playerX - 1, playerY] != '#')
                            playerX--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (map[playerX + 1, playerY] != '#')
                            playerX++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (map[playerX, playerY - 1] != '#')
                            playerY--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (map[playerX, playerY + 1] != '#')
                            playerY++;
                        break;
                    case ConsoleKey.F7:
                        if (thisEnemy != null)
                        {
                            player.Attack(player, thisEnemy);
                            if (thisEnemy.CurrentHealth <= 0)
                                map[playerX, playerY] = 'X';
                        }    
                        break;
                    case ConsoleKey.F1:
                        Environment.Exit(0);
                        break;
                }
                
                Console.Clear();
            }
        }

        // Инициализация игрока и списка противников
        private static void InitPlayerAndEnemy(ref Player player, ref Enemy[] enemyList, ref char[,] map)
        {
            Random rnd = new Random();
            // Инициализация игрока и противников
            player.SetPlayer("Player1", rnd.Next(8, 12), rnd.Next(4, 6), 6);
            for (int i = 0; i < enemyList.Length; i++)
            {
                if (rnd.Next(1, 10) % 2 == 0)
                {
                    enemyList[i] = new HumanEnemy($"Bandit_{i}", rnd.Next(5, 8), rnd.Next(2, 5), rnd.Next(2, 4), rnd.Next(-1, 1));
                    enemyList[i].Attack();
                }
                else
                {
                    enemyList[i] = new AnimalEnemy($"Animal_{i}", rnd.Next(5, 8), 0, rnd.Next(3, 5), rnd.Next(-1, 1));
                    enemyList[i].Attack();
                }
            }
            // Расстановка противников
            int enemyX, enemyY;
            for (int i = 0; i < enemyList.Length; i++)
            {
                while (true)
                {
                    enemyX = rnd.Next(map.GetLength(0)); enemyY = rnd.Next(map.GetLength(1));
                    if (map[enemyX, enemyY] != '#' && map[enemyX, enemyY] != 'E')
                    {
                        enemyList[i].CurrentPos = new Point(enemyX, enemyY);
                        map[enemyX, enemyY] = 'E';
                        break;
                    }
                }
            }
        }

        // Вывод на эксран показателей здоровья и защиты игрока
        private static void DrawBarPlayer(Player player)
        {
            Console.SetCursorPosition(21, 2);
            Console.Write("Player stats");

            ConsoleColor defaultColor = Console.BackgroundColor;
            StringBuilder bar = new StringBuilder("");
            // Health
            for (int i = 0; i < player.CurrentHealth; i++)
            {
                bar.Append(" ");
            }
            Console.SetCursorPosition(19, 4);
            Console.Write("HP:");
            Console.SetCursorPosition(22, 4);
            Console.Write('[');
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(bar.ToString());
            Console.BackgroundColor = defaultColor;
            bar.Remove(0, bar.Length);
            for (int i = 0; i < player.MaxHealth - player.CurrentHealth; i++)
            {
                bar.Append(" ");
            }
            Console.Write(bar.ToString() + ']');
            // Protectoion
            bar.Remove(0, bar.Length);
            for (int i = 0; i < player.CurrentProtection; i++)
            {
                bar.Append(" ");
            }
            Console.SetCursorPosition(19, 6);
            Console.Write("PT:");
            Console.SetCursorPosition(22, 6);
            Console.Write('[');
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(bar);
            Console.BackgroundColor = defaultColor;
            bar.Remove(0, bar.Length);
            for (int i = 0; i < player.MaxProtection - player.CurrentProtection; i++)
            {
                bar.Append(" ");
            }
            Console.Write(bar.ToString() + ']');
            // Damage
            Console.SetCursorPosition(19, 8);
            Console.Write("BaseAttack: " + player.BaseAttack);

        }
        // Вывод на эксран показателей здоровья и защиты противника
        private static void DrawBarEnemy(Enemy enemy)
        {
            Console.SetCursorPosition(41, 2);
            Console.Write($"{enemy.Name} stats");

            ConsoleColor defaultColor = Console.BackgroundColor;
            StringBuilder bar = new StringBuilder("");
            // Health
            for (int i = 0; i < enemy.CurrentHealth; i++)
            {
                bar.Append(" ");
            }
            Console.SetCursorPosition(39, 4);
            Console.Write("HP:");
            Console.SetCursorPosition(42, 4);
            Console.Write('[');
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(bar.ToString());
            Console.BackgroundColor = defaultColor;
            bar.Remove(0, bar.Length);
            for (int i = 0; i < enemy.MaxHealth - enemy.CurrentHealth; i++)
            {
                bar.Append(" ");
            }
            Console.Write(bar.ToString() + ']');
            // Protectoion
            bar.Remove(0, bar.Length);
            for (int i = 0; i < enemy.CurrentProtection; i++)
            {
                bar.Append(" ");
            }
            Console.SetCursorPosition(39, 6);
            Console.Write("PT:");
            Console.SetCursorPosition(42, 6);
            Console.Write('[');
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(bar);
            Console.BackgroundColor = defaultColor;
            bar.Remove(0, bar.Length);
            for (int i = 0; i < enemy.MaxProtection - enemy.CurrentProtection; i++)
            {
                bar.Append(" ");
            }
            Console.Write(bar.ToString() + ']');
            // Attack
            Console.SetCursorPosition(42, 8);
            Console.Write("F7 - Attack");
        }
    }
}