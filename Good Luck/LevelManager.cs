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
        private Room startingRoom;

        //Constructor
        public LevelManager(ContentManager content, EntityManager entityManager)
        {
            this.entityManager = entityManager;
            this.content = content;
        }





        //Methods
        public void AddRoom(Room room)
        {
            //Start off the tree
            if(startingRoom == null)
            {
                startingRoom = new Room("Content/StartRoom.txt", content, entityManager);
            }

            //Add room where applicable

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

