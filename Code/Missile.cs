using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace xnaDemo
{
    class Missile
    {
        //Constructor that feeds instance it's info (if default's not chosen).
        public Missile() { }
        public Missile(System.Drawing.Point atLocation, Texture2D image)
        {
            this.Position = atLocation;
            this.Image = image;
        }

        //Bomb's position.
        public Point Position;

        //Bomb's image.
        public Texture2D Image;
    }
}
