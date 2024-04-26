using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomConfigurationData : MonoBehaviour
{
    [SerializeField] GameObject[] roomPrefabs;

    private static Dictionary<int, List<string>> doorConfigurations;
    private static Dictionary<int, List<Vector2>> tilePositionsMap;
    private static Dictionary<int, float> offsetMap;
    private Dictionary<string, string> compatibilityMap;

    private string[] roomAttachment;


    void Awake()
    {
        doorConfigurations = new Dictionary<int, List<string>>();
        tilePositionsMap = new Dictionary<int, List<Vector2>>();
        offsetMap = new Dictionary<int, float>();
        compatibilityMap = new Dictionary<string, string>
        {
            ["LeftDoor"] = "RightDoor",
            ["RightDoor"] = "LeftDoor",
            ["TopDoor"] = "BottomDoor",
            ["BottomDoor"] = "TopDoor"
        };
        roomAttachment = new string[2];
        InitializeRoomData();
    }

    /* Method to check if the next room and the current room have matching doors at some points--that is, if they can be attached
     * It randomizes the order of the available attachment doors for the current room 
     * This is so that the system won't attach rooms the same way every time */
    public bool CompareRoomDoors(GameObject currentRoom, int nextRoomIndex)
    {
        RoomController currentRoomController = currentRoom.GetComponent<RoomController>();
        List<string> currentRoomDoorConfiguration = currentRoomController.GetDoorTypes();
        List<string> nextRoomDoorConfiguration = GetDoorConfiguration(nextRoomIndex);

        System.Random rng = new(); //make it random so that
        List<string> shuffledCurrentRoomDoorConfiguration = currentRoomDoorConfiguration.OrderBy(x => rng.Next()).ToList();

        foreach (string currentRoomDoorType in shuffledCurrentRoomDoorConfiguration)
        {
            foreach (string nextRoomDoorType in nextRoomDoorConfiguration)
            {
                if (compatibilityMap[currentRoomDoorType].Contains(nextRoomDoorType))
                {
                    roomAttachment[0] = currentRoomDoorType;
                    roomAttachment[1] = nextRoomDoorType;
                    return true;
                }
            }
        }

        return false;
    }

    public List<string> GetDoorConfiguration(int roomPrefabIndex)
    {
        return doorConfigurations[roomPrefabIndex];
    }

    public string[] GetRoomAttachments()
    {
        return roomAttachment;
    }

    public float GetRoomOffset(int roomPrefabIndex) {
        return offsetMap[roomPrefabIndex];
    }

    public List<Vector2> GetTilePositions(int roomPrefabIndex)
    {
        return tilePositionsMap[roomPrefabIndex];
    }

    /* Method to store all of the necessary room data from the prefabs into the relevant dictionaries
     * This allows the system to instantiate each room only once, and then do all comparisons using the dictionaries from that point onward
     * Repeatedly instantiating and deleting prefabs is resource intensive, so this makes the system more efficient */
    private void InitializeRoomData()
    {
        int roomPrefabIndex = 0;

        foreach (GameObject roomPrefab in roomPrefabs)
        {
            GameObject room = Instantiate(roomPrefab);
            RoomController roomController = room.GetComponent<RoomController>();

            List<string> doorTypes = roomController.GetDoorTypes();
            doorConfigurations.Add(roomPrefabIndex, doorTypes);

            List<Vector2> tilePositions = roomController.GetAllLocalTilePositions();
            tilePositionsMap.Add(roomPrefabIndex, tilePositions);

            float offset = roomController.GetHorizontalPlacementHeightOffset();
            offsetMap.Add(roomPrefabIndex, offset);
            roomPrefabIndex++;
            Destroy(room);
        }
    }
}