using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room
{
    public Vector2 gridPos;
    public int type;
    public bool doorTop, doorBot, doorLeft, doorRight;
    public float xSize, ySize;
    public GameObject wallBlock;
    private List<GameObject> _roomWalls = new List<GameObject>();

    public Room(Vector2 _gridPos, int _type){
        gridPos = _gridPos;
    }

}
