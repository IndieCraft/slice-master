using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FruitCutted : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("MoveToMixer", .2f);
	}
	
	// Update is called once per frame
	void MoveToMixer() {
        gameObject.layer = 8;
        GetComponent<Rigidbody2D>().drag = 0;
        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<Collider2D>().isTrigger = false;
    }
}
