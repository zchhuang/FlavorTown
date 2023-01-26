using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    Vector2 worldSize = new Vector2(4,4);
	Room[,] map;
    public GameObject layoutRoom;
    public Color startColor, endColor;
    public int distanceToEnd;

    public Transform generatorPoint;

    public enum Direction { up, right, down, left }

    public float xOffset = 18f, yOffset = 10f;

    public LayerMask layerMask;

    private GameObject endRoom;
    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomPreFabs rooms;
    private List<GameObject> generatedOutlines = new List<GameObject>();

    // Variables for room generation
    // public RoomCenter centerStart, centerEnd;
    // public RoomCenter[] potentialCenters;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate start room
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;
        Direction selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint(selectedDirection);

        for(int i = 0; i < distanceToEnd; i++){
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
            layoutRoomObjects.Add(newRoom);
            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint(selectedDirection);

            while(Physics2D.OverlapCircle(generatorPoint.position, 0.2f, layerMask))
            {
                MoveGenerationPoint(selectedDirection);
            }

            if(i+1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count-1);
                endRoom = newRoom;
            }
        }

        // Create room outlines
        CreateRoomOutline(Vector3.zero);
        foreach(GameObject room in layoutRoomObjects){
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);

        // foreach(GameObject outline in generatedOutlines){
        //     bool generateCenter = true;
        //     if(outline.transform.position == Vector3.zero){
        //         Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
        //         generateCenter = false;
        //     }
        //     if(outline.transform.position == endRoom.transform.position){
        //         Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
        //         generateCenter = false;
        //     }
        //     if(generateCenter){
        //         int centerSelect = Random.Range(0, potentialCenters.Length);
        //         Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
        //     }
            
        // }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    public void MoveGenerationPoint(Direction selectedDirection)
    {
        switch(selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;

            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;

            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;

            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPosition){
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), 0.2f, layerMask);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), 0.2f, layerMask);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), 0.2f, layerMask);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), 0.2f, layerMask);

        int directionCount = 0;
        if(roomAbove){
            directionCount++;
        }
        if(roomBelow){
            directionCount++;
        }
        if(roomRight){
            directionCount++;
        }
        if(roomLeft){
            directionCount++;
        }

        switch(directionCount){
            case 0:
                Debug.LogError("Found no room exists.");
                break;

            case 1:
                if(roomAbove){
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }
                if(roomBelow){
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }
                if(roomRight){
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }
                if(roomLeft){
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }
                break;

            case 2:
                if(roomLeft && roomAbove){
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation));
                }
                if(roomLeft && roomBelow){
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftDown, roomPosition, transform.rotation));
                }
                if(roomLeft && roomRight){
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }
                if(roomAbove && roomBelow){
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }
                if(roomAbove && roomRight){
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }
                if(roomBelow && roomRight){
                    generatedOutlines.Add(Instantiate(rooms.doubleDownRight, roomPosition, transform.rotation));
                }
                break;

            case 3:
                if(roomLeft && roomAbove && roomBelow){
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftUpDown, roomPosition, transform.rotation));
                }
                if(roomLeft && roomAbove && roomRight){
                generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation));
                }
                if(roomLeft && roomBelow && roomRight){
                generatedOutlines.Add(Instantiate(rooms.tripleLeftDownRight, roomPosition, transform.rotation));
                }
                if(roomAbove && roomBelow && roomRight){
                generatedOutlines.Add(Instantiate(rooms.tripleUpDownRight, roomPosition, transform.rotation));
                }
                break;

            case 4:
                generatedOutlines.Add(Instantiate(rooms.fourway, roomPosition, transform.rotation));
                break;
        }
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