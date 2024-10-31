using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(FruitGenerator))]
public class StorePos : MonoBehaviour {
    FruitGenerator fg;
	// Use this for initialization
	void Start () {
        fg = GetComponent<FruitGenerator>();
        fg.Pos = new Vector3[transform.childCount];
        for (int i = 0; i < fg.Pos.Length; i++)
        {
            fg.Pos[i] = transform.GetChild(i).localPosition;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
