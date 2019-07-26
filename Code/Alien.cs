using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace xnaDemo
{
    class Alien
    {
        //Holds info about the alien.
        private int whatLevel = 1;

        //Constructor that feeds instance it's info (if default's not chosen).
        public Alien()
        { }
        public Alien(int onLevel, System.Drawing.Point atLocation, Texture2D image)
        {
            this.Level = onLevel;
            this.Position = atLocation;
            this.Image = image;
        }

        //Allows client to set alien's level.
        public int Level
        {
            set
            {
                //If alien's level is 1 or 2, value is valid.
                if((value == 1) || (value == 2))
                {
                    whatLevel = value;
                }
                else //If level's not 1 or 2, then an out of range exception is thrown.
                {
                    System.ArgumentOutOfRangeException wrongLevelNum = new System.ArgumentOutOfRangeException("Level",
                        value.ToString() + " is not a valid level of an alien.  Only levels 1 and 2 are.");
                    throw wrongLevelNum;
                }
            }
            get
            {
                //Property's actually stored in 'whatLevel'
                return whatLevel;
            }
        }

        //Properties for alien's position.
        public System.Drawing.Point Position;

        //Property for alien's image.
        public Texture2D Image;
    }
}
