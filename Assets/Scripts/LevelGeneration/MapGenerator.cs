using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    Vector2 worldSize = new Vector2(4, 4);
    Room[,] map;

    List<Vector2> usedPositions = new List<Vector2>();
    int gridSizeX, gridSizeY, fullGridSizeX, fullGridSizeY, numberOfRooms = 10;

    public GameObject layoutRoom;
    public Color startColor, endColor;
    public int distanceToEnd;

    public Transform generatorPoint;

    public float xOffset = 18f, yOffset = 10f;
    private float _xWallSize, _yWallSize;
    public GameObject wallBlock;
    private List<GameObject> _roomWalls = new List<GameObject>();
    public LayerMask layerMask;

    // private GameObject _startRoom;
    // private GameObject _endRoom;
    // private List<GameObject> _layoutRoomObjects = new List<GameObject>();

    // public RoomPreFabs rooms;
    // private List<GameObject> _generatedOutlines = new List<GameObject>();

    // Variables for room generation
    // public RoomCenter centerStart, centerEnd;
    // public RoomCenter[] potentialCenters;

    // Start is called before the first frame update
    void Start()
    {
        _xWallSize = wallBlock.GetComponent<BoxCollider2D>().size.x;
        _yWallSize = wallBlock.GetComponent<BoxCollider2D>().size.y;
        // Set number of rooms underneath the maximum rooms the grid can fit.
        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        { 
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }
        
        // These are half extents of the map
        gridSizeX = Mathf.RoundToInt(worldSize.x); 
        gridSizeY = Mathf.RoundToInt(worldSize.y);
        // These are the full lengths of the map
        fullGridSizeX = gridSizeX * 2;
        fullGridSizeY = gridSizeY * 2;
        CreateRoomMap();
        CreateRooms();
    }

    // CreateRoomMap handles the overall room generation within the designated grid
    void CreateRoomMap()
    {
        map = new Room[fullGridSizeX+1, fullGridSizeY+1];

        // Instantiate start room
        map[gridSizeX, gridSizeY] = new Room(Vector2.zero, 1); // Grid positions need to be halved as they are full extents
        usedPositions.Insert(0, Vector2.zero);
        Vector2 currentPos = Vector2.zero;

        // These numbers I pulled from a github.  They're used to force branching later on in the map generation.
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
        for (int i = 0; i < numberOfRooms; i++)
        {
            float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);

            currentPos = NewPosition();
            // Within this loop, if the new position selected is valid, it will try to select a branching room.
            if (NumberOfNeighbors(currentPos) > 1 && Random.value > randomCompare)
            {
                int iterations = 0;
                while (NumberOfNeighbors(currentPos) > 1 && iterations < 20)
                {
                    currentPos = SelectiveNewPosition();
                    iterations++;
                }
            }
    

            // Update the Map with the newly generated room
            map[(int)currentPos.x + gridSizeX, (int)currentPos.y + gridSizeY] = new Room(currentPos, 0);
            usedPositions.Insert(0, currentPos);
        }
    }

    // CreateRooms generates all of the rooms according to the generated room map
    void CreateRooms()
    {
        foreach (Room room in map)
        {
            if (room == null) continue;

            Vector2 drawPos = room.gridPos;
            int posX = (int) drawPos.x, posY = (int) drawPos.y;
            Vector2 roomCoords = new Vector2(posX * xOffset, posY * yOffset); 
            Instantiate(layoutRoom, roomCoords, Quaternion.identity).GetComponent<SpriteRenderer>().color = startColor;
            GenerateWalls((float) 3.0, roomCoords, Quaternion.identity);
        }
    }

    // CheckValidPos checks to see if the position is within bounds and not used yet.
    bool CheckValidPos(Vector2 checkPos)
    {
        int posX = (int) checkPos.x, posY = (int) checkPos.y;
        return (!usedPositions.Contains(checkPos) && posX < gridSizeX && posX >= -gridSizeX && posY < gridSizeY && posY >= -gridSizeY); 
    }

    // Gets a random adjacent position on the map grid, ignoring bounds.
    Vector2 GetRandomAdjacentPosition(Vector2 position) 
    {
        Vector2[] directions = new Vector2[4] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
        int posX = 0, posY = 0, dirX = 0, dirY = 0;
        posX = (int) position.x; //capture its x, y position
        posY = (int) position.y;

        // Randomly Choose a direction to go in between (up, right, left, down)
        int dirIndex = Random.Range(0, 4);
        dirX = (int) directions[dirIndex].x;
        dirY = (int) directions[dirIndex].y;
        Vector2 newPosition = new Vector2(dirX + posX, dirY + posY);
        return newPosition;
    }

    // Finds a completely random new position to build a room at 
    Vector2 NewPosition()
    {
        Vector2 checkPos = Vector2.zero;
        int iterations = 0;
        do
        {
            int index = Mathf.RoundToInt(Random.value * (usedPositions.Count - 1)); // pick a random room
            checkPos = GetRandomAdjacentPosition(usedPositions[index]);
            iterations++;

        } while (!CheckValidPos(checkPos) && iterations < 20); 

        return checkPos;
    }

    // Finds a selective, more likely to be branching position to build a room at
    Vector2 SelectiveNewPosition()
    {
        int index = 0, inc = 0;
        Vector2 checkPos = Vector2.zero;
        int iterations = 0;
        do
        {
            inc = 0;
            do
            {
                // Get a random room that has only one neighbor if possible
                index = Mathf.RoundToInt(Random.value * (usedPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbors(usedPositions[index]) > 1 && inc < 100);
            checkPos = GetRandomAdjacentPosition(usedPositions[index]);
            iterations++;
        } while (!CheckValidPos(checkPos) && iterations < 50);
        return checkPos;
    }

    // NumberOfNeighbors gives us the number of neighboring rooms. This is mostly used during room generation.
    int NumberOfNeighbors(Vector2 pos)
    {
        int ret = 0; // Iterates for each side with a room.

        if (usedPositions.Contains(pos + Vector2.right))
        { // using Vector.[direction] as short hands, for simplicity
            ret++;
        }
        if (usedPositions.Contains(pos + Vector2.left))
        {
            ret++;
        }
        if (usedPositions.Contains(pos + Vector2.up))
        {
            ret++;
        }
        if (usedPositions.Contains(pos + Vector2.down))
        {
            ret++;
        }
        return ret;
    }

    public void GenerateWalls(float radius, Vector3 centerPoint, Quaternion instantiatedRotation)
    {
        // convert the rotation degrees to radians.
        float radiansMultiplier = ((float)System.Math.PI) / 180;

        float xStart = centerPoint.x + radius;
        float yStart = centerPoint.y;

        float currentAngle = 0;

        while (currentAngle < 360)
        {
            float nextAngle = currentAngle + Random.Range(20, 90);

            // no small edges, ensure degree difference is always greater than 20
            if (360 - nextAngle < 20)
            {
                nextAngle = 360;
            }

            // define second point (rotated 1 iteration further).
            float xNext = (float)(centerPoint.x + System.Math.Cos(nextAngle * radiansMultiplier) * radius);
            float yNext = (float)(centerPoint.y + System.Math.Sin(nextAngle * radiansMultiplier) * radius);

            // get relative distance between the two points.
            float xDist = xNext - xStart;
            float yDist = yNext - yStart;

            //linearly interpolate X-wise (horizontally)
            int xMultiplier = xDist > 0 ? 1 : -1;
            xDist = System.Math.Abs(xDist);
            for (int i = 0; i < System.Math.Round(xDist / _xWallSize); i++)
            {
                GameObject horizontalRoomTile = GameObject.Instantiate(wallBlock, new Vector3(xStart + (xMultiplier * i * _xWallSize), yStart, centerPoint.z), instantiatedRotation);
                _roomWalls.Add(horizontalRoomTile);
            }

            float xFinal = xStart + (xMultiplier * (float)System.Math.Round(xDist / _xWallSize) * _xWallSize);

            //linearly interpolate Y-wise (vertically)
            int yMultiplier = yDist > 0 ? 1 : -1;
            yDist = System.Math.Abs(yDist);
            for (int i = 0; i < System.Math.Round(yDist / _yWallSize); i++)
            {
                GameObject verticalRoomTile = GameObject.Instantiate(wallBlock, new Vector3(xFinal, yStart + (yMultiplier * i * _yWallSize), centerPoint.z), instantiatedRotation);
                _roomWalls.Add(verticalRoomTile);
            }
            xStart = xFinal;
            yStart += (yMultiplier * (float) System.Math.Round(yDist / _yWallSize) * _yWallSize);
            currentAngle = nextAngle;
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}

[System.Serializable]
public class RoomPreFabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleLeftUp, doubleLeftDown, doubleLeftRight, doubleUpDown, doubleUpRight, doubleDownRight,
        tripleLeftUpDown, tripleLeftUpRight, tripleLeftDownRight, tripleUpDownRight,
        fourway;
}