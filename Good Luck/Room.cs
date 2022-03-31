using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Good_Luck
{
    class Room
    {
        //fields
        private int width;
        private int height;
        private List<Wall> walls;
        private List<Tile> tiles;
        private List<Wall> doors;

        //constructor
        public Room(string filename, ContentManager content)
        {
            //Will need to know where the doors are in order to connect rooms
            tiles = new List<Tile>();
            //Loading in tiles
            StreamReader input = null;
            try
            {
                input = new StreamReader(filename);
                string line = null;
                ////Get the height and width
                //if ((line = input.ReadLine()) != null)
                //{
                //    string[] data = line.Split(',');
                //    width = Int32.Parse(data[0]);
                //    height = Int32.Parse(data[1]);
                //}
                //loop through each line in the file 
                int y = 0;
                while ((line = input.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    //Go through the line to generate each tile in the row
                    for (int x = 0; x < data.Length; x++)
                    {
                        tiles.Add(new Tile(data[x], content, new Rectangle((x*50),(y*50),50,50)));
                    }
                    //increment the collumn spacing
                    y++;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading file: " + e.Message);
            }
            finally
            {
                //If the file was opened, close it
                if (input != null)
                {
                    input.Close();
                }
            }
        }

        //Methods 
        public void Draw(SpriteBatch sb)
        {
            foreach (Tile tile in tiles)
            {
                tile.Draw(sb);
            }
        }

    }
}
