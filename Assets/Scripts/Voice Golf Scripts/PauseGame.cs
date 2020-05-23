using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>(); //Instantiating dictionary to assign phrases.

    public Button Play;
    public Button Return;
    public Button End;

    void Start()
    {
        actions.Add("Resume", Resume);
        actions.Add("Menu", Menu);
        actions.Add("Quit", Quit);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += Recognization; /* When phrase is recognised, execute recognization function */
        keywordRecognizer.Start(); /* Execute on start */

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

    public void Resume()
    {
        Play = GameObject.Find("ResumeButton").GetComponent<Button>();
        Play.Select();

        if (GameIsPaused)
        {

            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;

        }
        else
        {
            Pause();
        }

        Debug.Log("Resume");

    }

    public void Menu()
    {

        Return = GameObject.Find("MenuButton").GetComponent<Button>();
        Return.Select();

        if (GameIsPaused)
        {

            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Pause();
        }


        Debug.Log("Menu");


    }

    public void  Quit()
    {
        End = GameObject.Find("QuitButton").GetComponent<Button>();
        End.Select();

        Application.Quit();
  
        Debug.Log("Quit Game");
    }

    public void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    
}
