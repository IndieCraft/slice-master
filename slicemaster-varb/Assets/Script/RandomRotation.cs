using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomRotation : MonoBehaviour {

   
    HingeJoint2D hinge;
    public JointMotor2D mot;
    [SerializeField]
    float[] RotSpeed;
    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Joint2D>().connectedBody = GetComponent<Rigidbody2D>();
        }
        RanAng();        
    }
    void Update()
    {
        
    }
    void Invokes()
    {        
        float ran = Random.Range(2.2f,7.2f);
        Invoke("RanAng", ran);
    }
    void RanAng()
    {
        hinge = GetComponent<HingeJoint2D>();
        mot = hinge.motor;
        mot.motorSpeed = Random.Range(300, -300);
        if (mot.motorSpeed > 100 || mot.motorSpeed < -100)
        {
            GetComponent<HingeJoint2D>().motor = mot;
        } else
        {
            mot.motorSpeed = RotSpeed[Random.Range(0, RotSpeed.Length)];
            GetComponent<HingeJoint2D>().motor = mot;
        }
        Invokes();
    }
}
