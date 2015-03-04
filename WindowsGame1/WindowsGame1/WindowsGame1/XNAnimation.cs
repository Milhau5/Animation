using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{   
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Sprite cutman;
        Texture2D character; // Keeps image of character
        Texture2D rock;
        Vector2 Position = new Vector2(200, 200); // Holds the position of character on screen
        Vector2 Spot = new Vector2(350, 200);
        Point frameSize = new Point(27, 39); // Holds first width and height of the single frame we want to see on screen every time (height used to be 49)
        Point rockSize = new Point(35, 35);
        Point currentFrame = new Point(0, 0); // Holds position of where we are in the spritesheet (first frame, first line)
        Point objectFrame = new Point(0, 2);
        Point sheetSize = new Point(440, 435); // Holds size of spritesheet
        float speed = 15; // Speed

        KeyboardState currentState;
        KeyboardState theKeyboardState;
        KeyboardState oldKeyboardState;
        bool isWalking = false;
        enum State
        {
            Walking,
            Punch,
            Jump,
            JumpForward,
            JumpBackwards
        }

        State mCurrentState = State.Walking; // First state is the character walking
        //State mGoState = State.JumpForward;

        TimeSpan nextFrameInterval = TimeSpan.FromSeconds((float)1 / 16); // Used to adjust animation frame speed
        TimeSpan nextFrame; // Used to change frame

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 100); // Adjust how fast things happen on XNA
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load our picture of the character to animate
            character = Content.Load<Texture2D>("RobotMasters");
            rock = Content.Load<Texture2D>("RobotMasters");

            //Animation stuff??

            // TODO: use this.Content to load your game content here
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
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            //First we allow for the taking of input
            currentState = Keyboard.GetState();
            //theKeyboardState = Keyboard.GetState();
            
            if (mCurrentState == State.Walking)
            {
                currentFrame.X++; // Advance position by 1 until frame it at the 2nd, then restart at first spot (frame 0)
                if (currentFrame.X >= 2 && !isWalking)
                    currentFrame.X = 0;
                /*if (mGoState == State.JumpForward)
                {
                   currentFrame.X++; 
                  if (currentFrame.X >= 3)
                        currentFrame.X = 4;
                }*/
                
                if (currentState.IsKeyDown(Keys.Right))
                {
                    frameSize = new Point(27, 39);
                    Position.Y = 200; 
                    currentFrame.Y = 0;
                    //currentFrame.X = 7;
                    currentFrame.X++;
                    if (currentFrame.X >= 8)
                        currentFrame.X = 4;
                    Position.X += speed;
                }
                else if (currentState.IsKeyDown(Keys.Left))
                {
                    isWalking = true;
                    frameSize = new Point(20, 40); //why the heck aren't all the sprites the same size?
                    Position.Y = 200;
                    currentFrame.Y = 0;
                    //currentFrame.X = 3; //?
                    currentFrame.X++;
                    //mCurrentState = State.JumpForward;
                    if (currentFrame.X >= 6)
                        currentFrame.X = 3;
                    Position.X -= speed;
                }
                else if (currentState.IsKeyUp(Keys.Left))
                {
                    isWalking = false;
                    frameSize = new Point(27, 39);
                    mCurrentState = State.Walking;
                }
                // Prevent player from moving off the left edge of the screen
                if (Position.X < 0)
                    Position = new Vector2(0, Position.Y);
                /* Prevent player from moving off the right edge of the screen
                // Makes an int variable that defines what the right edge of the screen is
                // Then we do a check
                int rightEdge = GraphicsDevice.Viewport.Width - character.Bounds.Width;
                if (Position.X > rightEdge)
                    Position = new Vector2(rightEdge, Position.Y);*/ 
                
                if (Position.X + 27 > Spot.X) // ???
                    Position = new Vector2(Spot.X - 28, Position.Y);
                //^Collision detection with 'rock' (Gutsman)^
            }
            oldKeyboardState = theKeyboardState; //Will be used for a single key press

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(character, Position, new Rectangle(
                                frameSize.X * currentFrame.X, //give position of frame each time on the X
                                frameSize.Y * currentFrame.Y, //give position of frame each time on the Y
                                frameSize.X, //set width of frame
                                frameSize.Y), //set height of frame
                                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(rock, Spot, new Rectangle(
                                rockSize.X * objectFrame.X, //give position of frame each time on the X
                                rockSize.Y * objectFrame.Y, //give position of frame each time on the Y
                                rockSize.X, //set width of frame
                                rockSize.Y), //set height of frame
                                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();
            //^^Give name of Texture 2D variable, then position.
            //Then make a rectangle

            base.Draw(gameTime);
        }
    }
}
