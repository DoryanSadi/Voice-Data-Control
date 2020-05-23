using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using UnityEngine.Windows.Speech; 
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button Begin;
    public Button Set;
    public Button HTP;
    public Button Leave;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> action = new Dictionary<string, Action>(); //Instantiating dictionary 

    // Start is called before the first frame update
    void Start()
    {
        action.Add("StartGame", StartGame);
        action.Add("Settings", Settings);
        action.Add("HowToPlay", HowToPlay);
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

    public void StartGame()
    {
        Begin = GameObject.Find("Start Game").GetComponent<Button>();
        Begin.Select();

        Debug.Log("Start Game"); 
        SceneManager.LoadScene("VoiceGolf");
    }

    public void Settings()
    {
        Set = GameObject.Find("Settings").GetComponent<Button>();
        Set.Select(); 

        Debug.Log("Settings");
        SceneManager.LoadScene("SettingsMenu");
    }

    public void HowToPlay()
    {
        HTP = GameObject.Find("How To Play").GetComponent<Button>();
        HTP.Select();

        Debug.Log("How To Play");
        SceneManager.LoadScene("HowToPlayMenu");
    }


    // Update is called once per frame
    public void Quit()
    {
        Leave = GameObject.Find("Quit").GetComponent<Button>();
        Leave.Select(); 

        Debug.Log("Quit Game");
        Application.Quit();
        
    }
}
