using UnityEngine;
using UnityEngine.UI;

public class FollowUs : MonoBehaviour {

    public string URL;
    public void EarnByVisit()
    {
        GetComponent<AudioSource>().Play();
        Application.OpenURL(URL);
        if (PlayerPrefs.GetInt(gameObject.name) == 0)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 5);
            //Toast.instance.ShowMessage("You've got 20 Apples!");
            PlayerPrefs.SetInt(gameObject.name, 1);
        }
    }
}
