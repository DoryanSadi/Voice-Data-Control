using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonRules : MonoBehaviour
{
    public static int Shots = 0;
    public static int remainingShots = 10;
    public Text remainingStrokes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        remainingStrokes.GetComponent<Text>().text = Shots.ToString() + " : " + remainingShots.ToString();
    }
}
