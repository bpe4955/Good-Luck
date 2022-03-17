using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using System.IO;

namespace Good_Luck
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

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

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        //Methods
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
