using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoinScript : MonoBehaviour {

    public GameObject CoinSound;
	// Use this for initialization
	void Start () {
        cointest();
    }
	
    public void cointest()
    {
        transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("Coins").ToString();
        //Debug.Log("Current coins test " + PlayerPrefs.GetInt("Coins"));
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Coin" )
        {
            GameObject ab = Instantiate(CoinSound, transform.position, Quaternion.identity);
            Destroy(ab, 1);
            GetComponent<Animator>().SetTrigger("CoinCollected");
            Destroy(col.gameObject);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1);
            //Debug.Log("Current coins " + PlayerPrefs.GetInt("Coins"));
            transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("Coins").ToString();
        }
    }
}
