using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerScript : MonoBehaviour {

    int FruitCount;
    int TotalFruit;
    GameManager gm;
    // for initialization
    void Start() {
        gm = FindObjectOfType<GameManager>();
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Cutted")
        {
            transform.GetChild(0).GetChild(FruitCount).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(FruitCount).GetComponent<SpriteRenderer>().color = col.transform.GetChild(0).GetComponent<ParticleSystem>().main.startColor.color;
            col.transform.GetChild(0).gameObject.SetActive(true);
            col.transform.GetChild(0).position = new Vector3(-1.8f, -1.8f, 0);
            col.transform.GetChild(0).eulerAngles = Vector3.zero;
            GetComponent<Animator>().SetTrigger("FruitHit");
            Destroy(col.transform.GetChild(0).gameObject, 1);
            col.transform.GetChild(0).parent = null;
            Destroy(col.gameObject, .02f);
            FruitCount++;            
            if (TotalFruit == FruitCount)
            {
                gm.CheckComplete();
                FindObjectOfType<KnifeScript>().cantPlay = true;
                GetComponent<AudioSource>().Play();
                FruitCount = 0;
                Invoke("FruitDesIns",.8f);
            }
        }
        if (col.gameObject.tag == "CuttedExtra")
        {
            col.transform.GetChild(0).gameObject.SetActive(true);
            col.transform.GetChild(0).position = new Vector3(-1.8f, -1.6f, 0);
            col.transform.GetChild(0).eulerAngles = Vector3.zero;
            Destroy(col.transform.GetChild(0).gameObject, 1);
            col.transform.GetChild(0).parent = null;
            Destroy(col.gameObject, .02f);
        }
    }
    void FruitDesIns()
    {
        FindObjectOfType<KnifeScript>().cantPlay = false;
        if (Camera.main.GetComponent<Animator>().GetInteger("Playable") == 1)
        {
            //Debug.Log("Triggered "+ gm.activeFruitCont.transform.childCount);
            Destroy(gm.activeFruitCont);
            gm.activeFruitCont = Instantiate(gm.FruitStorage.GetChild(Random.Range(gm.FruitStorage.childCount - 1, 0)).gameObject, new Vector3(0f, 1.8f, 0f), Quaternion.identity);
            gm.activeFruitCont.SetActive(true);
            TotalFruit = gm.activeFruitCont.transform.childCount;
            transform.parent.GetChild(0).gameObject.SetActive(true);
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
        }       
    }
    public void InsFruitCon ()
    {
        Destroy(gm.activeFruitCont);
        gm.activeFruitCont = Instantiate(gm.FruitStorage.GetChild(Random.Range(gm.FruitStorage.childCount - 1, 0)).gameObject, new Vector3(0f, 1.8f, 0f), Quaternion.identity);
        gm.activeFruitCont.SetActive(true);
        TotalFruit = gm.activeFruitCont.transform.childCount;
    }
    public void DesFruitCon()
    {
        FruitCount = 0;
        Destroy(gm.activeFruitCont);
        TotalFruit = 0;
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
    }
}
