using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BTD
{
    class Projectiles
    {
        public static List<Projectiles> ProjectilesList;

        public Texture2D texture;
        public Enemy targetEnemy;
        public float speed;
        public Vector2 position;
        public int damage;
        private float rotation;

        public Vector2 Origin { get { return new Vector2(this.position.X + this.texture.Width / 2, this.position.Y + this.texture.Height / 2); } }

        static Projectiles()
        {
            ProjectilesList = new List<Projectiles>();
        }

        public Projectiles(Texture2D texture, float speed, Enemy targetEnemy, Vector2 position, int damage)
        {
            this.texture = texture;
            this.speed = speed;
            this.targetEnemy = targetEnemy;
            this.position = position;
            this.damage = damage;
            this.rotation = 0f;
            ProjectilesList.Add(this);
        }

        public static void UpdateProjectiles()
        {
            for (int i = ProjectilesList.Count - 1; i >= 0; i--)
            {
                ProjectilesList[i].UpdateProjectile();
            }
        }

        public void UpdateProjectile()
        {
            rotation = 0f;

            float a = targetEnemy.Origin.X - position.X;
            float b = targetEnemy.Origin.Y - position.Y;
            float distance = (float)Math.Sqrt(a * a + b * b);

            float percentMove = speed / distance;
            position.X += a * percentMove;
            position.Y += b * percentMove;

            // Can't figure out so it doesn't curve.
            rotation = ((float)Math.Atan2(a, -b)) * (float)(180 / Math.PI);

            if (distance <= speed)
            {
                targetEnemy.UpdateEnemyHealth(this.targetEnemy, this.damage);
                ProjectilesList.Remove(this);
            }
        }

        public static void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectiles projectile in ProjectilesList)
            {
                spriteBatch.Draw(projectile.texture, projectile.position, null, Color.White, MathHelper.ToRadians(projectile.rotation), new Vector2(projectile.texture.Width / 2, projectile.texture.Height / 2), 1f, SpriteEffects.None, 0.59f);
            }
        }
    }
}
