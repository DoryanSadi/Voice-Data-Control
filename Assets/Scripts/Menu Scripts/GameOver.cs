using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> action = new Dictionary<string, Action>(); //Instantiating dictionary 

    public Button Restart;
    public Button MenuAgain;
    public Button QuitAgain;

    // Start is called before the first frame update
    void Start()
    {
        action.Add("StartAgain", StartAgain);
        action.Add("MainMenu", MainMenu);
        action.Add("Quit", Quit);

        keywordRecognizer = new KeywordRecognizer(action.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += Recognization;
        keywordRecognizer.Start();

    }

    private void Recognization(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        action[speech.text].Invoke();

        StringBuilder status = new StringBuilder();
        status.AppendFormat("{0} ({1})[2]", speech.text, speech.confidence, Environment.NewLine);
        status.AppendFormat("\tTimestamp: {0}{1}", speech.phraseStartTime, Environment.NewLine);
        status.AppendFormat("\tDuration: {0} seconds{1}", speech.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(status.ToString());
    }

    public void StartAgain()
    {
  
        Restart = GameObject.Find("Start Again").GetComponent<Button>();
        Restart.Select();

        if (GameBase.remainingShots == 0)
        {
            GameBase.remainingShots = 10;
            GameBase.Shots = 0;
            SceneManager.LoadScene("VoiceGolf");
        }
        else if (CannonRules.remainingShots == 0)
        {
            CannonRules.remainingShots = 10;
            CannonRules.Shots = 0;
            SceneManager.LoadScene("VoiceGolf");
        }
        else if (GameBase.remainingShots < 10 && GameBase.Shots > 0)
        {
            GameBase.remainingShots = 10;
            GameBase.Shots = 0;
            SceneManager.LoadScene("VoiceGolf");

        }
        else if (CannonRules.remainingShots < 10 && CannonRules.Shots > 0)
        {
            CannonRules.remainingShots = 10;
            CannonRules.Shots = 0;
            SceneManager.LoadScene("VoiceGolf");
        }


        Debug.Log("Start Again ");

    }

    public void MainMenu()
    {
   
        MenuAgain = GameObject.Find("MainMenu").GetComponent<Button>();
        MenuAgain.Select();

        SceneManager.LoadScene("MainMenu");
        Debug.Log("Full Screen");

    }

    public void Quit()
    {
     
        QuitAgain = GameObject.Find("Quit").GetComponent<Button>();
        QuitAgain.Select();

        Application.Quit();
        Debug.Log("Full Screen");

    }

}
