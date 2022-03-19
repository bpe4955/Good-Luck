using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        GameOver
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
        SpriteFont MetalMania20;

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

            smallSquare = Content.Load<Texture2D>("ButtonDefault");
            smallSquareGray = Content.Load<Texture2D>("ButtonHover");
            buttonClick = Content.Load<Texture2D>("ButtonClick");
            MetalMania20 = Content.Load<SpriteFont>("MetalMania20");
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
                    buttons.Add(new Button(GameState.Tutorial, new Rectangle(20, 10, 79, 20), smallSquare, smallSquareGray, buttonClick));
                    buttons.Add(new Button(GameState.Credits, new Rectangle(20, 40, 79, 20), smallSquare, smallSquareGray, buttonClick));
                    buttons.Add(new Button(GameState.Options, new Rectangle(20, 70, 79, 20), smallSquare, smallSquareGray, buttonClick));

                    //Check if buttons are clicked and act accordingly
                    CheckButtons();

                    
                    break;
                case GameState.Tutorial:
                    //Reset the button list
                    buttons.Clear();
                    //Back Button
                    buttons.Add(new Button(GameState.Title, new Rectangle(20, 10, 50, 20), smallSquare, smallSquareGray, buttonClick));
                    //Check if buttons are clicked and change gameState
                    CheckButtons();

                    break;
                case GameState.Credits:
                    //Reset the button list
                    buttons.Clear();
                    //Back Button
                    buttons.Add(new Button(GameState.Title, new Rectangle(20, 10, 50, 20), smallSquare, smallSquareGray, buttonClick));
                    //Check if buttons are clicked and change gameState
                    CheckButtons();
                    break;
                case GameState.Game:
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
                    buttons.Add(new Button(GameState.Title, new Rectangle(20, 10, 50, 20), smallSquare, smallSquareGray, buttonClick));
                    //Check if buttons are clicked and change gameState
                    CheckButtons();
                    break;
                case GameState.Keybinds:
                    break;
                case GameState.GameOver:
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
                    //Draw all the buttons
                    DrawButtons();
                    break;
                case GameState.Tutorial:
                    //Draw all the buttons
                    DrawButtons();
                    break;
                case GameState.Credits:
                    //Draw all the buttons
                    DrawButtons();
                    break;
                case GameState.Game:
                    break;
                case GameState.Pause:
                    break;
                case GameState.Options:
                    //Draw all the buttons
                    DrawButtons();
                    break;
                case GameState.Keybinds:
                    break;
                case GameState.GameOver:
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        //Methods
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
