using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Good_Luck
{
    class LevelManager
    {
        //Fields
        private ContentManager content;
        private EntityManager entityManager;

        private List<Room> possibleRooms = new List<Room>();
        private List<Room[]> adjacencyList = new List<Room[]>();
        private Room startingRoom;
        private Room currentRoom;

        //Property
        public List<Room[]> AdjacencyList { get => adjacencyList; }


        //Constructor
        public LevelManager(ContentManager content, EntityManager entityManager)
        {
            this.entityManager = entityManager;
            this.content = content;
        }





        //Methods
        /// <summary>
        /// Add a room to the graph, connecting it to other verticies through doors/edges
        /// </summary>
        /// <param name="room">The room to try to add to the floor</param>
        public void AddRoom(Room room)
        {
            //Start off the graph with starting room
            if(adjacencyList.Count == 0)
            {
                startingRoom = new Room("Content/MiddleRoom.txt", content, entityManager);
                adjacencyList.Add(new Room[5]);
                // first value in list is the starting room, 
                // and 4 other values in the array are adjacent rooms
                // [1] - Top, [2] - Right, [3] - Bottom, [4] - Left
                adjacencyList[0][0] = startingRoom;
            }

            //Try to add room to starting room
            adjacencyList.Add(new Room[5]);
            if(room.HasBottomDoor && startingRoom.HasTopDoor)
            {
                adjacencyList[0][1] = room;
                adjacencyList[1][0] = room;
                adjacencyList[1][3] = adjacencyList[0][0];
                room.HasBottomDoor = false;
                startingRoom.HasTopDoor = false;
            }
            else if (room.HasLeftDoor && startingRoom.HasRightDoor)
            {
                adjacencyList[0][2] = room;
                adjacencyList[1][0] = room;
                adjacencyList[1][4] = adjacencyList[0][0];
                room.HasLeftDoor = false;
                startingRoom.HasRightDoor = false;
            }
            else if (room.HasTopDoor && startingRoom.HasBottomDoor)
            {
                adjacencyList[0][3] = room;
                adjacencyList[1][0] = room;
                adjacencyList[1][1] = adjacencyList[0][0];
                room.HasTopDoor = false;
                startingRoom.HasBottomDoor = false;
            }
            else if (room.HasRightDoor && startingRoom.HasLeftDoor)
            {
                adjacencyList[0][4] = room;
                adjacencyList[1][2] = room;
                room.HasRightDoor = false;
                startingRoom.HasLeftDoor = false;
            }

            //If it cannot fit onto the starting room
        }

        //If no enemies, enable doors
        public void UpdateRooms()
        {

        }

        public bool LoadAllRooms(string filename)
        {
            //loop through every room file and add it to possible rooms
            return false;
        }
    }

}

