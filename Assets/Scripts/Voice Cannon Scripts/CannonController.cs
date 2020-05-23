using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CannonController : MonoBehaviour
{
    //Voice Controls Setup 
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>(); 

    // Rotation Variables 
    public int speed;
    public float friction;
    public float lerpSpeed;
    float xDegrees;
    float yDegrees;
    Quaternion fromRotation;
    Quaternion toRotation;
    Camera camera;

    public Button Halt;
    public GameObject Pausing;

    // Firing Variables. 

    public GameObject cannonBall;
    public Slider slider; 
    Rigidbody RBcannonball;
    public Transform shotPosition;
    public GameObject destruction;
    public float powerLevel;
    public int powerMultiplier = 100;

    GameManager GM; 

    // Start is called before the first frame update
    void Start()
    {
        actions.Add("Pause", Pause);
        actions.Add("Lower", Lower);
        actions.Add("Power", Power);
        actions.Add("Shoot", Shoot); 
        camera = Camera.main;
        GM = (GameManager)FindObjectOfType(typeof(GameManager));
        powerLevel *= powerMultiplier;

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += Recognization;
        keywordRecognizer.Start(); 

    }

    private void Recognization(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke(); 
    }

    private void Shoot()
    {
        TriggerCannon();
    }

    public void Lower()
    {
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        slider.direction = Slider.Direction.RightToLeft;

        slider.minValue = 0.0001f;
        slider.maxValue = 1;

        if (slider.value <= slider.maxValue && slider.value > slider.minValue)
        {
            slider.value = slider.value - 0.100f;
        }
    }

    public void Pause()
    {
        Halt = GameObject.Find("PauseGame").GetComponent<Button>();
        Halt.Select();

        Pausing.SetActive(true);
    }

        public void Power()
    {
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        slider.direction = Slider.Direction.LeftToRight;

        slider.minValue = 0.0001f;
        slider.maxValue = 1;

        if (slider.value >= slider.minValue && slider.value <= slider.maxValue)
        {
            slider.value = slider.value + 0.100f;
        }
    }

    // Update is called once per frame
    void Update()
    {
 
        if (CannonRules.remainingShots == 0)
        {
            SceneManager.LoadScene("Win");
        }

        RaycastHit attack;  
        Ray ray = camera.ScreenPointToRay(Input.mousePosition); //mouse position on 'x' and 'y' 

        if (Physics.Raycast(ray, out attack)) //Creats a ray from our camera to the 'x' and 'y' co-ordinates of our mouse.
        {                                     //Making sure raycast is hitting cannon. 
            if (attack.transform.gameObject.tag == "Cannon")
            {
                if (Input.GetMouseButton(0)) //Checking to make sure left mouse button is down. 
                {
                    xDegrees -= Input.GetAxis("Mouse Y") * speed * friction; /* These variables controls how quickly we can control our rotation" */
                    yDegrees += Input.GetAxis("Mouse X") * speed * friction; 
                    fromRotation = transform.rotation;
                    toRotation = Quaternion.Euler(xDegrees, yDegrees, 0); /* rotating the cannon on the x and y axis not the z axis */ 
                    transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed); /* smoothing out the rotation */ 
                }
            }
        }
    }

    public void TriggerCannon(){

        CannonRules.Shots += 1;
        CannonRules.remainingShots -= 1;
        powerLevel = slider.value * powerMultiplier; 
        GameObject cannonBallReplica = Instantiate(cannonBall, shotPosition.position, transform.rotation) as GameObject;
        RBcannonball = cannonBallReplica.GetComponent<Rigidbody>(); 
        RBcannonball.AddForce(transform.forward * powerLevel);
        Instantiate(destruction, shotPosition.position, shotPosition.rotation);
        GM.SwitchToCannonBall();

    }


}
