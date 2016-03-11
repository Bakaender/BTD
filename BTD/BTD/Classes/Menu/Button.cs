using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BTD
{
    class Button
    {
        private Rectangle position;
        private string label;
        private string description;

        public Rectangle Position 
        {
            get { return this.position; }
            set { this.position = value; } 
        }

        public string Label
        {
            get { return this.label; }
            set { this.label = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public Button(string label, string description)
        {
            this.label = label;
            this.description = description;
        }
    }
}
