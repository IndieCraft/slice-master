using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public Toggle musicToggle;
    public Toggle vibrationToggle;
    public Toggle soundToggle;

    void Start()
    {
        soundToggle.isOn = GameManager.Sound;
        vibrationToggle.isOn = GameManager.Vibration;
    }

    public void ToggleSound(bool arg0)
    {
        GameManager.Sound = arg0;
        if (arg0) {
            GetComponent<AudioSource>().Play();
            AudioListener.pause = false;
        }
        else AudioListener.pause = true;
    }

    public void ToggleVibration(bool arg0)
    {
        GameManager.Vibration = arg0;
        if (arg0) GetComponent<AudioSource>().Play();
    }
}
