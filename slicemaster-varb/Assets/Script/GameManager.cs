using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [HideInInspector]
    public int ScoreCount;
    public Text Score;
    public Text LevelText;
    public Slider LevelSlider;
    public Transform FruitStorage;
    public GameObject LevComp;
    public GameObject GameOver;
    public GameObject ExitMen;
    public GameObject ExitGame;
    public GameObject LeaderboardBtn;
    public GameObject SettingsBtn;
    public GameObject RemoveAdsBtn;
    public GameObject KnifeShop;
    float SliderValue;
    [HideInInspector]
    public GameObject activeFruitCont;

    // Use this for initialization
    void Awake () {
        ZPlayerPrefs.Initialize("%&Viva-PlayPROP3RTY459-91Slice!Master%", "8f8KNy45P6V7Q9Vt");
        if (!GameManager.Sound) AudioListener.pause = true;
        if (PlayerPrefs.GetInt("first") == 0)
        {
            PlayerPrefs.SetInt("LevCount", 1);
            PlayerPrefs.SetInt("first", 1);
        }
        LevelText.text = "LEVEL " + PlayerPrefs.GetInt("LevCount");        
    }

    // Update is called once per frame
    void Update () {
        LevelSlider.value = Mathf.Lerp(LevelSlider.value, SliderValue, 10f * Time.smoothDeltaTime);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(FindObjectOfType<KnifeScript>().Playable == 1) ExitMen.SetActive(true);
            else if (!KnifeShop.active) ExitGame.SetActive(true);

        }
        //TAKING SCREENSHOTS
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            ScreenCapture.CaptureScreenshot("0.png");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ScreenCapture.CaptureScreenshot("1.png");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ScreenCapture.CaptureScreenshot("2.png");
        }*/
    }
    public void ScoreSet()
    {       
        Score.text = ScoreCount.ToString();
    }
    public void SliderSet()
    {
        if (LevelSlider.value < 1)
        {
            SliderValue = SliderValue + .1f / (PlayerPrefs.GetInt("LevCount"));
        }
    }
    public void CheckComplete()
    {
        if (LevelSlider.value >= 1)
        {
            int score;
            score = FindObjectOfType<GameManager>().ScoreCount;
            LevComp.SetActive(true);
            PlayerPrefs.SetInt("TotScore", PlayerPrefs.GetInt("TotScore") + score);

            VPGPlayGames.instance.UpdateScore();
            VPGPlayGames.instance.UnlockLevelAch();
        }

    }
    public void SliderLoad()
    {
        SliderValue = 0;
        LevelText.text = "LEVEL " + PlayerPrefs.GetInt("LevCount");
    }
    public void Reload()
    {
        
    }

    public static bool Sound
    {
        get
        {
            return PlayerPrefs.GetInt("GameSound", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("GameSound", value ? 1 : 0);
        }
    }
    public static bool Vibration
    {
        get
        {
            return PlayerPrefs.GetInt("GameVibration", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("GameVibration", value ? 1 : 0);
        }
    }
}