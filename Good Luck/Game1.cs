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
        //List<Button> buttons;
        Button[][] buttons;

        //Assets
        Texture2D buttonDefault;
        Texture2D buttonHover;
        Texture2D buttonClick;
        Texture2D buttonImage;
        Texture2D menuItemTextures;
        Texture2D playerTexture;
        public static Texture2D collectibleTexture;
        public static Texture2D enemyTexture;
        public static Texture2D sadEnemy;
        public static Texture2D shooterEnemyTexture;
        public static Texture2D carrotTexture;
        Texture2D bulletTexture;
        Texture2D[] health;

        Rectangle playerRect;
        public static Rectangle enemyRect;
        Player player;
        public static Texture2D healthCollectibleTexture;

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
        public static int screenScale;

        //Save File Fields
        private string saveFileName;
        private int highScoreCount;
        HighScoreData saveData;

        //Testing room loading
        LevelManager levelManager;
        Room startingRoom;

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
            screenScale = 2;
            _graphics.PreferredBackBufferWidth *= screenScale;
            _graphics.PreferredBackBufferHeight *= screenScale;
            _graphics.ApplyChanges();
            //buttons = new List<Button>();
            playerRect = new Rectangle(250 * screenScale, 250 * screenScale, 50 * screenScale, 50 * screenScale);
            enemyRect = new Rectangle(50 * screenScale, 100 * screenScale, 100 * screenScale, 100 * screenScale);
            bullets = new List<Bullet>();

            buttons = new Button[7][]
           {
                new Button[6],
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
            saveFileName = "../../../HighScores.txt";
            highScoreCount = 1;
            //Check to see if save file exists
            if (!File.Exists(saveFileName))
            {
                //If the file doesn't exist, make a default one
                HighScoreData data = new HighScoreData(highScoreCount);
                for (int i = 0; i < highScoreCount; i++)
                {
                    data.levels[i] = (0);
                    data.scores[i] = (000);
                }

                HighScoreData.SaveHighScores(data, saveFileName);
            }
            //Load in save data from file to variable
            saveData = HighScoreData.LoadHighScores(saveFileName);

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
            buttonDefault = buttonImage.GetTexture(new Rectangle(0, height * 2, width, height), GraphicsDevice);
            buttonHover = buttonImage.GetTexture(new Rectangle(0, height * 1, width, height), GraphicsDevice);
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
            for (int i = 0; i < 11; ++i)
            {
                health[i] = healthTexture.GetTexture(new Rectangle(i * 320, 0, 320, 320), GraphicsDevice);
            }

            MetalManiaButtons = Content.Load<SpriteFont>("MetalManiaButtons");
            MetalManiaTitle = Content.Load<SpriteFont>("MetalManiaTitle");
            MetalManiaNormal = Content.Load<SpriteFont>("MetalManiaNormal");
            JelleeRoman20 = Content.Load<SpriteFont>("JelleeRoman20");
            menuItemTextures = Content.Load<Texture2D>("MenuImages");
            playerTexture = Content.Load<Texture2D>("Player");
            enemyTexture = Content.Load<Texture2D>("BunnyBomb");
            sadEnemy = Content.Load<Texture2D>("SadBunny");
            collectibleTexture = Content.Load<Texture2D>("Collectible");
            shooterEnemyTexture = Content.Load<Texture2D>("BunnyBazooka");
            carrotTexture = Content.Load<Texture2D>("Carrot");
            healthCollectibleTexture = Content.Load<Texture2D>("HealthCollectible");

            bulletTexture = Content.Load<Texture2D>("Bullet");

            //Creates the buttons
            width = 190 * screenScale;
            int midX = (_graphics.PreferredBackBufferWidth / 2) - (width / 2);
            height = (int)Math.Round(width * .25f);
            int currentY = 115 * screenScale;
            int spacing = (int)(height * 1.1f);
            {
                buttons[0][0] = new Button(GameState.Game, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);
                buttons[0][0].buttonClickAction += Restart;
                currentY += spacing;
                buttons[0][1] = new Button(GameState.Tutorial, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);
                currentY += spacing;
                buttons[0][2] = new Button(GameState.Options, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);
                currentY += spacing;
                buttons[0][3] = new Button(GameState.Credits, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);
                currentY += spacing;
                buttons[0][4] = new Button(GameState.Exit, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);
                currentY += spacing;
                buttons[0][5] = new Button(GameState.Game, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);
                buttons[0][5].buttonClickAction += Restart;

                currentY = 370 * screenScale;
                buttons[1][0] = new Button(GameState.Title, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);

                buttons[2][0] = new Button(GameState.Title, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);

                buttons[4][1] = new Button(GameState.Title, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);
                buttons[4][0] = new Button(GameState.Keybinds, new Rectangle(midX, currentY - spacing, width, height), buttonDefault, buttonHover, buttonClick);

                buttons[5][0] = new Button(GameState.Options, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);

                buttons[6][0] = new Button(GameState.Title, new Rectangle(midX, currentY - spacing, width, height), buttonDefault, buttonHover, buttonClick);
                buttons[6][1] = new Button(GameState.Exit, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);

                currentY = (_graphics.PreferredBackBufferHeight / 2) - spacing;
                buttons[3][0] = new Button(GameState.Game, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);
                currentY += spacing;
                buttons[3][1] = new Button(GameState.Title, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);
                currentY += spacing;
                buttons[3][2] = new Button(GameState.Exit, new Rectangle(midX, currentY, width, height), buttonDefault, buttonHover, buttonClick);
            }

            spacing = 60 * screenScale;
            int initX = 315 * screenScale;
            int initY = 190 * screenScale;
            for (int i = 0; i < 4; ++i)
            {
                Point pos = new Point(initX, initY + (spacing * (i - 1)));
                keybindButtons[i] = new KeybindButton(menuItems[2], new Color(.5f, .5f, .5f), bindings[i], lightPurple, JelleeRoman20, pos);
            }

            // Entity Loading
            player = new Player(playerRect, playerTexture, 5 * screenScale, 10, 0, 6, 4);
            // enemy = Extensions.CreateBunny();

            entityManager = new EntityManager(player);
            //entityManager.Enemies[].Add(enemy);
            //entityManager.Walls.Add(wall);

            //Testing room loading 
            levelManager = new LevelManager(Content, entityManager);

            //Setting up starting room
            startingRoom = levelManager.LoadRoom(new Room("Content/RoomMiddle.Level", Content, entityManager));
            levelManager.SetStartRoom(startingRoom);

            //TOP ROOM LOADING
            levelManager.LoadRoom(new Room("Content/RoomTop.level", Content, entityManager));
            levelManager.LoadRoom(new Room("Content/SplitInTwoTopRoom.level", Content, entityManager));
            levelManager.LoadRoom(new Room("Content/HealthCollectiblesTopRoom.level", Content, entityManager));
            //levelManager.LoadRoom(new Room("Content/SmallerBlockWallTopRoom.level", Content, entityManager));

            //RIGHT ROOM LOADING
            levelManager.LoadRoom(new Room("Content/RoomRight.level", Content, entityManager));
            //levelManager.LoadRoom(new Room("Content/BlockedOffRightRoom.level", Content, entityManager));
            levelManager.LoadRoom(new Room("Content/HellRightRoom.level", Content, entityManager));
            levelManager.LoadRoom(new Room("Content/SpiralRightRoom.level", Content, entityManager));

            //BOTTOM ROOM LOADING
            levelManager.LoadRoom(new Room("Content/RoomBottom.Level", Content, entityManager));
            levelManager.LoadRoom(new Room("Content/ChungusFunnyBottomRoom.Level", Content, entityManager));

            //LEFT ROOM LOADING
            levelManager.LoadRoom(new Room("Content/RoomLeft.Level", Content, entityManager));
            levelManager.LoadRoom(new Room("Content/CShapeLeftRoom.level", Content, entityManager));
            //levelManager.LoadRoom(new Room("Content/BlockedOffLeftRoom.Level", Content, entityManager));


            //Hooking up events
            entityManager.DoorCollided += levelManager.ChangeRoom;
            entityManager.PlayerInWall += levelManager.MovePlayerToDoor;

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

                //
                // GAME
                //
                case GameState.Game:
                    if (SingleKeyPress(Keys.Escape))
                    {
                        gameState = GameState.Pause;
                    }
                    if (SingleMouseClick(MouseButton.Left))
                    {
                        entityManager.Bullets.Add(player.Shoot(mouseState, bulletTexture));
                    }
                    
                    //If all rooms visited, go to next level
                    if (levelManager.goToNextLevelList.Count == 5 && levelManager.CurrentRoom == startingRoom)
                    {
                        NextLevel();
                    }

                    //Loop through every bullet
                    entityManager.UpdateEntities(_graphics, kb, bulletTexture, gameTime, mouseState);
                    if (player.Health <= 0)
                    {
                        SaveHighScore();
                        saveData = HighScoreData.LoadHighScores(saveFileName);
                        gameState = GameState.GameOver;
                    }
                    //This exists entirely to have a break point and debug
                    if (kb.IsKeyDown(Keys.Space))
                    {
                        break;
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
                    DrawTextToButton("God Mode", buttons[0][5].Rect, 3.5f);
                    break;

                case GameState.Tutorial:
                    DisplayBackAndTitle("Controls");
                    //Keys
                    int spacing = 55 * screenScale;
                    int initX = 100 * screenScale;
                    int initY = 200 * screenScale;
                    for (int i = 1; i < 4; ++i)
                    {
                        Point pos = new Point(initX + (spacing * (i - 1)), initY);
                        DrawKeybind(pos, i);
                    }
                    DrawKeybind(new Point(initX + spacing, initY - spacing), 0);
                    _spriteBatch.DrawString(MetalManiaNormal, "To move", new Vector2(initX + (int)(spacing / 4), initY + spacing), lightPurple);
                    //Mouse
                    _spriteBatch.Draw(menuItems[3], new Rectangle(400 * screenScale, initY - (int)(spacing / 1.5f), 84 * screenScale, 132 * screenScale), Color.White);
                    _spriteBatch.DrawString(MetalManiaNormal, "Left-Click\nto shoot", new Vector2(525 * screenScale, initY - (int)(spacing / 1.5f)), lightPurple);
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
                                                              "   - Michaela Castle\n", new Vector2(100 * screenScale, 100 * screenScale), lightPurple);
                    //Draw all the buttons
                    DrawButtons(2);
                    DrawTextToButton("Back", buttons[2][0].Rect, 2.5f);
                    break;

                //
                // Game
                //
                case GameState.Game:
                    levelManager.CurrentRoom.Draw(_spriteBatch);
                    entityManager.Draw(_spriteBatch);
                    DrawHud();

                    break;

                case GameState.Pause:
                    DisplayBackAndTitle("Paused");
                    //Draw pause symbols
                    int widthDisplacement = 150 * screenScale;
                    initY = 65 * screenScale;
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
                            new Vector2(keybindButtons[i].Rect.X + 75 * screenScale, keybindButtons[i].Rect.Y), lightPurple);
                    }
                    //Draw all the buttons
                    DrawButtons(5);
                    DrawTextToButton("Back", buttons[5][0].Rect, 3.5f);
                    break;

                case GameState.GameOver:
                    DisplayBackAndTitle("Game Over");
                    //Draw the skulls
                    widthDisplacement = 150 * screenScale;
                    initY = 55 * screenScale;
                    _spriteBatch.Draw(menuItems[5], new Rectangle(widthDisplacement, initY, menuItems[5].Width, menuItems[5].Height), Color.White);
                    _spriteBatch.Draw(menuItems[5], new Rectangle(_graphics.PreferredBackBufferWidth - widthDisplacement - menuItems[5].Width,
                                                                  initY, menuItems[5].Width, menuItems[5].Height), Color.White);
                    //Draw the crossbones
                    int halfWidth = _graphics.PreferredBackBufferWidth / 2;
                    _spriteBatch.Draw(menuItems[4], new Rectangle(
                        halfWidth - (menuItems[4].Width / 2), initY + 55 * screenScale,
                        menuItems[4].Width, menuItems[4].Height), Color.White);
                    //Display the high score and level
                    initY = 200 * screenScale;
                    spacing = 50 * screenScale;
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

        //
        //Methods

        /// <summary>
        /// Draws the game hud
        /// </summary>
        private void DrawHud()
        {
            _spriteBatch.Draw(health[entityManager.Player.MaxHealth - entityManager.Player.Health],
                new Rectangle(_graphics.PreferredBackBufferWidth - 60 * screenScale, 0, 60 * screenScale, 60 * screenScale), Color.White);
            _spriteBatch.DrawString(MetalManiaButtons, $"Level: {levelManager.Level}\nScore: {player.TotalScore}", new Vector2(10 * screenScale, 0), lightPurple);
        }

        /// <summary>
        /// Draws a keybind button with the correct keybinding
        /// </summary>
        /// <param name="pos">Were to put it</param>
        /// <param name="i">What keybind to draw</param>
        private void DrawKeybind(Point pos, int i)
        {
            _spriteBatch.Draw(menuItems[2], new Rectangle(pos.X, pos.Y, 50 * screenScale, 50 * screenScale), Color.White);
            Vector2 fontSize = JelleeRoman20.MeasureString(bindings[i].ToString());
            _spriteBatch.DrawString(JelleeRoman20, bindings[i].ToString(),
                new Vector2(pos.X + (int)(25 * screenScale - (fontSize.X / 2)),
                            pos.Y + (int)(25 * screenScale - (fontSize.Y / 2))), lightPurple);
        }
        /// <summary>
        /// Draws text on top of a button
        /// </summary>
        /// <param name="text">What to draw</param>
        /// <param name="buttonRect">Which button to draw on top of</param>
        /// <param name="widthPlacement">Where on the button to put it</param>
        private void DrawTextToButton(string text, Rectangle buttonRect, float widthPlacement)
        {
            Vector2 fontSize = MetalManiaButtons.MeasureString(text);
            _spriteBatch.DrawString(MetalManiaButtons, text,
                                new Vector2((int)(buttonRect.X + buttonRect.Width / widthPlacement),
                                                  buttonRect.Y + (int)((buttonRect.Height / 2f) - (fontSize.Y / 2f))), darkPurple);
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
                (int)((_graphics.PreferredBackBufferWidth / 2) - (titleSize / 2)), 50 * screenScale), lightPurple);
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
                        if (index == 0 && i == 5)
                        {
                            entityManager.Player.DefenseStat = 10000000;
                        }
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
            else if (Keyboard.GetState().GetPressedKeyCount() > 0)
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
            for (int i = 0; i < buttons[index].Length; ++i)
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
            HighScoreData data = HighScoreData.LoadHighScores(saveFileName);

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
                data.levels[scoreIndex] = levelManager.Level;

                HighScoreData.SaveHighScores(data, saveFileName);
            }

        }

        /// <summary>
        /// Sets the Game into a playable state
        /// Resets player health, score, location
        /// Configures room layout generation
        /// </summary>
        private void Restart()
        {
            player.Health = player.MaxHealth;
            player.IsActive = true;
            player.TotalScore = 0;
            player.DefenseStat = 0;
            player.Rect = new Rectangle((400 - 25)*screenScale, (240 - 25) * screenScale, 50*screenScale, 50 * screenScale);


            levelManager.Level = 0;
            levelManager.NextLevel();
        }

        /// <summary>
        /// Sets the Game into a playable state
        /// Configures room layout generation
        /// </summary>
        public void NextLevel()
        {
            player.Rect = new Rectangle((400 - 25) * screenScale, (240 - 25) * screenScale, 50 * screenScale, 50 * screenScale);
            player.TotalScore += 100 * levelManager.Level;

            levelManager.NextLevel();
        }
    }
}
