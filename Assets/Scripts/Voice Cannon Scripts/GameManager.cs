using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public float LowScore = 1;
    public float MediumScore = 5;
    public float HighScore = 10;


    public Text scoreText;
    public float score = 0;

    public Camera CameraCannon;
    public Camera CameraCannonBall;
    FollowCamera followCamera; 

    void Start()
    {
        CameraCannonBall.enabled = false; 
    }

    public void SwitchToCannonBall()
    {
        CameraCannonBall.enabled = true;
        followCamera = CameraCannonBall.GetComponent<FollowCamera>();
        followCamera.target = GameObject.FindGameObjectWithTag("CannonBall").transform;
        CameraCannon.enabled = false; 
    }
    
    public void SwitchToCannon()
    {
        CameraCannon.enabled = true;
        CameraCannonBall.enabled = false;
        ResetCameraCannonBall(); 
    }

    public void ResetCameraCannonBall()
    {
        CameraCannonBall.transform.position = followCamera.startingPos; 
    }

    public void BlueTargetHit()
    {
        score += HighScore;
        scoreText.text = "Score: " + score; 
    }

    public void YellowTargetHit()
    {
        score += MediumScore;
        scoreText.text = "Score: " + score; 
    }

   public void RedTargetHit()
    {
        score += LowScore;
        scoreText.text = "Score: " + score; 
    }

}
