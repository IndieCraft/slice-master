using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KnifeSelDeactive : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Deact()
    {
        gameObject.SetActive(false);
    }
}
