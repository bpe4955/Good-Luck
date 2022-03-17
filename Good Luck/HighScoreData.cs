using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using System.IO;

namespace Good_Luck
{
    [Serializable]
    public struct HighScoreData
    {
        //Fields
        public int[] scores;
        public int[] levels;
        public int count;

        //Constructor
        /// <summary>
        /// Creates lists to store scores and furthest floors
        /// </summary>
        /// <param name="count">The amount of scores to store</param>
        public HighScoreData(int count)
        {
            scores = new int[count];
            levels = new int[count];
            this.count = count;
        }

        //Methods
        /// <summary>
        /// Save the high score data using an XmlSerializer
        /// </summary>
        /// <param name="data">The struct containing data for scores and furthest floors</param>
        /// <param name="fileName">The name of the file to write to or create</param>
        public static void SaveHighScores(HighScoreData data, string fileName)
        {
            //Open the file, creating it if necessary
            FileStream stream = File.Open(fileName, FileMode.OpenOrCreate);
            try
            {
                //Convert object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                //Close the file
                stream.Close();
            }
        }

        /// <summary>
        /// Load the high score data using an XmlSerializer
        /// </summary>
        /// <param name="fileName">The name of the file to read from or create</param>
        /// <returns>The struct containing data for scores and furthest floors</returns>
        public static HighScoreData LoadHighScores(string fileName)
        {
            HighScoreData data;

            //Open the file
            FileStream stream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Read);
            try
            {
                //Read data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                data = (HighScoreData)serializer.Deserialize(stream);
            }
            finally
            {
                //Close the file
                stream.Close();
            }

            return data;
        }
    }
}
