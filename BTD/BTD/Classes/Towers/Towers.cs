using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BTD
{
    class Towers
    {
        private Rectangle sourceTexture;
        private Vector2 position;
        private float drawDepth;
        private Texture2D projectileTexture;
        private float range;
        private float attackSpeed;
        private float projectileSpeed;
        private float timeSinceLastShot;
        private int damage;

        public Rectangle SourceTexture { get { return this.sourceTexture;} set { this.sourceTexture = value;} }
        public Vector2 Position { get { return this.position; } set { this.position = value; } }
        public float DrawDepth { get { return this.drawDepth;} set { this.drawDepth = value;} }
        public float Range { get { return this.range; } set { this.range = value; } }
        public float AttackSpeed { get { return this.attackSpeed; } set { this.attackSpeed = value; } }
        public float TimeSinceLastShot { get { return this.timeSinceLastShot; } set { this.timeSinceLastShot = value; } }
        public Vector2 Origin { get { return new Vector2(this.position.X + this.sourceTexture.Width / 2, this.position.Y + this.sourceTexture.Height / 2); } }

        public Towers(Rectangle sourceTexture, Vector2 position, float drawDepth, float range, float attackSpeed, float projectileSpeed, int damage, Texture2D projectileTexture)
        {
            this.sourceTexture = sourceTexture;
            this.position = position;
            this.drawDepth = drawDepth;
            this.range = range;
            this.attackSpeed = attackSpeed;
            this.projectileSpeed = projectileSpeed;
            this.damage = damage;
            this.projectileTexture = projectileTexture;
            this.timeSinceLastShot = attackSpeed;
        }

        public void Shoot(Enemy enemy)
        {
            if (timeSinceLastShot >= attackSpeed)
            {
                new Projectiles(projectileTexture, projectileSpeed, enemy, this.Origin, this.damage);
                timeSinceLastShot = 0f;
            }
        }

        public void DrawTower(SpriteBatch spriteBatch, Texture2D towerTextures)
        {
            spriteBatch.Draw(towerTextures, this.position, this.sourceTexture, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, this.drawDepth);
        }
    }
}
