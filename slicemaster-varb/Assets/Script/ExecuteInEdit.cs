using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExecuteInEdit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i =1; i < transform.childCount; i++)
        {
			transform.GetChild(i).position = new Vector3( transform.GetChild(i - 1).position.x-5, 1.8f,0);
            transform.GetChild(i).name = "FruitContainer" + i;
        }
	}
}
