﻿// Brian Egan
// 3-30-2022
// Tile with texture, rectangle, and property that will be contained in a room
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Good_Luck
{
    /// <summary>
    /// Enum to determine what spawns on the tile
    /// </summary>
    enum TileProperty
    {
        Default,
        OneEnemy,
        TwoEnemy,
        OneCollectible,
        TwoCollectible,
        PlayerSpawn
    }

    class Tile
    {
        //Fields
        private Texture2D texture;
        private Rectangle rect;
        private TileProperty property;
        private int wallThickness = 7*Game1.screenScale;

        private Wall topWall;
        private Wall sideWall;
        private Wall centerWall;


        //Properties
        /// <summary>
        /// Get the tile's texture but cannot set it
        /// </summary>
        public Texture2D Texture { get => texture; private set => texture = value; }
        /// <summary>
        /// Get the tile's rectangle but cannot set it
        /// </summary>
        public Rectangle Rect { get => rect; private set => rect = value; }
        /// <summary>
        /// Get and set the tile's property
        /// </summary>
        public TileProperty Property { get => property; set => property = value; }
        /// <summary>
        /// Get whether the tile has a door or not
        /// </summary>
        public bool HasDoor { get; private set; }
        /// <summary>
        /// Get the tile's code
        /// </summary>
        public string Code { get; }

        //Constructor
        public Tile(Texture2D texture, Rectangle rect)
        {
            this.texture = texture;
            this.rect = rect;
            this.property = TileProperty.Default;
        }
        /// <summary>
        /// Constructor used for loading in a tile from a code
        /// </summary>
        /// <param name="code">The tile's code from the external tool</param>
        /// <param name="content">The manager needed to load content</param>
        /// <param name="rect">The position and size of the tile</param>
        public Tile(string code, ContentManager content, Rectangle rect)
        {
            Code = code;
            this.rect = rect;
            HasDoor = false;
            LoadTexture(code.Substring(0, 2), content);
            LoadProperty(code.Substring(2, 2));
        }

        //Methods
        /// <summary>
        /// Draws the tile with its texture in its rectangle
        /// </summary>
        /// <param name="sb">The SpriteBatch needed to draw the tile</param>
        public void Draw(SpriteBatch sb)
        {
            if(texture != null)
                sb.Draw(texture, rect, Color.White);
        }
        /// <summary>
        /// Loads in the texture for the tile based on the first two character's of its code
        /// Also generate walls where necessary
        /// </summary>
        /// <param name="prefix">the first two character's of its code</param>
        /// <param name="content">The manager needed to load content</param>
        public void LoadTexture(string prefix, ContentManager content)
        {
            switch (prefix)
            {
                //Top-Left
                case ("01"):
                    texture = content.Load<Texture2D>("Tiles/tile01");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y, rect.Width, wallThickness)); //Top Wall
                    sideWall = new Wall(new Rectangle(rect.X, rect.Y, wallThickness, rect.Height)); //Left Wall
                    break;
                //Top-Center
                case ("02"):
                    texture = content.Load<Texture2D>("Tiles/tile02");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y, rect.Width, wallThickness)); //Top Wall
                    break;
                //Top-Right
                case ("03"):
                    texture = content.Load<Texture2D>("Tiles/tile03");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y, rect.Width, wallThickness)); //Top Wall
                    sideWall = new Wall(new Rectangle(rect.X + rect.Width - wallThickness, rect.Y, wallThickness, rect.Height)); //Right Wall
                    break;
                //Left
                case ("04"):
                    texture = content.Load<Texture2D>("Tiles/tile04");
                    sideWall = new Wall(new Rectangle(rect.X, rect.Y, wallThickness, rect.Height)); //Left Wall
                    break;
                //Center
                case ("05"):
                    texture = content.Load<Texture2D>("Tiles/tile05");
                    break;
                //Right
                case ("06"):
                    texture = content.Load<Texture2D>("Tiles/tile06");
                    sideWall = new Wall(new Rectangle(rect.X + rect.Width - wallThickness, rect.Y, wallThickness, rect.Height)); //Right Wall
                    break;
                //Bottom-Left
                case ("07"):
                    texture = content.Load<Texture2D>("Tiles/tile07");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y+rect.Height-wallThickness, rect.Width, wallThickness)); //Bottom Wall
                    sideWall = new Wall(new Rectangle(rect.X, rect.Y, wallThickness, rect.Height)); //Left Wall
                    break;
                //Bottom-Center
                case ("08"):
                    texture = content.Load<Texture2D>("Tiles/tile08");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y + rect.Height - wallThickness, rect.Width, wallThickness)); //Bottom Wall
                    break;
                //Bottom-Right
                case ("09"):
                    texture = content.Load<Texture2D>("Tiles/tile09");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y + rect.Height - wallThickness, rect.Width, wallThickness)); //Bottom Wall
                    sideWall = new Wall(new Rectangle(rect.X + rect.Width - wallThickness, rect.Y, wallThickness, rect.Height)); //Right Wall
                    break;
                //Top-Door
                case ("10"):
                    texture = content.Load<Texture2D>("Tiles/tile10");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y, rect.Width, wallThickness)); //Top Wall
                    topWall.IsDoor = true;
                    HasDoor = true;
                    break;
                //Right-Door
                case ("11"):
                    texture = content.Load<Texture2D>("Tiles/tile11");
                    sideWall = new Wall(new Rectangle(rect.X + rect.Width - wallThickness, rect.Y, wallThickness, rect.Height)); //Right Wall
                    sideWall.IsDoor = true;
                    HasDoor = true;
                    break;
                //Bottom-Door
                case ("12"):
                    texture = content.Load<Texture2D>("Tiles/tile12");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y + rect.Height - wallThickness, rect.Width, wallThickness)); //Bottom Wall
                    topWall.IsDoor = true;
                    HasDoor = true;
                    break;
                //Left-Door
                case ("13"):
                    texture = content.Load<Texture2D>("Tiles/tile13");
                    sideWall = new Wall(new Rectangle(rect.X, rect.Y, wallThickness, rect.Height)); //Left Wall
                    sideWall.IsDoor = true;
                    HasDoor = true;
                    break;
                //Center-Wall
                case ("14"):
                    texture = content.Load<Texture2D>("Tiles/tile14");
                    centerWall = new Wall(new Rectangle(rect.X+wallThickness*3, rect.Y+ wallThickness * 3, (int)(wallThickness*5.5), (int)(wallThickness * 5.5))); //center Wall
                    break;
                //Whole-Wall
                case ("15"):
                    texture = content.Load<Texture2D>("Tiles/tile15");
                    centerWall = new Wall(new Rectangle(rect.X, rect.Y, rect.Width, rect.Height)); //full Wall
                    break;
                //Stair
                case ("16"):
                    texture = content.Load<Texture2D>("Tiles/tile16");
                    centerWall = new Wall(new Rectangle(rect.X, rect.Y, rect.Width, rect.Height)); //full Wall
                    centerWall.IsStair = true;
                    break;
            }
        }
        /// <summary>
        /// Loads in the property for the tile based on the last two character's of its code
        /// </summary>
        /// <param name="suffix">the last two character's of its code</param>
        public void LoadProperty(string suffix)
        {
            switch (suffix)
            {
                case ("e1"):
                    property = TileProperty.OneEnemy;
                    break;
                case ("e2"):
                    property = TileProperty.TwoEnemy;
                    break;
                case ("c1"):
                    property = TileProperty.OneCollectible;
                    break;
                case ("c2"):
                    property = TileProperty.TwoCollectible;
                    break;
                case ("pp"):
                    property = TileProperty.PlayerSpawn;
                    break;
                case ("ff"):
                    property = TileProperty.Default;
                    break;
            }
        }
        /// <summary>
        /// returns a list of all the tile's walls
        /// </summary>
        /// <returns>a list of all the tile's walls</returns>
        public List<Wall> GetWalls()
        {
            List<Wall> walls = new List<Wall>();
            if (topWall != null) { walls.Add(topWall); }
            if (sideWall != null) { walls.Add(sideWall); }
            if (centerWall != null) { walls.Add(centerWall); }
            return walls;
        }

    }
}
