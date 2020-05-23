using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float vitality = 5f;
    public GameObject destruction;
    public float minimumY = -20f;

    GameManager GM; 

    // Start is called before the first frame update
    void Start()
    {
        GM = (GameManager)FindObjectOfType(typeof(GameManager)); 
        
    }

    // Update is called once per frame
    void Update()
    {
        StatusCheck();      
    }

    void StatusCheck()
    {
        vitality -= Time.deltaTime; 

        if (vitality < 0)
        {
            DestroyCannonBall();
        }

        if (transform.position.y < minimumY)
        {
            DestroyCannonBall();
        }

    }

    void DestroyCannonBall()
    {
        Instantiate(destruction, transform.position, transform.rotation);
        GM.SwitchToCannon();
        Destroy(this.gameObject);
    }

    void OnCollisionEnter (Collision coll)
    {
        if (coll.gameObject.tag == "HighScore Target")
        {
            GM.BlueTargetHit();
            DestroyCannonBall(); 
        }
        if (coll.gameObject.tag == "MediumScore Target")
        {
            GM.YellowTargetHit();
            DestroyCannonBall();
        }
        if (coll.gameObject.tag == "LowScore Target")
        {
            GM.RedTargetHit();
            DestroyCannonBall();
        }
    }
}
