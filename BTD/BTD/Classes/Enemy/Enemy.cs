using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BTD
{
    enum Direction
    {
        Right,
        Down,
        Up,
        Left 
    }

    class Enemy
    {
        static List<int> moveDistances;
        static Vector2 cornerOneSpawnLocation;
        static List<Direction> cornerOneDirections;
        static Vector2 cornerTwoSpawnLocation;
        static List<Direction> cornerTwoDirections;
        static Vector2 cornerThreeSpawnLocation;
        static List<Direction> cornerThreeDirections;
        static Vector2 cornerFourSpawnLocation;
        static List<Direction> cornerFourDirections;

        public static List<Enemy> enemyList;

        public Texture2D texture;
        public Vector2 position;
        public int velocity;
        public static float drawDepth = 0.6f; //Need to change from static if I start drawing enemies on different layers.
        public Direction direction;
        public int distanceTraveled;

        public int spawnCorner;
        public int waypointIndex;

        public float animationTime;
        public int animationStep;
        
        private int health;

        public int Health
        {
            get { return this.health; }
            set { this.health = value; }
        }

        private int goldValue;

        public int GoldValue
        {
            get { return goldValue; }
            set { goldValue = value; }
        }

        public Vector2 Origin { get { return new Vector2(this.position.X + 16, this.position.Y + 16); } }

        static Enemy()
        {
            moveDistances = new List<int>();
            cornerOneDirections = new List<Direction>();
            cornerTwoDirections = new List<Direction>();
            cornerThreeDirections = new List<Direction>();
            cornerFourDirections = new List<Direction>();

            //Eventually read this in from a file, or from the map file.
            moveDistances.Add(146 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerOneDirections.Add(Direction.Right);
            cornerTwoDirections.Add(Direction.Down);
            cornerThreeDirections.Add(Direction.Left);
            cornerFourDirections.Add(Direction.Up);
            moveDistances.Add(20 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerOneDirections.Add(Direction.Up);
            cornerTwoDirections.Add(Direction.Right);
            cornerThreeDirections.Add(Direction.Down);
            cornerFourDirections.Add(Direction.Left);
            moveDistances.Add(20 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerOneDirections.Add(Direction.Left);
            cornerTwoDirections.Add(Direction.Up);
            cornerThreeDirections.Add(Direction.Right);
            cornerFourDirections.Add(Direction.Down);
            moveDistances.Add(132 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerOneDirections.Add(Direction.Down);
            cornerTwoDirections.Add(Direction.Left);
            cornerThreeDirections.Add(Direction.Up);
            cornerFourDirections.Add(Direction.Right);
            moveDistances.Add(20 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerOneDirections.Add(Direction.Right);
            cornerTwoDirections.Add(Direction.Down);
            cornerThreeDirections.Add(Direction.Left);
            cornerFourDirections.Add(Direction.Up);
            moveDistances.Add(20 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerOneDirections.Add(Direction.Up);
            cornerTwoDirections.Add(Direction.Right);
            cornerThreeDirections.Add(Direction.Down);
            cornerFourDirections.Add(Direction.Left);
            moveDistances.Add(132 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerOneDirections.Add(Direction.Left);
            cornerTwoDirections.Add(Direction.Up);
            cornerThreeDirections.Add(Direction.Right);
            cornerFourDirections.Add(Direction.Down);
            moveDistances.Add(20 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerOneDirections.Add(Direction.Down);
            cornerTwoDirections.Add(Direction.Left);
            cornerThreeDirections.Add(Direction.Up);
            cornerFourDirections.Add(Direction.Right);
            moveDistances.Add(20 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerOneDirections.Add(Direction.Right);
            cornerTwoDirections.Add(Direction.Down);
            cornerThreeDirections.Add(Direction.Left);
            cornerFourDirections.Add(Direction.Up);
            moveDistances.Add(86 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerOneDirections.Add(Direction.Up);
            cornerTwoDirections.Add(Direction.Right);
            cornerThreeDirections.Add(Direction.Down);
            cornerFourDirections.Add(Direction.Left);
            moveDistances.Add(46 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerOneDirections.Add(Direction.Right);
            cornerTwoDirections.Add(Direction.Down);
            cornerThreeDirections.Add(Direction.Left);
            cornerFourDirections.Add(Direction.Up);
            moveDistances.Add(20 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerOneDirections.Add(Direction.Down);
            cornerTwoDirections.Add(Direction.Left);
            cornerThreeDirections.Add(Direction.Up);
            cornerFourDirections.Add(Direction.Right);
  
            cornerOneSpawnLocation = new Vector2(0, 34 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerTwoSpawnLocation = new Vector2(126 * Game1.WORLD_TEXTURE_DIMENSIONS, 0);
            cornerThreeSpawnLocation = new Vector2(160 * Game1.WORLD_TEXTURE_DIMENSIONS, 126 * Game1.WORLD_TEXTURE_DIMENSIONS);
            cornerFourSpawnLocation = new Vector2(34 * Game1.WORLD_TEXTURE_DIMENSIONS, 160 * Game1.WORLD_TEXTURE_DIMENSIONS);

            enemyList = new List<Enemy>();
        }

        public Enemy(Texture2D texture, int velocity, int spawnCorner, int health, int goldValue)
        {
            Random spawner = new Random();

            this.texture = texture;
            this.velocity = velocity;
            this.spawnCorner = spawnCorner;
            this.waypointIndex = 0;
            this.distanceTraveled = 0;
            this.health = health;
            this.animationStep = 0;
            this.animationTime = 0f;
            this.goldValue = goldValue;

            if (this.spawnCorner == 0)
            {
                this.direction = cornerOneDirections[waypointIndex];
                this.position = new Vector2(cornerOneSpawnLocation.X + spawner.Next(0, 5 * Game1.WORLD_TEXTURE_DIMENSIONS), cornerOneSpawnLocation.Y + spawner.Next(0, 5 * Game1.WORLD_TEXTURE_DIMENSIONS));
            }
            else if (this.spawnCorner == 1)
            {
                this.direction = cornerTwoDirections[waypointIndex];
                this.position = new Vector2(cornerTwoSpawnLocation.X + spawner.Next(0, 5 * Game1.WORLD_TEXTURE_DIMENSIONS), cornerTwoSpawnLocation.Y + spawner.Next(0, 5 * Game1.WORLD_TEXTURE_DIMENSIONS));
            }
            else if (this.spawnCorner == 2)
            {
                this.direction = cornerThreeDirections[waypointIndex];
                this.position = new Vector2(cornerThreeSpawnLocation.X + spawner.Next(0, 5 * Game1.WORLD_TEXTURE_DIMENSIONS), cornerThreeSpawnLocation.Y + spawner.Next(0, 5 * Game1.WORLD_TEXTURE_DIMENSIONS));
            }
            else if (this.spawnCorner == 3)
            {
                this.direction = cornerFourDirections[waypointIndex];
                this.position = new Vector2(cornerFourSpawnLocation.X + spawner.Next(0, 5 * Game1.WORLD_TEXTURE_DIMENSIONS), cornerFourSpawnLocation.Y + spawner.Next(0, 5 * Game1.WORLD_TEXTURE_DIMENSIONS));
            }

            Enemy.enemyList.Add(this);
        }

        static public void UpdateEnemies()
        {
            for (int i = enemyList.Count() - 1; i >= 0; i--)
            {
                if (enemyList[i].distanceTraveled < moveDistances[enemyList[i].waypointIndex])
                {
                    switch (enemyList[i].direction)
                    {
                        case Direction.Right:
                            if (enemyList[i].distanceTraveled + enemyList[i].velocity > moveDistances[enemyList[i].waypointIndex])
                            {
                                enemyList[i].position.X += moveDistances[enemyList[i].waypointIndex] - enemyList[i].distanceTraveled;
                                enemyList[i].distanceTraveled += moveDistances[enemyList[i].waypointIndex] - enemyList[i].distanceTraveled;
                            }
                            else
                            {
                                enemyList[i].position.X += enemyList[i].velocity;
                                enemyList[i].distanceTraveled += enemyList[i].velocity;
                            }
                            break;

                        case Direction.Down:
                            if (enemyList[i].distanceTraveled + enemyList[i].velocity > moveDistances[enemyList[i].waypointIndex])
                            {
                                enemyList[i].position.Y += moveDistances[enemyList[i].waypointIndex] - enemyList[i].distanceTraveled;
                                enemyList[i].distanceTraveled += moveDistances[enemyList[i].waypointIndex] - enemyList[i].distanceTraveled;
                            }
                            else
                            {
                                enemyList[i].position.Y += enemyList[i].velocity;
                                enemyList[i].distanceTraveled += enemyList[i].velocity;
                            }
                            break;

                        case Direction.Up:
                            if (enemyList[i].distanceTraveled + enemyList[i].velocity > moveDistances[enemyList[i].waypointIndex])
                            {
                                enemyList[i].position.Y -= moveDistances[enemyList[i].waypointIndex] - enemyList[i].distanceTraveled;
                                enemyList[i].distanceTraveled += moveDistances[enemyList[i].waypointIndex] - enemyList[i].distanceTraveled;
                            }
                            else
                            {
                                enemyList[i].position.Y -= enemyList[i].velocity;
                                enemyList[i].distanceTraveled += enemyList[i].velocity;
                            }
                            break;

                        case Direction.Left:
                            if (enemyList[i].distanceTraveled + enemyList[i].velocity > moveDistances[enemyList[i].waypointIndex])
                            {
                                enemyList[i].position.X -= moveDistances[enemyList[i].waypointIndex] - enemyList[i].distanceTraveled;
                                enemyList[i].distanceTraveled += moveDistances[enemyList[i].waypointIndex] - enemyList[i].distanceTraveled;
                            }
                            else
                            {
                                enemyList[i].position.X -= enemyList[i].velocity;
                                enemyList[i].distanceTraveled += enemyList[i].velocity;
                            }
                            break;
                    }  
                }
                else
                {
                    enemyList[i].waypointIndex++;
                    if (enemyList[i].waypointIndex < moveDistances.Count())
                    {
                        enemyList[i].distanceTraveled = 0;
                        if (enemyList[i].spawnCorner == 0)
                            enemyList[i].direction = cornerOneDirections[enemyList[i].waypointIndex];
                        else if (enemyList[i].spawnCorner == 1)
                        {
                            enemyList[i].direction = cornerTwoDirections[enemyList[i].waypointIndex];
                        }
                        else if (enemyList[i].spawnCorner == 2)
                        {
                            enemyList[i].direction = cornerThreeDirections[enemyList[i].waypointIndex];
                        }
                        else if (enemyList[i].spawnCorner == 3)
                        {
                            enemyList[i].direction = cornerFourDirections[enemyList[i].waypointIndex];
                        }
                    }
                    else
                    {
                        Variables.numberLives--;
                        enemyList.Remove(enemyList[i]);
                    }
                }
            }
        }

        public void UpdateEnemyHealth(Enemy enemy, int damage)
        {
            enemy.health -= damage;
            if (enemy.health <= 0)
            {
                Variables.gold += enemy.goldValue;
                enemy.goldValue = 0;
                Enemy.enemyList.Remove(enemy);
            }
        }

        static public void UpdateEnemyAnimation(GameTime gameTime)
        {
            foreach (Enemy enemy in enemyList)
            {
                if (enemy.animationTime < 64)
                    enemy.animationTime += gameTime.ElapsedGameTime.Milliseconds;
                else
                {
                    if (enemy.animationStep < 6)
                        enemy.animationStep++;
                    else
                        enemy.animationStep = 0;
                    enemy.animationTime = 0f;
                }
            }
        }

        static public void DrawEnemies(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in Enemy.enemyList)
            {
                switch (enemy.direction)
                {
                    case Direction.Right:
                        spriteBatch.Draw(enemy.texture, enemy.position, new Rectangle(0, 32 * enemy.animationStep, 32, 32), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, Enemy.drawDepth);
                        break;
                    case Direction.Down:
                        spriteBatch.Draw(enemy.texture, enemy.position, new Rectangle(0, 32 * enemy.animationStep, 32, 32), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.FlipHorizontally, Enemy.drawDepth);
                        break;
                    case Direction.Up:
                        spriteBatch.Draw(enemy.texture, enemy.position, new Rectangle(0, 32 * enemy.animationStep, 32, 32), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, Enemy.drawDepth);
                        break;
                    case Direction.Left:
                        spriteBatch.Draw(enemy.texture, enemy.position, new Rectangle(0, 32 * enemy.animationStep, 32, 32), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.FlipHorizontally, Enemy.drawDepth);
                        break;
                }
                //spriteBatch.Draw(enemy.texture, enemy.position, null, Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, Enemy.drawDepth);
            }
        }
    }
}
