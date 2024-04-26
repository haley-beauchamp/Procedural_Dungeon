using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{
    [SerializeField] Tilemap walls;
    [SerializeField] float horizontalPlacementHeightOffset;

    private List<string> doorTypes;
    private List<string> removedDoors;
    private float roomHeight;
    private float roomWidth;

    void Awake()
    {
        InitializeDoorTypes();
        Vector3Int size = walls.size;
        roomHeight = size.y;
        roomWidth = size.x;
    }

    public float GetRoomWidth()
    {
        return roomWidth;
    }

    public float GetRoomHeight()
    {
        return roomHeight;
    }

    public List<string> GetDoorTypes()
    {
        return doorTypes;
    }

    public void UpdateAvailableDoors(string doorType)
    {
        doorTypes.Remove(doorType);
        removedDoors.Add(doorType);
    }

    public Vector3 GetDoorPosition(string doorType)
    {
        return FindChildWithTag(transform, doorType).transform.position;
    }

    public float GetHorizontalPlacementHeightOffset()
    {
        return horizontalPlacementHeightOffset;
    }

    private void InitializeDoorTypes()
    {
        doorTypes = new List<string>();
        removedDoors = new List<string>();

        if (FindChildWithTag(transform, "LeftDoor") != null)
        {
            doorTypes.Add("LeftDoor");
        }
        if (FindChildWithTag(transform, "RightDoor") != null)
        {
            doorTypes.Add("RightDoor");
        }
        if (FindChildWithTag(transform, "TopDoor") != null)
        {
            doorTypes.Add("TopDoor");
        }
        if (FindChildWithTag(transform, "BottomDoor") != null)
        {
            doorTypes.Add("BottomDoor");
        }
    }

    public GameObject FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
            else
            {
                GameObject foundChild = FindChildWithTag(child, tag);
                if (foundChild != null)
                {
                    return foundChild;
                }
            }
        }
        return null;
    }

    public void CreateDoors()
    {
        foreach (string doorType in removedDoors)
        {
            Destroy(FindChildWithTag(transform, doorType));
        }
    }

    /* Method to get ALL relevant tiles, which is wall tiles and floor tiles, in local space, to preserve the dimensions of the room
     * This is for initializing the room data, so that we can check if any part of the room would touch the floor of an existing room */
    public List<Vector2> GetAllLocalTilePositions()
    {
        List<Vector2> positions = new();

        GameObject tilemapObject = FindChildWithTag(transform, "Floor");
        Tilemap floorTilemap = tilemapObject.GetComponent<Tilemap>();
        foreach (var position in floorTilemap.cellBounds.allPositionsWithin)
        {
            if (floorTilemap.HasTile(position))
            {
                Vector3 tileLocalPosition = floorTilemap.CellToLocalInterpolated(position + new Vector3(0.5f, 0.5f, 0)); // Center of the cell
                positions.Add(new Vector2(tileLocalPosition.x, tileLocalPosition.y));
            }
        }

        GameObject wallTilemapObject = FindChildWithTag(transform, "Wall");
        Tilemap wallTilemap = wallTilemapObject.GetComponent<Tilemap>();
        foreach (var position in wallTilemap.cellBounds.allPositionsWithin)
        {
            if (wallTilemap.HasTile(position))
            {
                Vector3 tileLocalPosition = wallTilemap.CellToLocalInterpolated(position + new Vector3(0.5f, 0.5f, 0));
                positions.Add(new Vector2(tileLocalPosition.x, tileLocalPosition.y));
            }
        }

        return positions;
    }

    /* Method to get ONLY the Floor tiles in world space to directly compare with the tiles in a new room
     * This is for checking whether a new room would overlap with the floor of existing rooms
     * The reason being, walls are allowed to overlap, so we don't want to check for collisions there */
    public List<Vector2> GetWorldFloorTilePositions() 
    {
        GameObject tilemapObject = FindChildWithTag(transform, "Floor");
        Tilemap tilemap = tilemapObject.GetComponent<Tilemap>();
        List<Vector2> worldPositions = new();

        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position))
            {
                Vector3 tileLocalPosition = tilemap.CellToLocalInterpolated(position + new Vector3(0.5f, 0.5f, 0));
                Vector3 tileWorldPosition = tilemap.transform.TransformPoint(tileLocalPosition);
                worldPositions.Add(new Vector2(tileWorldPosition.x, tileWorldPosition.y));
            }
        }

        return worldPositions;
    }
}