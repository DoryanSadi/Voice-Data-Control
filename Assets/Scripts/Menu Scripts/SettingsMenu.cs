using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> action = new Dictionary<string, Action>(); //Instantiating dictionary 

    public Slider VolumeSlider;
    public Slider BrightnessSlider;
    public Toggle FourEightyP;
    public Toggle SevenTwentyP;
    public Toggle TenEightyP;
    public Toggle MusicOn;
    public Toggle MusicOff;
    public Toggle Full_Screen;
    public Toggle Windowed_Mode;

    public AudioSource audioData;
    public AudioMixer audioMixer;

    bool is_fullscreen;

    // Start is called before the first frame update
    void Start()
    {

        action.Add("Back", Back);
        action.Add("Light", Light);
        action.Add("Dark", Dark);
        action.Add("Increase", Increase);
        action.Add("Decrease", Decrease);
        action.Add("Low", Low);
        action.Add("Medium", Medium);
        action.Add("High", High);
        action.Add("Activate", Activate);
        action.Add("Deactivate", Deactivate);
        action.Add("FullScreen", FullScreen);
        action.Add("WindowedMode", WindowedMode);


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

    public void SetVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10 (sliderValue) * 20); //Presents slider value as a logarithm(base 10), multiplying by a factor of twenty.
        Debug.Log(VolumeSlider.value);
    }

    public void Back()
    {
        Debug.Log("Back");
        SceneManager.LoadScene("MainMenu");
    }

    public void Light()
    {
        BrightnessSlider = GameObject.Find("BrightnessSlider").GetComponent<Slider>();
        BrightnessSlider.direction = Slider.Direction.LeftToRight;


        BrightnessSlider.minValue = 0.0001f;
        BrightnessSlider.maxValue = 1;

        if (BrightnessSlider.value >= BrightnessSlider.minValue && BrightnessSlider.value <= BrightnessSlider.maxValue)
        {
            BrightnessSlider.value = BrightnessSlider.value + 0.100f;
        }

        Debug.Log(BrightnessSlider.value);
        Debug.Log("BU");

    }

    public void Dark()
    {
        BrightnessSlider = GameObject.Find("BrightnessSlider").GetComponent<Slider>();
        BrightnessSlider.direction = Slider.Direction.RightToLeft;


        BrightnessSlider.minValue = 0.0001f;
        BrightnessSlider.maxValue = 1;

        if (BrightnessSlider.value <= BrightnessSlider.maxValue && BrightnessSlider.value > BrightnessSlider.minValue)
        {
            BrightnessSlider.value = BrightnessSlider.value - 0.100f;
        }

        Debug.Log(BrightnessSlider.value);
        Debug.Log("BD");
       
    }

    public void Increase()
    {
        VolumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        VolumeSlider.direction = Slider.Direction.LeftToRight;

       
        VolumeSlider.minValue = 0.0001f;
        VolumeSlider.maxValue = 1;

        if (VolumeSlider.value >= VolumeSlider.minValue && VolumeSlider.value <= VolumeSlider.maxValue)
        {
            VolumeSlider.value = VolumeSlider.value + 0.100f;
        }

        Debug.Log(VolumeSlider.value);
        Debug.Log("VU");
    }


    public void Decrease()
    {
        VolumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        VolumeSlider.direction = Slider.Direction.RightToLeft;

        VolumeSlider.minValue = 0.0001f;
        VolumeSlider.maxValue = 1;

        if (VolumeSlider.value <= VolumeSlider.maxValue && VolumeSlider.value > VolumeSlider.minValue);
        {
            VolumeSlider.value = VolumeSlider.value - 0.100f;
        }
        

        Debug.Log(VolumeSlider.value);
        Debug.Log("VD");
    
    }

    public void Low()
    {
        FourEightyP = GameObject.Find("480p").GetComponent<Toggle>();
        FourEightyP.Select();

        QualitySettings.SetQualityLevel(0);
        Debug.Log("480p");

    }

    public void Medium()
    {
        SevenTwentyP = GameObject.Find("720p").GetComponent<Toggle>();
        SevenTwentyP.Select();

        Debug.Log("720p");
        QualitySettings.SetQualityLevel(1);

    }

    public void High()
    {
        TenEightyP = GameObject.Find("1080p").GetComponent<Toggle>();
        TenEightyP.Select();

        Debug.Log("1080p");
        QualitySettings.SetQualityLevel(2);

    }

    public void Activate()
    {

        audioData = GameObject.Find("VolumeSlider").GetComponent<AudioSource>();
        MusicOn = GameObject.Find("On").GetComponent<Toggle>();
        audioData.Play();
        MusicOn.Select();

        Debug.Log("On");

    }

    public void Deactivate()
    {

        audioData = GameObject.Find("VolumeSlider").GetComponent<AudioSource>();
        MusicOff = GameObject.Find("Off").GetComponent<Toggle>();
        audioData.Stop();
        MusicOff.Select();

        Debug.Log("Off");

    }

    public void FullScreen()
    {
        Full_Screen = GameObject.Find("Full Screen").GetComponent<Toggle>();
        Full_Screen.Select();

        Screen.SetResolution(1080, 720, true);
        Debug.Log("Full Screen");

    }

    public void WindowedMode()
    {
        Windowed_Mode = GameObject.Find("Windowed Mode").GetComponent<Toggle>();
        Windowed_Mode.Select();

        Screen.SetResolution(860, 634, true);
        Debug.Log("Windowed Mode");

    }

    void Update()
    {

    }

    // Update is called once per frame
    public void Quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();

    }
}
