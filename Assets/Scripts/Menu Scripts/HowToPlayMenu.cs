using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using UnityEngine.Windows.Speech; 
using UnityEngine.SceneManagement;

public class HowToPlayMenu : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> action = new Dictionary<string, Action>(); //Instantiating dictionary 

    // Start is called before the first frame update
    void Start()
    {
        action.Add("Back", Back);

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
    
    public void Back()
    {
        Debug.Log("Back");
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    public void Quit()
    {
        Debug.Log("QuitGame");
        Application.Quit();
        
    }
}
