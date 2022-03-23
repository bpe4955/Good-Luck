using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

namespace Good_Luck
{
    //Enums
    /// <summary>
    /// The various states of the game with different screens
    /// </summary>
    enum GameState
    {
        Title,
        Tutorial,
        Credits,
        Game,
        Pause,
        Options,
        Keybinds,
        GameOver,
        Exit
    }
    /// <summary>
    /// enum to define which <see cref="MouseButton"/> you want to check for 
    /// similar to <see cref="Keys"/> for <see cref="Keyboard"/> input
    /// </summary>
    enum MouseButton
    {
        Left,
        Right
    }

    public class Game1 : Game
    {
        //Fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameState gameState;
        MouseState previousMouseState;
        MouseState mouseState;
        KeyboardState kb;
        KeyboardState previousKb;
        List<Button> buttons;

        //Assets
        Texture2D smallSquare;
        Texture2D smallSquareGray;
        Texture2D buttonClick;
        Texture2D menuItemTextures;
        Texture2D playerTexture;

        Rectangle playerRect;
        Player player;

        //This will hold the backdrop, pause, key box
        //mouse image, crossbones, and skull
        //source rectangle data in that order
        Rectangle[] menuItems;
        SpriteFont MetalManiaButtons;
        SpriteFont MetalManiaTitle;
        SpriteFont MetalManiaNormal;
        SpriteFont JelleeRoman20;

        //Misc
        Color lightPurple;
        Color darkPurple;
        Keys[] bindings;

        //Save File Fields
        private string fileName;
        private int highScoreCount;
        HighScoreData saveData;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //Initialize fields
            gameState = GameState.Title;
            buttons = new List<Button>();
            menuItems = new Rectangle[6]
            {
                new Rectangle(0, 0, 500, 500),
                new Rectangle(505, 0, 59, 56),
                new Rectangle(505, 61, 50, 50),
                new Rectangle(505, 116, 84, 132),
                new Rectangle(0, 505, 272, 76),
                new Rectangle(277, 505, 52, 63)
            };
            playerRect = new Rectangle(250, 250, 50, 50);
            

            lightPurple = new Color(232, 216, 255);
            darkPurple = new Color(21, 0, 51);
            bindings = new Keys[4]
            {
                Keys.W,
                Keys.A,
                Keys.S,
                Keys.D
            };

            //Name of save file with the number of high scores
            fileName = "../../../HighScores.txt";
            highScoreCount = 5;
            //Check to see if save file exists
            if (!File.Exists(fileName))
            {
                //If the file doesn't exist, make a default one
                HighScoreData data = new HighScoreData(highScoreCount);
                for (int i = 0; i < highScoreCount; i++)
                {
                    data.levels[i] = (0);
                    data.scores[i] = (000);
                }

                HighScoreData.SaveHighScores(data, fileName);
            }
            //Load in save data from file to variable
            saveData = HighScoreData.LoadHighScores(fileName);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Texture Loading
            smallSquare = Content.Load<Texture2D>("ButtonDefault");
            smallSquareGray = Content.Load<Texture2D>("ButtonHover");
            buttonClick = Content.Load<Texture2D>("ButtonClick");
            MetalManiaButtons = Content.Load<SpriteFont>("MetalManiaButtons");
            MetalManiaTitle = Content.Load<SpriteFont>("MetalManiaTitle");
            MetalManiaNormal = Content.Load<SpriteFont>("MetalManiaNormal");
            JelleeRoman20 = Content.Load<SpriteFont>("JelleeRoman20");
            menuItemTextures = Content.Load<Texture2D>("MenuImages");
            playerTexture = Content.Load<Texture2D>("smallSquare");

            // Entity Loading
            player = new Player(playerRect, playerTexture, 5, 10, 0);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //Get input from mouse and keyboard
            mouseState = Mouse.GetState();
            kb = Keyboard.GetState();

            switch (gameState)
            {
                case GameState.Title:
                    //Reset the button list
                    buttons.Clear();
                    //Create buttons
                    int width = 198;
                    int midX = (_graphics.PreferredBackBufferWidth / 2) - (width / 2);
                    int height = (int)Math.Round(width * .25f);
                    int spacing = (int)(height * 1.1f);
                    int currentY = 135;
                    buttons.Add(new Button(GameState.Game, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick));
                    currentY += spacing;
                    buttons.Add(new Button(GameState.Tutorial, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick));
                    currentY += spacing;
                    buttons.Add(new Button(GameState.Options, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick));
                    currentY += spacing;
                    buttons.Add(new Button(GameState.Credits, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick));
                    currentY += spacing;
                    //There's no exit... But there's and exit button
                    buttons.Add(new Button(GameState.Exit, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick));

                    //Check if buttons are clicked and act accordingly
                    CheckButtons();

                    
                    break;
                case GameState.Tutorial:
                    //Reset the button list
                    buttons.Clear();
                    //Back Button
                    width = 198;
                    midX = (_graphics.PreferredBackBufferWidth / 2) - (width / 2);
                    height = (int)Math.Round(width * .25f);
                    buttons.Add(new Button(GameState.Title, new Rectangle(midX, 370, width, height), smallSquare, smallSquareGray, buttonClick));
                    //Check if buttons are clicked and change gameState
                    CheckButtons();

                    break;
                case GameState.Credits:
                    //Reset the button list
                    buttons.Clear();
                    //Back Button
                    width = 198;
                    midX = (_graphics.PreferredBackBufferWidth / 2) - (width / 2);
                    height = (int)Math.Round(width * .25f);
                    buttons.Add(new Button(GameState.Title, new Rectangle(midX, 370, width, height), smallSquare, smallSquareGray, buttonClick));
                    //Check if buttons are clicked and change gameState
                    CheckButtons();
                    break;
                case GameState.Game:
                    player.Move(kb);
                    if (SingleMouseClick(MouseButton.Left))
                    {
                        player.Shoot();
                    }
                    break;
                case GameState.Pause:
                    //Reset the button list
                    buttons.Clear();
                    //Back Button
                    buttons.Add(new Button(GameState.Title, new Rectangle(20, 10, 50, 20), smallSquare, smallSquareGray, buttonClick));
                    //Check if buttons are clicked and change gameState
                    CheckButtons();
                    break;
                case GameState.Options:
                    //Reset the button list
                    buttons.Clear();
                    //Back Button
                    width = 198;
                    midX = (_graphics.PreferredBackBufferWidth / 2) - (width / 2);
                    height = (int)Math.Round(width * .25f);
                    spacing = (int)(height * 1.1f);
                    buttons.Add(new Button(GameState.Keybinds, new Rectangle(midX, 370 - spacing, width, height), smallSquare, smallSquareGray, buttonClick));
                    buttons.Add(new Button(GameState.Title, new Rectangle(midX, 370, width, height), smallSquare, smallSquareGray, buttonClick));
                    //Check if buttons are clicked and change gameState
                    CheckButtons();
                    break;
                case GameState.Keybinds:
                    //Reset the button list
                    buttons.Clear();
                    //Back Button
                    width = 198;
                    midX = (_graphics.PreferredBackBufferWidth / 2) - (width / 2);
                    height = (int)Math.Round(width * .25f);
                    buttons.Add(new Button(GameState.Options, new Rectangle(midX, 370, width, height), smallSquare, smallSquareGray, buttonClick));
                    //Check if buttons are clicked and change gameState
                    CheckButtons();
                    break;
                case GameState.GameOver:
                    break;
                case GameState.Exit:
                    Exit();
                    break;
            }

            previousKb = kb;
            previousMouseState = mouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.Title:
                    DisplayBackAndTitle("Good Luck~");


                    //Draw all the buttons
                    DrawButtons();

                    if (buttons.Count >= 5)
                    {
                        string[] buttonText = new string[5]
                        {
                            "Start",
                            "Tutorial",
                            "Options",
                            "Credits",
                            "Exit"
                        };
                        for(int i = 0; i < 5; ++i)
                        {
                            _spriteBatch.DrawString(MetalManiaButtons, buttonText[i],
                                new Vector2((int)(buttons[i].Rect.X + buttons[i].Rect.Width / 3.5f),
                                                  buttons[i].Rect.Y + buttons[i].Rect.Height / 10), darkPurple);
                        }

                    }


                    break;
                case GameState.Tutorial:
                    DisplayBackAndTitle("Controls");

                    //Keys
                    int spacing = 55;
                    int initX = 100;
                    int initY = 200;
                    Vector2 fontSize;
                    for (int i = 1; i < 4; ++i)
                    {
                        Point pos = new Point(initX + (spacing * (i - 1)), initY);
                        _spriteBatch.Draw(menuItemTextures, new Rectangle(pos.X, pos.Y, 50, 50), menuItems[2], Color.White);
                        fontSize = JelleeRoman20.MeasureString(bindings[i].ToString());
                        _spriteBatch.DrawString(JelleeRoman20, bindings[i].ToString(),
                            new Vector2(pos.X + (int)(25 - (fontSize.X / 2)),
                                        pos.Y + (int)(25 - (fontSize.Y / 2))), lightPurple);
                    }
                    _spriteBatch.Draw(menuItemTextures, new Rectangle(initX + spacing, initY - spacing, 50, 50), menuItems[2], Color.White);
                    fontSize = JelleeRoman20.MeasureString(bindings[0].ToString());
                    _spriteBatch.DrawString(JelleeRoman20, bindings[0].ToString(),
                        new Vector2(initX + spacing + (int)(25 - (fontSize.X / 2)),
                                    initY - spacing + (int)(25 - (fontSize.Y / 2))), lightPurple);

                    _spriteBatch.DrawString(MetalManiaNormal, "To move", new Vector2(initX + (int)(spacing/4), initY + spacing), lightPurple);

                    //Mouse
                    _spriteBatch.Draw(menuItemTextures, new Rectangle(400, initY - (int)(spacing / 1.5f), 84, 132), menuItems[3], Color.White);
                    _spriteBatch.DrawString(MetalManiaNormal, "Left-Click\nto shoot", new Vector2(525, initY - (int)(spacing / 1.5f)), lightPurple);

                    //Draw all the buttons
                    DrawButtons();

                    if (buttons.Count > 0)
                    {
                        _spriteBatch.DrawString(MetalManiaButtons, "Back",
                            new Vector2((int)(buttons[0].Rect.X + buttons[0].Rect.Width / 2.5f),
                                              buttons[0].Rect.Y + buttons[0].Rect.Height / 10), darkPurple);
                    }
                    break;
                case GameState.Credits:
                    DisplayBackAndTitle("Controls");

                    //Draw names
                    _spriteBatch.DrawString(MetalManiaNormal, "Made by...\n" +
                                                              "   - Aaron Bush\n" +
                                                              "   - Brian Egan\n" +
                                                              "   - John Haley\n" +
                                                              "   - Michaela Castle\n", new Vector2(100, 100), lightPurple);

                    //Draw all the buttons
                    DrawButtons();

                    if (buttons.Count > 0)
                    {
                        _spriteBatch.DrawString(MetalManiaButtons, "Back",
                            new Vector2((int)(buttons[0].Rect.X + buttons[0].Rect.Width / 2.5f),
                                              buttons[0].Rect.Y + buttons[0].Rect.Height / 10), darkPurple);
                    }
                    break;
                case GameState.Game:
                    player.Draw(_spriteBatch);
                    break;
                case GameState.Pause:
                    break;
                case GameState.Options:
                    DisplayBackAndTitle("Options");

                    //Draw all the buttons
                    DrawButtons();

                    if (buttons.Count > 1)
                    {
                        _spriteBatch.DrawString(MetalManiaButtons, "Keybinds",
                            new Vector2((int)(buttons[0].Rect.X + buttons[0].Rect.Width / 3.5f),
                                              buttons[0].Rect.Y + buttons[0].Rect.Height / 10), darkPurple);
                        _spriteBatch.DrawString(MetalManiaButtons, "Back",
                            new Vector2((int)(buttons[1].Rect.X + buttons[1].Rect.Width / 3.5f),
                                              buttons[1].Rect.Y + buttons[1].Rect.Height / 10), darkPurple);
                    }

                    break;
                case GameState.Keybinds:
                    DisplayBackAndTitle("Keybindings");

                    string[] text = new string[4]
                    {
                        "Up",
                        "Left",
                        "Down",
                        "Right"
                    };

                    //Keys
                    spacing = 60;
                    initX = 315;
                    initY = 190;
                    for (int i = 0; i < 4; ++i)
                    {
                        Point pos = new Point(initX, initY + (spacing * (i - 1)));
                        _spriteBatch.Draw(menuItemTextures, new Rectangle(pos.X, pos.Y, 50, 50), menuItems[2], Color.White);
                        fontSize = JelleeRoman20.MeasureString(bindings[i].ToString());
                        _spriteBatch.DrawString(JelleeRoman20, bindings[i].ToString(),
                            new Vector2(pos.X + (int)(25 - (fontSize.X / 2)),
                                        pos.Y + (int)(25 - (fontSize.Y / 2))), lightPurple);
                        _spriteBatch.DrawString(MetalManiaNormal, text[i], new Vector2(pos.X + 75, pos.Y), lightPurple);
                    }

                    //Draw all the buttons
                    DrawButtons();

                    if (buttons.Count > 0)
                    {
                        _spriteBatch.DrawString(MetalManiaButtons, "Back",
                            new Vector2((int)(buttons[0].Rect.X + buttons[0].Rect.Width / 2.5f),
                                              buttons[0].Rect.Y + buttons[0].Rect.Height / 10), darkPurple);
                    }
                    break;
                case GameState.GameOver:
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        //Methods


        private void DisplayBackAndTitle(string title)
        {
            _spriteBatch.Draw(menuItemTextures, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
                        menuItems[0], Color.White);
            float titleSize = MetalManiaTitle.MeasureString(title).X;
            _spriteBatch.DrawString(MetalManiaTitle, title, new Vector2(
                (int)((_graphics.PreferredBackBufferWidth / 2) - (titleSize / 2)), 50), lightPurple);
        }

        /// <summary>
        /// Checks if the key press is done once.
        /// </summary>
        /// <param name="key"> The key being pressed</param>
        /// <returns></returns>
        private bool SingleKeyPress(Keys key)
        {
            return kb.IsKeyDown(key) && previousKb.IsKeyUp(key);
        }

        /// <summary>
        /// Checks if a desired mouse button has been newly pressed
        /// </summary>
        /// <param name="button">the mouse button to check for (left or right)</param>
        /// <returns>True if the desired button has been pressed down this frame but not the last</returns>
        private bool SingleMouseClick(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return (mouseState.LeftButton.Equals(ButtonState.Pressed) && !previousMouseState.LeftButton.Equals(ButtonState.Pressed));
                case MouseButton.Right:
                    return (mouseState.RightButton.Equals(ButtonState.Pressed) && !previousMouseState.RightButton.Equals(ButtonState.Pressed));
                //Default case otherwise visual studio complains
                default:
                    return false;
            }
        }
        /// <summary>
        /// Loop through every button to check if it was clicked,
        /// If it was, call its clicked method to change the state
        /// </summary>
        private void CheckButtons()
        {
            //If the left mouse button is clicked, loop through buttons to see if they were clicked
            if (SingleMouseClick(MouseButton.Left))
            {
                for (int i = 0; i < buttons.Count;)
                {
                    //If a button was clicked, change the gameState to the button's state
                    if (buttons[i].Collision(mouseState))
                    {
                        gameState = buttons[i].ClickedGetState();
                        return;
                    }
                    else
                    {
                        i++;
                    };
                }
            }
        }

        /// <summary>
        /// Draw every button in the buttons list
        /// </summary>
        private void DrawButtons()
        {
            foreach (Button button in buttons)
            {
                //Check for collision to determine for each button if the
                //cursor is hovering over them then draw accordingly
                button.Collision(mouseState);
                button.Draw(_spriteBatch);
            }
        }

        // To Be Finished when player class
        ///// <summary>
        ///// Sorts and saves the player's data
        ///// </summary>
        //private void SaveHighScore()
        //{
        //    //Create the data to save
        //    HighScoreData data = HighScoreData.LoadHighScores(fileName);
        //
        //    int scoreIndex = -1;
        //    //Loop through saved data to find where to place new data
        //    for (int i = 0; i < highScoreCount; i++)
        //    {
        //        if (player.TotalScore > data.scores[i])
        //        {
        //            scoreIndex = i;
        //            break;
        //        }
        //    }
        //
        //    //If new score is found, insert into list
        //    if (scoreIndex > -1)
        //    {
        //        for (int i = highScoreCount - 1; i > scoreIndex; i--)
        //        {
        //            data.scores[i] = data.scores[i - 1];
        //            data.levels[i] = data.levels[i - 1];
        //        }
        //
        //        data.scores[scoreIndex] = player.TotalScore;
        //        data.levels[scoreIndex] = level;
        //
        //        HighScoreData.SaveHighScores(data, fileName);
        //    }
        //
        //}
    }
}
