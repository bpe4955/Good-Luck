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
        int level;
        MouseState previousMouseState;
        MouseState mouseState;
        KeyboardState kb;
        KeyboardState previousKb;
        //List<Button> buttons;
        Button[][] buttons;

        //Assets
        Texture2D smallSquare;
        Texture2D smallSquareGray;
        Texture2D buttonClick;
        Texture2D buttonImage;
        Texture2D menuItemTextures;
        Texture2D playerTexture;
        Texture2D enemyTexture;
        Texture2D wallTexture;
        Texture2D bulletTexture;
        Texture2D[] health;

        Rectangle playerRect;
        Rectangle enemyRect;
        Rectangle wallRect;
        Player player;
        Enemy enemy;
        Wall wall;

        List<Bullet> bullets;

        //This will hold the backdrop, pause, key box
        //mouse image, crossbones, and skull
        //textures in that order
        Texture2D[] menuItems;
        SpriteFont MetalManiaButtons;
        SpriteFont MetalManiaTitle;
        SpriteFont MetalManiaNormal;
        SpriteFont JelleeRoman20;

        //Misc
        Color lightPurple;
        Color darkPurple;
        public static Keys[] bindings;
        KeybindButton[] keybindButtons;
        EntityManager entityManager;

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
            //buttons = new List<Button>();
            playerRect = new Rectangle(250, 250, 50, 50);
            enemyRect = new Rectangle(50, 100, 100, 100);
            wallRect = new Rectangle(500, 250, 75, 75);
            bullets = new List<Bullet>();
            level = 0;

            buttons = new Button[7][]
            {
                new Button[5],
                new Button[1],
                new Button[1],
                new Button[3],
                new Button[2],
                new Button[1],
                new Button[2]
            };

            lightPurple = new Color(232, 216, 255);
            darkPurple = new Color(21, 0, 51);
            bindings = new Keys[4]
            {
                Keys.W,
                Keys.A,
                Keys.S,
                Keys.D
            };
            keybindButtons = new KeybindButton[4];

            //Name of save file with the number of high scores
            fileName = "../../../HighScores.txt";
            highScoreCount = 1;
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
            menuItemTextures = Content.Load<Texture2D>("MenuImagesV2");
            buttonImage = Content.Load<Texture2D>("Button");
            int height = buttonImage.Height / 3;
            int width = buttonImage.Width;
            smallSquare = buttonImage.GetTexture(new Rectangle(0, height * 2, width, height), GraphicsDevice);
            smallSquareGray = buttonImage.GetTexture(new Rectangle(0, height * 1, width, height), GraphicsDevice);
            buttonClick = buttonImage.GetTexture(new Rectangle(0, 0, width, height), GraphicsDevice);

            menuItems = new Texture2D[6]
            {
                menuItemTextures.GetTexture(new Rectangle(0, 0, 800, 480), GraphicsDevice),
                menuItemTextures.GetTexture(new Rectangle(805, 0, 59, 56), GraphicsDevice),
                menuItemTextures.GetTexture(new Rectangle(805, 61, 50, 50), GraphicsDevice),
                menuItemTextures.GetTexture(new Rectangle(805, 116, 84, 132), GraphicsDevice),
                menuItemTextures.GetTexture(new Rectangle(300, 485, 272, 76), GraphicsDevice),
                menuItemTextures.GetTexture(new Rectangle(577, 485, 52, 63), GraphicsDevice)
            };

            Texture2D healthTexture = Content.Load<Texture2D>("HealthSpriteSheet");
            health = new Texture2D[11];
            for(int i = 0; i < 11; ++i)
            {
                health[i] = healthTexture.GetTexture(new Rectangle(i * 320, 0, 320, 320), GraphicsDevice);
            }

            MetalManiaButtons = Content.Load<SpriteFont>("MetalManiaButtons");
            MetalManiaTitle = Content.Load<SpriteFont>("MetalManiaTitle");
            MetalManiaNormal = Content.Load<SpriteFont>("MetalManiaNormal");
            JelleeRoman20 = Content.Load<SpriteFont>("JelleeRoman20");
            menuItemTextures = Content.Load<Texture2D>("MenuImages");
            playerTexture = Content.Load<Texture2D>("smallSquare");
            enemyTexture = Content.Load<Texture2D>("BunnyBomb");
            wallTexture = Content.Load<Texture2D>("smallSquareGray");
            
            bulletTexture = Content.Load<Texture2D>("Bullet");

            //Creates the buttons
            width = 198;
            int midX = (_graphics.PreferredBackBufferWidth / 2) - (width / 2);
            height = (int)Math.Round(width * .25f);
            int currentY = 135;
            int spacing = (int)(height * 1.1f);
            {
                buttons[0][0] = new Button(GameState.Game, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);
                currentY += spacing;
                buttons[0][1] = new Button(GameState.Tutorial, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);
                currentY += spacing;
                buttons[0][2] = new Button(GameState.Options, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);
                currentY += spacing;
                buttons[0][3] = new Button(GameState.Credits, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);
                currentY += spacing;
                buttons[0][4] = new Button(GameState.Exit, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);

                currentY = 370;
                buttons[1][0] = new Button(GameState.Title, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);

                buttons[2][0] = new Button(GameState.Title, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);

                buttons[4][1] = new Button(GameState.Title, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);
                buttons[4][0] = new Button(GameState.Keybinds, new Rectangle(midX, currentY - spacing, width, height), smallSquare, smallSquareGray, buttonClick);

                buttons[5][0] = new Button(GameState.Options, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);

                buttons[6][0] = new Button(GameState.Title, new Rectangle(midX, currentY - spacing, width, height), smallSquare, smallSquareGray, buttonClick);
                buttons[6][1] = new Button(GameState.Exit, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);

                currentY = (_graphics.PreferredBackBufferHeight / 2) - spacing;
                buttons[3][0] = new Button(GameState.Game, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);
                currentY += spacing;
                buttons[3][1] = new Button(GameState.Title, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);
                currentY += spacing;
                buttons[3][2] = new Button(GameState.Exit, new Rectangle(midX, currentY, width, height), smallSquare, smallSquareGray, buttonClick);
            }

            spacing = 60;
            int initX = 315;
            int initY = 190;
            for (int i = 0; i < 4; ++i)
            {
                Point pos = new Point(initX, initY + (spacing * (i - 1)));
                keybindButtons[i] = new KeybindButton(menuItems[2], new Color(.5f, .5f, .5f), bindings[i], lightPurple, JelleeRoman20, pos);
            }

            // Entity Loading
            player = new Player(playerRect, playerTexture, 5, 10, 0, 6, 4);
            enemy = new Enemy(enemyRect, enemyTexture, 5, 10, 6, 20);
            wall = new Wall(wallRect, wallTexture);

            entityManager = new EntityManager(player);
            entityManager.Enemies.Add(enemy);
            entityManager.Walls.Add(wall);
        }

        protected override void Update(GameTime gameTime)
        {
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();*/
            //Get input from mouse and keyboard
            mouseState = Mouse.GetState();
            kb = Keyboard.GetState();

            switch (gameState)
            {
                case GameState.Title:
                    CheckButtons(0);
                    break;
                case GameState.Tutorial:
                    CheckButtons(1);
                    break;
                case GameState.Credits:
                    CheckButtons(2);
                    break;
                case GameState.Game:
                    if (SingleKeyPress(Keys.Escape))
                    {
                        gameState = GameState.Pause;
                    }
                    if (SingleMouseClick(MouseButton.Left))
                    {
                        entityManager.Bullets.Add(player.Shoot(mouseState, bulletTexture));
                    }
                    //Loop through every bullet
                    entityManager.UpdateEntities(_graphics, kb);
                    if(entityManager.Enemies.Count == 0)
                    {
                        SaveHighScore();
                        saveData = HighScoreData.LoadHighScores(fileName);
                        gameState = GameState.GameOver;
                    }
                    break;
                case GameState.Pause:
                    if (SingleKeyPress(Keys.Escape))
                    {
                        gameState = GameState.Game;
                    }
                    CheckButtons(3);
                    break;
                case GameState.Options:
                    CheckButtons(4);
                    break;
                case GameState.Keybinds:
                    CheckButtons(5);
                    CheckKeybinds();
                    break;
                case GameState.GameOver:
                    CheckButtons(6);
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
                    DrawButtons(0);
                    DrawTextToButton("Start", buttons[0][0].Rect, 3.5f);
                    DrawTextToButton("Tutorial", buttons[0][1].Rect, 3.5f);
                    DrawTextToButton("Options", buttons[0][2].Rect, 3.5f);
                    DrawTextToButton("Credits", buttons[0][3].Rect, 3.5f);
                    DrawTextToButton("Exit", buttons[0][4].Rect, 3.5f);
                    break;
                case GameState.Tutorial:
                    DisplayBackAndTitle("Controls");

                    //Keys
                    int spacing = 55;
                    int initX = 100;
                    int initY = 200;
                    for (int i = 1; i < 4; ++i)
                    {
                        Point pos = new Point(initX + (spacing * (i - 1)), initY);
                        DrawKeybind(pos, i);
                    }
                    DrawKeybind(new Point(initX + spacing, initY - spacing), 0);

                    _spriteBatch.DrawString(MetalManiaNormal, "To move", new Vector2(initX + (int)(spacing/4), initY + spacing), lightPurple);

                    //Mouse
                    _spriteBatch.Draw(menuItems[3], new Rectangle(400, initY - (int)(spacing / 1.5f), 84, 132), Color.White);
                    _spriteBatch.DrawString(MetalManiaNormal, "Left-Click\nto shoot", new Vector2(525, initY - (int)(spacing / 1.5f)), lightPurple);

                    //Draw all the buttons
                    DrawButtons(1);
                    DrawTextToButton("Back", buttons[1][0].Rect, 2.5f);
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
                    DrawButtons(2);
                    DrawTextToButton("Back", buttons[2][0].Rect, 2.5f);
                    break;

                case GameState.Game:
                    entityManager.Draw(_spriteBatch);
                    DrawHud();
                    break;

                case GameState.Pause:
                    DisplayBackAndTitle("Paused");

                    //Draw pause symbols
                    int widthDisplacement = 150;
                    initY = 65;
                    _spriteBatch.Draw(menuItems[1], new Rectangle(widthDisplacement, initY, menuItems[1].Width, menuItems[1].Height), Color.White);
                    _spriteBatch.Draw(menuItems[1], new Rectangle(_graphics.PreferredBackBufferWidth - widthDisplacement - menuItems[1].Width,
                                                                  initY, menuItems[1].Width, menuItems[1].Height), Color.White);

                    //Draw all the buttons
                    DrawButtons(3);
                    DrawTextToButton("Resume", buttons[3][0].Rect, 3.5f);
                    DrawTextToButton("Menu", buttons[3][1].Rect, 3.5f);
                    DrawTextToButton("Quit", buttons[3][2].Rect, 3.5f);
                    break;

                case GameState.Options:
                    DisplayBackAndTitle("Options");

                    //Draw all the buttons
                    DrawButtons(4);
                    DrawTextToButton("Keybinds", buttons[4][0].Rect, 3.5f);
                    DrawTextToButton("Back", buttons[4][1].Rect, 3.5f);

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
                    for (int i = 0; i < 4; ++i)
                    {
                        keybindButtons[i].Draw(_spriteBatch);
                        _spriteBatch.DrawString(MetalManiaNormal, text[i],
                            new Vector2(keybindButtons[i].Rect.X + 75, keybindButtons[i].Rect.Y), lightPurple);
                    }

                    //Draw all the buttons
                    DrawButtons(5);
                    DrawTextToButton("Back", buttons[5][0].Rect, 3.5f);
                    break;
                case GameState.GameOver:
                    DisplayBackAndTitle("Game Over");

                    //Draw the skulls
                    widthDisplacement = 150;
                    initY = 55;
                    _spriteBatch.Draw(menuItems[5], new Rectangle(widthDisplacement, initY, menuItems[5].Width, menuItems[5].Height), Color.White);
                    _spriteBatch.Draw(menuItems[5], new Rectangle(_graphics.PreferredBackBufferWidth - widthDisplacement - menuItems[5].Width,
                                                                  initY, menuItems[5].Width, menuItems[5].Height), Color.White);

                    //Draw the crossbones
                    int halfWidth = _graphics.PreferredBackBufferWidth / 2;
                    _spriteBatch.Draw(menuItems[4], new Rectangle(
                        halfWidth - (menuItems[4].Width / 2), initY + 55,
                        menuItems[4].Width, menuItems[4].Height), Color.White);

                    //Display the high score and level
                    initY = 200;
                    spacing = 50;
                    Vector2 fontSize;
                    if (saveData.scores.Length > 0)
                    {
                        fontSize = MetalManiaNormal.MeasureString($"Highscore: {saveData.scores[0]}");
                        _spriteBatch.DrawString(MetalManiaNormal, $"Highscore: {saveData.scores[0]}", new Vector2(
                            (int)(halfWidth - (fontSize.X / 2)), initY), lightPurple);
                    }
                    fontSize = MetalManiaNormal.MeasureString($"Level: {saveData.levels[0]}");
                    _spriteBatch.DrawString(MetalManiaNormal, $"Level: {saveData.levels[0]}", new Vector2(
                        (int)(halfWidth - (fontSize.X / 2)), initY + spacing), lightPurple);

                    DrawButtons(6);
                    DrawTextToButton("Menu", buttons[6][0].Rect, 2.5f);
                    DrawTextToButton("Quit", buttons[6][1].Rect, 2.5f);
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        //Methods

        /// <summary>
        /// Draws the game hud
        /// </summary>
        private void DrawHud()
        {
            _spriteBatch.Draw(health[entityManager.Player.MaxHealth - entityManager.Player.Health],
                new Rectangle(_graphics.PreferredBackBufferWidth - 80, 0, 80, 80), Color.White);
            _spriteBatch.DrawString(MetalManiaButtons, $"Level: {level}\nScore: xxx", new Vector2(10, 5), lightPurple);
        }

        /// <summary>
        /// Draws a keybind button with the correct keybinding
        /// </summary>
        /// <param name="pos">Were to put it</param>
        /// <param name="i">What keybind to draw</param>
        private void DrawKeybind(Point pos, int i)
        {
            _spriteBatch.Draw(menuItems[2], new Rectangle(pos.X, pos.Y, 50, 50), Color.White);
            Vector2 fontSize = JelleeRoman20.MeasureString(bindings[i].ToString());
            _spriteBatch.DrawString(JelleeRoman20, bindings[i].ToString(),
                new Vector2(pos.X + (int)(25 - (fontSize.X / 2)),
                            pos.Y + (int)(25 - (fontSize.Y / 2))), lightPurple);
        }
        /// <summary>
        /// Draws text on top of a button
        /// </summary>
        /// <param name="text">What to draw</param>
        /// <param name="buttonRect">Which button to draw on top of</param>
        /// <param name="widthPlacement">Where on the button to put it</param>
        private void DrawTextToButton(string text, Rectangle buttonRect, float widthPlacement)
        {
            _spriteBatch.DrawString(MetalManiaButtons, text,
                                new Vector2((int)(buttonRect.X + buttonRect.Width / widthPlacement),
                                                  buttonRect.Y + buttonRect.Height / 10), darkPurple);
        }
        /// <summary>
        /// Displays the background of the menuscreens and the title at the top
        /// </summary>
        /// <param name="title">The title of that menu screen</param>
        private void DisplayBackAndTitle(string title)
        { 
            _spriteBatch.Draw(menuItems[0], new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
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
        /// <param name="index">Which set of buttons to check</param>
        private void CheckButtons(int index)
        {
            if (SingleMouseClick(MouseButton.Left))
            {
                for (int i = 0; i < buttons[index].Length;)
                {
                    //If a button was clicked, change the gameState to the button's state
                    if (buttons[index][i].Collision(mouseState))
                    {
                        gameState = buttons[index][i].ClickedGetState();
                        return;
                    }
                    else
                    {
                        i++;
                    };
                }
            }
        }

        private void CheckKeybinds()
        {
            if (SingleMouseClick(MouseButton.Left))
            {
                for (int i = 0; i < 4; ++i)
                {
                    if (keybindButtons[i].Collision(mouseState))
                    {
                        keybindButtons[i].Selected = true;
                    }
                }
            }
            else if(Keyboard.GetState().GetPressedKeyCount() > 0)
            {
                for (int i = 0; i < 4; ++i)
                {
                    keybindButtons[i].Rebind();
                    bindings[i] = keybindButtons[i].Key;
                }
            }
        }

        /// <summary>
        /// Draw every button in the buttons list
        /// </summary>
        /// /// <param name="index">Which set of buttons to draw</param>
        private void DrawButtons(int index)
        {
            for(int i = 0; i < buttons[index].Length; ++i)
            {
                //Check for collision to determine for each button if the
                //cursor is hovering over them then draw accordingly
                buttons[index][i].Collision(mouseState);
                buttons[index][i].Draw(_spriteBatch);
            }
        }

        /// <summary>
        /// Draw every bullet in the bullet list
        /// </summary>
        private void DrawBullets()
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(_spriteBatch);
                bullet.Move();
            }
        }

        /// <summary>
        /// Sorts and saves the player's data
        /// </summary>
        private void SaveHighScore()
        {
            //Create the data to save
            HighScoreData data = HighScoreData.LoadHighScores(fileName);
        
            int scoreIndex = -1;
            //Loop through saved data to find where to place new data
            for (int i = 0; i < highScoreCount; i++)
            {
                if (player.TotalScore > data.scores[i])
                {
                    scoreIndex = i;
                    break;
                }
            }
        
            //If new score is found, insert into list
            if (scoreIndex > -1)
            {
                for (int i = highScoreCount - 1; i > scoreIndex; i--)
                {
                    data.scores[i] = data.scores[i - 1];
                    data.levels[i] = data.levels[i - 1];
                }
        
                data.scores[scoreIndex] = player.TotalScore;
                data.levels[scoreIndex] = level;
        
                HighScoreData.SaveHighScores(data, fileName);
            }
        
        }
    }
}
