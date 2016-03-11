using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BTD
{
    class InterfaceItem
    {
        public Texture2D texture;
        private float layerDepth;
        private Vector2 position;
        private float horizontalScale;
        private float verticalScale;
        private Rectangle sourceTexture;

        public Vector2 Position { get { return position; } set { position = value; } }
        public float HorizontalScale { set { horizontalScale = value; } }
        public float VerticalScale { set { verticalScale = value; } }
        public Rectangle SourceTexture { get { return sourceTexture; } set { sourceTexture = value; } }

        public InterfaceItem(Texture2D texture, float layerDepth, float horizontalScale, float verticalScale, Rectangle sourceTexture)
        {
            this.texture = texture;
            this.layerDepth = layerDepth;
            this.horizontalScale = horizontalScale;
            this.verticalScale = verticalScale;
            this.sourceTexture = sourceTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, sourceTexture, Color.White, 0f, Vector2.Zero, new Vector2(this.horizontalScale, this.verticalScale), SpriteEffects.None, this.layerDepth);
        } 
    }
}
