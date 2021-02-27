using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel;
using System.Numerics;

namespace Maze
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Maze maze;
        enum screen { MAZE_5, MAZE_10, MAZE_15, MAZE_20, HIGH_SCORE, CREDITS, MAIN }
        screen curScreen;
        Texture2D backgroundTexture;
        Texture2D circleTexture;
        Texture2D ballTexture;
        Texture2D mainMenuTexture;
        Texture2D squareTexture;
        Texture2D lineTexture;
        Microsoft.Xna.Framework.Vector2 ballPosition;
        bool buttonPressed;
        Cell curCell;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            curScreen = screen.MAIN; 
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            buttonPressed = false;
        }

        protected override void Initialize()
        {
            ballPosition = new Microsoft.Xna.Framework.Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 650;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("paper background");
            circleTexture = Content.Load<Texture2D>("circle");
            mainMenuTexture = Content.Load<Texture2D>("main");
            ballTexture = Content.Load<Texture2D>("circle");
            squareTexture = new Texture2D(GraphicsDevice, 1, 1);
            lineTexture = new Texture2D(GraphicsDevice, 1, 1);
            lineTexture.SetData<Color>(new Color[] { Color.Black });
            squareTexture.SetData(new[] { Color.Black });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var kstate = Keyboard.GetState();
            if (!buttonPressed)
            {
                if (kstate.IsKeyDown(Keys.F1))
                {
                    //New Game 5x5
                    curScreen = screen.MAZE_5;
                    maze = new Maze(5);
                    buttonPressed = true;
                    ballPosition = new Microsoft.Xna.Framework.Vector2(0, 530);
                    curCell = maze.cells[0][0];
                }
                if (kstate.IsKeyDown(Keys.F2))
                {
                    //New Game 10x10
                    curScreen = screen.MAZE_10;
                    maze = new Maze(10);
                    buttonPressed = true;
                    ballPosition = new Microsoft.Xna.Framework.Vector2(0, 590);
                    curCell = maze.cells[0][0];
                }
                if (kstate.IsKeyDown(Keys.F3))
                {
                    //New Game 15x15
                    curScreen = screen.MAZE_15;
                    maze = new Maze(15);
                    buttonPressed = true;
                    ballPosition = new Microsoft.Xna.Framework.Vector2(0, 610);
                    curCell = maze.cells[0][0];
                }
                if (kstate.IsKeyDown(Keys.F4))
                {
                    //New Game 20x20
                    curScreen = screen.MAZE_20;
                    maze = new Maze(20);
                    buttonPressed = true;
                    ballPosition = new Microsoft.Xna.Framework.Vector2(2, 619);
                    curCell = maze.cells[0][0];
                }
                if (kstate.IsKeyDown(Keys.F5))
                {
                    //Display High Scores
                    curScreen = screen.HIGH_SCORE;
                    buttonPressed = true;
                }
                if (kstate.IsKeyDown(Keys.F6))
                {
                    //Display Credits
                    curScreen = screen.CREDITS;
                    buttonPressed = true;
                }
                if (kstate.IsKeyDown(Keys.Up))
                {
                    if (curCell.walls[0].openYN)
                    {
                        ballPosition.Y -= 600 / maze.size;
                        buttonPressed = true;
                        curCell = maze.cells[curCell.Y + 1][curCell.X];
                    }
                }

                if (kstate.IsKeyDown(Keys.Down))
                {
                    if (curCell.walls[2].openYN)
                    {
                        ballPosition.Y += 600 / maze.size;
                        buttonPressed = true;
                        curCell = maze.cells[curCell.Y - 1][curCell.X];
                    }
                }

                if (kstate.IsKeyDown(Keys.Left))
                {
                    if (curCell.walls[3].openYN)
                    {
                        ballPosition.X -= 600 / maze.size;
                        buttonPressed = true;
                        curCell = maze.cells[curCell.Y][curCell.X - 1];
                    }
                }

                if (kstate.IsKeyDown(Keys.Right))
                {
                    if (curCell.walls[1].openYN)
                    {
                        ballPosition.X += 600 / maze.size;
                        buttonPressed = true;
                        curCell = maze.cells[curCell.Y][curCell.X+1];
                    }
                }
            }
            else if(kstate.IsKeyUp(Keys.Up) && kstate.IsKeyUp(Keys.Right) && kstate.IsKeyUp(Keys.Down) && kstate.IsKeyUp(Keys.Left) && kstate.IsKeyUp(Keys.F1) && kstate.IsKeyUp(Keys.F2) && kstate.IsKeyUp(Keys.F3) && kstate.IsKeyUp(Keys.F4) && kstate.IsKeyUp(Keys.F5) && kstate.IsKeyUp(Keys.F6))
            {
                buttonPressed = false;
            }

            base.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 50, 600, 600), Color.White);

            switch(curScreen)
            {
                case screen.MAIN:
                    _spriteBatch.Draw(mainMenuTexture, new Rectangle(10, 50, 600, 600), Color.White);
                    break;
                case screen.CREDITS:
                    break;
                case screen.HIGH_SCORE:
                    break;
                case screen.MAZE_5:
                case screen.MAZE_10:
                case screen.MAZE_15:
                case screen.MAZE_20:
                    for (int i = 0; i < maze.size; i++)
                    {
                        for (int j = 0; j < maze.size; j++)
                        {
                            //DrawLine(_spriteBatch, new Microsoft.Xna.Framework.Vector2(120, 50), new Microsoft.Xna.Framework.Vector2(120, 170));
                            Wall[] wallKey = maze.GetWalls(i, j);
                            int Ytop = 50 + 600 - (j * (600 / maze.size));
                            int Ybottom = 50 + 600 - ((j+1) * (600 / maze.size));
                            int Xright = i * (600/maze.size);
                            int Xleft = (i + 1) * (600 / maze.size);
                            if (!wallKey[0].openYN || j == maze.size-1)
                            {
                                //Draw North wall
                                DrawLine(_spriteBatch, new Microsoft.Xna.Framework.Vector2(Xleft, Ytop), new Microsoft.Xna.Framework.Vector2(Xright, Ytop));
                            }
                            if (!wallKey[1].openYN || i == maze.size-1)
                            {
                                //Draw East wall
                                DrawLine(_spriteBatch, new Microsoft.Xna.Framework.Vector2(Xright, Ytop), new Microsoft.Xna.Framework.Vector2(Xright, Ybottom));
                            }
                            if (!wallKey[2].openYN || j == 0)
                            {
                                //Draw South wall
                                DrawLine(_spriteBatch, new Microsoft.Xna.Framework.Vector2(Xleft, Ybottom), new Microsoft.Xna.Framework.Vector2(Xright, Ybottom));
                            }
                            if (!wallKey[3].openYN || i == 0)
                            {
                                //Draw West wall
                                DrawLine(_spriteBatch, new Microsoft.Xna.Framework.Vector2(Xleft, Ytop), new Microsoft.Xna.Framework.Vector2(Xleft, Ybottom));
                            }

                        }
                    }
                    _spriteBatch.Draw(ballTexture, new Rectangle((int)ballPosition.X, (int)ballPosition.Y, 600 / maze.size, 600 / maze.size), Color.Red);
                    //_spriteBatch.Draw(ballTexture, ballPosition, null, Color.White, 0f, new Microsoft.Xna.Framework.Vector2(ballTexture.Width / 2, ballTexture.Height / 2), Microsoft.Xna.Framework.Vector2.One, SpriteEffects.None, 0f);
                    break;
            }
            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        
        public void DrawLine(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Vector2 start, Microsoft.Xna.Framework.Vector2 end)
        {
            Microsoft.Xna.Framework.Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            spriteBatch.Draw(lineTexture,
                new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 5),
                null,
                Color.Red,
                angle,
                new Microsoft.Xna.Framework.Vector2(0, 0),
                SpriteEffects.None,
                0);
        }
    }
}
