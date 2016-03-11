using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BTD
{
    static class Tower
    {
        public static List<Towers> BuiltTowers = new List<Towers>();

        public static List<List<string>> Towers = new List<List<string>>();
        /* Sub List Indexes. Need to add stuff like damage, Armor etc.
         * 0 = Name
         * 1 = Gold Cost
         * 2 = Lumber Cost
         * 3 = Damage Type
         * 4 = Range
         * 5 = Description
         * 6 = Targetable Type (Land and/or Air)
         * 7 = Source texture X cordinate
         * 8 = Source texture Y cordinate
         * 9 = attack speed
         * 10 = projectile speed
         * 11 = damage
         */ 

        public static void GetTowerList(Hero hero)
        {
            StreamReader reader = new StreamReader(hero.ToString() + ".txt");

            while (!reader.EndOfStream)
            {
                string[] temp = new string[12];
                string line = reader.ReadLine();
                temp = line.Split(';');

                List<string> templist = new List<string>();
                int count2 = 0;
                while (count2 < 12)
                {
                    templist.Add(temp[count2]);
                    count2++;
                }

                Towers.Add(templist);
            }
        }

        public static int GetSelectedTowerGoldCost(int index)
        {
            return int.Parse(Towers[index][1]);
        }

        public static int GetSelectedTowerLumberCost(int index)
        {
            return int.Parse(Towers[index][2]);
        }

        public static void BuildTower(int towerIndex, Vector2 position, Texture2D projectileTexture)
        {
            BuiltTowers.Add(new Towers(new Rectangle(int.Parse(Towers[towerIndex][7]), int.Parse(Towers[towerIndex][8]), 64, 64), position, 0.8f, float.Parse(Towers[towerIndex][4]), float.Parse(Towers[towerIndex][9]), float.Parse(Towers[towerIndex][10]), int.Parse(Towers[towerIndex][11]) , projectileTexture));            
        }

        public static void CheckFire(GameTime gameTime)
        {
            foreach (Towers tower in BuiltTowers)
            {
                int enemyCount = 0;
                tower.TimeSinceLastShot += gameTime.ElapsedGameTime.Milliseconds;

                foreach (Enemy enemy in Enemy.enemyList)
                {
                    if (enemyCount == 0)
                    {
                        float a = enemy.Origin.X - tower.Origin.X;
                        float b = enemy.Origin.Y - tower.Origin.Y;
                        float distance = (float)Math.Sqrt(a * a + b * b);

                        if (distance <= tower.Range)
                        {
                            tower.Shoot(enemy);
                            enemyCount++;
                        }
                    }
                    else
                        break;
                }
            }
        }

        public static void DrawTowers(SpriteBatch spriteBatch, Texture2D towerTextures)
        {
            foreach (Towers tower in Tower.BuiltTowers)
            {
                tower.DrawTower(spriteBatch, towerTextures);
            }
        }
    }
}
