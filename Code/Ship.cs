using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace xnaDemo
{
    class Ship
    {
       
        private Vector2 _pos;                           // position of ship (upper left)
        public enum Direction { Left, Right, Up, Down };
        public static Texture2D Texture { get; set; }

        public Ship(Vector2 pos)
        {
            _pos = pos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, _pos, Color.White);
        }

        public void move(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left: _pos.X -= 5;
                    break;
                case Direction.Right: _pos.X += 5;
                    break;
                case Direction.Up: _pos.Y -= 5;
                    break;
                case Direction.Down: _pos.Y += 5;
                    break;
            }
        }

        // returns center top of texture (i.e., where the gun is located)
        public Vector2 GunPosition
        {
            get
            {
                return new Vector2(_pos.X + (Texture.Width/2), _pos.Y);
            }
        }


    }
}
