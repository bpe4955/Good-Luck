// Brian Egan
// 4-6-2022
// Manage all levels on the floor, along with adding them to
// the list and moving on to another floor
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Good_Luck
{
    class LevelManager
    {

        //Fields
        private ContentManager content;
        private EntityManager entityManager;
        private int lastDoorIndex;
        private int level;

        private List<Room> possibleRooms = new List<Room>();
        private List<Room> floorRooms = new List<Room>();
        private List<Room[]> adjacencyList = new List<Room[]>();
        private Room startingRoom;
        private Room currentRoom;

        //Property
        /// <summary>
        /// Get the adjacency list, cannot be set
        /// </summary>
        public List<Room[]> AdjacencyList { get => adjacencyList; }
        /// <summary>
        /// Get the current room, can only be set in this class
        /// </summary>
        internal Room CurrentRoom { get => currentRoom; private set => currentRoom = value; }
        /// <summary>
        /// Get the current level number, can only be set in this class
        /// </summary>
        public int Level { get => level; private set => level = value; }


        //Constructor
        /// <summary>
        /// Give access to the content manager and the entity manager
        /// </summary>
        /// <param name="content">Content Manager</param>
        /// <param name="entityManager">Entity Manager handling all entities</param>
        public LevelManager(ContentManager content, EntityManager entityManager)
        {
            this.entityManager = entityManager;
            this.content = content;
            //Initialize level to 0
            level = 0;
        }





        //Methods
        /// <summary>
        /// Add a room to the graph, connecting it to other verticies through doors/edges
        /// </summary>
        /// <param name="room">The room to try to add to the floor</param>
        public void AddRoom(Room room)
        {
            //Try to add room to starting room
            adjacencyList.Add(new Room[5]);
            if (room.HasBottomDoor && startingRoom.HasTopDoor) // Top Room
            {
                adjacencyList[0][1] = room; // Connect center room to new room
                adjacencyList[^1][0] = room; // Connect new room to itself
                floorRooms.Add(room); // Add to list of rooms on floor
                adjacencyList[^1][3] = adjacencyList[0][0]; // Connect new room to center room
                //Disable doors to avoid overlapping edges
                room.HasBottomDoor = false;
                startingRoom.HasTopDoor = false;
                //Create another list of enemies
                entityManager.Enemies.Add(new List<Enemy>());
            }
            else if (room.HasLeftDoor && startingRoom.HasRightDoor) // Right Room
            {
                adjacencyList[0][2] = room;
                adjacencyList[^1][0] = room;
                floorRooms.Add(room); // Add to list of rooms on floor
                adjacencyList[^1][4] = adjacencyList[0][0];
                room.HasLeftDoor = false;
                startingRoom.HasRightDoor = false;
                //Create another list of enemies
                entityManager.Enemies.Add(new List<Enemy>());
            }
            else if (room.HasTopDoor && startingRoom.HasBottomDoor) // Bottom Room
            {
                adjacencyList[0][3] = room;
                adjacencyList[^1][0] = room;
                floorRooms.Add(room); // Add to list of rooms on floor
                adjacencyList[^1][1] = adjacencyList[0][0];
                room.HasTopDoor = false;
                startingRoom.HasBottomDoor = false;
                //Create another list of enemies
                entityManager.Enemies.Add(new List<Enemy>());
            }
            else if (room.HasRightDoor && startingRoom.HasLeftDoor) // Left Room
            {
                adjacencyList[0][4] = room;
                adjacencyList[^1][0] = room;
                floorRooms.Add(room); // Add to list of rooms on floor
                adjacencyList[^1][2] = adjacencyList[0][0];
                room.HasRightDoor = false;
                startingRoom.HasLeftDoor = false;
                //Create another list of enemies
                entityManager.Enemies.Add(new List<Enemy>());
            }

            //If it cannot fit onto the starting room
        }
        /// <summary>
        /// To be called before adding rooms, sets the start room
        /// </summary>
        /// <param name="startRoom">The default "RoomMiddle" room</param>
        public void SetStartRoom(Room startRoom)
        {
            //Clear the list to restart
            adjacencyList.Clear();
            startingRoom = startRoom;
            adjacencyList.Add(new Room[5]);
            // first value in list is the starting room, 
            // and 4 other values in the array are adjacent rooms
            // [1] - Top, [2] - Right, [3] - Bottom, [4] - Left
            adjacencyList[0][0] = startingRoom;
            currentRoom = startingRoom;
            floorRooms.Add(startingRoom);
            //Create another list of enemies
            entityManager.Enemies.Add(new List<Enemy>());
            LoadCurrentRoom();
        }
        /// <summary>
        /// Clear the old room data and load the current room with all its walls
        /// </summary>
        public void LoadCurrentRoom()
        {
            entityManager.Walls.Clear();
            entityManager.Bullets.Clear();
            entityManager.roomIndex = floorRooms.IndexOf(currentRoom);
            GenerateEnemies();
            entityManager.Walls.AddRange(currentRoom.Walls);
        }
        /// <summary>
        /// 
        /// </summary>
        private void GenerateEnemies()
        {
            int roomIndex = floorRooms.IndexOf(currentRoom);
            //Loop through every tile
            foreach (Tile tile in currentRoom.Tiles)
            {
                //See what the property of the tile is
                switch (tile.Property)
                {
                    case TileProperty.Default:
                        break;
                    case TileProperty.OneEnemy:
                        entityManager.Enemies[roomIndex].Add(tile.Rect.CreateBunny());
                        tile.Property = TileProperty.Default;
                        break;
                    case TileProperty.TwoEnemy:
                        entityManager.Enemies[roomIndex].Add(tile.Rect.CreateBunny());
                        entityManager.Enemies[roomIndex].Add(tile.Rect.CreateBunny());
                        tile.Property = TileProperty.Default;
                        break;
                    case TileProperty.OneCollectible:
                        break;
                    case TileProperty.TwoCollectible:
                        break;
                    case TileProperty.PlayerSpawn:
                        break;
                }
            }
        }
        /// <summary>
        /// Takes in a door and changed the current room to what
        /// that door/edge corresponds to
        /// </summary>
        /// <param name="door">The door being collided with</param>
        public void ChangeRoom(Wall door)
        {
            //if door matches top door object, swap current room with top room
            if (currentRoom.DoorLocations["top"] != -1 && currentRoom.Tiles[currentRoom.DoorLocations["top"]].GetWalls().Contains(door))
            {
                currentRoom = adjacencyList[floorRooms.IndexOf(CurrentRoom)][1];
                lastDoorIndex = 3;
            }
            //if door matches right door object, swap current room with right room
            if (currentRoom.DoorLocations["right"] != -1 && currentRoom.Tiles[currentRoom.DoorLocations["right"]].GetWalls().Contains(door))
            {
                currentRoom = adjacencyList[floorRooms.IndexOf(CurrentRoom)][2];
                lastDoorIndex = 4;
            }
            //if door matches bottom door object, swap current room with bottom room
            if (currentRoom.DoorLocations["bottom"] != -1 && currentRoom.Tiles[currentRoom.DoorLocations["bottom"]].GetWalls().Contains(door))
            {
                currentRoom = adjacencyList[floorRooms.IndexOf(CurrentRoom)][3];
                lastDoorIndex = 1;
            }
            //if door matches left door object, swap current room with left room
            if (currentRoom.DoorLocations["left"] != -1 && currentRoom.Tiles[currentRoom.DoorLocations["left"]].GetWalls().Contains(door))
            {
                currentRoom = adjacencyList[floorRooms.IndexOf(CurrentRoom)][4];
                lastDoorIndex = 2;
            }
            LoadCurrentRoom();
        }
        /// <summary>
        /// Move player to the door tile that they enter from
        /// </summary>
        /// <param name="player">The player object</param>
        public void MovePlayerToDoor(Player player)
        {
            Vector2 pos = new Vector2(player.Rect.X, player.Rect.Y);
            //Get the position of the door
            Tile doorTile;
            switch (lastDoorIndex)
            {
                //Go to top door
                case 1:
                    doorTile = currentRoom.Tiles[currentRoom.DoorLocations["top"]];
                    pos.X = doorTile.Rect.X + 10;
                    pos.Y = doorTile.Rect.Y + 10;
                    break;
                //Go to right door
                case 2:
                    doorTile = currentRoom.Tiles[currentRoom.DoorLocations["right"]];
                    pos.X = doorTile.Rect.X + 10;
                    pos.Y = doorTile.Rect.Y + 10;
                    break;
                //Go to bottom door
                case 3:
                    doorTile = currentRoom.Tiles[currentRoom.DoorLocations["bottom"]];
                    pos.X = doorTile.Rect.X + 10;
                    pos.Y = doorTile.Rect.Y + 10;
                    break;
                //Go to left door
                case 4:
                    doorTile = currentRoom.Tiles[currentRoom.DoorLocations["left"]];
                    pos.X = doorTile.Rect.X + 10;
                    pos.Y = doorTile.Rect.Y + 10;
                    break;
            }

            //Update player location
            player.Rect = new Rectangle((int)pos.X, (int)pos.Y, player.Rect.Width, player.Rect.Height);
        }
        /// <summary>
        /// Adds a room to the list of possible rooms
        /// </summary>
        /// <param name="room">The room to add to the list of possible rooms</param>
        public Room LoadRoom(Room room)
        {
            possibleRooms.Add(room);
            return room;
        }

        public void NextLevel()
        {
            level++;
            //Reset floor info
            floorRooms.Clear();
            SetStartRoom(possibleRooms[0]);
            //Add rooms in random order
            Random r = new Random();
            foreach (int i in Enumerable.Range(1, possibleRooms.Count-1).OrderBy(x => r.Next()))
            {
                AddRoom(new Room(possibleRooms[i],content,entityManager));
            }
            currentRoom = floorRooms[0];
        }
    }

}

