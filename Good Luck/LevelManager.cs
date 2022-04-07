﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        public List<Room[]> AdjacencyList { get => adjacencyList; }
        internal Room CurrentRoom { get => currentRoom; private set => currentRoom = value; }
        public int Level { get => level; private set => level = value; }


        //Constructor
        public LevelManager(ContentManager content, EntityManager entityManager)
        {
            this.entityManager = entityManager;
            this.content = content;
            level = 1;
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
            }
            else if (room.HasLeftDoor && startingRoom.HasRightDoor) // Right Room
            {
                adjacencyList[0][2] = room;
                adjacencyList[^1][0] = room;
                floorRooms.Add(room); // Add to list of rooms on floor
                adjacencyList[^1][4] = adjacencyList[0][0];
                room.HasLeftDoor = false;
                startingRoom.HasRightDoor = false;
            }
            else if (room.HasTopDoor && startingRoom.HasBottomDoor) // Bottom Room
            {
                adjacencyList[0][3] = room;
                adjacencyList[^1][0] = room;
                floorRooms.Add(room); // Add to list of rooms on floor
                adjacencyList[^1][1] = adjacencyList[0][0];
                room.HasTopDoor = false;
                startingRoom.HasBottomDoor = false;
            }
            else if (room.HasRightDoor && startingRoom.HasLeftDoor) // Left Room
            {
                adjacencyList[0][4] = room;
                adjacencyList[^1][0] = room;
                floorRooms.Add(room); // Add to list of rooms on floor
                adjacencyList[^1][2] = adjacencyList[0][0];
                room.HasRightDoor = false;
                startingRoom.HasLeftDoor = false;
            }

            //If it cannot fit onto the starting room
        }
        /// <summary>
        /// To be called before adding rooms, sets the start room
        /// </summary>
        /// <param name="startRoom">The default "RoomMiddle" room</param>
        public void SetStartRoom(Room startRoom)
        {
            startingRoom = startRoom;
            adjacencyList.Add(new Room[5]);
            // first value in list is the starting room, 
            // and 4 other values in the array are adjacent rooms
            // [1] - Top, [2] - Right, [3] - Bottom, [4] - Left
            adjacencyList[0][0] = startingRoom;
            currentRoom = startingRoom;
            floorRooms.Add(startingRoom);
            LoadCurrentRoom();
        }
        /// <summary>
        /// Clear the old room data and load the current room with all its walls
        /// </summary>
        public void LoadCurrentRoom()
        {
            entityManager.Walls.Clear();
            entityManager.Walls.AddRange(currentRoom.Walls);
        }
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

        public bool LoadAllRooms(string filename)
        {
            //loop through every room file and add it to possible rooms
            return false;
        }
    }

}

