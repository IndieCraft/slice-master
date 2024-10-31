using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class FruitGenerator : MonoBehaviour {

    public Vector3[] Pos;

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < Pos.Length; i++)
        {           
            transform.GetChild(i).localPosition = Pos[i];
        }
    }
}
