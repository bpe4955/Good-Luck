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
        private List<Wall> walls = new List<Wall>();
        private List<Tile> tiles;
        private Dictionary<string, int> doorLocations;

        //Connecting fields
        private bool hasTopDoor;
        private bool hasLeftDoor;
        private bool hasBottomDoor;
        private bool hasRightDoor;

        //Parameters
        /// <summary>
        /// Get and set whether the door has an viable top-door
        /// </summary>
        public bool HasTopDoor { get => hasTopDoor; set => hasTopDoor = value; }
        /// <summary>
        /// Get and set whether the door has an viable left-door
        /// </summary>
        public bool HasLeftDoor { get => hasLeftDoor; set => hasLeftDoor = value; }
        /// <summary>
        /// Get and set whether the door has an viable bottom-door
        /// </summary>
        public bool HasBottomDoor { get => hasBottomDoor; set => hasBottomDoor = value; }
        /// <summary>
        /// Get and set whether the door has an viable right-door
        /// </summary>
        public bool HasRightDoor { get => hasRightDoor; set => hasRightDoor = value; }
        /// <summary>
        /// Get the list of walls in the room
        /// </summary>
        public List<Wall> Walls { get => walls; }
        /// <summary>
        /// Get the dictionary of door locations
        /// </summary>
        public Dictionary<string, int> DoorLocations { get => doorLocations; }
        /// <summary>
        /// Get the list of tiles in the room
        /// </summary>
        internal List<Tile> Tiles { get => tiles; }

        //constructor
        public Room(string filename, ContentManager content, EntityManager entityManager)
        {
            //Will need to know where the doors are in order to connect rooms (will be first line of external tool)
            hasTopDoor = false;
            hasBottomDoor = false;
            hasLeftDoor = false;
            hasRightDoor = false;
            doorLocations = new Dictionary<string, int>();
            tiles = new List<Tile>();
            //Loading in tiles
            StreamReader input = null;
            try
            {
                input = new StreamReader(filename);
                string line = null;
                //Get the door information
                line = input.ReadLine();
                int doorIndex;
                Int32.TryParse(line, out doorIndex);
                doorLocations.Add("top", doorIndex);
                line = input.ReadLine();
                doorIndex = Int32.Parse(line);
                doorLocations.Add("left", doorIndex);
                line = input.ReadLine();
                doorIndex = Int32.Parse(line);
                doorLocations.Add("right", doorIndex);
                line = input.ReadLine();
                doorIndex = Int32.Parse(line);
                doorLocations.Add("bottom", doorIndex);


                //loop through each line in the file 
                int y = 0;
                while ((line = input.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    //Go through the line to generate each tile in the row
                    for (int x = 0; x < data.Length; x++)
                    {
                        tiles.Add(new Tile(data[x], content, new Rectangle((x * 80) * Game1.screenScale, (y * 80) * Game1.screenScale, 80 * Game1.screenScale, 80 * Game1.screenScale)));
                        walls.AddRange(tiles[y*10+x].GetWalls());
                    }
                    //increment the collumn spacing
                    y++;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading file: " + e.Message);
                System.Diagnostics.Debug.WriteLine("Error reading file " + filename);
            }
            finally
            {
                //If the file was opened, close it
                if (input != null)
                {
                    input.Close();
                }
            }
            CheckForDoors();

        }
        /// <summary>
        /// Creates a copy of a room
        /// </summary>
        /// <param name="copy">The room being copied</param>
        /// <param name="content">Content Manager</param>
        /// <param name="entityManager">Entity Manager of the game</param>
        public Room(Room copy, ContentManager content, EntityManager entityManager)
        {
            this.hasBottomDoor = copy.hasBottomDoor;
            this.hasTopDoor = copy.hasTopDoor;
            this.hasLeftDoor = copy.hasLeftDoor;
            this.hasRightDoor = copy.hasRightDoor;
            doorLocations = copy.doorLocations;
            //Copy other tiles
            tiles = new List<Tile>();
            for (int i = 0; i < copy.tiles.Count; i++)
            {
                tiles.Add(new Tile(copy.tiles[i].Code, content, new Rectangle(copy.tiles[i].Rect.X, copy.tiles[i].Rect.Y, copy.tiles[i].Rect.Width, copy.tiles[i].Rect.Height)));
                walls.AddRange(tiles[i].GetWalls());
            }
            CheckForDoors();
        }


        //Methods 
        /// <summary>
        /// Draws the room and all its tiles
        /// </summary>
        /// <param name="sb">Sprite_Batch needed to draw</param>
        public void Draw(SpriteBatch sb)
        {
            foreach (Tile tile in tiles)
            {
                tile.Draw(sb);
            }
        }
        /// <summary>
        /// Check top, bottom, left, and right tiles for doors
        /// and set values accordingly
        /// </summary>
        private void CheckForDoors()
        {
            if (doorLocations["top"] != -1) { HasTopDoor = true; }
            if (doorLocations["left"] != -1) { HasLeftDoor = true; }
            if (doorLocations["right"] != -1) { HasRightDoor = true; }
            if (doorLocations["bottom"] != -1) { HasBottomDoor = true; }
        }

    }
}
