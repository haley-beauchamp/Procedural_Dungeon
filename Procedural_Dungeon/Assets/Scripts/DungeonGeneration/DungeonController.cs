using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    [SerializeField] GameObject[] roomPrefabs;
    [SerializeField] int startRoomIndex;
    [SerializeField] int bossRoomIndex;
    [SerializeField] int shopRoomIndex;

    [SerializeField] int minRoomsPerCrawler;
    [SerializeField] int maxRoomsPerCrawler;
    [SerializeField] int numberOfCrawlers;

    private readonly int numberOfSpecialRooms = 2;
    private bool bossRoomGenerated = false;
    private bool shopGenerated = false;

    private GameObject startRoom;
    private List<GameObject> rooms;
    private RoomConfigurationData roomConfigurationData;

   // private List<Vector2> debugTilePositions = new List<Vector2>();

    void Start()
    {
        roomConfigurationData = gameObject.GetComponent<RoomConfigurationData>();
        rooms = new List<GameObject>();
        GenerateDungeon();
    }

    /* Method to start the dungeon generation
     * It creates the start room and adds it to the list of rooms
     * Then, for the number of crawlers, calls the GenerateRooms() method
     * And once all crawlers have finished placing rooms, this opens the walls to create doors where there were connections */
    void GenerateDungeon()
    {
        startRoom = Instantiate(roomPrefabs[startRoomIndex]);
        rooms.Add(startRoom);

        for (int i = 0; i < numberOfCrawlers; i++)
        {
            GenerateRooms();
        }

        foreach (GameObject room in rooms)
        {
            room.GetComponent<RoomController>().CreateDoors();
        }
    }

    /* Method to actually handle the logic for picking and placing rooms, which runs once per crawler
     * It picks a random number in the specified range, then attempts to generate and place that number of rooms
     * For the last few rooms for each crawler, if the shop and boss rooms have not been generated, the method will attempt to place them
     * This ensures there are multiple opportunities for those rooms to be placed
     * If it is not the last few rooms of a crawler or those rooms have already been placed, it picks a random room
     * It determines if the new room will match up with the current room by checking if they have corresponding doors */
    void GenerateRooms()
    {
        int randomRoomCount = Random.Range(minRoomsPerCrawler, maxRoomsPerCrawler + 1);

        GameObject currentRoom = startRoom;
        int nextRoomIndex = 0;

        for (int i = 0; i < randomRoomCount; i++)
        {
            Vector3 potentialPosition = new();
            bool floorsIntersect = true;
            int iterations = 0;
            while (floorsIntersect && (iterations < 10))
            {
                bool canPlaceRoom = false;
                while (!canPlaceRoom)
                { //try to generate special rooms at the end of each crawler, if they haven't been generated yet... but move on if it takes too many iterations
                    if ((i >= (randomRoomCount - numberOfSpecialRooms / 2)) && (!bossRoomGenerated || !shopGenerated) && (iterations < 5))
                    {
                        if (!shopGenerated)
                        {
                            nextRoomIndex = shopRoomIndex;
                        }
                        else
                        {
                            nextRoomIndex = bossRoomIndex;
                        }
                    }
                    else
                    {
                        bool isValidRoom = false;

                        while (!isValidRoom)
                        {
                            int randomRoom = Random.Range(1, roomPrefabs.Length);

                            //if you generate a special room that has already been generated
                            if ((shopGenerated && (randomRoom == shopRoomIndex)) || (bossRoomGenerated && (randomRoom == bossRoomIndex)))
                            {
                                continue;
                            } else if ((nextRoomIndex == bossRoomIndex) && (i == 0)) //if the boss room spawns first
                            {
                                continue;
                            }
                            else
                            {
                                nextRoomIndex = randomRoom; //otherwise add the room
                                isValidRoom = true;
                            }
                        }
                    }
                    canPlaceRoom = roomConfigurationData.CompareRoomDoors(currentRoom, nextRoomIndex);
                }
                potentialPosition = CalculateRoomPosition(currentRoom, nextRoomIndex);
                if (ValidatePosition(potentialPosition, nextRoomIndex))
                {
                    floorsIntersect = false;
                    if (nextRoomIndex == shopRoomIndex) { //only set boss and shop spawn booleans to true if they were successfully placed
                        shopGenerated = true;
                    }
                    if (nextRoomIndex == bossRoomIndex)
                    {
                        bossRoomGenerated = true;
                    }
                }
                iterations++;
            }

            if (!floorsIntersect)
            {
                GameObject nextRoomPrefab = roomPrefabs[nextRoomIndex];
                GameObject nextRoom = Instantiate(nextRoomPrefab, potentialPosition, Quaternion.identity);
                rooms.Add(nextRoom);

                RoomController currentRoomController = currentRoom.GetComponent<RoomController>();
                RoomController nextRoomController = nextRoom.GetComponent<RoomController>();
                string[] roomAttachments = roomConfigurationData.GetRoomAttachments();
                currentRoomController.UpdateAvailableDoors(roomAttachments[0]);
                nextRoomController.UpdateAvailableDoors(roomAttachments[1]);

                currentRoom = nextRoom;
            }
        }
    }

    /* Calculate where the new room would be based on the position of the door in the current room that the next room will be attached to
     * If the door is "horizontal," ie left or right, the position is affected by the width of the current room, and offsets specified for the next room
     * If it is "vertical," ie top or bottom, the position is affected by the height of the current room
     * Each calculation has a 1 added or subtracted to account for overlap */
    private Vector3 CalculateRoomPosition(GameObject currentRoom, int nextRoomIndex)
    {
        RoomController currentRoomController = currentRoom.GetComponent<RoomController>();
        string[] roomAttachment = roomConfigurationData.GetRoomAttachments();
        Vector3 position = Vector3.zero;
        if ((roomAttachment[0] == "LeftDoor") || (roomAttachment[0] == "RightDoor"))
        {
            float roomWidth = currentRoomController.GetRoomWidth();
            if (roomAttachment[0] == "LeftDoor")
            {
                position = new Vector3(-roomWidth + 1, 0, 0);
            }
            else
            {
                position = new Vector3(roomWidth - 1, 0, 0);
            }

            //add the height offset for horizontal placement for the current room, to ensure doors align
            position += new Vector3(0, roomConfigurationData.GetRoomOffset(nextRoomIndex), 0); 
            position -= new Vector3(0, currentRoomController.GetHorizontalPlacementHeightOffset(), 0);
            //subtract the height offset for horizontal placement for the previous room, as shapes like "L" will affect these calculations
        }
        else if ((roomAttachment[0] == "TopDoor") || (roomAttachment[0] == "BottomDoor"))
        {
            float roomHeight = currentRoomController.GetRoomHeight();
            if (roomAttachment[0] == "TopDoor")
            {
                position = new Vector3(0, roomHeight - 1, 0);
            }
            else
            {
                position = new Vector3(0, -roomHeight + 1, 0);
            }
        }

        return currentRoom.transform.position + position;
    }

    /* Method checks if any of the tiles in the new room would collide with existing floor tiles
     * This prevents rooms from overlapping in an unintended manner, which would affect the player's movement 
     * It adds the previously calculated position to the local position of the new tiles to convert it to the intended worldspace
     * It then checks collisions for those tiles against the world space positions of all existing floor tiles */
    private bool ValidatePosition(Vector3 potentialPosition, int nextRoomIndex)
    {
        Vector2 position = new(potentialPosition.x, potentialPosition.y);
        List<Vector2> localTilePositions = roomConfigurationData.GetTilePositions(nextRoomIndex);
        List<Vector2> worldTilePositions = new();

        foreach (var localPosition in localTilePositions)
        {
            Vector2 worldPosition = position + localPosition;
            worldTilePositions.Add(worldPosition);
        }

        HashSet<Vector2> newRoomTileSet = new(worldTilePositions);

        foreach (GameObject room in rooms)
        {
            List<Vector2> existingRoomWorldTiles = room.GetComponent<RoomController>().GetWorldFloorTilePositions();
            foreach (Vector2 tilePosition in existingRoomWorldTiles)
            {
                //debugTilePositions.Add(tilePosition);
                if (newRoomTileSet.Contains(tilePosition))
                {
                    return false;
                }
            }
        }
        return true;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    foreach (Vector2 pos in debugTilePositions)
    //    {
    //        Gizmos.DrawCube(new Vector3(pos.x, pos.y, 0), Vector3.one * 0.5f);  // Adjust the size as needed
    //    }
    //}
}