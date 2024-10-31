using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour {

    public GameObject KnifeSel;
    public GameObject SettingsPanel;
    bool knifeMenAct;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && knifeMenAct == true)
        {
            knifeMenAct = false;
            GameObject.Find("BackBTN").transform.parent.GetComponent<Animator>().SetTrigger("KnifeMenuBack");
            GameObject.Find("BackBTN").GetComponent<AudioSource>().Play();
        }
/*        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
        }*/
    }
    public void NoThanks()
    {
        transform.parent.parent.GetComponent<AudioSource>().Play();
        transform.parent.gameObject.SetActive(false);
        Camera.main.GetComponent<Animator>().SetInteger("Playable", 0);
        Camera.main.GetComponent<AudioSource>().Play();
        if (GameObject.Find("Drink"))
        {
            GameObject.Find("Drink").SetActive(false);
        }
        FindObjectOfType<GameManager>().SliderLoad();
        FindObjectOfType<GameManager>().ScoreCount = 0;
        FindObjectOfType<GameManager>().ScoreSet();
        FindObjectOfType<MixerScript>().DesFruitCon();
        FindObjectOfType<KnifeScript>().Playable = 0;
        FindObjectOfType<KnifeScript>().GetComponent<SpriteRenderer>().enabled = true;
        FindObjectOfType<GameManager>().LeaderboardBtn.SetActive(true);
        FindObjectOfType<GameManager>().SettingsBtn.SetActive(true);
        //FindObjectOfType<GameManager>().RemoveAdsBtn.SetActive(true);
        VPAdsController.instance.ShowInterstitial();
    }
    public void WatchVideo()
    {
        transform.parent.parent.GetComponent<AudioSource>().Play();
        //FindObjectOfType<AdMobManager>().Revive = true;
        FindObjectOfType<VPAdsController>().BTN = GetComponent<ButtonManager>();
        FindObjectOfType<VPAdsController>().ShowRewardedAd();

    }
    public void RewardVideo()
    {
        FindObjectOfType<KnifeScript>().GetComponent<SpriteRenderer>().enabled = true;
        transform.parent.gameObject.SetActive(false);
    }
    public void NextLevel ()
    {
        if (GameObject.Find("Drink"))
        {
            GameObject.Find("Drink").SetActive(false);
        }
        FindObjectOfType<GameManager>().ScoreCount = 0;
        FindObjectOfType<GameManager>().ScoreSet();
        transform.parent.parent.GetComponent<AudioSource>().Play();
        Camera.main.GetComponent<AudioSource>().Play();
        transform.parent.gameObject.SetActive(false);
        FindObjectOfType<MixerScript>().DesFruitCon();
        FindObjectOfType<KnifeScript>().Playable = 0;
        PlayerPrefs.SetInt("LevCount", PlayerPrefs.GetInt("LevCount") + 1);
        Camera.main.GetComponent<Animator>().SetInteger("Playable", 0);
        FindObjectOfType<GameManager>().SliderLoad();
        FindObjectOfType<GameManager>().LeaderboardBtn.SetActive(true);
        FindObjectOfType<GameManager>().SettingsBtn.SetActive(true);
        //FindObjectOfType<GameManager>().RemoveAdsBtn.SetActive(true);
        //FindObjectOfType<AdMobManager>().showInterstitial();
    }
    public void KnifeSelect()
    {
        if (PlayerPrefs.GetInt("KnifeSel" + gameObject.name) == 1)
        {
            PlayerPrefs.SetInt("KnifeSel", int.Parse(gameObject.name));
            transform.parent.parent.parent.GetChild(2).GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<Image>().sprite;
            GetComponent<AudioSource>().Play();
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                transform.parent.GetChild(i).GetComponent<KnifeSel>().Check();
            }
            FindObjectOfType<KnifeScript>().KnifeSpriteChange();
        }
        else
        {
            if (PlayerPrefs.GetInt("Coins") >= 40)
            {
                transform.parent.GetComponent<AudioSource>().Play();
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - 40);
                FindObjectOfType<CoinScript>().cointest();
                transform.parent.parent.GetChild(3).gameObject.SetActive(true);
                transform.parent.parent.GetChild(3).position = Input.mousePosition;
                PlayerPrefs.SetInt("KnifeSel" + gameObject.name, 1);
                PlayerPrefs.SetInt("KnifeSel", int.Parse(gameObject.name));
                transform.parent.parent.parent.GetChild(2).GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<Image>().sprite;
                FindObjectOfType<KnifeScript>().KnifeSpriteChange();
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    transform.parent.GetChild(i).GetComponent<KnifeSel>().Check();
                }
            }
            else
            {
                transform.GetChild(0).GetComponent<AudioSource>().Play();
                transform.parent.parent.GetChild(2).GetComponent<Text>().text = "YOU NEED MORE ";
            }
        }
    }
    public void KnifeCustomize()
    {
        GetComponent<AudioSource>().Play();
        knifeMenAct = true;
        KnifeSel.SetActive(true);
    }
    public void KnifeCustomizeBack()
    {
        GetComponent<AudioSource>().Play();
        knifeMenAct = false;
        transform.parent.GetComponent<Animator>().SetTrigger("KnifeMenuBack");
    }
    public void ExitNo()
    {
        transform.parent.parent.parent.GetComponent<AudioSource>().Play();
        transform.parent.parent.gameObject.SetActive(false);
    }
    public void ExitYes()
    {
        transform.parent.parent.parent.GetComponent<AudioSource>().Play();
        transform.parent.parent.gameObject.SetActive(false);
        FindObjectOfType<MixerScript>().DesFruitCon();
        FindObjectOfType<KnifeScript>().Playable = 0;
        FindObjectOfType<GameManager>().SliderLoad();
        Camera.main.GetComponent<Animator>().SetInteger("Playable", 0);
        FindObjectOfType<GameManager>().ScoreCount = 0;
        FindObjectOfType<GameManager>().ScoreSet();
        FindObjectOfType<GameManager>().LeaderboardBtn.SetActive(true);
        FindObjectOfType<GameManager>().SettingsBtn.SetActive(true);
        //FindObjectOfType<GameManager>().RemoveAdsBtn.SetActive(true);
    }
    public void ExitGameNo()
    {
        transform.parent.parent.parent.GetComponent<AudioSource>().Play();
        transform.parent.parent.gameObject.SetActive(false);
    }
    public void ExitGameYes()
    {

        transform.parent.parent.parent.GetComponent<AudioSource>().Play();
        transform.parent.parent.gameObject.SetActive(false);
        FindObjectOfType<MixerScript>().DesFruitCon();
        FindObjectOfType<KnifeScript>().Playable = 0;
        FindObjectOfType<GameManager>().SliderLoad();
        Camera.main.GetComponent<Animator>().SetInteger("Playable", 0);
        Application.Quit();
    }
    public void OpenSettings()
    {
        GetComponent<AudioSource>().Play();
        SettingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        GetComponent<AudioSource>().Play();
        SettingsPanel.SetActive(false);
    }

    public void RemoveAds()
    {
        ZPlayerPrefs.SetInt("remove_ads", 1);
        ZPlayerPrefs.Save();
        VPAdsController.instance.DestroyAds();
    }
    public void RemoveAdsFailed()
    {
        //Toast.instance.ShowMessage("Purchase failed. Please retry later.");
    }
}
