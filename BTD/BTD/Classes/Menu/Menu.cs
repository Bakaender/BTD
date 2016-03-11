using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BTD
{
    class Menu
    {
        private Texture2D menuBackground;
        private Texture2D menuButton;
        private SpriteFont font;
        private Rectangle menuArea;
        private List<Button> menuButtons = new List<Button>();
        public bool isActive = true;

        public Rectangle MenuArea { get { return menuArea; } }

        public Menu(Texture2D menuBackground, Texture2D menuButton, SpriteFont font)
        {
            this.menuBackground = menuBackground;
            this.menuButton = menuButton;
            this.font = font;
        }

        public void CreateButton(string label, string description)
        {
            Button button = new Button(label, description);
            menuButtons.Add(button);
        }

        public void UpdatePositions(Rectangle menuArea)
        {
            this.menuArea = menuArea;
            for (int i = 0; i < menuButtons.Count; i++)
            {
                menuButtons[i].Position = new Rectangle(menuArea.X + (menuArea.Width - menuButton.Width) / 2, menuArea.Y + menuButton.Height * i + menuButton.Height / 2 + (menuButton.Height / 2) * i, menuButton.Width, menuButton.Height);
            }
            int height = menuButtons.Count % 2;
            if (height == 0)
                this.menuArea.Height = ((menuButtons.Count + 1 + menuButtons.Count / 2) * menuButton.Height) - menuButton.Height / 2;
            else
                this.menuArea.Height = (menuButtons.Count + 1 + menuButtons.Count / 2) * menuButton.Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(menuBackground, menuArea, null, Color.Blue, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);

            foreach (Button button in menuButtons)
            {
                spriteBatch.Draw(menuButton, button.Position, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                Vector2 stringSize = font.MeasureString(button.Label);
                spriteBatch.DrawString(font, button.Label.ToString(), new Vector2(button.Position.X + (menuButton.Width - stringSize.X) / 2, button.Position.Y + (menuButton.Height - stringSize.Y) / 2), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        public Button CheckClick(MouseState oldMouseState, MouseState currentMouseState, Vector2 mouseWorldPosition)
        {
            foreach (Button button in menuButtons)
            {
                if (button.Position.Contains(new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
                {
                    if (oldMouseState.LeftButton != ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        return button;
                    }
                }
            }

            return null;
        }

        public string CheckHover(MouseState oldMouseState, MouseState currentMouseState, Vector2 mouseWorldPosition)
        {
            foreach (Button button in menuButtons)
            {
                if (button.Position.Contains(new Point((int)mouseWorldPosition.X, (int)mouseWorldPosition.Y)))
                {
                    return button.Description;
                }
            }

            return "";
        }
    }
}
