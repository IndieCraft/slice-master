using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDrinkSprite : MonoBehaviour {

    [SerializeField]
    Sprite[] DrinkGlass;
    public int coinCount = 3;
    public GameObject Coin;
    int coincon;
	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().sprite = DrinkGlass[Random.Range(0, DrinkGlass.Length)];
    }

    // Update is called once per frame
    public void Deactive () {
        GetComponent<SpriteRenderer>().sprite = DrinkGlass[Random.Range(0, DrinkGlass.Length)];
        gameObject.SetActive(false);
	}
    public void Coins()
    {
        transform.GetChild(0).GetComponent<AudioSource>().Play();
        InvokeRepeating("CoinInst",0,.1f);
    }
    void CoinInst()
    {
        if (coincon < coinCount)
        {
            coincon++;
            Instantiate(Coin, transform.position, Quaternion.identity);
        }
        else
        {
            coincon = 0;
            CancelInvoke("CoinInst");           
        }
       
    }
}
