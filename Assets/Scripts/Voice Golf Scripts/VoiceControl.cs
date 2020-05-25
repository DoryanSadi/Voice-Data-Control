using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VoiceControl : MonoBehaviour
{
    public float addForce = 0;
    public Transform aimArrow;
    public Camera defaultCamera;
    public float zScale = 2;
    public Transform exitPortal;

    public Transform target;

    public Button Halt;
    public GameObject Pausing;
  

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>(); //Instantiating dictionary to assign phrases.

    void Start()
    {

        actions.Add("Pause", Pause);
        actions.Add("Forward", Forward);
        actions.Add("Back", Back);
        actions.Add("Force", Force);
        actions.Add("Minimum", Minimum);            /* Adding phrases to use as executable functions when recognised */
        actions.Add("RotateRight", RotateRight);
        actions.Add("RotateLeft", RotateLeft);
        actions.Add("Stop", Stop);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += Recognization; /* When phrase is recognised, execute recognization function */
        keywordRecognizer.Start(); /* Execute on start */ 
    }

    public void Update()
    {
        defaultCamera.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        
        if(GameBase.remainingShots == 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        if (target.position.y < -1f)
        {
            SceneManager.LoadScene("GameOver");
        }
    
        
    }

    private void Recognization(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();

        StringBuilder status = new StringBuilder();
        status.AppendFormat("{0} ({1})[2]", speech.text, speech.confidence, Environment.NewLine);
        status.AppendFormat("\tTimestamp: {0}{1}", speech.phraseStartTime, Environment.NewLine);
        status.AppendFormat("\tDuration: {0} seconds{1}", speech.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(status.ToString());
    }

    public void Pause()
    {
        Halt = GameObject.Find("PauseGame").GetComponent<Button>();
        Halt.Select();

        Pausing.SetActive(true); 

    }

    private void RotateRight()
    {
        target.transform.Rotate(0, 10, 0);

 
    }

    private void RotateLeft()
    {
        target.transform.Rotate(0, -10, 0);
    }

    private void Forward()
    {
        GetComponent<Rigidbody>().AddRelativeForce(0, 0, -addForce);
        aimArrow.GetComponent<Renderer>().enabled = false;
        GameBase.Shots += 1;
        GameBase.remainingShots -= 1;
        Debug.Log(GameBase.Shots + "" +GameBase.remainingShots);
    }

    private void Back()
    {
        GetComponent<Rigidbody>().AddRelativeForce(0, 0, addForce);
        aimArrow.GetComponent<Renderer>().enabled = false;
        GameBase.Shots += 1;
        GameBase.remainingShots -= 1;
        Debug.Log(GameBase.Shots + "" + GameBase.remainingShots);
    }

    private void Minimum()
    {
        addForce += 90;
        zScale += .1f;
        if (addForce >= 900)
        {
            addForce = 900;
            zScale = 5;
        }
        else if (addForce < 0)
        {
            addForce = 0;
            zScale = 2;
        }
        aimArrow.GetComponent<Transform>().localScale = new Vector3(2, 2, zScale);

    }


    private void Force()
    {
        addForce += 300;
        zScale += 1;
        if (addForce >= 900)
        {
            addForce = 900;
            zScale = 5; 
        }
        else if(addForce < 0)
            {
                addForce = 0;
                zScale = 2;
            }
        aimArrow.GetComponent<Transform>().localScale = new Vector3(2, 2, zScale);
    }

    private void Stop()
    {
        StartCoroutine(CeaseActivity());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Goal")
        {
            SceneManager.LoadScene("VoiceGolf2");

            GameBase.remainingShots = 10;
            GameBase.Shots = 0;

        }

        if (other.name == "Portal")
        {
            transform.position = exitPortal.transform.position; //Position of ball is changed to area of "exit" portal. 
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.z, 0, 0); //Maintains velocity (speed) of ball after teleport
            defaultCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z + 5); //Camera position is adjusted towards teleported area.
        }
    }

    IEnumerator CeaseActivity()
    {
        yield return new WaitForSeconds(5);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(.1f);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        transform.localEulerAngles = new Vector3(0, 0, 0);
        aimArrow.GetComponent<Renderer>().enabled = true;
        addForce = 0;
        aimArrow.GetComponent<Transform>().localScale = new Vector3(2, 2, 2);
    }

}
