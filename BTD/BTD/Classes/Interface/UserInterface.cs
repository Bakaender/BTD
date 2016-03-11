using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BTD
{
    class UserInterface
    {
        public int windowWidth;
        public int windowHeight;
        public float interfaceVerticalScale;
        public float interfaceHorizontalScale;
        public int mapWidth;
        public int mapHeight;
        public float overlayVerticalScale;
        public float overlayHorizontalScale;

        public InterfaceItem miniMap;
        public InterfaceItem miniMapOverlay;
        public InterfaceItem miniMapBorder;
        public InterfaceItem commandsBackground;
        public InterfaceItem commandsBorder;
        public InterfaceItem buildCommand;
        //public InterfaceItem sellCommand;
        public InterfaceItem cancelCommand;
        public InterfaceItem informationBorder;
        public InterfaceItem heroThumb;

        static public List<InterfaceItem> buildTowerThumbs = new List<InterfaceItem>();

        static public List<InterfaceItem> interfaceItems = new List<InterfaceItem>();

        public UserInterface(int windowWidth, int windowHeight, float interfaceVerticalScale, float interfaceHorizontalScale, int mapWidth, int mapHeight)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.interfaceVerticalScale = interfaceVerticalScale;
            this.interfaceHorizontalScale = interfaceHorizontalScale;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
        }

        public void CreateInterfaceItems(Texture2D miniMapTexture, Texture2D miniMapOverlayTexture, Texture2D miniMapBorderTexture, Texture2D commandsBorderTexture, Texture2D commandsBackground, Texture2D informationBorderTexture, Texture2D buildSellButtons)
        {
            this.miniMap = new InterfaceItem(miniMapTexture, 0.5f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(0, 0, miniMapTexture.Width, miniMapTexture.Height));
            interfaceItems.Add(this.miniMap);

            this.CalculateOverlayScale();
            this.miniMapOverlay = new InterfaceItem(miniMapOverlayTexture, 0.4f, overlayHorizontalScale, overlayVerticalScale, new Rectangle(0, 0, miniMapOverlayTexture.Width, miniMapOverlayTexture.Height));
            interfaceItems.Add(this.miniMapOverlay);

            this.miniMapBorder = new InterfaceItem(miniMapBorderTexture, 0.5f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(0, 0, miniMapBorderTexture.Width, miniMapBorderTexture.Height));
            interfaceItems.Add(this.miniMapBorder);

            this.commandsBorder = new InterfaceItem(commandsBorderTexture, 0.5f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(0, 0, commandsBorderTexture.Width, commandsBorderTexture.Height));
            interfaceItems.Add(this.commandsBorder);

            this.commandsBackground = new InterfaceItem(commandsBackground, 0.5f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(0, 0, commandsBackground.Width, commandsBackground.Height));
            interfaceItems.Add(this.commandsBackground);

            this.informationBorder = new InterfaceItem(informationBorderTexture, 0.5f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(0, 0, informationBorderTexture.Width, informationBorderTexture.Height));
            interfaceItems.Add(this.informationBorder);

            this.buildCommand = new InterfaceItem(buildSellButtons, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(0, 0, 60, 60));
            interfaceItems.Add(this.buildCommand);

            //this.sellCommand = new InterfaceItem(buildSellButtons, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(60, 0, 60, 60));

            this.cancelCommand = new InterfaceItem(buildSellButtons, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(120, 0, 60, 60));
        }

        public void CreateHeroThumb(Texture2D heroThumbs, Texture2D towerThumbs, Hero hero, Rectangle heroSource)
        {
            switch (hero)
            {
                case Hero.Arthus:
                    if (heroThumb == null)
                    {
                        heroThumb = new InterfaceItem(heroThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, heroSource);
                        Tower.GetTowerList(hero);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        buildTowerThumbs.Add(new InterfaceItem(towerThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(60 * i, 0, 60, 60)));
                    }
                    break;

                case Hero.Jaina:
                    if (heroThumb == null)
                    {
                        heroThumb = new InterfaceItem(heroThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, heroSource);
                        Tower.GetTowerList(hero);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        buildTowerThumbs.Add(new InterfaceItem(towerThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(60 * i, 60, 60, 60)));
                    }
                    break;

                case Hero.Pyrus:
                    if (heroThumb == null)
                    {
                        heroThumb = new InterfaceItem(heroThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, heroSource);
                        Tower.GetTowerList(hero);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        buildTowerThumbs.Add(new InterfaceItem(towerThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(60 * i, 120, 60, 60)));
                    }
                    break;

                case Hero.Coldreaver:
                    if (heroThumb == null)
                    {
                        heroThumb = new InterfaceItem(heroThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, heroSource);
                        Tower.GetTowerList(hero);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        buildTowerThumbs.Add(new InterfaceItem(towerThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(60 * i, 180, 60, 60)));
                    }
                    break;

                case Hero.Thrall:
                    if (heroThumb == null)
                    {
                        heroThumb = new InterfaceItem(heroThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, heroSource);
                        Tower.GetTowerList(hero);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        buildTowerThumbs.Add(new InterfaceItem(towerThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(60 * i, 240, 60, 60)));
                    }
                    break;

                case Hero.Arachne:
                    if (heroThumb == null)
                    {
                        heroThumb = new InterfaceItem(heroThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, heroSource);
                        Tower.GetTowerList(hero);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        buildTowerThumbs.Add(new InterfaceItem(towerThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(60 * i, 300, 60, 60)));
                    }
                    break;

                case Hero.Tichondrius:
                    if (heroThumb == null)
                    {
                        heroThumb = new InterfaceItem(heroThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, heroSource);
                        Tower.GetTowerList(hero);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        buildTowerThumbs.Add(new InterfaceItem(towerThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(60 * i, 360, 60, 60)));
                    }
                    break;

                case Hero.Joranda:
                    if (heroThumb == null)
                    {
                        heroThumb = new InterfaceItem(heroThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, heroSource);
                        Tower.GetTowerList(hero);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        buildTowerThumbs.Add(new InterfaceItem(towerThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(60 * i, 420, 60, 60)));
                    }
                    break;

                case Hero.Furion:
                    if (heroThumb == null)
                    {
                        heroThumb = new InterfaceItem(heroThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, heroSource);
                        Tower.GetTowerList(hero);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        buildTowerThumbs.Add(new InterfaceItem(towerThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(60 * i, 480, 60, 60)));
                    }
                    break;

                case Hero.Tyrande:
                    if (heroThumb == null)
                    {
                        heroThumb = new InterfaceItem(heroThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, heroSource);
                        Tower.GetTowerList(hero);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        buildTowerThumbs.Add(new InterfaceItem(towerThumbs, 0.4f, interfaceHorizontalScale, interfaceVerticalScale, new Rectangle(60 * i, 540, 60, 60)));
                    }
                    break;
            }
            interfaceItems.Add(heroThumb);            
        }

        public void CalculateOverlayScale()
        {
            this.overlayHorizontalScale = (float)windowWidth / mapWidth * (miniMap.texture.Width * interfaceHorizontalScale);
            this.overlayVerticalScale = (float)windowHeight / mapHeight * (miniMap.texture.Height * interfaceVerticalScale);
        }

        public void UpdatePositions(Vector2 cameraPos)
        {
            miniMap.Position = new Vector2(cameraPos.X - windowWidth / 2, (cameraPos.Y + windowHeight / 2) - (miniMap.texture.Height * interfaceVerticalScale));
            miniMapBorder.Position = new Vector2(cameraPos.X - windowWidth / 2, (cameraPos.Y + windowHeight / 2) - (miniMapBorder.texture.Height * interfaceVerticalScale));
            miniMapOverlay.Position = new Vector2(miniMap.Position.X + ((cameraPos.X - windowWidth / 2) / ((mapWidth - windowWidth) / (miniMap.texture.Width * interfaceHorizontalScale - overlayHorizontalScale))),
                                    miniMap.Position.Y + (cameraPos.Y - windowHeight / 2) / ((mapHeight - windowHeight) / (miniMap.texture.Height * interfaceVerticalScale - overlayVerticalScale)));
            commandsBorder.Position = new Vector2(cameraPos.X + windowWidth / 2 - commandsBorder.texture.Width * interfaceHorizontalScale, cameraPos.Y + windowHeight / 2 - commandsBorder.texture.Height * interfaceVerticalScale);
            commandsBackground.Position = new Vector2(cameraPos.X + windowWidth / 2 - commandsBackground.texture.Width * interfaceHorizontalScale, cameraPos.Y + windowHeight / 2 - commandsBackground.texture.Height * interfaceVerticalScale);
            informationBorder.Position = new Vector2(cameraPos.X - (informationBorder.texture.Width * interfaceHorizontalScale) / 2, (cameraPos.Y + windowHeight / 2) - (informationBorder.texture.Height * interfaceVerticalScale));
            buildCommand.Position = new Vector2(commandsBackground.Position.X + 2 * interfaceHorizontalScale, commandsBackground.Position.Y + 2 * interfaceVerticalScale);
            //sellCommand.Position = new Vector2(commandsBackground.Position.X + 64 * interfaceHorizontalScale, commandsBackground.Position.Y + 2 * interfaceVerticalScale);
            cancelCommand.Position = new Vector2(commandsBackground.Position.X + 188 * interfaceHorizontalScale, commandsBackground.Position.Y + 188 * interfaceVerticalScale);

            if (heroThumb != null)
                heroThumb.Position = new Vector2(informationBorder.Position.X + 40 * interfaceHorizontalScale, informationBorder.Position.Y + 40 * interfaceVerticalScale);

            if (Game1.building)
            {
                for (int i = 0; i < buildTowerThumbs.Count; i++)
                {
                    // TODO Need to change to take into account multiple builders.
                    if (i < 4)
                        buildTowerThumbs[i].Position = new Vector2(commandsBackground.Position.X + (60 * i + (2 * (i + 1))) * interfaceHorizontalScale, commandsBackground.Position.Y + 2 * interfaceVerticalScale);
                    else if (i > 3 && i < 8)
                        buildTowerThumbs[i].Position = new Vector2(commandsBackground.Position.X + (60 * (i - 4) + (2 * (i - 4 + 1))) * interfaceHorizontalScale, commandsBackground.Position.Y + 64 * interfaceVerticalScale);
                    else if (i > 7 && i < 10)
                        buildTowerThumbs[i].Position = new Vector2(commandsBackground.Position.X + (60 * (i - 8) + (2 * (i - 8 + 1))) * interfaceHorizontalScale, commandsBackground.Position.Y + 126 * interfaceVerticalScale);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (InterfaceItem item in interfaceItems)
            {
                item.Draw(spriteBatch);
            }

            if (Game1.building)
            {
                foreach (InterfaceItem item in buildTowerThumbs)
                {
                    item.Draw(spriteBatch);
                }
            }
        } 
    }
}
