using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;
public class KnifeScript : MonoBehaviour {
    [HideInInspector]
    public int FruitsDes;
    bool clicked;
    [SerializeField]
    GameObject Splash;
    [SerializeField]
    Sprite Circle;
    public GameObject Vignette;
    public Sprite[] Knifes;
    [HideInInspector]
    public int Playable;
    public bool cantPlay;
   // Use this for initialization
   void Start () {
        FruitsDes = 0;
        Vignette.SetActive(false);
        KnifeSpriteChange();
    }
    public void KnifeSpriteChange()
    {
        GetComponent<SpriteRenderer>().sprite = Knifes[PlayerPrefs.GetInt("KnifeSel")];
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (cantPlay == false)
            {
                GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                if (thisButton == null)
                {
                    if (Playable == 0)
                    {
                        Playable++;
                        Camera.main.GetComponent<Animator>().SetInteger("Playable", 1);
                        FindObjectOfType<MixerScript>().InsFruitCon();
                        FindObjectOfType<GameManager>().LeaderboardBtn.SetActive(false);
                        FindObjectOfType<GameManager>().SettingsBtn.SetActive(false);
                        //FindObjectOfType<GameManager>().RemoveAdsBtn.SetActive(false);
                        Debug.Log("GAME START");
                    }
                    else
                    {
                        if (clicked == false)
                        {
                            clicked = true;
                            GetComponent<AudioSource>().Play();
                            GetComponent<TrignometricMovement>().enabled = false;
                            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            GetComponent<Rigidbody2D>().AddForce(Vector3.up * 1300, ForceMode2D.Force);
                            Destroy(gameObject, 1);
                            Invoke("Inst", .2f);
                        }
                    }
                }
            }           
        }
        if (clicked)
        {
            transform.Rotate(0, 0, -20);
        }
	}
    void Inst()
    {
        GameObject ab = Instantiate(this.gameObject, new Vector3(0,-2.5f,0), Quaternion.identity);
        ab.GetComponent<TrignometricMovement>().enabled = true;

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Fruits")
        {
            col.gameObject.GetComponent<AudioSource>().Play();
            col.gameObject.GetComponent<Collider2D>().enabled = false;
            col.gameObject.GetComponent<SpriteRenderer>().sprite = Circle;
            col.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .5f);
        }
        if (col.gameObject.tag == "MainCamera")
        {           
            if (FruitsDes == 0)
            {
                cantPlay = true;
                Invoke("gameOv", .5f);
                FindObjectOfType<KnifeScript>().GetComponent<SpriteRenderer>().enabled = false;
                CameraShaker.Instance.ShakeOnce(2f, 30, 0, .5f);
                if(GameManager.Vibration) Handheld.Vibrate();
                Vignette.SetActive(false);
                Vignette.SetActive(true);
            }
        }
    }

    void gameOv()
    {
        FindObjectOfType<GameManager>().GameOver.SetActive(true);
        cantPlay = false;
    }
}
