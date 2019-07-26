using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Forms;          // needed so that we can add normal buttons, etc.

// This project doesn't do anything useful, but it illustrates the functions you'll need
// to implement your game.
namespace xnaDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics; // this represents the graphics device 
        private SpriteBatch spriteBatch;        // all drawing takes place via this object
        private Texture2D backGroundTexture;    // will point to a picture to use for background

        //points to textures of missiles (that you shoot)
        private LinkedList<Missile> missiles = new LinkedList<Missile>();

        //points to a list of textures of bombs (that aliens shoot)
        private LinkedList<Bomb> bombs = new LinkedList<Bomb>();

        //points to a list of aliens (that drop bombs)
        private LinkedList<Alien> aliens = new LinkedList<Alien>();

        //Whether or not game's being played.
        bool gameActive = false;
        
        //Whether or not game's ended.
        bool gameOver = false;

        //Is invicibility cheat on?
        bool invincibleCheat = false;

        //What skill level (0=beginner 1=medium 2=expert)
        int skill;

        //what level is currently being played.
        private int currentLevel = 1;

        //Was the key pressed down a moment ago? (for keeping user from holding down key and shooting)
        private bool keyDownLast = false;

        private Vector2 missilePos;             // position of the missle
        private int onWhatLevel = 1;            // Which level is currently being played? (1 or 2)
        private int numAliensKilled = 0;       // how many times has an alien been killed?
        private int numLivesLeft = 4;           // how many lives are left?
        private Rectangle viewPort;             // tells us the size of the drawable area in the window
        private Ship myShip;                    // reference to the ship controlled by the user
        private KeyboardState oldState;         // keeps previous state of keys pressed, so we can
                                                // detect when a change in state occurs

        // NOTE: we really ought to have a class to keep up with the missle information; 
        // this is left as an exercise for the reader

        const int PANEL_WIDTH = 100;    // width of a panel we'll add for adding buttons, etc.

        // Controls we'll add to form, just for sake of illustration.
        Panel gameControlPanel; // control panel
        Label lblLifeCount;     // a label to show how many lives you have laeft.
        Label lblKillCount; // a label to show how many aliens you killed.

        // Other global vars for game's fine workings.
        int framesPassedInInterval = 0; //# of frames during an inteveral (which lasts 10 seconds).

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            InitializeControlPanel();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            viewPort = GraphicsDevice.Viewport.Bounds;
            oldState = Keyboard.GetState();
            missilePos = Vector2.Zero;

            myShip = new Ship(new Vector2((viewPort.Width - PANEL_WIDTH)/2, 
                                          viewPort.Height - 40));
            base.Initialize();
        }

        // Since this is an xna project; we can't easily drag/drop controls onto the
        // main Form like we have been doing in the past. But, we can do it programatically,
        // as illustrated below.
        private void InitializeControlPanel()
        {
            // instantiate a panel, lives label, and kills label
            gameControlPanel = new Panel();
            lblLifeCount = new Label();
            lblKillCount = new Label();

            // setup the panel control. Note: this panel will overlay the viewport
            // for 100 pixels; we should adjust for this (not shown in example)
            this.gameControlPanel.Dock = DockStyle.Right;
            this.gameControlPanel.Width = PANEL_WIDTH;

            //Sets up the start button and adds it to the panel.
            Button startBtn = new Button();
            startBtn.Location = new System.Drawing.Point(10, 10);
            startBtn.Text = "Start";
            startBtn.Click += new EventHandler(startBtn_Click);
            this.gameControlPanel.Controls.Add(startBtn);

            // setup the lives label control and add it to the panel             
            this.lblLifeCount.Text = "Lives: ";
            this.lblLifeCount.Location = new System.Drawing.Point(10, startBtn.Top + startBtn.Height + 10);
            this.lblLifeCount.AutoSize = true;
            this.gameControlPanel.Controls.Add(this.lblLifeCount);

            // setup the kill label control and add it to the panel
            this.lblKillCount.Text = "Kills: ";
            this.lblKillCount.Location = new System.Drawing.Point(10, lblLifeCount.Top + lblLifeCount.Height + 10);
            this.lblKillCount.AutoSize = true;
            this.gameControlPanel.Controls.Add(this.lblKillCount);

            // setup the settings button and add it to the panel
            Button settingsBtn = new Button();
            settingsBtn.Location = new System.Drawing.Point(10, lblKillCount.Top + lblKillCount.Height + 10);
            settingsBtn.Text = "Settings";
            settingsBtn.Click += new EventHandler(settingsBtn_Click);
            this.gameControlPanel.Controls.Add(settingsBtn);


            // get a reference to game window  
            // note that the 'as Form' is the same as a type cast
            Form mainForm = Control.FromHandle(this.Window.Handle) as Form;

            // add the panel to the game window form
            mainForm.Controls.Add(gameControlPanel);

            //Sets window's title.
            mainForm.Text = "FinalProgram - SpaceInvaders";
        }

        //Checks to see if two items collided (but only if first is on left and and second is on right)
        public bool DetectCollisionForOne(System.Drawing.Point location1, System.Drawing.Point size1,
                                System.Drawing.Point location2)
        {
            //If second item's top, left corner is within bound of first item's.
            if((location2.X >= location1.X) && (location2.X <= location1.X + size1.X) &&
                (location2.Y >= location1.Y) && (location2.Y <= location1.Y + size1.Y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Fors starting or restarting level.
        void LoadAliens()
        {
            //There are ten enemies on screen to start with.
            for (int onAlien = 1; onAlien <= 10; onAlien++)
            {
                //Determines if level 1 or 2 alien isneeded.
                Texture2D alienImage;
                if(currentLevel == 1)
                {
                    alienImage = Content.Load<Texture2D>("alien_1");
                }
                else
                {
                    alienImage = Content.Load<Texture2D>("alien_2");
                }
                
                aliens.AddLast(new Alien(currentLevel, new System.Drawing.Point(55 * onAlien, 5), alienImage));
            }
        }

        // Handling of button clicks.

        void startBtn_Click(object sender, EventArgs e)
        {
            if(!gameActive)
            {
                //When everything's reset, currentLevel = 1.
                currentLevel = 1;

                //Gets rid of everything and resets life states.
                bombs.Clear();
                missiles.Clear();
                numLivesLeft = 4;
                numAliensKilled = 0;

                //Puts aliens back
                LoadAliens();

                //Game can move.
                gameActive = true;
                gameOver = false;
            }
        }

        void settingsBtn_Click(object sender, EventArgs e)
        {
            //Pause game while setting things.
            gameActive = false;

            //Shows form and sets its controls to what game mode you're in.
            SettingsForm settingsFormInst = new SettingsForm();

            //Changes option positions to current settings.
            settingsFormInst.optionInvincible.Checked = invincibleCheat;
            settingsFormInst.skillLevel.SelectedIndex = skill;

            //If OK, changes game settings to options.
            if (settingsFormInst.ShowDialog() == DialogResult.OK)
            {
                invincibleCheat = settingsFormInst.optionInvincible.Checked;
                skill = settingsFormInst.skillLevel.SelectedIndex;
            }

            //After close, game is like it was before.
            if (!gameOver)
            {
                gameActive = true;
            }
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Reads skill level and whether or not invincible is on from file.
            StreamReader fileStream = new StreamReader("Settings.txt");

            invincibleCheat = bool.Parse(fileStream.ReadLine());
            skill = int.Parse(fileStream.ReadLine());

            fileStream.Close();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load textures into memory once during LoadContent()
            backGroundTexture = Content.Load<Texture2D>("stars");
            Ship.Texture = Content.Load<Texture2D>("ship");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// This gets called ~60 times per second
        protected override void Update(GameTime gameTime)
        {
            if(gameActive)
            {
                moveShip();     // if a arrow key is held down, move the ship

                fireMissle();   // create a missle if the space bar was hit

                moveMissles();  //For active missiles, update positions

                moveBombs(); //For active bombs, update positions.

                moveAliens(); //For live aliens, update their positions.

                //If two seconds since last bomb, random live alien drops bomb.
                if (framesPassedInInterval % 120 == 0)
                {
                    //If there's no aliens, DivideByZero error occurs.
                    try
                    {
                        int alienChosen = new Random().Next() % aliens.Count; //Which alien to pick of those that are alive.
                       
                        //Get bomb's image (depending on level)
                        Texture2D bombImage;
                        if(currentLevel == 1)
                        {
                            bombImage = Content.Load<Texture2D>("alien_bomb_1");
                        }
                        else
                        {
                            bombImage = Content.Load<Texture2D>("alien_bomb_2");
                        }

                        bombs.AddLast(new Bomb(currentLevel, aliens.ElementAt(alienChosen).Position,
                                    bombImage)); //Adds a bomb at position where picked alien is.
                    }
                    catch { } //No aliens? No bombs drop.
                }

                //Seeing if missile has hit an alien.
                LinkedList<Alien> aliensToRemove = new LinkedList<Alien>(); //Collects aliens that got hit
                foreach(Missile currentMissile in missiles)
                {
                    foreach(Alien currentAlien in aliens)
                    {
                        //Alien size for detecting collision.
                        System.Drawing.Point alienSize = new System.Drawing.Point();
                        if(currentLevel == 1)
                        {
                            alienSize.X = 42;
                            alienSize.Y = 48;
                        }
                        else
                        {
                            alienSize.X = 52;
                            alienSize.Y = 35;
                        }

                        //If missile's position is within alien's "rectangle" (where it is):
                        //[is it within x bounds and y bounds?]
                        if( DetectCollisionForOne(currentAlien.Position, alienSize, currentMissile.Position) ||
                            DetectCollisionForOne(currentMissile.Position, new System.Drawing.Point(5,10), currentAlien.Position) )
                        {
                            //If alien's hit, both alien and missile are gone.
                            aliensToRemove.AddLast(currentAlien);

                            //One gets killed.
                            numAliensKilled++;
                        }
                    }
                }

                //Goes through "aliens to remove" and gets rid of them.
                foreach(Alien currentAlien in aliensToRemove)
                {
                    aliens.Remove(currentAlien);
                }

                //Updates lives.
                lblLifeCount.Text = "Lives: " + numLivesLeft;
                if (numLivesLeft == 0) //If all lives gone, end game
                {
                    //If lives are zero and game's going, end it.
                    if(!gameOver)
                    {
                        gameActive = false;
                        gameOver = true;

                        MessageBox.Show("Game Over.");
                    }
                }

                //Updates kills.
                lblKillCount.Text = "Kills: " + numAliensKilled;

                //Are aliens destroyed? (If yes, go to next level and get rid of missiles).
                if((aliens.Count == 0) && !gameOver)
                {
                    if(currentLevel == 1)
                    {
                        //Ups level num, re-loads aliens, and resets other stuff.
                        currentLevel = 2;
                        numAliensKilled = 0;
                        LoadAliens();
                        missiles.Clear();
                    }
                    else
                    {
                        //Win if game's not over and end it.
                         if(!gameOver)
                        {
                            MessageBox.Show("You won!");
                            gameActive = false;
                            gameOver = true;
                        }
                    }
                }

                //Since an interval lasts 10 seconds, 'framesPassed' needs to reset to 0 and 1-increment it.
                if (framesPassedInInterval == 600)
                {
                    framesPassedInInterval = 1;
                }
                else
                {
                    framesPassedInInterval++;
                }

                
            }  
        }

        // Check to see if an arrow key is currently pressed. Take appropriate
        // action to move ship (note, no boundary checking is done to ensure
        // ship doesn't move off-screen; this should be fixed in your program).
        private void moveShip()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                myShip.move(Ship.Direction.Left);  
            }
            else if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                myShip.move(Ship.Direction.Right);
            }

        }

        // Check to see if the space key is down. If so, set flag and position so
        // that the Draw will Draw the missle. Note: this is a little different than
        // the ship, because we only want to fire the missle on a button click
        private void fireMissle()
        {
            KeyboardState newState = Keyboard.GetState();

            // Is the SPACE key down?
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            {
               
                // If it was not down the previous time we checked, then this is a 
                // new key press. If the key was down in the old state, they're just
                // holding it down, so we ignore this state.
                if (!keyDownLast)
                {
                    //Adds missile to list of those in the air.
                    missiles.AddLast(new Missile(new System.Drawing.Point((int)myShip.GunPosition.X,
                                     (int)myShip.GunPosition.Y), Content.Load<Texture2D>("missile")));

                    lblKillCount.Text = "Kills: " + numAliensKilled.ToString();

                    //When code segment rolls around again, key was pressed last time unless it gets "unpressed".
                    keyDownLast = true;
                }
            }
            else
            {
                keyDownLast = false;
            }


            // Update saved state.
            oldState = newState;
        }

        // If the missle is alive, move it up a few clicks until it goes off screen
        public void moveMissles()
        {
            //Holds collection of missiles that left the screen.
            LinkedList<Missile> missilesToRemove = new LinkedList<Missile>();

            //Goes through list of missiles currently in flight and updates their position.s
            foreach(Missile currentMissile in missiles)
            {
                //If missile is visible (still within game screen's bounds).
                if (currentMissile.Position.Y > -11)
                {
                    currentMissile.Position.Y -= 3;
                }
                else
                {
                    //Adds current missile to list of them to remove since it's off the screen.
                    missilesToRemove.AddLast(currentMissile);
                }
            }

            //In collection of "missiles to remove", goes through them and removes all from main 'missiles'.
            foreach(Missile currentMissile in missilesToRemove)
            {
                missiles.Remove(currentMissile);
            }
        }

        public void moveBombs()
        {
            //Holds collection of bombs that left the screen.
            LinkedList<Bomb> bombsToRemove = new LinkedList<Bomb>();

            //Goes through list of bombs currently in flight and updates their positions.
            foreach (Bomb currentBomb in bombs)
            {
                //For checking to see if bomb hit ship.
                int bombX = currentBomb.Position.X;
                int bombY = currentBomb.Position.Y;
                int bombW = 38;
                int bombH = 40;

                int shipX = (int)myShip.GunPosition.X;
                int shipY = (int)myShip.GunPosition.Y;
                int shipW = 20;
                int shipH = 20;

                //If bomb collided with ship; also makes gun a little bit
                if( DetectCollisionForOne(currentBomb.Position, new System.Drawing.Point(38, 40),
                    new System.Drawing.Point((int)myShip.GunPosition.X, (int)myShip.GunPosition.Y)) )
                {
                    //Adds current bomb to list of them to remove since it's gotten as low as ship.
                    bombsToRemove.AddLast(currentBomb);

                    //Lose life only if "invincible" cheat is off.
                    if (!invincibleCheat)
                    {
                        numLivesLeft--;
                    }
                }
                else
                {
                    //If bomb is still on screen, move it; otherwise, delete it
                    if(currentBomb.Position.Y < viewPort.Height)
                    {
                        //Skill is 1 or 2; if skill's higher, move bomb more
                        currentBomb.Position.Y += currentLevel + skill + 1;
                    }
                    else
                    {
                        bombsToRemove.AddLast(currentBomb);
                    }
                }
            }

            //In collection of "bombs to remove", goes through them and removes all from main 'missiles'.
            foreach (Bomb currentBomb in bombsToRemove)
            {
                bombs.Remove(currentBomb);
            }
        }

        public void moveAliens()
        {
            bool shouldAliensClear = false; //If aliens reached bottom of screen, they should reset.
            foreach(Alien currentAlien in aliens)
            {
                if (framesPassedInInterval % 2 == 0) //Only move every 5 frames
                {
                    if (currentAlien.Position.Y > myShip.GunPosition.Y + 5) //If aliens pass gun, restart level.
                    {
                        //Resets kills
                        numAliensKilled = 0;

                        //When aliens are cleared and reset(because they hit), previous missiles shouldn't kill
                        //new aliens
                        missiles.Clear();

                        //Aliens should start over.
                        shouldAliensClear = true;
                    }
                    else
                    {
                        currentAlien.Position.Y += skill + currentLevel;
                    }
                }
            }

            if(shouldAliensClear)
            {
                //Re-loads aliens.
                aliens.Clear();

                LoadAliens();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// This is called in the main loop after Update to render the current state of the game 
        /// on the screen. 
        protected override void Draw(GameTime gameTime)
        {
            // spriteBatch is an object that allows us to draw everything
            // on screen (it contains the Draw functions).
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            
            // draw the background
            spriteBatch.Draw(backGroundTexture, viewPort, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            
            // draw the ship over background
            myShip.Draw(spriteBatch);

            //Draws all aliens in linked list of them.
            foreach(Alien currentAlien in aliens)
            {
                spriteBatch.Draw(currentAlien.Image, new Vector2(currentAlien.Position.X, currentAlien.Position.Y),
                    Color.White);
            }
            
            //Draws all missiles currently in flight.
            foreach(Missile currentMissile in missiles)
            {
                spriteBatch.Draw(currentMissile.Image, new Vector2(currentMissile.Position.X, currentMissile.Position.Y),
                    Color.White);
            }

            //Draws all bombs currently falling.
            foreach(Bomb currentBomb in bombs)
            {
                spriteBatch.Draw(currentBomb.Image, new Vector2(currentBomb.Position.X, currentBomb.Position.Y),
                    Color.White);
            }
           
            // This tells the graphics pipeline that we're done drawing this frame,
            // and asks it to push the pixels to the graphics frame buffer.
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
