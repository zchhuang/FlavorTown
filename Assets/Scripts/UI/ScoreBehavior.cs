using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBehavior : MonoBehaviour
{
    public Text thisText;
    private int score;

    void Start()
    {
        thisText = GetComponent<Text>();

        // set score value to be zero
        score = 0;
    }

    void Update()
    {
        // When P is hit
        if (Input.GetKeyDown(KeyCode.P))
        {
            // add 500 points to score
            score += 500;
        }
        // update text of Text element
        thisText.text = "h" + score;
    }

}