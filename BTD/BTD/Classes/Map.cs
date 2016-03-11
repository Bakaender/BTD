using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BTD
{

    class Map
    {
        private StreamReader reader;
        private int mapWidth = 0;
        private int mapHeight = 0;
        private int textureDimensions;

        public static List<string> map = new List<string>();

        public int MapWidth  
        {
            get { return this.mapWidth; }
        }

        public int MapHeight
        {
            get { return this.mapHeight; }
        }

        public Map(int textureDimensions)
        {
            CreateMap(textureDimensions);
        }

        public void CreateMap(int textureDimensions)
        {
            mapWidth = 0;
            mapHeight = 0;

            this.textureDimensions = textureDimensions;
            reader = new StreamReader("map.txt");
            this.mapWidth = reader.ReadLine().Length / 2 + 1;

            while (!reader.EndOfStream)
            {
                string[] temp = new string[mapWidth];
                string line = reader.ReadLine();
                temp = line.Split(',');
                
                int count = 0;
                while (count < mapWidth)
                {
                    map.Add(temp[count]);
                    count++;
                }

                mapHeight++;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D mapTexture)
        {
            for (int i = 0; i < mapHeight; i++)
            {
                int count = 0;
                while (count < mapWidth)
                {
                    if (map[i * mapWidth + count] == "G")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 0, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }     
                    else if (map[i * mapWidth + count] == "R")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 1, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    else if (map[i * mapWidth + count] == "W")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 6, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    else if (map[i * mapWidth + count] == "S")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 13, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    else if (map[i * mapWidth + count] == "B")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 2, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    else if (map[i * mapWidth + count] == "L")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 3, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    else if (map[i * mapWidth + count] == "H")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 4, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    else if (map[i * mapWidth + count] == "T")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 5, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    else if (map[i * mapWidth + count] == "M")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 13, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    else if (map[i * mapWidth + count] == "WTL")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 9, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }                    
                    else if (map[i * mapWidth + count] == "WTR")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 10, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    else if (map[i * mapWidth + count] == "WBL")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 11, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    else if (map[i * mapWidth + count] == "WBR")
                    {
                        spriteBatch.Draw(mapTexture, new Rectangle(count * textureDimensions, i * textureDimensions, textureDimensions, textureDimensions),
                                         new Rectangle(textureDimensions * 12, 0, textureDimensions, textureDimensions), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    count++;
                        
                }
            }
        }
    }
}
