using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KnifeSel : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        Check();
    }
	
	// Update is called once per frame
	 public void Check () {
        PlayerPrefs.SetInt("KnifeSel0", 1);
        if (PlayerPrefs.GetInt("KnifeSel") == int.Parse(gameObject.name))
        {
            GetComponent<Outline>().enabled = true;
            transform.parent.parent.parent.GetChild(2).GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<Image>().sprite;
        }
        else
        {
            GetComponent<Outline>().enabled = false;
        }
        if (PlayerPrefs.GetInt("KnifeSel" + gameObject.name) == 1)
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 1);
        }
    }
}
