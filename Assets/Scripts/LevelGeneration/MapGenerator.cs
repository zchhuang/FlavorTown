using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    Vector2 worldSize = new Vector2(4, 4);
    Room[,] map;

    public GameObject layoutRoom;
    public Color startColor, endColor;
    public int distanceToEnd;

    public Transform generatorPoint;

    public enum Direction { up, right, down, left }

    public float xOffset = 18f, yOffset = 10f;

    public LayerMask layerMask;

    private GameObject _startRoom;
    private GameObject _endRoom;
    private List<GameObject> _layoutRoomObjects = new List<GameObject>();

    public RoomPreFabs rooms;
    private List<GameObject> _generatedOutlines = new List<GameObject>();

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

        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
            _layoutRoomObjects.Add(newRoom);

            // Select random direction
            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint(selectedDirection);

            // If there is already a room in the same position, continue to move in the same direction until there is an empty spot
            while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, layerMask))
            {
                MoveGenerationPoint(selectedDirection);
            }

            // If at last room, make endRoom
            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                _layoutRoomObjects.RemoveAt(_layoutRoomObjects.Count - 1);
                _endRoom = newRoom;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    public void MoveGenerationPoint(Direction selectedDirection)
    {
        switch (selectedDirection)
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
}

[System.Serializable]
public class RoomPreFabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleLeftUp, doubleLeftDown, doubleLeftRight, doubleUpDown, doubleUpRight, doubleDownRight,
        tripleLeftUpDown, tripleLeftUpRight, tripleLeftDownRight, tripleUpDownRight,
        fourway;
}