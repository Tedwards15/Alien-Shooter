using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace xnaDemo
{
    class Bomb
    {
        //If bom's level 1 or level 2.
        private int whatLevel;

        //Constructor that feeds instance it's info (if default's not chosen).
        public Bomb() { }
        public Bomb(int onLevel, System.Drawing.Point atLocation, Texture2D image)
        {
            this.Level = onLevel;
            this.Position = atLocation;
            this.Image = image;
        }

        public int Level
        {
            set
            {
                //If setting value's 1 or 2, then set property to that.
                if((value == 1) || (value == 2))
                {
                    whatLevel = value;
                }
                else
                {
                    //POSSIBLE PROBLEM
                    System.ArgumentOutOfRangeException wrongLevelNum = new System.ArgumentOutOfRangeException("Level",
                        value.ToString() + " is not a valid level of a bomb.  Only levels 1 and 2 are.");
                    throw wrongLevelNum;
                }
            }
        }

        //Bomb's position.
        public Point Position;

        //Bomb's image.
        public Texture2D Image;
    }
}
