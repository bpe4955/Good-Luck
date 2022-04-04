// Brian Egan
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
        private int wallThickness = 7;

        private Wall topWall;
        private Wall sideWall;


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
        /// Get the tile's property but cannot set it
        /// May want to change it so its property does its effect for one frame
        /// then gets set to default
        /// </summary>
        public TileProperty Property { get => property; private set => property = value; }

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
        public Tile(string code, ContentManager content, EntityManager entityManager, Rectangle rect)
        {
            this.rect = rect;
            LoadTexture(code.Substring(0, 2), content);
            LoadProperty(code.Substring(2, 2));
            GenerateWalls(entityManager);
            
        }

        //Methods
        /// <summary>
        /// Draws the tile with its texture in its rectangle
        /// </summary>
        /// <param name="sb">The SpriteBatch needed to draw the tile</param>
        public void Draw(SpriteBatch sb)
        {
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
                    texture = content.Load<Texture2D>("tile01");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y, rect.Width, wallThickness)); //Top Wall
                    sideWall = new Wall(new Rectangle(rect.X, rect.Y, wallThickness, rect.Height)); //Left Wall
                    break;
                //Top-Center
                case ("02"):
                    texture = content.Load<Texture2D>("tile02");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y, rect.Width, wallThickness)); //Top Wall
                    break;
                //Top-Right
                case ("03"):
                    texture = content.Load<Texture2D>("tile03");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y, rect.Width, wallThickness)); //Top Wall
                    sideWall = new Wall(new Rectangle(rect.X + rect.Width - wallThickness, rect.Y, wallThickness, rect.Height)); //Right Wall
                    break;
                //Left
                case ("04"):
                    texture = content.Load<Texture2D>("tile04");
                    sideWall = new Wall(new Rectangle(rect.X, rect.Y, wallThickness, rect.Height)); //Left Wall
                    break;
                //Center
                case ("05"):
                    texture = content.Load<Texture2D>("tile05");
                    break;
                //Right
                case ("06"):
                    texture = content.Load<Texture2D>("tile06");
                    sideWall = new Wall(new Rectangle(rect.X + rect.Width - wallThickness, rect.Y, wallThickness, rect.Height)); //Right Wall
                    break;
                //Bottom-Left
                case ("07"):
                    texture = content.Load<Texture2D>("tile07");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y+rect.Height-wallThickness, rect.Width, wallThickness)); //Bottom Wall
                    sideWall = new Wall(new Rectangle(rect.X, rect.Y, wallThickness, rect.Height)); //Left Wall
                    break;
                //Bottom-Center
                case ("08"):
                    texture = content.Load<Texture2D>("tile08");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y + rect.Height - wallThickness, rect.Width, wallThickness)); //Bottom Wall
                    break;
                //Bottom-Right
                case ("09"):
                    texture = content.Load<Texture2D>("tile09");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y + rect.Height - wallThickness, rect.Width, wallThickness)); //Bottom Wall
                    sideWall = new Wall(new Rectangle(rect.X + rect.Width - wallThickness, rect.Y, wallThickness, rect.Height)); //Right Wall
                    break;
                //Top-Door
                case ("10"):
                    texture = content.Load<Texture2D>("tile10");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y, rect.Width, wallThickness)); //Top Wall
                    topWall.IsDoor = true;
                    break;
                //Right-Door
                case ("11"):
                    texture = content.Load<Texture2D>("tile11");
                    sideWall = new Wall(new Rectangle(rect.X + rect.Width - wallThickness, rect.Y, wallThickness, rect.Height)); //Right Wall
                    sideWall.IsDoor = true;
                    break;
                //Bottom-Door
                case ("12"):
                    texture = content.Load<Texture2D>("tile12");
                    topWall = new Wall(new Rectangle(rect.X, rect.Y + rect.Height - wallThickness, rect.Width, wallThickness)); //Bottom Wall
                    topWall.IsDoor = true;
                    break;
                //Left-Door
                case ("13"):
                    texture = content.Load<Texture2D>("tile13");
                    sideWall = new Wall(new Rectangle(rect.X, rect.Y, wallThickness, rect.Height)); //Left Wall
                    sideWall.IsDoor = true;
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
        /// Give the wall data to the EntityManager to allow for collision
        /// </summary>
        /// <param name="entityManager">The class that handles entity collisions</param>
        private void GenerateWalls(EntityManager entityManager)
        {
            if (topWall != null) { entityManager.Walls.Add(topWall); }
            if (sideWall != null) { entityManager.Walls.Add(sideWall); }
        }

    }
}
